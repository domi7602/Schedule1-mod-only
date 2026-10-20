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
      2. README.md                         -> Mod-Tabelle "| [**<Mod>**](...) | <Kategorie> | X.Y.Z | ..."
                                              (plus: jede weitere Zeile mit **<Mod>** oder <b><Mod></b>,
                                              z.B. Featured-Karten "<b><Mod></b> · vX.Y.Z", muss dieselbe
                                              Version tragen)
      3. AGENTS.md                         -> "| **<Mod>** ... (vX.Y.Z ..."
      4. AGENTS.md                         -> "**<Mod> vX.Y.Z (...):**" (Detail-Header, falls vorhanden)
      5. docs/compatibility.md             -> Verification matrix, Versionszelle
                                              ("Verified on"/"Date" werden bewusst nicht geprueft)
      6. Mod-Code selbst                   -> keine zweite Versionsquelle: keine Version-named
                                              Felder mit abweichendem Literal, keine Log-Strings,
                                              die eine alte Release-Version desselben Mods nennen

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
    # README-Tabellenzeile: "| [**<Mod>**](Source/Mods/<Mod>/) | <Kategorie> | X.Y.Z | ...".
    # Die erste SemVer-Zelle nach dem Namen ist die Versionszelle ("v"-Praefix optional).
    $readme = Get-FirstMatch -Path $readmePath `
                             -Pattern ('^\|\s*\[?\*\*' + $escaped + '\*\*[^\r\n|]*\|[^\r\n|]*\|\s*v?(' + $rxSemVer + ')\s*\|')
    # Zusaetzlich: alle anderen README-Zeilen, die den Mod fett nennen (Featured-Karten
    # "<b><Mod></b> · vX.Y.Z"), duerfen keine abweichende Version tragen.
    $readmeExtra = @()
    if (Test-Path -LiteralPath $readmePath) {
        $rxBold = [regex]('(\*\*|<b>)' + $escaped + '(\*\*|</b>)')
        # Die Mod-Tabellenzeile wird ueber ihre Versionszelle geprueft. Der Rest
        # der Zeile ist Prosa (auch historische Versionsnennungen) und darf
        # keinen zweiten Versionszwang erzeugen.
        $rxTableLine = '^\|\s*\[?\*\*' + $escaped + '\*\*[^\r\n|]*\|'
        $rxVer = [regex]('\bv?(' + $rxSemVer + ')\b')
        foreach ($line in [System.IO.File]::ReadAllLines($readmePath, [System.Text.Encoding]::UTF8)) {
            if (-not $rxBold.IsMatch($line)) { continue }
            if ($line -match $rxTableLine) { continue }
            foreach ($vm in $rxVer.Matches($line)) {
                # Explicit historical/prior annotations are documentation, not drift.
                $prefix = $line.Substring(0, $vm.Index)
                if ($prefix -match '(?i)(prior|historical[^;|]{0,40})\s*$') { continue }
                $readmeExtra += $vm.Groups[1].Value
            }
        }
    }
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
    $readmeExtraDrift = @($readmeExtra | Where-Object { $_ -ne $expected } | Sort-Object -Unique)
    $ok = ($null -ne $expected -and $json -eq $expected -and $readme -eq $expected -and
           $agents -eq $expected -and ($null -eq $agentsHeader -or $agentsHeader -eq $expected) -and
           $readmeExtraDrift.Count -eq 0)
    $row.Sync = $ok
    $rows += $row

    if (-not $ok) {
        if (-not $code) {
            $problems += "$name : keine Version im Code gefunden (weder MelonInfo noch ModVersion) - Mod gilt als Drift."
        }
        else {
            if ($json -ne $expected) { $problems += ("{0} : mod.json={1} != code={2}" -f $name, $(if ($json) { $json } else { 'fehlt' }), $expected) }
            if ($readme -ne $expected) { $problems += ("{0} : README.md={1} != code={2}" -f $name, $(if ($readme) { $readme } else { 'fehlt' }), $expected) }
            if ($readmeExtraDrift.Count -gt 0) { $problems += ("{0} : README.md (weitere Nennung)={1} != code={2}" -f $name, ($readmeExtraDrift -join ','), $expected) }
            if ($agents -ne $expected) { $problems += ("{0} : AGENTS.md={1} != code={2}" -f $name, $(if ($agents) { $agents } else { 'fehlt' }), $expected) }
            if ($agentsHeader -and $agentsHeader -ne $expected) { $problems += ("{0} : AGENTS.md detail header={1} != code={2}" -f $name, $agentsHeader, $expected) }
        }
    }
}

# ---------------------------------------------------------------------------
# 6. docs/compatibility.md -> Verification matrix.
#    Prueft wird nur die Versionszelle gegen den Code. "Verified on"/"Date" bleiben
#    bewusst ungeprueft: die Matrix dokumentiert, WELCHE Version WANN geprueft wurde,
#    und ein Pruefdatum darf aelter sein als der Release der aktuellen Version.
#    Eine Matrix-Zeile, die eine Version nennt, die es im Code nicht gibt, ist Drift —
#    bump-version.ps1 schreibt nur die aktuelle Zeile dieser Matrix mit (historische
#    Abschnitte und Verified-on/Date bleiben unangetastet).
# ---------------------------------------------------------------------------
$compatPath = Join-Path $workspaceRoot 'docs/compatibility.md'
if (-not (Test-Path -LiteralPath $compatPath)) {
    $warnings += 'docs/compatibility.md fehlt -> Matrix-Check uebersprungen.'
} else {
    $compatText = [System.IO.File]::ReadAllText($compatPath, [System.Text.Encoding]::UTF8)
    $section = [regex]::Match($compatText, '(?m)^## Verification matrix\r?\n(.*?)(?=^##\s|\z)', 'Singleline')
    if (-not $section.Success) {
        $warnings += 'docs/compatibility.md : Abschnitt "## Verification matrix" nicht gefunden -> Matrix-Check uebersprungen.'
    } else {
        $matrixByMod = @{}
        foreach ($line in ($section.Groups[1].Value -split "\r?\n")) {
            $m = [regex]::Match($line, '^\|\s*([A-Za-z_][\w]*)\s*\|\s*v?(' + $rxSemVer + ')\s*\|')
            if ($m.Success) {
                $rowMod = $m.Groups[1].Value
                if (-not $matrixByMod.ContainsKey($rowMod)) { $matrixByMod[$rowMod] = @() }
                $matrixByMod[$rowMod] = @($matrixByMod[$rowMod]) + $m.Groups[2].Value
            }
        }
        foreach ($mod in $mods) {
            $name = $mod.Name
            $code = Get-CodeVersion -ModDir $mod.FullName
            if (-not $matrixByMod.ContainsKey($name)) {
                $problems += "$name : keine Zeile in der Verification matrix von docs/compatibility.md."
                continue
            }
            if (@($matrixByMod[$name]).Count -gt 1) {
                $problems += ("{0} : {1} Zeilen in der Verification matrix von docs/compatibility.md (erwartet genau 1, mehrdeutig)." -f $name, @($matrixByMod[$name]).Count)
                continue
            }
            if ($code -and $matrixByMod[$name][0] -ne $code.Version) {
                $problems += ("{0} : compatibility.md matrix={1} != code={2}" -f $name, $matrixByMod[$name][0], $code.Version)
            }
        }
    }
}

# ---------------------------------------------------------------------------
# 7. Versions-Quellen im Mod-Code selbst.
#    bump-version.ps1 schreibt nur MelonInfo bzw. Constants.ModVersion. Jede andere
#    Version im Code ist eine zweite Quelle und driftet still: PocketShop meldete
#    "initialized (v0.3.3)" und "PhoneApp system (v0.3.2)", waehrend MelonInfo langst
#    0.3.6 sagte — drei Versionsangaben fuer einen Mod in einem Log. Geprueft wird:
#      a) ein Feld/const mit "Version" im Namen, dessen Literal nicht der Code-Version
#         entspricht (PotScanner: ModVersion IST die Code-Version -> kein Befund),
#      b) jeder String-Literal ausserhalb von Kommentaren, der eine Version nennt, die
#         dieser Mod laut CHANGELOG schon released hat, aber nicht die aktuelle ist.
#    Kommentare sind ausdruecklich erlaubt: "v0.3.1 fuehrte X ein" ist Dokumentation.
# ---------------------------------------------------------------------------
$rxBlockComment = [regex]'/\*[\s\S]*?\*/'
$rxLineComment  = [regex]'//[^\r\n]*'
$rxVersionField = [regex]('\b(?:const|static\s+readonly|string)\s+(\w*[Vv]ersion\w*)\s*=\s*"(' + $rxSemVer + ')"')
$rxQuotedString = [regex]'"[^"\r\n]*"'

foreach ($mod in $mods) {
    $name = $mod.Name
    $code = Get-CodeVersion -ModDir $mod.FullName
    if (-not $code) { continue }   # bereits als Drift gemeldet
    $expected = $code.Version

    $stale = @()
    $changelogPath = Join-Path $mod.FullName 'docs/CHANGELOG.md'
    if (Test-Path -LiteralPath $changelogPath) {
        $clText = [System.IO.File]::ReadAllText($changelogPath, [System.Text.Encoding]::UTF8)
        $stale = @([regex]::Matches($clText, '(?m)^##\s+(' + $rxSemVer + ')') |
                   ForEach-Object { $_.Groups[1].Value } |
                   Where-Object { $_ -ne $expected } | Sort-Object -Unique)
    }

    $srcDir = Join-Path $mod.FullName 'src'
    foreach ($f in (Get-ChildItem -LiteralPath $srcDir -Filter '*.cs' -Recurse -File -ErrorAction SilentlyContinue |
                    Where-Object { $_.FullName -notmatch '[\\/](obj|bin)[\\/]' })) {
        $raw = [System.IO.File]::ReadAllText($f.FullName, [System.Text.Encoding]::UTF8)
        $codeOnly = $rxLineComment.Replace($rxBlockComment.Replace($raw, ''), '')
        $rel = $f.FullName.Substring($mod.FullName.Length).TrimStart('\', '/')

        foreach ($fm in $rxVersionField.Matches($codeOnly)) {
            if ($fm.Groups[2].Value -ne $expected) {
                $problems += ("{0} : {1} haelt {2}={3} != code={4} (zweite Versionsquelle im Code)" -f `
                              $name, $rel, $fm.Groups[1].Value, $fm.Groups[2].Value, $expected)
            }
        }

        if ($stale.Count -eq 0) { continue }
        foreach ($qm in $rxQuotedString.Matches($codeOnly)) {
            foreach ($vm in [regex]::Matches($qm.Value, $rxSemVer)) {
                if ($stale -contains $vm.Value) {
                    $problems += ("{0} : {1} nennt v{2} (alt; Code-Version ist {3}) in: {4}" -f `
                                  $name, $rel, $vm.Value, $expected, $qm.Value)
                }
            }
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
Write-Host 'Fix (schreibt MelonInfo + mod.json + CHANGELOG + AGENTS.md + README.md + compatibility.md-Matrixzeile):' -ForegroundColor DarkGray
Write-Host '  pwsh Tools/bump-version.ps1 -Mod <Name> -Version <Code-Version>' -ForegroundColor DarkGray
Write-Host 'Nach dem Fix pruefen: pwsh Tools/check-version-sync.ps1' -ForegroundColor DarkGray
exit 1