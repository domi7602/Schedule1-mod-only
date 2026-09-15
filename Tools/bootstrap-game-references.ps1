[CmdletBinding()]
param(
    [string]$GameDir = $(if ($env:SCHEDULE1_PATH) { $env:SCHEDULE1_PATH } else { 'C:\Program Files (x86)\Steam\steamapps\common\Schedule I' }),
    [string]$OutputDir = 'GameReferences\decompiled',
    [switch]$Force
)

$ErrorActionPreference = 'Stop'

$workspaceRoot = Split-Path -Parent $PSScriptRoot
$assembliesDir = Join-Path $GameDir 'MelonLoader\Il2CppAssemblies'
$outputRoot = Join-Path $workspaceRoot $OutputDir
$toolDir = Join-Path $workspaceRoot '.tools'
$ilspycmd = Join-Path $toolDir 'ilspycmd.exe'

if (-not (Test-Path -LiteralPath $assembliesDir)) {
    throw "IL2CPP assemblies not found: $assembliesDir"
}

if (-not (Test-Path -LiteralPath $ilspycmd)) {
    New-Item -ItemType Directory -Path $toolDir -Force | Out-Null
    dotnet tool install ilspycmd --tool-path $toolDir
    if ($LASTEXITCODE -ne 0) {
        throw 'Failed to install ilspycmd.'
    }
}

foreach ($assemblyName in 'Assembly-CSharp.dll', 'Assembly-CSharp-firstpass.dll') {
    $assemblyPath = Join-Path $assembliesDir $assemblyName
    if (-not (Test-Path -LiteralPath $assemblyPath)) {
        Write-Warning "Skipping missing assembly: $assemblyPath"
        continue
    }

    $targetDir = Join-Path $outputRoot ([System.IO.Path]::GetFileNameWithoutExtension($assemblyName))
    if (Test-Path -LiteralPath $targetDir) {
        if (-not $Force) {
            throw "Output already exists: $targetDir. Re-run with -Force to replace it."
        }
        Remove-Item -LiteralPath $targetDir -Recurse -Force
    }

    Write-Host "Decompiling $assemblyName -> $targetDir" -ForegroundColor Cyan
    & $ilspycmd --outputdir $targetDir $assemblyPath
    if ($LASTEXITCODE -ne 0) {
        throw "ilspycmd failed for $assemblyName (exit $LASTEXITCODE)."
    }
}

Write-Host "Game references ready: $outputRoot" -ForegroundColor Green
