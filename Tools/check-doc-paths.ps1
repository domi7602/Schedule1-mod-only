#Requires -Version 7
<#
.SYNOPSIS
    Prueft lokale Dateipfade in der selbst gepflegten Markdown-Dokumentation.

.DESCRIPTION
    Inventar: Git-getrackte und ungetrackte, nicht ignorierte Markdown-Dateien
    im Root sowie unter docs/, Skills/, Tools/ und Source/Mods/. Dazu kommen
    ThirdParty/README.md und GameReferences/README.md. Generierte bin/, obj/,
    Library/, build/logs/, vendored/node_modules und docs/upstream sowie
    Submodul-Inhalte werden nicht als eigene Dokumentation gescannt.

    Echte Inline-Links/Bilder und Referenzdefinitionen werden relativ zur
    Markdown-Datei aufgeloest, auch ../README.md und einfache Dateinamen.
    Winkelklammern und URL-kodierte Dateinamen mit Leerzeichen sind erlaubt.
    Ein gleichnamiger Pfad im Repo-Root kann einen kaputten Link nicht retten.
    Inline-Code bleibt eine getrennte Heuristik: nur Pfade mit bekanntem
    Repo-Root-Segment und Slash, existent relativ zum Root oder Dokument.
    Historische Inline-Code-Pfade duerfen auf derselben Zeile mit
    <!-- doc-paths: historical-reference (nichtleere Begruendung) -->
    markiert werden. Nur deren Code-Heuristik wird ausgenommen; echte Links
    bleiben auf dieser Zeile pruefpflichtig. Die Markierung gilt nicht weiter.

    Fenced Code (Backticks/Tilden) und Links innerhalb von Inline-Code werden
    ignoriert. URLs, absolute Pfade, Anker ohne Datei und Pfad-Platzhalter
    werden nicht geprueft. Anker-Ziele selbst werden NICHT validiert.
    Dies ist ein Dateipfad-Guard, kein vollstaendiger CommonMark-Parser:
    HTML-Links, mehrzeilige Linkziele und aufgeloeste Referenz-Labels werden
    nicht geprueft; lokale Referenzdefinitionen werden direkt validiert.

    Fehlende gitignorierte Ziele, generierte GameReferences/decompiled und
    lokale ThirdParty/local.build.props sind erlaubte lokale Voraussetzungen.
    Ziele in leeren Submodulen werden als lokal nicht pruefbar gemeldet;
    bei initialisierten Submodulen werden Zielpfade normal geprueft.
.EXAMPLE
    pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/check-doc-paths.ps1
#>
[CmdletBinding()]
param(
    [switch]$Quiet,
    # Regex-Ausnahmen gelten fuer normalisierte Repo-relative Zielpfade.
    [string[]]$Allow = @()
)

$ErrorActionPreference = 'Stop'
$workspaceRoot = [IO.Path]::GetFullPath((Split-Path $PSScriptRoot -Parent))
$Allow = @('^GameReferences/decompiled(/|$)', '^ThirdParty/[^/]+/local\.build\.props$') + $Allow
$historicRoots = @('Knowledge', 'memory', 'Archive', 'Docs', '.agents')
$rootEntries = @(Get-ChildItem -LiteralPath $workspaceRoot -Force | Select-Object -ExpandProperty Name)
$submodulePaths = @()
$gitmodules = Join-Path $workspaceRoot '.gitmodules'
if (Test-Path -LiteralPath $gitmodules) {
    foreach ($line in [IO.File]::ReadAllLines($gitmodules)) {
        if ($line -match '^\s*path\s*=\s*(.+?)\s*$') {
            $submodulePaths += ($Matches[1] -replace '\\', '/').TrimEnd('/')
        }
    }
}
$submoduleEmpty = @{}
$optionalTargets = @{}

