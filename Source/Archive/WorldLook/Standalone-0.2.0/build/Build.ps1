#requires -Version 5.1
[CmdletBinding()]
param(
    [string]$GamePath,
    [string]$UnityPath,
    [string]$DotNetPath = 'dotnet',
    [ValidateSet('Release', 'Debug')]
    [string]$Configuration = 'Release',
    [switch]$CheckOnly
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'
$projectRoot = [System.IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
$unityProject = Join-Path $projectRoot 'unity'
$projectFile = Join-Path $projectRoot 'WorldLook.csproj'
$expectedUnity = '2022.3.62f3'
$runLogs = $null

function Require-File {
    param([string]$Path, [string]$Description)
    if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) {
        throw ($Description + ' fehlt: ' + $Path)
    }
}

function Get-Sha256Hex {
    # This Windows PowerShell installation does not provide Get-FileHash
    # (Microsoft.PowerShell.Utility 3.1.0.0 exports no such command), so the
    # hash is computed directly with the .NET cryptography API.
    param([string]$Path)
    $sha = [System.Security.Cryptography.SHA256]::Create()
    try {
        $stream = [System.IO.File]::OpenRead($Path)
        try {
            return ([System.BitConverter]::ToString($sha.ComputeHash($stream)) -replace '-', '').ToLowerInvariant()
        }
        finally { $stream.Dispose() }
    }
    finally { $sha.Dispose() }
}

function Resolve-FullDirectory {
    param([string]$Path)
    $expanded = [Environment]::ExpandEnvironmentVariables($Path)
    if (-not (Test-Path -LiteralPath $expanded -PathType Container)) {
        throw ('Ordner nicht gefunden: ' + $expanded)
    }
    return (Get-Item -LiteralPath $expanded).FullName
}

function Test-GameDirectory {
    param([string]$Path)
    if ([string]::IsNullOrWhiteSpace($Path)) { return $false }
    return (Test-Path -LiteralPath (Join-Path $Path 'MelonLoader\Il2CppAssemblies\Assembly-CSharp.dll') -PathType Leaf)
}

function Find-GameDirectory {
    param([string]$ExplicitPath)
    if (-not [string]::IsNullOrWhiteSpace($ExplicitPath)) {
        return Resolve-FullDirectory $ExplicitPath
    }
    if (-not [string]::IsNullOrWhiteSpace($env:SCHEDULE_I_GAME_DIR)) {
        return Resolve-FullDirectory $env:SCHEDULE_I_GAME_DIR
    }

    $steamRoots = New-Object 'System.Collections.Generic.List[string]'
    foreach ($location in @(
        @{ Key = 'HKCU:\Software\Valve\Steam'; Name = 'SteamPath' },
        @{ Key = 'HKLM:\SOFTWARE\WOW6432Node\Valve\Steam'; Name = 'InstallPath' },
        @{ Key = 'HKLM:\SOFTWARE\Valve\Steam'; Name = 'InstallPath' }
    )) {
        try {
            $property = Get-ItemProperty -LiteralPath $location.Key -Name $location.Name -ErrorAction Stop
            $value = [string]$property.($location.Name)
            if (-not [string]::IsNullOrWhiteSpace($value)) { $steamRoots.Add($value) }
        }
        catch { }
    }
    foreach ($programFiles in @(${env:ProgramFiles(x86)}, $env:ProgramFiles)) {
        if (-not [string]::IsNullOrWhiteSpace($programFiles)) {
            $steamRoots.Add((Join-Path $programFiles 'Steam'))
        }
    }

    $libraryRoots = New-Object 'System.Collections.Generic.List[string]'
    foreach ($steamRoot in $steamRoots) {
        $libraryRoots.Add($steamRoot)
        $libraryFile = Join-Path $steamRoot 'steamapps\libraryfolders.vdf'
        if (-not (Test-Path -LiteralPath $libraryFile -PathType Leaf)) { continue }
        $content = [IO.File]::ReadAllText($libraryFile)
        foreach ($match in [regex]::Matches($content, '"path"\s+"(?<path>(?:\\.|[^"\\])*)"')) {
            $libraryRoots.Add($match.Groups['path'].Value.Replace('\\', '\'))
        }
        # Legacy Steam library files used numbered path entries directly.
        foreach ($match in [regex]::Matches($content, '"\d+"\s+"(?<path>[A-Za-z]:[^"]+)"')) {
            $libraryRoots.Add($match.Groups['path'].Value.Replace('\\', '\'))
        }
    }

    $found = New-Object 'System.Collections.Generic.List[string]'
    $seen = New-Object 'System.Collections.Generic.HashSet[string]' ([StringComparer]::OrdinalIgnoreCase)
    foreach ($libraryRoot in $libraryRoots) {
        $candidate = Join-Path $libraryRoot 'steamapps\common\Schedule I'
        if ((Test-GameDirectory $candidate) -and $seen.Add([IO.Path]::GetFullPath($candidate))) {
            $found.Add((Resolve-FullDirectory $candidate))
        }
    }
    if ($found.Count -eq 1) { return $found[0] }
    if ($found.Count -gt 1) {
        throw ('Mehrere passende Spielinstallationen gefunden. Bitte -GamePath angeben: ' + ($found -join '; '))
    }
    throw 'Schedule I mit erzeugten IL2CPP-Assemblies wurde nicht gefunden. Das Spiel einmal mit MelonLoader starten, dann Build.ps1 -GamePath "D:\SteamLibrary\steamapps\common\Schedule I" verwenden.'
}

function Find-UnityExecutable {
    param([string]$ExplicitPath)
    if ([string]::IsNullOrWhiteSpace($ExplicitPath)) { $ExplicitPath = $env:UNITY_EDITOR_PATH }
    if (-not [string]::IsNullOrWhiteSpace($ExplicitPath)) {
        $expanded = [Environment]::ExpandEnvironmentVariables($ExplicitPath)
        Require-File $expanded 'Unity-Editor'
        return (Get-Item -LiteralPath $expanded).FullName
    }
    foreach ($programFiles in @($env:ProgramFiles, ${env:ProgramFiles(x86)})) {
        if ([string]::IsNullOrWhiteSpace($programFiles)) { continue }
        $candidate = Join-Path $programFiles ('Unity\Hub\Editor\' + $expectedUnity + '\Editor\Unity.exe')
        if (Test-Path -LiteralPath $candidate -PathType Leaf) { return $candidate }
    }
    throw ('Unity ' + $expectedUnity + ' wurde nicht gefunden. -UnityPath "D:\Unity\Hub\Editor\' + $expectedUnity + '\Editor\Unity.exe" angeben. Es wird kein Editor automatisch installiert.')
}

function Resolve-Executable {
    param([string]$Name)
    if (Test-Path -LiteralPath $Name -PathType Leaf) { return (Get-Item -LiteralPath $Name).FullName }
    $command = Get-Command -Name $Name -CommandType Application -ErrorAction SilentlyContinue | Select-Object -First 1
    if ($null -eq $command) { throw ('Programm nicht gefunden: ' + $Name + '. Ein .NET SDK ab Version 6 wird benoetigt; die Runtime allein reicht nicht.') }
    return $command.Source
}

function ConvertTo-WindowsArgument {
    param([AllowEmptyString()][string]$Value)
    # ProcessStartInfo.Arguments on Windows uses CRT escaping. No cmd.exe or shell.
    $builder = New-Object System.Text.StringBuilder
    [void]$builder.Append('"')
    $slashes = 0
    foreach ($character in $Value.ToCharArray()) {
        if ($character -eq '\') { $slashes++; continue }
        if ($character -eq '"') {
            [void]$builder.Append(('\' * (2 * $slashes + 1)))
            [void]$builder.Append('"')
        }
        else {
            [void]$builder.Append(('\' * $slashes))
            [void]$builder.Append($character)
        }
        $slashes = 0
    }
    [void]$builder.Append(('\' * (2 * $slashes)))
    [void]$builder.Append('"')
    return $builder.ToString()
}

function Invoke-CheckedProcess {
    param([string]$Executable, [string[]]$Arguments, [string]$WorkingDirectory, [string]$LogPath)
    $start = New-Object System.Diagnostics.ProcessStartInfo
    $start.FileName = $Executable
    $start.Arguments = (($Arguments | ForEach-Object { ConvertTo-WindowsArgument $_ }) -join ' ')
    $start.WorkingDirectory = $WorkingDirectory
    $start.UseShellExecute = $false
    $start.CreateNoWindow = $true
    $start.RedirectStandardOutput = $true
    $start.RedirectStandardError = $true
    $process = New-Object System.Diagnostics.Process
    $process.StartInfo = $start
    try {
        if (-not $process.Start()) { throw ('Prozess konnte nicht gestartet werden: ' + $Executable) }
        $stdoutTask = $process.StandardOutput.ReadToEndAsync()
        $stderrTask = $process.StandardError.ReadToEndAsync()
        $process.WaitForExit()
        $stdout = $stdoutTask.GetAwaiter().GetResult()
        $stderr = $stderrTask.GetAwaiter().GetResult()
        $exitCode = $process.ExitCode
        if (-not [string]::IsNullOrWhiteSpace($LogPath)) {
            [IO.File]::WriteAllText($LogPath, ($stdout + [Environment]::NewLine + $stderr), (New-Object Text.UTF8Encoding $false))
        }
        if (-not [string]::IsNullOrWhiteSpace($stdout)) { Write-Host ($stdout.TrimEnd()) }
        if (-not [string]::IsNullOrWhiteSpace($stderr)) { Write-Host ($stderr.TrimEnd()) }
        if ($exitCode -ne 0) { throw ('Prozess fehlgeschlagen, Exitcode ' + $exitCode + ': ' + $Executable + '. Log: ' + $LogPath) }
        return $stdout
    }
    finally { $process.Dispose() }
}

try {
    if ([Environment]::OSVersion.Platform -ne [PlatformID]::Win32NT) {
        throw 'Dieses Buildskript ist fuer Windows und die Windows-IL2CPP-Version von Schedule I vorgesehen.'
    }
    Require-File $projectFile 'WorldLook.csproj'
    Require-File (Join-Path $unityProject 'ProjectSettings\ProjectVersion.txt') 'Unity-Projektversion'
    Require-File (Join-Path $unityProject 'Assets\Editor\BuildWorldLook.cs') 'Unity-Buildskript'

    $resolvedGame = Find-GameDirectory $GamePath
    $resolvedUnity = Find-UnityExecutable $UnityPath
    $resolvedDotNet = Resolve-Executable $DotNetPath
    $interop = Join-Path $resolvedGame 'MelonLoader\Il2CppAssemblies'
    $melon = Join-Path $resolvedGame 'MelonLoader\net6'
    $s1api = Join-Path $resolvedGame 'Mods\S1API.Il2Cpp.MelonLoader.dll'
    foreach ($name in @(
        'Assembly-CSharp.dll', 'Il2CppScheduleOne.Core.dll', 'Il2Cppmscorlib.dll', 'UnityEngine.CoreModule.dll',
        'UnityEngine.AssetBundleModule.dll', 'UnityEngine.InputLegacyModule.dll',
        'UnityEngine.UI.dll', 'Unity.TextMeshPro.dll',
        'Unity.RenderPipelines.Core.Runtime.dll', 'Unity.RenderPipelines.Universal.Runtime.dll'
    )) { Require-File (Join-Path $interop $name) ('Spiel-Interop: ' + $name) }
    foreach ($name in @('MelonLoader.dll', 'Il2CppInterop.Runtime.dll')) {
        Require-File (Join-Path $melon $name) ('MelonLoader: ' + $name)
    }
    Require-File $s1api 'S1 API 3.2.1'
    $s1apiIdentity = [Reflection.AssemblyName]::GetAssemblyName($s1api)
    $s1apiFileVersion = [Diagnostics.FileVersionInfo]::GetVersionInfo($s1api).FileVersion
    # The installed S1API 3.2.1 IL2CPP file is named S1API.Il2Cpp.MelonLoader.dll, but its assembly
    # identity is plain 'S1API' (measured 2026-10-21: S1API, Version=3.2.1.0). Accept both: the file
    # name is the loader contract, the FileVersion below is the version evidence.
    if ($s1apiIdentity.Name -notin @('S1API', 'S1API.Il2Cpp.MelonLoader')) {
        throw ('Unerwarteter S1-API-Assemblyname: ' + $s1apiIdentity.Name + '. Pfad: ' + $s1api)
    }
    if ($s1apiFileVersion -ne '3.2.1.0') {
        throw ('S1 API FileVersion 3.2.1.0 wird erwartet; vorhanden: ' + $s1apiFileVersion + '. Pfad: ' + $s1api)
    }
    $projectVersion = [IO.File]::ReadAllText((Join-Path $unityProject 'ProjectSettings\ProjectVersion.txt'))
    if ($projectVersion -notmatch ('(?m)^m_EditorVersion:\s*' + [regex]::Escape($expectedUnity) + '\s*$')) {
        throw ('Das Shaderprojekt muss Unity ' + $expectedUnity + ' verwenden.')
    }

    $stamp = [DateTime]::UtcNow.ToString('yyyyMMdd-HHmmss-fff')
    $runLogs = Join-Path $PSScriptRoot ('logs\' + $stamp)
    [void](New-Item -ItemType Directory -Path $runLogs -Force)
    Write-Host ('Spiel:  ' + $resolvedGame)
    Write-Host ('Unity:  ' + $resolvedUnity)
    Write-Host ('Logs:   ' + $runLogs)
    $sdkVersion = (Invoke-CheckedProcess -Executable $resolvedDotNet -Arguments @('--version') -WorkingDirectory $PSScriptRoot -LogPath (Join-Path $runLogs 'dotnet-version.log')).Trim()
    $sdkMatch = [regex]::Match($sdkVersion, '^(?<major>\d+)\.\d+\.\d+')
    if (-not $sdkMatch.Success -or [int]$sdkMatch.Groups['major'].Value -lt 6) {
        throw ('Ein .NET SDK ab Version 6 wird benoetigt. Gefunden: ' + $sdkVersion)
    }
    Write-Host ('SDK:    ' + $sdkVersion + '; Ziel-Framework net6.0; S1 API FileVersion ' + $s1apiFileVersion)
    Write-Host ('S1 API AssemblyVersion (zur Dokumentation): ' + $s1apiIdentity.Version)
    if ($CheckOnly) {
        Write-Host 'Dateien und SDK gefunden. CheckOnly hat weder Unity-Build noch .NET-Restore/Compile ausgefuehrt.'
        Write-Host 'Editorlizenz, Paketaufloesung, Shaderkompilierung und Ingame-Verhalten werden erst im weiteren Ablauf geprueft.'
        exit 0
    }

    $assets = Join-Path $projectRoot 'assets'
    [void](New-Item -ItemType Directory -Path $assets -Force)
    $bundle = Join-Path $assets 'worldlook-shaders.bundle'
    $shaderReceipt = Join-Path $assets 'shader-build.json'
    # Only generated inputs are removed. A previous successful dist stays intact
    # until both new compilers have completed successfully.
    foreach ($generated in @($bundle, $shaderReceipt)) {
        if (Test-Path -LiteralPath $generated -PathType Leaf) { Remove-Item -LiteralPath $generated -Force }
    }
    Write-Host '1/3: Shaderbundle mit Unity bauen. Der erste Paketimport kann laenger dauern.'
    $unityLog = Join-Path $runLogs 'unity-build.log'
    [void](Invoke-CheckedProcess -Executable $resolvedUnity -Arguments @(
        '-batchmode', '-quit', '-projectPath', $unityProject,
        '-executeMethod', 'BuildWorldLook.Build', '-buildTarget', 'Win64',
        '-logFile', $unityLog
    ) -WorkingDirectory $projectRoot -LogPath (Join-Path $runLogs 'unity-process.log'))
    Require-File $bundle 'Neu gebautes Shaderbundle'
    Require-File $shaderReceipt 'Neuer Shader-Buildbericht'
    if ((Get-Item -LiteralPath $bundle).Length -eq 0) { throw 'Das neue Shaderbundle ist leer.' }
    $receipt = [IO.File]::ReadAllText($shaderReceipt) | ConvertFrom-Json
    if ($receipt.unityVersion -ne $expectedUnity) { throw ('Falsche Unity-Version im Shader-Buildbericht: ' + $receipt.unityVersion) }
    $bundleHash = Get-Sha256Hex $bundle
    if ($receipt.bundleSha256 -ne $bundleHash) { throw 'Shaderbundle und Shader-Buildbericht stimmen im SHA-256 nicht ueberein.' }

    $buildProperties = @(
        ('-p:GameDir=' + $resolvedGame),
        ('-p:Il2CppAssemblies=' + $interop),
        ('-p:MelonLoaderAssemblies=' + $melon),
        ('-p:S1ApiPath=' + $s1api)
    )
    Write-Host '2/3: .NET-Referenzen aufloesen und WorldLook kompilieren.'
    try {
        [void](Invoke-CheckedProcess -Executable $resolvedDotNet -Arguments (@(
            'restore', $projectFile, '--configfile', (Join-Path $PSScriptRoot 'NuGet.Config'),
            '--verbosity', 'minimal'
        ) + $buildProperties) -WorkingDirectory $PSScriptRoot -LogPath (Join-Path $runLogs 'dotnet-restore.log'))
    }
    catch {
        throw ('Restore fehlgeschlagen. Das Ziel ist net6.0. Fehlende .NET-6-Referenzpakete benoetigen Zugriff auf nuget.org oder eine lokale Installation dieser Pakete. Es wurden keine SDKs automatisch installiert. Details: ' + $_.Exception.Message)
    }
    [void](Invoke-CheckedProcess -Executable $resolvedDotNet -Arguments (@(
        'build', $projectFile, '--no-restore', '--configuration', $Configuration,
        '--verbosity', 'minimal'
    ) + $buildProperties) -WorkingDirectory $PSScriptRoot -LogPath (Join-Path $runLogs 'dotnet-build.log'))
    $binaryDirectory = Join-Path $projectRoot ('bin\' + $Configuration + '\net6.0')
    $dll = Join-Path $binaryDirectory 'WorldLook.dll'
    Require-File $dll 'Neue WorldLook.dll'
    $identity = [Reflection.AssemblyName]::GetAssemblyName($dll)
    if ($identity.Name -ne 'WorldLook') { throw 'Die erzeugte DLL hat einen unerwarteten Assemblynamen.' }

    Write-Host '3/3: Eigenes Build-Ergebnis zusammenstellen.'
    $sourceHashes = New-Object 'System.Collections.Generic.List[object]'
    foreach ($folder in @((Join-Path $projectRoot 'src'), (Join-Path $unityProject 'Assets'))) {
        foreach ($file in (Get-ChildItem -LiteralPath $folder -Recurse -File | Where-Object { $_.Extension -in @('.cs', '.shader', '.hlsl') } | Sort-Object FullName)) {
            $sourceHashes.Add([ordered]@{
                file = $file.FullName.Substring($projectRoot.Length + 1).Replace('\', '/')
                sha256 = Get-Sha256Hex $file.FullName
            })
        }
    }
    $buildReport = [ordered]@{
        artifact = 'WorldLook'
        builtAtUtc = [DateTime]::UtcNow.ToString('o')
        configuration = $Configuration
        targetFramework = 'net6.0'
        dotnetSdk = $sdkVersion
        unityVersion = $receipt.unityVersion
        s1ApiFileVersion = $s1apiFileVersion
        s1ApiAssemblyVersion = $s1apiIdentity.Version.ToString()
        assemblyVersion = $identity.Version.ToString()
        dllSha256 = Get-Sha256Hex $dll
        embeddedBundleSha256 = $bundleHash
        sourceFiles = $sourceHashes.ToArray()
        inGameVerified = $false
    }
    $staging = Join-Path $runLogs 'staging'
    [void](New-Item -ItemType Directory -Path $staging -Force)
    Copy-Item -LiteralPath $dll -Destination (Join-Path $staging 'WorldLook.dll')
    Copy-Item -LiteralPath $shaderReceipt -Destination (Join-Path $staging 'shader-build.json')
    [IO.File]::WriteAllText((Join-Path $staging 'build-info.json'), ($buildReport | ConvertTo-Json -Depth 8), (New-Object Text.UTF8Encoding $false))
    $pdb = Join-Path $binaryDirectory 'WorldLook.pdb'
    if (Test-Path -LiteralPath $pdb -PathType Leaf) { Copy-Item -LiteralPath $pdb -Destination (Join-Path $staging 'WorldLook.pdb') }
    # Explicitly copy only this mod and its own metadata, never game/loader DLLs.
    $destination = Join-Path $projectRoot 'dist'
    [void](New-Item -ItemType Directory -Path $destination -Force)
    foreach ($file in (Get-ChildItem -LiteralPath $staging -File)) {
        Copy-Item -LiteralPath $file.FullName -Destination (Join-Path $destination $file.Name) -Force
    }
    Write-Host ('BUILD ERFOLGREICH: ' + (Join-Path $destination 'WorldLook.dll')) -ForegroundColor Green
    Write-Host 'Das Shaderbundle ist in der DLL eingebettet. Es wurden keine Dateien ins Spielverzeichnis kopiert.'
    Write-Host ('Buildnachweise: ' + (Join-Path $destination 'build-info.json'))
    Write-Host ('Compilerlogs:  ' + $runLogs)
    Write-Host 'Ein erfolgreicher Build ersetzt die anschliessende Pruefung im Spiel nicht.'
    exit 0
}
catch {
    Write-Host ('BUILD FEHLGESCHLAGEN: ' + $_.Exception.Message) -ForegroundColor Red
    if (-not [string]::IsNullOrWhiteSpace($runLogs)) { Write-Host ('Logs: ' + $runLogs) }
    exit 1
}
