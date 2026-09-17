#Requires -Version 7
<#
.SYNOPSIS
    Prueft, ob die Mod-Version ueberall synchron ist (Drift-Guard fuer CI + pre-commit).

.DESCRIPTION
    Single Source of Truth der Version ist der CODE:
      - [assembly: MelonInfo(typeof(X), "Name", "1.2.3", "Autor")]  (Dateiname frei:
        Mod.cs, MinimapMod.cs, MoreSaveSlotsMod.cs, ...), sonst
      - Constants.ModVersion = "1.2.3"  (PotScanner-Muster).

    Dagegen geprueft werden drei Doku-Orte, die bump-version.ps1 mitschreibt:
      1. Source/Mods/<Mod>/docs/mod.json   -> "version"
      2. README.md                         -> "* **<Mod>** (vX.Y.Z): ..."
      3. AGENTS.md                         -> "| **<Mod>** ... (vX.Y.Z ..."
      4. AGENTS.md                         -> "**<Mod> vX.Y.Z (...):**" (Detail-Header, falls vorhanden)

    Fehlt eine Mod in README.md oder AGENTS.md, gilt das ebenfalls als Drift.
    Exit 1 bei Drift (CI/pre-commit-tauglich), 0 wenn alles synchron ist.

.EXAMPLE
    pwsh Tools/check-version-sync.ps1
    pwsh Tools/check-version-sync.ps1 -Quiet   # nur Drift-Zeilen ausgeben
#>
[CmdletBinding()]
param(
    [switch]$Quiet
)

$ErrorActionPreference = 'Stop'

$workspaceRoot = Split-Path $PSScriptRoot -Parent
$modsRoot = Join-Path $workspaceRoot 'Source/Mods'
$agentsPath = Join-Path $workspaceRoot 'AGENTS.md'
$readmePath = Join-Path $workspaceRoot 'README.md'

# SemVer-Kern + optionale Pre-Release-Suffixe (0.0.2-mvp, 0.1.0-beta.1)
$rxSemVer = '\d+\.\d+\.\d+(?:-[\w\.]+)?'

function Get-CodeVersion {
    param([Parameter(Mandatory)][string]$ModDir)

    $srcDir = Join-Path $ModDir 'src'
    if (-not (Test-Path -LiteralPath $srcDir)) { return $null }

    $files = Get-ChildItem -LiteralPath $srcDir -Filter '*.cs' -Recurse -File -ErrorAction SilentlyContinue
    if (-not $files) { return $null }

    # 1. MelonInfo-Attribut (Dateiname ist frei). Gesucht wird die Zeile mit
    #    "MelonInfo("; genommen wird das erste String-Literal, das wie SemVer
    #    aussieht. Das ist robust gegen beliebige Type-Ausdruecke
    #    (typeof(X.Y), typeof(X), ...) — ein Regex ueber die schliessende
    #    Klammer von typeof(...) hinweg waere fragil.
    $rxMelonCall = [regex]'MelonInfo\s*\(([^\r\n]*)\)'
    $rxQuoted = [regex]'"([^"]*)"'
    $rxExact = [regex]('^' + $rxSemVer + '$')
    foreach ($f in $files) {
        foreach ($line in [System.IO.File]::ReadAllLines($f.FullName, [System.Text.Encoding]::UTF8)) {
            if ($line -notmatch 'MelonInfo\s*\(') { continue }
            $call = $rxMelonCall.Match($line)
            if (-not $call.Success) { continue }
            foreach ($q in $rxQuoted.Matches($call.Groups[1].Value)) {
                if ($rxExact.IsMatch($q.Groups[1].Value)) {
                    return [pscustomobject]@{ Version = $q.Groups[1].Value; Source = $f.Name }
                }
            }
        }
    }

    # 2. Fallback: Constants.ModVersion (PotScanner)
    $rxConst = [regex]('ModVersion\s*=\s*"(' + $rxSemVer + ')"')
    foreach ($f in $files) {
        $text = [System.IO.File]::ReadAllText($f.FullName, [System.Text.Encoding]::UTF8)
        $m = $rxConst.Match($text)
        if ($m.Success) { return [pscustomobject]@{ Version = $m.Groups[1].Value; Source = "$($f.Name) (ModVersion)" } }
    }

    return $null
}

function Get-FirstMatch {
    param([string]$Path, [string]$Pattern)

    if (-not (Test-Path -LiteralPath $Path)) { return $null }
    $text = [System.IO.File]::ReadAllText($Path, [System.Text.Encoding]::UTF8)
    $m = [regex]::Match($text, $Pattern, [System.Text.RegularExpressions.RegexOptions]::Multiline)
    if ($m.Success) { return $m.Groups[1].Value }
    return $null
}

$mods = Get-ChildItem -LiteralPath $modsRoot -Directory | Where-Object {
    # Shared: Library ohne eigenes Release-ZIP. _DiagPerfCounter: Dev-Tool,
    # seit 2026-09-16 von package-release.ps1 ausgeschlossen (kein README nötig).
    $_.Name -ne 'Shared' -and $_.Name -ne '_DiagPerfCounter' -and (Test-Path -LiteralPath (Join-Path $_.FullName 'src'))
} | Sort-Object Name

