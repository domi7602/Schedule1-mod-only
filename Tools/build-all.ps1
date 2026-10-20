# Builds every mod (Release by default) and deploys to <GameDir>\Mods\.
#
# Usage:
#   pwsh Tools/build-all.ps1                      # build + auto-deploy (default)
#   pwsh Tools/build-all.ps1 -Config Debug
#   pwsh Tools/build-all.ps1 -NoDeploy            # build only
#   pwsh Tools/build-all.ps1 -GameDir "D:\Games\Schedule I"
#
# NoDeploy is effective when the switch is present OR S1NoDeploy is already
# 'true' in the environment. The effective value is passed to MSBuild as
# -p:S1NoDeploy=... so MSBuild sees exactly the value computed here. Deploy
# preflight (game present, game not running, safe target) runs before the build
# when deployment is enabled.

[CmdletBinding()]
param(
    [ValidateSet('Debug', 'Release')]
    [string]$Config = 'Release',

    [switch]$NoDeploy,

    [string]$GameDir,

    [string]$DotnetPath
)

$ErrorActionPreference = 'Stop'

function Get-S1EffectiveNoDeploy {
    param(
        [bool]$NoDeploySwitch,
        [AllowNull()][string]$EnvironmentValue
    )
    if ($NoDeploySwitch) { return $true }
    return ([string]$EnvironmentValue).Trim() -eq 'true'
}

function Resolve-S1DotnetPath {
    param([string]$Requested)

    if (-not [string]::IsNullOrWhiteSpace($Requested)) {
        if (Test-Path -LiteralPath $Requested) {
            return (Resolve-Path -LiteralPath $Requested).Path
        }
        $command = Get-Command $Requested -ErrorAction SilentlyContinue
        if ($command) { return $command.Source }
        throw "dotnet not found at '$Requested'."
    }

    # Same fallback order as Tools/test-all.ps1: user-local SDK first, then PATH.
    $userLocal = Join-Path $env:USERPROFILE '.dotnet\dotnet.exe'
    if (Test-Path -LiteralPath $userLocal) { return $userLocal }

    $command = Get-Command dotnet -ErrorAction SilentlyContinue
    if ($command) { return $command.Source }

    throw "dotnet not found (checked '$userLocal' and PATH)."
}

function Assert-S1DotnetSdk {
    param([Parameter(Mandatory)][string]$DotnetPath)

    $raw = @(& $DotnetPath --list-sdks 2>&1)
    if ($LASTEXITCODE -ne 0) {
        throw "dotnet --list-sdks failed (exit $LASTEXITCODE): $($raw -join ' ')"
    }

    $versions = @()
    foreach ($line in $raw) {
        $text = [string]$line
        if ($text -match '^\s*(\d+)\.(\d+)\.(\d+)') {
            $versions += [version]::new([int]$Matches[1], [int]$Matches[2], [int]$Matches[3])
        }
    }
    if ($versions.Count -eq 0) {
        throw "No .NET SDK found by '$DotnetPath --list-sdks'. Install the .NET 8+ SDK (C# 12)."
    }

    $selected = @(& $DotnetPath --version 2>&1)
    if ($LASTEXITCODE -ne 0 -or ($selected -join '').Trim() -notmatch '^(\d+)\.') {
        throw "Unable to resolve the active .NET SDK: $($selected -join ' ')"
    }
    if ([int]$Matches[1] -lt 8) {
        throw "The active .NET SDK must be 8 or newer for C# 12; selected: $($selected -join ' ')"
    }
    return $versions
}

function New-S1BuildArguments {
    param(
        [Parameter(Mandatory)][string]$Solution,
        [Parameter(Mandatory)][string]$Config,
        [bool]$NoDeploy,
        [string]$GameDir
    )

    $buildArgs = @('build', $Solution, '-c', $Config, '--nologo')
    $buildArgs += "-p:S1NoDeploy=$($NoDeploy.ToString().ToLowerInvariant())"
    if (-not [string]::IsNullOrWhiteSpace($GameDir)) {
        $buildArgs += "-p:GameDir=$($GameDir.Trim())"
    }
    return $buildArgs
}

function Invoke-S1Build {
    $workspaceRoot = Split-Path $PSScriptRoot -Parent
    $modRoot = Join-Path $workspaceRoot 'Source\Mods'
    $solution = Join-Path $modRoot 'S1Mods.sln'
    $genSolution = Join-Path $PSScriptRoot 'gen-sln.ps1'
    $preflight = Join-Path $PSScriptRoot 'check-deploy-preflight.ps1'

    $noDeployEffective = Get-S1EffectiveNoDeploy -NoDeploySwitch $NoDeploy.IsPresent -EnvironmentValue $env:S1NoDeploy
    $dotnet = Resolve-S1DotnetPath -Requested $DotnetPath
    $sdkVersions = Assert-S1DotnetSdk -DotnetPath $dotnet

    Write-Host "dotnet      : $dotnet" -ForegroundColor DarkGray
    Write-Host "SDK versions: $(($sdkVersions | ForEach-Object ToString) -join ', ')" -ForegroundColor DarkGray
    Write-Host "Deploy      : $(if ($noDeployEffective) { 'disabled (NoDeploy)' } else { 'enabled (default)' })" -ForegroundColor DarkGray

    if (-not (Test-Path -LiteralPath $solution)) {
        Write-Host "Solution missing — generating..." -ForegroundColor Yellow
        if (-not (Test-Path -LiteralPath $genSolution)) {
            throw "Solution '$solution' is missing and '$genSolution' does not exist."
        }
        & $genSolution
        if ($LASTEXITCODE -ne 0) {
            throw "gen-sln.ps1 failed (exit $LASTEXITCODE)."
        }
    }

    if (-not $noDeployEffective) {
        if (-not (Test-Path -LiteralPath $preflight)) {
            throw "Deploy preflight script missing: '$preflight'."
        }
        $preflightArgs = @('-NoProfile', '-NonInteractive', '-ExecutionPolicy', 'Bypass', '-File', $preflight, '-Operation', 'Build')
        if (-not [string]::IsNullOrWhiteSpace($GameDir)) {
            $preflightArgs += @('-GameDir', $GameDir.Trim())
        }
        & pwsh @preflightArgs
        if ($LASTEXITCODE -ne 0) {
            throw "Deploy preflight failed (exit $LASTEXITCODE); build aborted before any copy."
        }
    }

    $buildArgs = New-S1BuildArguments -Solution $solution -Config $Config -NoDeploy $noDeployEffective -GameDir $GameDir
    Write-Host "Building $solution ($Config)..." -ForegroundColor Cyan
    & $dotnet @buildArgs
    if ($LASTEXITCODE -ne 0) {
        throw "Build failed (exit $LASTEXITCODE)."
    }
    Write-Host "Build OK." -ForegroundColor Green
}

if ($MyInvocation.InvocationName -ne '.') {
    Invoke-S1Build
}
