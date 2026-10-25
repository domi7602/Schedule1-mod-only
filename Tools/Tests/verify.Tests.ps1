#Requires -Version 7
# Isolated process tests: every helper and dotnet is fake; the real generator is
# exercised separately against tiny fixture projects. Never recurse into this suite.
[CmdletBinding()]
param([string]$ScratchRoot)

$ErrorActionPreference = 'Stop'
$PSNativeCommandUseErrorActionPreference = $false
$repoRoot = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
if (-not $ScratchRoot) { $ScratchRoot = Join-Path $env:LOCALAPPDATA 'Schedule1Tools/verification-tests' }
$scratchFull = [IO.Path]::GetFullPath($ScratchRoot)
$repoFull = [IO.Path]::GetFullPath($repoRoot).TrimEnd('\', '/')
if ($scratchFull.Equals($repoFull, [StringComparison]::OrdinalIgnoreCase) -or
    $scratchFull.StartsWith($repoFull + [IO.Path]::DirectorySeparatorChar, [StringComparison]::OrdinalIgnoreCase)) {
    throw 'Test fixtures must be outside the repository.'
}
$suiteRoot = Join-Path $scratchFull ('verify-tests-' + [guid]::NewGuid().ToString('N'))
$null = New-Item -ItemType Directory -Path $suiteRoot -Force
$pwsh = (Get-Process -Id $PID).Path
$script:passed = 0
$script:failed = 0

function Check {
    param([bool]$Condition, [string]$Message)
    if ($Condition) { $script:passed++ }
    else { $script:failed++; Write-Host "FAIL: $Message" -ForegroundColor Red }
}

function Write-Fixture {
    param([string]$Path, [string]$Content)
    $null = New-Item -ItemType Directory -Path (Split-Path $Path -Parent) -Force
    [IO.File]::WriteAllText($Path, ($Content -replace "`r`n", "`n"), [Text.UTF8Encoding]::new($false))
}

$helperTemplate = @'
__PARAM__
$entry = [ordered]@{ Name = '__NAME__'; Arguments = @($args); NoDeploy = $env:S1NoDeploy; Location = (Get-Location).Path; ScratchRoot = $ScratchRoot }
($entry | ConvertTo-Json -Compress) | Add-Content -LiteralPath $env:VERIFY_TEST_TRACE
Write-Output 'fixture __NAME__'
if ($env:VERIFY_TEST_FAIL -eq '__NAME__') { exit 7 }
exit 0
'@
$fakeDotnet = @'
$verb = $args[0]
([ordered]@{ Name = "dotnet-$verb"; Arguments = @($args); NoDeploy = $env:S1NoDeploy; Location = (Get-Location).Path } | ConvertTo-Json -Compress) | Add-Content -LiteralPath $env:VERIFY_TEST_TRACE
if ($env:VERIFY_TEST_FAIL -eq "dotnet-$verb") { Write-Output 'injected native failure'; exit 9 }
if ($verb -eq '--version') {
    if ($env:VERIFY_TEST_OLD_SDK -eq 'true') { Write-Output '6.0.999' } else { Write-Output '8.0.999' }
} else { Write-Output "fixture dotnet $verb" }
exit 0
'@

function New-Case {
    param([string]$Name, [switch]$Game)
    $root = Join-Path $suiteRoot $Name
    $null = New-Item -ItemType Directory -Path (Join-Path $root 'Tools') -Force
    Copy-Item -LiteralPath (Join-Path $repoRoot 'Tools/verify.ps1') -Destination (Join-Path $root 'Tools/verify.ps1')
    Copy-Item -LiteralPath (Join-Path $repoRoot 'Tools/dotnet-sdk.ps1') -Destination (Join-Path $root 'Tools/dotnet-sdk.ps1')
    foreach ($helper in @('gen-sln', 'check-version-sync', 'check-doc-paths', 'check-skills-health', 'check-xml', 'build-all', 'test-all')) {
        Write-Fixture (Join-Path $root "Tools/$helper.ps1") ($helperTemplate.Replace('__PARAM__', '').Replace('__NAME__', $helper))
    }
    Write-Fixture (Join-Path $root 'Tools/Tests/a.Tests.ps1') ($helperTemplate.Replace('__PARAM__', 'param([string]$ScratchRoot)').Replace('__NAME__', 'suite-a'))
    Write-Fixture (Join-Path $root 'Tools/Tests/z-added.Tests.ps1') ($helperTemplate.Replace('__PARAM__', '').Replace('__NAME__', 'suite-z'))
    Write-Fixture (Join-Path $root 'fake-dotnet.ps1') $fakeDotnet
    Write-Fixture (Join-Path $root 'fake-dotnet.cmd') "@echo off`npwsh -NoProfile -ExecutionPolicy Bypass -File `"%~dp0fake-dotnet.ps1`" %*`nexit /b %ERRORLEVEL%`n"
    Write-Fixture (Join-Path $root 'Source/Mods/S1Mods.sln') 'fixture solution'
    if ($Game) { Write-Fixture (Join-Path $root 'game/MelonLoader/Il2CppAssemblies/Assembly-CSharp.dll') '' }
    return $root
}

function Run-Case {
    param([string]$Root, [string]$Mode = 'Auto', [string]$Fail = '', [switch]$ListOnly, [switch]$OldSdk,
        [ValidateSet('Explicit', 'Profile', 'Local')][string]$ScratchLocation = 'Explicit')
    $names = @('SCHEDULE1_PATH', 'VERIFY_TEST_TRACE', 'VERIFY_TEST_FAIL', 'VERIFY_TEST_OLD_SDK', 'S1NoDeploy', 'USERPROFILE', 'LOCALAPPDATA')
    $saved = @{}
    foreach ($name in $names) { $saved[$name] = [Environment]::GetEnvironmentVariable($name, 'Process') }
    try {
        $env:SCHEDULE1_PATH = Join-Path $Root 'game'
        $env:VERIFY_TEST_TRACE = Join-Path $Root 'trace.jsonl'
        $env:VERIFY_TEST_FAIL = $Fail
        $env:VERIFY_TEST_OLD_SDK = $OldSdk.IsPresent.ToString().ToLowerInvariant()
        $env:S1NoDeploy = 'false'
        $env:USERPROFILE = if ($ScratchLocation -eq 'Local') { $null } else { Join-Path $Root 'p' }
        $env:LOCALAPPDATA = Join-Path $Root 'l'
        $artifactRoot = switch ($ScratchLocation) {
            'Explicit' { Join-Path $Root 'artifacts' }
            'Profile' { Join-Path $env:USERPROFILE '.cache/Schedule1Tools/verification' }
            'Local' { Join-Path $env:LOCALAPPDATA 'Schedule1Tools/verification' }
        }
        $runnerArgs = @('-NoProfile', '-ExecutionPolicy', 'Bypass', '-File', (Join-Path $Root 'Tools/verify.ps1'), '-Mode', $Mode,
            '-DotnetPath', (Join-Path $Root 'fake-dotnet.cmd'))
        if ($ScratchLocation -eq 'Explicit') { $runnerArgs += @('-ScratchRoot', $artifactRoot) }
        if ($ListOnly) { $runnerArgs += '-ListOnly' }
        $output = @(& $pwsh @runnerArgs 2>&1) -join "`n"
        $code = $LASTEXITCODE
        Write-Fixture (Join-Path $Root 'runner-output.log') $output
        $trace = if (Test-Path -LiteralPath $env:VERIFY_TEST_TRACE) { @(Get-Content -LiteralPath $env:VERIFY_TEST_TRACE | ForEach-Object { $_ | ConvertFrom-Json }) } else { @() }
        $reports = @(Get-ChildItem -LiteralPath $artifactRoot -Filter report.json -Recurse -File -ErrorAction SilentlyContinue)
        $report = if ($reports.Count -gt 0) { Get-Content -LiteralPath $reports[-1].FullName -Raw | ConvertFrom-Json } else { $null }
        return [pscustomobject]@{ Code = $code; Output = $output; Trace = $trace; Report = $report; Artifacts = $artifactRoot }
    }
    finally {
        foreach ($name in $names) { [Environment]::SetEnvironmentVariable($name, $saved[$name], 'Process') }
    }
}

