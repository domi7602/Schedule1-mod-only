#Requires -Version 7
<#
.SYNOPSIS
    Baut pro Mod ein Review-/Upload-Set (max. N Dateien) aus dem aktuellen Quellstand.

.DESCRIPTION
    Hintergrund: externe Review-Uploads (z.B. ChatGPT) haben ein Limit von 20
    Anhaengen pro Nachricht. Mods mit mehr als ~19 Quelldateien passen nicht hinein.
    Dieses Skript erzeugt deshalb KOPIEN unter <repo>/.scratch/gpt-uploads/<Mod>/ —
    der Repo-Quellcode bleibt unveraendert. Es ist ein reiner Generator, kein Refactor.

    Regeln:
      * Passt ein Mod inkl. csproj in das Limit, werden seine Dateien verbatim kopiert.
      * Sonst wird pro Top-Level-Ordner des src/-Baums ein Block-Bundle erzeugt
        (die Ordnerstruktur aus der Repo-Sortierung liefert die natuerlichen Schnitte).
        Reicht das noch nicht, werden die kleinsten Einheiten zusammengelegt.
      * In einem Bundle steht jede Originaldatei in einem eigenen Block. Gegenueber dem
        Original geaendert wird ausschliesslich:
          - die file-scoped `namespace X;`-Zeile wird zur Blockform `namespace X { ... }`
          - `extern alias`-Zeilen werden an den Bundel-Anfang gehoben (muessen dort stehen)
        Alles andere bleibt verbatim. Jede Datei ist mit `// from: <pfad>` markiert.

    Das Skript ist idempotent: das Ausgabeverzeichnis wird pro Mod geleert.

.PARAMETER Mod
    Nur diesen einen Mod verarbeiten (Ordnername unter Source/Mods).

.PARAMETER OutDir
    Zielverzeichnis. Default: <repo>/.scratch/gpt-uploads

.PARAMETER MaxFiles
    Obergrenze pro Mod inkl. csproj. Default: 20.

.EXAMPLE
    pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/build-upload-sets.ps1
    pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/build-upload-sets.ps1 -Mod HitmanPhone
#>
[CmdletBinding()]
param(
    [string]$Mod,
    [string]$OutDir,
    [int]$MaxFiles = 20
)

$ErrorActionPreference = 'Stop'

$repo = Split-Path $PSScriptRoot -Parent
if (-not $OutDir) { $OutDir = Join-Path $repo '.scratch/gpt-uploads' }
$modsRoot = Join-Path $repo 'Source/Mods'
$utf8NoBom = [System.Text.UTF8Encoding]::new($false)

function Write-Lf([string]$Path, [string]$Text) {
    [System.IO.File]::WriteAllText($Path, $Text, $utf8NoBom)
}

