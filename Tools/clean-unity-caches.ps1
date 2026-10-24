#Requires -Version 7
<#
.SYNOPSIS
Lists disposable caches in old WorldLook Unity build runs; -Apply removes only those caches.
.DESCRIPTION
No path parameter is accepted. The repository is derived from this script's Tools directory.
Recognized runs are build/logs/yyyyMMdd-HHmmss-fff-xxxxxxxx/unity/Library and
build/logs/p-xxxxxxxx/Library. The working unity/Library, backups and all unlisted
children (including Bee, BuildPlayerData, ScriptAssemblies, reports and traces) remain.
Diagnostic extensions and known build/preview receipts found inside an otherwise
allowlisted cache abort preflight; a cache name never authorizes deleting that evidence.
Apply hashes protected WorldLook/backup files before and after, and retains a receipt
under .scratch/unity-cache-cleanup. Failures can leave partial cache removal; no rollback.
.EXAMPLE
pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/clean-unity-caches.ps1
.EXAMPLE
pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/clean-unity-caches.ps1 -Apply -WhatIf
.EXAMPLE
pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/clean-unity-caches.ps1 -Apply
#>
[CmdletBinding(SupportsShouldProcess)]
param([switch]$Apply)

function Assert-S1CachePlainPath {
    param([string]$Root, [string]$Path)
    $full = [IO.Path]::GetFullPath($Path)
    $prefix = $Root.TrimEnd('\', '/') + [IO.Path]::DirectorySeparatorChar
    if ($full -ne $Root -and -not $full.StartsWith($prefix, [StringComparison]::OrdinalIgnoreCase)) {
        throw "Path outside the repository: $full"
    }
    $cursor = $full
    while ($cursor) {
        $item = Get-Item -LiteralPath $cursor -Force -ErrorAction Stop
        if ($item.Attributes -band [IO.FileAttributes]::ReparsePoint) { throw "Reparse path refused: $cursor" }
        $parent = Split-Path -Parent $cursor
        if ($parent -eq $cursor) { break }
        $cursor = $parent
    }
    return $full
}

function Get-S1CacheTree {
    param([string]$Root, [string]$Path, [string[]]$Excluded = @())
    # Enumerate one level at a time: never traverse a link before checking it.
    $pending = [Collections.Generic.Stack[string]]::new()
    $pending.Push($Path)
    while ($pending.Count) {
        $next = $pending.Pop()
        if ($Excluded -contains $next) { continue }
        $item = Get-Item -LiteralPath $next -Force -ErrorAction Stop
        if ($item.Attributes -band [IO.FileAttributes]::ReparsePoint) { throw "Reparse child refused: $next" }
        $item
        if ($item.PSIsContainer) {
            foreach ($child in @(Get-ChildItem -LiteralPath $next -Force -ErrorAction Stop)) { $pending.Push($child.FullName) }
        }
    }
}

function Get-S1CacheActiveProcesses {
    @(Get-Process -ErrorAction Stop | Where-Object {
        $_.ProcessName -match '^(Unity|UnityShaderCompiler|AssetImportWorker|Schedule I)$'
    })
}

function Assert-S1CacheIdle {
    $active = @(Get-S1CacheActiveProcesses)
    if ($active.Count) { throw ('Active Unity/game processes: ' + (($active | ForEach-Object ProcessName) -join ', ')) }
}

function Get-S1CacheTargets {
    param([string]$Root, [string]$Logs, [string]$OnlyPath)
    if (-not (Test-Path -LiteralPath $Logs)) { return }
    $null = Assert-S1CachePlainPath $Root $Logs
    $directoryNames = @('Artifacts', 'BurstCache', 'PackageCache', 'ShaderCache', 'TempArtifacts')
    $fileNames = @('ArtifactDB', 'ArtifactDB-lock', 'SourceAssetDB', 'SourceAssetDB-lock', 'ShaderCache.db')
    foreach ($run in @(Get-ChildItem -LiteralPath $Logs -Force | Sort-Object Name)) {
        $relative = $null
        if ($run.Name -cmatch '^\d{8}-\d{6}-\d{3}-[0-9a-f]{8}$') {
            $parsed = [DateTime]::MinValue
            if ([DateTime]::TryParseExact($run.Name.Substring(0, 19), 'yyyyMMdd-HHmmss-fff',
                    [Globalization.CultureInfo]::InvariantCulture, [Globalization.DateTimeStyles]::None, [ref]$parsed)) {
                $relative = 'unity/Library'
            }
        }
        elseif ($run.Name -cmatch '^p-[0-9a-f]{8}$') { $relative = 'Library' }
        if (-not $relative) { continue }
        $null = Assert-S1CachePlainPath $Root $run.FullName
        if (-not $run.PSIsContainer) { throw "Run is not a directory: $($run.FullName)" }
        $library = Join-Path $run.FullName $relative
        if (-not (Test-Path -LiteralPath $library)) { continue }
        $null = Assert-S1CachePlainPath $Root $library
        if (-not (Test-Path -LiteralPath $library -PathType Container)) { throw "Library is not a directory: $library" }
        foreach ($name in ($directoryNames + $fileNames)) {
            $path = Join-Path $library $name
            if ($OnlyPath -and $path -ne $OnlyPath) { continue }
            if (-not (Test-Path -LiteralPath $path)) { continue }
            $null = Assert-S1CachePlainPath $Root $path
            $tree = @(Get-S1CacheTree $Root $path)
            $isDirectory = (Get-Item -LiteralPath $path -Force).PSIsContainer
            if ($isDirectory -ne ($directoryNames -contains $name)) { throw "Unexpected cache entry type: $path" }
            $files = @($tree | Where-Object { -not $_.PSIsContainer })
            foreach ($file in $files) {
                # These are evidence, even if a future Unity/tool version places them
                # in an otherwise disposable cache. Do not move or delete them here.
                if ($file.Name -match '\.(log|binlog|buildreport|traceevents|dmp|crash)$' -or
                    $file.Name -in @('shader-build.json', 'build-info.json', 'preview.json',
                        'inputs.json', 'shader-inputs.json', 'shader-check.json', 'receipt.json', 'BuildReport.json')) {
                    throw "Diagnostic/report file inside cache; preserve and review manually: $($file.FullName)"
                }
            }
            [long]$bytes = 0
            foreach ($file in $files) { $bytes += $file.Length }
            $stamp = ($tree | Sort-Object FullName | ForEach-Object {
                '{0}|{1}|{2}|{3}' -f $_.FullName, $_.PSIsContainer, $(if ($_.PSIsContainer) { 0 } else { $_.Length }), $_.LastWriteTimeUtc.Ticks
            }) -join "`n"
            [pscustomobject]@{
                Path = $path; Library = $library; Files = $files.Count; Bytes = $bytes
                Stamp = [Convert]::ToHexString([Security.Cryptography.SHA256]::HashData([Text.Encoding]::UTF8.GetBytes($stamp)))
            }
        }
    }
}

function Get-S1CacheProtectedSnapshot {
    param([string]$Root, [object[]]$Targets)
    $excluded = @($Targets | ForEach-Object Path)
    foreach ($relative in @('Source/Mods/WorldLook', '.backups')) {
        $path = Join-Path $Root $relative
        if (-not (Test-Path -LiteralPath $path)) { continue }
        $null = Assert-S1CachePlainPath $Root $path
        foreach ($file in @(Get-S1CacheTree $Root $path $excluded)) {
            if ($file.PSIsContainer) { continue }
            [pscustomobject]@{
                Path = $file.FullName; Bytes = $file.Length; ModifiedUtcTicks = $file.LastWriteTimeUtc.Ticks
                SHA256 = (Get-FileHash -LiteralPath $file.FullName -Algorithm SHA256 -ErrorAction Stop).Hash
            }
        }
    }
}

function Compare-S1CacheProtectedSnapshot {
    param([object[]]$Before, [object[]]$After)
    $index = @{}
    foreach ($file in $After) { $index[$file.Path] = $file }
    foreach ($file in $Before) {
        if (-not $index.ContainsKey($file.Path)) { 'Missing: ' + $file.Path; continue }
        $actual = $index[$file.Path]
        if ($file.SHA256 -cne $actual.SHA256 -or $file.Bytes -ne $actual.Bytes -or $file.ModifiedUtcTicks -ne $actual.ModifiedUtcTicks) {
            'Changed: ' + $file.Path
        }
        $index.Remove($file.Path)
    }
    foreach ($path in $index.Keys) { 'Added: ' + $path }
}

function Remove-S1CacheEntry {
    param([string]$Root, [string]$Path, [bool]$Directory)
    $null = Assert-S1CachePlainPath $Root $Path
    # Never recursively delete: unexpected new contents make Directory.Delete fail closed.
    if ($Directory) { [IO.Directory]::Delete($Path, $false) }
    else { [IO.File]::Delete($Path) }
    if (Test-Path -LiteralPath $Path) { throw "Cache entry remains: $Path" }
}

function Write-S1CacheReceipt {
    param([string]$Path, $Receipt)
    [IO.File]::WriteAllText($Path, ($Receipt | ConvertTo-Json -Depth 10), [Text.UTF8Encoding]::new($false))
}

function Invoke-S1UnityCacheCleanup {
    [CmdletBinding(SupportsShouldProcess)]
    param([switch]$Apply)
    Set-StrictMode -Version Latest
    $ErrorActionPreference = 'Stop'
    $root = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..')).TrimEnd('\', '/')
    $null = Assert-S1CachePlainPath $root $root
    foreach ($marker in @('AGENTS.md', 'Source/Mods/WorldLook/build/Build.ps1', 'Source/Mods/WorldLook/src/WorldLook.csproj')) {
        $path = Join-Path $root $marker
        $null = Assert-S1CachePlainPath $root $path
        if (-not (Test-Path -LiteralPath $path -PathType Leaf)) { throw "Repository marker is not a file: $path" }
    }
    Assert-S1CacheIdle
    $logs = Join-Path $root 'Source/Mods/WorldLook/build/logs'
    $targets = @(Get-S1CacheTargets $root $logs)
    [long]$bytes = 0
    [long]$files = 0
    foreach ($target in $targets) {
        $bytes += $target.Bytes; $files += $target.Files
        Write-Host ('{0,12:N0} bytes  {1,7:N0} files  {2}' -f $target.Bytes, $target.Files, [IO.Path]::GetRelativePath($root, $target.Path))
    }
    $result = [pscustomobject]@{ Status = 'Preview'; Targets = $targets; Files = $files; Bytes = $bytes; Receipt = $null }
    Write-Host ('{0} cache entries; {1:N0} files; {2:N0} bytes. Bee, BuildPlayerData, ScriptAssemblies and all unlisted paths are retained.' -f $targets.Count, $files, $bytes)
    if (-not $Apply) { return $result }
    if (-not $targets.Count) { $result.Status = 'NoChanges'; return $result }
    if (-not $PSCmdlet.ShouldProcess($logs, "Remove $($targets.Count) allowlisted cache entries ($bytes bytes); retain evidence and working Library")) {
        $result.Status = 'NotApplied'; return $result
    }

    # Validate and hash all protected trees before any deletion or receipt write.
    $before = @(Get-S1CacheProtectedSnapshot $root $targets)
    $fresh = @(Get-S1CacheTargets $root $logs)
    if (($fresh | ConvertTo-Json -Depth 4 -Compress) -cne ($targets | ConvertTo-Json -Depth 4 -Compress)) {
        throw 'Cache plan changed during preflight; nothing removed.'
    }
    Assert-S1CacheIdle
    $receiptRoot = $root
    foreach ($part in @('.scratch', 'unity-cache-cleanup', ([DateTime]::UtcNow.ToString('yyyyMMdd-HHmmss-fff') + '-' + [guid]::NewGuid().ToString('N')))) {
        $receiptRoot = Join-Path $receiptRoot $part
        if (-not (Test-Path -LiteralPath $receiptRoot)) { $null = New-Item -ItemType Directory -Path $receiptRoot }
        $null = Assert-S1CachePlainPath $root $receiptRoot
    }
    $receiptPath = Join-Path $receiptRoot 'receipt.json'
    $result.Receipt = $receiptPath
    $entries = @($targets | ForEach-Object { [pscustomobject]@{ Path = $_.Path; Files = $_.Files; Bytes = $_.Bytes; Status = 'Pending'; Error = $null } })
    $receipt = [ordered]@{
        StartedUtc = [DateTime]::UtcNow.ToString('o'); CompletedUtc = $null; Status = 'Applying'; Root = $root
        PlannedFiles = $files; PlannedBytes = $bytes; CompletedTargetFiles = 0L; CompletedTargetBytes = 0L
        Targets = $entries; ProtectedFiles = $before.Count; ProtectedCheck = 'Pending'; ProtectedDifferences = @()
        Error = $null; Limit = 'No rollback. Completed-target totals exclude any partially removed failing target. Close Unity/game throughout; concurrent filesystem mutation is unsupported.'
    }
    Write-S1CacheReceipt (Join-Path $receiptRoot 'protected.before.json') $before
    Write-S1CacheReceipt $receiptPath $receipt
    $failure = $null
    try {
        for ($i = 0; $i -lt $targets.Count; $i++) {
            Assert-S1CacheIdle
            $target = $targets[$i]
            # All targets passed preflight. Recheck this complete tree before mutation;
            # avoid repeatedly walking every other large cache for each target.
            $current = @(Get-S1CacheTargets $root $logs -OnlyPath $target.Path)
            if ($current.Count -ne 1 -or $current[0].Stamp -cne $target.Stamp) { throw 'Cache plan changed after preflight.' }
            $entries[$i].Status = 'Removing'
            Write-S1CacheReceipt $receiptPath $receipt
            $tree = @(Get-S1CacheTree $root $target.Path)
            foreach ($item in @($tree | Where-Object { -not $_.PSIsContainer })) { Remove-S1CacheEntry $root $item.FullName $false }
            foreach ($item in @($tree | Where-Object PSIsContainer | Sort-Object { $_.FullName.Length } -Descending)) { Remove-S1CacheEntry $root $item.FullName $true }
            $entries[$i].Status = 'Removed'
            $receipt.CompletedTargetFiles += $target.Files
            $receipt.CompletedTargetBytes += $target.Bytes
            Write-S1CacheReceipt $receiptPath $receipt
        }
    }
    catch {
        $failure = $_.Exception.Message
        foreach ($entry in $entries) {
            if ($entry.Status -eq 'Removing') { $entry.Status = 'FailedPartialPossible'; $entry.Error = $failure }
        }
    }
    try {
        # Exclude original cache roots even after failure; surviving cache bytes are not protected evidence.
        $after = @(Get-S1CacheProtectedSnapshot $root $targets)
        $receipt.ProtectedDifferences = @(Compare-S1CacheProtectedSnapshot $before $after)
        $receipt.ProtectedCheck = if ($receipt.ProtectedDifferences.Count) { 'Mismatch' } else { 'Passed' }
        Write-S1CacheReceipt (Join-Path $receiptRoot 'protected.after.json') $after
        if ($receipt.ProtectedDifferences.Count -and -not $failure) { $failure = 'Protected-file preservation mismatch.' }
    }
    catch {
        $receipt.ProtectedCheck = 'Failed'
        if (-not $failure) { $failure = 'Protected-file check failed: ' + $_.Exception.Message }
        else { $failure += '; protected-file check failed: ' + $_.Exception.Message }
    }
    $receipt.CompletedUtc = [DateTime]::UtcNow.ToString('o')
    $receipt.Error = $failure
    $receipt.Status = if ($failure) { 'Failed' } else { 'Completed' }
    Write-S1CacheReceipt $receiptPath $receipt
    if ($failure) { throw "Cleanup failed; partial removal is possible, no rollback. $failure Receipt: $receiptPath" }
    $result.Status = 'Completed'
    Write-Host "Protected files unchanged; receipt: $receiptPath"
    return $result
}

if ($MyInvocation.InvocationName -ne '.') {
    try { Invoke-S1UnityCacheCleanup -Apply:$Apply | Format-List Status, Files, Bytes, Receipt; exit 0 }
    catch { Write-Error $_; exit 1 }
}
