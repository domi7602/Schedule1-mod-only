<#
.SYNOPSIS
    Automated high-speed backup of Schedule I game, saves, and modding workspace to D:\ drive.
.DESCRIPTION
    Uses multi-threaded robocopy to mirror the entire Game folder, Savegames, and Workspace
    to D:\Schedule 1\Backups\ with timestamped folders and a Latest snapshot.
#>

param(
    [string]$TargetRoot = "D:\Schedule 1\Backups"
)

$ErrorActionPreference = "Stop"
$sw = [System.Diagnostics.Stopwatch]::StartNew()

$timestamp = Get-Date -Format "yyyy-MM-dd_HH-mm-ss"
$backupDir = Join-Path $TargetRoot "Backup_$timestamp"
$latestDir = Join-Path $TargetRoot "Latest"

Write-Host "============================================================" -ForegroundColor Cyan
Write-Host "  SCHEDULE I — AUTOMATED BACKUP TO D:\ DRIVE" -ForegroundColor Cyan
Write-Host "  Destination: $backupDir" -ForegroundColor Yellow
Write-Host "============================================================" -ForegroundColor Cyan

# 1. Ensure target directories exist
if (!(Test-Path $TargetRoot)) {
    New-Item -ItemType Directory -Path $TargetRoot -Force | Out-Null
}
New-Item -ItemType Directory -Path $backupDir -Force | Out-Null

# Robocopy helper
function Run-FastCopy {
    param(
        [string]$Source,
        [string]$Destination,
        [string]$Label,
        [string[]]$Excludes = @()
    )

    if (!(Test-Path $Source)) {
        Write-Host "  [SKIP] $Label not found at: $Source" -ForegroundColor DarkGray
        return
    }

    Write-Host "`n  [*] Backing up $Label..." -ForegroundColor White
    New-Item -ItemType Directory -Path $Destination -Force | Out-Null

    $excludeArgs = @()
    if ($Excludes.Count -gt 0) {
        $excludeArgs += "/XD"
        $excludeArgs += $Excludes
    }

    $robocopyArgs = @(
        $Source,
        $Destination,
        "/E",          # copy subdirectories, including Empty ones
        "/MT:16",      # 16 worker threads
        "/R:1",        # retry 1 time
        "/W:1",        # wait 1 second
        "/NP",         # no progress percentage
        "/NDL",        # no directory list
        "/NFL",        # no file list
        "/NJH",        # no job header
        "/NJS"         # no job summary
    ) + $excludeArgs

    & robocopy.exe @robocopyArgs | Out-Null
    $code = $LASTEXITCODE
    # Robocopy exit codes 0-7 mean success/files copied
    if ($code -le 7) {
        Write-Host "  [OK] $Label successfully backed up." -ForegroundColor Green
    } else {
        Write-Host "  [WARN] Robocopy finished with exit code $code for $Label." -ForegroundColor Yellow
    }
}

# 2. Backup Game Folder (Steam) — Aufloesung wie Directory.Build.props:
#    $env:SCHEDULE1_PATH sonst Default-Install-Pfad.
$gameSource = if ($env:SCHEDULE1_PATH) { $env:SCHEDULE1_PATH } else { "C:\Program Files (x86)\Steam\steamapps\common\Schedule I" }
$gameDest = Join-Path $backupDir "Game"
Run-FastCopy -Source $gameSource -Destination $gameDest -Label "Game Directory (MelonLoader + Mods + Vanilla)"

# 3. Backup Savegames (AppData) — maschinenunabhaengig via USERPROFILE.
$savesSource = Join-Path $env:USERPROFILE "AppData\LocalLow\TVGS\Schedule I"
$savesDest = Join-Path $backupDir "Saves"
Run-FastCopy -Source $savesSource -Destination $savesDest -Label "Savegames & Settings (AppData)"

# 4. Backup Workspace (Source, Skills, docs) — dieses Repo selbst (liegt
#    seit der Neuinstallation 2026-09 im Spiel-Ordner).
$wsSource = Split-Path -Parent $PSScriptRoot
$wsDest = Join-Path $backupDir "Workspace"
$wsExcludes = @("bin", "obj", ".vs", "Temp", "Library", "Build", "Builds", "objIL2CPP", "binIL2CPP", ".git")
Run-FastCopy -Source $wsSource -Destination $wsDest -Label "Modding Workspace" -Excludes $wsExcludes

# 5. Mirror to Latest
Write-Host "`n  [*] Updating 'Latest' mirror..." -ForegroundColor White
& robocopy.exe $backupDir $latestDir /MIR /MT:16 /R:1 /W:1 /NP /NDL /NFL /NJH /NJS | Out-Null
Write-Host "  [OK] 'Latest' mirror updated." -ForegroundColor Green

$sw.Stop()
$totalSize = (Get-ChildItem -Path $backupDir -Recurse -File | Measure-Object -Property Length -Sum).Sum / 1GB

Write-Host "`n============================================================" -ForegroundColor Cyan
Write-Host ("  BACKUP COMPLETED IN {0:F1}s! Total Size: {1:F2} GB" -f $sw.Elapsed.TotalSeconds, $totalSize) -ForegroundColor Green
Write-Host "  Location: $backupDir" -ForegroundColor Yellow
Write-Host "============================================================`n" -ForegroundColor Cyan
