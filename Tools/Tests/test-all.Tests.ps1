#Requires -Version 7
<#
.SYNOPSIS
    Regression suite for Tools/test-all.ps1 (no Pester required).

.DESCRIPTION
    Every case builds a disposable copy of the runner inside its own fixture
    repository below a scratch directory outside the real repository, so the
    actual Source/Tests projects, mods and docs are never touched.

    Two kinds of contract are covered:

      * Process/verdict contracts, driven by a fake dotnet on PATH-equivalent
        (the runner's -DotnetPath). The fake records its arguments and writes a
        TRX file whose counters the case chooses. This makes it cheap to prove
        that zero-test runs, missing TRX files, malformed counters, skipped
        selections, unsafe classifications and SDK preflight failures all fail.
      * Real xunit fixtures, executed by the real dotnet: one project with a
        single passing [Fact] (must pass) and one project with no facts at all
        (xunit exits 0 - the runner must still fail it).

.EXAMPLE
    pwsh Tools/Tests/test-all.Tests.ps1
    pwsh Tools/Tests/test-all.Tests.ps1 -ScratchRoot D:\temp\test-all-tests -KeepArtifacts
#>
[CmdletBinding()]
param(
    [string]$ScratchRoot,
    [switch]$KeepArtifacts
)

$ErrorActionPreference = "Stop"

$repoRoot = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
$script:RunnerScript = Join-Path $repoRoot "Tools/test-all.ps1"
if (-not (Test-Path -LiteralPath $script:RunnerScript)) {
    throw "Runner under test not found: $script:RunnerScript"
}

# Fixtures live in an explicit Hermes scratch directory, never in TMPDIR (which is
# observed to point at the OS temp dir) and never inside the repository.
if (-not $ScratchRoot) {
    if ($env:TEST_ALL_TEST_SCRATCH) { $ScratchRoot = $env:TEST_ALL_TEST_SCRATCH }
    elseif ($env:LOCALAPPDATA) { $ScratchRoot = Join-Path $env:LOCALAPPDATA "hermes\cache\scratch" }
    elseif ($env:USERPROFILE) { $ScratchRoot = Join-Path $env:USERPROFILE ".cache\hermes\scratch" }
    else { throw "No scratch directory available (set TEST_ALL_TEST_SCRATCH)." }
}
$ScratchRoot = [System.IO.Path]::GetFullPath($ScratchRoot)
$repoFull = [System.IO.Path]::GetFullPath($repoRoot).TrimEnd('\', '/')
if ($ScratchRoot.Equals($repoFull, [System.StringComparison]::OrdinalIgnoreCase) -or
    $ScratchRoot.StartsWith($repoFull + [System.IO.Path]::DirectorySeparatorChar, [System.StringComparison]::OrdinalIgnoreCase)) {
    throw "ScratchRoot must not be inside the repository: $ScratchRoot"
}
$systemTemp = [System.IO.Path]::GetFullPath([System.IO.Path]::GetTempPath()).TrimEnd('\', '/')
if ($ScratchRoot.TrimEnd('\', '/').Equals($systemTemp, [System.StringComparison]::OrdinalIgnoreCase) -or
    $ScratchRoot.StartsWith($systemTemp + [System.IO.Path]::DirectorySeparatorChar, [System.StringComparison]::OrdinalIgnoreCase)) {
    throw "ScratchRoot must not be the OS temp directory: $ScratchRoot"
}
# Leave room for SDK obj/bin paths when nested inside an isolated verification
# run. Long assembly paths can surface as misleading MSB3030 missing-file errors.
$ScratchRoot = Join-Path $ScratchRoot ("tat-" + [guid]::NewGuid().ToString('N').Substring(0, 12))
$null = New-Item -ItemType Directory -Path $ScratchRoot -Force

$script:Passed = 0
$script:Failed = 0
$script:Failures = @()
$utf8NoBom = New-Object System.Text.UTF8Encoding($false)

function Check {
    param([bool]$Condition, [string]$Message)
    if ($Condition) { $script:Passed++ }
    else {
        $script:Failed++
        $script:Failures += $Message
        Write-Host "  FAIL: $Message" -ForegroundColor Red
    }
}

function Check-Equal {
    param($Expected, $Actual, [string]$Message)
    Check ($Expected -eq $Actual) "$Message (expected '$Expected', got '$Actual')"
}

function Check-Match {
    param([string]$Text, [string]$Pattern, [string]$Message)
    Check ($Text -match $Pattern) "$Message (pattern '$Pattern' not found)"
}

function Check-NotMatch {
    param([string]$Text, [string]$Pattern, [string]$Message)
    Check (-not ($Text -match $Pattern)) "$Message (unexpected pattern '$Pattern')"
}

function Write-FixtureFile {
    param([string]$Root, [string]$Relative, [string]$Content)
    $full = Join-Path $Root $Relative
    $parent = Split-Path $full -Parent
    if (-not (Test-Path -LiteralPath $parent)) { $null = New-Item -ItemType Directory -Path $parent -Force }
    [System.IO.File]::WriteAllText($full, $Content, $utf8NoBom)
}

$fakeDotnetPs1 = @'
# No param() block on purpose: the runner passes '-p:S1NoDeploy=true', which a
# param block would try to bind as a parameter name.
$ErrorActionPreference = 'Stop'
$ArgList = @($args)

$behavior = $null
$behaviorPath = $env:FAKE_DOTNET_BEHAVIOR
if ($behaviorPath -and (Test-Path -LiteralPath $behaviorPath)) {
    $raw = [System.IO.File]::ReadAllText($behaviorPath)
    if ($raw.Trim()) { $behavior = $raw | ConvertFrom-Json }
}

function Get-Value {
    param($Object, [string]$Name, $Default)
    if ($null -ne $Object -and $null -ne $Object.PSObject.Properties[$Name]) { return $Object.$Name }
    return $Default
}

$verb = if ($ArgList.Count -gt 0) { $ArgList[0] } else { '' }

if ($verb -eq '--version') {
    $text = [string](Get-Value $behavior 'version' '8.0.999')
    Write-Output $text
    exit ([int](Get-Value $behavior 'versionExit' 0))
}

if ($verb -eq '--list-sdks') {
    $lines = @(Get-Value $behavior 'sdks' @('8.0.999 [C:\fake\sdk]'))
    foreach ($line in $lines) { Write-Output $line }
    exit ([int](Get-Value $behavior 'sdksExit' 0))
}

if ($verb -eq 'test') {
    $test = Get-Value $behavior 'test' $null
    $kind = [string](Get-Value $test 'trx' 'pass')
    $exitCode = [int](Get-Value $test 'exitCode' 0)

    $trxName = $null
    $loggerIndex = [Array]::IndexOf($ArgList, '--logger')
    if ($loggerIndex -ge 0 -and $loggerIndex + 1 -lt $ArgList.Count) {
        if ($ArgList[$loggerIndex + 1] -match 'LogFileName=(?<name>.+)$') { $trxName = $Matches['name'] }
    }
    $resultsIndex = [Array]::IndexOf($ArgList, '--results-directory')
    $resultsDir = if ($resultsIndex -ge 0 -and $resultsIndex + 1 -lt $ArgList.Count) { $ArgList[$resultsIndex + 1] } else { (Get-Location).Path }

    if ($kind -ne 'missing' -and $trxName) {
        $null = New-Item -ItemType Directory -Path $resultsDir -Force
        $counters = switch ($kind) {
            'pass' { 'total="1" executed="1" passed="1" failed="0"' }
            'zero' { 'total="0" executed="0" passed="0" failed="0"' }
            'fail' { 'total="2" executed="2" passed="1" failed="1"' }
            'invalid' { 'total="1" executed="many" passed="1" failed="0"' }
            'missing-attribute' { 'total="1" passed="1" failed="0"' }
            'inconsistent' { 'total="0" executed="1" passed="1" failed="0"' }
            default { throw "Unknown fake TRX kind '$kind'" }
        }
        $xml = "<?xml version=`"1.0`" encoding=`"utf-8`"?>`n" +
               "<TestRun xmlns=`"http://microsoft.com/schemas/VisualStudio/TeamTest/2010`">`n" +
               "  <ResultSummary outcome=`"Completed`">`n" +
               "    <Counters $counters />`n" +
               "  </ResultSummary>`n" +
               "</TestRun>`n"
        [System.IO.File]::WriteAllText((Join-Path $resultsDir $trxName), $xml)
    }

    if ($kind -eq 'pass') { Write-Output 'Passed!  - Failed: 0, Passed: 1' }
    else { Write-Output "fake dotnet test: $kind (exit $exitCode)" }
    exit $exitCode
}

if ($verb -eq 'run') {
    $run = Get-Value $behavior 'run' $null
    foreach ($line in @(Get-Value $run 'output' @())) { Write-Output ([string]$line) }
    exit ([int](Get-Value $run 'exitCode' 0))
}

Write-Output "fake dotnet: unsupported verb '$verb'"
exit 2
'@

$fakeDotnetCmd = @'
@echo off
rem Records the raw command line: cmd's %* keeps '-p:S1NoDeploy=true' intact,
rem which PowerShell argument binding would otherwise split.
if not "%FAKE_DOTNET_RECORD%"=="" echo %*>> "%FAKE_DOTNET_RECORD%"
pwsh -NoProfile -NoLogo -File "%~dp0fake-dotnet.ps1" %*
exit /b %ERRORLEVEL%
'@

$xunitProjectTemplate = @'
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net6.0</TargetFramework>
    <Nullable>enable</Nullable>
    <IsPackable>false</IsPackable>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.13.0" />
    <PackageReference Include="xunit" Version="2.9.3" />
    <PackageReference Include="xunit.runner.visualstudio" Version="3.0.2" />
  </ItemGroup>
</Project>
'@

$gameRequiredProjectTemplate = @'
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net6.0</TargetFramework>
    <IsPackable>false</IsPackable>
  </PropertyGroup>
  <ItemGroup>
    <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.13.0" />
    <PackageReference Include="xunit" Version="2.9.3" />
    <PackageReference Include="xunit.runner.visualstudio" Version="3.0.2" />
  </ItemGroup>
  <ItemGroup>
    <ProjectReference Include="..\..\Mods\Shared\src\Shared.csproj" />
  </ItemGroup>
</Project>
'@

$consoleProjectTemplate = @'
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net6.0</TargetFramework>
    <Nullable>enable</Nullable>
  </PropertyGroup>
</Project>
'@

function New-FixtureRepo {
    param([Parameter(Mandatory)][string]$Name)

    $root = Join-Path $ScratchRoot $Name
    if (Test-Path -LiteralPath $root) { Remove-Item -LiteralPath $root -Recurse -Force }
    $null = New-Item -ItemType Directory -Path (Join-Path $root "Tools") -Force
    $null = New-Item -ItemType Directory -Path (Join-Path $root "Source\Tests") -Force
    Copy-Item -LiteralPath $script:RunnerScript -Destination (Join-Path $root "Tools/test-all.ps1")
    Copy-Item -LiteralPath (Join-Path $repoRoot 'Tools/dotnet-sdk.ps1') -Destination (Join-Path $root 'Tools/dotnet-sdk.ps1')
    Write-FixtureFile $root "fake-dotnet.ps1" $fakeDotnetPs1
    Write-FixtureFile $root "fake-dotnet.cmd" $fakeDotnetCmd
    Write-FixtureFile $root "fake-behavior.json" "{}"
    return $root
}

function Add-XunitProject {
    param(
        [Parameter(Mandatory)][string]$Root,
        [Parameter(Mandatory)][string]$Name,
        [switch]$GameRequired,
        [string]$SourceText
    )
    $relative = "Source/Tests/$Name/$Name.csproj"
    Write-FixtureFile $Root $relative $(if ($GameRequired) { $gameRequiredProjectTemplate } else { $xunitProjectTemplate })
    if ($SourceText) { Write-FixtureFile $Root "Source/Tests/$Name/Tests.cs" $SourceText }
}

function Add-ConsoleProject {
    param(
        [Parameter(Mandatory)][string]$Root,
        [string]$Name = "LightControl.Tests"
    )
    Write-FixtureFile $Root "Source/Tests/$Name/$Name.csproj" $consoleProjectTemplate
    Write-FixtureFile $Root "Source/Tests/$Name/Program.cs" "internal static class Program { private static int Main() => 0; }"
}

function Invoke-Runner {
    param(
        [Parameter(Mandatory)][string]$Root,
        [string[]]$RunnerArgs = @(),
        [hashtable]$Behavior = @{},
        [switch]$RealDotnet,
        [hashtable]$Environment = @{}
    )

    $behaviorPath = Join-Path $Root "fake-behavior.json"
    $recordPath = Join-Path $Root "fake-invocations.jsonl"
    Remove-Item -LiteralPath $recordPath -Force -ErrorAction SilentlyContinue
    if (-not $RealDotnet) {
        ($Behavior | ConvertTo-Json -Depth 10) | Set-Content -LiteralPath $behaviorPath -Encoding utf8
    }

    $savedBehavior = $env:FAKE_DOTNET_BEHAVIOR
    $savedRecord = $env:FAKE_DOTNET_RECORD
    $savedEnv = @{}
    foreach ($key in $Environment.Keys) {
        $savedEnv[$key] = [Environment]::GetEnvironmentVariable($key, "Process")
        [Environment]::SetEnvironmentVariable($key, [string]$Environment[$key], "Process")
    }
    if (-not $RealDotnet) {
        $env:FAKE_DOTNET_BEHAVIOR = $behaviorPath
        $env:FAKE_DOTNET_RECORD = $recordPath
    }

    try {
        $runnerPath = Join-Path $Root "Tools/test-all.ps1"
        $cliArgs = @("-NoProfile", "-ExecutionPolicy", "Bypass", "-File", $runnerPath) + $RunnerArgs
        $output = & pwsh @cliArgs 2>&1 | Out-String
        $code = $LASTEXITCODE
    }
    finally {
        $env:FAKE_DOTNET_BEHAVIOR = $savedBehavior
        $env:FAKE_DOTNET_RECORD = $savedRecord
        foreach ($key in $Environment.Keys) { [Environment]::SetEnvironmentVariable($key, $savedEnv[$key], "Process") }
    }

    $invocations = @()
    if (Test-Path -LiteralPath $recordPath) {
        $invocations = @(
            Get-Content -LiteralPath $recordPath |
                Where-Object { $_.Trim() }
        )
    }

    return [pscustomobject]@{
        ExitCode    = $code
        Output      = $output
        Invocations = $invocations
        Root        = $Root
    }
}

function Get-FakeDotnetPath {
    param([string]$Root)
    return (Join-Path $Root "fake-dotnet.cmd")
}

Write-Host ""
Write-Host "test-all.Tests.ps1 | scratch: $ScratchRoot" -ForegroundColor Cyan
Write-Host ""

# ---------------------------------------------------------------------------
# 1. xunit verdicts from TRX counters (fake dotnet: exit code is not evidence)
# ---------------------------------------------------------------------------
Write-Host "xunit TRX contracts" -ForegroundColor Cyan

$fixture = New-FixtureRepo "xunit-pass"
Add-XunitProject -Root $fixture -Name "Sample.Tests"
$result = Invoke-Runner -Root $fixture -RunnerArgs @("-Selection", "pure", "-ScratchRoot", (Join-Path $fixture "artifacts"), "-DotnetPath", (Get-FakeDotnetPath $fixture)) -Behavior @{ test = @{ trx = "pass"; exitCode = 0 } }
Check-Equal 0 $result.ExitCode "one passing xunit project passes"
Check-Match $result.Output '1/1 passed' "passing project reports its counters"

$fixture = New-FixtureRepo "xunit-zero"
Add-XunitProject -Root $fixture -Name "Empty.Tests"
$result = Invoke-Runner -Root $fixture -RunnerArgs @("-Selection", "pure", "-ScratchRoot", (Join-Path $fixture "artifacts"), "-DotnetPath", (Get-FakeDotnetPath $fixture)) -Behavior @{ test = @{ trx = "zero"; exitCode = 0 } }
Check-Equal 1 $result.ExitCode "zero executed tests fails even though dotnet test exited 0"
Check-Match $result.Output 'executed=0' "zero-test failure names the executed counter"

$fixture = New-FixtureRepo "xunit-missing-trx"
Add-XunitProject -Root $fixture -Name "NoTrx.Tests"
$result = Invoke-Runner -Root $fixture -RunnerArgs @("-Selection", "pure", "-ScratchRoot", (Join-Path $fixture "artifacts"), "-DotnetPath", (Get-FakeDotnetPath $fixture)) -Behavior @{ test = @{ trx = "missing"; exitCode = 0 } }
Check-Equal 1 $result.ExitCode "a missing TRX fails even with exit code 0"
Check-Match $result.Output 'produced no TRX' "missing TRX failure is explicit"

$fixture = New-FixtureRepo "xunit-invalid"
Add-XunitProject -Root $fixture -Name "BadCounters.Tests"
$result = Invoke-Runner -Root $fixture -RunnerArgs @("-Selection", "pure", "-ScratchRoot", (Join-Path $fixture "artifacts"), "-DotnetPath", (Get-FakeDotnetPath $fixture)) -Behavior @{ test = @{ trx = "invalid"; exitCode = 0 } }
Check-Equal 1 $result.ExitCode "non-numeric TRX counters fail"
Check-Match $result.Output 'Invalid TRX' "invalid counter failure is explicit"

$fixture = New-FixtureRepo "xunit-missing-counter"
Add-XunitProject -Root $fixture -Name "NoExecuted.Tests"
$result = Invoke-Runner -Root $fixture -RunnerArgs @("-Selection", "pure", "-ScratchRoot", (Join-Path $fixture "artifacts"), "-DotnetPath", (Get-FakeDotnetPath $fixture)) -Behavior @{ test = @{ trx = "missing-attribute"; exitCode = 0 } }
Check-Equal 1 $result.ExitCode "an absent 'executed' counter fails"
Check-Match $result.Output "missing the 'executed' attribute" "absent counter is named"

$fixture = New-FixtureRepo "xunit-failing"
Add-XunitProject -Root $fixture -Name "Failing.Tests"
$result = Invoke-Runner -Root $fixture -RunnerArgs @("-Selection", "pure", "-ScratchRoot", (Join-Path $fixture "artifacts"), "-DotnetPath", (Get-FakeDotnetPath $fixture)) -Behavior @{ test = @{ trx = "fail"; exitCode = 1 } }
Check-Equal 1 $result.ExitCode "failed xunit tests fail the run"
Check-Match $result.Output '1 failed test' "failure count comes from the TRX"

$fixture = New-FixtureRepo "xunit-exit0-but-failing"
Add-XunitProject -Root $fixture -Name "Quiet.Tests"
$result = Invoke-Runner -Root $fixture -RunnerArgs @("-Selection", "pure", "-ScratchRoot", (Join-Path $fixture "artifacts"), "-DotnetPath", (Get-FakeDotnetPath $fixture)) -Behavior @{ test = @{ trx = "fail"; exitCode = 0 } }
Check-Equal 1 $result.ExitCode "TRX failures fail the project even when dotnet test exits 0"

# ---------------------------------------------------------------------------
# 2. S1NoDeploy must travel as an explicit MSBuild property
# ---------------------------------------------------------------------------
Write-Host "deploy guard" -ForegroundColor Cyan

$fixture = New-FixtureRepo "no-deploy"
Add-XunitProject -Root $fixture -Name "Unit.Tests"
Add-ConsoleProject -Root $fixture -Name "LightControl.Tests"
$result = Invoke-Runner -Root $fixture -RunnerArgs @("-Selection", "pure", "-ScratchRoot", (Join-Path $fixture "artifacts"), "-DotnetPath", (Get-FakeDotnetPath $fixture)) -Behavior @{ test = @{ trx = "pass"; exitCode = 0 }; run = @{ output = @("RESULT: 2/2 passed; 0 failed."); exitCode = 0 } }
$testInvocations = @($result.Invocations | Where-Object { $_ -match '^test\s' })
$runInvocations = @($result.Invocations | Where-Object { $_ -match '^run\s' })
Check-Equal 1 $testInvocations.Count "xunit project is executed with dotnet test"
Check-Equal 1 $runInvocations.Count "console project is executed with dotnet run"
foreach ($text in @($testInvocations + $runInvocations)) {
    Check ($text -match '\-p:S1NoDeploy=true') "every dotnet invocation carries -p:S1NoDeploy=true ($($text.Split(' ')[0]))"
}

# ---------------------------------------------------------------------------
# 3. Per-project unique TRX files, outside the OS temp directory
# ---------------------------------------------------------------------------
Write-Host "TRX placement" -ForegroundColor Cyan

$fixture = New-FixtureRepo "trx-unique"
Add-XunitProject -Root $fixture -Name "One.Tests"
Add-XunitProject -Root $fixture -Name "Two.Tests"
$artifacts = Join-Path $fixture "artifacts"
$result = Invoke-Runner -Root $fixture -RunnerArgs @("-Selection", "pure", "-ScratchRoot", $artifacts, "-DotnetPath", (Get-FakeDotnetPath $fixture)) -Behavior @{ test = @{ trx = "pass"; exitCode = 0 } }
Check-Equal 0 $result.ExitCode "two passing projects pass"
$trxFiles = @(Get-ChildItem -LiteralPath $artifacts -Recurse -Filter "*.trx" -File)
Check-Equal 2 $trxFiles.Count "one TRX per project"
Check-Equal 2 @($trxFiles | Select-Object -ExpandProperty Name -Unique).Count "TRX file names are unique"
Check (@($trxFiles | Where-Object { $_.FullName -match '[\\/]One\.Tests[\\/]' }).Count -eq 1) "One.Tests TRX lives in its own project directory"
Check (@($trxFiles | Where-Object { $_.FullName -match '[\\/]Two\.Tests[\\/]' }).Count -eq 1) "Two.Tests TRX lives in its own project directory"
Check (@($trxFiles | Where-Object { $_.FullName.StartsWith($systemTemp, [System.StringComparison]::OrdinalIgnoreCase) }).Count -eq 0) "TRX files are not written below the OS temp directory"

$fixture = New-FixtureRepo "scratch-fallback"
Add-XunitProject -Root $fixture -Name "Fallback.Tests"
# TMPDIR points at the OS temp dir, the situation observed on this host: the runner
# must fall through to the Hermes scratch dir instead of writing there.
$result = Invoke-Runner -Root $fixture -RunnerArgs @("-Selection", "pure", "-DotnetPath", (Get-FakeDotnetPath $fixture)) -Behavior @{ test = @{ trx = "pass"; exitCode = 0 } } -Environment @{ TMPDIR = $systemTemp; TEST_ALL_SCRATCH = "" }
Check-Equal 0 $result.ExitCode "run succeeds when TMPDIR is the OS temp directory"
$artifactLine = ($result.Output -split "`r?`n" | Where-Object { $_ -match '^Artifacts: ' } | Select-Object -First 1)
Check ($artifactLine -and -not $artifactLine.Substring("Artifacts: ".Length).StartsWith($systemTemp, [System.StringComparison]::OrdinalIgnoreCase)) "artifacts do not land in the OS temp directory when TMPDIR points there"
Check ($artifactLine -match 'hermes[\\/]cache[\\/]scratch') "artifacts fall back to the Hermes scratch directory"

# ---------------------------------------------------------------------------
# 4. Console host contract (LightControl only, RESULT line, no fixed minimum)
# ---------------------------------------------------------------------------
Write-Host "console contract" -ForegroundColor Cyan

$fixture = New-FixtureRepo "console-pass"
Add-ConsoleProject -Root $fixture -Name "LightControl.Tests"
$result = Invoke-Runner -Root $fixture -RunnerArgs @("-Selection", "pure", "-ScratchRoot", (Join-Path $fixture "artifacts"), "-DotnetPath", (Get-FakeDotnetPath $fixture)) -Behavior @{ run = @{ output = @("LightControl.Core behavioral checks | 4 cases", "PASS C01", "RESULT: 4/4 passed; 0 failed. Pure-model simulation only."); exitCode = 0 } }
Check-Equal 0 $result.ExitCode "a small passing console host passes (no hard-coded case minimum)"
Check-Match $result.Output '4/4 passed' "console counters are reported"

$fixture = New-FixtureRepo "console-failures"
Add-ConsoleProject -Root $fixture -Name "LightControl.Tests"
$result = Invoke-Runner -Root $fixture -RunnerArgs @("-Selection", "pure", "-ScratchRoot", (Join-Path $fixture "artifacts"), "-DotnetPath", (Get-FakeDotnetPath $fixture)) -Behavior @{ run = @{ output = @("RESULT: 2/3 passed; 1 failed."); exitCode = 0 } }
Check-Equal 1 $result.ExitCode "console failures fail even with exit code 0"
Check-Match $result.Output '1 failed case' "console failure count is reported"

$fixture = New-FixtureRepo "console-zero"
Add-ConsoleProject -Root $fixture -Name "LightControl.Tests"
$result = Invoke-Runner -Root $fixture -RunnerArgs @("-Selection", "pure", "-ScratchRoot", (Join-Path $fixture "artifacts"), "-DotnetPath", (Get-FakeDotnetPath $fixture)) -Behavior @{ run = @{ output = @("RESULT: 0/0 passed; 0 failed."); exitCode = 0 } }
Check-Equal 1 $result.ExitCode "a degenerate 0/0 console RESULT fails"
Check-Match $result.Output 'denominator is zero' "zero denominator is named"

$fixture = New-FixtureRepo "console-no-result"
Add-ConsoleProject -Root $fixture -Name "LightControl.Tests"
$result = Invoke-Runner -Root $fixture -RunnerArgs @("-Selection", "pure", "-ScratchRoot", (Join-Path $fixture "artifacts"), "-DotnetPath", (Get-FakeDotnetPath $fixture)) -Behavior @{ run = @{ output = @("all good, honestly"); exitCode = 0 } }
Check-Equal 1 $result.ExitCode "console output without RESULT fails"
Check-Match $result.Output 'no RESULT line' "missing RESULT line is named"

$fixture = New-FixtureRepo "console-unknown"
Add-ConsoleProject -Root $fixture -Name "Mystery.Tests"
$result = Invoke-Runner -Root $fixture -RunnerArgs @("-Selection", "pure", "-ScratchRoot", (Join-Path $fixture "artifacts"), "-DotnetPath", (Get-FakeDotnetPath $fixture)) -Behavior @{ run = @{ output = @("RESULT: 1/1 passed; 0 failed."); exitCode = 0 } }
Check-Equal 2 $result.ExitCode "an undeclared console host is a classification error"
Check-Match $result.Output 'no declared output contract' "undeclared console host is named"
Check-Equal 0 @($result.Invocations).Count "an undeclared console host is never executed"

# ---------------------------------------------------------------------------
# 5. Selection rules: nothing executed is a failure, game requirement is explicit
# ---------------------------------------------------------------------------
Write-Host "selection rules" -ForegroundColor Cyan

$fixture = New-FixtureRepo "all-skipped"
Add-XunitProject -Root $fixture -Name "NeedsGame.Tests" -GameRequired
$result = Invoke-Runner -Root $fixture -RunnerArgs @("-Selection", "pure", "-ScratchRoot", (Join-Path $fixture "artifacts"), "-DotnetPath", (Get-FakeDotnetPath $fixture)) -Behavior @{ test = @{ trx = "pass"; exitCode = 0 } }
Check-Equal 1 $result.ExitCode "a selection where every project is skipped fails"
Check-Match $result.Output 'nothing was verified' "all-skipped failure is explicit"
Check-Equal 0 @($result.Invocations | Where-Object { $_ -match '^(test|run)\s' }).Count "nothing is executed when every project is skipped"

$fixture = New-FixtureRepo "game-missing"
Add-XunitProject -Root $fixture -Name "NeedsGame.Tests" -GameRequired
$noGame = Join-Path $fixture "no-game-installed"
$result = Invoke-Runner -Root $fixture -RunnerArgs @("-Selection", "all", "-ScratchRoot", (Join-Path $fixture "artifacts"), "-DotnetPath", (Get-FakeDotnetPath $fixture)) -Behavior @{ test = @{ trx = "pass"; exitCode = 0 } } -Environment @{ SCHEDULE1_PATH = $noGame }
Check-Equal 2 $result.ExitCode "-Selection all without game assemblies is a configuration error"
Check-Match $result.Output 'IL2CPP assemblies' "missing game assemblies are named"

# ---------------------------------------------------------------------------
# 6. Classifier safety and misplaced test projects
# ---------------------------------------------------------------------------
Write-Host "classifier safety" -ForegroundColor Cyan

$fixture = New-FixtureRepo "misplaced"
Add-XunitProject -Root $fixture -Name "Placed.Tests"
Write-FixtureFile $fixture "Source/Mods/SomeMod/SomeMod.Tests.csproj" $xunitProjectTemplate
$result = Invoke-Runner -Root $fixture -RunnerArgs @("-Selection", "pure", "-ScratchRoot", (Join-Path $fixture "artifacts"), "-DotnetPath", (Get-FakeDotnetPath $fixture)) -Behavior @{ test = @{ trx = "pass"; exitCode = 0 } }
Check-Equal 2 $result.ExitCode "a *Tests.csproj under Source/Mods aborts the run"
Check-Match $result.Output 'outside Source/Tests' "misplaced project is named"

$fixture = New-FixtureRepo "misplaced-in-bin"
Add-XunitProject -Root $fixture -Name "Placed.Tests"
Write-FixtureFile $fixture "Source/Mods/SomeMod/bin/Debug/SomeMod.Tests.csproj" $xunitProjectTemplate
$result = Invoke-Runner -Root $fixture -RunnerArgs @("-Selection", "pure", "-ScratchRoot", (Join-Path $fixture "artifacts"), "-DotnetPath", (Get-FakeDotnetPath $fixture)) -Behavior @{ test = @{ trx = "pass"; exitCode = 0 } }
Check-Equal 0 $result.ExitCode "bin/obj copies of a test csproj are ignored"

$fixture = New-FixtureRepo "custom-directory-build"
Add-XunitProject -Root $fixture -Name "Placed.Tests"
Write-FixtureFile $fixture "Source/Tests/Directory.Build.props" "<Project><PropertyGroup><NoWarn>CS9999</NoWarn></PropertyGroup></Project>"
$result = Invoke-Runner -Root $fixture -RunnerArgs @("-Selection", "pure", "-ScratchRoot", (Join-Path $fixture "artifacts"), "-DotnetPath", (Get-FakeDotnetPath $fixture)) -Behavior @{ test = @{ trx = "pass"; exitCode = 0 } }
Check-Equal 2 $result.ExitCode "a Directory.Build.props under Source/Tests aborts the run"
Check-Match $result.Output 'custom MSBuild files' "unsupported MSBuild file is named"

$fixture = New-FixtureRepo "explicit-import"
$importedProject = $xunitProjectTemplate.Replace("</Project>", "  <Import Project=`"extra.targets`" />`n</Project>")
Write-FixtureFile $fixture "Source/Tests/Imported.Tests/Imported.Tests.csproj" $importedProject
$result = Invoke-Runner -Root $fixture -RunnerArgs @("-Selection", "pure", "-ScratchRoot", (Join-Path $fixture "artifacts"), "-DotnetPath", (Get-FakeDotnetPath $fixture)) -Behavior @{ test = @{ trx = "pass"; exitCode = 0 } }
Check-Equal 2 $result.ExitCode "an explicit <Import> aborts the run"
Check-Match $result.Output 'explicit <Import>' "explicit import is named"

# ---------------------------------------------------------------------------
# 7. dotnet SDK preflight (runtime-only / SDK < 8) and -ListOnly
# ---------------------------------------------------------------------------
Write-Host "SDK preflight" -ForegroundColor Cyan

$fixture = New-FixtureRepo "sdk-too-old"
Add-XunitProject -Root $fixture -Name "Placed.Tests"
$result = Invoke-Runner -Root $fixture -RunnerArgs @("-Selection", "pure", "-ScratchRoot", (Join-Path $fixture "artifacts"), "-DotnetPath", (Get-FakeDotnetPath $fixture)) -Behavior @{ version = "6.0.100"; sdks = @("6.0.100 [C:\fake\sdk]"); test = @{ trx = "pass"; exitCode = 0 } }
Check-Equal 2 $result.ExitCode "an SDK older than 8 aborts before running"
Check-Match $result.Output 'SDK.*8 or newer' "old SDK failure names the requirement"
Check-Equal 0 @($result.Invocations | Where-Object { $_ -match '^test\s' }).Count "no project is executed after an SDK preflight failure"

$fixture = New-FixtureRepo 'active-sdk-too-old'
Add-XunitProject -Root $fixture -Name 'Placed.Tests'
$result = Invoke-Runner -Root $fixture -RunnerArgs @('-Selection', 'pure', '-ScratchRoot', (Join-Path $fixture 'artifacts'), '-DotnetPath', (Get-FakeDotnetPath $fixture)) -Behavior @{ version = '6.0.100'; sdks = @('6.0.100 [C:\fake\sdk]', '8.0.999 [C:\fake\sdk]'); test = @{ trx = 'pass'; exitCode = 0 } }
Check-Equal 2 $result.ExitCode 'active SDK 6 is rejected even with SDK 8 installed'
Check-Equal 0 @($result.Invocations | Where-Object { $_ -match '^test\s' }).Count 'SDK inventory cannot bypass active-version preflight'
Check (-not (Test-Path -LiteralPath (Join-Path $fixture 'artifacts'))) 'failed SDK preflight creates no test artifacts'

$fixture = New-FixtureRepo "runtime-only"
Add-XunitProject -Root $fixture -Name "Placed.Tests"
$result = Invoke-Runner -Root $fixture -RunnerArgs @("-Selection", "pure", "-ScratchRoot", (Join-Path $fixture "artifacts"), "-DotnetPath", (Get-FakeDotnetPath $fixture)) -Behavior @{ versionExit = 1; version = "dotnet: no SDK found"; sdks = @(); test = @{ trx = "pass"; exitCode = 0 } }
Check-Equal 2 $result.ExitCode "a runtime-only dotnet aborts before running"
Check-Match $result.Output '--version failed' "runtime-only failure is explicit"
Check-Equal 1 ([regex]::Matches($result.Output, 'ERROR:').Count) "the preflight fails exactly once, not once per project"

$fixture = New-FixtureRepo "list-only"
Add-XunitProject -Root $fixture -Name "Placed.Tests"
$result = Invoke-Runner -Root $fixture -RunnerArgs @("-Selection", "pure", "-ListOnly", "-DotnetPath", (Get-FakeDotnetPath $fixture))
Check-Equal 0 $result.ExitCode "-ListOnly exits 0"
Check-Equal 0 @($result.Invocations).Count "-ListOnly invokes dotnet not at all"
Check-Match $result.Output 'List-only' "list-only mode is announced"

# ---------------------------------------------------------------------------
# 8. Real xunit fixtures: one passing fact, and a project with no facts at all
# ---------------------------------------------------------------------------
Write-Host "real xunit fixtures" -ForegroundColor Cyan

$realDotnet = (Get-Command dotnet -ErrorAction SilentlyContinue)
if (-not $realDotnet) {
    Check $false "real dotnet must be available for the real xunit fixtures"
}
else {
    $fixture = New-FixtureRepo "real-pass"
    Add-XunitProject -Root $fixture -Name "RealPass.Tests" -SourceText @'
public class RealPassTests
{
    [Xunit.Fact]
    public void OnePlusOneIsTwo()
    {
        Xunit.Assert.Equal(2, 1 + 1);
    }
}
'@
    $result = Invoke-Runner -Root $fixture -RunnerArgs @("-Selection", "pure", "-ScratchRoot", (Join-Path $fixture "artifacts")) -RealDotnet -Environment @{ DOTNET_CLI_TELEMETRY_OPTOUT = "1"; DOTNET_NOLOGO = "1" }
    if ($result.ExitCode -ne 0 -or $result.Output -notmatch '1/1 passed') {
        Write-Host "Real passing fixture failed; retained files: $fixture"
        Write-Host $result.Output
    }
    Check-Equal 0 $result.ExitCode "a real xunit project with one passing fact passes"
    Check-Match $result.Output '1/1 passed' "the real run reports one executed test"

    $fixture = New-FixtureRepo "real-zero"
    Add-XunitProject -Root $fixture -Name "RealZero.Tests" -SourceText @'
public class RealZeroTests
{
    // Deliberately not a [Fact]: the project compiles but contains no test at all.
    public static int Helper() => 42;
}
'@
    $result = Invoke-Runner -Root $fixture -RunnerArgs @("-Selection", "pure", "-ScratchRoot", (Join-Path $fixture "artifacts")) -RealDotnet -Environment @{ DOTNET_CLI_TELEMETRY_OPTOUT = "1"; DOTNET_NOLOGO = "1" }
    if ($result.ExitCode -ne 1 -or $result.Output -notmatch 'executed=0') {
        Write-Host "Real zero-fact fixture failed unexpectedly; retained files: $fixture"
        Write-Host $result.Output
    }
    Check-Equal 1 $result.ExitCode "a real xunit project with no facts fails (the zero-test hole)"
    Check-Match $result.Output 'executed=0' "the real zero-fact run is rejected on its counters"
}

# ---------------------------------------------------------------------------
# Summary
# ---------------------------------------------------------------------------
Write-Host ""
$total = $script:Passed + $script:Failed
if ($script:Failed -eq 0) {
    Write-Host "test-all.Tests: $($script:Passed)/$total checks passed." -ForegroundColor Green
}
else {
    Write-Host "test-all.Tests: $($script:Failed) of $total checks FAILED:" -ForegroundColor Red
    $script:Failures | ForEach-Object { Write-Host "  - $_" -ForegroundColor Red }
}
Write-Host "Scratch: $ScratchRoot"
if (-not $KeepArtifacts -and $script:Failed -eq 0) {
    Remove-Item -LiteralPath $ScratchRoot -Recurse -Force -ErrorAction SilentlyContinue
}
elseif ($KeepArtifacts) {
    Write-Host "Artifacts kept (-KeepArtifacts)."
}

if ($script:Failed -gt 0) { exit 1 }
exit 0
