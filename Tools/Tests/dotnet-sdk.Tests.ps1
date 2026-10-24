#Requires -Version 7
# Fake hosts only: no SDK build, game access, network or machine configuration.
[CmdletBinding()]
param([string]$ScratchRoot)

$ErrorActionPreference = 'Stop'
$repo = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
if (-not $ScratchRoot) { $ScratchRoot = Join-Path $env:USERPROFILE '.cache/Schedule1Tools/tests' }
$scratch = Join-Path ([IO.Path]::GetFullPath($ScratchRoot)) ('sdk-' + [guid]::NewGuid().ToString('N').Substring(0, 12))
$null = New-Item -ItemType Directory -Path $scratch -Force
$script:passed = 0
$script:failed = 0
function Check([bool]$Condition, [string]$Name) {
    if ($Condition) { $script:passed++ } else { $script:failed++; Write-Host "FAIL: $Name" }
}
function Reject([scriptblock]$Body, [string]$Pattern, [string]$Name) {
    $message = ''
    try { & $Body | Out-Null } catch { $message = $_.Exception.Message }
    Check ($message -match $Pattern) "$Name (observed: $message)"
}
function Write-HostFixture([string]$Directory, [string]$Name = 'dotnet.cmd', [string]$Version = '8.0.999', [string]$Sdks = '8.0.999 [C:\fake\sdk]', [int]$VersionExit = 0, [int]$SdksExit = 0) {
    $null = New-Item -ItemType Directory -Path $Directory -Force
    $text = @'
@echo off
echo %*>> "%~dp0calls.txt"
if "%~1"=="--version" (
  echo __VERSION__
  exit /b __VERSION_EXIT__
)
if "%~1"=="--list-sdks" (
  __SDKS__
  exit /b __SDKS_EXIT__
)
exit /b 9
'@
    $sdksCommand = if ($Sdks) { 'echo ' + $Sdks } else { 'rem no installed SDK' }
    $text = $text.Replace('__VERSION__', $Version).Replace('__SDKS__', $sdksCommand).Replace('__VERSION_EXIT__', [string]$VersionExit).Replace('__SDKS_EXIT__', [string]$SdksExit)
    $path = Join-Path $Directory $Name
    [IO.File]::WriteAllText($path, $text)
    return $path
}

$savedPath = $env:PATH
$savedProfile = $env:USERPROFILE
$savedRoot = $env:DOTNET_ROOT
$savedLocation = Get-Location
try {
    $env:DOTNET_ROOT = 'unchanged-fixture-value'
    $onPath = Write-HostFixture (Join-Path $scratch 'path host')
    $explicit = Write-HostFixture (Join-Path $scratch 'explicit host') 'chosen.cmd'
    $env:USERPROFILE = Join-Path $scratch 'profile'
    $localDir = Join-Path $env:USERPROFILE '.dotnet'
    $null = New-Item -ItemType Directory -Path $localDir -Force
    $local = Join-Path $localDir 'dotnet.exe'
    [IO.File]::WriteAllText($local, 'resolver-only fixture, never executed')
    $env:PATH = Split-Path $onPath -Parent
    Set-Location -LiteralPath $scratch
    . (Join-Path $repo 'Tools/dotnet-sdk.ps1')
    Check (-not (Test-Path -LiteralPath (Join-Path (Split-Path $onPath -Parent) 'calls.txt'))) 'Loading helpers never invokes a host.'
    Check ((Resolve-S1DotnetPath -Requested $explicit) -eq $explicit) 'Explicit path wins over PATH and local.'
    Check ((Resolve-S1DotnetPath -Requested 'dotnet') -eq $onPath) 'Explicit application name resolves on PATH.'
    Reject { Resolve-S1DotnetPath -Requested (Join-Path $scratch 'missing.exe') } 'not found' 'Missing explicit host never silently falls back.'
    Reject { Resolve-S1DotnetPath -Requested $localDir } 'not found' 'Directories are not executable hosts.'
    Check ((Resolve-S1DotnetPath) -eq $onPath) 'PATH host wins over local host.'
    $env:PATH = ''
    Check ((Resolve-S1DotnetPath) -eq $local) 'Local host is the fallback when PATH has none.'
    Move-Item -LiteralPath $local -Destination (Join-Path $localDir 'tool-only.txt')
    Reject { Resolve-S1DotnetPath } 'not found' 'A tools-only local directory is not an SDK host.'
    $env:USERPROFILE = $null
    Reject { Resolve-S1DotnetPath } 'not found' 'Missing profile produces a clear discovery failure.'
    Check ((Assert-S1DotnetSdk -DotnetPath $explicit) -eq '8.0.999') 'Valid active SDK is accepted.'
    $old = Write-HostFixture (Join-Path $scratch 'old') -Version '6.0.100' -Sdks '8.0.999 [C:\fake\sdk]'
    Reject { Assert-S1DotnetSdk -DotnetPath $old } 'active.*8 or newer' 'Active SDK 6 is rejected even when SDK 8 is installed.'
    $runtime = Write-HostFixture (Join-Path $scratch 'runtime') -Version 'No SDK found' -VersionExit 4
    Reject { Assert-S1DotnetSdk -DotnetPath $runtime } '--version failed.*4' 'Runtime-only host failure retains its exit code.'
    $empty = Write-HostFixture (Join-Path $scratch 'empty') -Sdks ''
    Reject { Assert-S1DotnetSdk -DotnetPath $empty } 'No .NET SDK is installed' 'Empty SDK inventory is rejected.'
    $failedList = Write-HostFixture (Join-Path $scratch 'failed-list') -SdksExit 5
    Reject { Assert-S1DotnetSdk -DotnetPath $failedList } '--list-sdks failed.*5' 'SDK inventory failure is not a pass.'
    Reject { Assert-S1SdkVersion '8.0.999 extra text' } 'no valid SDK version' 'Malformed SDK output is rejected.'
    Check ((Assert-S1SdkVersion '10.0.100-preview.1') -eq '10.0.100-preview.1') 'Newer SDK version suffixes remain supported.'
    Check ($env:DOTNET_ROOT -eq 'unchanged-fixture-value') 'Helpers never change DOTNET_ROOT.'
}
finally {
    $env:PATH = $savedPath
    $env:USERPROFILE = $savedProfile
    $env:DOTNET_ROOT = $savedRoot
    Set-Location -LiteralPath $savedLocation
}
Write-Host "dotnet-sdk: $script:passed passed, $script:failed failed. Fixtures: $scratch"
if ($script:failed) { exit 1 }
exit 0
