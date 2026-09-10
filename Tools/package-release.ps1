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

# Fail fast ohne Spiel-Assemblies: dotnet build braucht MelonLoader/Il2CppAssemblies
# (gleiche Aufloesung wie Directory.Build.props: $env:SCHEDULE1_PATH sonst Defaultpfad).
$gameDir = if ($env:SCHEDULE1_PATH) { $env:SCHEDULE1_PATH } else { 'C:\Program Files (x86)\Steam\steamapps\common\Schedule I' }
$probeRef = Join-Path $gameDir 'MelonLoader\Il2CppAssemblies\Assembly-CSharp.dll'
if (-not (Test-Path -LiteralPath $probeRef)) {
    Write-Error "Spiel-Assemblies nicht gefunden (gesucht: $probeRef). Release-Packaging braucht eine Schedule-I-Installation — `$env:SCHEDULE1_PATH setzen oder lokal/self-hosted bauen. Siehe AGENTS.md."
    exit 1
}

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
    # Staging spiegelt die Deploy-Konvention (Directory.Build.targets, seit 2026-09):
    #   Mods\              -> DLL + PNGs + Bundles (nur von MelonLoader ladbare Dateien)
    #   UserData\<Mod>\    -> mod.json (Metadaten) + <Mod>.pdb (Debug-Symbole)
    # JSON/PDB gehoeren NIEMALS nach Mods\.
    $stagingModsDir = Join-Path $tempDir 'Mods'
    $stagingUserDataDir = Join-Path $tempDir "UserData\$modName"
    New-Item -ItemType Directory -Path $stagingModsDir -Force | Out-Null
    New-Item -ItemType Directory -Path $stagingUserDataDir -Force | Out-Null

    # Copy DLL -> Mods\
    Copy-Item -Path $dllPath -Destination $stagingModsDir -Force

    # Copy mod.json as mod.json -> UserData\<Mod>\
    if (Test-Path $modJsonPath) {
        Copy-Item -Path $modJsonPath -Destination (Join-Path $stagingUserDataDir "mod.json") -Force
    }

    # Copy PDB -> UserData\<Mod>\ (wenn vom Build erzeugt)
    $pdbPath = Join-Path (Split-Path -Parent $dllPath) "$modName.pdb"
    if (Test-Path $pdbPath) {
        Copy-Item -Path $pdbPath -Destination $stagingUserDataDir -Force
    }

    # Copy PNGs (src\*.png + assets\*.png, vgl. Directory.Build.targets) -> Mods\
    $pngs = @()
    $pngs += Get-ChildItem -Path (Join-Path $modDir.FullName 'src') -Filter '*.png' -File -ErrorAction SilentlyContinue
    $assetsDir = Join-Path $modDir.FullName 'assets'
    if (Test-Path $assetsDir) {
        $pngs += Get-ChildItem -Path $assetsDir -Filter '*.png' -File -ErrorAction SilentlyContinue
    }
    foreach ($png in $pngs) {
        Copy-Item -Path $png.FullName -Destination $stagingModsDir -Force
    }

    # Copy AssetBundles from assets/ -> Mods\
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
