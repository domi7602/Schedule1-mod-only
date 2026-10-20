#Requires -Version 7
<#
.SYNOPSIS
    Regression suite for Tools/bump-version.ps1 and check-version-sync matrix rows.

.DESCRIPTION
    Runs without Pester. Creates disposable fixtures under a configurable scratch
    directory (never inside the repository) and copies the scripts under test into
    each fixture, so the repository's real mods and docs are never modified.

.EXAMPLE
    pwsh Tools/Tests/bump-version.Tests.ps1
    pwsh Tools/Tests/bump-version.Tests.ps1 -ScratchRoot D:\temp\bump-tests -KeepArtifacts
#>
[CmdletBinding()]
param(
    [string]$ScratchRoot,
    [switch]$KeepArtifacts
)

$ErrorActionPreference = "Stop"

$repoRoot = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
$script:BumpScript = Join-Path $repoRoot "Tools/bump-version.ps1"
$script:SyncScript = Join-Path $repoRoot "Tools/check-version-sync.ps1"
foreach ($p in @($script:BumpScript, $script:SyncScript)) {
    if (-not (Test-Path -LiteralPath $p)) { throw "Script under test fehlt: $p" }
}

if (-not $ScratchRoot) {
    if ($env:BUMP_VERSION_TEST_SCRATCH) { $ScratchRoot = $env:BUMP_VERSION_TEST_SCRATCH }
    else { $ScratchRoot = Join-Path ([System.IO.Path]::GetTempPath()) ("bump-version-tests-" + [guid]::NewGuid().ToString('N')) }
}
$ScratchRoot = [System.IO.Path]::GetFullPath($ScratchRoot)
$repoFull = [System.IO.Path]::GetFullPath($repoRoot).TrimEnd('\', '/')
if ($ScratchRoot.StartsWith($repoFull + [System.IO.Path]::DirectorySeparatorChar, [System.StringComparison]::OrdinalIgnoreCase) -or
    $ScratchRoot.Equals($repoFull, [System.StringComparison]::OrdinalIgnoreCase)) {
    throw "ScratchRoot darf nicht im Repository liegen: $ScratchRoot"
}
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

function Read-Text {
    param([string]$Path)
    return [System.IO.File]::ReadAllText($Path, [System.Text.Encoding]::UTF8)
}

function Write-FixtureFile {
    param([string]$Root, [string]$Relative, [string]$Content)
    $full = Join-Path $Root $Relative
    $parent = Split-Path $full -Parent
    if (-not (Test-Path -LiteralPath $parent)) { $null = New-Item -ItemType Directory -Path $parent -Force }
    [System.IO.File]::WriteAllText($full, $Content, $utf8NoBom)
}

function New-Fixture {
    param(
        [Parameter(Mandatory)][string]$Name,
        [string]$ModName = 'OldMod',
        [string]$Version = '1.0.0',
        [string]$CompatDate = '2026-10-10',
        [string]$AgentsStatus = '',
        [string]$ReadmeStatus = 'Active (owner-confirmed 2026-10-10)',
        [string]$ReadmeDesc = 'Test mod; owner-confirmed 2026-10-10.',
        [switch]$OmitCompatRow,
        [switch]$DuplicateCompatRow,
        [switch]$DuplicateAgentsRow
    )

    $root = Join-Path $ScratchRoot $Name
    if (Test-Path -LiteralPath $root) { Remove-Item -LiteralPath $root -Recurse -Force }
    $null = New-Item -ItemType Directory -Path $root -Force
    $null = New-Item -ItemType Directory -Path (Join-Path $root 'Tools') -Force

    Copy-Item -LiteralPath $script:BumpScript -Destination (Join-Path $root 'Tools/bump-version.ps1')
    Copy-Item -LiteralPath $script:SyncScript -Destination (Join-Path $root 'Tools/check-version-sync.ps1')

    if (-not $AgentsStatus) {
        $AgentsStatus = "✅ active (v$Version, **Verified 2026-10-10**)"
    }

    $modSource = @(
        'using MelonLoader;'
        "[assembly: MelonInfo(typeof($ModName.Mod), `"$ModName`", `"$Version`", `"Tests`")]"
        "namespace $ModName { public class Mod { } }"
    ) -join "`n"
    Write-FixtureFile $root "Source/Mods/$ModName/src/Mod.cs" $modSource

    $modJson = @(
        '{'
        "  `"name`": `"$ModName`","
        "  `"version`": `"$Version`","
        '  "description": "fixture"'
        '}'
    ) -join "`n"
    Write-FixtureFile $root "Source/Mods/$ModName/docs/mod.json" $modJson
    Write-FixtureFile $root "Source/Mods/$ModName/docs/README.md" "# $ModName`n"
    Write-FixtureFile $root "Source/Mods/$ModName/docs/CHANGELOG.md" (
        "# Changelog - $ModName`n`n## $Version (2026-10-01)`n- Initial.`n")

    $agents = @(
        '# AGENTS'
        ''
        '| Mod | Status | Path | Dll | Notes |'
        '|---|---|---|---|---|'
        "| **$ModName** | $AgentsStatus | Mods/$ModName/ | $ModName.dll | test |"
    )
    if ($DuplicateAgentsRow) {
        $agents += "| **$ModName** | ✅ active (v$Version, **Verified 2026-10-09**) | Mods/$ModName/ | $ModName.dll | duplicate |"
    }
    $agents += @('', '## Mod Details', '', "**$ModName v$Version (2026-10-10):** test section.", '')
    Write-FixtureFile $root 'AGENTS.md' ($agents -join "`n")

    $readme = @(
        '# README'
        ''
        '| Mod | Category | Version | Status | Description |'
        '|---|---|---|---|---|'
        "| [**$ModName**](Source/Mods/$ModName/) | Phone | $Version | $ReadmeStatus | $ReadmeDesc |"
        ''
        '<table><tr><td>'
        "      <b>$ModName</b> · v$Version<br>"
        '</td></tr></table>'
        ''
    )
    Write-FixtureFile $root 'README.md' ($readme -join "`n")

    $compat = @(
        '# Compatibility'
        ''
        '## Verification matrix'
        ''
        '| Mod | Version | Verified on | Date | Needs S1API | Needs S1MAPI | Notes |'
        '|---|---|---|---|---|---|---|'
    )
    if (-not $OmitCompatRow) {
        $row = "| $ModName | $Version | v0.4.7f12 | $CompatDate | yes | no | Owner confirmed 2026-10-10. |"
        $compat += $row
        if ($DuplicateCompatRow) { $compat += $row }
    }
    $compat += ''
    Write-FixtureFile $root 'docs/compatibility.md' ($compat -join "`n")

    return $root
}

function Add-SecondMod {
    param([string]$Root, [string]$ModName = 'OtherMod', [string]$Version = '1.0.0')

    $modSource = @(
        'using MelonLoader;'
        "[assembly: MelonInfo(typeof($ModName.Mod), `"$ModName`", `"$Version`", `"Tests`")]"
        "namespace $ModName { public class Mod { } }"
    ) -join "`n"
    Write-FixtureFile $Root "Source/Mods/$ModName/src/Mod.cs" $modSource
    Write-FixtureFile $Root "Source/Mods/$ModName/docs/mod.json" (
        "{`n  `"name`": `"$ModName`",`n  `"version`": `"$Version`"`n}`n")
    Write-FixtureFile $Root "Source/Mods/$ModName/docs/README.md" "# $ModName`n"
    Write-FixtureFile $Root "Source/Mods/$ModName/docs/CHANGELOG.md" (
        "# Changelog - $ModName`n`n## $Version (2026-10-01)`n- Initial.`n")

    $agentsPath = Join-Path $Root 'AGENTS.md'
    $agents = Read-Text $agentsPath
    $firstRow = "| **OldMod** | ✅ active (v1.0.0, **Verified 2026-10-10**) | Mods/OldMod/ | OldMod.dll | test |"
    $secondRow = "| **$ModName** | ✅ active (v$Version, **Verified 2026-10-09**) | Mods/$ModName/ | $ModName.dll | second |"
    $agents = $agents.Replace($firstRow, $firstRow + "`n" + $secondRow)
    [System.IO.File]::WriteAllText($agentsPath, $agents, $utf8NoBom)

    $readmePath = Join-Path $Root 'README.md'
    $readme = Read-Text $readmePath
    $firstReadmeRow = "| [**OldMod**](Source/Mods/OldMod/) | Phone | 1.0.0 | Active (owner-confirmed 2026-10-10) | Test mod; owner-confirmed 2026-10-10. |"
    $secondReadmeRow = "| [**$ModName**](Source/Mods/$ModName/) | Phone | $Version | Active | Second test mod. |"
    $readme = $readme.Replace($firstReadmeRow, $firstReadmeRow + "`n" + $secondReadmeRow)
    $readme = $readme.Replace("      <b>OldMod</b> · v1.0.0<br>",
                              "      <b>OldMod</b> · v1.0.0<br>`n      <b>$ModName</b> · v$Version<br>")
    [System.IO.File]::WriteAllText($readmePath, $readme, $utf8NoBom)

    $compatPath = Join-Path $Root 'docs/compatibility.md'
    $compat = Read-Text $compatPath
    $firstCompatRow = '| OldMod | 1.0.0 | v0.4.7f12 | 2026-10-10 | yes | no | Owner confirmed 2026-10-10. |'
    $secondCompatRow = "| $ModName | $Version | v0.4.7f12 | 2026-10-09 | yes | no | Second. |"
    $compat = $compat.Replace($firstCompatRow, $firstCompatRow + "`n" + $secondCompatRow)
    [System.IO.File]::WriteAllText($compatPath, $compat, $utf8NoBom)
}

function Invoke-UnderTest {
    param([string]$Script, [string[]]$Arguments = @())
    $output = & pwsh -NoProfile -File $Script @Arguments 2>&1 | Out-String
    $code = $LASTEXITCODE
    return [pscustomobject]@{ Exit = $code; Output = $output }
}

function Get-FixtureHash {
    param([string]$Root)
    $files = Get-ChildItem -LiteralPath $Root -Recurse -File |
             Where-Object { $_.FullName -notmatch '[\\/]Tools[\\/]' } |
             Sort-Object FullName
    return (($files | ForEach-Object {
        $_.FullName.Substring($Root.Length) + ':' + (Get-FileHash -LiteralPath $_.FullName -Algorithm SHA256).Hash
    }) -join "`n")
}

function Run-Test {
    param([string]$Name, [scriptblock]$Body)
    Write-Host "`n[TEST] $Name" -ForegroundColor Cyan
    try { & $Body }
    catch {
        $script:Failed++
        $script:Failures += "$Name : unhandled error: $_"
        Write-Host "  FAIL: unhandled error: $_" -ForegroundColor Red
    }
}

# ---------------------------------------------------------------------------

Run-Test 'version transition converts prior acceptance to open and keeps evidence historical' {
    $root = New-Fixture -Name 'transition-old-acceptance'
    $logging = @(
        'using MelonLoader;'
        'namespace OldMod { static class Logging { public static void Init() {'
        '    MelonLogger.Msg("Registered (v1.0.0).");'
        '    MelonLogger.Msg("Migrated from v0.9.9.");'
        '} } }'
    ) -join "`n"
    Write-FixtureFile $root 'Source/Mods/OldMod/src/Logging.cs' $logging
    $res = Invoke-UnderTest -Script (Join-Path $root 'Tools/bump-version.ps1') -Arguments @('-Mod', 'OldMod', '-Version', '1.0.1')
    Check-Equal 0 $res.Exit 'bump exits 0'

    $code = Read-Text (Join-Path $root 'Source/Mods/OldMod/src/Mod.cs')
    Check-Match $code '"1\.0\.1"' 'MelonInfo carries the new version'
    $logging = Read-Text (Join-Path $root 'Source/Mods/OldMod/src/Logging.cs')
    Check-Match $logging 'MelonLogger\.Msg\("Registered \(v1\.0\.1\)\."\);' 'current-version runtime breadcrumb updated with intact literal'
    Check-Match $logging 'Migrated from v0\.9\.9' 'historical runtime wording left untouched'
    $json = Read-Text (Join-Path $root 'Source/Mods/OldMod/docs/mod.json')
    Check-Match $json '"version":\s*"1\.0\.1"' 'mod.json carries the new version'
    $changelog = Read-Text (Join-Path $root 'Source/Mods/OldMod/docs/CHANGELOG.md')
    Check-Match $changelog '## 1\.0\.1 \(' 'CHANGELOG has a new header'

    $compat = Read-Text (Join-Path $root 'docs/compatibility.md')
    Check-Match $compat '\|\s*OldMod\s*\|\s*1\.0\.1\s*\|' 'compatibility version updated'
    Check-Match $compat 'Date[^\r\n]*In-Game-Verify open \(prior v1\.0\.0: 2026-10-10\)|In-Game-Verify open \(prior v1\.0\.0: 2026-10-10\)' 'bare prior date converted to open with prior version/date'
    Check-Match $compat 'Verification evidence is historical for v1\.0\.0' 'compatibility notes mark evidence historical'

    $agents = Read-Text (Join-Path $root 'AGENTS.md')
    Check-Match $agents '\| \*\*OldMod\*\* \|[^\r\n]*v1\.0\.1' 'AGENTS matrix version updated'
    Check-Match $agents '\*\*In-Game-Verify open\*\* \(prior v1\.0\.0: \*\*Verified 2026-10-10\*\*\)' 'AGENTS status converted with prior version/date'
    Check-Match $agents '\*\*OldMod v1\.0\.1 \(prior v1\.0\.0: 2026-10-10\):\*\*' 'AGENTS detail header marked with prior version/date'

    $readme = Read-Text (Join-Path $root 'README.md')
    Check-Match $readme '\| \[\*\*OldMod\*\*\][^\r\n]*\| 1\.0\.1 \|' 'README table version updated'
    Check-Match $readme 'In-Game-Verify open \(prior v1\.0\.0: Active \(owner-confirmed 2026-10-10\)\)' 'README Active status converted with prior label'
    Check-Match $readme 'Verification evidence is historical for v1\.0\.0' 'README description marked historical'
    Check-Match $readme '<b>OldMod</b> · v1\.0\.1' 'README featured card updated'

    $sync = Invoke-UnderTest -Script (Join-Path $root 'Tools/check-version-sync.ps1')
    Check-Equal 0 $sync.Exit 'check-version-sync passes after the bump'
}

Run-Test 'already-open acceptance cells are preserved' {
    $root = New-Fixture -Name 'already-open' -CompatDate 'In-Game-Verify open' `
        -AgentsStatus '🧪 experimental (v1.0.0; **In-Game-Verify open**)' `
        -ReadmeStatus 'Experimental; in-game verify open' `
        -ReadmeDesc 'No acceptance claimed.'
    $before = Read-Text (Join-Path $root 'docs/compatibility.md')
    $res = Invoke-UnderTest -Script (Join-Path $root 'Tools/bump-version.ps1') -Arguments @('-Mod', 'OldMod', '-Version', '1.0.1')
    Check-Equal 0 $res.Exit 'bump exits 0'
    $after = Read-Text (Join-Path $root 'docs/compatibility.md')
    Check-Match $after '\|\s*OldMod\s*\|\s*1\.0\.1\s*\|[^\r\n]*\|\s*In-Game-Verify open\s*\|' 'open Date cell preserved'
    Check-NotMatch $after 'prior v1\.0\.0' 'open Date cell gets no prior annotation'

    $agents = Read-Text (Join-Path $root 'AGENTS.md')
    Check-Match $agents '\*\*In-Game-Verify open\*\*' 'AGENTS open label preserved'
    Check-NotMatch $agents '\| \*\*OldMod\*\* \|[^\r\n]*prior v1\.0\.0' 'AGENTS matrix open row gets no prior annotation'

    $readme = Read-Text (Join-Path $root 'README.md')
    Check-Match $readme 'Experimental; in-game verify open' 'README open status preserved'
}

Run-Test 'already-open prior evidence stays byte-identical apart from the version' {
    $status = '🧪 bounded reverse supervision (v0.8.19; 19 CS + project; 605 tests; final delta review PASS; In-Game-Verify open; prior driver-safety **Verified 2026-10-11** on 0.8.3)'
    $root = New-Fixture -Name 'already-open-prior-evidence' -Version '0.8.19' -AgentsStatus $status `
        -CompatDate 'In-Game-Verify open' `
        -ReadmeStatus 'Experimental; in-game verify open' `
        -ReadmeDesc 'No acceptance claimed.'
    $res = Invoke-UnderTest -Script (Join-Path $root 'Tools/bump-version.ps1') -Arguments @('-Mod', 'OldMod', '-Version', '0.8.20')
    Check-Equal 0 $res.Exit 'bump exits 0'

    $agents = Read-Text (Join-Path $root 'AGENTS.md')
    $expectedRow = '| **OldMod** | ' + $status.Replace('v0.8.19;', 'v0.8.20;') + ' | Mods/OldMod/ | OldMod.dll | test |'
    $actualRow = @(($agents -split "`r?`n") | Where-Object { $_ -match '^\| \*\*OldMod\*\* \|' })
    Check-Equal 1 $actualRow.Count 'exactly one AGENTS matrix row'
    Check-Equal $expectedRow $actualRow[0] 'prior acceptance evidence preserved verbatim'
    Check-NotMatch $agents 'historical for v0\.8\.19' 'no historical version is attached to prior evidence'
}

Run-Test 'plain Verified outside a bold label aborts before any write' {
    $root = New-Fixture -Name 'mixed-plain-verified' `
        -AgentsStatus '✅ active (v1.0.0, **Verified 2026-10-10**; core Verified 2026-09-15)'
    $before = Get-FixtureHash $root
    $res = Invoke-UnderTest -Script (Join-Path $root 'Tools/bump-version.ps1') -Arguments @('-Mod', 'OldMod', '-Version', '1.0.1')
    Check-Equal 2 $res.Exit 'bump aborts with exit 2'
    Check-Match $res.Output 'core Verified 2026-09-15' 'error quotes the unreconciled status'
    Check-Match $res.Output 'manuell abstimmen' 'error asks for manual status reconciliation'
    Check-Equal $before (Get-FixtureHash $root) 'fixture is byte-identical after the abort'
}

Run-Test 'missing compatibility row aborts before any write' {
    $root = New-Fixture -Name 'missing-compat-row' -OmitCompatRow
    $before = Get-FixtureHash $root
    $res = Invoke-UnderTest -Script (Join-Path $root 'Tools/bump-version.ps1') -Arguments @('-Mod', 'OldMod', '-Version', '1.0.1')
    Check-Equal 2 $res.Exit 'bump aborts with exit 2'
    Check-Match $res.Output '0 Matrixzeilen' 'error names the missing compatibility row'
    Check-Match $res.Output 'KEINE Datei geschrieben' 'error states no file was written'
    Check-Equal $before (Get-FixtureHash $root) 'fixture is byte-identical after the abort'
}

Run-Test 'duplicate compatibility rows abort before any write' {
    $root = New-Fixture -Name 'duplicate-compat-row' -DuplicateCompatRow
    $before = Get-FixtureHash $root
    $res = Invoke-UnderTest -Script (Join-Path $root 'Tools/bump-version.ps1') -Arguments @('-Mod', 'OldMod', '-Version', '1.0.1')
    Check-Equal 2 $res.Exit 'bump aborts with exit 2'
    Check-Match $res.Output '2 Matrixzeilen' 'error names the duplicate compatibility rows'
    Check-Equal $before (Get-FixtureHash $root) 'fixture is byte-identical after the abort'
}

Run-Test 'duplicate AGENTS matrix rows abort before any write' {
    $root = New-Fixture -Name 'duplicate-agents-row' -DuplicateAgentsRow
    $before = Get-FixtureHash $root
    $res = Invoke-UnderTest -Script (Join-Path $root 'Tools/bump-version.ps1') -Arguments @('-Mod', 'OldMod', '-Version', '1.0.1')
    Check-Equal 2 $res.Exit 'bump aborts with exit 2'
    Check-Match $res.Output '2 Matrixzeilen' 'error names the duplicate AGENTS rows'
    Check-Equal $before (Get-FixtureHash $root) 'fixture is byte-identical after the abort'
}

Run-Test 'unsupported plain AGENTS Verified label aborts before any write' {
    $root = New-Fixture -Name 'plain-agents-verified' -AgentsStatus '✅ active (v1.0.0, Verified 2026-10-10)'
    $before = Get-FixtureHash $root
    $res = Invoke-UnderTest -Script (Join-Path $root 'Tools/bump-version.ps1') -Arguments @('-Mod', 'OldMod', '-Version', '1.0.1')
    Check-Equal 2 $res.Exit 'bump aborts with exit 2'
    Check-Match $res.Output 'manuell abstimmen' 'error asks for manual status reconciliation'
    Check-Equal $before (Get-FixtureHash $root) 'fixture is byte-identical after the abort'
}

Run-Test 'missing README table row aborts before any write' {
    $root = New-Fixture -Name 'missing-readme-row'
    $readmePath = Join-Path $root 'README.md'
    $readme = Read-Text $readmePath
    $row = "| [**OldMod**](Source/Mods/OldMod/) | Phone | 1.0.0 | Active (owner-confirmed 2026-10-10) | Test mod; owner-confirmed 2026-10-10. |`n"
    [System.IO.File]::WriteAllText($readmePath, $readme.Replace($row, ''), $utf8NoBom)
    $before = Get-FixtureHash $root
    $res = Invoke-UnderTest -Script (Join-Path $root 'Tools/bump-version.ps1') -Arguments @('-Mod', 'OldMod', '-Version', '1.0.1')
    Check-Equal 2 $res.Exit 'bump aborts with exit 2'
    Check-Match $res.Output '0 Mod-Tabellenzeilen' 'error names the missing README row'
    Check-Equal $before (Get-FixtureHash $root) 'fixture is byte-identical after the abort'
}

Run-Test 'dry-run reports the plan and writes nothing' {
    $root = New-Fixture -Name 'dry-run'
    $before = Get-FixtureHash $root
    $res = Invoke-UnderTest -Script (Join-Path $root 'Tools/bump-version.ps1') -Arguments @('-Mod', 'OldMod', '-Version', '1.0.1', '-DryRun')
    Check-Equal 0 $res.Exit 'dry-run exits 0'
    Check-Match $res.Output 'DryRun abgeschlossen' 'dry-run reports completion'
    Check-Match $res.Output 'In-Game-Verify open \(prior v1\.0\.0' 'dry-run shows the acceptance conversion'
    Check-Equal $before (Get-FixtureHash $root) 'fixture is byte-identical after dry-run'
}

Run-Test 'same-version reconciliation is safe and idempotent' {
    $root = New-Fixture -Name 'same-version' -Version '0.1.0-dev.13' -CompatDate 'In-Game-Verify open' `
        -AgentsStatus '🧪 experimental (v0.1.0-dev.13; **In-Game-Verify open**)' `
        -ReadmeStatus 'Experimental; in-game verify open' `
        -ReadmeDesc 'No acceptance claimed.'
    $before = Get-FixtureHash $root
    $res1 = Invoke-UnderTest -Script (Join-Path $root 'Tools/bump-version.ps1') -Arguments @('-Mod', 'OldMod', '-Version', '0.1.0-dev.13')
    Check-Equal 0 $res1.Exit 'same-version run exits 0'
    Check-Match $res1.Output 'Keine Änderungen|Keine .nderungen' 'same-version run reports no changes'
    Check-Equal $before (Get-FixtureHash $root) 'same-version run writes nothing'
    $res2 = Invoke-UnderTest -Script (Join-Path $root 'Tools/bump-version.ps1') -Arguments @('-Mod', 'OldMod', '-Version', '0.1.0-dev.13')
    Check-Equal 0 $res2.Exit 'second same-version run exits 0'
    Check-Equal $before (Get-FixtureHash $root) 'second same-version run is idempotent'
}

Run-Test '-Mod All composes shared documentation edits without losing a mod' {
    $root = New-Fixture -Name 'all-two-mods'
    Add-SecondMod $root
    $res = Invoke-UnderTest -Script (Join-Path $root 'Tools/bump-version.ps1') -Arguments @('-Mod', 'All', '-Version', '1.0.1')
    Check-Equal 0 $res.Exit 'bulk bump exits 0'

    Check-Match (Read-Text (Join-Path $root 'Source/Mods/OldMod/src/Mod.cs')) '"1\.0\.1"' 'first mod code updated'
    Check-Match (Read-Text (Join-Path $root 'Source/Mods/OtherMod/src/Mod.cs')) '"1\.0\.1"' 'second mod code updated'

    $agents = Read-Text (Join-Path $root 'AGENTS.md')
    Check-Match $agents '\| \*\*OldMod\*\* \|[^\r\n]*v1\.0\.1' 'first AGENTS row updated'
    Check-Match $agents '\| \*\*OtherMod\*\* \|[^\r\n]*v1\.0\.1' 'second AGENTS row updated'
    Check-Match $agents '\| \*\*OtherMod\*\* \|[^\r\n]*In-Game-Verify open' 'second AGENTS verification label converted'

    $compat = Read-Text (Join-Path $root 'docs/compatibility.md')
    Check-Match $compat '\|\s*OldMod\s*\|\s*1\.0\.1\s*\|' 'first compatibility row updated'
    Check-Match $compat '\|\s*OtherMod\s*\|\s*1\.0\.1\s*\|' 'second compatibility row updated'

    $readme = Read-Text (Join-Path $root 'README.md')
    Check-Match $readme '\| \[\*\*OldMod\*\*\][^\r\n]*\| 1\.0\.1 \|' 'first README row updated'
    Check-Match $readme '\| \[\*\*OtherMod\*\*\][^\r\n]*\| 1\.0\.1 \|' 'second README row updated'

    $sync = Invoke-UnderTest -Script (Join-Path $root 'Tools/check-version-sync.ps1')
    Check-Equal 0 $sync.Exit 'check-version-sync passes after the bulk bump'
}

Run-Test 'check-version-sync still catches featured-card drift' {
    $root = New-Fixture -Name 'card-drift'
    $readmePath = Join-Path $root 'README.md'
    $readme = Read-Text $readmePath
    [System.IO.File]::WriteAllText($readmePath, $readme.Replace('<b>OldMod</b> · v1.0.0', '<b>OldMod</b> · v9.9.9'), $utf8NoBom)
    $res = Invoke-UnderTest -Script (Join-Path $root 'Tools/check-version-sync.ps1')
    Check-Equal 1 $res.Exit 'sync exits 1 on featured-card drift'
    Check-Match $res.Output 'weitere Nennung' 'sync reports featured-card drift'
}

Run-Test 'check-version-sync reports duplicate current matrix rows instead of last-wins' {
    $root = New-Fixture -Name 'sync-duplicate-compat-row' -DuplicateCompatRow
    $res = Invoke-UnderTest -Script (Join-Path $root 'Tools/check-version-sync.ps1')
    Check-Equal 1 $res.Exit 'sync exits 1 on duplicate matrix rows'
    Check-Match $res.Output '2 Zeilen in der Verification matrix' 'sync reports the duplicate count'
    Check-Match $res.Output 'mehrdeutig' 'sync marks the row ambiguous'
}

# ---------------------------------------------------------------------------

Write-Host ""
Write-Host ("Tests: {0} passed, {1} failed" -f $script:Passed, $script:Failed) -ForegroundColor $(if ($script:Failed -eq 0) { 'Green' } else { 'Red' })
if ($script:Failed -gt 0) {
    Write-Host "Failures:" -ForegroundColor Red
    foreach ($f in $script:Failures) { Write-Host "  - $f" -ForegroundColor Red }
    Write-Host "Artifacts kept at: $ScratchRoot" -ForegroundColor Yellow
    exit 1
}

if (-not $KeepArtifacts) {
    Remove-Item -LiteralPath $ScratchRoot -Recurse -Force -ErrorAction SilentlyContinue
} else {
    Write-Host "Artifacts kept at: $ScratchRoot" -ForegroundColor DarkGray
}
exit 0