function Test-InUninitializedSubmodule([string]$Token) {
    foreach ($sub in $submodulePaths) {
        if ($Token -ne $sub -and -not $Token.StartsWith("$sub/", [StringComparison]::OrdinalIgnoreCase)) { continue }
        if (-not $submoduleEmpty.ContainsKey($sub)) {
            $subDir = Join-Path $workspaceRoot $sub
            $submoduleEmpty[$sub] = -not (Test-Path -LiteralPath $subDir -PathType Container) -or
                @(Get-ChildItem -LiteralPath $subDir -Recurse -File -Force -ErrorAction SilentlyContinue |
                    Select-Object -First 1).Count -eq 0
        }
        return $submoduleEmpty[$sub]
    }
    return $false
}

function Invoke-DocPathGit {
    param([string[]]$Arguments)
    # Console OutputEncoding is not UTF-8 on every host. Decoding a native git
    # pipeline with that code page drops non-ASCII names before Test-Path runs.
    # -c is process-local and does not change the user's Git configuration.
    $start = [System.Diagnostics.ProcessStartInfo]::new('git')
    $start.UseShellExecute = $false
    $start.CreateNoWindow = $true
    $start.RedirectStandardOutput = $true
    $start.RedirectStandardError = $true
    $start.StandardOutputEncoding = [Text.UTF8Encoding]::new($false)
    $start.StandardErrorEncoding = [Text.UTF8Encoding]::new($false)
    foreach ($config in @('core.quotepath=false', 'i18n.logOutputEncoding=utf-8')) {
        $start.ArgumentList.Add('-c')
        $start.ArgumentList.Add($config)
    }
    $start.ArgumentList.Add('-C')
    $start.ArgumentList.Add($workspaceRoot)
    foreach ($argument in $Arguments) { $start.ArgumentList.Add($argument) }
    $start.Environment['GIT_OPTIONAL_LOCKS'] = '0'
    $process = [System.Diagnostics.Process]::Start($start)
    try {
        $stdout = $process.StandardOutput.ReadToEndAsync()
        $stderr = $process.StandardError.ReadToEndAsync()
        $process.WaitForExit()
        $errorText = $stderr.GetAwaiter().GetResult()
        if ($process.ExitCode -ne 0) {
            throw "Git-Markdown-Inventar konnte nicht gelesen werden (exit $($process.ExitCode)): $errorText"
        }
        return $stdout.GetAwaiter().GetResult()
    }
    finally { $process.Dispose() }
}

function Get-ScanFiles {
    # -z preserves spaces/non-ASCII names; Git is read-only even in an unborn repo.
    $inventory = Invoke-DocPathGit @('ls-files', '-z', '--cached', '--others', '--exclude-standard', '--', ':(icase)*.md')
    foreach ($path in ($inventory -split "`0" | Sort-Object -Unique)) {
        if (-not $path) { continue }
        if ($path -notmatch '^[^/]+\.[mM][dD]$|^(docs|Skills|Tools|Source/Mods)/|^(ThirdParty|GameReferences)/README\.md$') { continue }
        if ($path -match '(^|/)(bin|obj|Library|node_modules|vendor|vendored)/|(^|/)build/logs/|(^|/)docs/upstream/') { continue }
        $inSubmodule = $false
        foreach ($sub in $submodulePaths) {
            if ($path.StartsWith("$sub/", [StringComparison]::OrdinalIgnoreCase)) { $inSubmodule = $true; break }
        }
        if ($inSubmodule) { continue }
        $fullPath = Join-Path $workspaceRoot $path
        # A tracked document deleted in this working tree is no longer a source.
        if (Test-Path -LiteralPath $fullPath -PathType Leaf) { Get-Item -LiteralPath $fullPath }
    }
}

