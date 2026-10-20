#Requires -Version 7
<#
.SYNOPSIS
    Deploy an explicitly allowlisted set of ThirdParty DLLs into <GameDir>\Mods\.

.DESCRIPTION
    Deployment is opt-in. ThirdParty/.deployallow is the only source of truth
    and ships empty, so no third-party DLL is deployed by default. Patterns are
    POSIX globs relative to ThirdParty/ (one per line, # comments allowed).

    Safety rules:
      - Missing allowlist is an error (never fall back to "deploy everything").
      - A non-empty allowlist whose pattern matches no DLL is an error naming
        the pattern; an empty allowlist remains a clean no-op.
      - A DLL name that belongs to a workspace mod (any *.csproj under
        Source/Mods, including Shared) is rejected even when bytes match,
        because the flat Mods\ directory would be ambiguous.
      - Two allowlisted DLLs with the same file name are rejected.
      - GameDir is resolved before the shared mutex is taken; the game-running
        guard and every target check run inside the mutex, before any copy.
      - Every selected destination is validated first. A destination that
        already exists with different bytes is never overwritten: third-party
        DLLs may be owner-managed. Copying is allowed only for a new file or a
        byte-identical existing file. Updating an existing DLL is a manual,
        owner-managed action (delete/replace it yourself, then re-run).
      - Files are copied to a temporary sibling, hash-verified, then moved into
        place without overwrite. If the destination changes during staging the
        move is refused.
      - Nothing is deleted automatically: an empty allowlist does not uninstall
        DLLs copied by earlier runs.

    The mutex name is shared with deploy-mod.ps1: "Schedule1Deploy_" plus the
    first 8 bytes (16 lowercase hex chars) of SHA256 over the UTF-8 bytes of
    the resolved GameDir, trimmed of its ending separator and lowercased.
    That serialises mod and third-party deploys into the same install.

    Dot-sourcing this file defines its helpers and does not deploy. Tests use
    that to inject a fake process provider.

.PARAMETER GameDir
    Absolute Schedule I install root. Forwarded to check-deploy-preflight.ps1.

.PARAMETER ThirdPartyRoot
    Directory to scan. Defaults to <repo>\ThirdParty. Tests point this at a
    scratch fixture so no real third-party DLL is ever copied.

.PARAMETER AllowlistPath
    Allowlist file. Defaults to <ThirdPartyRoot>\.deployallow.

.PARAMETER MutexTimeoutSeconds
    How long to wait for a concurrent deploy before failing.

.EXAMPLE
    pwsh -NoProfile -File Tools/deploy-thirdparty.ps1 -GameDir "D:\Games\Schedule I"
#>

[CmdletBinding()]
param(
    [string]$GameDir,
    [string]$ThirdPartyRoot,
    [string]$AllowlistPath,
    [int]$MutexTimeoutSeconds = 120,
    [switch]$NoDeploy
)

$ErrorActionPreference = 'Stop'

$script:S1DeployMutexPrefix = 'Schedule1Deploy_'
$script:PreflightPath = Join-Path $PSScriptRoot 'check-deploy-preflight.ps1'
. $script:PreflightPath

function Throw-S1Deploy {
    param([Parameter(Mandatory)][string]$Message)
    throw [System.InvalidOperationException]::new($Message)
}

function Get-S1ThirdPartyMutexName {
    # Must stay byte-for-byte compatible with deploy-mod.ps1's mutex scheme so
    # mod and third-party deploys into one install serialise on one mutex.
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

function Resolve-S1ThirdPartyRoot {
    param([string]$ThirdPartyRoot)

    if ([string]::IsNullOrWhiteSpace($ThirdPartyRoot)) {
        $ThirdPartyRoot = Join-Path (Get-S1WorkspaceRoot) 'ThirdParty'
    }
    if (-not [System.IO.Path]::IsPathRooted($ThirdPartyRoot)) {
        Throw-S1Deploy "ThirdPartyRoot must be absolute; got '$ThirdPartyRoot'."
    }
    $full = [System.IO.Path]::TrimEndingDirectorySeparator([System.IO.Path]::GetFullPath($ThirdPartyRoot))
    if (-not (Test-Path -LiteralPath $full -PathType Container)) {
        Throw-S1Deploy "ThirdPartyRoot not found: '$full'."
    }
    return $full
}

function Read-S1DeployAllowlist {
    param([Parameter(Mandatory)][string]$Path)

    if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) {
        Throw-S1Deploy "Deploy allowlist missing: '$Path'. Create an empty .deployallow to deploy nothing; there is no implicit deploy-all fallback."
    }

    $patterns = @(
        Get-Content -LiteralPath $Path |
            ForEach-Object { $_.Trim() } |
            Where-Object { $_ -ne '' -and -not $_.StartsWith('#') }
    )

    foreach ($pattern in $patterns) {
        if ($pattern -match '^[A-Za-z]:' -or $pattern.StartsWith('/') -or $pattern.StartsWith('\')) {
            Throw-S1Deploy "Allowlist pattern must be relative to ThirdParty/: '$pattern'."
        }
        if (($pattern -split '/') -contains '..') {
            Throw-S1Deploy "Allowlist pattern must not contain '..': '$pattern'."
        }
        if ($pattern.Contains('\')) {
            Throw-S1Deploy "Allowlist pattern must use forward slashes: '$pattern'."
        }
    }

    return $patterns
}

function Convert-S1GlobToRegex {
    param([Parameter(Mandatory)][string]$Pattern)

    $builder = [System.Text.StringBuilder]::new('^')
    $i = 0
    while ($i -lt $Pattern.Length) {
        $char = $Pattern[$i]
        if ($char -eq '*') {
            if (($i + 1) -lt $Pattern.Length -and $Pattern[$i + 1] -eq '*') {
                if (($i + 2) -lt $Pattern.Length -and $Pattern[$i + 2] -eq '/') {
                    [void]$builder.Append('(?:.*/)?')
                    $i += 3
                    continue
                }
                [void]$builder.Append('.*')
                $i += 2
                continue
            }
            [void]$builder.Append('[^/]*')
            $i++
            continue
        }
        if ($char -eq '?') {
            [void]$builder.Append('[^/]')
            $i++
            continue
        }
        [void]$builder.Append([regex]::Escape([string]$char))
        $i++
    }
    [void]$builder.Append('$')
    return $builder.ToString()
}

function Get-S1ReservedModNames {
    # Workspace mod DLLs own their names in the flat Mods\ directory. Enumerate
    # project file names (the assembly name defaults to the project name) so a
    # third-party DLL can never silently shadow or be shadowed by a mod.
    param([Parameter(Mandatory)][string]$WorkspaceRoot)

    $modsRoot = Join-Path $WorkspaceRoot 'Source/Mods'
    if (-not (Test-Path -LiteralPath $modsRoot -PathType Container)) {
        Throw-S1Deploy "Cannot enumerate workspace mod DLL names: '$modsRoot' not found."
    }

    $names = @(
        Get-ChildItem -LiteralPath $modsRoot -Recurse -File -Filter '*.csproj' -ErrorAction Stop |
            Where-Object { $_.FullName -notmatch '[\\/](obj|bin)[\\/]' } |
            ForEach-Object { $_.BaseName }
    )
    if ($names.Count -eq 0) {
        Throw-S1Deploy "Cannot enumerate workspace mod DLL names: no *.csproj under '$modsRoot'."
    }
    return $names
}

function Select-S1ThirdPartyDlls {
    param(
        [Parameter(Mandatory)][string]$ThirdPartyRoot,
        [string[]]$Patterns
    )

    $patterns = @($Patterns)
    $regexes = @($patterns | ForEach-Object { [regex]::new((Convert-S1GlobToRegex $_), 'IgnoreCase') })
    $all = @(Get-ChildItem -LiteralPath $ThirdPartyRoot -Recurse -File -Filter '*.dll' -ErrorAction Stop)

    # Every non-empty allowlist line must be productive. A typo must fail loudly
    # instead of looking like a successful (empty) deploy.
    $matchesByPattern = @{}
    for ($p = 0; $p -lt $patterns.Count; $p++) {
        $matched = @()
        foreach ($dll in $all) {
            $relative = [System.IO.Path]::GetRelativePath($ThirdPartyRoot, $dll.FullName).Replace('\', '/')
            if ($regexes[$p].IsMatch($relative)) { $matched += $dll }
        }
        if ($matched.Count -eq 0) {
            Throw-S1Deploy "Allowlist pattern matched no DLLs: '$($patterns[$p])'. Fix or remove the pattern; no files were copied."
        }
        $matchesByPattern[$patterns[$p]] = $matched
    }

    $selected = @()
    foreach ($pattern in $patterns) {
        foreach ($dll in $matchesByPattern[$pattern]) {
            $relative = [System.IO.Path]::GetRelativePath($ThirdPartyRoot, $dll.FullName).Replace('\', '/')
            if (@($selected | Where-Object { $_.FullName -eq $dll.FullName }).Count -gt 0) { continue }
            $selected += [pscustomobject]@{
                FullName     = $dll.FullName
                RelativePath = $relative
                DestName     = $dll.Name
            }
        }
    }

    $collisions = @($selected | Group-Object -Property DestName | Where-Object Count -gt 1)
    if ($collisions.Count -gt 0) {
        $details = ($collisions | ForEach-Object {
            "  $($_.Name): " + (($_.Group | ForEach-Object RelativePath) -join ', ')
        }) -join [Environment]::NewLine
        Throw-S1Deploy "Ambiguous deploy target(s): multiple allowlisted DLLs share a file name in the flat Mods\ directory. Refusing to guess.`n$details"
    }

    return $selected
}

function Get-S1Sha256 {
    param([Parameter(Mandatory)][string]$Path)
    return (Get-FileHash -LiteralPath $Path -Algorithm SHA256 -ErrorAction Stop).Hash
}

function New-S1ThirdPartyInstallPlan {
    # Validates every destination before the first byte is written so a bad
    # target can never leave a partially applied deployment.
    param(
        [Parameter(Mandatory)]$Selected,
        [Parameter(Mandatory)][string]$ModsDir,
        [Parameter(Mandatory)][string[]]$ReservedNames
    )

    $reserved = [System.Collections.Generic.HashSet[string]]::new([System.StringComparer]::OrdinalIgnoreCase)
    foreach ($name in $ReservedNames) { [void]$reserved.Add($name) }

    $modsPrefix = [System.IO.Path]::TrimEndingDirectorySeparator(
        [System.IO.Path]::GetFullPath($ModsDir)) + [System.IO.Path]::DirectorySeparatorChar
    $plan = @()

    foreach ($item in $Selected) {
        $destBaseName = [System.IO.Path]::GetFileNameWithoutExtension($item.DestName)
        if ($reserved.Contains($destBaseName)) {
            Throw-S1Deploy "Third-party DLL '$($item.RelativePath)' would deploy as '$($item.DestName)', which is reserved by a workspace mod (Source/Mods/*.csproj). Refusing to collide; no files were copied."
        }

        $destination = [System.IO.Path]::GetFullPath((Join-Path $ModsDir $item.DestName))
        if (-not $destination.StartsWith($modsPrefix, [System.StringComparison]::OrdinalIgnoreCase)) {
            Throw-S1Deploy "Deploy destination escaped Mods\: '$destination'."
        }

        $sourceHash = Get-S1Sha256 -Path $item.FullName
        $existing = Get-Item -LiteralPath $destination -Force -ErrorAction SilentlyContinue
        $action = 'New'
        if ($null -ne $existing) {
            if (($existing.Attributes -band [System.IO.FileAttributes]::ReparsePoint) -ne 0) {
                Throw-S1Deploy "Destination is a reparse point/symlink; refusing to deploy: '$destination'."
            }
            if ($existing.PSIsContainer) {
                Throw-S1Deploy "Ambiguous deploy target (directory with DLL name): '$destination'."
            }
            $existingHash = Get-S1Sha256 -Path $destination
            if ($existingHash -eq $sourceHash) {
                $action = 'Unchanged'
            }
            else {
                Throw-S1Deploy "Refusing to overwrite existing DLL '$destination': it already exists with different content and may be owner-managed. Updating a third-party DLL is a manual action: delete or replace that file yourself, then re-run. No files were copied."
            }
        }

        $plan += [pscustomobject]@{
            Item        = $item
            Destination = $destination
            SourceHash  = $sourceHash
            Action      = $action
        }
    }

    return $plan
}

function Copy-S1FileStaged {
    # Copy to a temp sibling, verify, then move into place without overwriting.
    # A destination that appears or changes while staging is refused.
    param(
        [Parameter(Mandatory)][string]$Source,
        [Parameter(Mandatory)][string]$Destination,
        [Parameter(Mandatory)][string]$ExpectedHash
    )

    $tempPath = Join-Path (Split-Path -Parent $Destination) (
        '.s1tp-' + [guid]::NewGuid().ToString('N') + '.tmp')

    try {
        Copy-Item -LiteralPath $Source -Destination $tempPath -Force -ErrorAction Stop
        $stagedHash = Get-S1Sha256 -Path $tempPath
        if ($stagedHash -ne $ExpectedHash) {
            Throw-S1Deploy "Hash mismatch after staging: '$Source' ($ExpectedHash) != '$tempPath' ($stagedHash)."
        }

        $current = Get-Item -LiteralPath $Destination -Force -ErrorAction SilentlyContinue
        if ($null -ne $current) {
            if (($current.Attributes -band [System.IO.FileAttributes]::ReparsePoint) -ne 0 -or $current.PSIsContainer) {
                Throw-S1Deploy "Destination changed to a directory/reparse point during deploy: '$Destination'."
            }
            $currentHash = Get-S1Sha256 -Path $Destination
            if ($currentHash -eq $ExpectedHash) {
                Remove-Item -LiteralPath $tempPath -Force
                return 'Unchanged'
            }
            Throw-S1Deploy "Destination '$Destination' changed while this deploy was staging; refusing to replace it. No files were overwritten."
        }

        try {
            [System.IO.File]::Move($tempPath, $Destination, $false)
        }
        catch [System.IO.IOException] {
            # Somebody created the file between the check and the move.
            $current = Get-Item -LiteralPath $Destination -Force -ErrorAction SilentlyContinue
            if ($null -ne $current -and -not $current.PSIsContainer -and
                (($current.Attributes -band [System.IO.FileAttributes]::ReparsePoint) -eq 0) -and
                (Get-S1Sha256 -Path $Destination) -eq $ExpectedHash) {
                Remove-Item -LiteralPath $tempPath -Force
                return 'Unchanged'
            }
            Throw-S1Deploy "Destination '$Destination' appeared or changed during deploy; refusing to replace it. No files were overwritten."
        }

        $finalHash = Get-S1Sha256 -Path $Destination
        if ($finalHash -ne $ExpectedHash) {
            Throw-S1Deploy "Hash mismatch after move: '$Destination' ($finalHash) != expected ($ExpectedHash)."
        }
        return 'Copied'
    }
    finally {
        if (Test-Path -LiteralPath $tempPath -PathType Leaf) {
            Remove-Item -LiteralPath $tempPath -Force -ErrorAction SilentlyContinue
        }
    }
}

function Invoke-S1ThirdPartyDeploy {
    [CmdletBinding()]
    param(
        [string]$GameDir,
        [string]$ThirdPartyRoot,
        [string]$AllowlistPath,
        [int]$MutexTimeoutSeconds = 120,
        [switch]$NoDeploy
    )

    $noDeployEffective = $NoDeploy -or (([string]$env:S1NoDeploy).Trim() -eq 'true')
    if ($noDeployEffective) {
        Write-Host '[deploy-thirdparty] NoDeploy active — nothing copied.' -ForegroundColor Yellow
        return [pscustomobject]@{
            Status    = 'NoDeploy'
            Deployed  = 0
            Unchanged = 0
            Installed = 0
            Selected  = 0
            NoDeploy  = $true
        }
    }

    $resolvedRoot = Resolve-S1ThirdPartyRoot -ThirdPartyRoot $ThirdPartyRoot
    if ([string]::IsNullOrWhiteSpace($AllowlistPath)) {
        $AllowlistPath = Join-Path $resolvedRoot '.deployallow'
    }
    if (-not [System.IO.Path]::IsPathRooted($AllowlistPath)) {
        Throw-S1Deploy "AllowlistPath must be absolute; got '$AllowlistPath'."
    }
    $AllowlistPath = [System.IO.Path]::GetFullPath($AllowlistPath)

    $patterns = @(Read-S1DeployAllowlist -Path $AllowlistPath)
    if ($patterns.Count -eq 0) {
        Write-Warning '[deploy-thirdparty] Allowlist is empty: nothing deployed. This does NOT uninstall DLLs copied by earlier runs; delete those files from Mods\ yourself if that is what you want.'
        Write-Host '[deploy-thirdparty] Empty allowlist — nothing to deploy.' -ForegroundColor Cyan
        return [pscustomobject]@{
            Status    = 'NothingToDeploy'
            Deployed  = 0
            Unchanged = 0
            Installed = 0
            Selected  = 0
            NoDeploy  = $false
        }
    }

    $selected = @(Select-S1ThirdPartyDlls -ThirdPartyRoot $resolvedRoot -Patterns $patterns)

    # Resolve GameDir before locking so the mutex key is always the canonical
    # install path, and so path/usage errors do not hold the shared mutex.
    $resolvedGame = Resolve-S1GameDir -GameDir $GameDir
    $mutexName = Get-S1ThirdPartyMutexName -GameDir $resolvedGame.GameDir

    $mutex = $null
    $ownsMutex = $false
    try {
        $mutex = [System.Threading.Mutex]::new($false, $mutexName)
        try {
            $ownsMutex = $mutex.WaitOne([TimeSpan]::FromSeconds($MutexTimeoutSeconds), $false)
        }
        catch [System.Threading.AbandonedMutexException] {
            # The previous owner died holding the mutex. Ownership transfers to
            # us; continue, because skipping would hide a half-done deploy.
            $ownsMutex = $true
            Write-Warning '[deploy-thirdparty] Recovered an abandoned deploy mutex; continuing.'
        }

        if (-not $ownsMutex) {
            Throw-S1Deploy "Another deploy still holds mutex '$mutexName' after ${MutexTimeoutSeconds}s. Refusing to report success without deploying."
        }

        # Must run after acquiring the mutex and immediately before copies:
        # the game may have been started while we were waiting.
        $context = Invoke-S1DeployPreflight -GameDir $resolvedGame.GameDir -Operation 'ThirdParty'

        if (-not (Test-Path -LiteralPath $context.ModsDir -PathType Container)) {
            if (Test-Path -LiteralPath $context.ModsDir) {
                Throw-S1Deploy "Deploy target '$($context.ModsDir)' exists but is not a directory."
            }
            New-Item -ItemType Directory -Path $context.ModsDir -Force | Out-Null
        }

        $workspaceRoot = Split-Path -Parent (Split-Path -Parent $script:PreflightPath)
        $reservedNames = @(Get-S1ReservedModNames -WorkspaceRoot $workspaceRoot)
        $plan = @(New-S1ThirdPartyInstallPlan -Selected $selected -ModsDir $context.ModsDir -ReservedNames $reservedNames)

        $deployed = 0
        $unchanged = 0
        foreach ($entry in $plan) {
            $item = $entry.Item
            if ($entry.Action -eq 'Unchanged') {
                Write-Host "  same  $($item.RelativePath)" -ForegroundColor DarkGray
                $unchanged++
                continue
            }

            $result = Copy-S1FileStaged -Source $item.FullName -Destination $entry.Destination -ExpectedHash $entry.SourceHash
            if ($result -eq 'Unchanged') {
                Write-Host "  same  $($item.RelativePath)" -ForegroundColor DarkGray
                $unchanged++
            }
            else {
                Write-Host "  copy  $($item.RelativePath)  ->  $($entry.Destination)" -ForegroundColor Green
                $deployed++
            }
        }

        $installed = $deployed + $unchanged
        Write-Host "[deploy-thirdparty] Done. Installed: $installed (copied: $deployed, unchanged: $unchanged), Selected: $($selected.Count)." -ForegroundColor Cyan
        return [pscustomobject]@{
            Status    = 'Deployed'
            Deployed  = $deployed
            Unchanged = $unchanged
            Installed = $installed
            Selected  = $selected.Count
            GameDir   = $context.GameDir
            ModsDir   = $context.ModsDir
            NoDeploy  = $false
        }
    }
    finally {
        if ($ownsMutex -and $null -ne $mutex) {
            try { $mutex.ReleaseMutex() | Out-Null }
            catch { Write-Warning "[deploy-thirdparty] Failed to release mutex: $($_.Exception.Message)" }
        }
        if ($null -ne $mutex) { $mutex.Dispose() }
    }
}

if ($MyInvocation.InvocationName -ne '.') {
    try {
        $null = Invoke-S1ThirdPartyDeploy -GameDir $GameDir -ThirdPartyRoot $ThirdPartyRoot -AllowlistPath $AllowlistPath -MutexTimeoutSeconds $MutexTimeoutSeconds -NoDeploy:$NoDeploy
        exit 0
    }
    catch {
        Write-Host "[deploy-thirdparty] ERROR: $($_.Exception.Message)" -ForegroundColor Red
        # Preserve preflight's specific codes (2/3/4/5) instead of flattening
        # every failure to 2.
        $code = 2
        if ($null -ne $_.Exception.Data -and $_.Exception.Data.Contains('S1ExitCode')) {
            $code = [int]$_.Exception.Data['S1ExitCode']
        }
        exit $code
    }
}
