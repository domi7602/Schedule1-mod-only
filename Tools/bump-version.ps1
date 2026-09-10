#Requires -Version 7
<#
.SYNOPSIS
    Bumps version across the 4 source-of-truth files for a mod.

.DESCRIPTION
    Synchronisiert die Version über:
      1. Source/Mods/<Mod>/src/*.cs (Datei mit MelonInfo-Attribut: Mod.cs,
         MoreSaveSlotsMod.cs, MinimapMod.cs, ...; PotScanner nutzt
         Constants.ModVersion und wird bewusst nur gewarnt, nicht geraten)
      2. Source/Mods/<Mod>/docs/mod.json -> "version": "x.y.z"
      3. Source/Mods/<Mod>/docs/CHANGELOG.md -> prepend "## x.y.z - YYYY-MM-DD"
      4. AGENTS.md -> Mod-Matrix Zeile

.EXAMPLE
    pwsh Tools/bump-version.ps1 -Mod NotesApp -Version 1.0.1
    pwsh Tools/bump-version.ps1 -Mod All -Version 1.0.1 -DryRun
    pwsh Tools/bump-version.ps1 -Mod StackLimitMod -Version 0.2.0 -DryRun
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory)][string]$Mod,
    [Parameter(Mandatory)][string]$Version,
    [switch]$DryRun
)

$ErrorActionPreference = "Stop"

if ($Version -notmatch '^\d+\.\d+\.\d+(-[\w\.]+)?$') {
    Write-Error "Version '$Version' ist kein SemVer (erwartet z.B. 1.0.2 oder 0.1.0-beta.1)."
    exit 1
}

$workspaceRoot = Split-Path $PSScriptRoot -Parent
$modsRoot = Join-Path $workspaceRoot "Source/Mods"
$agentsPath = Join-Path $workspaceRoot "AGENTS.md"
$today = Get-Date -Format "yyyy-MM-dd"
$utf8NoBom = New-Object System.Text.UTF8Encoding($false)

function Update-ModVersion {
    param([string]$ModName, [string]$NewVersion, [bool]$IsDryRun)

    $modDir = Join-Path $modsRoot $ModName
    if (-not (Test-Path -LiteralPath $modDir)) {
        Write-Warning "Mod '$ModName' nicht gefunden: $modDir — skip."
        return $false
    }

    # 2026-09-10: MelonInfo per Inhalt suchen statt per Dateiname "Mod.cs"
    # (MoreSaveSlotsMod.cs, MinimapMod.cs; PotScanner referenziert nur
    # Constants.ModVersion und faellt bewusst in die Warnung unten).
    $modCsPath = $null
    $melonFinder = '\[assembly:\s*MelonInfo\('
    $candidate = Get-ChildItem -LiteralPath (Join-Path $modDir "src") -Filter "*.cs" -Recurse -ErrorAction SilentlyContinue |
        Where-Object { (Get-Content -LiteralPath $_.FullName -Raw -ErrorAction SilentlyContinue) -match $melonFinder } |
        Select-Object -First 1
    if ($candidate) { $modCsPath = $candidate.FullName }

    $modJsonPath = Join-Path $modDir "docs/mod.json"
    $changelogPath = Join-Path $modDir "docs/CHANGELOG.md"

    $changed = @()

    # 1. Mod.cs MelonInfo
    if ($modCsPath -and (Test-Path -LiteralPath $modCsPath)) {
        $content = [System.IO.File]::ReadAllText($modCsPath, [System.Text.Encoding]::UTF8)
        $pattern = '(\[assembly:\s*MelonInfo\(.*?"[^"]*"\s*,\s*")[^"]+("\s*,)'
        if ($content -match $pattern) {
            $newContent = [regex]::Replace($content, $pattern, "`${1}$NewVersion`${2}")
            # also handle "Initialized (vX.Y.Z)" log lines (MelonLogger.Msg / Log.Info)
            $newContent = [regex]::Replace($newContent, '(\.Msg\("Initialized \(v)[^\)]+(\)."\))', "`${1}$NewVersion`${2}")
            $newContent = [regex]::Replace($newContent, '(\.Info\("Initialized \(v)[^\)]+(\)."\))', "`${1}$NewVersion`${2}")
            $newContent = [regex]::Replace($newContent, '(\.Msg\("\[.+?\] initialisiert \(v)[^\)]+(\)."\))', "`${1}$NewVersion`${2}")
            if ($newContent -ne $content) {
                $changed += "Mod.cs MelonInfo -> $NewVersion"
                if (-not $IsDryRun) { [System.IO.File]::WriteAllText($modCsPath, $newContent, $utf8NoBom) }
            }
        } else {
            Write-Warning "  [$ModName] MelonInfo-Pattern nicht gefunden in $modCsPath"
        }
    } else {
        Write-Warning "  [$ModName] Mod.cs nicht gefunden — skip Mod.cs"
    }

    # 2. docs/mod.json
    if (Test-Path -LiteralPath $modJsonPath) {
        $jsonRaw = [System.IO.File]::ReadAllText($modJsonPath, [System.Text.Encoding]::UTF8)
        try {
            $json = $jsonRaw | ConvertFrom-Json
            $oldVer = $json.version
            if ($oldVer -ne $NewVersion) {
                $newJsonRaw = [regex]::Replace($jsonRaw, '"version"\s*:\s*"[^"]+"', "`"version`": `"$NewVersion`"")
                $changed += "mod.json $oldVer -> $NewVersion"
                if (-not $IsDryRun) { [System.IO.File]::WriteAllText($modJsonPath, $newJsonRaw, $utf8NoBom) }
            }
        } catch {
            Write-Warning "  [$ModName] mod.json JSON-Parse fehlgeschlagen: $_"
        }
    } else {
        Write-Warning "  [$ModName] docs/mod.json nicht gefunden"
    }

    # 3. docs/CHANGELOG.md
    if (Test-Path -LiteralPath $changelogPath) {
        $clRaw = [System.IO.File]::ReadAllText($changelogPath, [System.Text.Encoding]::UTF8)
        if ($clRaw -notmatch "##\s+$([regex]::Escape($NewVersion))\b") {
            $header = "## $NewVersion ($today)`n- Version bump.`n`n"
            # Insert after first "# Changelog" header (Instanz-Replace mit Count=1,
            # siehe AGENTS.md-Fix oben — statisches Replace mit ", 1" waere IgnoreCase).
            if ($clRaw -match "(?m)^# Changelog\s*\r?\n") {
                $clRx = [regex]"(?m)(^# Changelog\s*\r?\n)"
                $newCl = $clRx.Replace($clRaw, "`$1`n$header", 1)
            } else {
                $newCl = "# Changelog`n`n$header`n$clRaw"
            }
            $changed += "CHANGELOG.md prepend ## $NewVersion"
            if (-not $IsDryRun) { [System.IO.File]::WriteAllText($changelogPath, $newCl, $utf8NoBom) }
        }
    } else {
        Write-Warning "  [$ModName] docs/CHANGELOG.md nicht gefunden"
    }

    # 4. AGENTS.md matrix row (version inside "| **Mod** | ✅ active (vX.Y.Z, ...")
    if (Test-Path -LiteralPath $agentsPath) {
        $agentsRaw = [System.IO.File]::ReadAllText($agentsPath, [System.Text.Encoding]::UTF8)
        $escapedMod = [regex]::Escape($ModName)
        $lines = $agentsRaw -split "`r?`n"
        $newLines = @()
        $agentsChanged = $false
        foreach ($line in $lines) {
            if ($line -match "\*\*$escapedMod\*\*") {
                # 2026-09-10 FIX: [regex]::Replace(.., .., .., 1) bindet die 1 an
                # RegexOptions (IgnoreCase), NICHT an einen Count — es ersetzte ALLE
                # Treffer (u.a. die Game-Version v0.4.6f13 -> vX.Y.Zf13). Instanz-Methode
                # mit Count=1 ersetzt nur die erste (die Mod-Version).
                $rx = [regex]"v\d+\.\d+\.\d+(-[\w\.]+)?"
                $newLine = $rx.Replace($line, "v$NewVersion", 1)
                if ($newLine -ne $line) { $agentsChanged = $true; $changed += "AGENTS.md matrix -> v$NewVersion" }
                $newLines += $newLine
            } else {
                $newLines += $line
            }
        }
        if ($agentsChanged -and -not $IsDryRun) {
            $newAgents = $newLines -join "`r`n"
            [System.IO.File]::WriteAllText($agentsPath, $newAgents, $utf8NoBom)
        }
    }

    if ($changed.Count -gt 0) {
        $prefix = $IsDryRun ? "[DryRun] [$ModName] würde ändern:" : "[OK] [$ModName] geändert:"
        Write-Host $prefix -ForegroundColor $(if ($IsDryRun) { "Yellow" } else { "Green" })
        foreach ($c in $changed) { Write-Host "  - $c" -ForegroundColor Gray }
        return $true
    } else {
        Write-Host "[Skip] [$ModName] bereits auf $NewVersion oder keine Treffer." -ForegroundColor DarkGray
        return $false
    }
}

# Resolve target mods
$targets = @()
if ($Mod -eq "All") {
    $targets = Get-ChildItem -LiteralPath $modsRoot -Directory | Where-Object {
        $_.Name -ne "Shared" -and $_.Name -ne "Archive" -and (Test-Path (Join-Path $_.FullName "src"))
    } | ForEach-Object { $_.Name }
    if ($targets.Count -eq 0) { Write-Error "Keine Mods gefunden in $modsRoot"; exit 1 }
} else {
    $targets = @($Mod)
}

Write-Host "=== bump-version v$Version -> $($targets -join ', ') $(if($DryRun){'(DryRun)'}) ===" -ForegroundColor Cyan

$totalChanged = 0
foreach ($t in $targets) {
    $did = Update-ModVersion -ModName $t -NewVersion $Version -IsDryRun $DryRun.IsPresent
    if ($did) { $totalChanged++ }
}

Write-Host ""
if ($DryRun) {
    Write-Host "DryRun abgeschlossen: $totalChanged Mod(s) würden geändert. Ohne -DryRun erneut ausführen." -ForegroundColor Magenta
} else {
    Write-Host "Fertig: $totalChanged Mod(s) aktualisiert auf v$Version." -ForegroundColor Green
    Write-Host "Hinweis: Danach 'pwsh Tools/gen-sln.ps1' nur wenn Mod hinzugefügt/entfernt wurde; Version-Bumps brauchen keinen SLN-Rebuild." -ForegroundColor DarkGray
}
