#Requires -Version 7
<#
.SYNOPSIS
    Bumps version across the source-of-truth files for one mod (or all mods).

.DESCRIPTION
    Synchronisiert die Version über:
      1. Source/Mods/<Mod>/src/*.cs (Datei mit MelonInfo-Attribut; PotScanner
         nutzt Constants.ModVersion)
      2. Source/Mods/<Mod>/docs/mod.json -> "version"
      3. Source/Mods/<Mod>/docs/CHANGELOG.md -> prepend "## x.y.z - YYYY-MM-DD"
      4. AGENTS.md -> Mod-Matrix-Zeile + Detail-Header
      5. README.md -> Mod-Tabellenzeile + Featured-Karte
      6. docs/compatibility.md -> aktuelle Zeile in "## Verification matrix"

    Truthfulness rules:
      - ALL expected rows are located and validated BEFORE any file is written.
        A missing or ambiguous row aborts the whole run with exit code 2 and no
        partial success. Rows are never invented.
      - A version change NEVER carries a verification/acceptance label forward.
        A bare prior date becomes "In-Game-Verify open (prior vOLD: DATE)";
        AGENTS/README acceptance labels are converted to an open label that
        retains the prior version and date. Already-open rows are preserved
        byte-identically (only the version token changes); no historical
        version is guessed for their prior evidence. A "Verified" claim outside
        a supported bold label aborts for manual status reconciliation.
        Evidence notes are explicitly marked historical.
      - Same-version reconciliation is idempotent and does not touch acceptance
        labels (LightControl use case).

.EXAMPLE
    pwsh Tools/bump-version.ps1 -Mod NotesApp -Version 1.0.4 -DryRun
    pwsh Tools/bump-version.ps1 -Mod All -Version 1.0.4
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory)][string]$Mod,
    [Parameter(Mandatory)][string]$Version,
    [switch]$DryRun
)

$ErrorActionPreference = "Stop"

if ($Version -notmatch '^\d+\.\d+\.\d+(-[\w\.]+)?$') {
    Write-Host "ERROR: Version '$Version' ist kein SemVer (erwartet z.B. 1.0.4 oder 0.1.0-beta.1)." -ForegroundColor Red
    exit 1
}

$workspaceRoot = Split-Path $PSScriptRoot -Parent
$modsRoot = Join-Path $workspaceRoot "Source/Mods"
$agentsPath = Join-Path $workspaceRoot "AGENTS.md"
$readmePath = Join-Path $workspaceRoot "README.md"
$compatPath = Join-Path $workspaceRoot "docs/compatibility.md"
$today = Get-Date -Format "yyyy-MM-dd"
$utf8NoBom = New-Object System.Text.UTF8Encoding($false)
$rxSemVer = '\d+\.\d+\.\d+(?:-[\w\.]+)?'

function Read-Utf8([string]$Path) {
    return [System.IO.File]::ReadAllText($Path, [System.Text.Encoding]::UTF8)
}

function Get-CodeVersionInfo {
    param([string]$ModDir)

    $srcDir = Join-Path $ModDir "src"
    if (-not (Test-Path -LiteralPath $srcDir)) {
        throw "src/ fehlt: $srcDir"
    }

    $files = @(Get-ChildItem -LiteralPath $srcDir -Filter "*.cs" -Recurse -File -ErrorAction SilentlyContinue |
               Where-Object { $_.FullName -notmatch '[\\/](obj|bin)[\\/]' })

    $melonFiles = @($files | Where-Object { (Read-Utf8 $_.FullName) -match '\[assembly:\s*MelonInfo\(' })
    if ($melonFiles.Count -gt 1) {
        throw ("{0} Dateien enthalten ein MelonInfo-Attribut (erwartet genau 1): {1}" -f
               $melonFiles.Count, (($melonFiles | ForEach-Object { $_.Name }) -join ', '))
    }

    if ($melonFiles.Count -eq 1) {
        $file = $melonFiles[0]
        $text = Read-Utf8 $file.FullName
        $call = [regex]::Match($text, '(?s)\[assembly:\s*MelonInfo\((?<args>.*?)\)\s*\]')
        if (-not $call.Success) { throw "MelonInfo-Attribut in $($file.Name) nicht parsebar." }
        $quoted = [regex]::Matches($call.Groups['args'].Value, '"([^"]*)"')
        foreach ($q in $quoted) {
            if ($q.Groups[1].Value -match ('^' + $rxSemVer + '$')) {
                return [pscustomobject]@{
                    Version     = $q.Groups[1].Value
                    Path        = $file.FullName
                    Kind        = 'MelonInfo'
                    MatchIndex  = $call.Groups['args'].Index + $q.Groups[1].Index
                    MatchLength = $q.Groups[1].Length
                }
            }
        }
        # PotScanner keeps the literal in Constants.ModVersion; fall through to it.
    }

    # Fallback: Constants.ModVersion (PotScanner)
    $hits = @()
    foreach ($f in $files) {
        $text = Read-Utf8 $f.FullName
        foreach ($m in [regex]::Matches($text, ('ModVersion\s*=\s*"(' + $rxSemVer + ')"'))) {
            $hits += [pscustomobject]@{
                Version     = $m.Groups[1].Value
                Path        = $f.FullName
                Kind        = 'ModVersion'
                MatchIndex  = $m.Groups[1].Index
                MatchLength = $m.Groups[1].Length
            }
        }
    }
    if ($hits.Count -eq 0) { throw "Keine Version im Code gefunden (weder MelonInfo noch Constants.ModVersion)." }
    if ($hits.Count -gt 1) { throw "Mehrdeutige Version im Code: $($hits.Count) ModVersion-Treffer." }
    return $hits[0]
}

function Update-CurrentVersionStrings {
    param([string]$Text, [string]$OldVersion, [string]$NewVersion)

    # Runtime breadcrumbs such as "Registered ... (v0.5.0)." are current-version
    # markers. Historical wording ("migrated from v0.4.0") is left untouched.
    $rx = [regex]('"(?<pre>[^"\r\n]*\(v)' + [regex]::Escape($OldVersion) + '(?<post>\)[^"\r\n]*)"')
    return $rx.Replace($Text, {
        param($m)
        if ($m.Value -match '(?i)\b(from|previous|prior|historical|legacy|migrat\w*)\b') { return $m.Value }
        return '"' + $m.Groups['pre'].Value + $NewVersion + $m.Groups['post'].Value + '"'
    })
}

function Get-ChangelogContent {
    param([string]$Path, [string]$NewVersion)

    if (-not (Test-Path -LiteralPath $Path)) { throw "docs/CHANGELOG.md fehlt." }
    $raw = Read-Utf8 $Path
    if ($raw -match "##\s+$([regex]::Escape($NewVersion))\b") { return $null }

    $header = "## $NewVersion ($today)`n- Version bump.`n`n"
    if ($raw -match "(?m)^## ") {
        $rx = [regex]"(?m)(^## )"
        return $rx.Replace($raw, "$header`$1", 1)
    }
    if ($raw -match "(?m)^# Changelog[^\r\n]*\r?\n") {
        $rx = [regex]"(?m)(^# Changelog[^\r\n]*\r?\n)"
        return $rx.Replace($raw, "`$1`n$header", 1)
    }
    return "# Changelog`n`n$header`n$raw"
}

function Convert-AgentsStatus {
    param([string]$Status, [string]$OldVersion)

    # Already-open rows are verbatim evidence. Only the leading version token
    # may change; never guess or attach a historical version to prior evidence.
    if ($Status -match 'In-Game-Verify open') { return $Status }

    $label = [regex]'(?i)\*\*[^*]*(?:verified|owner[- ]confirmed)[^*]*\*\*'
    $verifiedMatches = $label.Matches($Status)

    # A Verified claim outside the supported bold labels cannot be carried
    # forward silently: abort for manual reconciliation before any write.
    if ($label.Replace($Status, '') -match '(?i)\bVerified\b') {
        throw "AGENTS.md-Status enthält 'Verified' ohne unterstütztes fettes Label: '$Status'. Status bitte manuell abstimmen."
    }
    if ($verifiedMatches.Count -eq 0) { return $Status }

    $state = @{ Index = 0 }
    return $label.Replace($Status, {
        param($m)
        $state.Index++
        if ($state.Index -eq 1) {
            return "**In-Game-Verify open** (prior v$OldVersion`: $($m.Value))"
        }
        return "$($m.Value) (historical)"
    })
}

function Get-AgentsEdits {
    param([string]$ModName, [string]$OldVersion, [string]$NewVersion, [bool]$IsTransition, [string]$Path)

    if (-not (Test-Path -LiteralPath $Path)) { throw "AGENTS.md fehlt." }
    $raw = Read-Utf8 $Path
    $lines = $raw -split "`r?`n"
    $esc = [regex]::Escape($ModName)

    $matrixIndexes = @()
    $headerIndexes = @()
    for ($i = 0; $i -lt $lines.Count; $i++) {
        if ($lines[$i] -match ('^\|\s*\*\*' + $esc + '\*\*\s*\|')) { $matrixIndexes += $i }
        elseif ($lines[$i] -match ('^\*\*' + $esc + ' v' + $rxSemVer)) { $headerIndexes += $i }
    }
    if ($matrixIndexes.Count -ne 1) {
        throw ("AGENTS.md: {0} Matrixzeilen für '{1}' (erwartet genau 1)." -f $matrixIndexes.Count, $ModName)
    }
    if ($headerIndexes.Count -gt 1) {
        throw ("AGENTS.md: {0} Detail-Header für '{1}' (erwartet höchstens 1)." -f $headerIndexes.Count, $ModName)
    }

    $edits = @()

    $mi = $matrixIndexes[0]
    $line = $lines[$mi]
    $cell = [regex]::Match($line, ('^(\|\s*\*\*' + $esc + '\*\*\s*\|)([^|]*)(\|.*)$'))
    if (-not $cell.Success) { throw "AGENTS.md-Matrixzeile nicht parsebar: $line" }
    $oldStatus = $cell.Groups[2].Value
    $newStatus = ([regex]('v' + $rxSemVer)).Replace($oldStatus, "v$NewVersion", 1)
    if ($IsTransition) {
        $newStatus = Convert-AgentsStatus -Status $newStatus -OldVersion $OldVersion
    }
    if ($newStatus -cne $oldStatus) {
        $edits += [pscustomobject]@{
            Old  = $line
            New  = $cell.Groups[1].Value + $newStatus + $cell.Groups[3].Value
            Note = "AGENTS.md matrix -> v$NewVersion (verification label not carried)"
        }
    }

    if ($headerIndexes.Count -eq 1) {
        $hi = $headerIndexes[0]
        $hline = $lines[$hi]
        $newHline = ([regex]('(^\*\*' + $esc + ' v)' + $rxSemVer)).Replace($hline, "`${1}$NewVersion", 1)
        if ($IsTransition) {
            $hdr = [regex]::Match($newHline, ('^(\*\*.*? v' + [regex]::Escape($NewVersion) + ' \()([^)]*)(\):\*\*.*)$'))
            if ($hdr.Success -and $hdr.Groups[2].Value -notmatch 'prior v') {
                $newHline = $hdr.Groups[1].Value + "prior v$OldVersion`: " + $hdr.Groups[2].Value + $hdr.Groups[3].Value
            }
        }
        if ($newHline -cne $hline) {
            $edits += [pscustomobject]@{
                Old  = $hline
                New  = $newHline
                Note = "AGENTS.md detail header -> v$NewVersion"
            }
        }
    }

    return @($edits)
}

function Convert-ReadmeStatus {
    param([string]$Status, [string]$OldVersion)

    if ($Status -match '(?i)In-Game-Verify open|in-game verify open') { return $Status }
    if ($Status -match '^\s*Active\b' -or $Status -match '(?i)\bVerified\b|owner[- ]confirmed') {
        return " In-Game-Verify open (prior v$OldVersion`: $($Status.Trim())) "
    }
    return $Status
}

function Get-ReadmeEdits {
    param([string]$ModName, [string]$OldVersion, [string]$NewVersion, [bool]$IsTransition, [string]$Path)

    if (-not (Test-Path -LiteralPath $Path)) { throw "README.md fehlt." }
    $raw = Read-Utf8 $Path
    $lines = $raw -split "`r?`n"
    $esc = [regex]::Escape($ModName)
    $rxTable = '^\|\s*\[\*\*' + $esc + '\*\*\]\([^)]*\)\s*\|'
    $rxCard = '^\s*<b>' + $esc + '</b>'

    $tableIndexes = @(); $cardIndexes = @(); $otherIndexes = @()
    for ($i = 0; $i -lt $lines.Count; $i++) {
        if ($lines[$i] -match $rxTable) { $tableIndexes += $i }
        elseif ($lines[$i] -match $rxCard) { $cardIndexes += $i }
        elseif ($lines[$i] -match ('\*\*' + $esc + '\*\*|<b>' + $esc + '</b>')) { $otherIndexes += $i }
    }
    if ($tableIndexes.Count -ne 1) {
        throw ("README.md: {0} Mod-Tabellenzeilen für '{1}' (erwartet genau 1)." -f $tableIndexes.Count, $ModName)
    }
    if ($otherIndexes.Count -gt 0) {
        throw ("README.md: nicht klassifizierte fette Nennung für '{1}' in Zeile(n) {2}." -f $ModName, (($otherIndexes + 1) -join ', '))
    }

    $edits = @()

    $ti = $tableIndexes[0]
    $tableLine = $lines[$ti]
    $cells = $tableLine.Split('|')
    if ($cells.Count -lt 7) { throw "README.md-Mod-Zeile hat zu wenige Spalten: $tableLine" }
    $oldVersionCell = $cells[3]
    $cells[3] = ([regex]$rxSemVer).Replace($oldVersionCell, $NewVersion, 1)

    if ($IsTransition) {
        $oldStatus = $cells[4]
        $cells[4] = Convert-ReadmeStatus -Status $oldStatus -OldVersion $OldVersion
        # Dated acceptance in the description must not silently describe the new version.
        $desc = $cells[5]
        if ($desc -match '(?i)(verified in-game\s+\d{4}-\d{2}-\d{2}|owner[- ]confirmed\s+\d{4}-\d{2}-\d{2})' -and
            $desc -notmatch '(?i)historical|prior v') {
            $cells[5] = $desc.TrimEnd() + " Verification evidence is historical for v$OldVersion; no acceptance for v$NewVersion. "
        }
    }
    $newTableLine = $cells -join '|'
    if ($newTableLine -cne $tableLine) {
        $edits += [pscustomobject]@{
            Old  = $tableLine
            New  = $newTableLine
            Note = "README.md table -> $NewVersion (verification label not carried)"
        }
    }

    foreach ($ci in $cardIndexes) {
        $card = $lines[$ci]
        $vers = [regex]::Matches($card, '\bv?(' + $rxSemVer + ')\b')
        if ($vers.Count -ne 1) {
            throw ("README.md: Featured-Karte für '{0}' enthält {1} Versions-Tokens (erwartet genau 1): {2}" -f
                   $ModName, $vers.Count, $card.Trim())
        }
        $newCard = ([regex]('\bv?' + $rxSemVer + '\b')).Replace($card, "v$NewVersion", 1)
        if ($newCard -cne $card) {
            $edits += [pscustomobject]@{
                Old  = $card
                New  = $newCard
                Note = "README.md featured card -> v$NewVersion"
            }
        }
    }

    return @($edits)
}

function Get-CompatibilityEdits {
    param([string]$ModName, [string]$OldVersion, [string]$NewVersion, [bool]$IsTransition, [string]$Path)

    if (-not (Test-Path -LiteralPath $Path)) { throw "docs/compatibility.md fehlt." }
    $raw = Read-Utf8 $Path
    $sections = [regex]::Matches($raw, '(?m)^## Verification matrix\s*$')
    if ($sections.Count -ne 1) {
        throw ("docs/compatibility.md: {0} Abschnitte '## Verification matrix' (erwartet genau 1)." -f $sections.Count)
    }

    $lines = $raw -split "`r?`n"
    $esc = [regex]::Escape($ModName)
    $inMatrix = $false; $rowLine = -1; $rowCount = 0
    for ($i = 0; $i -lt $lines.Count; $i++) {
        if (-not $inMatrix) {
            if ($lines[$i] -match '^## Verification matrix\s*$') { $inMatrix = $true }
            continue
        }
        if ($lines[$i] -match '^##\s') { break }
        if ($lines[$i] -match ('^\|\s*' + $esc + '\s*\|')) { $rowCount++; $rowLine = $i }
    }
    if ($rowCount -ne 1) {
        throw ("docs/compatibility.md: {0} Matrixzeilen für '{1}' (erwartet genau 1)." -f $rowCount, $ModName)
    }

    $line = $lines[$rowLine]
    $cells = $line.Split('|')
    if ($cells.Count -lt 9) { throw "docs/compatibility.md-Matrixzeile hat zu wenige Spalten: $line" }

    $oldVersionCell = $cells[2]
    $changes = @()
    $cells[2] = ([regex]$rxSemVer).Replace($oldVersionCell, $NewVersion, 1)
    if ($cells[2] -cne $oldVersionCell) { $changes += "compatibility.md matrix -> $NewVersion" }

    $dateWasConverted = $false
    $oldDate = $cells[4].Trim()
    if ($IsTransition -and $oldDate -notmatch 'In-Game-Verify open' -and
        $oldDate -notmatch '^\s*(—|-)?\s*$') {
        $cells[4] = " In-Game-Verify open (prior v$OldVersion`: $oldDate) "
        $dateWasConverted = $true
        $changes += "compatibility.md Date -> In-Game-Verify open (prior v$OldVersion)"
    }

    $notes = $cells[7]
    $notesNeedHistory = $dateWasConverted
    if (-not $notesNeedHistory -and $IsTransition) {
        if ($notes -match '(?i)(verified in-game|owner[- ]confirmed|verified on|verification evidence)' -and
            $notes -notmatch '(?i)historical|prior v') {
            $notesNeedHistory = $true
        }
    }
    if ($notesNeedHistory -and $notes -notmatch [regex]::Escape("historical for v$OldVersion")) {
        $note = if ($oldDate -match 'In-Game-Verify open') {
            "Verification evidence is historical for v$OldVersion (prior status: In-Game-Verify open); no acceptance for v$NewVersion."
        } else {
            "Verification evidence is historical for v$OldVersion ($oldDate); no acceptance for v$NewVersion."
        }
        $cells[7] = if ($notes.Trim().Length -gt 0) { $notes.TrimEnd() + " " + $note + " " } else { " $note " }
        $changes += "compatibility.md notes -> evidence marked historical"
    }

    $newLine = $cells -join '|'
    if ($newLine -cne $line) {
        return @([pscustomobject]@{
            Old  = $line
            New  = $newLine
            Note = ($changes -join '; ')
        })
    }
    return @()
}

function Build-ModPlan {
    param([string]$ModName, [string]$NewVersion)

    $modDir = Join-Path $modsRoot $ModName
    if (-not (Test-Path -LiteralPath $modDir)) { throw "Mod '$ModName' nicht gefunden: $modDir" }

    $code = Get-CodeVersionInfo -ModDir $modDir
    $oldVersion = $code.Version
    $isTransition = ($oldVersion -ne $NewVersion)

    $writes = @()
    $changes = @()

    # 1. Code version.
    if ($isTransition) {
        $codeText = Read-Utf8 $code.Path
        $newCode = $codeText.Substring(0, $code.MatchIndex) + $NewVersion +
                   $codeText.Substring($code.MatchIndex + $code.MatchLength)
        $newCode = [regex]::Replace($newCode, ('Initialized \(v' + [regex]::Escape($oldVersion) + '\)'), "Initialized (v$NewVersion)")
        $newCode = [regex]::Replace($newCode, ('initialisiert \(v' + [regex]::Escape($oldVersion) + '\)'), "initialisiert (v$NewVersion)")
        $newCode = Update-CurrentVersionStrings -Text $newCode -OldVersion $oldVersion -NewVersion $NewVersion
        if ($newCode -cne $codeText) {
            $writes += [pscustomobject]@{ Path = $code.Path; Content = $newCode }
            $changes += "$($code.Kind) $oldVersion -> $NewVersion"
        }

        $srcDir = Join-Path $modDir "src"
        foreach ($f in (Get-ChildItem -LiteralPath $srcDir -Filter "*.cs" -Recurse -File -ErrorAction SilentlyContinue |
                        Where-Object { $_.FullName -notmatch '[\\/](obj|bin)[\\/]' -and $_.FullName -ne $code.Path })) {
            $otherText = Read-Utf8 $f.FullName
            $otherNew = Update-CurrentVersionStrings -Text $otherText -OldVersion $oldVersion -NewVersion $NewVersion
            if ($otherNew -cne $otherText) {
                $writes += [pscustomobject]@{ Path = $f.FullName; Content = $otherNew }
                $changes += "log string in $($f.Name) -> $NewVersion"
            }
        }
    }

    # 2. docs/mod.json -- exactly one version literal.
    $modJsonPath = Join-Path $modDir "docs/mod.json"
    if (-not (Test-Path -LiteralPath $modJsonPath)) { throw "docs/mod.json fehlt." }
    $jsonRaw = Read-Utf8 $modJsonPath
    $jsonVersions = [regex]::Matches($jsonRaw, '"version"\s*:\s*"([^"]+)"')
    if ($jsonVersions.Count -ne 1) {
        throw ("docs/mod.json: {0} Versions-Literale (erwartet genau 1)." -f $jsonVersions.Count)
    }
    try { $null = $jsonRaw | ConvertFrom-Json } catch { throw "docs/mod.json ist kein gültiges JSON: $_" }
    if ($jsonVersions[0].Groups[1].Value -ne $NewVersion) {
        $newJson = $jsonRaw.Substring(0, $jsonVersions[0].Groups[1].Index) + $NewVersion +
                   $jsonRaw.Substring($jsonVersions[0].Groups[1].Index + $jsonVersions[0].Groups[1].Length)
        $writes += [pscustomobject]@{ Path = $modJsonPath; Content = $newJson }
        $changes += "mod.json $($jsonVersions[0].Groups[1].Value) -> $NewVersion"
    }

    # 3. CHANGELOG.
    $changelogPath = Join-Path $modDir "docs/CHANGELOG.md"
    $newCl = Get-ChangelogContent -Path $changelogPath -NewVersion $NewVersion
    if ($null -ne $newCl) {
        $writes += [pscustomobject]@{ Path = $changelogPath; Content = $newCl }
        $changes += "CHANGELOG.md prepend ## $NewVersion"
    }

    # 4. AGENTS.md (line edits, composed across -Mod All).
    $agentsEdits = Get-AgentsEdits -ModName $ModName -OldVersion $oldVersion -NewVersion $NewVersion -IsTransition $isTransition -Path $agentsPath
    if ($agentsEdits.Count -gt 0) {
        $writes += [pscustomobject]@{ Path = $agentsPath; Edits = $agentsEdits }
        $changes += $agentsEdits.Note
    }

    # 5. README.md (line edits, composed across -Mod All).
    $readmeEdits = Get-ReadmeEdits -ModName $ModName -OldVersion $oldVersion -NewVersion $NewVersion -IsTransition $isTransition -Path $readmePath
    if ($readmeEdits.Count -gt 0) {
        $writes += [pscustomobject]@{ Path = $readmePath; Edits = $readmeEdits }
        $changes += $readmeEdits.Note
    }

    # 6. docs/compatibility.md (line edits, composed across -Mod All).
    $compatEdits = Get-CompatibilityEdits -ModName $ModName -OldVersion $oldVersion -NewVersion $NewVersion -IsTransition $isTransition -Path $compatPath
    if ($compatEdits.Count -gt 0) {
        $writes += [pscustomobject]@{ Path = $compatPath; Edits = $compatEdits }
        $changes += $compatEdits.Note
    }

    return [pscustomobject]@{
        Mod          = $ModName
        Old          = $oldVersion
        New          = $NewVersion
        Changes      = @($changes)
        Writes       = @($writes)
        IsTransition = $isTransition
    }
}

# ---------------------------------------------------------------------------
# Preflight: build every plan and compose all edits in memory before writing.
# ---------------------------------------------------------------------------
$targets = @()
if ($Mod -eq "All") {
    $targets = @(Get-ChildItem -LiteralPath $modsRoot -Directory | Where-Object {
        $_.Name -ne "Shared" -and $_.Name -ne "Archive" -and (Test-Path (Join-Path $_.FullName "src"))
    } | ForEach-Object { $_.Name } | Sort-Object)
    if ($targets.Count -eq 0) { Write-Host "ERROR: Keine Mods gefunden in $modsRoot" -ForegroundColor Red; exit 1 }
} else {
    $targets = @($Mod)
}

$plans = @()
$errors = @()
foreach ($t in $targets) {
    try { $plans += Build-ModPlan -ModName $t -NewVersion $Version }
    catch { $errors += "[$t] $($_.Exception.Message)" }
}

# Compose shared-file edits and prove every anchor line still exists exactly once.
$fileContents = [ordered]@{}
if ($errors.Count -eq 0) {
    foreach ($p in $plans) {
        foreach ($w in $p.Writes) {
            if ($null -ne $w.PSObject.Properties['Edits']) {
                if (-not $fileContents.Contains($w.Path)) { $fileContents[$w.Path] = Read-Utf8 $w.Path }
                $text = $fileContents[$w.Path]
                foreach ($edit in $w.Edits) {
                    $occurrences = ([regex]::Matches($text, [regex]::Escape($edit.Old))).Count
                    if ($occurrences -ne 1) {
                        $errors += ("[{0}] {1}: Ankerzeile {2}-mal gefunden (erwartet 1): {3}" -f
                                    $p.Mod, (Split-Path $w.Path -Leaf), $occurrences, $edit.Old.Trim())
                        continue
                    }
                    $text = $text.Replace($edit.Old, $edit.New)
                }
                $fileContents[$w.Path] = $text
            } else {
                if ($fileContents.Contains($w.Path)) {
                    $errors += ("[{0}] {1}: interner Konflikt (Datei mehrfach als Ganzes geplant)." -f $p.Mod, $w.Path)
                    continue
                }
                $fileContents[$w.Path] = $w.Content
            }
        }
    }
}

if ($errors.Count -gt 0) {
    Write-Host ""
    Write-Host "ABORT: Preflight fehlgeschlagen — es wurde KEINE Datei geschrieben." -ForegroundColor Red
    foreach ($e in $errors) { Write-Host "  - $e" -ForegroundColor Red }
    Write-Host "Erwartet werden eindeutige, vorhandene Metadatenzeilen (Code, mod.json, CHANGELOG, AGENTS.md, README.md, docs/compatibility.md)." -ForegroundColor DarkGray
    exit 2
}

Write-Host "=== bump-version v$Version -> $($targets -join ', ') $(if($DryRun){'(DryRun)'}) ===" -ForegroundColor Cyan

$pending = @($plans | Where-Object { $_.Writes.Count -gt 0 })
if ($DryRun) {
    foreach ($p in $pending) {
        $prefix = if ($p.IsTransition) { "[DryRun] [$($p.Mod)] v$($p.Old) -> v$($p.New)" } else { "[DryRun] [$($p.Mod)] same version" }
        Write-Host $prefix -ForegroundColor Yellow
        foreach ($c in $p.Changes) { Write-Host "  - $c" -ForegroundColor Gray }
    }
    if ($pending.Count -eq 0) { Write-Host "[DryRun] Keine Änderungen." -ForegroundColor DarkGray }
    Write-Host ""
    Write-Host "DryRun abgeschlossen: $($pending.Count) Mod(s) würden geändert. Es wurde nichts geschrieben." -ForegroundColor Magenta
    exit 0
}

foreach ($entry in $fileContents.GetEnumerator()) {
    [System.IO.File]::WriteAllText($entry.Key, $entry.Value, $utf8NoBom)
}

Write-Host ""
$totalChanged = $pending.Count
if ($totalChanged -eq 0) {
    Write-Host "Keine Änderungen: alle Ziele bereits auf v$Version." -ForegroundColor DarkGray
} else {
    Write-Host "Fertig: $totalChanged Mod(s) aktualisiert auf v$Version." -ForegroundColor Green
    Write-Host "Hinweis: Danach 'pwsh Tools/gen-sln.ps1' nur wenn Mod hinzugefügt/entfernt wurde; Version-Bumps brauchen keinen SLN-Rebuild." -ForegroundColor DarkGray
}
exit 0