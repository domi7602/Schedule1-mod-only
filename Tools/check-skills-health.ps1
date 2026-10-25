<#
.SYNOPSIS
Health check for the Skills/ directory (schedule1-mod-only workspace).

.DESCRIPTION
Static scans only — no game required. Reports, per category:
  1. SKILL.md issues: missing/mismatched frontmatter, empty description, missing canonical runtime-config pointer
  2. Reference files: missing verification header (> verified / > UNVERIFIED)
  3. Duplicate reference filenames across skills (drift risk)
  4. Pinned game/framework/tool versions in Skills; non-existent "OnSaveLoaded" event.
     A version literal can be kept as historical/observed evidence by placing
     '<!-- skills-health: version-evidence (reason) -->' on its line or the line
     immediately above. The parenthesized reason is mandatory and must be nonempty.
     The annotation exempts the whole annotated line (and the whole following line
     when placed above) — never a single token — so keep evidence lines short.
     It only exempts version/toolchain patterns: never stale API names and never
     structural markers such as a 'Version anchor' heading.
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
    @{ Pattern = '\b\d+\.\d+\.\d+f\d+\b'; Label = 'game build number (keep current runtime in AGENTS.md)'; Kind = 'version' },
    @{ Pattern = '(?i)\bS1API\s+\d+(?:\.\d+)*(?:-beta\.\d+)?'; Label = 'S1API release number (keep dependency state in AGENTS.md)'; Kind = 'version' },
    @{ Pattern = '(?i)\bS1MAPI\s+\d+(?:\.\d+)*'; Label = 'S1MAPI release number (keep dependency state in AGENTS.md)'; Kind = 'version' },
    @{ Pattern = '(?i)\bMelonLoader\s+\d+(?:\.\d+)*'; Label = 'MelonLoader release number (keep runtime state in AGENTS.md)'; Kind = 'version' },
    @{ Pattern = '(?i)\bUnity(?: Editor)?\s+20\d{2}\.\d+'; Label = 'Unity release number (keep runtime state in AGENTS.md)'; Kind = 'version' },
    @{ Pattern = '(?<![\d.])\d+\.\d+\.\d+(?![\d.])'; Label = 'concrete semantic version (use a source-of-truth file or placeholder)'; Kind = 'version' },
    @{ Pattern = '(?i)\.NET\s+\d+|\bSDK\s+\d+(?:\.\d+)*|\bFishNet\s+\d'; Label = 'toolchain/library version (keep current values in canonical config)'; Kind = 'version' },
    @{ Pattern = '(?i)\bnet\d+\.\d+|\bC#\s+\d+'; Label = 'target framework/language version (keep build settings in AGENTS.md/project props)'; Kind = 'version' },
    @{ Pattern = '(?i)\bilspycmd\s+\d+(?:\.\d+)*'; Label = 'ilspycmd release number (avoid toolchain pins in Skills)'; Kind = 'version' },
    @{ Pattern = '(?i)\bPython\s+\d+(?:\.\d+)*|\bmcp\s*[>=]+\s*\d'; Label = 'Python/MCP package version (use repository setup files)'; Kind = 'version' },
    @{ Pattern = '\bv\d+\.\d+\.\d+\b'; Label = 'concrete release version (use a source-of-truth file)'; Kind = 'version' },
    @{ Pattern = 'Version anchor'; Label = 'per-skill version anchor heading (structural convention: versions are centralized)'; Kind = 'structure' },
    @{ Pattern = 'GameLifecycle\.OnSaveLoaded'; Label = 'GameLifecycle.OnSaveLoaded (event does not exist)'; Kind = 'api' },
    @{ Pattern = 'Color\.brown|Color\.cream'; Label = 'non-existent UnityEngine color'; Kind = 'api' },
    @{ Pattern = '(?<![.\w])UITheme\.Initialize\('; Label = 'bare UITheme.Initialize (use S1Mods.Shared.UITheme.InitializeForTextApp/Dashboard)'; Kind = 'api' }
)
# Narrow, adjacent-line opt-in for version literals that are evidence rather than pin
# guidance. A nonempty parenthesized reason is mandatory. By design the annotation
# applies to the WHOLE annotated line (and the whole following line when it sits above),
# not to the nearest token — a deliberately simple rule, so keep evidence lines short.
# It never applies to API-name or structural patterns, and never beyond the next line.
$versionEvidencePattern = '<!--\s*skills-health:\s*version-evidence\s*\((?=[^)]*[^\s)])[^)]*\)\s*-->'
$quotedDescriptionPattern = @'
^(?:"(?<value>(?:\\.|[^"\\])*)"|'(?<value>(?:''|[^'])*)')(?:[ \t]+#.*)?$
'@

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
    if ($skillRaw -notmatch '(?i)AGENTS\.md') { Add-Warn "$($s.Name): no link to canonical runtime/dependency configuration" }
    # The repository profile uses one-line descriptions. Check the actual metadata
    # block; body text and keyword catalogs are not substitutes for a description.
    $frontmatter = [regex]::Match($skillRaw, '\A---\r?\n(?<body>.*?)\r?\n---(?:\r?\n|\z)', 'Singleline')
    $description = [regex]::Match($frontmatter.Groups['body'].Value, '(?m)^description:[ \t]*([^\r\n]*)')
    $descriptionValue = $description.Groups[1].Value.Trim()
    $quotedDescription = [regex]::Match($descriptionValue, $quotedDescriptionPattern)
    if ($quotedDescription.Success) {
        $descriptionText = $quotedDescription.Groups['value'].Value.Trim()
    }
    elseif ($descriptionValue.StartsWith('"') -or $descriptionValue.StartsWith("'")) {
        $descriptionText = '' # Incomplete or malformed quoted metadata is not a description.
    }
    else {
        $descriptionText = ($descriptionValue -replace '[ \t]+#.*$', '').Trim()
    }
    if (-not $descriptionText -or $descriptionValue.StartsWith('#')) {
        Add-Fail "$($s.Name): missing or empty frontmatter description"
    }
}

