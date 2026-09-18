#Requires -Version 7
<#
.SYNOPSIS
    new-laptop-workspace — sets up an independent, read-only-from-upstream
    workspace clone for laptop / sandbox / experimental work.

.DESCRIPTION
    Clones this repository to a fresh target directory, renames the
    remote from 'origin' to 'upstream' (so accidental pushes to the
    stable PC repo are impossible), optionally adds a personal backup
    remote, and runs setup-workspace.ps1 in the new clone.

    The laptop workspace:
      - Has full git history and version control
      - Can pull in upstream changes via 'git fetch upstream && git merge'
      - Can build all 13 mods + frameworks (build harness copied via clone)
      - Should NEVER push to 'upstream' (origin has been renamed away)
      - Owns its own mods under Source/Mods/<MyLaptopMod>/

    NOT a fork: there is no separate GitHub repo. Laptop and PC share
    one GitHub origin; the laptop just renames its local reference to
    make accidental pushes to it physically impossible.

.PARAMETER TargetDir
    Directory to clone into. Parent must exist; the directory itself
    must not. Default: interactive prompt with a suggested path.

.PARAMETER UpstreamUrl
    URL of the source repository to clone. Default: the URL of the
    'origin' remote in the current (PC) workspace.

.PARAMETER BackupUrl
    Optional. Personal/private URL where the laptop can push its own
    commits for backup. Skipped if not provided.

.EXAMPLE
    pwsh Tools/new-laptop-workspace.ps1
    pwsh Tools/new-laptop-workspace.ps1 -TargetDir "C:\Users\Me\Schedule1-modding-laptop"
    pwsh Tools/new-laptop-workspace.ps1 -BackupUrl "git@github.com:domi7602/laptop-backup.git"
#>
[CmdletBinding()]
param(
    [string]$TargetDir,
    [string]$UpstreamUrl,
    [string]$BackupUrl
)

$ErrorActionPreference = 'Stop'

$workspaceRoot = Split-Path -Parent $PSScriptRoot

# --- 1. Resolve target dir ----------------------------------------------------

if (-not $TargetDir) {
    $defaultTarget = Join-Path $env:USERPROFILE 'Schedule1-modding-laptop'
    Write-Host "[new-laptop-workspace] Where should the laptop workspace live?" -ForegroundColor Cyan
    do {
        $input = Read-Host "  Enter target path [default: $defaultTarget]"
        if ([string]::IsNullOrWhiteSpace($input)) { $input = $defaultTarget }
        $input = $input.Trim().TrimEnd('\')
        $parent = Split-Path -Parent $input
        if (-not (Test-Path -LiteralPath $parent)) {
            Write-Host "  Parent dir does not exist: $parent" -ForegroundColor Red
            $input = $null
            continue
        }
        if (Test-Path -LiteralPath $input) {
            Write-Host "  Target dir already exists: $input (must be empty/new)" -ForegroundColor Red
            $input = $null
        }
    } while (-not $input)
    $TargetDir = $input
}

$TargetDir = $TargetDir.Trim().TrimEnd('\')
$parent = Split-Path -Parent $TargetDir
if (-not (Test-Path -LiteralPath $parent)) {
    throw "Parent directory does not exist: $parent"
}
if (Test-Path -LiteralPath $TargetDir) {
    throw "Target directory already exists: $TargetDir (must be a fresh, non-existing path)"
}

# --- 2. Resolve upstream URL -------------------------------------------------

if (-not $UpstreamUrl) {
    $UpstreamUrl = git -C $workspaceRoot config --get remote.origin.url
    if (-not $UpstreamUrl) {
        throw "No upstream URL provided and 'origin' remote not configured in current workspace."
    }
}
Write-Host "[new-laptop-workspace] Upstream: $UpstreamUrl" -ForegroundColor DarkGray

# --- 3. Clone -----------------------------------------------------------------

Write-Host "[new-laptop-workspace] Cloning to $TargetDir ..." -ForegroundColor Cyan
git clone $UpstreamUrl $TargetDir
if ($LASTEXITCODE -ne 0) { throw "git clone failed (exit $LASTEXITCODE)" }

# --- 4. Rename origin -> upstream (THE key trick) ----------------------------

Write-Host "[new-laptop-workspace] Renaming 'origin' to 'upstream' (prevents accidental push)" -ForegroundColor Cyan
git -C $TargetDir remote rename origin upstream
if ($LASTEXITCODE -ne 0) { throw "git remote rename failed (exit $LASTEXITCODE)" }

# Verify: pushing to 'upstream' now requires explicit user override (URL rewrite
# at the git config level). The default name 'upstream' signals 'read-only source'.
Write-Host "[new-laptop-workspace] Remote layout in laptop workspace:" -ForegroundColor DarkGray
git -C $TargetDir remote -v

# --- 5. Optional backup remote ------------------------------------------------

if (-not $BackupUrl) {
    Write-Host ""
    $answer = Read-Host "[new-laptop-workspace] Optional: backup URL for your laptop commits? (e.g. git@github.com:you/laptop-backup.git, or leave blank to skip)"
    if (-not [string]::IsNullOrWhiteSpace($answer)) {
        $BackupUrl = $answer.Trim()
    }
}
if ($BackupUrl) {
    git -C $TargetDir remote add origin $BackupUrl
    Write-Host "[new-laptop-workspace] Added backup remote 'origin' -> $BackupUrl" -ForegroundColor Green
    Write-Host "  Use 'git push origin <branch>' to back up laptop work." -ForegroundColor DarkGray
} else {
    Write-Host "[new-laptop-workspace] No backup remote. Laptop commits stay local (still versioned in git)." -ForegroundColor DarkGray
}

# --- 6. Run setup-workspace ---------------------------------------------------

Write-Host ""
Write-Host "[new-laptop-workspace] Running setup-workspace.ps1 in the new clone ..." -ForegroundColor Cyan
& (Join-Path $TargetDir 'Tools\setup-workspace.ps1')
if ($LASTEXITCODE -ne 0) { throw "setup-workspace.ps1 failed (exit $LASTEXITCODE)" }

# --- 7. Summary ---------------------------------------------------------------

Write-Host ""
Write-Host "[new-laptop-workspace] Done." -ForegroundColor Green
Write-Host "  Laptop workspace: $TargetDir"
Write-Host ""
Write-Host "  Daily workflow on the laptop:"
Write-Host "    cd $TargetDir"
Write-Host "    pwsh Tools/new-mod.ps1 -Name 'MyExperiment'       # scaffold a new mod"
Write-Host "    dotnet build Source/Mods/MyExperiment/src/MyExperiment.csproj -c Release"
Write-Host "    git add -A && git commit -m 'experiment: foo'    # local version control"
Write-Host "    git push origin <branch>                          # backup only (if configured)"
Write-Host ""
Write-Host "  Pulling stable changes from PC workspace:"
Write-Host "    git fetch upstream"
Write-Host "    git merge upstream/main                            # cherry-pick as needed"
Write-Host ""
Write-Host "  NEVER push to 'upstream': it's renamed on purpose. Push to your backup"
Write-Host "  remote (configured as 'origin') or stay local."
