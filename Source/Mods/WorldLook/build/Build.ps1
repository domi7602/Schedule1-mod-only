#requires -Version 7.0
[CmdletBinding()]
param(
    [string]$GamePath,
    [string]$UnityPath,
    [string]$DotNetPath,
    [ValidateSet('Release', 'Debug')]
    [string]$Configuration = 'Release',
    [switch]$CheckOnly,
    [switch]$ShadersOnly
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$projectRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
$unitySource = Join-Path $projectRoot 'unity'
$projectFile = Join-Path $projectRoot 'src/WorldLook.csproj'
$expectedUnity = '2022.3.62f3'
$runLogs = $null
. (Join-Path $PSScriptRoot 'Verify-ShaderBundle.ps1') -ProjectRoot $projectRoot -DefinitionsOnly

function Resolve-Executable {
    param([string]$Name)
    if (Test-Path -LiteralPath $Name -PathType Leaf) { return (Get-Item -LiteralPath $Name).FullName }
    $command = Get-Command -Name $Name -CommandType Application -ErrorAction SilentlyContinue | Select-Object -First 1
    if ($null -eq $command) { throw ('Executable not found: ' + $Name) }
    return $command.Source
}

function Find-UnityExecutable {
    param([string]$ExplicitPath)
    if ([string]::IsNullOrWhiteSpace($ExplicitPath)) { $ExplicitPath = $env:UNITY_EDITOR_PATH }
    if (-not [string]::IsNullOrWhiteSpace($ExplicitPath)) {
        return Resolve-Executable ([Environment]::ExpandEnvironmentVariables($ExplicitPath))
    }
    foreach ($directory in @($env:ProgramFiles, ${env:ProgramFiles(x86)})) {
        if ([string]::IsNullOrWhiteSpace($directory)) { continue }
        foreach ($relative in @(('Unity ' + $expectedUnity + '/Editor/Unity.exe'), ('Unity/Hub/Editor/' + $expectedUnity + '/Editor/Unity.exe'))) {
            $candidate = Join-Path $directory $relative
            if (Test-Path -LiteralPath $candidate -PathType Leaf) { return $candidate }
        }
    }
    throw ('Unity ' + $expectedUnity + ' not found. Supply -UnityPath. No editor is installed automatically.')
}

function Find-GameDirectory {
    param([string]$ExplicitPath)
    if ([string]::IsNullOrWhiteSpace($ExplicitPath)) { $ExplicitPath = $env:SCHEDULE1_PATH }
    # Compatibility with the original standalone script; canonical variable wins.
    if ([string]::IsNullOrWhiteSpace($ExplicitPath)) { $ExplicitPath = $env:SCHEDULE_I_GAME_DIR }
    if ([string]::IsNullOrWhiteSpace($ExplicitPath)) { $ExplicitPath = 'C:/Program Files (x86)/Steam/steamapps/common/Schedule I' }
    $path = [IO.Path]::GetFullPath([Environment]::ExpandEnvironmentVariables($ExplicitPath))
    Require-File (Join-Path $path 'MelonLoader/Il2CppAssemblies/Assembly-CSharp.dll')
    return $path
}

function Assert-GameClosed {
    if (@(Get-Process -Name 'Schedule I' -ErrorAction SilentlyContinue).Count -ne 0) {
        throw 'Close Schedule I before building WorldLook.'
    }
}

function Invoke-CheckedProcess {
    param([string]$Executable, [string[]]$Arguments, [string]$WorkingDirectory, [string]$LogPath)
    $start = [Diagnostics.ProcessStartInfo]::new()
    $start.FileName = $Executable
    foreach ($argument in $Arguments) { $start.ArgumentList.Add($argument) }
    $start.WorkingDirectory = $WorkingDirectory
    $start.UseShellExecute = $false
    $start.CreateNoWindow = $true
    $start.RedirectStandardOutput = $true
    $start.RedirectStandardError = $true
    $process = [Diagnostics.Process]::new()
    $process.StartInfo = $start
    try {
        if (-not $process.Start()) { throw ('Could not start ' + $Executable) }
        $stdoutTask = $process.StandardOutput.ReadToEndAsync()
        $stderrTask = $process.StandardError.ReadToEndAsync()
        $process.WaitForExit()
        $stdout = $stdoutTask.GetAwaiter().GetResult()
        $stderr = $stderrTask.GetAwaiter().GetResult()
        [IO.File]::WriteAllText($LogPath, ($stdout + [Environment]::NewLine + $stderr))
        if (-not [string]::IsNullOrWhiteSpace($stdout)) { Write-Host ($stdout.TrimEnd()) }
        if (-not [string]::IsNullOrWhiteSpace($stderr)) { Write-Host ($stderr.TrimEnd()) }
        if ($process.ExitCode -ne 0) { throw ('Process failed (exit ' + $process.ExitCode + '): ' + $Executable + '. Log: ' + $LogPath) }
        return $stdout
    }
    finally { $process.Dispose() }
}

function Get-ProductionInputHashes {
    $paths = @(Get-ShaderInputHashes $projectRoot | ForEach-Object { $_.path })
    foreach ($file in Get-ChildItem -LiteralPath (Join-Path $projectRoot 'src') -Recurse -File) {
        $relative = [IO.Path]::GetRelativePath($projectRoot, $file.FullName).Replace('\', '/')
        if ($file.Extension -in @('.cs', '.csproj') -and $relative -notmatch '(^|/)(bin|obj)/') { $paths += $relative }
    }
    foreach ($path in ($paths | Sort-Object -Unique)) {
        [pscustomobject]@{ path = $path; sha256 = Get-Sha256Hex (Join-Path $projectRoot $path) }
    }
}

try {
    if (-not $IsWindows) { throw 'This build targets Windows IL2CPP / D3D11.' }
    Require-File $projectFile
    $inputs = @(Get-ShaderInputHashes $projectRoot)
    $projectVersion = Get-Content -LiteralPath (Join-Path $unitySource 'ProjectSettings/ProjectVersion.txt') -Raw
    if ($projectVersion -notmatch ('(?m)^m_EditorVersion:\s*' + [regex]::Escape($expectedUnity) + '\s*$')) {
        throw ('The shader project must use Unity ' + $expectedUnity)
    }
    $packages = Get-Content -LiteralPath (Join-Path $unitySource 'Packages/manifest.json') -Raw | ConvertFrom-Json
    if ($packages.dependencies.'com.unity.render-pipelines.universal' -cne '14.0.11' -or
        $packages.dependencies.'com.unity.render-pipelines.core' -cne '14.0.11') {
        throw 'Preserve the validated URP/Core manifest pins (14.0.11; editor resolves 14.0.12).'
    }
    $legacyEditors = @(Get-ChildItem -LiteralPath (Join-Path $unitySource 'Assets/Editor') -Filter '*.cs' -File |
        Where-Object { $_.Name -cne 'BuildWorldLook.cs' })
    if ($legacyEditors.Count -gt 0) { throw 'Retire the legacy editor scripts outside active Unity Assets before building.' }
    $resolvedUnity = Find-UnityExecutable $UnityPath
    $stamp = [DateTime]::UtcNow.ToString('yyyyMMdd-HHmmss-fff') + '-' + [Guid]::NewGuid().ToString('N').Substring(0, 8)
    $runLogs = Join-Path $PSScriptRoot ('logs/' + $stamp)
    [void](New-Item -ItemType Directory -Path $runLogs)
    Write-Host ('Unity: ' + $resolvedUnity)
    Write-Host ('Logs:  ' + $runLogs)

    if (-not $ShadersOnly) {
        $resolvedGame = Find-GameDirectory $GamePath
        $interop = Join-Path $resolvedGame 'MelonLoader/Il2CppAssemblies'
        foreach ($name in @('Assembly-CSharp.dll', 'Il2CppScheduleOne.Core.dll', 'Il2Cppmscorlib.dll', 'UnityEngine.CoreModule.dll',
            'UnityEngine.AssetBundleModule.dll', 'UnityEngine.InputLegacyModule.dll', 'UnityEngine.UI.dll', 'Unity.TextMeshPro.dll',
            'Unity.RenderPipelines.Core.Runtime.dll', 'Unity.RenderPipelines.Universal.Runtime.dll')) {
            Require-File (Join-Path $interop $name)
        }
        foreach ($name in @('MelonLoader.dll', '0Harmony.dll', 'Il2CppInterop.Runtime.dll', 'Il2CppInterop.Common.dll')) {
            Require-File (Join-Path $resolvedGame ('MelonLoader/net6/' + $name))
        }
        $s1api = Join-Path $resolvedGame 'Mods/S1API.Il2Cpp.MelonLoader.dll'
        Require-File $s1api
        $s1apiIdentity = [Reflection.AssemblyName]::GetAssemblyName($s1api)
        $s1apiFileVersion = [Diagnostics.FileVersionInfo]::GetVersionInfo($s1api).FileVersion
        if ($s1apiIdentity.Name -notin @('S1API', 'S1API.Il2Cpp.MelonLoader') -or $s1apiFileVersion -ne '3.2.1.0') {
            throw 'The installed S1API 3.2.1 stable IL2CPP assembly is required.'
        }
        if ([string]::IsNullOrWhiteSpace($DotNetPath)) {
            $localDotNet = Join-Path $HOME '.dotnet/dotnet.exe'
            $DotNetPath = if (Test-Path -LiteralPath $localDotNet -PathType Leaf) { $localDotNet } else { 'dotnet' }
        }
        $resolvedDotNet = Resolve-Executable $DotNetPath
        $sdkVersion = (Invoke-CheckedProcess $resolvedDotNet @('--version') $PSScriptRoot (Join-Path $runLogs 'dotnet-version.log')).Trim()
        if ($sdkVersion -notmatch '^(?<major>\d+)\.\d+\.\d+' -or [int]$Matches['major'] -lt 8) {
            throw ('C# 12 requires .NET SDK 8 or newer; found ' + $sdkVersion + '. No SDK is installed automatically.')
        }
        Write-Host ('Game:  ' + $resolvedGame + '; SDK ' + $sdkVersion + '; S1API ' + $s1apiFileVersion)
    }
    if ($CheckOnly) {
        Write-Host 'Preflight passed. Unity build, package restore and C# compile were not run.'
        exit 0
    }
    Assert-GameClosed

    # Each run owns a fresh project/output; old bundle/cache success cannot mask
    # a failed new shader build. Only the water shader and current editor enter it.
    $unityProject = Join-Path $runLogs 'unity'
    foreach ($inputFile in $inputs | Where-Object { $_.path.StartsWith('unity/', [StringComparison]::Ordinal) }) {
        $destination = Join-Path $unityProject $inputFile.path.Substring('unity/'.Length)
        [void](New-Item -ItemType Directory -Path (Split-Path $destination -Parent) -Force)
        Copy-Item -LiteralPath (Join-Path $projectRoot $inputFile.path) -Destination $destination
        if ((Get-Sha256Hex $destination) -cne $inputFile.sha256) { throw ('Source changed while copying: ' + $inputFile.path) }
    }
    $inputManifest = Join-Path $runLogs 'shader-inputs.json'
    [IO.File]::WriteAllText($inputManifest, (@{ inputs = $inputs } | ConvertTo-Json -Depth 6))
    $shaderOutput = Join-Path $runLogs 'shader-output'
    Write-Host '1/3: Rebuilding and loading the water-only shader bundle in Unity.'
    [void](Invoke-CheckedProcess -Executable $resolvedUnity -Arguments @(
        '-batchmode', '-quit', '-projectPath', $unityProject, '-executeMethod', 'BuildWorldLook.Build',
        '-buildTarget', 'Win64', '-force-d3d11', '-logFile', (Join-Path $runLogs 'unity-build.log'),
        '-worldLookRoot', $projectRoot, '-worldLookOutput', $shaderOutput, '-worldLookInputs', $inputManifest
    ) -WorkingDirectory $projectRoot -LogPath (Join-Path $runLogs 'unity-process.log'))
    $verified = Assert-ShaderBundle -Root $projectRoot -Assets $shaderOutput

    # This nested location is deliberately outside the deploy helper's top-level
    # assets/*.bundle scan. The DLL is the only distribution of these shaders.
    $assets = Join-Path $projectRoot 'assets/embedded'
    [void](New-Item -ItemType Directory -Path $assets -Force)
    $previous = Join-Path $runLogs 'previous-embedded'
    [void](New-Item -ItemType Directory -Path $previous)
    $published = @()
    try {
        foreach ($name in @('worldlook-shaders.bundle', 'shader-build.json')) {
            $target = Join-Path $assets $name
            if (Test-Path -LiteralPath $target -PathType Leaf) { Copy-Item -LiteralPath $target -Destination (Join-Path $previous $name) }
            $published += $name
            Copy-Item -LiteralPath (Join-Path $shaderOutput $name) -Destination $target -Force
        }
        $verified = Assert-ShaderBundle -Root $projectRoot -Assets $assets
    }
    catch {
        foreach ($name in $published) {
            $backup = Join-Path $previous $name
            $target = Join-Path $assets $name
            if (Test-Path -LiteralPath $backup -PathType Leaf) { Copy-Item -LiteralPath $backup -Destination $target -Force }
            elseif (Test-Path -LiteralPath $target -PathType Leaf) { Remove-Item -LiteralPath $target }
        }
        throw
    }
    [IO.File]::WriteAllText((Join-Path $runLogs 'shader-check.json'), ([ordered]@{
        bundleSha256 = $verified.bundleSha256; receiptSha256 = $verified.receiptSha256;
        checkedAtUtc = [DateTime]::UtcNow.ToString('o'); inGameVerified = $false
    } | ConvertTo-Json))
    if ($ShadersOnly) {
        Write-Host ('SHADER BUILD PASSED: ' + $verified.bundleSha256)
        Write-Host ('Receipt SHA-256: ' + $verified.receiptSha256)
        Write-Host 'No C# restore/build or deployment performed. Editor contract checks do not prove native GPU/game behavior.'
        exit 0
    }

    Assert-GameClosed
    $productionInputs = @(Get-ProductionInputHashes)
    # Global properties propagate to Shared and all inherited repository targets.
    # BOTH restore and build explicitly disable auto-deploy; no shielding props.
    $buildProperties = @(('-p:GameDir=' + $resolvedGame), '-p:S1NoDeploy=true')
    Write-Host '2/3: Restoring and compiling the canonical repo project (S1NoDeploy=true).'
    [void](Invoke-CheckedProcess -Executable $resolvedDotNet -Arguments (@(
        'restore', $projectFile, '--verbosity', 'minimal'
    ) + $buildProperties) -WorkingDirectory $PSScriptRoot -LogPath (Join-Path $runLogs 'dotnet-restore.log'))
    [void](Invoke-CheckedProcess -Executable $resolvedDotNet -Arguments (@(
        'build', $projectFile, '--no-restore', '--configuration', $Configuration, '--verbosity', 'minimal'
    ) + $buildProperties) -WorkingDirectory $PSScriptRoot -LogPath (Join-Path $runLogs 'dotnet-build.log'))
    $afterInputs = @(Get-ProductionInputHashes)
    if (($productionInputs | ConvertTo-Json -Compress) -cne ($afterInputs | ConvertTo-Json -Compress)) {
        throw 'Production inputs changed during compile; the result has no verified build receipt. Repeat the build.'
    }
    $verified = Assert-ShaderBundle -Root $projectRoot -Assets $assets
    $binaryDirectory = Join-Path $projectRoot ('src/bin/' + $Configuration + '/net6.0')
    $dll = Join-Path $binaryDirectory 'WorldLook.dll'
    Require-File $dll
    $identity = [Reflection.AssemblyName]::GetAssemblyName($dll)
    if ($identity.Name -cne 'WorldLook') { throw 'Unexpected output assembly identity.' }
    $assembly = [Reflection.Assembly]::Load([IO.File]::ReadAllBytes($dll))
    $stream = $assembly.GetManifestResourceStream('WorldLook.Shaders.bundle')
    if ($null -eq $stream) { throw 'WorldLook.Shaders.bundle is missing from the compiled DLL.' }
    try {
        $sha = [Security.Cryptography.SHA256]::Create()
        try { $embeddedHash = ([BitConverter]::ToString($sha.ComputeHash($stream)) -replace '-', '').ToLowerInvariant() }
        finally { $sha.Dispose() }
        if ($embeddedHash -cne $verified.bundleSha256) { throw 'The compiled embedded resource differs from the verified bundle.' }
    }
    finally { $stream.Dispose() }

    Write-Host '3/3: Recording this build; game installation remains untouched.'
    $inherited = @()
    foreach ($name in @('Directory.Build.props', 'Directory.Build.targets')) {
        $path = [IO.Path]::GetFullPath((Join-Path $projectRoot ('../' + $name)))
        $inherited += [ordered]@{ path = $path; sha256 = Get-Sha256Hex $path }
    }
    $report = [ordered]@{
        artifact = 'WorldLook'; builtAtUtc = [DateTime]::UtcNow.ToString('o'); configuration = $Configuration;
        targetFramework = 'net6.0'; dotnetSdk = $sdkVersion; unityVersion = $expectedUnity;
        s1ApiFileVersion = $s1apiFileVersion; s1ApiSha256 = Get-Sha256Hex $s1api;
        assemblyVersion = $identity.Version.ToString(); dllPath = $dll; dllSha256 = Get-Sha256Hex $dll;
        embeddedResourceName = 'WorldLook.Shaders.bundle'; embeddedBundleSha256 = $embeddedHash;
        shaderReceiptSha256 = $verified.receiptSha256; sourceFiles = $productionInputs; inheritedBuildFiles = $inherited;
        deployed = $false; inGameVerified = $false
    }
    [IO.File]::WriteAllText((Join-Path $runLogs 'build-info.json'), ($report | ConvertTo-Json -Depth 8))
    Write-Host ('BUILD PASSED: ' + $dll)
    Write-Host ('Receipt: ' + (Join-Path $runLogs 'build-info.json'))
    exit 0
}
catch {
    Write-Host ('BUILD FAILED: ' + $_.Exception.Message) -ForegroundColor Red
    if ($null -ne $runLogs) { Write-Host ('Logs: ' + $runLogs) }
    exit 1
}
