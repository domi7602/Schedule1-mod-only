#Requires -Version 7
<#
.SYNOPSIS
    Deploy-ThirdParty — iterates ThirdParty/*.dll, applies .deployignore whitelist,
    and force-copies the rest to <GameDir>\Mods\.
.DESCRIPTION
    Aufgerufen aus Source/Mods/Directory.Build.targets (Target: DeployThirdParty,
    AfterTargets: Build). Drei Env-Vars werden gelesen:
      SCHEDULE1_PATH   -> Spiel-Wurzel (Fallback: 'C:\Program Files (x86)\Steam\steamapps\common\Schedule I')
      GITHUB_OUTPUT    -> irrelevant (nur CI-Hilfskanal)
    Standard-Skript-Errors (Datei fehlt, Permission denied) brechen mit Exit 1 ab.
.EXAMPLE
    pwsh -NoProfile -File Tools/deploy-thirdparty.ps1
#>

$ErrorActionPreference = 'Stop'

# Process-lock (Mutex): MSBuild ruft dieses Script aus jedem csproj parallel auf.
# Ohne Lock wuerden 16 Prozesse gleichzeitig dieselbe PhoneScroll.dll loeschen
# und kopieren -> I/O-Race, exit 1. Wir verwenden ein benanntes Mutex, das alle
# nachfolgenden Aufrufe ueberspringen laesst (nur der erste kopiert).
$mutexName = 'Global\Schedule1DeployThirdParty'
$mutex = New-Object System.Threading.Mutex($false, $mutexName)
try {
    $acquired = $mutex.WaitOne([TimeSpan]::FromSeconds(1), $false)
    if (-not $acquired) {
        # Anderer Prozess laeuft bereits — ueberspringen.
        Write-Host "[deploy-thirdparty] Skipped (another build process is deploying)." -ForegroundColor DarkGray
        exit 0
    }
} catch {
    # Falls Mutex nicht verfuegbar (z.B. ohne Global-Rechte), trotzdem weiterlaufen.
    Write-Host "[deploy-thirdparty] Mutex unavailable, proceeding without lock: $($_.Exception.Message)" -ForegroundColor DarkYellow
}

try {

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$workspaceRoot = Split-Path -Parent $scriptDir
$thirdPartyRoot = Join-Path $workspaceRoot 'ThirdParty'
$deployIgnorePath = Join-Path $thirdPartyRoot '.deployignore'

$gameDir = if ($env:SCHEDULE1_PATH) { $env:SCHEDULE1_PATH } else { 'C:\Program Files (x86)\Steam\steamapps\common\Schedule I' }
$modsDir = Join-Path $gameDir 'Mods'

# Hard precheck (analog package-release.ps1)
$probeRef = Join-Path $gameDir 'MelonLoader\Il2CppAssemblies\Assembly-CSharp.dll'
if (-not (Test-Path -LiteralPath $probeRef)) {
    Write-Host "[deploy-thirdparty] Game not found at '$gameDir' — skip." -ForegroundColor Yellow
    exit 0
}

if (-not (Test-Path -LiteralPath $thirdPartyRoot)) {
    Write-Host "[deploy-thirdparty] No ThirdParty/ folder — skip." -ForegroundColor Yellow
    exit 0
}

# Whitelist laden (Posix-Glob, eine Zeile pro Pattern, # = Kommentar)
$ignorePatterns = @()
if (Test-Path -LiteralPath $deployIgnorePath) {
    $ignorePatterns = Get-Content -LiteralPath $deployIgnorePath |
        Where-Object { $_ -notmatch '^\s*#' -and $_ -notmatch '^\s*$' } |
        ForEach-Object { $_.Trim() }
}

function Test-Ignored {
    param([string]$RelativePath, [string[]]$Patterns)
    foreach ($p in $Patterns) {
        # Posix-Glob: * = beliebig, ? = einzelnes Zeichen, \ = Escape
        $regex = '^' + ($p -replace '\\', '\\\\' -replace '\*', '.*' -replace '\?', '.') + '$'
        if ($RelativePath -match $regex) { return $true }
    }
    return $false
}

# Sammle alle DLLs relativ zu ThirdParty/
$dlls = Get-ChildItem -Path $thirdPartyRoot -Recurse -Filter '*.dll' -File
$deployed = 0
$skipped = 0
foreach ($dll in $dlls) {
    $rel = $dll.FullName.Substring($thirdPartyRoot.Length + 1).Replace('\', '/')
    if (Test-Ignored -RelativePath $rel -Patterns $ignorePatterns) {
        Write-Host "  skip  $rel" -ForegroundColor DarkGray
        $skipped++
        continue
    }
    # Force-deploy analog zu Directory.Build.targets (SkipUnchangedFiles=false)
    $destPath = Join-Path $modsDir $dll.Name
    if (Test-Path -LiteralPath $destPath) {
        Remove-Item -LiteralPath $destPath -Force
    }
    Copy-Item -LiteralPath $dll.FullName -Destination $destPath -Force
    Write-Host "  copy  $rel  ->  $destPath" -ForegroundColor Green
    $deployed++
}

Write-Host "[deploy-thirdparty] Done. Deployed: $deployed, Skipped: $skipped." -ForegroundColor Cyan

} finally {
    if ($mutex) {
        $mutex.ReleaseMutex() | Out-Null
        $mutex.Dispose()
    }
}
