#requires -Version 7.0
[CmdletBinding()]
param(
    [string]$ProjectRoot = (Join-Path $PSScriptRoot '..'),
    [string]$AssetDirectory,
    [switch]$DefinitionsOnly
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Require-File {
    param([string]$Path)
    if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) { throw ('Required file is missing: ' + $Path) }
}

function Get-Sha256Hex {
    param([string]$Path)
    return (Get-FileHash -LiteralPath $Path -Algorithm SHA256).Hash.ToLowerInvariant()
}

function Get-ShaderInputHashes {
    param([string]$Root)
    $rootPath = [IO.Path]::GetFullPath($Root).TrimEnd([IO.Path]::DirectorySeparatorChar)
    $paths = @(
        'unity/Assets/S1Mods/ReferenceWater.shader',
        'unity/Assets/S1Mods/ReferenceWater.shader.meta',
        'unity/Assets/Editor/BuildWorldLook.cs',
        'unity/Assets/Editor/BuildWorldLook.cs.meta',
        'unity/Packages/manifest.json',
        'unity/Packages/packages-lock.json',
        'unity/ProjectSettings/ProjectVersion.txt'
    )
    foreach ($folder in @('build', 'unity/ProjectSettings')) {
        $filter = if ($folder -eq 'build') { '*.ps1' } else { '*.asset' }
        foreach ($file in Get-ChildItem -LiteralPath (Join-Path $rootPath $folder) -Filter $filter -File) {
            $paths += [IO.Path]::GetRelativePath($rootPath, $file.FullName).Replace('\', '/')
        }
    }
    foreach ($path in ($paths | Sort-Object -Unique)) {
        $fullPath = Join-Path $rootPath $path
        Require-File $fullPath
        [pscustomobject]@{ path = $path; sha256 = Get-Sha256Hex $fullPath }
    }
}

function Assert-ShaderBundle {
    param([string]$Root, [string]$Assets)
    $bundlePath = Join-Path $Assets 'worldlook-shaders.bundle'
    $receiptPath = Join-Path $Assets 'shader-build.json'
    Require-File $bundlePath
    Require-File $receiptPath
    $receipt = Get-Content -LiteralPath $receiptPath -Raw | ConvertFrom-Json
    if ($receipt.schemaVersion -ne 1 -or $receipt.unityVersion -cne '2022.3.62f3' -or
        $receipt.platform -cne 'StandaloneWindows64' -or $receipt.graphicsApi -cne 'Direct3D11' -or
        $receipt.colorSpace -cne 'Linear' -or $receipt.requestedWaterVariants -ne 8 -or
        $receipt.loadedWaterVariants -ne 8 -or $receipt.urpPackageVersion -cne '14.0.12' -or
        $receipt.corePackageVersion -cne '14.0.12') {
        throw 'The shader receipt has an unsupported editor, pipeline, platform or variant contract. Rebuild with build/Build.ps1 -ShadersOnly.'
    }
    $bundleHash = Get-Sha256Hex $bundlePath
    if ($receipt.bundleSha256 -cne $bundleHash -or $receipt.bundleBytes -ne (Get-Item -LiteralPath $bundlePath).Length -or
        $receipt.bundleBytes -lt 32) {
        throw 'The shader bundle does not match its receipt (SHA-256/size). Rebuild with build/Build.ps1 -ShadersOnly.'
    }

    $expectedAssets = @('assets/s1mods/referencewater.shader', 'assets/buildsettings/worldlookvariants.shadervariants') | Sort-Object
    $actualAssets = @($receipt.bundleAssets | Sort-Object)
    if (($actualAssets -join '|') -cne ($expectedAssets -join '|')) { throw 'The bundle receipt must contain only ReferenceWater and its variant collection.' }
    $shaders = @($receipt.shaders)
    if ($shaders.Count -ne 1 -or $shaders[0].assetPath -cne 'Assets/S1Mods/ReferenceWater.shader' -or
        $shaders[0].shaderName -cne 'S1Mods/ReferenceWater' -or $shaders[0].passCount -lt 1 -or
        $shaders[0].editorSupported -ne $true) { throw 'The bundle receipt has an invalid loaded-water shader contract.' }
    $properties = @(
        '_ShallowTint', '_DeepTint', '_Absorption', '_SurfaceOpacity', '_ReflectionStrength',
        '_WaveAmp', '_WaveScale', '_WaveSpeed', '_RippleStrength', '_WindActivity',
        '_SunStrength', '_SunSharpness', '_FoamColor', '_FoamDepth', '_FoamStrength', '_SkyColor', '_AmbientColor'
    ) | Sort-Object
    if ((@($shaders[0].properties | Sort-Object) -join '|') -cne ($properties -join '|')) {
        throw 'The loaded-water shader must expose all 17 required properties.'
    }

    $expected = @(Get-ShaderInputHashes $Root)
    $recorded = @($receipt.inputs)
    if ($recorded.Count -ne $expected.Count) { throw 'Shader source inventory changed. Rebuild with build/Build.ps1 -ShadersOnly.' }
    $seen = [Collections.Generic.HashSet[string]]::new([StringComparer]::Ordinal)
    foreach ($inputFile in $recorded) {
        if (-not $seen.Add([string]$inputFile.path)) { throw ('Duplicate shader provenance entry: ' + $inputFile.path) }
    }
    foreach ($inputFile in $expected) {
        $match = @($recorded | Where-Object { $_.path -ceq $inputFile.path })
        if ($match.Count -ne 1 -or $match[0].sha256 -cne $inputFile.sha256) {
            throw ('Shader input is missing or stale: ' + $inputFile.path + '. Rebuild with build/Build.ps1 -ShadersOnly.')
        }
    }
    return [pscustomobject]@{
        bundleSha256 = $bundleHash
        receiptSha256 = Get-Sha256Hex $receiptPath
        receipt = $receipt
    }
}

if (-not $DefinitionsOnly) {
    try {
        $ProjectRoot = [IO.Path]::GetFullPath($ProjectRoot)
        if ([string]::IsNullOrWhiteSpace($AssetDirectory)) { $AssetDirectory = Join-Path $ProjectRoot 'assets/embedded' }
        $verified = Assert-ShaderBundle -Root $ProjectRoot -Assets $AssetDirectory
        Write-Host ('WorldLook shader bundle verified: ' + $verified.bundleSha256 + ' (8 requested/loaded variants, 17 properties).')
    }
    catch {
        Write-Error -Message $_.Exception.Message -ErrorAction Continue
        exit 1
    }
}