function Get-Candidates([string]$Text) {
    $plain = $Text
    $codeSpans = [regex]::Matches($Text, '(?<!`)(?<ticks>`+)(?!`)(?<body>.*?)(?<!`)\k<ticks>(?!`)')
    foreach ($span in $codeSpans) {
        [pscustomobject]@{ Token = $span.Groups['body'].Value; Kind = 'Code' }
    }
    # Mask spans before parsing links, retaining surrounding link-label syntax.
    for ($index = $codeSpans.Count - 1; $index -ge 0; $index--) {
        $span = $codeSpans[$index]
        $plain = $plain.Remove($span.Index, $span.Length).Insert($span.Index, (' ' * $span.Length))
    }
    # Balancing groups allow parentheses in unquoted destinations. Optional
    # Markdown titles are not part of the filesystem path.
    $inlinePattern = '\]\(\s*(?:<(?<angle>[^<>\r\n]*)>|(?<bare>(?:\\.|[^\\\s()]|\((?<depth>)|\)(?<-depth>))*(?(depth)(?!))))(?:\s+(?:"[^"]*"|''[^'']*''|\([^)]*\)))?\s*\)'
    foreach ($link in [regex]::Matches($plain, $inlinePattern)) {
        $token = if ($link.Groups['angle'].Success) { $link.Groups['angle'].Value } else { $link.Groups['bare'].Value }
        [pscustomobject]@{ Token = $token; Kind = 'Link' }
    }
    if ($plain -match '^\s{0,3}\[[^\]]+\]:\s*(?:<(?<angle>[^<>\r\n]*)>|(?<bare>\S+))') {
        $token = if ($Matches.ContainsKey('angle')) { $Matches['angle'] } else { $Matches['bare'] }
        [pscustomobject]@{ Token = $token; Kind = 'Link' }
    }
}

function Get-PathToken([string]$Token, [string]$Kind) {
    $t = $Token.Trim()
    if ($Kind -eq 'Code') {
        $t = $t.TrimEnd('.', ',', ';', '!', '?', ')', ']', '}', '"', "'", '»')
        if ($t -match '\s|\.\.|[:@]') { return $null }
    }
    if (-not $t -or $t -match '^[A-Za-z][A-Za-z0-9+.-]*:|^[/\\]|^[#?]') { return $null }
    if ($t -match '[<>\{\}*~]') { return $null }
    if ($Kind -eq 'Link') {
        # Markdown escaping is distinct from Windows path separators.
        $t = $t -replace '\\([()\[\] _!#])', '$1'
        $t = [System.Net.WebUtility]::HtmlDecode($t)
    }
    $t = ($t -split '[#?]', 2)[0]
    if ($Kind -eq 'Link') { $t = [Uri]::UnescapeDataString($t) }
    $t = ($t -replace '\\', '/').TrimEnd('/')
    if (-not $t) { return $null }
    if ($Kind -eq 'Code') {
        if ($t -notmatch '/') { return $null }
        $first = ($t -split '/')[0]
        if ($rootEntries -cnotcontains $first -and $historicRoots -cnotcontains $first) { return $null }
    }
    return $t
}

function Get-OptionalTargetReason([string]$Target) {
    if ($optionalTargets.ContainsKey($Target)) { return $optionalTargets[$Target] }
    $reason = $null
    foreach ($pattern in $Allow) {
        if ($Target -match $pattern) { $reason = 'dokumentierte lokale Voraussetzung / Allow'; break }
    }
    if (-not $reason -and (Test-InUninitializedSubmodule $Target)) {
        $reason = 'Submodul nicht initialisiert - lokal nicht pruefbar'
    }
    if (-not $reason) {
        & git -C $workspaceRoot check-ignore -q -- $Target 2>$null
        if ($LASTEXITCODE -eq 0) { $reason = 'gitignored - lokale Voraussetzung' }
        # Git returns 128 for a path inside an initialized submodule. That does
        # not exempt a missing target; local.build.props is explicit above.
    }
    $optionalTargets[$Target] = $reason
    return $reason
}

