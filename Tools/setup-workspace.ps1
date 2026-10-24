#Requires -Version 7
<#
.SYNOPSIS
    setup-workspace — generates local.build.props for S1API and S1MAPI
    on a freshly cloned machine, prompts for the game path if needed,
    optionally activates the pre-commit hook.

.DESCRIPTION
    Idempotent: re-running refreshes the machine-local dependency props.
    Both required templates and the game references are validated before any
    props are written. Hook configuration is opt-in and local to this checkout.

    Used both on the very first setup of a PC and after switching
    between machines with different Steam install paths.

.EXAMPLE
    pwsh Tools/setup-workspace.ps1
    pwsh Tools/setup-workspace.ps1 -GameDir "D:\SteamLibrary\steamapps\common\Schedule I"
#>
[CmdletBinding()]
param(
    [string]$GameDir,
    [switch]$EnableHooks,
    [switch]$NonInteractive
)

$ErrorActionPreference = 'Stop'
$PSNativeCommandUseErrorActionPreference = $false

$workspaceRoot = Split-Path -Parent $PSScriptRoot

function Resolve-GameDir {
    param([string]$Candidate)

    if ($Candidate) {
        if (-not (Test-Path -LiteralPath $Candidate -PathType Container)) {
            throw "Game directory not found: $Candidate"
        }
        return [IO.Path]::GetFullPath($Candidate)
    }

    # 1. SCHEDULE1_PATH env var
    if ($env:SCHEDULE1_PATH -and (Test-Path -LiteralPath $env:SCHEDULE1_PATH -PathType Container)) {
        Write-Host "[setup-workspace] Using SCHEDULE1_PATH: $env:SCHEDULE1_PATH" -ForegroundColor DarkGray
        return [IO.Path]::GetFullPath($env:SCHEDULE1_PATH)
    }

    # 2. Default
    $default = 'C:\Program Files (x86)\Steam\steamapps\common\Schedule I'
    if (Test-Path -LiteralPath $default -PathType Container) {
        Write-Host "[setup-workspace] Using default Steam path: $default" -ForegroundColor DarkGray
        return $default
    }

    if ($NonInteractive) { throw 'Game directory unavailable. Pass -GameDir or set SCHEDULE1_PATH for noninteractive setup.' }

    # 3. Interactively ask
    Write-Host "[setup-workspace] Schedule I not found in default location or SCHEDULE1_PATH." -ForegroundColor Yellow
    do {
        $input = Read-Host "  Enter the full path to your Schedule I install (e.g. D:\SteamLibrary\steamapps\common\Schedule I)"
        $input = $input.Trim().TrimEnd('\')
        if (-not (Test-Path -LiteralPath $input -PathType Container)) {
            Write-Host "  Not found: $input" -ForegroundColor Red
        }
    } while (-not (Test-Path -LiteralPath $input -PathType Container))
    return [IO.Path]::GetFullPath($input)
}

# Validate every required input before writing either dependency's props.
$templates = @(
    @{ Source = 'ThirdParty/S1API/example.build.props'; Target = 'ThirdParty/S1API/local.build.props' },
    @{ Source = 'ThirdParty/S1MAPI/local.build.props.example'; Target = 'ThirdParty/S1MAPI/local.build.props' }
)
foreach ($template in $templates) {
    $source = Join-Path $workspaceRoot $template.Source
    $target = Join-Path $workspaceRoot $template.Target
    if (-not (Test-Path -LiteralPath $source -PathType Leaf)) {
        throw "Required dependency template missing: $($template.Source). Run git submodule update --init --recursive before setup. No props were written."
    }
    if ((Test-Path -LiteralPath $target) -and -not (Test-Path -LiteralPath $target -PathType Leaf)) {
        throw "Expected a props file, not a directory: $target. No props were written."
    }
    $xml = [Xml.XmlDocument]::new()
    $xml.PreserveWhitespace = $true
    $xml.XmlResolver = $null
    $xml.LoadXml([IO.File]::ReadAllText($source))
    if (-not $xml.SelectSingleNode('/Project/PropertyGroup/Il2CppAssembliesPath')) {
        throw "Required IL2CPP property missing from $($template.Source). No props were written."
    }
    $template.Document = $xml
}

# Game path + smoke test (read-only; no game is started).

$GameDir = Resolve-GameDir -Candidate $GameDir
Write-Host "[setup-workspace] GameDir: $GameDir" -ForegroundColor Cyan

$probe = Join-Path $GameDir 'MelonLoader\Il2CppAssemblies\Assembly-CSharp.dll'
if (-not (Test-Path -LiteralPath $probe -PathType Leaf)) {
    throw "MelonLoader Il2Cpp assemblies missing at '$GameDir\MelonLoader\Il2CppAssemblies'. Is MelonLoader installed in this Schedule I directory?"
}
$probe2 = Join-Path $GameDir 'MelonLoader\net6\MelonLoader.dll'
if (-not (Test-Path -LiteralPath $probe2 -PathType Leaf)) {
    throw "MelonLoader net6 runtime missing at '$GameDir\MelonLoader\net6'. Is MelonLoader 0.7.3 (net6) installed?"
}
Write-Host "[setup-workspace] MelonLoader smoke test OK (Assembly-CSharp + MelonLoader.dll present)" -ForegroundColor Green

# Read Git configuration before writes; exit 1 means an unset key, not failure.
$git = (Get-Command git -CommandType Application -ErrorAction Stop | Select-Object -First 1).Source
$hooksPath = & $git -C $workspaceRoot config --get core.hooksPath 2>&1
if ($LASTEXITCODE -notin @(0, 1)) { throw "Reading core.hooksPath failed (exit $LASTEXITCODE): $hooksPath. No props were written." }
if ($LASTEXITCODE -eq 1) { $hooksPath = $null }

$paths = @{
    MelonLoaderAssembliesPath = Join-Path $GameDir 'MelonLoader/net6'
    MelonLoaderMonoAssembliesPath = Join-Path $GameDir 'MelonLoader/net35'
    MonoAssembliesPath = Join-Path $GameDir 'Schedule I_Data/Managed'
    LocalMonoDeploymentPath = $GameDir
    LocalIl2CppDeploymentPath = $GameDir
    Il2CppAssembliesPath = Join-Path $GameDir 'MelonLoader/Il2CppAssemblies'
    UserLibsPath = Join-Path $GameDir 'UserLibs'
}
foreach ($template in $templates) {
    foreach ($name in $paths.Keys) {
        foreach ($node in $template.Document.SelectNodes("/Project/PropertyGroup/$name")) {
            # InnerText handles XML escaping and literal '$' in installation paths.
            $node.InnerText = $paths[$name]
        }
    }
    $template.Content = $template.Document.OuterXml
}
foreach ($template in $templates) {
    $target = Join-Path $workspaceRoot $template.Target
    [IO.File]::WriteAllText($target, $template.Content, [Text.UTF8Encoding]::new($false))
    Write-Host "[setup-workspace] Wrote $($template.Target)" -ForegroundColor Cyan
}

# Existing hook choices are preserved. New activation is always explicit.
if (-not $hooksPath -and -not $EnableHooks -and -not $NonInteractive) {
    Write-Host ""
    $answer = Read-Host '[setup-workspace] Activate .githooks (pre-commit Quick, pre-push Auto)? [y/N]'
    $EnableHooks = $answer -match '^[Yy]'
}
if ($hooksPath) {
    Write-Host "[setup-workspace] core.hooksPath already set to '$hooksPath'" -ForegroundColor DarkGray
}
elseif ($EnableHooks) {
    & $git -C $workspaceRoot config --local core.hooksPath .githooks
    if ($LASTEXITCODE -ne 0) { throw "Enabling local Git hooks failed (exit $LASTEXITCODE). Props were written; hook activation did not complete." }
    Write-Host '[setup-workspace] core.hooksPath set to .githooks' -ForegroundColor Green
}
else { Write-Host '[setup-workspace] Hooks unchanged. Use -EnableHooks to opt in.' -ForegroundColor DarkGray }

# --- 5. Summary --------------------------------------------------------------

Write-Host ""
Write-Host "[setup-workspace] Done." -ForegroundColor Green
Write-Host "  Next steps:"
Write-Host '    pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/verify.ps1 -Mode Auto'
Write-Host '  Setup did not build, deploy or start the game. Close the game before verification builds.'
