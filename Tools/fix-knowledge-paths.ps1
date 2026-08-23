#requires -Version 7
<#
.SYNOPSIS
    Repariert kaputte Cross-Reference-Pfade in allen Knowledge/*.md-Dateien.

.DESCRIPTION
    Standardmaessig (Scope = XRef): Nur in der '## Cross-References'-Sektion,
    wie sie in ThirdParty-Per-Mod-Analysen verwendet wird. Ersetzt:
      Documentation/systems/    -> Analysis/Systems/
      Documentation/learnings/  -> Analysis/Learnings/
      Documentation/apis/       -> Analysis/APIs/
      Documentation/guides/     -> Analysis/Guides/
      Documentation/il2cpp/     -> Analysis/IL2CPP/
      Documentation/reference/  -> Analysis/Reference/

    Mit -Scope Prose (Vorsicht!): Ersetzt auch im Fliesstext, AUSSER innerhalb
    von ```csharp```/``` ... ```-Codebloecken. Fuer Source-Export/READMEs/Guides.

.PARAMETER Scope
    'XRef' (Default) oder 'Prose'. XRef beschränkt auf '## Cross-References'.

.PARAMETER WhatIf
    Nur anzeigen, was geaendert wuerde. Keine Schreibzugriffe.

.PARAMETER Apply
    Aenderungen tatsaechlich schreiben.

.EXAMPLE
    pwsh Tools/fix-knowledge-paths.ps1 -WhatIf -Scope Prose
    pwsh Tools/fix-knowledge-paths.ps1 -Apply  -Scope XRef    # 121 ThirdParty-Files
    pwsh Tools/fix-knowledge-paths.ps1 -Apply  -Scope Prose   # alles andere
#>

param(
    [ValidateSet('XRef', 'Prose')]
    [string]$Scope = 'XRef',

    [switch]$WhatIf,
    [switch]$Apply
)

$ErrorActionPreference = 'Stop'
$root = $PSScriptRoot | Split-Path -Parent | Join-Path -ChildPath 'Knowledge'

if (-not (Test-Path -LiteralPath $root)) {
    Write-Error "Pfad nicht gefunden: $root"
    exit 1
}

if (-not $WhatIf -and -not $Apply) {
    Write-Warning "Weder -WhatIf noch -Apply angegeben. Verwende -WhatIf als Default."
    $WhatIf = $true
}

$replacements = @{
    'Documentation/systems/'    = 'Analysis/Systems/'
    'Documentation/learnings/'  = 'Analysis/Learnings/'
    'Documentation/apis/'       = 'Analysis/APIs/'
    'Documentation/guides/'     = 'Analysis/Guides/'
    'Documentation/il2cpp/'     = 'Analysis/IL2CPP/'
    'Documentation/reference/'  = 'Analysis/Reference/'
}

$files = Get-ChildItem -LiteralPath $root -Filter '*.md' -File -Recurse
Write-Host "Gefunden: $($files.Count) Markdown-Dateien (Scope=$Scope)" -ForegroundColor Cyan

$stats = @{
    FilesScanned   = 0
    FilesModified  = 0
    FilesSkipped   = 0
    TotalReplaces  = 0
}

function Invoke-Replacements {
    param([string]$Text)
    $newText = $Text
    $localCount = 0
    foreach ($key in $replacements.Keys) {
        $count = ([regex]::Matches($newText, [regex]::Escape($key))).Count
        if ($count -gt 0) {
            $newText = $newText.Replace($key, $replacements[$key])
            $localCount += $count
        }
    }
    return @{ Text = $newText; Count = $localCount }
}

foreach ($file in $files) {
    $stats.FilesScanned++
    $content = Get-Content -LiteralPath $file.FullName -Raw -Encoding UTF8
    $newContent = $content
    $fileReplaces = 0

    if ($Scope -eq 'XRef') {
        # Nur Cross-References-Sektion
        $pattern = '(?ms)(##\s+Cross-References\s*\r?\n)(.*?)(?=\r?\n---|\z)'
        $newContent = [regex]::Replace($content, $pattern, {
            param($m)
            $header = $m.Groups[1].Value
            $section = $m.Groups[2].Value
            $r = Invoke-Replacements $section
            $script:statsProxy_total += $r.Count
            return $header + $r.Text
        })
        $fileReplaces = $script:statsProxy_total
        $script:statsProxy_total = 0
        if ($newContent -eq $content) {
            $stats.FilesSkipped++
            continue
        }
    }
    else {
        # Prose-Modus: Ersetze ueberall, AUSSER in ```-Code-Bloecken
        # Strategie: Code-Bloecke durch Platzhalter ersetzen, dann ersetzen, dann zurueck
        $cbPattern = '(?s)```.*?```'
        $codeBlocks = [System.Collections.Generic.List[hashtable]]::new()
        $placeholder = $content
        $idx = 0
        $placeholder = [regex]::Replace($placeholder, $cbPattern, {
            param($m)
            $token = "___CODEBLOCK_$idx___"
            $codeBlocks.Add(@{ Token = $token; Body = $m.Value })
            $idx++
            return $token
        })
        $r = Invoke-Replacements $placeholder
        $placeholder = $r.Text
        $fileReplaces = $r.Count

        # Code-Bloecke wieder einsetzen
        foreach ($cb in $codeBlocks) {
            $placeholder = $placeholder.Replace($cb.Token, $cb.Body)
        }
        $newContent = $placeholder
    }

    if ($newContent -ne $content -and $fileReplaces -gt 0) {
        $stats.FilesModified++
        $stats.TotalReplaces += $fileReplaces
        Write-Host ("  [{0}] {1} Ersetzungen" -f $file.Name, $fileReplaces) -ForegroundColor Yellow
        if ($Apply) {
            $utf8NoBom = New-Object System.Text.UTF8Encoding($false)
            [System.IO.File]::WriteAllText($file.FullName, $newContent, $utf8NoBom)
        }
    }
    elseif ($Scope -eq 'XRef') {
        $stats.FilesSkipped++
    }
}

Write-Host ""
Write-Host "=== Zusammenfassung ===" -ForegroundColor Green
Write-Host ("Dateien gescannt:        {0}" -f $stats.FilesScanned)
Write-Host ("Dateien geaendert:       {0}" -f $stats.FilesModified)
Write-Host ("Dateien ohne Match:      {0}" -f $stats.FilesSkipped)
Write-Host ("Ersetzungen total:       {0}" -f $stats.TotalReplaces)
if ($WhatIf) {
    Write-Host ""
    Write-Host "DRY-RUN: Keine Dateien geschrieben. Erneut mit -Apply aufrufen." -ForegroundColor Magenta
}
