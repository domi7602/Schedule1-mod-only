#Requires -Version 7
<#
.SYNOPSIS
    setup-workspace — generates local.build.props for S1API and S1MAPI
    on a freshly cloned machine, prompts for the game path if needed,
    optionally activates the pre-commit hook.

.DESCRIPTION
    Idempotent: re-running just refreshes the local.build.props files.
    Writes are confined to gitignored paths, so this script never
    produces a dirty working tree in the repo.

    Used both on the very first setup of a PC and after switching
    between machines with different Steam install paths.

.EXAMPLE
    pwsh Tools/setup-workspace.ps1
    pwsh Tools/setup-workspace.ps1 -GameDir "D:\SteamLibrary\steamapps\common\Schedule I"
#>
[CmdletBinding()]
param(
    [string]$GameDir
)

$ErrorActionPreference = 'Stop'

$workspaceRoot = Split-Path -Parent $PSScriptRoot

function Resolve-GameDir {
    param([string]$Candidate)

    if ($Candidate) {
        if (-not (Test-Path -LiteralPath $Candidate)) {
            throw "Game directory not found: $Candidate"
        }
        return $Candidate
    }

    # 1. SCHEDULE1_PATH env var
    if ($env:SCHEDULE1_PATH -and (Test-Path -LiteralPath $env:SCHEDULE1_PATH)) {
        Write-Host "[setup-workspace] Using SCHEDULE1_PATH: $env:SCHEDULE1_PATH" -ForegroundColor DarkGray
        return $env:SCHEDULE1_PATH
    }

    # 2. Default
    $default = 'C:\Program Files (x86)\Steam\steamapps\common\Schedule I'
    if (Test-Path -LiteralPath $default) {
        Write-Host "[setup-workspace] Using default Steam path: $default" -ForegroundColor DarkGray
        return $default
    }

    # 3. Interactively ask
    Write-Host "[setup-workspace] Schedule I not found in default location or SCHEDULE1_PATH." -ForegroundColor Yellow
    do {
        $input = Read-Host "  Enter the full path to your Schedule I install (e.g. D:\SteamLibrary\steamapps\common\Schedule I)"
        $input = $input.Trim().TrimEnd('\')
        if (-not (Test-Path -LiteralPath $input)) {
            Write-Host "  Not found: $input" -ForegroundColor Red
        }
    } while (-not (Test-Path -LiteralPath $input))
    return $input
}

# --- 1. Game path + smoke test -----------------------------------------------

$GameDir = Resolve-GameDir -Candidate $GameDir
Write-Host "[setup-workspace] GameDir: $GameDir" -ForegroundColor Cyan

$probe = Join-Path $GameDir 'MelonLoader\Il2CppAssemblies\Assembly-CSharp.dll'
if (-not (Test-Path -LiteralPath $probe)) {
    throw "MelonLoader Il2Cpp assemblies missing at '$GameDir\MelonLoader\Il2CppAssemblies'. Is MelonLoader installed in this Schedule I directory?"
}
$probe2 = Join-Path $GameDir 'MelonLoader\net6\MelonLoader.dll'
if (-not (Test-Path -LiteralPath $probe2)) {
    throw "MelonLoader net6 runtime missing at '$GameDir\MelonLoader\net6'. Is MelonLoader 0.7.3 (net6) installed?"
}
Write-Host "[setup-workspace] MelonLoader smoke test OK (Assembly-CSharp + MelonLoader.dll present)" -ForegroundColor Green

# --- 2. S1API local.build.props ----------------------------------------------

$s1ApiExample = Join-Path $workspaceRoot 'ThirdParty\S1API\example.build.props'
$s1ApiTarget  = Join-Path $workspaceRoot 'ThirdParty\S1API\local.build.props'

if (Test-Path -LiteralPath $s1ApiExample) {
    if (Test-Path -LiteralPath $s1ApiTarget) {
        Write-Host "[setup-workspace] S1API local.build.props already exists, refreshing." -ForegroundColor DarkGray
    } else {
        Write-Host "[setup-workspace] Creating ThirdParty/S1API/local.build.props" -ForegroundColor Cyan
    }
    $s1ApiContent = Get-Content -LiteralPath $s1ApiExample -Raw
    # Replace any path that mentions 'C:\Program Files (x86)\Steam\steamapps\common\Schedule I' with $GameDir.
    # The example template uses the canonical default; we just normalize it to whatever $GameDir resolved to.
    $s1ApiContent = $s1ApiContent -replace [regex]::Escape('C:\Program Files (x86)\Steam\steamapps\common\Schedule I'), $GameDir
    Set-Content -LiteralPath $s1ApiTarget -Value $s1ApiContent -Encoding UTF8 -NoNewline
} else {
    Write-Host "[setup-workspace] WARN: ThirdParty/S1API/example.build.props not found (submodule not initialized?). Skipping S1API props." -ForegroundColor Yellow
}

# --- 3. S1MAPI local.build.props ---------------------------------------------

$s1MapiExample = Join-Path $workspaceRoot 'ThirdParty\S1MAPI\local.build.props.example'
$s1MapiTarget  = Join-Path $workspaceRoot 'ThirdParty\S1MAPI\local.build.props'

if (Test-Path -LiteralPath $s1MapiExample) {
    if (Test-Path -LiteralPath $s1MapiTarget) {
        Write-Host "[setup-workspace] S1MAPI local.build.props already exists, refreshing." -ForegroundColor DarkGray
    } else {
        Write-Host "[setup-workspace] Creating ThirdParty/S1MAPI/local.build.props" -ForegroundColor Cyan
    }
    $s1MapiContent = Get-Content -LiteralPath $s1MapiExample -Raw
    # The S1MAPI example template targets an 'alternate' Steam branch path —
    # we rewrite every Steam-related path to $GameDir.
    $s1MapiContent = $s1MapiContent `
        -replace [regex]::Escape('D:\SteamLibrary\steamapps\common\Schedule I_alternate'), $GameDir `
        -replace [regex]::Escape('D:\SteamLibrary\steamapps\common\Schedule I_public'), $GameDir `
        -replace [regex]::Escape('C:\Program Files (x86)\Steam\steamapps\common\Schedule I'), $GameDir
    Set-Content -LiteralPath $s1MapiTarget -Value $s1MapiContent -Encoding UTF8 -NoNewline
} else {
    Write-Host "[setup-workspace] WARN: ThirdParty/S1MAPI/local.build.props.example not found. Skipping S1MAPI props." -ForegroundColor Yellow
}

# --- 4. Pre-commit hook (optional) -------------------------------------------

$hooksPath = git -C $workspaceRoot config --get core.hooksPath 2>$null
if (-not $hooksPath) {
    Write-Host ""
    $answer = Read-Host "[setup-workspace] Activate pre-commit hook (.githooks)? Runs format/version-sync/doc-paths checks before each commit. [y/N]"
    if ($answer -match '^[Yy]') {
        git -C $workspaceRoot config core.hooksPath .githooks
        Write-Host "[setup-workspace] core.hooksPath set to .githooks" -ForegroundColor Green
    } else {
        Write-Host "[setup-workspace] Skipped. Run later: git config core.hooksPath .githooks" -ForegroundColor DarkGray
    }
} else {
    Write-Host "[setup-workspace] core.hooksPath already set to '$hooksPath'" -ForegroundColor DarkGray
}

# --- 5. Summary --------------------------------------------------------------

Write-Host ""
Write-Host "[setup-workspace] Done." -ForegroundColor Green
Write-Host "  Next steps:"
Write-Host "    1. dotnet build Source/Mods/S1Mods.sln -c Release    # builds all 13 mods + frameworks"
Write-Host "    2. Start the game once to confirm MelonLoader logs the mods as loaded"
Write-Host "  Working tree stays clean: both local.build.props are gitignored."