function Get-ModSourceFiles([string]$SrcRoot) {
    Get-ChildItem -Path $SrcRoot -Recurse -File -Filter '*.cs' |
        Where-Object { $_.FullName -notmatch '[\\/](obj|bin)[\\/]' } |
        ForEach-Object {
            [pscustomobject]@{
                Rel  = [System.IO.Path]::GetRelativePath($SrcRoot, $_.FullName).Replace('\', '/')
                Name = $_.Name
                Full = $_.FullName
            }
        } | Sort-Object Rel
}

# Ein Block innerhalb eines Bundels: Inhalt verbatim, file-scoped namespace -> Blockform,
# extern alias nach oben gehoben.
function Convert-FileBlock([string]$FullPath, [string]$Rel, [System.Collections.Generic.List[string]]$Hoisted) {
    $lines = [System.IO.File]::ReadAllLines($FullPath)
    $body = [System.Collections.Generic.List[string]]::new()
    $nsOpen = $null
    $nsDepth = 0

    foreach ($line in $lines) {
        if ($line -match '^\s*extern\s+alias\s') {
            $Hoisted.Add($line)
            continue
        }
        if ($nsOpen -eq $null -and $line -match '^namespace\s+([A-Za-z_][\w\.]*)\s*;\s*$') {
            $nsOpen = $Matches[1]
            $body.Add("namespace $nsOpen")
            $body.Add('{')
            $nsDepth = 1
            continue
        }
        $body.Add($line)
    }

    $sb = [System.Text.StringBuilder]::new()
    [void]$sb.AppendLine("// ---------------- from: $Rel ----------------")
    foreach ($l in $body) { [void]$sb.AppendLine($l) }
    if ($nsDepth -eq 1) { [void]$sb.AppendLine('}') }
    return $sb.ToString()
}

function New-Bundle([string]$ModName, $Entries) {
    $hoisted = [System.Collections.Generic.List[string]]::new()
    $parts = [System.Collections.Generic.List[string]]::new()
    foreach ($e in $Entries) {
        $parts.Add((Convert-FileBlock -FullPath $e.Full -Rel $e.Rel -Hoisted $hoisted))
    }

    $sb = [System.Text.StringBuilder]::new()
    [void]$sb.AppendLine('// ' + ('=' * 66))
    [void]$sb.AppendLine("// MERGED FILE (review upload) - $ModName")
    [void]$sb.AppendLine('// Content of these original files, verbatim, one block each:')
    foreach ($e in $Entries) { [void]$sb.AppendLine("//   $($e.Rel)") }
    [void]$sb.AppendLine('// Only each file''s file-scoped namespace line was converted to block form.')
    if ($hoisted.Count -gt 0) {
        [void]$sb.AppendLine('// extern alias directives are hoisted below (they must precede all other elements).')
    }
    [void]$sb.AppendLine('// ' + ('=' * 66))
    [void]$sb.AppendLine()
    if ($hoisted.Count -gt 0) {
        foreach ($h in $hoisted) { [void]$sb.AppendLine($h) }
        [void]$sb.AppendLine()
    }
    foreach ($p in $parts) { [void]$sb.AppendLine($p) }
    return $sb.ToString()
}

# ---------------------------------------------------------------- collect mods
$modDirs = Get-ChildItem -Path $modsRoot -Directory |
    Where-Object { Test-Path (Join-Path $_.FullName 'src') } |
    Sort-Object Name
if ($Mod) {
    $modDirs = @($modDirs | Where-Object { $_.Name -eq $Mod })
    if ($modDirs.Count -eq 0) { throw "Mod '$Mod' not found under $modsRoot" }
}

$overview = [System.Collections.Generic.List[string]]::new()
$overview.Add('# GPT-Upload-Sets (max. ' + $MaxFiles + ' Dateien pro Mod)')
$overview.Add('')
$overview.Add('Generiert von `Tools/build-upload-sets.ps1`. **Kopien** - der Repo-Quellcode')
$overview.Add('bleibt unveraendert. Bei gemergten Dateien steht jede Originaldatei in einem')
$overview.Add('eigenen Block (Kopf-Kommentar + `// from: <pfad>`-Trenner).')
$overview.Add('')
$overview.Add('| Mod | src .cs | Set-Dateien | Bundles |')
$overview.Add('|---|---|---|---|')

$mergeDetails = [System.Collections.Generic.List[string]]::new()
$totalMods = 0

foreach ($m in $modDirs) {
    $srcRoot = Join-Path $m.FullName 'src'
    $files = @(Get-ModSourceFiles $srcRoot)
    $proj = Get-ChildItem -Path $srcRoot -File -Filter '*.csproj' | Select-Object -First 1
    if ($files.Count -eq 0) { continue }
    if (-not $proj) { throw "$($m.Name): no csproj found in src/" }

    $dest = Join-Path $OutDir $m.Name
    if (Test-Path $dest) { Remove-Item $dest -Recurse -Force }
    New-Item -ItemType Directory -Path $dest -Force | Out-Null

    $bundles = @()
    if ($files.Count + 1 -le $MaxFiles) {
        # verbatim: eine Ausgabedatei pro Quelldatei
        $names = @($files | ForEach-Object { $_.Name })
        $dup = $names | Group-Object | Where-Object Count -gt 1
        if ($dup) { throw "$($m.Name): duplicate basenames, cannot flatten: $($dup.Name -join ', ')" }
        foreach ($f in $files) {
            Copy-Item $f.Full (Join-Path $dest $f.Name)
        }
    }
    else {
        # Bundles: pro Top-Level-Ordner eine Einheit, Root-Dateien einzeln
        $units = [System.Collections.Generic.List[object]]::new()
        $byFolder = $files | Group-Object { ($_.Rel -split '/')[0] }
        foreach ($g in $byFolder) {
            $isRoot = ($g.Group[0].Rel -notmatch '/')
            if ($isRoot) {
                foreach ($f in $g.Group) {
                    $units.Add([pscustomobject]@{ Name = $f.Name; Entries = @($f); Size = (Get-Item $f.Full).Length })
                }
            }
            else {
                $sum = ($g.Group | ForEach-Object { (Get-Item $_.Full).Length } | Measure-Object -Sum).Sum
                $units.Add([pscustomobject]@{ Name = "$($g.Name).cs"; Entries = @($g.Group); Size = $sum })
            }
        }

        while ($units.Count + 1 -gt $MaxFiles) {
            $ordered = $units | Sort-Object Size
            $a = $ordered[0]
            $b = $ordered[1]
            $merged = [pscustomobject]@{
                Name    = [System.IO.Path]::GetFileNameWithoutExtension($a.Name) + '_' +
                          [System.IO.Path]::GetFileNameWithoutExtension($b.Name) + '.cs'
                Entries = @($a.Entries) + @($b.Entries)
                Size    = $a.Size + $b.Size
            }
            $units.Remove($a)
            $units.Remove($b)
            $units.Add($merged)
        }

        foreach ($u in ($units | Sort-Object Name)) {
            Write-Lf (Join-Path $dest $u.Name) (New-Bundle -ModName $m.Name -Entries $u.Entries)
            $bundles += ("{0} <- {1}" -f $u.Name, (($u.Entries | ForEach-Object { $_.Name }) -join ', '))
        }
    }

    Copy-Item $proj.FullName (Join-Path $dest $proj.Name)

    $setCount = (Get-ChildItem -Path $dest -File).Count
    if ($setCount -gt $MaxFiles) { throw "$($m.Name): produced $setCount files, limit is $MaxFiles" }

    $totalMods++
    $overview.Add("| $($m.Name) | $($files.Count) | $setCount | $(if ($bundles.Count) { $bundles.Count } else { '-' }) |")
    if ($bundles.Count -gt 0) {
        $mergeDetails.Add("### $($m.Name)")
        foreach ($b in $bundles) { $mergeDetails.Add("- $b") }
        $mergeDetails.Add('')
    }
    Write-Host ("{0,-24} {1,3} src .cs -> {2,3} Set-Dateien" -f $m.Name, $files.Count, $setCount)
}

if ($mergeDetails.Count -gt 0) {
    $overview.Add('')
    $overview.Add('## Bundles im Detail')
    $overview.Add('')
    $overview.AddRange($mergeDetails)
}
Write-Lf (Join-Path $OutDir 'UEBERSICHT.md') (($overview -join "`n") + "`n")

Write-Host ''
Write-Host "OK: $totalMods Mod-Sets geschrieben nach $OutDir (Limit $MaxFiles Dateien/Mod)."
