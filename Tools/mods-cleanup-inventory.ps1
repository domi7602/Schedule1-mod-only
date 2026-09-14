<#
.SYNOPSIS
    Inventur + sicherer Move von Runtime-DLLs aus <GameDir>\Mods\ nach MelonLoader\_archived\.

.DESCRIPTION
    MelonLoader scannt zwei Pfade parallel:
      - <GameDir>\Mods\            -> Mod-DLLs (eigene Mods + S1API)
      - <GameDir>\MelonLoader\net6\         -> Runtime (MelonLoader, MonoMod, 0Harmony, Mono.Cecil, Tomlet, Il2CppInterop.*)
      - <GameDir>\MelonLoader\Il2CppAssemblies\ -> Il2Cpp Interop Proxies (alle Il2Cpp*.dll + UnityEngine.*.dll)

    Aktuell liegen ~40 DLLs in Mods\, die bit-identische Kopien der Runtime sind.
    Sie verschmutzen die Mod-Liste, schlucken Ladezeit beim Hash-Scan und sind
    bei Updates eine Fehlerquelle.

    Dieses Skript verschiebt sie NICHT direkt. Standardmaessig nur -WhatIf.
    Erst mit -Apply werden sie tatsaechlich nach MelonLoader\_archived\runtime-dups\
    verschoben (Move, nicht Delete - reversibel).

.PARAMETER GameDir
    Pfad zum Spiel-Install. Default: C:\Program Files (x86)\Steam\steamapps\common\Schedule I

.PARAMETER Apply
    Ohne diesen Switch: nur Inventur ausgeben (Tabelle + Counts). NICHTS wird bewegt.

.EXAMPLE
    pwsh Tools/mods-cleanup-inventory.ps1
    # nur Inventur

.EXAMPLE
    pwsh Tools/mods-cleanup-inventory.ps1 -Apply
    # Move ausfuehren (Erst nach Sichtung der Inventur!)
#>

[CmdletBinding(SupportsShouldProcess = $true)]
param(
    [string]$GameDir = 'C:\Program Files (x86)\Steam\steamapps\common\Schedule I',
    [switch]$Apply
)

$ErrorActionPreference = 'Stop'
$modsDir = Join-Path $GameDir 'Mods'
$net6Dir = Join-Path $GameDir 'MelonLoader\net6'
$il2cppDir = Join-Path $GameDir 'MelonLoader\Il2CppAssemblies'
$archiveDir = Join-Path $GameDir 'MelonLoader\_archived\runtime-dups'

# --- Whitelist: was BLEIBT in Mods/ -----------------------------------------
# 1) Mod-DLLs (haben MelonInfo / MelonGame Attribut - Echte Mods)
# 2) PNG-Icons (gehoeren zur Mod-UI)
# 3) S1API Runtime
# 4) Bereits archivierte BackpackMod-Artefakte (in _archived/)
# Erkennung: alles, was KEIN bekanntes Runtime-Prefix hat, bleibt.
$runtimePrefixes = @(
    'Il2Cpp',                  # Il2Cpp Interop Proxies
    'MelonLoader',             # Runtime Bootstrap
    'Mono.Cecil',              # Runtime
    'MonoMod',                 # Runtime (Harmony-Alternative)
    'Tomlet',                  # Runtime (Toml Parser, von MelonLoader benutzt)
    'WebSocketDotNet',         # Runtime (von S1API benutzt, aber NICHT in net6/)
    'UnityEngine.',            # Unity-Engine-Module (gehoeren nach Il2CppAssemblies/)
    'Unity.',                  # Unity Packages (InputSystem, Mathematics, TextMeshPro, ...)
    '0Harmony'                 # Harmony - falls vorhanden (sollte in net6/ sein)
)

function Test-IsRuntimeDup {
    param([string]$Name)
    foreach ($p in $runtimePrefixes) {
        if ($Name.StartsWith($p, [System.StringComparison]::OrdinalIgnoreCase)) { return $true }
    }
    return $false
}

# --- Sammeln -----------------------------------------------------------------
if (-not (Test-Path $modsDir)) { throw "Mods-Verzeichnis nicht gefunden: $modsDir" }

$allItems = Get-ChildItem -Path $modsDir -Force | Where-Object {
    -not $_.PSIsContainer -or $_.Name -ne '_archived'
}

