#Requires -Version 7
<#
.SYNOPSIS
    Verify the current working files in a fresh, isolated checkout.
.DESCRIPTION
    Captures tracked files (including deletions), nonignored new files and clean
    submodule commits. Ignored outputs/configuration are intentionally excluded.
    The source worktree and index are never changed. The checkout, manifest and
    logs are retained in a unique scratch directory; nothing is deleted.
    This verifies working files, not the staged-only contents of a future commit.
#>
[CmdletBinding()]
param(
    [ValidateSet('Quick', 'Pure', 'Full', 'Auto')]
    [string]$Mode = 'Pure',
    [string]$ScratchRoot,
    [string]$DotnetPath,
    [switch]$SnapshotOnly
)

$ErrorActionPreference = 'Stop'

function Invoke-SnapshotGit {
    param([string]$Root, [string[]]$Arguments, [string]$InputText)
    $start = [System.Diagnostics.ProcessStartInfo]::new('git')
    $start.UseShellExecute = $false
    $start.CreateNoWindow = $true
    $start.RedirectStandardOutput = $true
    $start.RedirectStandardError = $true
    $start.RedirectStandardInput = $PSBoundParameters.ContainsKey('InputText')
    $start.StandardOutputEncoding = [System.Text.Encoding]::UTF8
    $start.StandardErrorEncoding = [System.Text.Encoding]::UTF8
    if ($start.RedirectStandardInput) { $start.StandardInputEncoding = [System.Text.UTF8Encoding]::new($false) }
    # Scratch checkouts have longer prefixes than the original Windows workspace.
    # Process-local only; never change the user's Git configuration.
    $start.ArgumentList.Add('-c')
    $start.ArgumentList.Add('core.longpaths=true')
    $start.ArgumentList.Add('-C')
    $start.ArgumentList.Add($Root)
    foreach ($argument in $Arguments) { $start.ArgumentList.Add($argument) }
    # Status must not refresh the source index as a side effect.
    $start.Environment['GIT_OPTIONAL_LOCKS'] = '0'
    $process = [System.Diagnostics.Process]::Start($start)
    try {
        $stdout = $process.StandardOutput.ReadToEndAsync()
        $stderr = $process.StandardError.ReadToEndAsync()
        if ($start.RedirectStandardInput) {
            $process.StandardInput.Write($InputText)
            $process.StandardInput.Close()
        }
        $process.WaitForExit()
        $output = $stdout.GetAwaiter().GetResult()
        $errorText = $stderr.GetAwaiter().GetResult()
        if ($process.ExitCode -ne 0) {
            throw "git $($Arguments[0]) failed (exit $($process.ExitCode)): $errorText"
        }
        return $output
    }
    finally { $process.Dispose() }
}

