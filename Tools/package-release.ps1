[CmdletBinding()]
param (
    [Parameter(Position = 0)]
    [string]$Mod = 'All',

    [Parameter()]
    [string]$OutputDir = 'Release'
)

$ErrorActionPreference = 'Stop'

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$workspaceRoot = Split-Path -Parent $scriptDir
$modsRoot = Join-Path $workspaceRoot 'Source\Mods'
$releaseDir = Join-Path $workspaceRoot $OutputDir

if (-not (Test-Path $releaseDir)) {
    New-Item -ItemType Directory -Path $releaseDir -Force | Out-Null
}

# Determine list of mods to package
$allMods = Get-ChildItem -Path $modsRoot -Directory | Where-Object {
    $_.Name -ne 'Shared' -and $_.Name -ne 'Archive' -and (Test-Path (Join-Path $_.FullName "src\$($_.Name).csproj"))
}

$targetMods = @()
if ($Mod -eq 'All' -or [string]::IsNullOrWhiteSpace($Mod)) {
    $targetMods = $allMods
} else {
    $found = $allMods | Where-Object { $_.Name -eq $Mod }
    if (-not $found) {
        Write-Error "Mod '$Mod' not found in $modsRoot"
        return
    }
    $targetMods = @($found)
}

Write-Host "=== Packaging Schedule I Mods for Release ===" -ForegroundColor Cyan
Write-Host "Target Mods count: $($targetMods.Count)" -ForegroundColor Gray

foreach ($modDir in $targetMods) {
    $modName = $modDir.Name
    $csprojPath = Join-Path $modDir.FullName "src\$modName.csproj"
    $docsPath = Join-Path $modDir.FullName 'docs'
    $modJsonPath = Join-Path $docsPath 'mod.json'
    
    # Read version
    $version = '1.0.0'
    if (Test-Path $modJsonPath) {
        try {
            $jsonContent = Get-Content $modJsonPath -Raw | ConvertFrom-Json
            if ($jsonContent.version) {
                $version = $jsonContent.version
            }
        } catch {}
    }

    Write-Host "`n--> Building $modName (v$version)..." -ForegroundColor Yellow
    dotnet build $csprojPath -c Release --nologo | Out-Null

    $dllPath = Join-Path $modDir.FullName "src\bin\Release\net6.0\$modName.dll"
    if (-not (Test-Path $dllPath)) {
        Write-Error "Build output DLL not found at: $dllPath"
        continue
    }

    # Prepare temp packaging staging dir
    $tempDir = Join-Path ([System.IO.Path]::GetTempPath()) "S1Release_$modName"
    if (Test-Path $tempDir) { Remove-Item -Path $tempDir -Recurse -Force }
    $stagingModsDir = Join-Path $tempDir 'Mods'
    New-Item -ItemType Directory -Path $stagingModsDir -Force | Out-Null

    # Copy DLL
    Copy-Item -Path $dllPath -Destination $stagingModsDir -Force

    # Copy mod.json as <ModName>.json
    if (Test-Path $modJsonPath) {
        Copy-Item -Path $modJsonPath -Destination (Join-Path $stagingModsDir "$modName.json") -Force
    }

    # Copy PNGs from src/
    $pngs = Get-ChildItem -Path (Join-Path $modDir.FullName 'src') -Filter '*.png' -File
    foreach ($png in $pngs) {
        Copy-Item -Path $png.FullName -Destination $stagingModsDir -Force
    }

    # Copy AssetBundles from assets/
    $assetsDir = Join-Path $modDir.FullName 'assets'
    if (Test-Path $assetsDir) {
        $bundles = Get-ChildItem -Path $assetsDir -Filter '*.bundle' -File
        foreach ($b in $bundles) {
            Copy-Item -Path $b.FullName -Destination $stagingModsDir -Force
        }
    }

    # Copy Docs (README, CHANGELOG) to root of zip
    $readme = Join-Path $docsPath 'README.md'
    if (Test-Path $readme) {
        Copy-Item -Path $readme -Destination $tempDir -Force
    }
    $changelog = Join-Path $docsPath 'CHANGELOG.md'
    if (Test-Path $changelog) {
        Copy-Item -Path $changelog -Destination $tempDir -Force
    }

    # Create Zip
    $zipFileName = "$modName-v$version.zip"
    $zipFilePath = Join-Path $releaseDir $zipFileName
    if (Test-Path $zipFilePath) {
        Remove-Item -Path $zipFilePath -Force
    }

    Compress-Archive -Path "$tempDir\*" -DestinationPath $zipFilePath -CompressionLevel Optimal
    Remove-Item -Path $tempDir -Recurse -Force

    $zipSize = (Get-Item $zipFilePath).Length / 1KB
    Write-Host "[OK] Created $zipFileName ($([Math]::Round($zipSize, 1)) KB) in $OutputDir" -ForegroundColor Green
}

Write-Host "`n=== Packaging Complete! ===" -ForegroundColor Cyan
