# Runs every test project under Source/Tests with an explicit classification.
#
# Usage:
#   pwsh Tools/test-all.ps1                     # game-free suites only (CI default)
#   pwsh Tools/test-all.ps1 -Selection all      # + game-required suites (needs game assemblies)
#   pwsh Tools/test-all.ps1 -ListOnly           # classify and print, run nothing
#
# Classification is derived from the csproj itself and is deliberately strict:
#   xunit        - Microsoft.NET.Test.Sdk + xunit, no game/IL2CPP references  -> dotnet test
#   console      - OutputType Exe, no test SDK, and a *declared* output contract
#                  (currently only LightControl.Tests)                        -> dotnet run
#   game-required- test project that references $(GameDir) or a Mods project  -> needs the game
# Anything that does not fit exactly one of those shapes aborts the run (exit 2)
# instead of being silently skipped. Game-required projects are skipped in
# -Selection pure and reported as skipped; -Selection all fails if the game
# assemblies are absent rather than reporting a false green. A selection in which
# nothing actually executed (every project skipped) is a failure, never a green.
#
# xunit results are read from a per-project TRX file with a unique name, written
# below a runner-owned scratch directory (never the OS temp directory):
#   - TMPDIR is honoured only when it points somewhere other than the OS temp dir;
#   - otherwise the explicit Hermes scratch dir (%LOCALAPPDATA%\hermes\cache\scratch)
#     is used.
# A TRX that is missing, unparseable, or carries absent/invalid counters is a
# failure, never a pass; executed must be > 0 (xunit itself exits 0 for a project
# with no facts at all) and failed must be 0.
#
# The LightControl console host is the only console contract known to this runner:
# its stdout is captured with Tee-Object and must contain
#   RESULT: <passed>/<total> passed; <failed> failed
# with a nonzero numerator, a nonzero denominator and zero failures. No absolute
# case-count minimum is asserted here - the real count belongs to the fixture, and
# a fixed minimum only rots. Any other console host fails classification until its
# output contract is declared in $script:ConsoleContracts.
#
# Builds never deploy: S1NoDeploy=true is passed to every dotnet test/run as an
# explicit MSBuild property (-p:S1NoDeploy=true). The environment variable alone is
# not trusted, because a fresh MSBuild environment may not see it.
#
# Preflight: dotnet --version must select SDK 8+ (a runtime-only host fails here)
# and dotnet --list-sdks must confirm an SDK installation. Either check fails once,
# before any project work, instead of producing one confusing error per project.
#
# Scope is Source/Tests, and every project found there must classify. A
# *Tests.csproj left behind under Source/Mods (bin/obj, Archive and ThirdParty
# excluded) aborts the run: such a project would never be executed by CI.

[CmdletBinding()]
param(
    [ValidateSet("pure", "all")]
    [string]$Selection = "pure",

    [ValidateSet("Debug", "Release")]
    [string]$Configuration = "Release",

    [string]$DotnetPath,

    # Runner-owned scratch root for TRX files and captured console output.
    # Resolved automatically when omitted; see Resolve-RunnerScratch.
    [string]$ScratchRoot,

    [switch]$ListOnly
)

$ErrorActionPreference = "Stop"
. (Join-Path $PSScriptRoot 'dotnet-sdk.ps1')

$repoRoot = Split-Path -Parent $PSScriptRoot
$testsRoot = Join-Path $repoRoot "Source\Tests"
$modsRoot = Join-Path $repoRoot "Source\Mods"

# Declared console output contracts. Key = test project name (csproj base name).
# Value = regex with named groups (passed, total, failed). A console project whose
# name is absent here is a classification error, never a silent run.
$script:ConsoleContracts = @{
    "LightControl.Tests" = 'RESULT:\s*(?<passed>\d+)/(?<total>\d+)\s+passed;\s*(?<failed>\d+)\s+failed'
}