function Get-SnapshotPath {
    param([string]$Root, [string]$Relative)
    $rootFull = [System.IO.Path]::TrimEndingDirectorySeparator([System.IO.Path]::GetFullPath($Root))
    $full = [System.IO.Path]::GetFullPath((Join-Path $rootFull $Relative))
    $rootPrefix = $rootFull.TrimEnd('\', '/') + [System.IO.Path]::DirectorySeparatorChar
    if (-not $full.StartsWith($rootPrefix, [StringComparison]::OrdinalIgnoreCase) -or
        $Relative -match '(^|[\\/])\.git([\\/]|$)') {
        throw "Snapshot path escapes the checkout or targets Git metadata: $Relative"
    }
    $cursor = $full
    # Check all existing ancestors, including those above Root. Otherwise a
    # junction above a nested ScratchRoot can redirect writes into the source.
    while ($cursor) {
        $item = Get-Item -LiteralPath $cursor -Force -ErrorAction SilentlyContinue
        if ($item -and ($item.Attributes -band [System.IO.FileAttributes]::ReparsePoint)) {
            throw "Reparse points/symlinks are not supported in snapshots: $cursor"
        }
        $parent = [System.IO.Directory]::GetParent($cursor)
        $cursor = if ($parent) { $parent.FullName } else { $null }
    }
    return $full
}

function Get-SnapshotState {
    param([string]$Root)
    $head = (Invoke-SnapshotGit $Root @('rev-parse', 'HEAD')).Trim()
    $entries = (Invoke-SnapshotGit $Root @('ls-files', '--stage', '-z')).Split([char]0, [StringSplitOptions]::RemoveEmptyEntries)
    $paths = [System.Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
    $modes = [System.Collections.Generic.Dictionary[string, string]]::new([StringComparer]::Ordinal)
    $modules = @()
    foreach ($entry in $entries) {
        if ($entry -notmatch '^(?<mode>\d+) (?<sha>[0-9a-f]+) (?<stage>\d+)\t(?<path>[\s\S]+)$') {
            throw 'Unrecognized Git index entry.'
        }
        if ($Matches.stage -ne '0') { throw "Resolve merge conflicts before snapshotting: $($Matches.path)" }
        $relative = $Matches.path
        $modeValue = $Matches.mode
        $sha = $Matches.sha
        $full = Get-SnapshotPath $Root $relative
        if ($modeValue -eq '160000') {
            if (Test-Path -LiteralPath (Join-Path $full '.git')) {
                if (Invoke-SnapshotGit $full @('status', '--porcelain', '-z', '--untracked-files=all', '--ignore-submodules=none')) {
                    throw "Submodule has uncommitted files; snapshot would omit them: $relative"
                }
                $sha = (Invoke-SnapshotGit $full @('rev-parse', 'HEAD')).Trim()
            }
            $modules += [ordered]@{ path = $relative; commit = $sha }
        }
        elseif ($modeValue -in @('100644', '100755')) {
            $null = $paths.Add($relative)
            $modes[$relative] = $modeValue
        }
        else { throw "Unsupported Git mode $modeValue at $relative (symlinks are not copied)." }
    }
    $newPaths = (Invoke-SnapshotGit $Root @('ls-files', '--others', '--exclude-standard', '-z')).Split([char]0, [StringSplitOptions]::RemoveEmptyEntries)
    foreach ($relative in $newPaths) {
        $null = $paths.Add($relative)
        $modes[$relative] = '100644'
    }
    $files = @()
    foreach ($relative in ($paths | Sort-Object -CaseSensitive)) {
        $full = Get-SnapshotPath $Root $relative
        if (-not (Test-Path -LiteralPath $full)) { continue }
        if (-not (Test-Path -LiteralPath $full -PathType Leaf)) { throw "Expected a file: $relative" }
        $files += [ordered]@{
            path = $relative
            mode = $modes[$relative]
            sha256 = (Get-FileHash -LiteralPath $full -Algorithm SHA256).Hash.ToLowerInvariant()
        }
    }
    $identity = [ordered]@{ head = $head; files = @($files); submodules = @($modules | Sort-Object { $_.path }) }
    $bytes = [System.Text.Encoding]::UTF8.GetBytes(($identity | ConvertTo-Json -Depth 8 -Compress))
    $digest = [Convert]::ToHexString([System.Security.Cryptography.SHA256]::HashData($bytes)).ToLowerInvariant()
    return [pscustomobject]@{
        head = $head
        digest = $digest
        files = $identity.files
        submodules = $identity.submodules
        status = Invoke-SnapshotGit $Root @('status', '--porcelain', '--untracked-files=all', '--ignore-submodules=none')
    }
}

function Invoke-CleanCheckoutVerification {
    $source = [System.IO.Path]::GetFullPath((Split-Path $PSScriptRoot -Parent)).TrimEnd('\', '/')
    $actualRoot = (Invoke-SnapshotGit $source @('rev-parse', '--show-toplevel')).Trim()
    if ([System.IO.Path]::GetFullPath($actualRoot).TrimEnd('\', '/') -ne $source) { throw 'Run this tool from its repository root Tools directory.' }
    if (-not $ScratchRoot) {
        # Keep the default outside packaged-app LOCALAPPDATA virtualization so
        # PowerShell and Git agree on the checkout's physical root.
        if ($env:USERPROFILE) { $ScratchRoot = Join-Path $env:USERPROFILE '.cache/Schedule1Tools/clean-checkouts' }
        elseif ($env:LOCALAPPDATA) { $ScratchRoot = Join-Path $env:LOCALAPPDATA 'Schedule1Tools/clean-checkouts' }
        else { throw 'No scratch root available; pass -ScratchRoot.' }
    }
    $scratch = [System.IO.Path]::GetFullPath($ScratchRoot).TrimEnd('\', '/')
    if ($scratch -eq $source -or $scratch.StartsWith($source + [System.IO.Path]::DirectorySeparatorChar, [StringComparison]::OrdinalIgnoreCase)) {
        throw 'ScratchRoot must be outside the source repository.'
    }
    # Validate existing parents before creating our unique directory.
    $null = Get-SnapshotPath (Split-Path $scratch -Parent) (Split-Path $scratch -Leaf)
    $before = Get-SnapshotState $source
    # capturedAtUtc carries the time; short names leave room for nested fixtures.
    $run = Join-Path $scratch ('clean-checkout-' + [guid]::NewGuid().ToString('N').Substring(0, 12))
    $null = New-Item -ItemType Directory -Path $run
    $checkout = Join-Path $run 'checkout'
    $manifestPath = Join-Path $run 'snapshot.json'
    $manifest = [ordered]@{
        schemaVersion = 1
        capturedAtUtc = [DateTime]::UtcNow.ToString('o')
        source = $source
        checkout = $checkout
        head = $before.head
        sourceDigest = $before.digest
        sourceStatus = $before.status
        files = $before.files
        submodules = $before.submodules
        mode = $Mode
        status = 'Preparing'
        verificationExitCode = $null
        error = $null
    }
    try {
        Write-Host "Snapshot: $run"
        $null = Invoke-SnapshotGit $source @('clone', '--quiet', '--no-checkout', '--no-hardlinks', '--', $source, $checkout)
        # Reconstruct the candidate index, rather than inheriting deleted files,
        # stale gitlinks or old executable bits from HEAD. Source index is read-only.
        $null = Invoke-SnapshotGit $checkout @('read-tree', '--empty')
        $null = Invoke-SnapshotGit $checkout @('update-ref', 'HEAD', $before.head)
        foreach ($file in $before.files) {
            $from = Get-SnapshotPath $source $file.path
            $to = Get-SnapshotPath $checkout $file.path
            $null = New-Item -ItemType Directory -Path (Split-Path $to -Parent) -Force
            Copy-Item -LiteralPath $from -Destination $to
            if ((Get-FileHash -LiteralPath $to -Algorithm SHA256).Hash.ToLowerInvariant() -ne $file.sha256) {
                throw "Source changed while copying: $($file.path)"
            }
        }
        # Only captured files exist here. Force is needed for explicitly tracked
        # ignored files; it never copies ignored local files from the source.
        $null = Invoke-SnapshotGit $checkout @('add', '--all', '--force')
        foreach ($modeValue in @('100644', '100755')) {
            $modePaths = @($before.files | Where-Object mode -eq $modeValue | ForEach-Object path)
            if ($modePaths.Count -gt 0) {
                $chmod = if ($modeValue -eq '100755') { '--chmod=+x' } else { '--chmod=-x' }
                # NUL-delimited stdin preserves unusual names and avoids Windows
                # command-line length limits for a repository-wide mode batch.
                $inputPaths = ($modePaths -join [char]0) + [char]0
                $null = Invoke-SnapshotGit $checkout @('update-index', $chmod, '-z', '--stdin') -InputText $inputPaths
            }
        }
        foreach ($module in $before.submodules) {
            $null = Invoke-SnapshotGit $checkout @('update-index', '--add', '--cacheinfo', "160000,$($module.commit),$($module.path)")
        }
        if ($before.submodules.Count -gt 0) {
            Write-Host 'Initializing pinned submodules in the isolated checkout...'
            $null = Invoke-SnapshotGit $checkout @('submodule', 'update', '--init', '--recursive')
        }
        if ((Get-SnapshotState $source).digest -ne $before.digest) { throw 'Source changed during snapshot creation; rerun after edits finish.' }
        # Copied bytes, not checkout line-ending conversion, define this candidate.
        foreach ($file in $before.files) {
            if ((Get-FileHash -LiteralPath (Get-SnapshotPath $checkout $file.path) -Algorithm SHA256).Hash.ToLowerInvariant() -ne $file.sha256) {
                throw "Snapshot identity differs before verification: $($file.path)"
            }
        }
        if ((Get-SnapshotState $checkout).digest -ne $before.digest) {
            throw 'Snapshot identity differs before verification (files, modes or submodules).'
        }
        $manifest.status = 'Captured'
        $manifest | ConvertTo-Json -Depth 10 | Set-Content -LiteralPath $manifestPath -Encoding utf8NoBOM
        if ($SnapshotOnly) {
            Write-Host 'Snapshot captured only; no verification was executed.'
        }
        else {
            $verifier = Get-SnapshotPath $checkout 'Tools/verify.ps1'
            if (-not (Test-Path -LiteralPath $verifier -PathType Leaf)) { throw 'Snapshot has no Tools/verify.ps1.' }
            $verifyArguments = @('-NoProfile', '-NonInteractive', '-ExecutionPolicy', 'Bypass', '-File', $verifier, '-Mode', $Mode, '-ScratchRoot', (Join-Path $run 'checks'))
            if ($DotnetPath) { $verifyArguments += @('-DotnetPath', $DotnetPath) }
            & pwsh @verifyArguments 2>&1 | Tee-Object -FilePath (Join-Path $run 'verification.log') | Out-Host
            $manifest.verificationExitCode = $LASTEXITCODE
            if ($LASTEXITCODE -ne 0) { throw "Isolated verification failed (exit $LASTEXITCODE)." }
            if ((Get-SnapshotState $source).digest -ne $before.digest) { throw 'Source changed during verification; the result does not cover the current worktree.' }
            $afterSnapshot = Get-SnapshotState $checkout
            if ($afterSnapshot.digest -ne $before.digest) { throw 'Verification changed snapshot source files; candidate identity no longer matches.' }
            $manifest.status = 'Passed'
            Write-Host "Isolated verification passed: $($before.digest)"
        }
    }
    catch {
        $manifest.status = 'Failed'
        $manifest.error = $_.Exception.Message
        throw
    }
    finally {
        $manifest | ConvertTo-Json -Depth 10 | Set-Content -LiteralPath $manifestPath -Encoding utf8NoBOM
        Write-Host "Manifest: $manifestPath"
    }
}

if ($MyInvocation.InvocationName -ne '.') {
    try { Invoke-CleanCheckoutVerification }
    catch { Write-Error $_; exit 1 }
}
