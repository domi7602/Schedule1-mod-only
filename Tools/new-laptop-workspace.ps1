#Requires -Version 7
<#
.SYNOPSIS
    new-laptop-workspace — sets up an independent
    workspace clone for laptop / sandbox / experimental work.

.DESCRIPTION
    Recursively clones this repository to a fresh target directory, renames the
    remote from 'origin' to 'upstream', optionally adds a personal backup
    remote, and runs setup-workspace.ps1 in the new clone.

    The laptop workspace:
      - Has full git history and version control
      - Can pull in upstream changes via 'git fetch upstream && git merge'
      - Includes the pinned submodules and current build harness
      - Uses 'upstream' as the source remote by convention, not a push barrier
      - Owns its own mods under Source/Mods/<MyLaptopMod>/

    NOT a fork: there is no separate GitHub repo. Laptop and PC share
    one GitHub source. Renaming a remote does not disable pushing to it.
    This helper never builds, deploys, commits or pushes. Failed new clones are
    retained for diagnosis; existing targets are never changed.

.PARAMETER TargetDir
    Directory to clone into. Parent must exist; the directory itself
    must not. Default: interactive prompt with a suggested path.

.PARAMETER UpstreamUrl
    URL of the source repository to clone. Default: the URL of the
    'origin' remote in the current (PC) workspace.

.PARAMETER BackupUrl
    Optional. Personal/private URL where the laptop can push its own
    commits for backup. Prompts when omitted unless -NonInteractive is set.

.EXAMPLE
    pwsh Tools/new-laptop-workspace.ps1
    pwsh Tools/new-laptop-workspace.ps1 -TargetDir "C:\Users\Me\Schedule1-modding-laptop"
    pwsh Tools/new-laptop-workspace.ps1 -BackupUrl "git@github.com:domi7602/laptop-backup.git"
#>
[CmdletBinding()]
param(
    [string]$TargetDir,
    [string]$UpstreamUrl,
    [string]$BackupUrl,
    [string]$GameDir,
    [switch]$EnableHooks,
    [switch]$NonInteractive
)

$ErrorActionPreference = 'Stop'
$PSNativeCommandUseErrorActionPreference = $false

$workspaceRoot = Split-Path -Parent $PSScriptRoot
$git = (Get-Command git -CommandType Application -ErrorAction Stop | Select-Object -First 1).Source

# --- 1. Resolve target dir ----------------------------------------------------

if (-not $TargetDir) {
    if ($NonInteractive) { throw 'Pass -TargetDir for noninteractive workspace creation.' }
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

$TargetDir = [IO.Path]::GetFullPath($TargetDir.Trim()).TrimEnd('\', '/')
$parent = Split-Path -Parent $TargetDir
if (-not (Test-Path -LiteralPath $parent -PathType Container)) {
    throw "Parent directory does not exist: $parent"
}
if (Test-Path -LiteralPath $TargetDir) {
    throw "Target directory already exists: $TargetDir (must be a fresh, non-existing path)"
}

# --- 2. Resolve upstream URL -------------------------------------------------

if (-not $UpstreamUrl) {
    $UpstreamUrl = & $git -C $workspaceRoot config --get remote.origin.url 2>&1
    if ($LASTEXITCODE -ne 0 -or -not $UpstreamUrl) {
        throw "No upstream URL provided and 'origin' remote not configured in current workspace."
    }
    $UpstreamUrl = ([string]$UpstreamUrl).Trim()
}
Write-Host "[new-laptop-workspace] Upstream: $UpstreamUrl" -ForegroundColor DarkGray

# --- 3. Clone -----------------------------------------------------------------

Write-Host "[new-laptop-workspace] Cloning to $TargetDir ..." -ForegroundColor Cyan
& $git -c core.longpaths=true clone --recurse-submodules -- $UpstreamUrl $TargetDir
if ($LASTEXITCODE -ne 0) { throw "git clone failed (exit $LASTEXITCODE)" }

# --- 4. Name the source remote ----------------------------------------------

Write-Host "[new-laptop-workspace] Renaming source remote to 'upstream' (naming convention; pushes remain possible)" -ForegroundColor Cyan
& $git -C $TargetDir remote rename origin upstream
if ($LASTEXITCODE -ne 0) { throw "git remote rename failed (exit $LASTEXITCODE)" }

Write-Host "[new-laptop-workspace] Remote layout in laptop workspace:" -ForegroundColor DarkGray
& $git -C $TargetDir remote -v
if ($LASTEXITCODE -ne 0) { throw "git remote inspection failed (exit $LASTEXITCODE)" }

# --- 5. Optional backup remote ------------------------------------------------

if (-not $BackupUrl -and -not $NonInteractive) {
    Write-Host ""
    $answer = Read-Host "[new-laptop-workspace] Optional: backup URL for your laptop commits? (e.g. git@github.com:you/laptop-backup.git, or leave blank to skip)"
    if (-not [string]::IsNullOrWhiteSpace($answer)) {
        $BackupUrl = $answer.Trim()
    }
}
if ($BackupUrl) {
    & $git -C $TargetDir remote add origin $BackupUrl
    if ($LASTEXITCODE -ne 0) { throw "git backup remote creation failed (exit $LASTEXITCODE)" }
    Write-Host "[new-laptop-workspace] Added backup remote 'origin' -> $BackupUrl" -ForegroundColor Green
    Write-Host "  Use 'git push origin <branch>' to back up laptop work." -ForegroundColor DarkGray
} else {
    Write-Host "[new-laptop-workspace] No backup remote. Laptop commits stay local (still versioned in git)." -ForegroundColor DarkGray
}

# --- 6. Run setup-workspace ---------------------------------------------------

Write-Host ""
Write-Host "[new-laptop-workspace] Running setup-workspace.ps1 in the new clone ..." -ForegroundColor Cyan
$setupScript = Join-Path $TargetDir 'Tools/setup-workspace.ps1'
if (-not (Test-Path -LiteralPath $setupScript -PathType Leaf)) { throw "Cloned workspace has no setup helper: $setupScript" }
$setupArguments = @('-NoProfile', '-ExecutionPolicy', 'Bypass', '-File', $setupScript)
if ($GameDir) { $setupArguments += @('-GameDir', $GameDir) }
if ($EnableHooks) { $setupArguments += '-EnableHooks' }
if ($NonInteractive) { $setupArguments += '-NonInteractive' }
$pwsh = (Get-Process -Id $PID).Path
& $pwsh @setupArguments
if ($LASTEXITCODE -ne 0) { throw "setup-workspace.ps1 failed (exit $LASTEXITCODE)" }

# --- 7. Summary ---------------------------------------------------------------

Write-Host ""
Write-Host "[new-laptop-workspace] Done." -ForegroundColor Green
Write-Host "  Laptop workspace: $TargetDir"
Write-Host ""
Write-Host "  Daily workflow on the laptop:"
Write-Host "    cd $TargetDir"
Write-Host '    pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/verify.ps1 -Mode Auto'
Write-Host '    Commit or push only when explicitly authorized.'
Write-Host ""
Write-Host "  Pulling stable changes from PC workspace:"
Write-Host "    git fetch upstream"
Write-Host "    git merge upstream/main                            # cherry-pick as needed"
Write-Host ""
Write-Host "  'upstream' names the source; it is not technically read-only."
Write-Host '  No build, deployment, commit or push was performed by this helper.'
