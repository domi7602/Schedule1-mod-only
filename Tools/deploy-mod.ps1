#Requires -Version 7
<#
.SYNOPSIS
    Deploy one mod project's complete file set and record a verifiable receipt.

.DESCRIPTION
    Replaces the per-file <Copy> flow that used to live in
    Source/Mods/Directory.Build.targets. The plan is derived here, not in MSBuild:

        Mods\<Mod>.dll                       DLL
        UserData\<Mod>\<Mod>.pdb             debug symbols
        UserData\<Mod>\mod.json              metadata
        Mods\*.png                           icons (project dir and assets\)
        Mods\*.bundle                        asset bundles (assets\)
        Mods\<Mod>\*.glb                     meshes (assets\); MelonLoader scans only DLLs

    Order of operations:
      1. acquire the per-install mutex (serializes parallel MSBuild projects);
      2. run Tools/check-deploy-preflight.ps1 with the effective GameDir;
      3. build the plan, reject duplicate destinations and unsafe/reparse targets;
      4. hash every source, hash existing destinations, skip unchanged files;
      5. back up every destination that will be overwritten into
         <GameDir>\UserData\.deploy\backups\<id>\ (files only, never recursive;
         no directory is created when nothing is overwritten);
      6. copy into a unique same-directory temp file through the production
         seam Copy-S1DeployTargetFile, verify its SHA256, then move it into
         place and verify the destination again;
      7. write an attempt-specific receipt next to the latest
         <GameDir>\UserData\.deploy\receipts\<Mod>.json with exact source/
         destination paths, per-entry states, hashes, stale prior-owned files,
         configuration and git revision+dirty state.

    On an in-process failure the copies made by this invocation are rolled back
    from the verified backups: destination/backup ancestors are re-validated,
    each backup is hash-checked before it is restored, files this run created
    are removed only while they still match the copied source hash, and every
    rollback failure is reported per entry. There is no cross-process atomicity
    claim: a crash, kill or power loss between two copies is not recovered
    automatically (the backup directory is left behind for manual recovery).

    Dot-sourcing this file defines the helpers for tests and does not deploy.
    Tests also replace the copy/restore seams to inject controlled failures.

.PARAMETER GameDir
    Schedule I install root. Omitted/empty -> SCHEDULE1_PATH -> Steam default
    (resolved by check-deploy-preflight.ps1).

.PARAMETER ProjectName
    MSBuild project name; also the UserData\<Mod>\ folder name.

.PARAMETER ProjectDir
    Absolute directory of the .csproj (MSBuildProjectDirectory).

.PARAMETER TargetPath
    Absolute path of the built DLL.

.PARAMETER PdbPath
    Optional explicit PDB. Defaults to the DLL path with a .pdb extension.

.PARAMETER AssetsDir
    Optional <mod>\assets directory. Missing directory means "no assets".

.PARAMETER ModJsonPath
    Optional <mod>\docs\mod.json. Missing file means "no metadata".

.PARAMETER Configuration
    Build configuration recorded in the receipt.

.PARAMETER MutexTimeoutSeconds
    How long to wait for a concurrent deploy of the same install.

.PARAMETER NoDeploy
    Skip all writes (also honours S1NoDeploy=true).

.EXAMPLE
    pwsh -NoProfile -File Tools/deploy-mod.ps1 -GameDir "D:\Games\Schedule I" `
        -ProjectName NotesApp -Configuration Release `
        -TargetPath ".build\NotesApp.dll" -ProjectDir "Source\Mods\NotesApp\src"
#>

[CmdletBinding()]
param(
    [string]$GameDir,
    [string]$ProjectName,
    [string]$ProjectDir,
    [string]$TargetPath,
    [string]$PdbPath,
    [string]$AssetsDir,
    [string]$ModJsonPath,
    [string]$Configuration = 'Debug',
    [int]$MutexTimeoutSeconds = 120,
    [switch]$NoDeploy
)

$ErrorActionPreference = 'Stop'

. (Join-Path $PSScriptRoot 'check-deploy-preflight.ps1')

$script:S1DeployMutexPrefix = 'Schedule1Deploy_'
$script:S1DeployRecordDirName = '.deploy'
$script:S1DeployTempPrefix = '.s1deploy-tmp-'
$script:S1DeployWorkspaceRoot = Split-Path -Parent $PSScriptRoot

function Throw-S1ModDeploy {
    param([Parameter(Mandatory)][string]$Message)
    throw [System.InvalidOperationException]::new($Message)
}

function Assert-S1AbsolutePath {
    param(
        [Parameter(Mandatory)][string]$Value,
        [Parameter(Mandatory)][string]$Label
    )
    if ([string]::IsNullOrWhiteSpace($Value)) {
        Throw-S1ModDeploy "$Label is empty."
    }
    if (-not [System.IO.Path]::IsPathRooted($Value)) {
        Throw-S1ModDeploy "$Label must be absolute; got '$Value'."
    }
}

function Test-S1ReparsePoint {
    param([Parameter(Mandatory)][System.IO.FileSystemInfo]$Item)
    return (($Item.Attributes -band [System.IO.FileAttributes]::ReparsePoint) -ne 0)
}

