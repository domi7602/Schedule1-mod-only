#Requires -Version 7
<#
.SYNOPSIS
    Tags a mod release and creates the GitHub release with notes and ZIP attached.

.DESCRIPTION
    Automates steps 2-3 of the release process (docs/release-process.md, section 4):
      1. Version-sync gate (code <-> mod.json <-> README <-> AGENTS).
      2. Clean working tree + branch pushed to origin.
      3. Release ZIP present (runs package-release.ps1 if missing).
      4. Annotated tag <Mod>-v<version> created and pushed.
      5. GitHub release created via 'gh release create' with the matching
         CHANGELOG.md section as release notes and the ZIP attached.

    Requires: PowerShell 7, git, GitHub CLI (gh) authenticated, and - when the
    ZIP still has to be packaged - Schedule I installed with game assemblies.

.PARAMETER Mod
    Mod folder name under Source/Mods/ (as listed in AGENTS.md, e.g. NotesApp).

.PARAMETER Draft
    Create the GitHub release as a draft so the notes can be reviewed first.

.PARAMETER DryRun
    Print the planned actions (tag, ZIP, notes preview) without creating the
    tag, pushing, or creating a release.

.EXAMPLE
    pwsh Tools/release-mod.ps1 -Mod NotesApp -DryRun
    pwsh Tools/release-mod.ps1 -Mod NotesApp
    pwsh Tools/release-mod.ps1 -Mod StackLimitMod -Draft
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory)][string]$Mod,
    [switch]$Draft,
    [switch]$DryRun
)

$ErrorActionPreference = 'Stop'

$workspaceRoot = Split-Path $PSScriptRoot -Parent
$modDir = Join-Path $workspaceRoot "Source/Mods/$Mod"
$modJsonPath = Join-Path $modDir 'docs/mod.json'
$changelogPath = Join-Path $modDir 'docs/CHANGELOG.md'

if (-not (Test-Path -LiteralPath $modJsonPath)) {
    Write-Error "No docs/mod.json for mod '$Mod' ($modJsonPath). Only Source/Mods/<Mod> mods with docs/mod.json can be released."
    exit 1
}
if (-not (Get-Command gh -ErrorAction SilentlyContinue)) {
    Write-Error "GitHub CLI (gh) not found. Install it (https://cli.github.com/) and run 'gh auth login'."
    exit 1
}

$meta = Get-Content $modJsonPath -Raw | ConvertFrom-Json
$version = $meta.version
if (-not $version) {
    Write-Error "docs/mod.json for '$Mod' has no 'version' field."
    exit 1
}

$tag = "$Mod-v$version"
$zipPath = Join-Path $workspaceRoot "Release/$tag.zip"
$isPrerelease = $version -match '-'

Write-Host "=== Release $Mod v$version (tag: $tag) ===" -ForegroundColor Cyan

# --- 1. Version-sync gate -----------------------------------------------------

Write-Host '[1/5] Version-sync gate ...' -ForegroundColor Yellow
& pwsh -NoProfile -File (Join-Path $workspaceRoot 'Tools/check-version-sync.ps1')
if ($LASTEXITCODE -ne 0) {
    Write-Error 'Version drift detected — bump with Tools/bump-version.ps1 and commit before releasing.'
    exit 1
}

# --- 2. Repository state ------------------------------------------------------

Write-Host '[2/5] Repository state ...' -ForegroundColor Yellow
$branch = (git -C $workspaceRoot rev-parse --abbrev-ref HEAD).Trim()
$dirty = git -C $workspaceRoot status --porcelain
if ($dirty) {
    Write-Error 'Working tree is not clean — commit and push first (release-process.md prerequisite).'
    exit 1
}
git -C $workspaceRoot fetch origin --quiet
$localHead = (git -C $workspaceRoot rev-parse HEAD).Trim()
$remoteHead = git -C $workspaceRoot rev-parse "origin/$branch" 2>$null
if ($remoteHead -and ($localHead -ne $remoteHead.Trim())) {
    Write-Error "Local '$branch' differs from origin/$branch — push (or pull) first."
    exit 1
}
if (-not $remoteHead) {
    Write-Warning "origin/$branch not found — skipping the up-to-date check."
}
$tagLocal = git -C $workspaceRoot rev-parse -q --verify "refs/tags/$tag" 2>$null
$tagRemote = git -C $workspaceRoot ls-remote --tags origin "refs/tags/$tag" 2>$null
if ($tagLocal -or $tagRemote) {
    Write-Error "Tag '$tag' already exists local or on origin — nothing to release."
    exit 1
}

