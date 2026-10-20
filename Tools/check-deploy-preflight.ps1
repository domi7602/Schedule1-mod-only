#Requires -Version 7
<#
.SYNOPSIS
    Fail-closed preflight for every write into the Schedule I install.

.DESCRIPTION
    Resolves the effective game directory (explicit -GameDir > SCHEDULE1_PATH >
    Steam default), refuses unsafe/ambiguous targets, and refuses to continue
    while the game is running. Every deploy path (MSBuild targets and
    Tools/deploy-thirdparty.ps1) must call this before it copies a file.

    This script is also dot-sourceable: dot-sourcing defines the helpers and
    does not execute the preflight. Tests use that to inject a fake process
    provider without touching the real game.

.PARAMETER GameDir
    Absolute Schedule I install root. When omitted, SCHEDULE1_PATH is used,
    then the Steam default.

.PARAMETER Operation
    Label used in output/JSON ("Mod", "Shared", "ThirdParty", "Build").

.PARAMETER Json
    Emit one compact JSON object instead of human-readable text.

.EXAMPLE
    pwsh -NoProfile -File Tools/check-deploy-preflight.ps1 -GameDir "D:\Games\Schedule I"

.NOTES
    Exit codes: 0 ok, 2 invalid/unsafe target or usage, 3 game install missing
    or not a Schedule I install, 4 game is running, 5 process state unknown.
#>

[CmdletBinding()]
param(
    # Aliases keep the CLI surface (-GameDir/-Operation/-Json) while avoiding
    # collisions when this file is dot-sourced into deploy-thirdparty.ps1.
    [Alias('GameDir')]
    [string]$S1GameDir,

    [Alias('Operation')]
    [string]$S1Operation = 'Mod',

    [Alias('Json')]
    [switch]$S1Json
)

$ErrorActionPreference = 'Stop'

function Throw-S1Preflight {
    param(
        [Parameter(Mandatory)][int]$Code,
        [Parameter(Mandatory)][string]$Message
    )
    $exception = [System.InvalidOperationException]::new($Message)
    $exception.Data['S1ExitCode'] = $Code
    throw $exception
}

function Get-S1WorkspaceRoot {
    param([string]$ToolsDirectory = $PSScriptRoot)
    return (Split-Path -Parent $ToolsDirectory)
}

function Resolve-S1GameDir {
    [CmdletBinding()]
    param([string]$GameDir)

    $candidate = $GameDir
    if ([string]::IsNullOrWhiteSpace($candidate)) { $candidate = $env:SCHEDULE1_PATH }
    if ([string]::IsNullOrWhiteSpace($candidate)) {
        $candidate = 'C:\Program Files (x86)\Steam\steamapps\common\Schedule I'
    }

    $candidate = $candidate.Trim()
    if ($candidate.Length -ge 2) {
        $first = $candidate[0]
        $last = $candidate[$candidate.Length - 1]
        if (($first -eq '"' -and $last -eq '"') -or ($first -eq "'" -and $last -eq "'")) {
            $candidate = $candidate.Substring(1, $candidate.Length - 2).Trim()
        }
    }

    if ([string]::IsNullOrWhiteSpace($candidate)) {
        Throw-S1Preflight -Code 2 -Message 'GameDir is empty after trimming.'
    }
    if (-not [System.IO.Path]::IsPathRooted($candidate)) {
        Throw-S1Preflight -Code 2 -Message "GameDir must be an absolute path; got '$candidate'."
    }

    try {
        $full = [System.IO.Path]::GetFullPath($candidate)
    }
    catch {
        Throw-S1Preflight -Code 2 -Message "GameDir is not a valid path: '$candidate' ($($_.Exception.Message))."
    }

    $full = [System.IO.Path]::TrimEndingDirectorySeparator($full)
    $pathRoot = [System.IO.Path]::GetPathRoot($full)
    if ([string]::Equals($full, $pathRoot, [System.StringComparison]::OrdinalIgnoreCase)) {
        Throw-S1Preflight -Code 2 -Message "GameDir must not be a drive root ('$full')."
    }

    # Never write into the repo (e.g. a copy-pasted relative path resolving to
    # the workspace). A real Steam library may contain the repo as a sibling,
    # but the reverse (game dir inside the repo) is always a mistake.
    $workspaceRoot = [System.IO.Path]::TrimEndingDirectorySeparator(
        [System.IO.Path]::GetFullPath((Get-S1WorkspaceRoot)))
    if ([string]::Equals($full, $workspaceRoot, [System.StringComparison]::OrdinalIgnoreCase)) {
        Throw-S1Preflight -Code 2 -Message "GameDir points at the repository root ('$full')."
    }
    if ($full.StartsWith($workspaceRoot + [System.IO.Path]::DirectorySeparatorChar,
            [System.StringComparison]::OrdinalIgnoreCase)) {
        Throw-S1Preflight -Code 2 -Message "GameDir is inside the repository ('$full'); refusing to deploy into the repo."
    }

    if (-not (Test-Path -LiteralPath $full -PathType Container)) {
        Throw-S1Preflight -Code 3 -Message "Schedule I install not found at '$full'."
    }

    # Same probe the rest of the repo uses: without MelonLoader/IL2CPP assemblies
    # this is not a deployable install, and a stale/half-created directory must
    # not silently receive DLLs.
    $probe = Join-Path $full 'MelonLoader\Il2CppAssemblies\Assembly-CSharp.dll'
    if (-not (Test-Path -LiteralPath $probe -PathType Leaf)) {
        Throw-S1Preflight -Code 3 -Message "Not a Schedule I + MelonLoader install: missing probe '$probe'."
    }

    $modsDir = Join-Path $full 'Mods'
    if (Test-Path -LiteralPath $modsDir) {
        if (-not (Test-Path -LiteralPath $modsDir -PathType Container)) {
            Throw-S1Preflight -Code 3 -Message "Deploy target '$modsDir' exists but is not a directory."
        }
        $modsItem = Get-Item -LiteralPath $modsDir -Force
        if (($modsItem.Attributes -band [System.IO.FileAttributes]::ReparsePoint) -ne 0) {
            Throw-S1Preflight -Code 3 -Message "Deploy target '$modsDir' is a reparse point/symlink; refusing an ambiguous deploy target."
        }
    }

    $userDataDir = Join-Path $full 'UserData'
    if (Test-Path -LiteralPath $userDataDir) {
        if (-not (Test-Path -LiteralPath $userDataDir -PathType Container)) {
            Throw-S1Preflight -Code 3 -Message "Deploy target '$userDataDir' exists but is not a directory."
        }
        $userDataItem = Get-Item -LiteralPath $userDataDir -Force
        if (($userDataItem.Attributes -band [System.IO.FileAttributes]::ReparsePoint) -ne 0) {
            Throw-S1Preflight -Code 3 -Message "Deploy target '$userDataDir' is a reparse point/symlink; refusing an ambiguous deploy target."
        }
    }

    return [pscustomobject]@{
        GameDir      = $full
        ModsDir      = $modsDir
        UserDataDir  = $userDataDir
        ProbePath    = $probe
        WorkspaceRoot = $workspaceRoot
    }
}