$rows = @()
$problems = @()
$warnings = @()

foreach ($mod in $mods) {
    $name = $mod.Name
    $escaped = [regex]::Escape($name)

    $code = Get-CodeVersion -ModDir $mod.FullName
    if (-not (Test-Path -LiteralPath (Join-Path $mod.FullName 'docs/CHANGELOG.md'))) {
        $problems += "$name : docs/CHANGELOG.md fehlt (bump-version.ps1 schreibt dort den Versions-Header)."
    }
    if (-not (Test-Path -LiteralPath (Join-Path $mod.FullName 'docs/README.md'))) {
        $warnings += "$name : docs/README.md fehlt -> Tools/package-release.ps1 legt dem Release-ZIP kein README bei."
    }
    $json = Get-FirstMatch -Path (Join-Path $mod.FullName 'docs/mod.json') `
                           -Pattern ('"version"\s*:\s*"(' + $rxSemVer + ')"')
    $readme = Get-FirstMatch -Path $readmePath `
                             -Pattern ('^\*\s+\*\*' + $escaped + '\*\*[^\r\n]*?\(v(' + $rxSemVer + ')\)')
    $agents = Get-FirstMatch -Path $agentsPath `
                             -Pattern ('^\|\s*\*\*' + $escaped + '\*\*[^\r\n]*?\(v(' + $rxSemVer + ')')
    # Detail-Header ("**<Mod> vX.Y.Z (datum):**") ist optional (nicht jede Mod hat eine Sektion).
    $agentsHeader = Get-FirstMatch -Path $agentsPath `
                                   -Pattern ('^\*\*' + $escaped + ' v(' + $rxSemVer + ')')

    $codeV = if ($code) { $code.Version } else { '(n/a)' }
    $row = [pscustomobject]@{
        Mod    = $name
        Code   = $codeV
        Json   = $(if ($json) { $json } else { '(fehlt)' })
        Readme = $(if ($readme) { $readme } else { '(fehlt)' })
        Agents = $(if ($agents) { $agents } else { '(fehlt)' })
        Hdr    = $(if ($agentsHeader) { $agentsHeader } else { '-' })
        Sync   = $false
    }

    $expected = if ($code) { $code.Version } else { $null }
    $ok = ($null -ne $expected -and $json -eq $expected -and $readme -eq $expected -and
           $agents -eq $expected -and ($null -eq $agentsHeader -or $agentsHeader -eq $expected))
    $row.Sync = $ok
    $rows += $row

    if (-not $ok) {
        if (-not $code) {
            $problems += "$name : keine Version im Code gefunden (weder MelonInfo noch ModVersion) - Mod gilt als Drift."
        }
        else {
            if ($json -ne $expected) { $problems += ("{0} : mod.json={1} != code={2}" -f $name, $(if ($json) { $json } else { 'fehlt' }), $expected) }
            if ($readme -ne $expected) { $problems += ("{0} : README.md={1} != code={2}" -f $name, $(if ($readme) { $readme } else { 'fehlt' }), $expected) }
            if ($agents -ne $expected) { $problems += ("{0} : AGENTS.md={1} != code={2}" -f $name, $(if ($agents) { $agents } else { 'fehlt' }), $expected) }
            if ($agentsHeader -and $agentsHeader -ne $expected) { $problems += ("{0} : AGENTS.md detail header={1} != code={2}" -f $name, $agentsHeader, $expected) }
        }
    }
}

if (-not $Quiet) {
    Write-Host '=== Version-Sync (Code ist Single Source of Truth) ===' -ForegroundColor Cyan
    $rows | Format-Table -AutoSize | Out-String -Width 200 | Write-Host
}

if ($warnings.Count -gt 0) {
    Write-Host ("Hinweise ({0}):" -f $warnings.Count) -ForegroundColor DarkYellow
    foreach ($w in $warnings) { Write-Host "  - $w" -ForegroundColor DarkYellow }
    Write-Host ''
}

if ($problems.Count -eq 0) {
    Write-Host ("Version-Sync OK: {0} Mods, 0 Drift." -f $rows.Count) -ForegroundColor Green
    exit 0
}

Write-Host ("Version-Drift in {0} Mod(s):" -f @($problems | ForEach-Object { $_.Split(' ')[0] } | Sort-Object -Unique).Count) -ForegroundColor Red
foreach ($p in $problems) { Write-Host "  - $p" -ForegroundColor Yellow }
Write-Host ''
Write-Host 'Fix (schreibt MelonInfo + mod.json + CHANGELOG + AGENTS.md + README.md):' -ForegroundColor DarkGray
Write-Host '  pwsh Tools/bump-version.ps1 -Mod <Name> -Version <Code-Version>' -ForegroundColor DarkGray
Write-Host 'Nach dem Fix pruefen: pwsh Tools/check-version-sync.ps1' -ForegroundColor DarkGray
exit 1