function Get-S1GitState {
    param([Parameter(Mandatory)][string]$WorkspaceRoot)

    $revision = $null
    $dirty = $null
    try {
        $git = Get-Command git -ErrorAction Stop
        $rev = & $git.Source -C $WorkspaceRoot rev-parse HEAD 2>$null
        if ($LASTEXITCODE -eq 0 -and -not [string]::IsNullOrWhiteSpace([string]$rev)) {
            $revision = ([string]$rev).Trim()
        }
        $status = @(& $git.Source -C $WorkspaceRoot status --porcelain 2>$null)
        if ($LASTEXITCODE -eq 0) {
            $dirty = ($status.Count -gt 0)
        }
    }
    catch {
        # Missing git or an unreadable repository leaves both fields null; the
        # receipt says so instead of inventing a revision.
    }

    return [pscustomobject]@{
        Revision = $revision
        Dirty    = $dirty
    }
}

function Get-S1DeployMutexName {
    param([Parameter(Mandatory)][string]$GameDir)

    $normalized = [System.IO.Path]::TrimEndingDirectorySeparator(
        [System.IO.Path]::GetFullPath($GameDir)).ToLowerInvariant()
    $sha = [System.Security.Cryptography.SHA256]::Create()
    try {
        $bytes = $sha.ComputeHash([System.Text.Encoding]::UTF8.GetBytes($normalized))
    }
    finally {
        $sha.Dispose()
    }
    $hex = -join ($bytes[0..7] | ForEach-Object { $_.ToString('x2') })
    return "$($script:S1DeployMutexPrefix)$hex"
}