$quickRoot = New-Case 'quick'
$quick = Run-Case $quickRoot 'Quick'
Check ($quick.Code -eq 0) 'Quick succeeds with passing fake checks.'
Check ($quick.Report.Mode -eq 'Quick' -and $quick.Report.Failed -eq 0) 'Quick JSON report records the selected mode and result.'
Check (($quick.Trace.Name -join ',') -eq 'dotnet---version,dotnet-format,gen-sln,check-version-sync,check-doc-paths,check-skills-health,check-xml') 'Quick calls exactly the shared quick checks, in order.'
Check (@($quick.Trace | Where-Object NoDeploy -ne 'true').Count -eq 0) 'Every child receives deployment disabled.'
Check (@($quick.Trace | Where-Object Location -ne $quickRoot).Count -eq 0) 'Checks run from the fixture repository even when invoked elsewhere.'
Check (($quick.Trace | Where-Object Name -eq 'gen-sln').Arguments -contains '-Check') 'Solution validation uses the nonmutating generator mode.'
Check (@($quick.Report.Steps | Where-Object { -not (Test-Path -LiteralPath $_.LogPath) }).Count -eq 0) 'Every executed check has a retained log.'

$pure = Run-Case (New-Case 'pure') 'Pure'
Check ($pure.Code -eq 0) 'Pure succeeds.'
$pureTest = @($pure.Trace | Where-Object Name -eq 'test-all')
Check ($pureTest.Count -eq 1 -and $pureTest[0].Arguments -contains 'pure') 'Pure runs game-free test-all exactly once.'
Check ($pure.Trace.Name -notcontains 'build-all') 'Pure never builds the solution.'
Check ($pure.Trace.Name -contains 'suite-a' -and $pure.Trace.Name -contains 'suite-z') 'All dynamically discovered tool suites run, including a newly named suite.'
Check (-not [string]::IsNullOrEmpty(($pure.Trace | Where-Object Name -eq 'suite-a').ScratchRoot)) 'A suite declaring ScratchRoot receives an isolated fixture directory.'
Check (($pure.Trace | Where-Object Name -eq 'suite-z').Arguments.Count -eq 0) 'Suites without ScratchRoot do not receive an unsupported switch.'

