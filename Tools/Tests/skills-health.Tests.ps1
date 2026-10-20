#Requires -Version 7
<#
.SYNOPSIS
    Regression suite for Tools/check-skills-health.ps1 version-evidence annotations.

.DESCRIPTION
    Builds disposable Skills fixtures outside the repository and proves that:
      - a version literal annotated with '<!-- skills-health: version-evidence (reason) -->'
        on its own line or the immediately preceding line is accepted as evidence;
      - the parenthesized reason is mandatory and must be nonempty;
      - the annotation applies to the WHOLE annotated line by design (not one token);
      - unannotated pin guidance is still warned about;
      - the annotation does not leak past the next line, and a marker placed BELOW a
        literal does not waive the line above it;
      - a 'Version anchor' structural marker is never exempted;
      - the annotation never suppresses stale API-name checks, and a near-miss tag
        ('skills-health: evidence') is not accepted;
      - a Skills tree holding exactly ONE reference file is scanned end to end
        (array-singleton regression: '$refs + $skillFiles' used to throw op_Addition
        and skip the version/link/CRLF sections);
      - the checker emits no unexpected PowerShell errors in either tree.

.EXAMPLE
    pwsh Tools/Tests/skills-health.Tests.ps1
#>
[CmdletBinding()]
param(
    [string]$ScratchRoot
)

$ErrorActionPreference = 'Stop'

$repoRoot = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
$checker = Join-Path $repoRoot 'Tools/check-skills-health.ps1'
if (-not (Test-Path -LiteralPath $checker)) { throw "Script under test missing: $checker" }