$files = @(Get-ScanFiles)
$problems = [Collections.Generic.List[string]]::new()
$localOnlyHits = [Collections.Generic.HashSet[string]]::new()
$seen = [Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
$checked = 0

foreach ($file in $files) {
    $source = [IO.Path]::GetRelativePath($workspaceRoot, $file.FullName) -replace '\\', '/'
    $lines = [IO.File]::ReadAllLines($file.FullName, [Text.Encoding]::UTF8)
    $fenceCharacter = $null
    $fenceLength = 0
    for ($i = 0; $i -lt $lines.Count; $i++) {
        if ($lines[$i] -match '^\s*(?<fence>`{3,}|~{3,})(?<tail>.*)$') {
            $fence = $Matches['fence']
            if (-not $fenceCharacter) { $fenceCharacter = $fence[0]; $fenceLength = $fence.Length }
            elseif ($fence[0] -eq $fenceCharacter -and $fence.Length -ge $fenceLength -and -not $Matches['tail'].Trim()) {
                $fenceCharacter = $null
            }
            continue
        }
        if ($fenceCharacter) { continue }

        $historicalCode = [regex]::IsMatch($lines[$i], '<!--\s*doc-paths:\s*historical-reference\s+\(\s*[^\s)][^)]*\)\s*-->')
        foreach ($candidate in (Get-Candidates $lines[$i])) {
            if ($historicalCode -and $candidate.Kind -eq 'Code') { continue }
            $token = Get-PathToken $candidate.Token $candidate.Kind
            if (-not $token) { continue }
            $relativePath = [IO.Path]::GetFullPath((Join-Path $file.DirectoryName $token))
            if ($candidate.Kind -eq 'Link') { $targets = @($relativePath) }
            else {
                $rootPath = [IO.Path]::GetFullPath((Join-Path $workspaceRoot $token))
                $targets = @($rootPath, $relativePath)
            }
            $selectedTarget = $targets[0]
            $valid = $false
            $localHint = $null
            foreach ($target in $targets) {
                if (Test-Path -LiteralPath $target) { $selectedTarget = $target; $valid = $true; break }
            }
            if (-not $valid) {
                foreach ($target in $targets) {
                    $repoTarget = [IO.Path]::GetRelativePath($workspaceRoot, $target) -replace '\\', '/'
                    if ($repoTarget -eq '..' -or $repoTarget.StartsWith('../')) { continue }
                    $reason = Get-OptionalTargetReason $repoTarget
                    if ($reason) {
                        $selectedTarget = $target; $valid = $true; $localHint = "$repoTarget ($reason)"; break
                    }
                }
            }
            # Include source and actual resolved target, never just the token.
            # A code span and link to the same file count once in this source.
            if (-not $seen.Add("$source`0$selectedTarget")) { continue }
            $checked++
            if ($localHint) { $null = $localOnlyHits.Add($localHint) }
            if (-not $valid) { $problems.Add("${source}:$($i + 1): Pfad nicht gefunden ($($candidate.Kind)): $token") }
        }
    }
}

if (-not $Quiet) {
    Write-Host ("=== Doc-Pfad-Check ({0} Markdown-Dateien, {1} Quelle/Ziel-Paare) ===" -f $files.Count, $checked) -ForegroundColor Cyan
    foreach ($hit in ($localOnlyHits | Sort-Object)) { Write-Host "  ~ $hit" -ForegroundColor DarkYellow }
}
if ($problems.Count -eq 0) {
    if (-not $Quiet) { Write-Host 'Doc-Pfad-Check OK (Dateipfade; Anker nicht geprueft).' -ForegroundColor Green }
    exit 0
}
Write-Host ("Tote Pfad-Referenzen: {0}" -f $problems.Count) -ForegroundColor Red
foreach ($problem in $problems) { Write-Host "  - $problem" -ForegroundColor Yellow }
Write-Host 'Fix: Ziel relativ zur Markdown-Datei korrigieren; begruendete lokale Ausnahmen ueber -Allow <regex>.' -ForegroundColor DarkGray
exit 1