$fullRoot = New-Case 'full' -Game
$full = Run-Case $fullRoot 'Full'
Check ($full.Code -eq 0) 'Full succeeds with fixture game references.'
$fullBuild = @($full.Trace | Where-Object Name -eq 'build-all')
$fullTest = @($full.Trace | Where-Object Name -eq 'test-all')
Check ($fullBuild.Count -eq 1 -and $fullBuild[0].Arguments -contains '-NoDeploy') 'Full builds once through build-all with explicit NoDeploy.'
Check ($fullTest.Count -eq 1 -and $fullTest[0].Arguments -contains 'all' -and $fullTest[0].Arguments -notcontains 'pure') 'Full runs all tests once, without a duplicate Pure run.'
Check ($fullBuild[0].Arguments -contains (Join-Path $fullRoot 'fake-dotnet.cmd') -and $fullTest[0].Arguments -contains (Join-Path $fullRoot 'fake-dotnet.cmd')) 'The explicit SDK host is forwarded to build and test helpers.'

$autoPure = Run-Case (New-Case 'auto-pure')
$autoFull = Run-Case (New-Case 'auto-full' -Game)
Check ($autoPure.Code -eq 0 -and $autoPure.Report.Mode -eq 'Pure') 'Auto selects Pure when game references are missing.'
Check ($autoFull.Code -eq 0 -and $autoFull.Report.Mode -eq 'Full') 'Auto selects Full when game references exist.'

