#Requires -Version 7
<#
.SYNOPSIS
    Common repository verification entry point. Never deploys.
.DESCRIPTION
    Quick: format, solution determinism, versions, documentation, skills and tracked MSBuild XML.
    Pure: Quick plus game-free test projects and every Tools/Tests/*.Tests.ps1.
    Full: Quick plus a compile-only Release build, all test projects and tool tests.
    Auto: Full when game references exist, otherwise Pure. No profile runs tests twice.
    Existing helpers own their individual checks; this script only coordinates them.
    Each execution writes report.json and step logs into a unique scratch directory.
    ListOnly prints the selected steps without resolving the SDK or writing files.
#>
[CmdletBinding()]
param(
    [ValidateSet('Quick', 'Pure', 'Full', 'Auto')]
    [string]$Mode = 'Auto',
    [string]$DotnetPath,
    [string]$ScratchRoot,
    [switch]$ListOnly
)

$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot 'dotnet-sdk.ps1')
$PSNativeCommandUseErrorActionPreference = $false
$repoRoot = Split-Path $PSScriptRoot -Parent
$gameDir = if ($env:SCHEDULE1_PATH) { $env:SCHEDULE1_PATH } else { 'C:\Program Files (x86)\Steam\steamapps\common\Schedule I' }
$gameProbe = Join-Path $gameDir 'MelonLoader/Il2CppAssemblies/Assembly-CSharp.dll'
$hasGame = Test-Path -LiteralPath $gameProbe -PathType Leaf
$effectiveMode = if ($Mode -eq 'Auto') { if ($hasGame) { 'Full' } else { 'Pure' } } else { $Mode }
$steps = [System.Collections.Generic.List[object]]::new()
$results = [System.Collections.Generic.List[object]]::new()
$runScratch = $null
$startedUtc = [DateTime]::UtcNow
$dotnet = if ($DotnetPath) { $DotnetPath } else { 'dotnet' }
$pwsh = (Get-Process -Id $PID).Path

function Add-ScriptStep {
    param([string]$Name, [string]$RelativePath, [string[]]$Arguments = @())
    $steps.Add([pscustomobject]@{
        Name = $Name
        Command = $pwsh
        Arguments = @('-NoProfile', '-NonInteractive', '-ExecutionPolicy', 'Bypass', '-File', (Join-Path $repoRoot $RelativePath)) + $Arguments
    })
}

function Invoke-VerifyStep {
    param($Step)
    $watch = [System.Diagnostics.Stopwatch]::StartNew()
    $logPath = Join-Path $runScratch ($Step.Name + '.log')
    $failure = $null
    $exitCode = 1
    Write-Host "[$($Step.Name)]" -ForegroundColor Cyan
    try {
        $command = $Step.Command
        $arguments = $Step.Arguments
        & $command @arguments 2>&1 | Tee-Object -FilePath $logPath | ForEach-Object { Write-Host $_ }
        $exitCode = $LASTEXITCODE
        if ($null -eq $exitCode) { $exitCode = 1; $failure = 'Child process returned no exit code.' }
        if ($exitCode -ne 0 -and -not $failure) { $failure = "Child process exited $exitCode." }
    }
    catch {
        $failure = $_.Exception.Message
        $failure | Out-File -LiteralPath $logPath -Append -Encoding utf8
    }
    $watch.Stop()
    $result = [pscustomobject]@{
        Name = $Step.Name
        Status = if ($exitCode -eq 0 -and -not $failure) { 'Passed' } else { 'Failed' }
        ExitCode = $exitCode
        Seconds = [Math]::Round($watch.Elapsed.TotalSeconds, 2)
        Command = $Step.Command
        Arguments = $Step.Arguments
        LogPath = $logPath
        Error = $failure
    }
    $results.Add($result)
    Write-Host "[$($Step.Name)] $($result.Status) ($($result.Seconds)s)"
    return $result
}

function Set-VerifyPlan {
    $steps.Clear()
    $steps.Add([pscustomobject]@{
        Name = 'format'
        Command = $dotnet
        Arguments = @('format', (Join-Path $repoRoot 'Source/Mods/S1Mods.sln'), '--verify-no-changes', '--verbosity', 'minimal')
    })
    Add-ScriptStep 'solution' 'Tools/gen-sln.ps1' @('-Check')
    Add-ScriptStep 'versions' 'Tools/check-version-sync.ps1'
    Add-ScriptStep 'docs' 'Tools/check-doc-paths.ps1'
    Add-ScriptStep 'skills' 'Tools/check-skills-health.ps1'
    Add-ScriptStep 'xml' 'Tools/check-xml.ps1'
    if ($effectiveMode -eq 'Quick') { return }
    if ($effectiveMode -eq 'Full') {
        Add-ScriptStep 'build' 'Tools/build-all.ps1' @('-NoDeploy', '-Config', 'Release', '-DotnetPath', $dotnet)
    }
    $selection = if ($effectiveMode -eq 'Full') { 'all' } else { 'pure' }
    $testArgs = @('-Selection', $selection, '-Configuration', 'Release', '-DotnetPath', $dotnet)
    if ($runScratch) { $testArgs += @('-ScratchRoot', (Join-Path $runScratch 'tests')) }
    Add-ScriptStep 'tests' 'Tools/test-all.ps1' $testArgs

    $suiteRoot = Join-Path $repoRoot 'Tools/Tests'
    $suites = @(Get-ChildItem -LiteralPath $suiteRoot -Filter '*.Tests.ps1' -File | Sort-Object Name)
    if ($suites.Count -eq 0) { throw "No tool regression suites found under '$suiteRoot'." }
    foreach ($suite in $suites) {
        $tokens = $null
        $parseErrors = $null
        $ast = [System.Management.Automation.Language.Parser]::ParseFile($suite.FullName, [ref]$tokens, [ref]$parseErrors)
        if ($parseErrors.Count -gt 0) { throw "Tool suite '$($suite.Name)' contains PowerShell parse errors: $($parseErrors -join '; ')" }
        $declared = if ($ast.ParamBlock) { @($ast.ParamBlock.Parameters.Name.VariablePath.UserPath) } else { @() }
        $suiteArgs = @()
        if ($runScratch -and $declared -contains 'ScratchRoot') {
            $fixtureName = $suite.BaseName -replace '\.Tests$', ''
            $suiteArgs += @('-ScratchRoot', (Join-Path $runScratch ('fixtures/' + $fixtureName)))
        }
        Add-ScriptStep ('tool-' + $suite.BaseName) ('Tools/Tests/' + $suite.Name) $suiteArgs
    }
}

Write-Host "Repository verification: requested=$Mode, selected=$effectiveMode"
Write-Host "Game references: $(if ($hasGame) { 'present' } else { 'absent' }) ($gameProbe)"
if ($ListOnly) {
    Set-VerifyPlan
    $steps | ForEach-Object { Write-Host "  $($_.Name): $($_.Command) $($_.Arguments -join ' ')" }
    Write-Host 'List-only: no checks executed and no files written.'
    if ($effectiveMode -eq 'Full' -and -not $hasGame) {
        Write-Host 'Full verification cannot run without game references.' -ForegroundColor Red
        exit 2
    }
    exit 0
}

$oldNoDeploy = $env:S1NoDeploy
$oldLocation = Get-Location
$exitCode = 1
try {
    if (-not $ScratchRoot) {
        # A packaged Windows host can virtualize LOCALAPPDATA: Git then sees a
        # different physical root in nested fixtures. Prefer the user profile.
        if ($env:USERPROFILE) { $ScratchRoot = Join-Path $env:USERPROFILE '.cache/Schedule1Tools/verification' }
        elseif ($env:LOCALAPPDATA) { $ScratchRoot = Join-Path $env:LOCALAPPDATA 'Schedule1Tools/verification' }
        else { throw 'No scratch root available; pass -ScratchRoot.' }
    }
    $scratchFull = [System.IO.Path]::GetFullPath($ScratchRoot)
    $systemTemp = [System.IO.Path]::GetFullPath([System.IO.Path]::GetTempPath()).TrimEnd('\', '/')
    if ($scratchFull.TrimEnd('\', '/').Equals($systemTemp, [StringComparison]::OrdinalIgnoreCase) -or
        $scratchFull.StartsWith($systemTemp + [System.IO.Path]::DirectorySeparatorChar, [StringComparison]::OrdinalIgnoreCase)) {
        throw 'ScratchRoot must be outside the OS temp directory (required by the existing test helpers).'
    }
    # Keep nested fixture paths below Windows/Git path limits. Timestamps live in
    # report.json; the directory only needs a unique identifier.
    $runScratch = Join-Path $scratchFull ('verify-' + [guid]::NewGuid().ToString('N').Substring(0, 12))
    $null = New-Item -ItemType Directory -Path $runScratch -Force
    Write-Host "Artifacts: $runScratch"
    if ($effectiveMode -eq 'Full' -and -not $hasGame) { throw "Full verification requires game references at '$gameProbe'. Use -Mode Pure on a game-free machine." }
    $dotnet = Resolve-S1DotnetPath -Requested $DotnetPath
    Set-Location -LiteralPath $repoRoot
    $env:S1NoDeploy = 'true'
    $sdk = Invoke-VerifyStep ([pscustomobject]@{ Name = 'sdk'; Command = $dotnet; Arguments = @('--version') })
    if ($sdk.Status -ne 'Passed') { throw 'The .NET SDK could not be resolved; see sdk.log.' }
    $version = [System.IO.File]::ReadAllText($sdk.LogPath).Trim()
    $null = Assert-S1SdkVersion -Output $version
    Set-VerifyPlan
    foreach ($step in $steps) {
        if ($step.Name -eq 'build' -and @($results | Where-Object { $_.Name -eq 'solution' -and $_.Status -ne 'Passed' }).Count -gt 0) {
            # build-all normally repairs a missing solution. Verification must not
            # turn a failed read-only check into an implicit source rewrite.
            $results.Add([pscustomobject]@{
                Name = 'build'; Status = 'Skipped'; ExitCode = $null; Seconds = 0
                Command = $step.Command; Arguments = $step.Arguments; LogPath = $null
                Error = 'Solution validation failed; compile step skipped to preserve the working tree.'
            })
            Write-Host '[build] Skipped: solution validation failed.'
            continue
        }
        $null = Invoke-VerifyStep $step
    }
    $exitCode = if (@($results | Where-Object Status -eq 'Failed').Count -eq 0) { 0 } else { 1 }
}
catch {
    $message = $_.Exception.Message
    Write-Host "Verification failed: $message" -ForegroundColor Red
    $results.Add([pscustomobject]@{
        Name = 'preflight'; Status = 'Failed'; ExitCode = 2; Seconds = 0
        Command = $null; Arguments = @(); LogPath = $null; Error = $message
    })
    $exitCode = 2
}
finally {
    $env:S1NoDeploy = $oldNoDeploy
    Set-Location -LiteralPath $oldLocation.Path
    $failed = @($results | Where-Object Status -eq 'Failed').Count
    $passed = @($results | Where-Object Status -eq 'Passed').Count
    $skipped = @($results | Where-Object Status -eq 'Skipped').Count
    Write-Host "Verification $effectiveMode : $passed passed, $failed failed, $skipped skipped. Exit $exitCode."
    if ($runScratch) {
        $reportPath = Join-Path $runScratch 'report.json'
        [ordered]@{
            SchemaVersion = 1
            Repository = $repoRoot
            RequestedMode = $Mode
            Mode = $effectiveMode
            StartedUtc = $startedUtc.ToString('o')
            FinishedUtc = [DateTime]::UtcNow.ToString('o')
            GameReferences = $hasGame
            GameProbe = $gameProbe
            Dotnet = $dotnet
            ExitCode = $exitCode
            Passed = $passed
            Failed = $failed
            Skipped = $skipped
            Steps = @($results.ToArray())
        } | ConvertTo-Json -Depth 8 | Set-Content -LiteralPath $reportPath -Encoding utf8
        Write-Host "Report: $reportPath"
    }
}
exit $exitCode
