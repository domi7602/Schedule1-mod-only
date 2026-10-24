#Requires -Version 7
# Dot-source only. These helpers neither build nor publish.

function Assert-S1ReleaseModName {
    param([Parameter(Mandatory)][string]$Mod)
    if ($Mod -notmatch '^[A-Za-z0-9][A-Za-z0-9_-]*$' -or $Mod -in @('All', 'Shared', '_DiagPerfCounter')) {
        throw "Invalid release mod name '$Mod'."
    }
}

function Read-S1ReleaseMetadata {
    param([Parameter(Mandatory)][string]$Path, [Parameter(Mandatory)][string]$Mod)
    Assert-S1ReleaseModName $Mod
    if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) { throw "Required mod metadata is missing: $Path" }
    try { $metadata = Get-Content -LiteralPath $Path -Raw | ConvertFrom-Json -ErrorAction Stop }
    catch { throw "Invalid mod metadata at '$Path': $($_.Exception.Message)" }
    Assert-S1ReleaseMetadataObject -Metadata $metadata -Mod $Mod
    return $metadata
}

function Assert-S1ReleaseMetadataObject {
    param($Metadata, [string]$Mod, [string]$Version)
    if ($null -eq $Metadata -or $Metadata.name -cne $Mod -or
        $Metadata.version -isnot [string] -or
        $Metadata.version -notmatch '^(0|[1-9]\d*)\.(0|[1-9]\d*)\.(0|[1-9]\d*)(?:-[0-9A-Za-z-]+(?:\.[0-9A-Za-z-]+)*)?(?:\+[0-9A-Za-z-]+(?:\.[0-9A-Za-z-]+)*)?$') {
        throw "Invalid mod metadata: expected name '$Mod' and a semantic version."
    }
    if ($Version -and $Metadata.version -cne $Version) { throw "Metadata version does not match '$Version'." }
}

function Get-S1ReleaseTextHash {
    param([Parameter(Mandatory)][string]$Text)
    return [Convert]::ToHexString([Security.Cryptography.SHA256]::HashData([Text.Encoding]::UTF8.GetBytes($Text))).ToLowerInvariant()
}

function Invoke-S1ReleaseGit {
    param([Parameter(Mandatory)][string]$Root, [Parameter(Mandatory)][string[]]$Arguments)
    $output = & git -C $Root @Arguments 2>&1
    if ($LASTEXITCODE -ne 0) { throw "git $($Arguments -join ' ') failed: $($output -join ' ')" }
    return ($output -join "`n").TrimEnd("`r", "`n")
}

