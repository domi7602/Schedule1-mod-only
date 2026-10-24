#Requires -Version 7
[CmdletBinding()]
param (
    [Parameter(Position = 0)][string]$Mod = 'All',
    [string]$OutputDir = 'Release'
)

$ErrorActionPreference = 'Stop'
. (Join-Path $PSScriptRoot 'release-manifest.ps1')
$workspaceRoot = Split-Path -Parent $PSScriptRoot
$modsRoot = Join-Path $workspaceRoot 'Source/Mods'
$releaseDir = [IO.Path]::GetFullPath($OutputDir, $workspaceRoot)
$gameDir = if ($env:SCHEDULE1_PATH) { $env:SCHEDULE1_PATH } else { 'C:\Program Files (x86)\Steam\steamapps\common\Schedule I' }
$probeRef = Join-Path $gameDir 'MelonLoader/Il2CppAssemblies/Assembly-CSharp.dll'
if (-not (Test-Path -LiteralPath $probeRef -PathType Leaf)) {
    throw "Game assemblies missing ($probeRef). Packaging requires a local/self-hosted Schedule I installation; set SCHEDULE1_PATH."
}
if (Get-Process -Name 'Schedule I' -ErrorAction SilentlyContinue) { throw 'Close Schedule I before building release packages.' }

$allMods = @(Get-ChildItem -LiteralPath $modsRoot -Directory | Where-Object {
    $_.Name -notin @('Shared', 'Archive', '_DiagPerfCounter') -and
    (Test-Path -LiteralPath (Join-Path $_.FullName "src/$($_.Name).csproj") -PathType Leaf)
} | Sort-Object Name)
if ([string]::IsNullOrWhiteSpace($Mod) -or $Mod -eq 'All') { $targetMods = $allMods }
else {
    Assert-S1ReleaseModName $Mod
    $targetMods = @($allMods | Where-Object Name -CEQ $Mod)
    if ($targetMods.Count -ne 1) { throw "Mod '$Mod' not found in $modsRoot." }
}
# Validate metadata for every requested mod before building any of them. Never invent a version.
foreach ($modDir in $targetMods) {
    $null = Read-S1ReleaseMetadata (Join-Path $modDir.FullName 'docs/mod.json') $modDir.Name
}
$null = New-Item -ItemType Directory -Path $releaseDir -Force

foreach ($modDir in $targetMods) {
    $modName = $modDir.Name
    $docsPath = Join-Path $modDir.FullName 'docs'
    $modJsonPath = Join-Path $docsPath 'mod.json'
    $metadata = Read-S1ReleaseMetadata $modJsonPath $modName
    $source = Get-S1ReleaseSourceIdentity $workspaceRoot
    Write-Host "Building $modName v$($metadata.version); HEAD $($source.Head), dirty=$($source.Dirty)" -ForegroundColor Cyan
    # Rebuild prevents a timestamp-valid old output from being accepted as this source's artifact.
    & dotnet build (Join-Path $modDir.FullName "src/$modName.csproj") -t:Rebuild -c Release --nologo -p:S1NoDeploy=true
    if ($LASTEXITCODE -ne 0) { throw "Build failed for $modName (exit $LASTEXITCODE). Existing ZIP retained." }
    Assert-S1ReleaseSourceIdentity (Get-S1ReleaseSourceIdentity $workspaceRoot) $source

    $dllPath = Join-Path $modDir.FullName "src/bin/Release/net6.0/$modName.dll"
    $sharedDll = Join-Path $modsRoot 'Shared/src/bin/Release/net6.0/Shared.dll'
    foreach ($required in @($dllPath, $sharedDll)) {
        if (-not (Test-Path -LiteralPath $required -PathType Leaf) -or (Get-Item -LiteralPath $required).Length -le 0) {
            throw "Required package DLL is missing or empty: $required"
        }
    }
    $staging = New-S1ReleaseStagingDirectory
    $zipPath = Join-Path $releaseDir "$modName-v$($metadata.version).zip"
    $pendingZip = Join-Path $releaseDir ('.s1release-' + [guid]::NewGuid().ToString('N') + '.zip')
    try {
        $stagingMods = Join-Path $staging.Path 'Mods'
        $stagingData = Join-Path $staging.Path "UserData/$modName"
        $null = New-Item -ItemType Directory -Path $stagingMods, $stagingData -Force
        Copy-Item -LiteralPath $dllPath, $sharedDll -Destination $stagingMods
        Copy-Item -LiteralPath $modJsonPath -Destination (Join-Path $stagingData 'mod.json')
        $pdb = Join-Path (Split-Path -Parent $dllPath) "$modName.pdb"
        if (Test-Path -LiteralPath $pdb -PathType Leaf) { Copy-Item -LiteralPath $pdb -Destination $stagingData }
        # Mirror Directory.Build.targets: icons/bundles in Mods, GLBs in Mods/<Mod>.
        foreach ($assetRoot in @((Join-Path $modDir.FullName 'src'), (Join-Path $modDir.FullName 'assets'))) {
            if (-not (Test-Path -LiteralPath $assetRoot -PathType Container)) { continue }
            $isAssets = [IO.Path]::GetFileName($assetRoot) -eq 'assets'
            foreach ($asset in @(Get-ChildItem -LiteralPath $assetRoot -File | Where-Object {
                $_.Extension -eq '.png' -or ($isAssets -and $_.Extension -in @('.bundle', '.glb'))
            })) {
                $destination = $stagingMods
                if ($asset.Extension -eq '.glb') {
                    $destination = Join-Path $stagingMods $modName
                    $null = New-Item -ItemType Directory -Path $destination -Force
                }
                $target = Join-Path $destination $asset.Name
                if (Test-Path -LiteralPath $target) { throw "Duplicate package asset destination: $target" }
                Copy-Item -LiteralPath $asset.FullName -Destination $target
            }
        }
        foreach ($name in @('README.md', 'CHANGELOG.md')) {
            $document = Join-Path $docsPath $name
            if (Test-Path -LiteralPath $document -PathType Leaf) { Copy-Item -LiteralPath $document -Destination $staging.Path }
        }
        $null = Write-S1ReleaseManifest -StagingRoot $staging.Path -Mod $modName -Source $source
        # Catch edits during build/copy before creating or replacing the output archive.
        Assert-S1ReleaseSourceIdentity (Get-S1ReleaseSourceIdentity $workspaceRoot) $source
        [IO.Compression.ZipFile]::CreateFromDirectory($staging.Path, $pendingZip, [IO.Compression.CompressionLevel]::Optimal, $false)
        $null = Assert-S1ReleaseArchive $pendingZip $modName $metadata.version $source
        [IO.File]::Move($pendingZip, $zipPath, $true)
        Write-Host "Created $zipPath (manifest and payload hashes verified)." -ForegroundColor Green
    } finally {
        if (Test-Path -LiteralPath $pendingZip) { Remove-Item -LiteralPath $pendingZip -Force }
        Remove-S1ReleaseStagingDirectory $staging
    }
}
