[CmdletBinding()]
param(
    [string]$GameDir = $(if ($env:SCHEDULE1_PATH) { $env:SCHEDULE1_PATH } else { 'C:\Program Files (x86)\Steam\steamapps\common\Schedule I' }),
    [string]$OutputDir = 'GameReferences\decompiled',
    [string]$IlSpyCmdVersion = '8.2.0.7535',
    [switch]$Force
)

# ilspycmd is pinned to the last net6.0-compatible release so the workspace's
# default .NET 6 toolchain can run it; newer majors require a newer runtime.
$ErrorActionPreference = 'Stop'

$workspaceRoot = Split-Path -Parent $PSScriptRoot
$assembliesDir = Join-Path $GameDir 'MelonLoader\Il2CppAssemblies'
$outputRoot = Join-Path $workspaceRoot $OutputDir
$toolDir = Join-Path $workspaceRoot '.cache\tools'
$ilspycmd = Join-Path $toolDir 'ilspycmd.exe'

if (-not (Test-Path -LiteralPath $assembliesDir)) {
    throw "IL2CPP assemblies not found: $assembliesDir"
}

function Test-IlSpyCmd {
    if (-not (Test-Path -LiteralPath $ilspycmd)) { return $false }
    try {
        & $ilspycmd --version *> $null
        return ($LASTEXITCODE -eq 0)
    } catch {
        return $false
    }
}

if (-not (Test-IlSpyCmd)) {
    New-Item -ItemType Directory -Path $toolDir -Force | Out-Null
    Write-Host "Installing ilspycmd $IlSpyCmdVersion into $toolDir ..." -ForegroundColor Cyan
    if (Test-Path -LiteralPath $ilspycmd) {
        dotnet tool update ilspycmd --tool-path $toolDir --version $IlSpyCmdVersion
        if ($LASTEXITCODE -ne 0) {
            # Broken or incompatible older installation: replace it.
            dotnet tool uninstall ilspycmd --tool-path $toolDir | Out-Null
            dotnet tool install ilspycmd --tool-path $toolDir --version $IlSpyCmdVersion
        }
    } else {
        dotnet tool install ilspycmd --tool-path $toolDir --version $IlSpyCmdVersion
    }
    if ($LASTEXITCODE -ne 0 -or -not (Test-IlSpyCmd)) {
        throw "Failed to install a runnable ilspycmd $IlSpyCmdVersion."
    }
}

foreach ($assemblyName in 'Assembly-CSharp.dll', 'Assembly-CSharp-firstpass.dll') {
    $assemblyPath = Join-Path $assembliesDir $assemblyName
    if (-not (Test-Path -LiteralPath $assemblyPath)) {
        Write-Warning "Skipping missing assembly: $assemblyPath"
        continue
    }

    $targetDir = Join-Path $outputRoot ([System.IO.Path]::GetFileNameWithoutExtension($assemblyName))
    $stagingDir = "$targetDir.tmp"
    if ((Test-Path -LiteralPath $targetDir) -and -not $Force) {
        throw "Output already exists: $targetDir. Re-run with -Force to replace it."
    }

    # Decompile into a staging folder and only replace the previous output on
    # success, so a failing tool (e.g. missing runtime) never destroys it.
    if (Test-Path -LiteralPath $stagingDir) { Remove-Item -LiteralPath $stagingDir -Recurse -Force }

    Write-Host "Decompiling $assemblyName -> $targetDir" -ForegroundColor Cyan
    & $ilspycmd --nested-directories --project --outputdir $stagingDir --disable-updatecheck $assemblyPath
    if ($LASTEXITCODE -ne 0) {
        Remove-Item -LiteralPath $stagingDir -Recurse -Force -ErrorAction SilentlyContinue
        throw "ilspycmd failed for $assemblyName (exit $LASTEXITCODE). Previous output kept."
    }

    if (Test-Path -LiteralPath $targetDir) { Remove-Item -LiteralPath $targetDir -Recurse -Force }
    Move-Item -LiteralPath $stagingDir -Destination $targetDir
}

Write-Host "Game references ready: $outputRoot" -ForegroundColor Green
