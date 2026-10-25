#Requires -Version 7
<#
.SYNOPSIS
    Game-free fixture tests for the tracked MSBuild XML guard.
#>
[CmdletBinding()]
param([string]$ScratchRoot)

$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
$checker = Join-Path $repoRoot 'Tools/check-xml.ps1'
$gitExecutable = (Get-Command git -CommandType Application -ErrorAction Stop | Select-Object -First 1).Source
if (-not $ScratchRoot) { $ScratchRoot = [IO.Path]::GetTempPath() }
$scratchFull = [IO.Path]::GetFullPath($ScratchRoot)
$repoFull = [IO.Path]::GetFullPath($repoRoot).TrimEnd('\', '/')
if ($scratchFull.Equals($repoFull, [StringComparison]::OrdinalIgnoreCase) -or
    $scratchFull.StartsWith($repoFull + [IO.Path]::DirectorySeparatorChar, [StringComparison]::OrdinalIgnoreCase)) {
    throw 'Fixture scratch must be outside the repository.'
}
$scratch = Join-Path $scratchFull ('xml-' + [guid]::NewGuid().ToString('N').Substring(0, 12))
$null = New-Item -ItemType Directory -Path $scratch
$script:passed = 0
$script:failed = 0

function Check([bool]$Condition, [string]$Message) {
    if ($Condition) { $script:passed++ }
    else { $script:failed++; Write-Host "FAIL: $Message" -ForegroundColor Red }
}

function Write-File([string]$Root, [string]$Path, [string]$Content) {
    $full = Join-Path $Root $Path
    $null = New-Item -ItemType Directory -Path (Split-Path $full -Parent) -Force
    [IO.File]::WriteAllText($full, ($Content -replace "`r`n", "`n"), [Text.UTF8Encoding]::new($false))
}

function Git([string]$Root, [string[]]$Arguments) {
    $output = & $gitExecutable -C $Root @Arguments 2>&1 | Out-String
    if ($LASTEXITCODE -ne 0) { throw "Fixture git failed: $output" }
    return $output.Trim()
}

function New-Fixture([string]$Name) {
    $root = Join-Path $scratch $Name
    $null = New-Item -ItemType Directory -Path (Join-Path $root 'Tools') -Force
    $null = Git $root @('init', '-q')
    Copy-Item -LiteralPath $checker -Destination (Join-Path $root 'Tools/check-xml.ps1')
    return $root
}

function Run-Fixture([string]$Root) {
    $output = & pwsh -NoProfile -NonInteractive -ExecutionPolicy Bypass -File (Join-Path $Root 'Tools/check-xml.ps1') 2>&1 | Out-String
    $code = $LASTEXITCODE
    $receipt = Join-Path $scratch ("run-{0}.txt" -f (Split-Path $Root -Leaf))
    [IO.File]::WriteAllText($receipt, "ExitCode: $code`n$output", [Text.UTF8Encoding]::new($false))
    return [pscustomobject]@{ Code = $code; Output = $output }
}

$validProject = @'
<Project>
  <PropertyGroup>
    <TargetFramework>net6.0</TargetFramework>
  </PropertyGroup>
</Project>
'@
$validProps = '<Project><PropertyGroup><ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally></PropertyGroup></Project>'
$validTargets = @'
<Project>
  <Target Name="DeployMod" />
  <Target Name="DeployShared" />
</Project>
'@
$missingQuotes = @'
<Project>
  <Target Name=DeployMod />
  <Target Name=DeployShared />
</Project>
'@

$root = New-Fixture 'valid'
Write-File $root 'Source/Mods/Example/src/Example.csproj' $validProject
Write-File $root 'Source/Mods/Directory.Build.props' $validProps
Write-File $root 'Source/Archive/Directory.Build.targets' $validTargets
$null = Git $root @('add', '--', 'Source/Mods/Example/src/Example.csproj', 'Source/Mods/Directory.Build.props', 'Source/Archive/Directory.Build.targets')
Write-File $root 'Source/Mods/Untracked/Untracked.csproj' $missingQuotes
$result = Run-Fixture $root
Check ($result.Code -eq 0) "tracked valid XML passes: $($result.Output)"
Check ($result.Output -match 'XML check \(3 files\)') 'untracked invalid project is not part of the tracked inventory'
Check ($result.Output -notmatch 'Untracked\.csproj') 'untracked invalid project is not reported'

$root = New-Fixture 'invalid'
Write-File $root 'Source/Archive/Directory.Build.targets' $missingQuotes
$null = Git $root @('add', '--', 'Source/Archive/Directory.Build.targets')
$result = Run-Fixture $root
Check ($result.Code -eq 1) 'missing attribute quotes fail'
Check ($result.Output.Contains('Source/Archive/Directory.Build.targets')) 'invalid XML names the tracked file'
Check ($result.Output -match 'Invalid XML: 1') 'one invalid file is reported once'

Write-Host "XML tests: $script:passed passed, $script:failed failed. Receipts: $scratch"
if ($script:failed -gt 0) { exit 1 }
exit 0