# --- 3. Release ZIP -----------------------------------------------------------

Write-Host '[3/5] Release ZIP ...' -ForegroundColor Yellow
if (Test-Path -LiteralPath $zipPath) {
    Write-Host "  Using existing Release/$tag.zip"
} elseif ($DryRun) {
    Write-Host "  [dry-run] would run: pwsh Tools/package-release.ps1 -Mod $Mod"
} else {
    & pwsh -NoProfile -File (Join-Path $workspaceRoot 'Tools/package-release.ps1') -Mod $Mod
    if ($LASTEXITCODE -ne 0) { exit 1 }
    if (-not (Test-Path -LiteralPath $zipPath)) {
        Write-Error "package-release.ps1 did not produce $zipPath."
        exit 1
    }
}

# --- 4. Release notes from the changelog --------------------------------------

Write-Host '[4/5] Release notes ...' -ForegroundColor Yellow
$notesBody = ''
if (Test-Path -LiteralPath $changelogPath) {
    $clRaw = [System.IO.File]::ReadAllText($changelogPath, [System.Text.Encoding]::UTF8)
    $sectionPattern = '(?ms)^##\s+' + [regex]::Escape($version) + '(\s|\r?\n).*?(?=^##\s|\z)'
    $notesBody = ([regex]::Match($clRaw, $sectionPattern)).Value.Trim()
    if (-not $notesBody) {
        Write-Warning "No '## $version' section found in docs/CHANGELOG.md - the notes will only contain the footer."
    }
} else {
    Write-Warning 'docs/CHANGELOG.md not found - the notes will only contain the footer.'
}
$footer = @(
    ''
    '---'
    'Requires MelonLoader 0.7.3 and S1API 3.2.x (S1MAPI where applicable).'
    'Game compatibility and per-mod verification: https://github.com/domi7602/Schedule1-mod-only/blob/main/docs/compatibility.md'
) -join "`n"
$notes = (($notesBody + "`n" + $footer).Trim()) + "`n"
$notesFile = Join-Path $env:TEMP "$tag-notes.md"
[System.IO.File]::WriteAllText($notesFile, $notes, [System.Text.UTF8Encoding]::new($false))
Write-Host "  Notes file: $notesFile"
Write-Host '  --- preview ---'
$notes -split "`n" | Select-Object -First 6 | ForEach-Object { Write-Host "  | $_" }
Write-Host '  ----------------'

if ($DryRun) {
    Write-Host "[dry-run] Would create tag '$tag', push it, and create the GitHub release (ZIP: $zipPath; draft: $($Draft.IsPresent); prerelease: $isPrerelease)." -ForegroundColor Green
    exit 0
}

# --- 5. Tag + GitHub release --------------------------------------------------

Write-Host '[5/5] Creating tag and GitHub release ...' -ForegroundColor Yellow
git -C $workspaceRoot tag -a $tag -m "$Mod v$version"
if ($LASTEXITCODE -ne 0) { Write-Error 'git tag failed.'; exit 1 }
git -C $workspaceRoot push origin "refs/tags/$tag"
if ($LASTEXITCODE -ne 0) { Write-Error 'git push of the tag failed.'; exit 1 }

$ghArgs = @('release', 'create', $tag, $zipPath, '--title', "$Mod v$version", '--notes-file', $notesFile, '--verify-tag')
if ($Draft) { $ghArgs += '--draft' }
if ($isPrerelease) { $ghArgs += '--prerelease' }
& gh @ghArgs
if ($LASTEXITCODE -ne 0) {
    $manual = @(
        'gh release create failed - the tag IS already pushed. Finish manually:'
        "  gh release create $tag `"$zipPath`" --title `"$Mod v$version`" --notes-file $notesFile"
        "Or undo the tag: git push origin :refs/tags/$tag ; git tag -d $tag"
    ) -join "`n"
    Write-Error $manual
    exit 1
}

Write-Host "`n[OK] Released $Mod v$version" -ForegroundColor Green
Write-Host "  Tag:     $tag"
Write-Host "  Release: https://github.com/domi7602/Schedule1-mod-only/releases/tag/$tag"