$missing = Run-Case (New-Case 'missing-game') 'Full'
Check ($missing.Code -ne 0 -and $missing.Trace.Count -eq 0) 'Explicit Full rejects missing references before running any command.'
Check ($missing.Report.ExitCode -ne 0 -and $missing.Report.Failed -gt 0) 'A failed prerequisite still produces a failure report.'

$failedFormat = Run-Case (New-Case 'failed-format') 'Quick' -Fail 'dotnet-format'
Check ($failedFormat.Code -ne 0 -and ($failedFormat.Report.Steps | Where-Object Name -eq 'format').ExitCode -eq 9) 'Native format failure propagates through exit status and JSON.'
Check ($failedFormat.Trace.Name -contains 'check-skills-health') 'Independent quick checks still run after a failure.'
$failedTest = Run-Case (New-Case 'failed-tests') 'Pure' -Fail 'test-all'
Check ($failedTest.Code -ne 0 -and ($failedTest.Report.Steps | Where-Object Name -eq 'tests').ExitCode -eq 7) 'Test helper failure cannot be hidden by later passing suites.'
$failedSuite = Run-Case (New-Case 'failed-suite') 'Pure' -Fail 'suite-a'
Check ($failedSuite.Code -ne 0 -and $failedSuite.Trace.Name -contains 'suite-z') 'Tool suite failures aggregate while later suites still run.'

$missingSolutionRoot = New-Case 'failed-solution-full' -Game
$missingSolution = Join-Path $missingSolutionRoot 'Source/Mods/S1Mods.sln'
Remove-Item -LiteralPath $missingSolution
# If orchestration incorrectly invokes build-all, this fake recreates the source
# file just as the real helper would. It must never be reached in verification.
Write-Fixture (Join-Path $missingSolutionRoot 'Tools/build-all.ps1') @'
Set-Content -LiteralPath 'Source/Mods/S1Mods.sln' -Value 'unexpected repair'
exit 0
'@
$failedSolution = Run-Case $missingSolutionRoot 'Full' -Fail 'gen-sln'
Check ($failedSolution.Code -ne 0 -and ($failedSolution.Report.Steps | Where-Object Name -eq 'build').Status -eq 'Skipped') 'Failed solution check skips dependent build and reports the skipped step.'
Check (-not (Test-Path -LiteralPath $missingSolution)) 'Full verification cannot regenerate a missing solution after Check fails.'
Check ($failedSolution.Trace.Name -contains 'test-all') 'Independent project tests still execute after the solution check fails.'

$old = Run-Case (New-Case 'old-sdk') 'Quick' -OldSdk
Check ($old.Code -ne 0 -and $old.Trace.Name -notcontains 'dotnet-format') 'An SDK below 8 is rejected before format/build/tests.'
$listRoot = New-Case 'list-only'
$list = Run-Case $listRoot 'Pure' -ListOnly
Check ($list.Code -eq 0 -and $list.Trace.Count -eq 0) 'ListOnly executes no child processes.'
Check (-not (Test-Path -LiteralPath (Join-Path $listRoot 'artifacts'))) 'ListOnly creates no report or scratch directory.'