function Assert-S1ReleaseIgnoredInputs {
    param([Parameter(Mandatory)][string]$Root)
    # Git-clean is insufficient for SDK wildcard inputs: *.tmp.cs and an ignored
    # src/scratch/Hidden.cs still compile. Reject invisible production inputs rather
    # than claim a clean artifact. All src extensions are covered, including extra
    # files/resources; bin/obj are the SDK-generated output boundary.
    $ignored = Invoke-S1ReleaseGit $Root @('ls-files', '-z', '--others', '--ignored', '--exclude-standard', '--',
        'Source/Mods/*/src/**', 'Source/Mods/*/assets/**', 'Source/Mods/*.props', 'Source/Mods/*.targets',
        'Source/Mods/.editorconfig', 'Source/Mods/*/.editorconfig')
    $unexpected = @($ignored.Split([char]0, [StringSplitOptions]::RemoveEmptyEntries) | Where-Object {
        $path = $_.Replace('\', '/')
        $sourceInput = $path -match '^Source/Mods/[^/]+/src/' -and $path -notmatch '^Source/Mods/[^/]+/src/(bin|obj)/'
        $runtimeAsset = $path -match '^Source/Mods/[^/]+/assets/.*\.(png|bundle|glb)$' -or
            $path -match '^Source/Mods/[^/]+/assets/embedded/'
        $buildControl = $path -match '^Source/Mods/([^/]+/)?[^/]+\.(props|targets)$' -or
            $path -match '^Source/Mods/([^/]+/)?\.editorconfig$'
        $sourceInput -or $runtimeAsset -or $buildControl
    })
    if ($unexpected.Count) {
        throw "Ignored production input(s) cannot be certified for release: $($unexpected -join ', '). Track the intended inputs or move them outside the production source/assets tree."
    }
}

function Get-S1ReleaseSourceIdentity {
    param([Parameter(Mandatory)][string]$Root, [int]$Depth = 0)
    if ($Depth -gt 8) { throw 'Submodule nesting exceeds the release identity limit.' }
    $rootPath = [IO.Path]::GetFullPath($Root)
    $head = Invoke-S1ReleaseGit $rootPath @('rev-parse', '--verify', 'HEAD')
    if ($head -notmatch '^[0-9a-f]{40,64}$') { throw 'Cannot establish the source HEAD.' }
    $submodules = Invoke-S1ReleaseGit $rootPath @('submodule', 'status', '--recursive')
    if ($submodules -match '(?m)^[-U]') { throw 'Release source has uninitialized or conflicted submodules.' }
    Assert-S1ReleaseIgnoredInputs $rootPath
    $status = Invoke-S1ReleaseGit $rootPath @('status', '--porcelain=v1', '-z', '--untracked-files=all', '--ignore-submodules=none')
    $paths = Invoke-S1ReleaseGit $rootPath @('ls-files', '-z', '--cached', '--others', '--exclude-standard')
    $entries = [Collections.Generic.List[object]]::new()
    foreach ($relative in @($paths.Split([char]0, [StringSplitOptions]::RemoveEmptyEntries) | Sort-Object -Unique)) {
        $path = [IO.Path]::GetFullPath((Join-Path $rootPath $relative))
        if (-not $path.StartsWith($rootPath.TrimEnd('\', '/') + [IO.Path]::DirectorySeparatorChar, [StringComparison]::OrdinalIgnoreCase)) {
            throw "Source input escapes repository: $relative"
        }
        if (-not (Test-Path -LiteralPath $path)) {
            $entries.Add([ordered]@{ Path = $relative; Missing = $true })
            continue
        }
        $item = Get-Item -LiteralPath $path -Force
        if ($item.Attributes -band [IO.FileAttributes]::ReparsePoint) { throw "Reparse source inputs are not supported for release identity: $relative" }
        if ($item.PSIsContainer) {
            if (-not (Test-Path -LiteralPath (Join-Path $path '.git'))) { throw "Submodule is not initialized: $relative" }
            $child = Get-S1ReleaseSourceIdentity -Root $path -Depth ($Depth + 1)
            $entries.Add([ordered]@{ Path = $relative; Source = $child })
        } else {
            $entries.Add([ordered]@{ Path = $relative; Sha256 = (Get-FileHash -LiteralPath $path -Algorithm SHA256).Hash.ToLowerInvariant() })
        }
    }
    $fingerprint = Get-S1ReleaseTextHash (ConvertTo-Json -InputObject @($entries.ToArray()) -Depth 20 -Compress)
    return [pscustomobject][ordered]@{ Head = $head; Dirty = [bool]$status; Fingerprint = $fingerprint }
}

function Assert-S1ReleaseSourceIdentity {
    param($Actual, $Expected, [switch]$RequireClean)
    foreach ($source in @($Actual, $Expected)) {
        if ($null -eq $source -or $source.Head -notmatch '^[0-9a-f]{40,64}$' -or
            $source.Fingerprint -notmatch '^[0-9a-f]{64}$' -or $source.Dirty -isnot [bool]) {
            throw 'Missing or invalid release source identity.'
        }
        if ($RequireClean -and $source.Dirty) { throw 'Publication requires clean source; dirty packages cannot be released.' }
    }
    if ($Actual.Head -cne $Expected.Head -or $Actual.Fingerprint -cne $Expected.Fingerprint -or $Actual.Dirty -ne $Expected.Dirty) {
        throw 'Package source identity differs from current source (HEAD, dirty state or content fingerprint). Repackage explicitly.'
    }
}

function New-S1ReleaseStagingDirectory {
    $parent = [IO.Path]::GetFullPath([IO.Path]::GetTempPath()).TrimEnd('\', '/')
    $path = Join-Path $parent ('s1release-' + [guid]::NewGuid().ToString('N'))
    $null = New-Item -ItemType Directory -Path $path -ErrorAction Stop
    return [pscustomobject]@{ Path = $path; Parent = $parent }
}

function Remove-S1ReleaseStagingDirectory {
    param([Parameter(Mandatory)]$Staging)
    $path = [IO.Path]::GetFullPath($Staging.Path).TrimEnd('\', '/')
    $parent = [IO.Path]::GetFullPath($Staging.Parent).TrimEnd('\', '/')
    $temp = [IO.Path]::GetFullPath([IO.Path]::GetTempPath()).TrimEnd('\', '/')
    if ($parent -ne $temp -or [IO.Path]::GetDirectoryName($path) -ne $parent -or
        [IO.Path]::GetFileName($path) -notmatch '^s1release-[0-9a-f]{32}$') {
        throw 'Refusing cleanup outside the owned release staging directory.'
    }
    if (-not (Test-Path -LiteralPath $path)) { return }
    $items = @((Get-Item -LiteralPath $path -Force)) + @(Get-ChildItem -LiteralPath $path -Recurse -Force)
    if (@($items | Where-Object { $_.Attributes -band [IO.FileAttributes]::ReparsePoint }).Count) {
        throw 'Refusing recursive cleanup through a reparse point.'
    }
    Remove-Item -LiteralPath $path -Recurse -Force
}

function Assert-S1ReleasePayloadPath {
    param([Parameter(Mandatory)][string]$Path)
    if ($Path.Contains('\') -or $Path.Contains(':') -or $Path.StartsWith('/') -or
        @($Path.Split('/') | Where-Object { $_ -in @('', '.', '..') }).Count) {
        throw "Unsafe or ambiguous ZIP payload path: $Path"
    }
}

function Write-S1ReleaseManifest {
    param([Parameter(Mandatory)][string]$StagingRoot, [Parameter(Mandatory)][string]$Mod, $Source)
    Assert-S1ReleaseModName $Mod
    Assert-S1ReleaseSourceIdentity $Source $Source
    $metaPath = Join-Path $StagingRoot "UserData/$Mod/mod.json"
    $metadata = Read-S1ReleaseMetadata $metaPath $Mod
    foreach ($required in @("Mods/$Mod.dll", 'Mods/Shared.dll')) {
        $path = Join-Path $StagingRoot $required
        if (-not (Test-Path -LiteralPath $path -PathType Leaf) -or (Get-Item -LiteralPath $path).Length -le 0) {
            throw "Required package DLL is missing or empty: $required"
        }
    }
    $entries = @(Get-ChildItem -LiteralPath $StagingRoot -Recurse -File | Sort-Object FullName | ForEach-Object {
        $relative = [IO.Path]::GetRelativePath($StagingRoot, $_.FullName).Replace('\', '/')
        Assert-S1ReleasePayloadPath $relative
        if ($relative -eq 'release-manifest.json') { throw 'Staging already contains a release manifest.' }
        [ordered]@{ Path = $relative; Length = $_.Length; Sha256 = (Get-FileHash -LiteralPath $_.FullName -Algorithm SHA256).Hash.ToLowerInvariant() }
    })
    $manifest = [ordered]@{ SchemaVersion = 1; Mod = $Mod; Version = $metadata.version; Source = $Source; Files = $entries }
    $path = Join-Path $StagingRoot 'release-manifest.json'
    [IO.File]::WriteAllText($path, (ConvertTo-Json $manifest -Depth 8), [Text.UTF8Encoding]::new($false))
    return $manifest
}

function Assert-S1ReleaseArchive {
    param([Parameter(Mandatory)][string]$ZipPath, [Parameter(Mandatory)][string]$Mod,
        [Parameter(Mandatory)][string]$Version, $Source, [switch]$RequireClean)
    Assert-S1ReleaseModName $Mod
    if (-not (Test-Path -LiteralPath $ZipPath -PathType Leaf)) { throw "Release ZIP is missing: $ZipPath" }
    $archive = [IO.Compression.ZipFile]::OpenRead($ZipPath)
    try {
        if ($archive.Entries.Count -gt 2048) { throw 'Release ZIP exceeds the 2048-entry limit.' }
        $files = [Collections.Generic.Dictionary[string, object]]::new([StringComparer]::OrdinalIgnoreCase)
        [long]$total = 0
        foreach ($entry in $archive.Entries) {
            $name = $entry.FullName
            if ($name.EndsWith('/')) { Assert-S1ReleasePayloadPath $name.TrimEnd('/'); continue }
            Assert-S1ReleasePayloadPath $name
            if (-not $files.TryAdd($name, $entry)) { throw "Duplicate ZIP entry: $name" }
            $total += $entry.Length
            if ($total -gt 1GB) { throw 'Release ZIP exceeds the 1 GiB uncompressed limit.' }
        }
        if (-not $files.ContainsKey('release-manifest.json')) { throw 'Release ZIP has no provenance manifest; repackage explicitly.' }
        if ($files['release-manifest.json'].Length -gt 1MB) { throw 'Release manifest exceeds 1 MiB.' }
        $reader = [IO.StreamReader]::new($files['release-manifest.json'].Open())
        try { $manifest = $reader.ReadToEnd() | ConvertFrom-Json -ErrorAction Stop } finally { $reader.Dispose() }
        if ($manifest.SchemaVersion -ne 1 -or $manifest.Mod -cne $Mod -or $manifest.Version -cne $Version) {
            throw 'Release manifest schema, mod or version does not match the requested release.'
        }
        Assert-S1ReleaseSourceIdentity $manifest.Source $Source -RequireClean:$RequireClean
        $declared = [Collections.Generic.HashSet[string]]::new([StringComparer]::OrdinalIgnoreCase)
        foreach ($record in @($manifest.Files)) {
            Assert-S1ReleasePayloadPath $record.Path
            if ($record.Path -eq 'release-manifest.json' -or -not $declared.Add($record.Path)) { throw 'Duplicate or self-referencing manifest payload.' }
            if (-not $files.ContainsKey($record.Path)) { throw "Manifest payload missing from ZIP: $($record.Path)" }
            $entry = $files[$record.Path]
            if ($record.Length -ne $entry.Length -or $record.Sha256 -notmatch '^[0-9a-f]{64}$') { throw "Invalid payload length/hash: $($record.Path)" }
            $stream = $entry.Open()
            try { $hash = [Convert]::ToHexString([Security.Cryptography.SHA256]::HashData($stream)).ToLowerInvariant() }
            finally { $stream.Dispose() }
            if ($hash -cne $record.Sha256) { throw "Payload SHA-256 mismatch: $($record.Path)" }
        }
        if ($declared.Count -ne ($files.Count - 1)) { throw 'ZIP contains undeclared payload files.' }
        foreach ($required in @("Mods/$Mod.dll", 'Mods/Shared.dll', "UserData/$Mod/mod.json")) {
            if (-not $declared.Contains($required) -or $files[$required].Length -le 0) { throw "Required package payload is missing or empty: $required" }
        }
        if ($files["UserData/$Mod/mod.json"].Length -gt 1MB) { throw 'Package metadata exceeds 1 MiB.' }
        $reader = [IO.StreamReader]::new($files["UserData/$Mod/mod.json"].Open())
        try { $metadata = $reader.ReadToEnd() | ConvertFrom-Json -ErrorAction Stop } finally { $reader.Dispose() }
        Assert-S1ReleaseMetadataObject $metadata $Mod $Version
        return $manifest
    } finally { $archive.Dispose() }
}
