[CmdletBinding()]
param(
    [ValidateSet('Core', 'Diagnostics', 'Mod')][string]$Mode = 'Core',
    [string]$DotNet = 'dotnet',
    [string]$ReferenceDirectory,
    [string]$OutputRoot,
    [string]$Version
)

$ErrorActionPreference = 'Stop'
Set-StrictMode -Version Latest
$ProjectRoot = [IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
$Utf8 = New-Object Text.UTF8Encoding($false)
$BuildRecord = $null
$RunDirectory = $null

function Write-JsonFile($Path, $Value) {
    [IO.File]::WriteAllText($Path, ($Value | ConvertTo-Json -Depth 12), $Utf8)
}

function Hash-Text([string]$Value) {
    $algorithm = [Security.Cryptography.SHA256]::Create()
    try { return ([BitConverter]::ToString($algorithm.ComputeHash($Utf8.GetBytes($Value)))).Replace('-', '').ToLowerInvariant() }
    finally { $algorithm.Dispose() }
}

function Get-SafeSourceFiles([string]$Directory) {
    $pending = New-Object 'System.Collections.Generic.Stack[string]'
    $pending.Push($Directory)
    while ($pending.Count -gt 0) {
        foreach ($entry in Get-ChildItem -LiteralPath $pending.Pop() -Force) {
            if (($entry.Attributes -band [IO.FileAttributes]::ReparsePoint) -ne 0) { continue }
            if ($entry.PSIsContainer) {
                if ($entry.Name -notin @('bin', 'obj', '.git', '.vs', 'artifacts', 'references')) { $pending.Push($entry.FullName) }
            } elseif ($entry.Extension -in @('.cs', '.csproj', '.json', '.resx', '.props', '.targets', '.xml')) {
                $entry
            }
        }
    }
}

function Invoke-DotNet([string[]]$Arguments, [string]$LogName) {
    $log = Join-Path $RunDirectory $LogName
    # Logs remain local under artifacts and are excluded from the source archive.
    & $DotNet @Arguments 2>&1 | Tee-Object -FilePath $log | ForEach-Object { Write-Host $_ }
    if ($LASTEXITCODE -ne 0) { throw "dotnet $($Arguments[0]) fehlgeschlagen (Exitcode $LASTEXITCODE). Kein Kandidat freigegeben; lokaler Beleg: $log" }
}

try {
    $integration = Get-Content -LiteralPath (Join-Path $PSScriptRoot 'integration-status.json') -Raw | ConvertFrom-Json
    if ($Mode -eq 'Mod' -and -not $integration.mod_implemented) {
        throw 'Die produktive LightControl-Mod ist noch nicht integriert. Core und Diagnostics sind getrennte Zwischenartefakte. Mod-Build und Release bleiben gesperrt.'
    }
    [xml]$properties = Get-Content -LiteralPath (Join-Path $ProjectRoot 'Directory.Build.props') -Raw
    $versionNode = $properties.SelectSingleNode('//Version')
    if ($null -eq $versionNode -or -not $versionNode.InnerText) { throw 'Version fehlt in Directory.Build.props. Keine implizite Releaseversion wird vergeben.' }
    if ($Version -and $Version -ne $versionNode.InnerText) { throw 'Version darf nicht von Directory.Build.props abweichen. Änderungen müssen zuerst im Quellstand und in den Loader-Attributen nachvollziehbar erfolgen.' }
    $Version = $versionNode.InnerText
    if ($Version -notmatch '^\d+\.\d+\.\d+(?:-[A-Za-z0-9.]+)?$') { throw 'Ungültige Projektversion.' }
    $sdk = (Get-Content -LiteralPath (Join-Path $ProjectRoot 'global.json') -Raw | ConvertFrom-Json).sdk.version
    if (-not $OutputRoot) { $OutputRoot = Join-Path $ProjectRoot 'artifacts/builds' }
    $OutputRoot = [IO.Path]::GetFullPath($OutputRoot)
    $artifactRoot = [IO.Path]::GetFullPath((Join-Path $ProjectRoot 'artifacts'))
    if (-not ($OutputRoot.Equals($artifactRoot, [StringComparison]::OrdinalIgnoreCase) -or $OutputRoot.StartsWith($artifactRoot + [IO.Path]::DirectorySeparatorChar, [StringComparison]::OrdinalIgnoreCase))) {
        throw 'OutputRoot muss im artifacts-Unterordner des Projekts liegen. Quellordner sind als Buildziel gesperrt.'
    }
    $walk = $ProjectRoot
    foreach ($part in $OutputRoot.Substring($ProjectRoot.Length + 1).Split([IO.Path]::DirectorySeparatorChar)) {
        $walk = Join-Path $walk $part
        if ((Test-Path -LiteralPath $walk) -and ((Get-Item -LiteralPath $walk).Attributes -band [IO.FileAttributes]::ReparsePoint)) { throw 'OutputRoot darf keinen Verzeichnislink enthalten.' }
    }
    $buildId = (Get-Date).ToUniversalTime().ToString('yyyyMMddTHHmmssZ') + '-' + [Guid]::NewGuid().ToString('N').Substring(0, 8)
    $RunDirectory = Join-Path $OutputRoot $buildId
    New-Item -ItemType Directory -Path $RunDirectory -ErrorAction Stop | Out-Null
    $snapshot = Join-Path $RunDirectory 'source'
    $binaryDirectory = Join-Path $RunDirectory 'bin'
    New-Item -ItemType Directory -Path $snapshot, $binaryDirectory | Out-Null
    $artifactKind = @{ Core = 'core-only'; Diagnostics = 'diagnostics'; Mod = 'mod' }[$Mode]
    $BuildRecord = [ordered]@{
        schema_version = 1; status = 'failed'; artifact_kind = $artifactKind; build_id = $buildId
        mod_version = $Version; configuration = 'Release'; sdk_expected = $sdk; sdk_version = $null
        started_utc = (Get-Date).ToUniversalTime().ToString('o'); fresh_source_snapshot = $true
        source_fingerprint = $null; source_files = @(); references = @(); artifacts = @()
        core_tests = 'not-run'; runtime_tests = 'not-run'; failure = $null
    }
    $sources = @()
    foreach ($name in @('global.json', 'NuGet.Config', 'nuget.config', 'Directory.Build.props', 'Directory.Build.targets', 'LightControl.sln', '.editorconfig')) {
        $path = Join-Path $ProjectRoot $name
        if (Test-Path -LiteralPath $path -PathType Leaf) { $sources += Get-Item -LiteralPath $path }
    }
    foreach ($folder in @('src', 'tests', 'assets')) {
        $path = Join-Path $ProjectRoot $folder
        if (Test-Path -LiteralPath $path -PathType Container) { $sources += @(Get-SafeSourceFiles $path) }
    }
    $sourceLines = @()
    foreach ($file in $sources | Sort-Object FullName -Unique) {
        if (($file.Attributes -band [IO.FileAttributes]::ReparsePoint) -ne 0) { throw "Quelle ist ein Link: $($file.Name)" }
        $relative = $file.FullName.Substring($ProjectRoot.Length + 1).Replace('\', '/')
        $destination = Join-Path $snapshot $relative
        New-Item -ItemType Directory -Path ([IO.Path]::GetDirectoryName($destination)) -Force | Out-Null
        Copy-Item -LiteralPath $file.FullName -Destination $destination
        $hash = (Get-FileHash -LiteralPath $destination -Algorithm SHA256).Hash.ToLowerInvariant()
        if ($hash -ne (Get-FileHash -LiteralPath $file.FullName -Algorithm SHA256).Hash.ToLowerInvariant()) { throw "Quelle änderte sich beim Erstellen des Snapshots: $relative" }
        $BuildRecord.source_files += [ordered]@{ path = $relative; sha256 = $hash }
        $sourceLines += "$relative`t$hash"
    }
    $BuildRecord.source_fingerprint = Hash-Text (($sourceLines -join "`n") + "`n")
    $project = switch ($Mode) {
        'Core' { 'tests/LightControl.Core.Tests/LightControl.Core.Tests.csproj' }
        'Diagnostics' { 'src/LightControl.Diagnostics/LightControl.Diagnostics.csproj' }
        'Mod' { 'src/LightControl.Mod/LightControl.Mod.csproj' }
    }
    $projectPath = Join-Path $snapshot $project
    if (-not (Test-Path -LiteralPath $projectPath -PathType Leaf)) { throw "Benötigtes Projekt fehlt: $project. Ein vorhandener alter Build wird nicht verwendet." }
    $extraArguments = @("-p:Version=$Version")
    if ($Mode -ne 'Core') {
        if (-not $ReferenceDirectory) { throw 'ReferenceDirectory ist erforderlich; es gibt keine automatische Suche nach einer Spielinstallation.' }
        $ReferenceDirectory = [IO.Path]::GetFullPath($ReferenceDirectory)
        $referenceNames = @('MelonLoader.dll')
        if ($Mode -eq 'Mod') {
            $contract = Join-Path $PSScriptRoot 'mod-build-references.json'
            if (-not (Test-Path -LiteralPath $contract -PathType Leaf)) { throw 'Die bestätigte Referenzliste mod-build-references.json fehlt; keine Referenznamen werden geraten.' }
            $referenceNames = @((Get-Content -LiteralPath $contract -Raw | ConvertFrom-Json).files)
        }
        $referenceSnapshot = Join-Path $RunDirectory 'references'
        New-Item -ItemType Directory -Path $referenceSnapshot | Out-Null
        foreach ($name in $referenceNames) {
            if ($name -notmatch '^[A-Za-z0-9_.-]+\.dll$') { throw 'Referenzliste enthält einen ungültigen Dateinamen.' }
            $path = Join-Path $ReferenceDirectory $name
            if (-not (Test-Path -LiteralPath $path -PathType Leaf)) { throw "Bestätigte Buildreferenz fehlt: $name. Kein alter Build wird übernommen." }
            $target = Join-Path $referenceSnapshot $name
            Copy-Item -LiteralPath $path -Destination $target
            $hash = (Get-FileHash -LiteralPath $target -Algorithm SHA256).Hash.ToLowerInvariant()
            if ($hash -ne (Get-FileHash -LiteralPath $path -Algorithm SHA256).Hash.ToLowerInvariant()) { throw "Referenz änderte sich während der Übernahme: $name" }
            $identity = [Reflection.AssemblyName]::GetAssemblyName($target)
            $BuildRecord.references += [ordered]@{ file = $name; sha256 = $hash; size = (Get-Item -LiteralPath $target).Length; assembly_name = $identity.Name; assembly_version = $identity.Version.ToString() }
        }
        $extraArguments += "-p:LightControlReferenceDirectory=$referenceSnapshot"
    }
    Push-Location $snapshot
    try {
        $actualSdk = (& $DotNet --version 2>&1 | Out-String).Trim()
        if ($LASTEXITCODE -ne 0 -or $actualSdk -ne $sdk) { throw "Erforderliches SDK $sdk ist nicht aktiv. dotnet meldet: $actualSdk" }
        $BuildRecord.sdk_version = $actualSdk
        Invoke-DotNet -Arguments (@('restore', $projectPath) + $extraArguments) -LogName 'restore.log'
        Invoke-DotNet -Arguments (@('build', $projectPath, '--configuration', 'Release', '--no-restore', '--output', $binaryDirectory) + $extraArguments) -LogName 'build.log'
        if ($Mode -eq 'Core') {
            $testRunner = Join-Path $binaryDirectory 'LightControl.Core.Tests.dll'
            if (-not (Test-Path -LiteralPath $testRunner -PathType Leaf)) { throw 'Der frisch erzeugte Kerntest-Runner fehlt.' }
            Invoke-DotNet -Arguments @($testRunner) -LogName 'core-tests.log'
            $BuildRecord.core_tests = 'passed'
        }
    } finally { Pop-Location }
    $assemblyFile = switch ($Mode) { 'Core' { 'LightControl.Core.dll' }; 'Diagnostics' { 'LightControl.Diagnostics.dll' }; 'Mod' { 'LightControl.dll' } }
    $candidate = Join-Path $binaryDirectory $assemblyFile
    if (-not (Test-Path -LiteralPath $candidate -PathType Leaf)) { throw "Frisch erwartete Assembly fehlt: $assemblyFile" }
    $assembly = [Reflection.AssemblyName]::GetAssemblyName($candidate)
    $BuildRecord.artifacts = @([ordered]@{
        path = $candidate.Substring($ProjectRoot.Length + 1).Replace('\', '/')
        sha256 = (Get-FileHash -LiteralPath $candidate -Algorithm SHA256).Hash.ToLowerInvariant()
        size = (Get-Item -LiteralPath $candidate).Length
        assembly_name = $assembly.Name; assembly_version = $assembly.Version.ToString()
    })
    $BuildRecord.status = 'succeeded'
    Write-JsonFile (Join-Path $RunDirectory 'build-record.json') $BuildRecord
    Write-Host "Erzeugt und als $artifactKind dokumentiert: $candidate"
    Write-Host 'Spielprüfungen T01-T18 wurden dadurch nicht ausgeführt. Paketierung erfolgt getrennt und ohne Neubuild.'
    exit 0
} catch {
    if ($null -ne $BuildRecord -and $null -ne $RunDirectory) {
        $BuildRecord.failure = $_.Exception.Message
        $BuildRecord.status = 'failed'
        $BuildRecord.artifacts = @()
        Write-JsonFile (Join-Path $RunDirectory 'build-record.json') $BuildRecord
    }
    Write-Error -Message $_.Exception.Message -ErrorAction Continue
    exit 1
}