function Test-IsSystemTemp {
    param([string]$Path)

    try {
        $candidate = [System.IO.Path]::GetFullPath($Path).TrimEnd([char[]]@('\', '/'))
    }
    catch {
        return $true
    }
    if (-not $candidate) { return $true }

    # Bare POSIX-style temp mounts are the OS temp dir even when the drive-qualified
    # path looks different.
    if ($candidate -match '^[A-Za-z]:[\\/](tmp|var[\\/]tmp)$' -or $candidate -match '^[\\/](tmp|var[\\/]tmp)$') {
        return $true
    }

    $systemTemp = [System.IO.Path]::GetFullPath([System.IO.Path]::GetTempPath()).TrimEnd([char[]]@('\', '/'))
    if ($candidate.Equals($systemTemp, [System.StringComparison]::OrdinalIgnoreCase)) { return $true }
    if ($candidate.StartsWith($systemTemp + [System.IO.Path]::DirectorySeparatorChar, [System.StringComparison]::OrdinalIgnoreCase)) {
        return $true
    }
    return $false
}

function Resolve-RunnerScratch {
    # TMPDIR is honoured only when it is a genuine custom directory: it commonly
    # points at the OS temp dir, which is exactly where artifacts must not go.
    $candidates = @()
    if ($env:TEST_ALL_SCRATCH) { $candidates += $env:TEST_ALL_SCRATCH }
    if ($env:TMPDIR) { $candidates += $env:TMPDIR }
    if ($env:TEMP) { $candidates += $env:TEMP }
    if ($env:LOCALAPPDATA) { $candidates += (Join-Path $env:LOCALAPPDATA "hermes\cache\scratch") }
    if ($env:USERPROFILE) { $candidates += (Join-Path $env:USERPROFILE ".cache\hermes\scratch") }

    foreach ($candidate in $candidates) {
        try {
            $full = [System.IO.Path]::GetFullPath($candidate)
        }
        catch {
            continue
        }
        if (Test-IsSystemTemp -Path $full) { continue }
        return $full
    }

    throw "No usable scratch directory: every candidate (TEST_ALL_SCRATCH, TMPDIR, TEMP, LOCALAPPDATA\hermes\cache\scratch) is the OS temp directory or unavailable."
}

function Get-ProjectKind {
    param([string]$Path)

    $xml = New-Object System.Xml.XmlDocument
    $xml.Load($Path)

    # Refuse anything that pulls in build logic this runner does not model.
    if (@($xml.SelectNodes("/Project/Import")).Count -gt 0) {
        throw "Project contains an explicit <Import>; custom MSBuild imports are not supported by this runner."
    }

    $outputType = @(
        $xml.SelectNodes("/Project/PropertyGroup") |
            ForEach-Object { $_.OutputType } |
            Where-Object { $_ }
    ) | Select-Object -First 1

    $packageRefs = @($xml.SelectNodes("/Project/ItemGroup/PackageReference"))
    $hasTestSdk = @($packageRefs | Where-Object { $_.GetAttribute("Include") -eq "Microsoft.NET.Test.Sdk" }).Count -gt 0
    $hasXunit = @($packageRefs | Where-Object { $_.GetAttribute("Include") -match '^xunit$' }).Count -gt 0

    $references = @($xml.SelectNodes("/Project/ItemGroup/Reference"))
    $projectRefs = @($xml.SelectNodes("/Project/ItemGroup/ProjectReference"))

    $hasGameReference = @(
        $references | Where-Object {
            # HintPath is a child element (<HintPath>...</HintPath>), not an attribute.
            $hint = $_.GetAttribute("HintPath")
            if (-not $hint) {
                $hintNode = $_.SelectSingleNode("HintPath")
                if ($hintNode) { $hint = $hintNode.InnerText }
            }
            $hint -match '\$\(GameDir\)|Il2CppAssemblies|MelonLoader'
        }
    ).Count -gt 0

    $hasModProjectReference = @(
        $projectRefs | Where-Object {
            $_.GetAttribute("Include") -match '(^|[\\/])Mods[\\/]'
        }
    ).Count -gt 0

    if ($outputType -eq "Exe") {
        if ($hasTestSdk) {
            throw "OutputType Exe and Microsoft.NET.Test.Sdk are both present; the runner cannot tell whether to use 'dotnet run' or 'dotnet test'."
        }
        if ($hasGameReference -or $hasModProjectReference) {
            throw "Console project references game/IL2CPP or a Mods project; game-required console execution is not supported by this runner."
        }
        $name = [System.IO.Path]::GetFileNameWithoutExtension($Path)
        if (-not $script:ConsoleContracts.ContainsKey($name)) {
            throw "Console test host '$name' has no declared output contract. Declare one in `$script:ConsoleContracts: an undeclared host cannot be judged by a hard-coded pass rule."
        }
        return [pscustomobject]@{ Kind = "console"; Detail = "OutputType Exe, declared output contract" }
    }

    if (-not ($hasTestSdk -and $hasXunit)) {
        throw "Not recognisable as an xunit test project (needs Microsoft.NET.Test.Sdk + xunit) or a console test host (OutputType Exe)."
    }

    if ($hasGameReference -or $hasModProjectReference) {
        return [pscustomobject]@{ Kind = "game-required"; Detail = "test SDK + xunit + game/Mods reference" }
    }

    return [pscustomobject]@{ Kind = "xunit"; Detail = "test SDK + xunit, game-free" }
}

function Read-TrxCounters {
    param([string]$Path)

    $xml = New-Object System.Xml.XmlDocument
    try {
        $xml.Load($Path)
    }
    catch {
        throw "TRX is not valid XML: $($_.Exception.Message)"
    }

    # VSTest writes a default xmlns, so match on local-name instead of a prefixed path.
    if (-not $xml.DocumentElement -or $xml.DocumentElement.LocalName -ne "TestRun") {
        throw "TRX root element is not 'TestRun'."
    }
    $node = $xml.SelectSingleNode("/*[local-name()='TestRun']/*[local-name()='ResultSummary']/*[local-name()='Counters']")
    if (-not $node) { throw "TRX has no /TestRun/ResultSummary/Counters element." }

    $counters = @{}
    foreach ($name in @("total", "executed", "passed", "failed")) {
        $raw = $node.GetAttribute($name)
        if (-not $raw) { throw "TRX Counters is missing the '$name' attribute." }
        $value = 0
        if (-not [int]::TryParse($raw, [ref]$value) -or $value -lt 0) {
            throw "TRX Counters '$name' is not a non-negative integer ('$raw')."
        }
        $counters[$name] = $value
    }

    if ($counters.total -lt $counters.executed) {
        throw "TRX Counters are inconsistent: total=$($counters.total) < executed=$($counters.executed)."
    }

    return [pscustomobject]$counters
}

if (-not (Test-Path -LiteralPath $testsRoot)) {
    Write-Host "ERROR: '$testsRoot' does not exist." -ForegroundColor Red
    exit 2
}

# Misplaced test projects outside Source/Tests are never executed by this runner
# (or by CI), so their presence is a hard error rather than dead weight.
if (Test-Path -LiteralPath $modsRoot) {
    $misplaced = @(
        Get-ChildItem -LiteralPath $modsRoot -Recurse -File -Filter "*Tests.csproj" |
            Where-Object {
                $_.FullName -notmatch '[\\/](bin|obj)[\\/]' -and
                $_.FullName -notmatch '[\\/](Archive|ThirdParty)[\\/]'
            }
    )
    if ($misplaced.Count -gt 0) {
        Write-Host "ERROR: test project(s) found outside Source/Tests - they would never run under CI:" -ForegroundColor Red
        $misplaced | ForEach-Object { Write-Host "  - $($_.FullName)" -ForegroundColor Red }
        exit 2
    }
}

$projects = @(
    Get-ChildItem -LiteralPath $testsRoot -Recurse -Filter "*.csproj" -File |
        Where-Object { $_.FullName -notmatch '[\\/](bin|obj)[\\/]' } |
        Sort-Object FullName
)

if ($projects.Count -eq 0) {
    Write-Host "ERROR: no test projects found under '$testsRoot'." -ForegroundColor Red
    exit 2
}

# Custom Directory.Build.props/targets under Source/Tests would change project
# behaviour in ways this runner does not evaluate. Reject them explicitly instead
# of growing a general MSBuild evaluator.
$buildOverrides = @(
    Get-ChildItem -LiteralPath $testsRoot -Recurse -File |
        Where-Object {
            $_.Name -in @("Directory.Build.props", "Directory.Build.targets", "Directory.Packages.props") -and
            $_.FullName -notmatch '[\\/](bin|obj)[\\/]'
        }
)
if ($buildOverrides.Count -gt 0) {
    Write-Host "ERROR: custom MSBuild files under Source/Tests are not supported by this runner:" -ForegroundColor Red
    $buildOverrides | ForEach-Object { Write-Host "  - $($_.FullName)" -ForegroundColor Red }
    exit 2
}

$classified = @()
$unclassified = @()
foreach ($project in $projects) {
    try {
        $kind = Get-ProjectKind -Path $project.FullName
        $classified += [pscustomobject]@{
            Name    = $project.BaseName
            Path    = $project.FullName
            RelPath = $project.FullName.Substring($repoRoot.Length).TrimStart('\', '/')
            Kind    = $kind.Kind
            Detail  = $kind.Detail
        }
    }
    catch {
        $unclassified += "$($project.FullName): $($_.Exception.Message)"
    }
}

if ($unclassified.Count -gt 0) {
    Write-Host "ERROR: unclassified test project(s) - refusing to guess or silently skip:" -ForegroundColor Red
    $unclassified | ForEach-Object { Write-Host "  - $_" -ForegroundColor Red }
    exit 2
}

$duplicates = @($classified | Group-Object Name | Where-Object Count -gt 1)
if ($duplicates.Count -gt 0) {
    Write-Host "ERROR: duplicate test project names:" -ForegroundColor Red
    $duplicates | ForEach-Object { Write-Host "  - $($_.Name)" -ForegroundColor Red }
    exit 2
}

$gameDir = if ($env:SCHEDULE1_PATH) { $env:SCHEDULE1_PATH } else { "C:\Program Files (x86)\Steam\steamapps\common\Schedule I" }
$gameProbe = Join-Path $gameDir "MelonLoader\Il2CppAssemblies\Assembly-CSharp.dll"
$hasGame = Test-Path -LiteralPath $gameProbe

if ($Selection -eq "all" -and -not $hasGame) {
    Write-Host "ERROR: -Selection all needs Schedule I's IL2CPP assemblies, which were not found:" -ForegroundColor Red
    Write-Host "  probe: $gameProbe" -ForegroundColor Red
    Write-Host "  Set SCHEDULE1_PATH or use -Selection pure on a game-free machine." -ForegroundColor Red
    exit 2
}

$dotnet = $null
$sdkVersion = "(not resolved - list only)"
$runScratch = $null
if (-not $ListOnly) {
    try {
        $dotnet = Resolve-S1DotnetPath -Requested $DotnetPath
        $sdkVersion = Assert-S1DotnetSdk -DotnetPath $dotnet
        $resolvedScratch = if ($ScratchRoot) { [System.IO.Path]::GetFullPath($ScratchRoot) } else { Resolve-RunnerScratch }
        if (Test-IsSystemTemp -Path $resolvedScratch) {
            throw "Refusing to write test artifacts into the OS temp directory: '$resolvedScratch'."
        }
        $runScratch = Join-Path $resolvedScratch ("test-all-" + (Get-Date -Format "yyyyMMdd-HHmmss") + "-" + [guid]::NewGuid().ToString('N').Substring(0, 8))
        $null = New-Item -ItemType Directory -Path $runScratch -Force
    }
    catch {
        Write-Host "ERROR: $($_.Exception.Message)" -ForegroundColor Red
        exit 2
    }
}

Write-Host ""
Write-Host "test-all.ps1 | selection=$Selection | configuration=$Configuration" -ForegroundColor Cyan
if ($ListOnly) {
    Write-Host "dotnet      : (not resolved - list only)"
    Write-Host "artifacts   : (none - list only)"
}
else {
    Write-Host "dotnet      : $dotnet"
    Write-Host "SDK version : $sdkVersion"
    Write-Host "artifacts   : $runScratch"
}
Write-Host "game probe  : $gameProbe ($(if ($hasGame) { 'present' } else { 'absent' }))"
Write-Host "S1NoDeploy  : -p:S1NoDeploy=true on every dotnet test/run (env alone is not trusted)"
Write-Host ""
Write-Host "Classification:"
foreach ($item in $classified) {
    $mark = if ($item.Kind -eq "game-required" -and $Selection -eq "pure") { "SKIP" } else { "RUN " }
    Write-Host ("  [{0}] {1,-32} {2,-14} {3}" -f $mark, $item.Name, $item.Kind, $item.Detail)
}
Write-Host ""

if ($ListOnly) {
    Write-Host "List-only: no tests executed." -ForegroundColor Yellow
    exit 0
}

$results = @()
$stopwatch = [System.Diagnostics.Stopwatch]::StartNew()

foreach ($project in $classified) {
    if ($Selection -eq "pure" -and $project.Kind -eq "game-required") {
        $results += [pscustomobject]@{
            Project  = $project.Name
            Kind     = $project.Kind
            Status   = "Skipped"
            Counters = "-"
            Reason   = "game-required, -Selection pure"
            Seconds  = 0.0
            ExitCode = $null
        }
        continue
    }

    Write-Host ("=== {0} ({1}) ===" -f $project.Name, $project.Kind) -ForegroundColor Cyan
    $projectWatch = [System.Diagnostics.Stopwatch]::StartNew()
    $projectDir = Join-Path $runScratch $project.Name
    $null = New-Item -ItemType Directory -Path $projectDir -Force
    $logPath = Join-Path $projectDir "$($project.Name).log"
    $exitCode = 1
    $countersText = "-"
    $reason = ""

    try {
        if ($project.Kind -eq "xunit" -or $project.Kind -eq "game-required") {
            # Unique TRX per project per run: a stale or shared file can never be
            # mistaken for this run's result.
            $trxName = "{0}-{1}.trx" -f $project.Name, [guid]::NewGuid().ToString('N').Substring(0, 12)
            $trxPath = Join-Path $projectDir $trxName
            $testArgs = @(
                "test", $project.Path,
                "-c", $Configuration,
                "--nologo",
                "-p:S1NoDeploy=true",
                "--logger", "trx;LogFileName=$trxName",
                "--results-directory", $projectDir
            )
            & $dotnet @testArgs 2>&1 | Tee-Object -FilePath $logPath
            $exitCode = $LASTEXITCODE

            if (-not (Test-Path -LiteralPath $trxPath)) {
                $reason = "xunit run produced no TRX at '$trxPath' (exit $exitCode); the run cannot be verified."
            }
            else {
                try {
                    $counters = Read-TrxCounters -Path $trxPath
                    $countersText = "{0}/{1} passed, {2} failed, {3} executed" -f `
                        $counters.passed, $counters.total, $counters.failed, $counters.executed

                    if ($counters.executed -le 0) {
                        $reason = "TRX reports executed=0: no test actually ran in this project."
                    }
                    elseif ($counters.passed -le 0) {
                        $reason = "TRX reports passed=0 with executed=$($counters.executed): nothing passed."
                    }
                    elseif ($counters.failed -ne 0) {
                        $reason = "TRX reports $($counters.failed) failed test(s)."
                    }
                    elseif ($exitCode -ne 0) {
                        $reason = "dotnet test exited $exitCode although the TRX counters look clean."
                    }
                }
                catch {
                    $reason = "Invalid TRX: $($_.Exception.Message)"
                }
            }
        }
        elseif ($project.Kind -eq "console") {
            $pattern = $script:ConsoleContracts[$project.Name]
            $runArgs = @(
                "run", "--project", $project.Path,
                "-c", $Configuration,
                "--no-launch-profile",
                "-p:S1NoDeploy=true"
            )
            & $dotnet @runArgs 2>&1 | Tee-Object -FilePath $logPath
            $exitCode = $LASTEXITCODE

            $output = if (Test-Path -LiteralPath $logPath) { [System.IO.File]::ReadAllText($logPath) } else { "" }
            $match = [regex]::Match($output, $pattern)
            if (-not $match.Success) {
                $reason = "Console output has no RESULT line matching the declared contract ('$pattern')."
            }
            else {
                $passedCount = [int]$match.Groups["passed"].Value
                $totalCount = [int]$match.Groups["total"].Value
                $failedCount = [int]$match.Groups["failed"].Value
                $countersText = "$passedCount/$totalCount passed, $failedCount failed"

                if ($totalCount -le 0) {
                    $reason = "RESULT denominator is zero: the console host exercised no cases."
                }
                elseif ($passedCount -le 0) {
                    $reason = "RESULT numerator is zero: no console case passed."
                }
                elseif ($passedCount -gt $totalCount) {
                    $reason = "RESULT is inconsistent: $passedCount passed of $totalCount."
                }
                elseif ($failedCount -ne 0) {
                    $reason = "RESULT reports $failedCount failed case(s)."
                }
                elseif ($exitCode -ne 0) {
                    $reason = "dotnet run exited $exitCode although RESULT reports no failures."
                }
            }
        }
        else {
            throw "Unhandled project kind '$($project.Kind)'."
        }
    }
    catch {
        Write-Host "ERROR while running $($project.Name): $($_.Exception.Message)" -ForegroundColor Red
        $exitCode = 1
        $reason = $_.Exception.Message
    }
    $projectWatch.Stop()

    $status = if ($exitCode -eq 0 -and -not $reason) { "Passed" } else { "Failed" }
    if ($status -eq "Passed") {
        Write-Host ("--- {0}: PASSED ({1}) ({2:n1}s) ---" -f $project.Name, $countersText, $projectWatch.Elapsed.TotalSeconds) -ForegroundColor Green
    }
    else {
        Write-Host ("--- {0}: FAILED (exit {1}) - {2} ---" -f $project.Name, $exitCode, $reason) -ForegroundColor Red
    }

    $results += [pscustomobject]@{
        Project  = $project.Name
        Kind     = $project.Kind
        Status   = $status
        Counters = $countersText
        Reason   = $reason
        Seconds  = [math]::Round($projectWatch.Elapsed.TotalSeconds, 1)
        ExitCode = $exitCode
    }
}

$stopwatch.Stop()

$passed = @($results | Where-Object Status -eq "Passed").Count
$failed = @($results | Where-Object Status -eq "Failed").Count
$skipped = @($results | Where-Object Status -eq "Skipped").Count
$executedProjects = $passed + $failed

Write-Host ""
Write-Host "===== RESULTS =====" -ForegroundColor Cyan
foreach ($result in $results) {
    $seconds = if ($result.Status -eq "Skipped") { "-" } else { "{0:n1}s" -f $result.Seconds }
    Write-Host ("  {0,-7} {1,-32} {2,-14} {3,8}  {4}" -f $result.Status.ToUpperInvariant(), $result.Project, $result.Kind, $seconds, $result.Counters)
    if ($result.Status -eq "Failed" -and $result.Reason) {
        Write-Host ("          -> {0}" -f $result.Reason) -ForegroundColor Red
    }
}
Write-Host ""
Write-Host ("Projects: {0} | Executed: {1} | Passed: {2} | Failed: {3} | Skipped: {4} | Elapsed: {5:n1}s" -f `
    $results.Count, $executedProjects, $passed, $failed, $skipped, $stopwatch.Elapsed.TotalSeconds) -ForegroundColor Cyan
Write-Host "Artifacts: $runScratch"

if ($skipped -gt 0) {
    Write-Host "Skipped projects are game-required. Use -Selection all on a machine with the game installed." -ForegroundColor Yellow
}

if ($failed -gt 0) {
    Write-Host "FAILED project(s): $((@($results | Where-Object Status -eq 'Failed' | ForEach-Object Project)) -join ', ')" -ForegroundColor Red
    exit 1
}

if ($executedProjects -eq 0) {
    Write-Host "ERROR: every project in this selection was skipped ($skipped); nothing was verified." -ForegroundColor Red
    exit 1
}

Write-Host "All selected test projects passed." -ForegroundColor Green
exit 0
