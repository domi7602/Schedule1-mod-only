<#
.SYNOPSIS
Health check for the Skills/ directory (schedule1-mod-only workspace).

.DESCRIPTION
Static scans only — no game required. Reports, per category:
  1. SKILL.md issues: missing/mismatched frontmatter, missing version anchor
  2. Reference files: missing verification header (> verified / > UNVERIFIED)
  3. Duplicate reference filenames across skills (drift risk)
  4. Stale version strings: "3.2.0", "3.2.1-beta.7" without 8, "0.4.6f13"/"0.4.7f6" claims, "OnSaveLoaded" (non-existent event)
  5. Broken relative links between skill files
  6. CRLF line endings (repo convention is LF)
Exit code 1 if any FAIL-level issue; WARN-level issues exit 0.

.PARAMETER Root
Workspace root. Defaults to the script's parent directory (repo root).

.EXAMPLE
pwsh -NoProfile -ExecutionPolicy Bypass -File Tools\check-skills-health.ps1
#>
param(
    [string]$Root = (Split-Path -Parent $PSScriptRoot)
)

$ErrorActionPreference = 'Stop'
$skillsRoot = Join-Path $Root 'Skills'
$fail = 0; $warn = 0
$stalePatterns = @(
    @{ Pattern = 'S1API 3\.2\.0|v3\.2\.0'; Label = 'S1API 3.2.0 (stable lacks 0.4.7f6+ renames)' },
    @{ Pattern = '3\.2\.1-beta\.7(?!\s*\+?)'; Label = 'S1API 3.2.1-beta.7 (deployed is beta.8)' },
    @{ Pattern = 'Game v0\.4\.7f6(?![0-9])'; Label = 'Game v0.4.7f6 (live is 0.4.7f9)' },
    @{ Pattern = 'Game v0\.4\.6f1[13]'; Label = 'Game v0.4.6f1x' },
    @{ Pattern = 'GameLifecycle\.OnSaveLoaded'; Label = 'GameLifecycle.OnSaveLoaded (event does not exist)' },
    @{ Pattern = 'Color\.brown|Color\.cream'; Label = 'non-existent UnityEngine color' },
    @{ Pattern = '(?<![.\w])UITheme\.Initialize\('; Label = 'bare UITheme.Initialize (use S1Mods.Shared.UITheme.InitializeForTextApp/Dashboard)' }
)

function Add-Fail($msg) { Write-Host "  [FAIL] $msg" -ForegroundColor Red; $script:fail++ }
function Add-Warn($msg) { Write-Host "  [WARN] $msg" -ForegroundColor Yellow; $script:warn++ }
function Add-Ok  ($msg) { Write-Host "  [ok]   $msg" -ForegroundColor DarkGray }

Write-Host "Skills health check — root: $skillsRoot"

# --- 1. SKILL.md checks -------------------------------------------------
$skills = Get-ChildItem $skillsRoot -Directory | Where-Object Name -like 'schedule1-*'
foreach ($s in $skills) {
    $skillPath = Join-Path $s.FullName 'SKILL.md'
    if (-not (Test-Path $skillPath)) { Add-Fail "$($s.Name): SKILL.md missing"; continue }
    $skillRaw = [System.IO.File]::ReadAllText($skillPath)
    if ($skillRaw -notmatch '(?m)^---\s*\n') { Add-Warn "$($s.Name): no frontmatter block" }
    elseif ($skillRaw -notmatch "(?m)^name:\s*$([regex]::Escape($s.Name))\s*$") { Add-Fail "$($s.Name): frontmatter name != directory name" }
    if ($skillRaw -notmatch '(?i)version anchor') { Add-Warn "$($s.Name): no version-anchor line" }
    $desc = if ($skillRaw -match '(?ms)^description:\s*(.+?)\n\w') { $Matches[1] } else { '' }
    if ($skillRaw -notmatch '(?m)^.*Keywords:') { Add-Warn "$($s.Name): description has no Keywords (README convention)" }
}

# --- 2. Reference verification headers ----------------------------------
$refs = Get-ChildItem $skillsRoot -Recurse -Filter *.md | Where-Object {
    $_.FullName -ne (Join-Path $skillsRoot 'README.md') -and $_.Name -ne 'SKILL.md' -and $_.Name -ne '_index.md' -and $_.Name -ne 'README.md'
}
foreach ($r in $refs) {
    $raw = [System.IO.File]::ReadAllText($r.FullName)
    $head = ($raw -split "`n", 3)[0..1] -join ' '
    if ($raw -match '(?m)^> (?:UNVERIFIED|verified|Canonical|\*\*Canonical|\*\*Redirect stub)') { Add-Ok "$($r.Name): header ok" }
    else { Add-Warn "$($r.Name): no verification header (add '> verified ...' or '> UNVERIFIED ...')" }
}

# --- 3. Duplicate filenames across skills -------------------------------
$dupes = $refs | Group-Object Name | Where-Object Count -gt 1
foreach ($d in $dupes) { Add-Warn "duplicate reference filename across skills: $($d.Name) x$($d.Count) — consolidation candidate" }

# --- 4. Stale version strings -------------------------------------------
foreach ($r in ($refs + (Get-ChildItem $skillsRoot -Recurse -Filter SKILL.md))) {
    $raw = [System.IO.File]::ReadAllText($r.FullName)
    if ($r.Name -eq 'update-breakage-log.md') { continue }  # logs stale versions by design
    foreach ($sp in $stalePatterns) {
        $hits = [regex]::Matches($raw, $sp.Pattern)
        foreach ($h in $hits) {
            $lineNo = ($raw.Substring(0, $h.Index) -split "`n").Count
            $line = ($raw -split "`n")[$lineNo - 1]
            # skip historical/evidence lines (breakage logs, verification logs, BROKEN markers)
            if ($line -match '(?i)historical|broken|verified|refuted|dead|stub|does not exist|removed|has NO Color|lacks|kennt') { continue }
            Add-Warn "$($r.Name):$lineNo stale string '$($h.Value)' — $($sp.Label)"
        }
    }
}

# --- 5. Relative link targets -------------------------------------------
foreach ($r in $refs) {
    $raw = [System.IO.File]::ReadAllText($r.FullName)
    $linkMatches = [regex]::Matches($raw, '\]\(([^)#\s]+\.md)\)')
    foreach ($lm in $linkMatches) {
        $target = $lm.Groups[1].Value
        if ($target -match '^https?://') { continue }
        $resolved = Join-Path $r.DirectoryName ($target -replace '/', '\')
        if (-not (Test-Path $resolved)) {
            Add-Fail "$($r.Name): broken relative link -> $target"
        }
    }
}

# --- 6. CRLF check -------------------------------------------------------
foreach ($r in ($refs + (Get-ChildItem $skillsRoot -Recurse -Filter SKILL.md))) {
    $bytes = [System.IO.File]::ReadAllBytes($r.FullName)
    for ($i = 0; $i -lt $bytes.Length - 1; $i++) {
        if ($bytes[$i] -eq 13 -and $bytes[$i+1] -eq 10) { Add-Warn "$($r.Name): CRLF endings (convention: LF)"; break }
    }
}

Write-Host ""
if ($fail -gt 0) { Write-Host "RESULT: $fail FAIL / $warn WARN" -ForegroundColor Red; exit 1 }
elseif ($warn -gt 0) { Write-Host "RESULT: 0 FAIL / $warn WARN (review recommended)" -ForegroundColor Yellow; exit 0 }
else { Write-Host "RESULT: clean" -ForegroundColor Green; exit 0 }
