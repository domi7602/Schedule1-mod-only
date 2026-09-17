#Requires -Version 7
<#
.SYNOPSIS
    Pfad-Existenz-Guard: Prueft, dass alle in Markdown referenzierten
    Repo-relativen Pfade tatsaechlich existieren (Drift-Guard fuer Doku,
    CI- + pre-commit-tauglich).

.DESCRIPTION
    Motivation (Repo-Audit 2026-09-16): AGENTS.md & Co. referenzierten
    Pfade, die nicht mehr existieren (Source/Archive/DayCounter, Knowledge/,
    .agents/...). check-version-sync.ps1 bewacht nur Versionsnummern —
    dieses Skript bewacht PFADE.

    Gescannt werden: Root-*.md, docs/**, Skills/**, ThirdParty/README.md,
    GameReferences/README.md. Extrahiert werden Inline-Code-Spans
    (`pfad/zu/x`) und Markdown-Links (...](pfad/zu/x), inclusive Bilder).
    Fenced Code-Blocks werden ignoriert.

    Ein Kandidat gilt als existent, wenn er
      a) relativ zum Repo-Root, oder
      b) relativ zur Markdown-Datei
    existiert. Pfade, die nur lokal existieren, weil sie gitignored sind
    (z.B. ThirdParty/S1API/local.build.props), zaehlen als OK mit Hinweis
    (auf einem frischen Clone/CI existieren sie nicht).

    Gefiltert (bewusst NICHT geprueft): URLs, absolute Pfade (C:\...),
    Platzhalter (<Mod>, {n}, *), game-seitige Root-Ordner (Mods\, UserData\
    — liegen ausserhalb des Worktrees), tokens ohne Slash, Pfade mit ":".

.EXAMPLE
    pwsh Tools/check-doc-paths.ps1
    pwsh Tools/check-doc-paths.ps1 -Quiet
#>
[CmdletBinding()]
param(
    [switch]$Quiet,
    # Regex-Whitelist fuer bewusst dokumentierte, nicht existierende Pfade.
    [string[]]$Allow = @()
)

$ErrorActionPreference = 'Stop'

$workspaceRoot = Split-Path $PSScriptRoot -Parent

# Submodule: Inhalt existiert nur nach `git submodule update --init` — auf
# frischen Clones/CI ist der Pfad ein leerer Verzeichnisstummel. Referenzen
# in Submodule hinein sind daher hier nicht verlaesslich pruefbar.
$defaultAllow = @('^ThirdParty/S1API/', '^ThirdParty/S1MAPI/')
$Allow = $defaultAllow + $Allow

# Historische Wurzeln: Layouts, die es im aktuellen Repo (bewusst) nicht
# mehr gibt. Pfade unter diesen Segmenten sind per Definition tot —
# genau die Drift-Klasse, die dieser Guard fangen soll.
$historicRoots = @('Knowledge', 'memory', 'Archive', 'Docs', '.agents')

# Dynamisch: aktuelle Top-Level-Eintraege des Repos. Ein Kandidat gilt nur
# als repo-relativer Pfad, wenn sein erstes Segment hier auftaucht — das
# haelt Prosa/API-Bezeichner (z.B. "UITheme.Sp/Dp", "try/finally",
# "AvatarFramework/Avatar.cs") raus.
$rootEntries = @(Get-ChildItem -LiteralPath $workspaceRoot -Force |
    Select-Object -ExpandProperty Name)

function Get-ScanFiles {
    $roots = @(
        (Join-Path $workspaceRoot 'AGENTS.md'),
        (Join-Path $workspaceRoot 'README.md'),
        (Join-Path $workspaceRoot 'DEVELOPERS.md'),
        (Join-Path $workspaceRoot 'CONTRIBUTING.md'),
        (Join-Path $workspaceRoot 'docs'),
        (Join-Path $workspaceRoot 'Skills'),
        (Join-Path $workspaceRoot 'ThirdParty/README.md'),
        (Join-Path $workspaceRoot 'GameReferences/README.md')
    )
    foreach ($r in $roots) {
        if (Test-Path -LiteralPath $r -PathType Leaf) { Get-Item -LiteralPath $r }
        elseif (Test-Path -LiteralPath $r -PathType Container) {
            Get-ChildItem -LiteralPath $r -Recurse -Filter '*.md' -File
        }
    }
}

function Get-Candidates {
    param([string]$Text)

    $found = @()
    # Inline-Code-Spans
    foreach ($m in [regex]::Matches($Text, '`([^`\n]+)`')) { $found += $m.Groups[1].Value }
    # Markdown-Links/Bilder
    foreach ($m in [regex]::Matches($Text, '\]\(([^)\s]+)\)')) { $found += $m.Groups[1].Value }
    return $found
}

function Test-IsPathCandidate {
    param([string]$Token)

    $t = $Token.Trim()
    $t = $t.TrimEnd('.', ',', ';', '!', '?', ')', ']', '}', '"', "'", '»')
    if (-not $t) { return $null }
    if ($t -match '^(https?|ftp|file)://') { return $null }
    if ($t -match '^[A-Za-z]:') { return $null }            # absolute Windows-Pfade
    if ($t -match '[<>\{\}\*~]') { return $null }           # Platzhalter/Globs
    if ($t -match '[:@]') { return $null }                  # git show sha:path, user@host
    if ($t -match '\.\.') { return $null }                  # Parent-Refs/Ellipsen: nicht pruefbar
    if ($t -match '\s') { return $null }                    # Prosa
    $t = $t -replace '\\', '/'
    $t = ($t -split '#')[0]                                 # Anker abtrennen
    $t = $t.TrimEnd('/')
    if (-not $t -or $t -notmatch '/') { return $null }      # ohne Slash: zu ambig

    $segments = $t -split '/'
    $first = $segments[0]

    # Repo-relativ: erstes Segment existiert aktuell im Repo-Root ...
    if ($rootEntries -ccontains $first) { return $t }
    # ... oder ist eine bekannte historische Wurzel (immer tot).
    if ($historicRoots -ccontains $first) { return $t }
    return $null
}

$files = @(Get-ScanFiles)
$problems = @()
$localOnlyHits = @()
$checked = 0
$seen = @{}

foreach ($file in $files) {
    $raw = [System.IO.File]::ReadAllText($file.FullName, [System.Text.Encoding]::UTF8)
    $fileDir = Split-Path -Parent $file.FullName
    $lines = $raw -split "`r?`n"

    # Fence-State pro Zeile tracken -> gemeldete Zeilennummern bleiben
    # Original-Zeilennummern der Datei.
    $inFence = $false
    for ($i = 0; $i -lt $lines.Count; $i++) {
        if ($lines[$i] -match '^\s*```') { $inFence = -not $inFence; continue }
        if ($inFence) { continue }

        foreach ($cand in (Get-Candidates -Text $lines[$i])) {
            $t = Test-IsPathCandidate -Token $cand
            if (-not $t) { continue }
            $key = "$t"
            if ($seen.ContainsKey($key)) { continue }

            $rootPath = Join-Path $workspaceRoot ($t -replace '/', [System.IO.Path]::DirectorySeparatorChar)
            $relPath = Join-Path $fileDir ($t -replace '/', [System.IO.Path]::DirectorySeparatorChar)

            if (Test-Path -LiteralPath $rootPath) {
                $seen[$key] = $true; $checked++
                continue
            }
            if (Test-Path -LiteralPath $relPath) {
                $seen[$key] = $true; $checked++
                continue
            }
            # Gitignored = existiert nur lokal (frischer Clone/CI hat die Datei
            # nicht) -> dokumentiert OK, aber als Hinweis listen.
            git -C $workspaceRoot check-ignore -q -- "$t" 2>$null
            if ($LASTEXITCODE -eq 0) {
                $seen[$key] = $true; $checked++
                $localOnlyHits += "$t (gitignored — existiert nur lokal)"
                continue
            }

            $allowed = $false
            foreach ($a in $Allow) { if ($t -match $a) { $allowed = $true; break } }
            if ($allowed) { $seen[$key] = $true; $checked++; continue }

            $seen[$key] = $true
            $problems += ("{0}:{1}: Pfad nicht gefunden: {2}" -f ($file.FullName.Substring($workspaceRoot.Length + 1)), ($i + 1), $t)
        }
    }
}

if (-not $Quiet) {
    Write-Host ("=== Doc-Pfad-Check ({0} Markdown-Dateien, {1} eindeutige Pfade) ===" -f $files.Count, $checked) -ForegroundColor Cyan
    if ($localOnlyHits.Count -gt 0) {
        Write-Host ("Nur-lokal vorhanden (gitignored, {0}):" -f $localOnlyHits.Count) -ForegroundColor DarkYellow
        foreach ($h in $localOnlyHits) { Write-Host "  ~ $h" -ForegroundColor DarkYellow }
    }
}

if ($problems.Count -eq 0) {
    if (-not $Quiet) { Write-Host "Doc-Pfad-Check OK: alle referenzierten Pfade existieren." -ForegroundColor Green }
    exit 0
}

Write-Host ("Tote Pfad-Referenzen: {0}" -f $problems.Count) -ForegroundColor Red
foreach ($p in $problems) { Write-Host "  - $p" -ForegroundColor Yellow }
Write-Host ''
Write-Host 'Fix: Pfad in der Markdown-Datei korrigieren/entfernen — oder begruendete'
Write-Host 'Ausnahme ueber -Allow <regex> (dauerhaft: allowlist im Skript-Kopf pflegen).' -ForegroundColor DarkGray
exit 1