function New-S1DeployPlan {
    param(
        [Parameter(Mandatory)][string]$ProjectName,
        [Parameter(Mandatory)][string]$ProjectDir,
        [Parameter(Mandatory)][string]$TargetPath,
        [string]$PdbPath,
        [string]$AssetsDir,
        [string]$ModJsonPath
    )

    if ($ProjectName -notmatch '^[A-Za-z0-9][A-Za-z0-9._-]*$' -or $ProjectName.Contains('..') -or
        $ProjectName.EndsWith('.') -or $ProjectName.EndsWith(' ')) {
        Throw-S1ModDeploy "ProjectName '$ProjectName' is not a safe folder/file name."
    }

    Assert-S1AbsolutePath -Value $ProjectDir -Label 'ProjectDir'
    Assert-S1AbsolutePath -Value $TargetPath -Label 'TargetPath'

    $projectDirFull = [System.IO.Path]::TrimEndingDirectorySeparator([System.IO.Path]::GetFullPath($ProjectDir))
    if (-not (Test-Path -LiteralPath $projectDirFull -PathType Container)) {
        Throw-S1ModDeploy "ProjectDir not found: '$projectDirFull'."
    }

    $dll = [System.IO.Path]::GetFullPath($TargetPath)
    if (-not (Test-Path -LiteralPath $dll -PathType Leaf)) {
        Throw-S1ModDeploy "Built DLL not found: '$dll'."
    }

    $plan = [System.Collections.Generic.List[object]]::new()
    $add = {
        param([string]$Kind, [string]$Source, [string]$DestinationRelative)
        $plan.Add([pscustomobject]@{
                Kind               = $Kind
                Source             = $Source
                DestinationRelative = $DestinationRelative
                Destination        = $null
                SourceSha256       = $null
                DestinationSha256  = $null
                Action             = $null
                ExistedBefore      = $false
                BackupPath         = $null
                OriginalSha256     = $null
                State              = 'Pending'
            })
    }

    & $add 'dll' $dll ("Mods\" + [System.IO.Path]::GetFileName($dll))

    if ([string]::IsNullOrWhiteSpace($PdbPath)) {
        $candidatePdb = [System.IO.Path]::ChangeExtension($dll, '.pdb')
    }
    else {
        Assert-S1AbsolutePath -Value $PdbPath -Label 'PdbPath'
        $candidatePdb = [System.IO.Path]::GetFullPath($PdbPath)
    }
    if (Test-Path -LiteralPath $candidatePdb -PathType Leaf) {
        & $add 'pdb' $candidatePdb ("UserData\$ProjectName\" + [System.IO.Path]::GetFileName($candidatePdb))
    }
    elseif (-not [string]::IsNullOrWhiteSpace($PdbPath)) {
        Throw-S1ModDeploy "PDB not found: '$candidatePdb'."
    }

    if (-not [string]::IsNullOrWhiteSpace($ModJsonPath)) {
        Assert-S1AbsolutePath -Value $ModJsonPath -Label 'ModJsonPath'
        $json = [System.IO.Path]::GetFullPath($ModJsonPath)
        # Absent optional metadata is not an error: MSBuild always passes the
        # conventional path, and Shared has no docs/ folder.
        if (Test-Path -LiteralPath $json -PathType Leaf) {
            & $add 'metadata' $json ("UserData\$ProjectName\mod.json")
        }
    }

    $assetDirs = @()
    if (-not [string]::IsNullOrWhiteSpace($AssetsDir)) {
        Assert-S1AbsolutePath -Value $AssetsDir -Label 'AssetsDir'
        $assetDirs += [System.IO.Path]::TrimEndingDirectorySeparator([System.IO.Path]::GetFullPath($AssetsDir))
    }

    foreach ($png in @(Get-ChildItem -LiteralPath $projectDirFull -File -Filter '*.png' -ErrorAction Stop | Sort-Object Name)) {
        & $add 'icon' $png.FullName ("Mods\" + $png.Name)
    }
    foreach ($dir in $assetDirs) {
        if (-not (Test-Path -LiteralPath $dir -PathType Container)) { continue }
        foreach ($png in @(Get-ChildItem -LiteralPath $dir -File -Filter '*.png' -ErrorAction Stop | Sort-Object Name)) {
            & $add 'icon' $png.FullName ("Mods\" + $png.Name)
        }
        foreach ($bundle in @(Get-ChildItem -LiteralPath $dir -File -Filter '*.bundle' -ErrorAction Stop | Sort-Object Name)) {
            & $add 'bundle' $bundle.FullName ("Mods\" + $bundle.Name)
        }
        foreach ($glb in @(Get-ChildItem -LiteralPath $dir -File -Filter '*.glb' -ErrorAction Stop | Sort-Object Name)) {
            & $add 'glb' $glb.FullName ("Mods\$ProjectName\" + $glb.Name)
        }
    }

    $duplicates = @($plan | Group-Object -Property DestinationRelative |
            Where-Object { $_.Count -gt 1 })
    if ($duplicates.Count -gt 0) {
        $details = ($duplicates | ForEach-Object {
                "  $($_.Name): " + (($_.Group | ForEach-Object { $_.Source }) -join ', ')
            }) -join [Environment]::NewLine
        Throw-S1ModDeploy "Ambiguous duplicate destination(s) in the plan. Refusing to guess which source should win.`n$details"
    }

    return @($plan)
}

function Assert-S1DeployDestination {
    <#
        Validates one destination and creates its parent directories. Every
        existing component between the game root and the file must be a real
        directory; an existing file must not be a directory or a reparse point.
    #>
    param(
        [Parameter(Mandatory)][string]$GameRoot,
        [Parameter(Mandatory)][string]$Destination,
        [switch]$NoCreate
    )

    $rootFull = [System.IO.Path]::TrimEndingDirectorySeparator([System.IO.Path]::GetFullPath($GameRoot))
    $full = [System.IO.Path]::GetFullPath($Destination)
    $prefix = $rootFull + [System.IO.Path]::DirectorySeparatorChar
    if (-not $full.StartsWith($prefix, [System.StringComparison]::OrdinalIgnoreCase)) {
        Throw-S1ModDeploy "Deploy destination escaped the game directory: '$full'."
    }

    $relative = [System.IO.Path]::GetRelativePath($rootFull, $full)
    $segments = @($relative.Split([char[]]@('\', '/'), [System.StringSplitOptions]::RemoveEmptyEntries))
    if ($segments.Count -eq 0 -or ($segments -contains '..')) {
        Throw-S1ModDeploy "Deploy destination is not a file under the game directory: '$full'."
    }

    $current = $rootFull
    for ($i = 0; $i -lt $segments.Count; $i++) {
        $current = Join-Path $current $segments[$i]
        $isLeaf = ($i -eq $segments.Count - 1)
        if (-not (Test-Path -LiteralPath $current)) {
            if (-not $isLeaf -and -not $NoCreate) {
                New-Item -ItemType Directory -Path $current -Force | Out-Null
            }
            continue
        }

        $item = Get-Item -LiteralPath $current -Force
        if (Test-S1ReparsePoint -Item $item) {
            Throw-S1ModDeploy "Deploy target is a reparse point/symlink; refusing an ambiguous write: '$current'."
        }
        if ($isLeaf) {
            if ($item.PSIsContainer) {
                Throw-S1ModDeploy "Deploy destination exists as a directory: '$current'."
            }
        }
        elseif (-not $item.PSIsContainer) {
            Throw-S1ModDeploy "Deploy destination parent is a file, not a directory: '$current'."
        }
    }
}

function Write-S1JsonAtomic {
    param(
        [Parameter(Mandatory)][string]$Path,
        [Parameter(Mandatory)]$Object
    )

    $directory = Split-Path -Parent $Path
    $temp = Join-Path $directory ('.tmp-' + [guid]::NewGuid().ToString('N') + '.json')
    try {
        $Object | ConvertTo-Json -Depth 8 | Set-Content -LiteralPath $temp -Encoding utf8NoBOM
        Move-Item -LiteralPath $temp -Destination $Path -Force
    }
    finally {
        if (Test-Path -LiteralPath $temp) {
            Remove-Item -LiteralPath $temp -Force -ErrorAction SilentlyContinue
        }
    }
}

function Copy-S1DeployTargetFile {
    # Production seam: tests replace this function to throw on a chosen copy.
    param(
        [Parameter(Mandatory)][string]$Source,
        [Parameter(Mandatory)][string]$Destination
    )
    Copy-Item -LiteralPath $Source -Destination $Destination -Force -ErrorAction Stop
}

function Restore-S1DeployTargetFile {
    # Production seam: tests replace this function to simulate a failed restore.
    param(
        [Parameter(Mandatory)][string]$Source,
        [Parameter(Mandatory)][string]$Destination
    )
    Copy-Item -LiteralPath $Source -Destination $Destination -Force -ErrorAction Stop
}

function Invoke-S1DeployRollback {
    <#
        Reverts the entries this invocation touched. Each destination and every
        backup ancestor is re-validated against the same game root before any
        write, the backup must live below this run's backup directory, and the
        backup hash is compared with the recorded original hash before the
        restore. New files are removed only while they still hold the exact
        bytes this run copied; an absent file is not counted as removed.
    #>
    param(
        [object[]]$Entries,
        [Parameter(Mandatory)][string]$GameRoot,
        [string]$BackupDir
    )

    $failures = [System.Collections.Generic.List[object]]::new()
    $restored = 0
    $removed = 0

    $backupRoot = $null
    if (-not [string]::IsNullOrWhiteSpace($BackupDir)) {
        $backupRoot = [System.IO.Path]::TrimEndingDirectorySeparator([System.IO.Path]::GetFullPath($BackupDir)) +
        [System.IO.Path]::DirectorySeparatorChar
    }

    for ($i = $Entries.Count - 1; $i -ge 0; $i--) {
        $entry = $Entries[$i]
        if ($entry.State -ne 'Attempted' -and $entry.State -ne 'Copied') {
            # Never touched by this run; nothing to revert.
            if ($entry.State -eq 'Pending') { $entry.State = 'NotAttempted' }
            continue
        }
        try {
            Assert-S1DeployDestination -GameRoot $GameRoot -Destination $entry.Destination -NoCreate

            if ($entry.ExistedBefore) {
                if ([string]::IsNullOrWhiteSpace($entry.BackupPath) -or
                    -not (Test-Path -LiteralPath $entry.BackupPath -PathType Leaf)) {
                    Throw-S1ModDeploy "Backup missing for '$($entry.DestinationRelative)'; cannot restore the previous file."
                }
                $backupFull = [System.IO.Path]::GetFullPath($entry.BackupPath)
                if ($null -eq $backupRoot -or
                    -not $backupFull.StartsWith($backupRoot, [System.StringComparison]::OrdinalIgnoreCase)) {
                    Throw-S1ModDeploy "Backup '$backupFull' is outside this run's backup directory; refusing to restore."
                }
                Assert-S1DeployDestination -GameRoot $GameRoot -Destination $backupFull -NoCreate

                # Verify the backup before it is allowed to overwrite the target.
                $backupHash = (Get-FileHash -LiteralPath $backupFull -Algorithm SHA256).Hash
                if ($backupHash -ne $entry.OriginalSha256) {
                    Throw-S1ModDeploy "Backup hash mismatch for '$($entry.DestinationRelative)' (expected $($entry.OriginalSha256), got $backupHash); refusing to restore."
                }

                Restore-S1DeployTargetFile -Source $backupFull -Destination $entry.Destination
                $hash = (Get-FileHash -LiteralPath $entry.Destination -Algorithm SHA256).Hash
                if ($hash -ne $entry.OriginalSha256) {
                    Throw-S1ModDeploy "Restored file hash mismatch for '$($entry.DestinationRelative)'."
                }
                $entry.State = 'Restored'
                $restored++
            }
            else {
                if (Test-Path -LiteralPath $entry.Destination) {
                    $item = Get-Item -LiteralPath $entry.Destination -Force
                    if ($item.PSIsContainer) {
                        Throw-S1ModDeploy "A directory appeared at '$($entry.DestinationRelative)'; not removing it."
                    }
                    if (Test-S1ReparsePoint -Item $item) {
                        Throw-S1ModDeploy "A reparse point appeared at '$($entry.DestinationRelative)'; not removing it."
                    }
                    $actualHash = (Get-FileHash -LiteralPath $entry.Destination -Algorithm SHA256).Hash
                    if ($actualHash -ne $entry.SourceSha256) {
                        Throw-S1ModDeploy "Not removing '$($entry.DestinationRelative)': its content is not the copy this run wrote."
                    }
                    Remove-Item -LiteralPath $entry.Destination -Force -ErrorAction Stop
                    $entry.State = 'Removed'
                    $removed++
                }
                else {
                    # The file this run would have created is already absent:
                    # nothing was removed, and it must not inflate the count.
                    $entry.State = 'NotAttempted'
                }
            }
        }
        catch {
            $entry.State = 'RollbackFailed'
            $failures.Add([ordered]@{
                    Path  = $entry.DestinationRelative
                    Error = $_.Exception.Message
                })
        }
    }

    return [pscustomobject]@{
        Restored = $restored
        Removed  = $removed
        Failures = @($failures)
    }
}

function Get-S1StagedCopyPath {
    # Unique same-directory temp name that keeps the destination leaf as its
    # suffix, so the Copy-S1DeployTargetFile fault seam still sees it.
    param([Parameter(Mandatory)][string]$Destination)
    $directory = Split-Path -Parent $Destination
    $leaf = Split-Path -Leaf $Destination
    return (Join-Path $directory ($script:S1DeployTempPrefix + [guid]::NewGuid().ToString('N') + '-' + $leaf))
}

function Read-S1DeployReceipt {
    param([string]$Path)
    try {
        if (-not [string]::IsNullOrWhiteSpace($Path) -and (Test-Path -LiteralPath $Path -PathType Leaf)) {
            return (Get-Content -LiteralPath $Path -Raw | ConvertFrom-Json)
        }
    }
    catch {
        # A corrupt receipt is not ownership evidence; ignore it.
    }
    return $null
}

function Get-S1PreviousDeployReceipt {
    <#
        Returns the most recent successful (Status=Deployed) receipt for the
        project. The latest file wins; after a failed run the latest file is a
        failure record, so attempt receipts are scanned so ownership history
        survives failures and no-op reruns.
    #>
    param(
        [Parameter(Mandatory)][string]$ReceiptsDir,
        [Parameter(Mandatory)][string]$ProjectName,
        [Parameter(Mandatory)][string]$LatestPath
    )

    $latest = Read-S1DeployReceipt -Path $LatestPath
    if ($null -ne $latest -and [string]$latest.Status -eq 'Deployed') { return $latest }

    $candidates = [System.Collections.Generic.List[object]]::new()
    if (Test-Path -LiteralPath $ReceiptsDir -PathType Container) {
        foreach ($file in @(Get-ChildItem -LiteralPath $ReceiptsDir -File -Filter "$ProjectName.*.json" -ErrorAction SilentlyContinue)) {
            if ($file.Name -eq "$ProjectName.json") { continue }
            $candidate = Read-S1DeployReceipt -Path $file.FullName
            if ($null -ne $candidate -and [string]$candidate.Status -eq 'Deployed') {
                $candidates.Add($candidate)
            }
        }
    }
    if ($candidates.Count -eq 0) { return $null }
    return ($candidates | Sort-Object -Property DeployedAtUtc -Descending | Select-Object -First 1)
}

function Get-S1StaleDeployArtifacts {
    <#
        Lists files owned by the previous successful deploy that this plan no
        longer deploys and that still exist. Reporting only: never deletes.
    #>
    param(
        [Parameter(Mandatory)][string]$GameRoot,
        [Parameter(Mandatory)][object[]]$Plan,
        $PreviousReceipt
    )

    $stale = [System.Collections.Generic.List[object]]::new()
    if ($null -eq $PreviousReceipt) { return @($stale) }

    $current = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::OrdinalIgnoreCase)
    foreach ($entry in $Plan) { [void]$current.Add([System.IO.Path]::GetFullPath($entry.Destination)) }

    $seen = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::OrdinalIgnoreCase)
    foreach ($prior in (@($PreviousReceipt.Entries) + @($PreviousReceipt.Stale))) {
        if ($null -eq $prior) { continue }
        $destination = [string]$prior.Destination
        if ([string]::IsNullOrWhiteSpace($destination) -and -not [string]::IsNullOrWhiteSpace([string]$prior.DestinationRelative)) {
            $destination = Join-Path $GameRoot ([string]$prior.DestinationRelative)
        }
        if ([string]::IsNullOrWhiteSpace($destination)) { continue }
        $destination = [System.IO.Path]::GetFullPath($destination)
        if ($current.Contains($destination) -or -not $seen.Add($destination)) { continue }
        # Prior receipts are evidence, not authority to read outside this install.
        Assert-S1DeployDestination -GameRoot $GameRoot -Destination $destination -NoCreate
        if (-not (Test-Path -LiteralPath $destination -PathType Leaf)) { continue }

        $hash = $null
        try { $hash = (Get-FileHash -LiteralPath $destination -Algorithm SHA256).Hash } catch { $hash = 'unreadable' }
        $stale.Add([ordered]@{
                Kind                  = $prior.Kind
                DestinationRelative   = $prior.DestinationRelative
                Destination           = $destination
                Sha256                = $hash
                PreviousAttemptId     = $PreviousReceipt.AttemptId
                PreviousDeployedAtUtc = $PreviousReceipt.DeployedAtUtc
            })
        Write-Warning "[deploy-mod] Stale previously deployed file is not in this plan (left in place): '$($prior.DestinationRelative)'."
    }

    return @($stale)
}

function New-S1DeployReceiptEntry {
    <#
        One receipt row. With -Recompute the destination hash is read from the
        install as it is now (null when absent, 'unreadable' plus an error when
        it exists but cannot be read), so failure receipts state the truth.
    #>
    param(
        [Parameter(Mandatory)]$Entry,
        [switch]$Recompute
    )

    $destinationHash = $Entry.DestinationSha256
    $readError = $null
    if ($Recompute) {
        $destinationHash = $null
        if (Test-Path -LiteralPath $Entry.Destination -PathType Leaf) {
            try {
                $destinationHash = (Get-FileHash -LiteralPath $Entry.Destination -Algorithm SHA256).Hash
            }
            catch {
                $destinationHash = 'unreadable'
                $readError = $_.Exception.Message
            }
        }
        elseif (Test-Path -LiteralPath $Entry.Destination) {
            $destinationHash = 'unreadable'
            $readError = 'destination exists but is not a regular file'
        }
    }

    $row = [ordered]@{
        Kind                = $Entry.Kind
        State               = $Entry.State
        Action              = if ($Entry.Action -eq 'Unchanged') { 'Unchanged' } else { $Entry.State }
        Source              = $Entry.Source
        SourceSha256        = $Entry.SourceSha256
        DestinationRelative = $Entry.DestinationRelative
        Destination         = $Entry.Destination
        DestinationSha256   = $destinationHash
    }
    if ($null -ne $readError) { $row['DestinationReadError'] = $readError }
    return $row
}

function Write-S1DeployReceipt {
    param(
        [Parameter(Mandatory)]$Object,
        [Parameter(Mandatory)][string]$AttemptPath,
        [Parameter(Mandatory)][string]$LatestPath,
        [string]$PreviousLatestPath,
        $PreviousReceipt
    )

    try {
        Write-S1JsonAtomic -Path $AttemptPath -Object $Object
        Write-S1JsonAtomic -Path $LatestPath -Object $Object
    }
    catch {
        $historical = if ($null -ne $PreviousReceipt) {
            "The previous receipt '$PreviousLatestPath' (attempt '$($PreviousReceipt.AttemptId)' at '$($PreviousReceipt.DeployedAtUtc)') is historical and does not describe this run."
        }
        else {
            "No previous successful receipt describes this install for the project."
        }
        Throw-S1ModDeploy ("Could not write deploy receipt '$AttemptPath' / '$LatestPath': " +
            "$($_.Exception.Message) $historical")
    }
}

function Invoke-S1ModDeploy {
    [CmdletBinding()]
    param(
        [string]$GameDir,
        [string]$ProjectName,
        [string]$ProjectDir,
        [string]$TargetPath,
        [string]$PdbPath,
        [string]$AssetsDir,
        [string]$ModJsonPath,
        [string]$Configuration = 'Debug',
        [int]$MutexTimeoutSeconds = 120,
        [switch]$NoDeploy
    )

    if ($NoDeploy -or (([string]$env:S1NoDeploy).Trim() -eq 'true')) {
        Write-Host '[deploy-mod] NoDeploy active - nothing copied.' -ForegroundColor Yellow
        return [pscustomobject]@{ Status = 'NoDeploy'; Project = $ProjectName; Copied = 0; Unchanged = 0 }
    }

    if ([string]::IsNullOrWhiteSpace($ProjectName)) { Throw-S1ModDeploy 'ProjectName is required.' }
    if ([string]::IsNullOrWhiteSpace($TargetPath)) { Throw-S1ModDeploy 'TargetPath is required.' }
    if ([string]::IsNullOrWhiteSpace($Configuration)) { $Configuration = 'Debug' }

    # Read-only resolution first so the mutex name is derived from the same
    # effective install the preflight will validate again inside the lock.
    $resolved = Resolve-S1GameDir -GameDir $GameDir
    $mutexName = Get-S1DeployMutexName -GameDir $resolved.GameDir
    $mutex = $null
    $ownsMutex = $false

    try {
        $mutex = [System.Threading.Mutex]::new($false, $mutexName)
        try {
            $ownsMutex = $mutex.WaitOne([TimeSpan]::FromSeconds($MutexTimeoutSeconds), $false)
        }
        catch [System.Threading.AbandonedMutexException] {
            # The previous owner died mid-deploy. Ownership transfers to us; we
            # still verify every target instead of trusting the leftover state.
            $ownsMutex = $true
            Write-Warning '[deploy-mod] Recovered an abandoned deploy mutex; continuing with verification.'
        }
        if (-not $ownsMutex) {
            Throw-S1ModDeploy "Another deploy of this install still holds mutex '$mutexName' after ${MutexTimeoutSeconds}s. Refusing to report success without deploying."
        }

        # Authoritative preflight: after the lock (the game may have started
        # while waiting) and before any write.
        $context = Invoke-S1DeployPreflight -GameDir $GameDir -Operation $ProjectName
        $gameRoot = $context.GameDir

        $plan = @(New-S1DeployPlan -ProjectName $ProjectName -ProjectDir $ProjectDir `
                -TargetPath $TargetPath -PdbPath $PdbPath -AssetsDir $AssetsDir -ModJsonPath $ModJsonPath)

        foreach ($entry in $plan) {
            $entry.Destination = [System.IO.Path]::GetFullPath((Join-Path $gameRoot $entry.DestinationRelative))
        }

        # Validate every destination before the first write so a rejected plan
        # leaves the install untouched.
        foreach ($entry in $plan) {
            Assert-S1DeployDestination -GameRoot $gameRoot -Destination $entry.Destination -NoCreate
        }

        # Hash sources and existing destinations; unchanged files are neither
        # backed up nor rewritten.
        $changed = [System.Collections.Generic.List[object]]::new()
        foreach ($entry in $plan) {
            if (-not (Test-Path -LiteralPath $entry.Source -PathType Leaf)) {
                Throw-S1ModDeploy "Plan source disappeared before deploy: '$($entry.Source)'."
            }
            $entry.SourceSha256 = (Get-FileHash -LiteralPath $entry.Source -Algorithm SHA256).Hash

            if (Test-Path -LiteralPath $entry.Destination -PathType Leaf) {
                $entry.ExistedBefore = $true
                $entry.OriginalSha256 = (Get-FileHash -LiteralPath $entry.Destination -Algorithm SHA256).Hash
                if ($entry.OriginalSha256 -eq $entry.SourceSha256) {
                    $entry.Action = 'Unchanged'
                    $entry.DestinationSha256 = $entry.OriginalSha256
                    $entry.State = 'NotAttempted'
                    continue
                }
            }

            $entry.Action = 'Copied'
            $changed.Add($entry)
        }

        $backupId = $null
        $backupDir = $null
        $recordsRoot = Join-Path $context.UserDataDir $script:S1DeployRecordDirName
        $receiptsDir = Join-Path $recordsRoot 'receipts'
        $receiptPath = Join-Path $receiptsDir "$ProjectName.json"

        $attemptId = '{0}-{1}' -f (Get-Date).ToUniversalTime().ToString('yyyyMMddTHHmmssfffZ'),
            [guid]::NewGuid().ToString('N').Substring(0, 12)
        $attemptReceiptPath = Join-Path $receiptsDir "$ProjectName.$attemptId.json"

        # Ownership history is read before this run writes anything: the latest
        # successful receipt, or (after a failed run) the newest successful
        # attempt receipt.
        $previousReceipt = Get-S1PreviousDeployReceipt -ReceiptsDir $receiptsDir `
            -ProjectName $ProjectName -LatestPath $receiptPath
        $stale = @(Get-S1StaleDeployArtifacts -GameRoot $gameRoot -Plan $plan -PreviousReceipt $previousReceipt)

        # The receipt directory is part of the deployment record, not a mod
        # file: create it under the same reparse/safety checks before writing.
        Assert-S1DeployDestination -GameRoot $gameRoot -Destination $receiptPath | Out-Null
        Assert-S1DeployDestination -GameRoot $gameRoot -Destination $attemptReceiptPath | Out-Null

        # A backup directory exists only when this run overwrites something.
        $overwritten = @($changed | Where-Object { $_.ExistedBefore })
        if ($overwritten.Count -gt 0) {
            $backupId = '{0}-{1}-{2}' -f (Get-Date).ToUniversalTime().ToString('yyyyMMddTHHmmssfffZ'),
                $ProjectName, [guid]::NewGuid().ToString('N').Substring(0, 8)
            $backupDir = Join-Path (Join-Path $recordsRoot 'backups') $backupId

            # Backup only the exact files that will be overwritten - never a
            # recursive copy of UserData (that would sweep saves/configs).
            Assert-S1DeployDestination -GameRoot $gameRoot -Destination (Join-Path $backupDir 'manifest.json') | Out-Null

            $manifestEntries = [System.Collections.Generic.List[object]]::new()
            $index = 0
            foreach ($entry in $overwritten) {
                $backupFile = Join-Path $backupDir ('{0:D3}-{1}' -f $index, (Split-Path -Leaf $entry.Destination))
                $index++
                Copy-Item -LiteralPath $entry.Destination -Destination $backupFile -Force -ErrorAction Stop
                $backupHash = (Get-FileHash -LiteralPath $backupFile -Algorithm SHA256).Hash
                if ($backupHash -ne $entry.OriginalSha256) {
                    Throw-S1ModDeploy "Backup verification failed for '$($entry.DestinationRelative)'; nothing was overwritten."
                }
                $entry.BackupPath = $backupFile
                $manifestEntries.Add([ordered]@{
                        DestinationRelative = $entry.DestinationRelative
                        Destination         = $entry.Destination
                        Backup              = $backupFile
                        Sha256              = $entry.OriginalSha256
                    })
            }

            Write-S1JsonAtomic -Path (Join-Path $backupDir 'manifest.json') -Object ([ordered]@{
                    Schema      = 1
                    Project     = $ProjectName
                    GameDir     = $gameRoot
                    CreatedUtc  = (Get-Date).ToUniversalTime().ToString('o')
                    Entries     = @($manifestEntries)
                })
        }

        $touched = [System.Collections.Generic.List[object]]::new()
        $deployError = $null
        $rollback = $null

        try {
            foreach ($entry in $changed) {
                Assert-S1DeployDestination -GameRoot $gameRoot -Destination $entry.Destination
                $touched.Add($entry)
                $entry.State = 'Attempted'

                # Stage beside the target, verify the staged bytes, then move
                # the verified file into place so a bad copy never replaces a
                # good target.
                $stagedPath = Get-S1StagedCopyPath -Destination $entry.Destination
                try {
                    Copy-S1DeployTargetFile -Source $entry.Source -Destination $stagedPath
                    $stagedHash = (Get-FileHash -LiteralPath $stagedPath -Algorithm SHA256).Hash
                    if ($stagedHash -ne $entry.SourceSha256) {
                        Throw-S1ModDeploy "Staged copy hash mismatch before replacing '$($entry.Destination)': expected $($entry.SourceSha256), got $stagedHash."
                    }
                    Move-Item -LiteralPath $stagedPath -Destination $entry.Destination -Force
                }
                finally {
                    if (Test-Path -LiteralPath $stagedPath) {
                        Remove-Item -LiteralPath $stagedPath -Force -ErrorAction SilentlyContinue
                    }
                }

                $destinationHash = (Get-FileHash -LiteralPath $entry.Destination -Algorithm SHA256).Hash
                if ($destinationHash -ne $entry.SourceSha256) {
                    Throw-S1ModDeploy "Hash mismatch after copy: '$($entry.Source)' ($($entry.SourceSha256)) != '$($entry.Destination)' ($destinationHash)."
                }
                $entry.DestinationSha256 = $destinationHash
                $entry.State = 'Copied'
            }

            $git = Get-S1GitState -WorkspaceRoot $script:S1DeployWorkspaceRoot
            $receipt = [ordered]@{
                Schema              = 2
                Status              = 'Deployed'
                Project             = $ProjectName
                Configuration       = $Configuration
                GameDir             = $gameRoot
                AttemptId           = $attemptId
                DeployedAtUtc       = (Get-Date).ToUniversalTime().ToString('o')
                GitRevision         = $git.Revision
                GitDirty            = $git.Dirty
                BackupId            = $backupId
                BackupDir           = $backupDir
                ReceiptPath         = $receiptPath
                AttemptReceiptPath  = $attemptReceiptPath
                PreviousReceipt     = if ($null -ne $previousReceipt) {
                    [ordered]@{
                        AttemptId     = $previousReceipt.AttemptId
                        DeployedAtUtc = $previousReceipt.DeployedAtUtc
                        Path          = $receiptPath
                    }
                } else { $null }
                Stale               = @($stale)
                Note                = 'Best-effort in-process rollback only; a crash or kill between writes is not recovered automatically.'
                Entries             = @($plan | ForEach-Object { New-S1DeployReceiptEntry -Entry $_ })
                Rollback            = [ordered]@{ Attempted = $false; Restored = 0; Removed = 0; Failures = @() }
            }

            Write-S1DeployReceipt -Object $receipt -AttemptPath $attemptReceiptPath -LatestPath $receiptPath `
                -PreviousLatestPath $receiptPath -PreviousReceipt $previousReceipt
        }
        catch {
            $deployError = $_
            $rollback = Invoke-S1DeployRollback -Entries @($touched) -GameRoot $gameRoot -BackupDir $backupDir

            foreach ($entry in $plan) {
                if ($entry.State -eq 'Pending') { $entry.State = 'NotAttempted' }
            }

            $status = if (@($rollback.Failures).Count -eq 0) { 'RolledBack' } else { 'RollbackFailed' }
            $failedReceipt = [ordered]@{
                Schema             = 2
                Status             = $status
                Project            = $ProjectName
                Configuration      = $Configuration
                GameDir            = $gameRoot
                AttemptId          = $attemptId
                FailedAtUtc        = (Get-Date).ToUniversalTime().ToString('o')
                Error              = $deployError.Exception.Message
                Note               = 'Best-effort in-process rollback only; a crash or kill between writes is not recovered automatically.'
                BackupId           = $backupId
                BackupDir          = $backupDir
                ReceiptPath        = $receiptPath
                AttemptReceiptPath = $attemptReceiptPath
                PreviousReceipt    = if ($null -ne $previousReceipt) {
                    [ordered]@{
                        AttemptId     = $previousReceipt.AttemptId
                        DeployedAtUtc = $previousReceipt.DeployedAtUtc
                        Path          = $receiptPath
                    }
                } else { $null }
                Stale              = @($stale)
                Entries            = @($plan | ForEach-Object { New-S1DeployReceiptEntry -Entry $_ -Recompute })
                Rollback           = [ordered]@{
                    Attempted = ($touched.Count -gt 0)
                    Restored  = $rollback.Restored
                    Removed   = $rollback.Removed
                    Failures  = @($rollback.Failures)
                }
            }
            try {
                # Attempt record first, then the latest pointer. Either failing
                # is reported below; this is best-effort after a rollback.
                Write-S1JsonAtomic -Path $attemptReceiptPath -Object $failedReceipt
                Write-S1JsonAtomic -Path $receiptPath -Object $failedReceipt
            }
            catch {
                Write-Warning "[deploy-mod] Could not write failure receipt '$attemptReceiptPath' / '$receiptPath': $($_.Exception.Message)"
            }
        }

        if ($null -ne $deployError) {
            $lines = [System.Collections.Generic.List[string]]::new()
            $lines.Add("[deploy-mod] $ProjectName failed: $($deployError.Exception.Message)")
            $lines.Add("[deploy-mod] Rollback: restored $($rollback.Restored), removed $($rollback.Removed), failures $(@($rollback.Failures).Count).")
            foreach ($failure in @($rollback.Failures)) {
                $lines.Add("  FAILED $($failure.Path): $($failure.Error)")
            }
            if (@($rollback.Failures).Count -gt 0) {
                $lines.Add('[deploy-mod] Partial rollback: the install is NOT back to its previous state. See the receipt and backup directory.')
            }
            Throw-S1ModDeploy ($lines -join [Environment]::NewLine)
        }

        $copied = @($plan | Where-Object { $_.Action -eq 'Copied' }).Count
        $unchanged = @($plan | Where-Object { $_.Action -eq 'Unchanged' }).Count
        Write-Host "[deploy-mod] $ProjectName -> Copied $copied, Unchanged $unchanged; receipt $receiptPath" -ForegroundColor Green

        return [pscustomobject]@{
            Status    = 'Deployed'
            Project   = $ProjectName
            Copied    = $copied
            Unchanged = $unchanged
            Receipt   = $receiptPath
            GameDir   = $gameRoot
        }
    }
    finally {
        if ($ownsMutex -and $null -ne $mutex) {
            try { $mutex.ReleaseMutex() | Out-Null }
            catch { Write-Warning "[deploy-mod] Failed to release mutex: $($_.Exception.Message)" }
        }
        if ($null -ne $mutex) { $mutex.Dispose() }
    }
}

if ($MyInvocation.InvocationName -ne '.') {
    try {
        $null = Invoke-S1ModDeploy -GameDir $GameDir -ProjectName $ProjectName -ProjectDir $ProjectDir `
            -TargetPath $TargetPath -PdbPath $PdbPath -AssetsDir $AssetsDir -ModJsonPath $ModJsonPath `
            -Configuration $Configuration -MutexTimeoutSeconds $MutexTimeoutSeconds -NoDeploy:$NoDeploy
        exit 0
    }
    catch {
        Write-Host "[deploy-mod] ERROR: $($_.Exception.Message)" -ForegroundColor Red
        $code = 2
        if ($null -ne $_.Exception.Data -and $_.Exception.Data.Contains('S1ExitCode')) {
            $code = [int]$_.Exception.Data['S1ExitCode']
        }
        exit $code
    }
}
