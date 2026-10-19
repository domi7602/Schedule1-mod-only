[CmdletBinding()]
param(
    [string]$GamePath,
    [string]$Branch,
    [string]$LogPath,
    [string]$OutputDirectory,
    [switch]$Interactive
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$Utf8 = New-Object Text.UTF8Encoding($false)

function Assert-NoLinkedAncestor([string]$Path) {
    $cursor = [IO.Path]::GetFullPath($Path)
    while ($cursor) {
        if ((Test-Path -LiteralPath $cursor) -and ((Get-Item -LiteralPath $cursor).Attributes -band [IO.FileAttributes]::ReparsePoint)) {
            throw 'Ausgewählte Pfade dürfen keine symbolischen Links oder Verzeichnisverknüpfungen enthalten. Bitte den tatsächlichen Zielpfad angeben.'
        }
        $parent = [IO.Directory]::GetParent($cursor)
        if ($null -eq $parent) { break }
        $cursor = $parent.FullName
    }
}

try {
    if ($Interactive) {
        if (-not $GamePath) { $GamePath = Read-Host 'Vollständiger Ordner der tatsächlich gestarteten Schedule-I-Installation' }
        if (-not $Branch) { $Branch = Read-Host 'Tatsächlich gestarteter Steam-Zweig (z. B. default oder beta; nicht raten)' }
        if (-not $LogPath) { $LogPath = Read-Host 'Expliziter Pfad zur aktuellen MelonLoader/Latest.log (optional; leer = keine Logdatei lesen)' }
    }
    if (-not $GamePath -or -not $Branch) { throw 'GamePath und Branch müssen ausdrücklich angegeben werden.' }
    if ($Branch -notmatch '^[A-Za-z0-9._ -]{1,64}$') { throw 'Branch darf nur einen kurzen Zweignamen enthalten; keine weiteren persönlichen Angaben.' }
    $GamePath = [IO.Path]::GetFullPath($GamePath.Trim('"'))
    if ($GamePath -eq [IO.Path]::GetPathRoot($GamePath)) { throw 'Bitte den konkreten Spielordner angeben, kein ganzes Laufwerk.' }
    $GamePath = $GamePath.TrimEnd([char[]]'\/')
    Assert-NoLinkedAncestor $GamePath
    if (-not (Test-Path -LiteralPath $GamePath -PathType Container)) { throw 'Der angegebene Spielordner existiert nicht.' }
    if ((Get-Item -LiteralPath $GamePath).Attributes -band [IO.FileAttributes]::ReparsePoint) { throw 'Bitte den tatsächlichen Spielordner statt eines Verzeichnislinks angeben.' }
    if (-not $OutputDirectory) { $OutputDirectory = Join-Path ([IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))) ('LightControl-Diagnostics-' + (Get-Date).ToUniversalTime().ToString('yyyyMMddTHHmmssZ') + '-' + [Guid]::NewGuid().ToString('N').Substring(0, 6)) }
    $OutputDirectory = [IO.Path]::GetFullPath($OutputDirectory)
    Assert-NoLinkedAncestor $OutputDirectory
    if ($OutputDirectory.Equals($GamePath, [StringComparison]::OrdinalIgnoreCase) -or $OutputDirectory.StartsWith($GamePath + [IO.Path]::DirectorySeparatorChar, [StringComparison]::OrdinalIgnoreCase)) { throw 'Die Ausgabe muss außerhalb der Spielinstallation liegen.' }
    if (Test-Path -LiteralPath $OutputDirectory) { throw 'Der Ausgabeordner existiert bereits. Bitte einen neuen Ordner verwenden.' }
    # Only these immediate conventional locations are inspected; no recursion/global search.
    $locations = @(
        [ordered]@{ relative = '.'; filter = @('GameAssembly.dll', 'UnityPlayer.dll') },
        [ordered]@{ relative = 'MelonLoader/net6'; filter = @('*.dll') },
        [ordered]@{ relative = 'MelonLoader/Il2CppAssemblies'; filter = @('*.dll') },
        [ordered]@{ relative = 'Mods'; filter = @('S1API*.dll') },
        [ordered]@{ relative = 'Plugins'; filter = @('S1API*.dll') },
        [ordered]@{ relative = 'UserLibs'; filter = @('S1API*.dll') }
    )
    $files = @()
    $checkedLocations = @()
    foreach ($location in $locations) {
        $directory = Join-Path $GamePath $location.relative
        $exists = Test-Path -LiteralPath $directory -PathType Container
        $linked = $false
        $walk = $GamePath
        foreach ($part in $location.relative.Split('/')) {
            if ($part -eq '.') { continue }
            $walk = Join-Path $walk $part
            if ((Test-Path -LiteralPath $walk) -and ((Get-Item -LiteralPath $walk).Attributes -band [IO.FileAttributes]::ReparsePoint)) { $linked = $true }
        }
        $checkedLocations += [ordered]@{ relative = $location.relative; exists = $exists; skipped_link = $linked }
        if (-not $exists -or $linked) { continue }
        $entries = @(Get-ChildItem -LiteralPath $directory -File | Where-Object {
            $name = $_.Name
            $matched = $false
            foreach ($pattern in $location.filter) { if ($name -like $pattern) { $matched = $true } }
            $matched
        })
        foreach ($file in $entries | Sort-Object Name) {
            if ($file.Attributes -band [IO.FileAttributes]::ReparsePoint) { continue }
            $entry = [ordered]@{
                path = $file.FullName.Substring($GamePath.Length + 1).Replace('\', '/')
                bytes = $file.Length; sha256 = $null; assembly_name = $null; assembly_version = $null
                file_version = $null; metadata_status = 'not-read'
            }
            try {
                $entry.sha256 = (Get-FileHash -LiteralPath $file.FullName -Algorithm SHA256).Hash.ToLowerInvariant()
                # GetAssemblyName reads identity metadata. No Assembly.Load/LoadFrom or game code execution.
                try {
                    $identity = [Reflection.AssemblyName]::GetAssemblyName($file.FullName)
                    $entry.assembly_name = $identity.Name
                    $entry.assembly_version = $identity.Version.ToString()
                    $entry.metadata_status = 'managed-identity-read'
                } catch { $entry.metadata_status = 'not-managed-or-unreadable' }
                $version = [Diagnostics.FileVersionInfo]::GetVersionInfo($file.FullName).FileVersion
                if ($version -and $version -match '^\s*[0-9][0-9A-Za-z.+ -]{0,70}\s*$') { $entry.file_version = $version.Trim() }
            } catch { $entry.metadata_status = 'read-failed'; $entry.sha256 = $null }
            $files += $entry
        }
    }
    $facts = @()
    $logRead = $false
    $logLimit = 5000
    if ($LogPath) {
        $LogPath = [IO.Path]::GetFullPath($LogPath.Trim('"'))
        Assert-NoLinkedAncestor $LogPath
        if (-not (Test-Path -LiteralPath $LogPath -PathType Leaf)) { throw 'Die ausdrücklich angegebene Logdatei existiert nicht.' }
        if ((Get-Item -LiteralPath $LogPath).Attributes -band [IO.FileAttributes]::ReparsePoint) { throw 'Die Logdatei darf kein Link sein.' }
        $lineNumber = 0
        # Reconstruct only whitelisted labels and numeric version tokens; never copy raw log lines.
        foreach ($line in Get-Content -LiteralPath $LogPath -TotalCount $logLimit) {
            $lineNumber++
            $versionPattern = '(?i)\b(MelonLoader|S1API|Unity(?: Version)?|Game Version)\b[\s\]:=-]*(?:version\s*[:= ]*)?v?(\d{1,4}\.\d{1,4}(?:\.\d{1,5})?(?:[abfp]\d{1,4})?)(?![A-Za-z0-9.])'
            foreach ($match in [regex]::Matches($line, $versionPattern)) {
                $facts += "Zeile ${lineNumber}: $($match.Groups[1].Value); Versionstoken $($match.Groups[2].Value)."
            }
            $runtimeMatch = [regex]::Match($line, '(?i)\bGame Type\b[\s\]:=-]*(il2cpp|mono)\b')
            if ($runtimeMatch.Success) { $facts += "Zeile ${lineNumber}: Game Type; Laufzeitkennzeichen $($runtimeMatch.Groups[1].Value.ToLowerInvariant())." }
        }
        $logRead = $true
    }
    New-Item -ItemType Directory -Path $OutputDirectory -ErrorAction Stop | Out-Null
    $report = [ordered]@{
        schema_version = 1; captured_utc = (Get-Date).ToUniversalTime().ToString('o')
        branch_user_declared = $Branch; installation_versions_verified = $false
        scope = 'explicit-game-path; conventional immediate reference directories only; no DLLs copied'
        checked_locations = $checkedLocations; files = $files
        log = [ordered]@{ explicitly_provided = $logRead; max_lines_inspected = $logLimit; raw_lines_copied = 0; fact_count = @($facts).Count }
    }
    [IO.File]::WriteAllText((Join-Path $OutputDirectory 'reference-inventory.json'), ($report | ConvertTo-Json -Depth 10), $Utf8)
    $excerpt = @('REDIGIERTER STARTLOG-AUSZUG', 'Nur bekannte Produktbezeichnungen, numerische Versionstoken und Laufzeitkennzeichen.', 'Zeilennummern beziehen sich auf die lokal ausdrücklich ausgewählte Logdatei.', 'Keine Originalzeilen, Benutzernamen, Installationspfade oder Sitzungsdaten werden übernommen.', '') + $facts
    if (-not $logRead) { $excerpt += 'Keine Logdatei ausgewählt; deshalb keine Laufzeitversionsbestätigung.' }
    if ($logRead -and @($facts).Count -eq 0) { $excerpt += 'Keine passenden Angaben erkannt. Dies ist keine Bestätigung von Versionen oder erfolgreichem Mod-Laden.' }
    [IO.File]::WriteAllLines((Join-Path $OutputDirectory 'log-excerpt-redacted.txt'), [string[]]$excerpt, $Utf8)
    $readme = @(
        'LIGHTCONTROL: KLEINES DIAGNOSEPAKET',
        'Das Spiel und seine DLLs wurden nicht gestartet oder verändert.',
        'Enthalten sind nur selbst erzeugte Referenzmetadaten und ein auf sichere Fakten beschränkter Logauszug.',
        'Es wurden keine Spiel-DLLs, Spielstände, Konfigurationen oder Original-Logs kopiert.',
        'Prüfe die beiden Dateien vor dem Hochladen. Die Metadaten sind noch kein Laufzeit-Funktionsnachweis.',
        'Sende zunächst reference-inventory.json und log-excerpt-redacted.txt. Weitere Dateien erst nach gezielter Auswahl.',
        'Die konventionellen Pfade können je Installation abweichen. Fehlende Einträge werden als fehlend gemeldet, nicht global gesucht.'
    )
    [IO.File]::WriteAllLines((Join-Path $OutputDirectory 'README_DIAGNOSTICS.txt'), [string[]]$readme, $Utf8)
    Write-Host "Diagnosedateien erstellt: $OutputDirectory"
    Write-Host 'Prüfe reference-inventory.json und log-excerpt-redacted.txt und sende nur diese beiden Dateien zurück.'
    exit 0
} catch {
    Write-Error -Message $_.Exception.Message -ErrorAction Continue
    exit 1
}
