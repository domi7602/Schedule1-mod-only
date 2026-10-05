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
$toolDir = Join-Path $workspaceRoot '.cache\tools'
$ilspycmd = Join-Path $toolDir 'ilspycmd.exe'

if (-not (Test-Path -LiteralPath $assembliesDir)) {
    throw "IL2CPP assemblies not found: $assembliesDir"
}

if (-not (Test-Path -LiteralPath $ilspycmd)) {
    New-Item -ItemType Directory -Path $toolDir -Force | Out-Null
    # Pinned version: bare `dotnet tool install ilspycmd` pulls 10.x/11.x which fails on SDK 8
    # (see schedule1-lifecycle-verify Pitfall 6). 9.1.0.7988 is the last line that installs and runs on net8.
    dotnet tool install ilspycmd --tool-path $toolDir --version 9.1.0.7988
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
    # -p = project mode: per-type .cs files + .csproj (matches the documented GameReferences layout;
    # without -p ilspycmd 8.2+ writes ONE monolithic file, which breaks all `GameReferences/decompiled/
    # Assembly-CSharp/Il2CppScheduleOne/<Area>/<Type>.cs` references in skills and docs).
    & $ilspycmd --outputdir $targetDir -p $assemblyPath
    if ($LASTEXITCODE -ne 0) {
        throw "ilspycmd failed for $assemblyName (exit $LASTEXITCODE)."
    }
}

Write-Host "Game references ready: $outputRoot" -ForegroundColor Green
