#requires -Version 7.0
[CmdletBinding()]
param(
    [Parameter(Mandatory)][string]$Bundle,
    [Parameter(Mandatory)][ValidatePattern('^[a-zA-Z0-9_-]+$')][string]$Label,
    [string]$OutputRoot,
    [string]$UnityPath = 'C:/Program Files/Unity 2022.3.62f3/Editor/Unity.exe'
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$mod = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '../..'))
$repo = [IO.Path]::GetFullPath((Join-Path $mod '../../..'))
$allowed = Join-Path $repo '.backups/WorldLook/water-detail-20261023-035705/preview'
if (-not $OutputRoot) { $OutputRoot = $allowed }
$OutputRoot = [IO.Path]::GetFullPath($OutputRoot)
$logs = Join-Path $mod 'build/logs'
if (-not ($OutputRoot.Equals($allowed, [StringComparison]::OrdinalIgnoreCase) -or
    $OutputRoot.StartsWith($allowed + [IO.Path]::DirectorySeparatorChar, [StringComparison]::OrdinalIgnoreCase) -or
    $OutputRoot.StartsWith($logs + [IO.Path]::DirectorySeparatorChar, [StringComparison]::OrdinalIgnoreCase))) {
    throw 'Use the approved preview backup directory or a subdirectory of build/logs.'
}
$Bundle = (Get-Item -LiteralPath $Bundle -ErrorAction Stop).FullName
if (-not (Test-Path -LiteralPath $UnityPath -PathType Leaf)) { throw 'The specified Unity editor is missing.' }
if (@(Get-Process -Name 'Schedule I' -ErrorAction SilentlyContinue).Count) { throw 'Close Schedule I before the preview.' }
$run = Join-Path $OutputRoot ($Label + '-' + [DateTime]::UtcNow.ToString('yyyyMMdd-HHmmss-fff'))
# Keep the PackageCache path short enough for Unity's Windows asset importers.
$project = Join-Path $logs ('p-' + [Guid]::NewGuid().ToString('N').Substring(0, 8))
$images = Join-Path $run 'images'
foreach ($directory in @((Join-Path $project 'Assets/Editor'), (Join-Path $project 'Packages'),
    (Join-Path $project 'ProjectSettings'), $images, (Join-Path $run 'input'))) {
    [void](New-Item -ItemType Directory -Path $directory -Force)
}
Copy-Item -LiteralPath (Join-Path $PSScriptRoot 'WorldLookWaterPreview.cs') -Destination (Join-Path $project 'Assets/Editor')
Copy-Item -LiteralPath $PSCommandPath -Destination (Join-Path $run 'input/Run-Preview.ps1')
Copy-Item -LiteralPath (Join-Path $mod 'unity/Packages/manifest.json'), (Join-Path $mod 'unity/Packages/packages-lock.json') -Destination (Join-Path $project 'Packages')
Copy-Item -LiteralPath (Join-Path $mod 'unity/ProjectSettings/ProjectVersion.txt'), (Join-Path $mod 'unity/ProjectSettings/ProjectSettings.asset') -Destination (Join-Path $project 'ProjectSettings')
# Only the scratch settings are changed. Starting in Linear avoids a color-space transition during capture.
$settings = Join-Path $project 'ProjectSettings/ProjectSettings.asset'
$text = [IO.File]::ReadAllText($settings).Replace('m_ActiveColorSpace: 0', 'm_ActiveColorSpace: 1')
[IO.File]::WriteAllText($settings, $text)
$bundleCopy = Join-Path $run 'input/worldlook-shaders.bundle'
$bundleHash = (Get-FileHash -LiteralPath $Bundle -Algorithm SHA256).Hash.ToLowerInvariant()
Copy-Item -LiteralPath $Bundle -Destination $bundleCopy
if ((Get-FileHash -LiteralPath $bundleCopy -Algorithm SHA256).Hash.ToLowerInvariant() -cne $bundleHash) { throw 'Bundle changed during copy.' }
$buildReceipt = Join-Path (Split-Path $Bundle -Parent) 'shader-build.json'
$shaderSourceHash = $null
if (Test-Path -LiteralPath $buildReceipt) {
    $metadata = Get-Content -LiteralPath $buildReceipt -Raw | ConvertFrom-Json
    if ($metadata.bundleSha256 -cne $bundleHash) { throw 'Sibling build receipt does not identify this bundle.' }
    $shaderSourceHash = ($metadata.inputs | Where-Object path -CEQ 'unity/Assets/S1Mods/ReferenceWater.shader').sha256
    Copy-Item -LiteralPath $buildReceipt -Destination (Join-Path $run 'input/shader-build.json')
}
[ordered]@{
    mode = 'actual asset-bundle GPU render'; originalBundle = $Bundle; bundleSha256 = $bundleHash
    scratchUnityProject = $project
    shaderSourceSha256FromMatchingBuildReceipt = $shaderSourceHash
    shaderSourceNote = 'Source hash is build provenance only; the loaded binary bundle is rendered, not source reimported.'
    fixtureSha256 = (Get-FileHash -LiteralPath (Join-Path $project 'Assets/Editor/WorldLookWaterPreview.cs')).Hash.ToLowerInvariant()
    runnerSha256 = (Get-FileHash -LiteralPath $PSCommandPath).Hash.ToLowerInvariant()
} | ConvertTo-Json | Set-Content -LiteralPath (Join-Path $run 'inputs.json') -Encoding utf8NoBOM
$start = [Diagnostics.ProcessStartInfo]::new($UnityPath)
$start.UseShellExecute = $false
foreach ($argument in @('-batchmode', '-projectPath', $project, '-executeMethod', 'WorldLookWaterPreview.Run',
    '-buildTarget', 'Win64', '-force-d3d11', '-logFile', (Join-Path $run 'unity-preview.log'),
    '-previewBundle', $bundleCopy, '-previewOutput', $images, '-previewLabel', $Label)) { $start.ArgumentList.Add($argument) }
Write-Host ('Preview: ' + $run)
$process = [Diagnostics.Process]::Start($start)
try {
    if (-not $process.WaitForExit(600000)) { $process.Kill($true); throw 'Preview exceeded ten minutes; owned Unity process stopped. Inspect its log.' }
    if ($process.ExitCode -ne 0) { throw ('Unity preview failed: exit ' + $process.ExitCode + '; ' + (Join-Path $run 'unity-preview.log')) }
} finally { $process.Dispose() }
$importErrors = Select-String -LiteralPath (Join-Path $run 'unity-preview.log') -Pattern 'error CS\d+|^Shader error in |Exception:|^Asset import failed,|Aborting batchmode'
if ($importErrors) { throw ('Unity compiler/import/render errors in log: ' + ($importErrors.Line -join [Environment]::NewLine)) }
$result = Get-Content -LiteralPath (Join-Path $images 'preview.json') -Raw | ConvertFrom-Json
if ($result.bundleSha256 -cne $bundleHash -or $result.errors.Count -ne 0 -or -not $result.repeatedTimeZeroPngIdentical) {
    throw 'Preview receipt validation failed.'
}
Write-Host ('GPU PREVIEW PASSED: ' + $images)
Write-Host ('Bundle SHA-256: ' + $bundleHash)