$inventory = foreach ($item in $allItems) {
    $isRuntimeDup = $item.Extension -eq '.dll' -and (Test-IsRuntimeDup $item.Name)

    # Pruefen, ob Original existiert (in net6/ oder Il2CppAssemblies/)
    $originalPath = $null
    $hashMods = $null
    $hashOriginal = $null
    if ($isRuntimeDup) {
        $candidates = @(
            (Join-Path $net6Dir $item.Name),
            (Join-Path $il2cppDir $item.Name)
        )
        foreach ($cand in $candidates) {
            if (Test-Path $cand) { $originalPath = $cand; break }
        }
        if ($originalPath) {
            $hashMods = (Get-FileHash $item.FullName -Algorithm SHA256).Hash
            $hashOriginal = (Get-FileHash $originalPath -Algorithm SHA256).Hash
        }
    }

    [pscustomobject]@{
        Name          = $item.Name
        Ext           = $item.Extension
        IsRuntimeDup  = $isRuntimeDup
        OriginalPath  = $originalPath
        HashIdentical = ($hashMods -and $hashOriginal -and $hashMods -eq $hashOriginal)
        SizeBytes     = $item.Length
    }
}

# --- Ausgabe -----------------------------------------------------------------
Write-Host ''
Write-Host "=== Inventur: $($inventory.Count) Eintraege in <GameDir>\Mods\ ===" -ForegroundColor Cyan
Write-Host ''

$runtimeDups = $inventory | Where-Object { $_.IsRuntimeDup }
$mods = $inventory | Where-Object { -not $_.IsRuntimeDup }

Write-Host ("Mods / Assets (BLEIBEN): {0}" -f $mods.Count) -ForegroundColor Green
$mods | Sort-Object Name | ForEach-Object { Write-Host "  $($_.Name)" }

Write-Host ''
Write-Host ("Runtime-Dopplungen (GEHEN nach _archived\runtime-dups\): {0}" -f $runtimeDups.Count) -ForegroundColor Yellow
$runtimeDups | Sort-Object Name | Format-Table Name, OriginalPath, HashIdentical, SizeBytes -AutoSize | Out-String | Write-Host

$identicalCount = ($runtimeDups | Where-Object { $_.HashIdentical }).Count
$mismatchCount = ($runtimeDups | Where-Object { -not $_.HashIdentical }).Count
$totalBytes = ($runtimeDups | Measure-Object -Property SizeBytes -Sum).Sum

Write-Host ''
Write-Host ("Identische Kopie (sicher zu moven): {0}" -f $identicalCount) -ForegroundColor $(if ($identicalCount -gt 0) { 'Green' } else { 'Gray' })
Write-Host ("Abweichend vom Original (!): {0}" -f $mismatchCount) -ForegroundColor $(if ($mismatchCount -gt 0) { 'Red' } else { 'Gray' })
Write-Host ("Gesamtvolumen: {0:N1} MB" -f ($totalBytes / 1MB))

if (-not $Apply) {
    Write-Host ''
    Write-Host "=== NUR INVENTUR (kein -Apply). Nichts wurde bewegt. ===" -ForegroundColor Cyan
    Write-Host "Erneut aufrufen mit -Apply um den Move auszufuehren." -ForegroundColor Cyan
    return
}

# --- Apply: tatsaechlich moven ----------------------------------------------
Write-Host ''
Write-Host "=== APPLY-Modus: verschiebe Runtime-Dopplungen nach $archiveDir ===" -ForegroundColor Cyan

if (-not (Test-Path $archiveDir)) {
    New-Item -ItemType Directory -Path $archiveDir -Force | Out-Null
}

$moved = 0
$skipped = 0
$failed = 0
foreach ($entry in $runtimeDups) {
    $src = Join-Path $modsDir $entry.Name
    $dst = Join-Path $archiveDir $entry.Name

    if (-not $entry.HashIdentical) {
        Write-Host "  [SKIP-MISMATCH] $($entry.Name) - weicht vom Original ab, wird NICHT gemoved." -ForegroundColor Red
        $skipped++
        continue
    }
    if ($entry.OriginalPath -notlike "*$($entry.Name)") {
        Write-Host "  [SKIP-NO-ORIGINAL] $($entry.Name) - kein Original in net6/ oder Il2CppAssemblies/ gefunden." -ForegroundColor Red
        $skipped++
        continue
    }

    try {
        Move-Item -LiteralPath $src -Destination $dst -Force
        Write-Host "  [MOVED] $($entry.Name)" -ForegroundColor Green
        $moved++
    } catch {
        Write-Host "  [FAIL] $($entry.Name): $($_.Exception.Message)" -ForegroundColor Red
        $failed++
    }
}

Write-Host ''
Write-Host "=== Fertig ===" -ForegroundColor Cyan
Write-Host "  Verschoben: $moved"
Write-Host "  Uebersprungen: $skipped"
Write-Host "  Fehler: $failed"
Write-Host "  Archiv: $archiveDir"