if (-not $ScratchRoot) {
    $ScratchRoot = Join-Path ([System.IO.Path]::GetTempPath()) ('skills-health-tests-' + [guid]::NewGuid().ToString('N'))
}
$ScratchRoot = [System.IO.Path]::GetFullPath($ScratchRoot)
$repoFull = [System.IO.Path]::GetFullPath($repoRoot).TrimEnd('\', '/')
if ($ScratchRoot.StartsWith($repoFull + [System.IO.Path]::DirectorySeparatorChar, [System.StringComparison]::OrdinalIgnoreCase) -or
    $ScratchRoot.Equals($repoFull, [System.StringComparison]::OrdinalIgnoreCase)) {
    throw "ScratchRoot must not be inside the repository: $ScratchRoot"
}

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

function Write-FixtureFile {
    param([string]$Path, [string]$Content)
    $dir = Split-Path -Parent $Path
    if (-not (Test-Path -LiteralPath $dir)) { $null = New-Item -ItemType Directory -Path $dir -Force }
    [System.IO.File]::WriteAllText($Path, $Content, $utf8NoBom)
}

$fixtureRoot = Join-Path $ScratchRoot 'fixture'
$skillDir = Join-Path $fixtureRoot 'Skills/schedule1-fixture'
$refDir = Join-Path $skillDir 'references'

Write-FixtureFile (Join-Path $skillDir 'SKILL.md') @"
---
name: schedule1-fixture
description: Fixture skill for checker regression tests. Keywords: fixture, checker
---

Fixture body. Canonical config lives in AGENTS.md.
"@

Write-FixtureFile (Join-Path $refDir 'annotated.md') @"
# Annotated historical evidence
> verified: fixture, not in game

API floor .NET 6 <!-- skills-health: version-evidence (framework API availability, not a pin) -->
Observed assembly 0.3.0-rc.15 <!-- skills-health: version-evidence (observed third-party metadata) -->
- Metadata mismatch ``0.3.0-rc.15`` vs <!-- skills-health: version-evidence (observed conflicting metadata) -->
  ``0.3.0-rc.7``
"@

Write-FixtureFile (Join-Path $refDir 'stale.md') @"
# Stale active guidance
> verified: fixture, not in game

Pin S1API 3.2.1 for every build.
Target net6.0 in all project files.
Version 4.5.6 <!-- skills-health: evidence -->
### Version anchor <!-- skills-health: version-evidence (structural heading, not a version literal) -->

Version 7.7.7 <!-- skills-health: version-evidence () -->

S1API 6.6.6 is a pin here.
<!-- skills-health: version-evidence (marker below must not waive the line above) -->

GameLifecycle.OnSaveLoaded is available <!-- skills-health: version-evidence (wrong kind) -->
"@

Write-FixtureFile (Join-Path $refDir 'whole-line.md') @"
# Whole-line annotation scope
> verified: fixture, not in game

S1API 5.5.5 and .NET 6 share one line. <!-- skills-health: version-evidence (reason applies to the whole line by design) -->
"@

Write-FixtureFile (Join-Path $refDir 'scoped.md') @"
# Adjacent-line scope
> verified: fixture, not in game

<!-- skills-health: version-evidence (covers only the next line) -->
S1API 9.9.9 is documented evidence.
S1API 8.8.8 is an unannotated pin.
"@

# A Skills tree with exactly one reference file: '$refs' collapses to a scalar FileInfo.
$singleRoot = Join-Path $ScratchRoot 'fixture-single'
$singleSkill = Join-Path $singleRoot 'Skills/schedule1-single'
Write-FixtureFile (Join-Path $singleSkill 'SKILL.md') @"
---
name: schedule1-single
description: Single-reference fixture. Keywords: fixture, single
---

Single fixture body. Canonical config lives in AGENTS.md.
"@
Write-FixtureFile (Join-Path $singleSkill 'references/only.md') @"
# Only reference file
> verified: fixture, not in game

Pin S1API 3.2.1 for every build.
"@

$unexpectedErrorPattern = '(?i)op_Addition|Method invocation failed|InvalidOperation:|ParserError|RuntimeException|Unhandled exception|CommandNotFoundException|Property .* cannot be found'
function Test-NoCheckerError {
    param([string]$Text, [string]$Label)
    Check ($Text -notmatch $unexpectedErrorPattern) "$Label emits no unexpected PowerShell error"
}

$output = & pwsh -NoProfile -ExecutionPolicy Bypass -File $checker -Root $fixtureRoot *>&1 | Out-String
$exit = $LASTEXITCODE

Check ($exit -eq 0) "checker exits 0 on the fixture (got $exit)"
Test-NoCheckerError $output 'multi-reference fixture'

Check ($output -notmatch 'annotated\.md:\d+ stale string') 'annotated evidence produces no stale-string warning'
Check ($output -match "annotated\.md:5 version evidence '0\.3\.0'") 'first annotated literal is reported as accepted evidence'
Check ($output -match "annotated\.md:7 version evidence '0\.3\.0'") 'continuation-line annotated literal is accepted'

Check ($output -match "stale\.md:\d+ stale string '3\.2\.1'") 'unannotated S1API pin is still warned'
Check ($output -match "stale\.md:\d+ stale string 'net6\.0'") 'unannotated target framework is still warned'
Check ($output -match "stale\.md:\d+ stale string '4\.5\.6'") 'near-miss tag does not suppress a version warning'
Check ($output -match "stale\.md:\d+ stale string '7\.7\.7'") 'empty parenthesized reason does not exempt a version literal'
Check ($output -match "stale\.md:\d+ stale string 'Version anchor'") 'structural Version anchor is never exempted by the annotation'
Check ($output -match "stale\.md:\d+ stale string 'S1API 6\.6\.6'") 'marker below a line does not waive that earlier line'
Check ($output -match "stale\.md:\d+ stale string 'GameLifecycle\.OnSaveLoaded'") 'version-evidence annotation never suppresses API-name warnings'

Check ($output -notmatch "whole-line\.md:\d+ stale string") 'annotation covers the whole annotated line'
Check ($output -match "whole-line\.md:4 version evidence 'S1API 5\.5\.5'") 'whole-line scope accepts the first literal on the annotated line'
Check ($output -match "whole-line\.md:4 version evidence '\.NET 6'") 'whole-line scope accepts a second literal sharing the annotated line'

Check ($output -notmatch "scoped\.md:\d+ stale string '9\.9\.9'") 'annotation covers the immediately following line'
Check ($output -match "scoped\.md:\d+ stale string '8\.8\.8'") 'annotation does not leak beyond the next line'

# --- single-reference tree (array-singleton regression) -------------------
$output2 = & pwsh -NoProfile -ExecutionPolicy Bypass -File $checker -Root $singleRoot *>&1 | Out-String
$exit2 = $LASTEXITCODE

Check ($exit2 -eq 0) "checker exits 0 on the single-reference fixture (got $exit2)"
Test-NoCheckerError $output2 'single-reference fixture'
Check ($output2 -match "only\.md:\d+ stale string 'S1API 3\.2\.1'") 'single-reference tree still reaches the version scan (active S1API pin warned)'
Check ($output2 -match "only\.md: header ok") 'single-reference tree still runs reference-header checks'

# ---------------------------------------------------------------------------

Write-Host ''
Write-Host ('Tests: {0} passed, {1} failed' -f $script:Passed, $script:Failed) -ForegroundColor $(if ($script:Failed -eq 0) { 'Green' } else { 'Red' })
if ($script:Failed -gt 0) {
    Write-Host 'Failures:' -ForegroundColor Red
    foreach ($f in $script:Failures) { Write-Host "  - $f" -ForegroundColor Red }
    Write-Host "Checker output (multi-reference fixture):" -ForegroundColor Yellow
    Write-Host $output
    Write-Host "Checker output (single-reference fixture):" -ForegroundColor Yellow
    Write-Host $output2
    Write-Host "Artifacts kept at: $ScratchRoot" -ForegroundColor Yellow
    exit 1
}

Remove-Item -LiteralPath $ScratchRoot -Recurse -Force -ErrorAction SilentlyContinue
exit 0