# --- 2. Reference verification headers ----------------------------------
# Force arrays: a single match collapses to a scalar FileInfo, and '$scalar + $array'
# calls op_Addition on FileInfo, which throws — sections 4-6 then never run and the
# tree looks clean despite the bug (regression-covered in Tools/Tests/skills-health.Tests.ps1).
$refs = @(Get-ChildItem $skillsRoot -Recurse -Filter *.md | Where-Object {
    $_.FullName -ne (Join-Path $skillsRoot 'README.md') -and $_.Name -ne 'SKILL.md' -and $_.Name -ne '_index.md' -and $_.Name -ne 'README.md'
})
$skillFiles = @(Get-ChildItem $skillsRoot -Recurse -Filter SKILL.md)
$scannedFiles = @($refs) + @($skillFiles)
foreach ($r in $refs) {
    $raw = [System.IO.File]::ReadAllText($r.FullName)
    $head = ($raw -split "`n", 3)[0..1] -join ' '
    if ($raw -match '(?m)^> (?:UNVERIFIED|verified|Canonical|\*\*Canonical|\*\*Redirect stub)') { Add-Ok "$($r.Name): header ok" }
    else { Add-Warn "$($r.Name): no verification header (add '> verified ...' or '> UNVERIFIED ...')" }
}

# --- 3. Duplicate filenames across skills -------------------------------
$dupes = $refs | Group-Object Name | Where-Object Count -gt 1
foreach ($d in $dupes) { Add-Warn "duplicate reference filename across skills: $($d.Name) x$($d.Count) — consolidation candidate" }

# --- 4. Version pins and stale API names --------------------------------
foreach ($r in $scannedFiles) {
    $raw = [System.IO.File]::ReadAllText($r.FullName)
    $lines = $raw -split "`n"
    foreach ($sp in $stalePatterns) {
        $hits = [regex]::Matches($raw, $sp.Pattern)
        foreach ($h in $hits) {
            $lineNo = ($raw.Substring(0, $h.Index) -split "`n").Count
            $line = $lines[$lineNo - 1]
            $prevLine = if ($lineNo -gt 1) { $lines[$lineNo - 2] } else { '' }
            $annotatedEvidence = $sp.Kind -eq 'version' -and
                ($line -match $versionEvidencePattern -or $prevLine -match $versionEvidencePattern)
            if ($annotatedEvidence) {
                Add-Ok "$($r.Name):$lineNo version evidence '$($h.Value)' — annotated historical/observed, not a pin"
                continue
            }
            if ($sp.Label -eq 'non-existent UnityEngine color' -and $line -match '(?i)NO Color|does not exist') { continue }
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
foreach ($r in $scannedFiles) {
    $bytes = [System.IO.File]::ReadAllBytes($r.FullName)
    for ($i = 0; $i -lt $bytes.Length - 1; $i++) {
        if ($bytes[$i] -eq 13 -and $bytes[$i+1] -eq 10) { Add-Warn "$($r.Name): CRLF endings (convention: LF)"; break }
    }
}

Write-Host ""
if ($fail -gt 0) { Write-Host "RESULT: $fail FAIL / $warn WARN" -ForegroundColor Red; exit 1 }
elseif ($warn -gt 0) { Write-Host "RESULT: 0 FAIL / $warn WARN (review recommended)" -ForegroundColor Yellow; exit 0 }
else { Write-Host "RESULT: clean" -ForegroundColor Green; exit 0 }