$profileRoot = New-Case 'default-profile'
$profile = Run-Case $profileRoot 'Quick' -ScratchLocation Profile
Check ($profile.Code -eq 0 -and $profile.Report.ExitCode -eq 0 -and (Test-Path -LiteralPath $profile.Artifacts)) 'Default verification retains its report under USERPROFILE/.cache/Schedule1Tools/verification.'
Check (-not (Test-Path -LiteralPath (Join-Path $profileRoot 'l/Schedule1Tools/verification'))) 'USERPROFILE default never writes the available LOCALAPPDATA fallback.'
$local = Run-Case (New-Case 'default-local') 'Quick' -ScratchLocation Local
Check ($local.Code -eq 0 -and $local.Report.ExitCode -eq 0 -and (Test-Path -LiteralPath $local.Artifacts)) 'Missing USERPROFILE falls back to LOCALAPPDATA/Schedule1Tools/verification.'
$defaultListRoot = New-Case 'default-list-only'
$defaultList = Run-Case $defaultListRoot 'Pure' -ListOnly -ScratchLocation Profile
# PowerShell itself may create its startup cache inside a synthetic profile;
# these assertions concern the verifier's own scratch/report destinations.
Check ($defaultList.Code -eq 0 -and $defaultList.Trace.Count -eq 0 -and -not (Test-Path -LiteralPath $defaultList.Artifacts) -and -not (Test-Path -LiteralPath (Join-Path $defaultListRoot 'l/Schedule1Tools/verification'))) 'Default ListOnly neither invokes the SDK nor creates profile/fallback scratch directories.'
Check (-not (Test-Path -LiteralPath (Join-Path $quickRoot 'p/.cache/Schedule1Tools/verification')) -and -not (Test-Path -LiteralPath (Join-Path $quickRoot 'l/Schedule1Tools/verification'))) 'Explicit ScratchRoot leaves both default destinations untouched.'

# Exercise the actual solution generator independently of the fake orchestration.
$generatorRoot = Join-Path $suiteRoot 'generator'
Write-Fixture (Join-Path $generatorRoot 'Source/Mods/Example/src/Example.csproj') '<Project />'
$null = New-Item -ItemType Directory -Path (Join-Path $generatorRoot 'Tools') -Force
$generator = Join-Path $generatorRoot 'Tools/gen-sln.ps1'
Copy-Item -LiteralPath (Join-Path $repoRoot 'Tools/gen-sln.ps1') -Destination $generator
$null = & $pwsh -NoProfile -ExecutionPolicy Bypass -File $generator
Check ($LASTEXITCODE -eq 0) 'Fixture solution generation succeeds.'
$solution = Join-Path $generatorRoot 'Source/Mods/S1Mods.sln'
$initialHash = (Get-FileHash -LiteralPath $solution).Hash
[IO.File]::SetLastWriteTimeUtc($solution, [DateTime]::new(2020, 1, 1, 0, 0, 0, [DateTimeKind]::Utc))
$initialTime = [IO.File]::GetLastWriteTimeUtc($solution)
$null = & $pwsh -NoProfile -ExecutionPolicy Bypass -File $generator -Check
Check ($LASTEXITCODE -eq 0 -and (Get-FileHash -LiteralPath $solution).Hash -eq $initialHash -and [IO.File]::GetLastWriteTimeUtc($solution) -eq $initialTime) 'Matching Check leaves solution bytes and timestamp unchanged.'
$crlf = [IO.File]::ReadAllText($solution).Replace("`n", "`r`n")
[IO.File]::WriteAllText($solution, $crlf)
$crlfHash = (Get-FileHash -LiteralPath $solution).Hash
$null = & $pwsh -NoProfile -ExecutionPolicy Bypass -File $generator -Check
Check ($LASTEXITCODE -eq 0 -and (Get-FileHash -LiteralPath $solution).Hash -eq $crlfHash) 'Check accepts CRLF/LF equivalence without rewriting CRLF.'
Write-Fixture (Join-Path $generatorRoot 'Source/Tests/New.Tests/New.Tests.csproj') '<Project />'
$null = & $pwsh -NoProfile -ExecutionPolicy Bypass -File $generator -Check
Check ($LASTEXITCODE -ne 0 -and (Get-FileHash -LiteralPath $solution).Hash -eq $crlfHash) 'A new project causes deterministic-check failure without updating the solution.'
Remove-Item -LiteralPath $solution
$null = & $pwsh -NoProfile -ExecutionPolicy Bypass -File $generator -Check
Check ($LASTEXITCODE -ne 0 -and -not (Test-Path -LiteralPath $solution)) 'Missing solution fails Check without creating one.'

Write-Host "RESULT: $script:passed/$($script:passed + $script:failed) passed; $script:failed failed"
Write-Host "Fixtures: $suiteRoot"
if ($script:failed -gt 0) { exit 1 }
exit 0