function Get-S1GameProcesses {
    # Fail closed: if process enumeration itself fails we cannot prove the game
    # is closed, so the caller must abort.
    try {
        $processes = @(Get-Process -ErrorAction Stop)
    }
    catch {
        Throw-S1Preflight -Code 5 -Message "Unable to enumerate processes; refusing to deploy (fail-closed): $($_.Exception.Message)"
    }

    $names = @('Schedule I', 'ScheduleI')
    return @($processes | Where-Object { $names -contains $_.ProcessName })
}

function Assert-S1GameNotRunning {
    $running = @(Get-S1GameProcesses)
    if ($running.Count -gt 0) {
        $ids = ($running | ForEach-Object { $_.Id }) -join ', '
        Throw-S1Preflight -Code 4 -Message "Schedule I is running (PID $ids). Close the game before deploying; no files were copied."
    }
}

function Invoke-S1DeployPreflight {
    [CmdletBinding()]
    param(
        [string]$GameDir,
        [string]$Operation = 'Mod'
    )

    $context = Resolve-S1GameDir -GameDir $GameDir
    Assert-S1GameNotRunning

    return [pscustomobject]@{
        Status       = 'Ok'
        Operation    = $Operation
        GameDir      = $context.GameDir
        ModsDir      = $context.ModsDir
        UserDataDir  = $context.UserDataDir
        GameRunning  = $false
        CheckedAtUtc = [DateTime]::UtcNow.ToString('o')
    }
}

if ($MyInvocation.InvocationName -ne '.') {
    try {
        $result = Invoke-S1DeployPreflight -GameDir $S1GameDir -Operation $S1Operation
        if ($S1Json) {
            $result | ConvertTo-Json -Compress -Depth 4
        }
        else {
            Write-Host "[deploy-preflight] OK ($S1Operation) -> $($result.GameDir)" -ForegroundColor Green
        }
        exit 0
    }
    catch {
        $code = 2
        if ($null -ne $_.Exception.Data -and $_.Exception.Data.Contains('S1ExitCode')) {
            $code = [int]$_.Exception.Data['S1ExitCode']
        }
        $message = $_.Exception.Message
        if ($S1Json) {
            [pscustomobject]@{
                Status    = 'Failed'
                Operation = $S1Operation
                ExitCode  = $code
                Error     = $message
            } | ConvertTo-Json -Compress -Depth 4
        }
        else {
            Write-Host "[deploy-preflight] ERROR: $message" -ForegroundColor Red
        }
        exit $code
    }
}
