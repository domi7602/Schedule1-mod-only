#Requires -Version 7
# Game-free fixtures. No game build, deployment, real-repository Git mutation or network access.
$ErrorActionPreference = 'Stop'
. (Join-Path (Split-Path -Parent $PSScriptRoot) 'release-manifest.ps1')
$script:Passed = 0
$script:Failed = 0
$fixture = New-S1ReleaseStagingDirectory
$source = [pscustomobject]@{ Head = ('a' * 40); Dirty = $false; Fingerprint = ('b' * 64) }

function Check {
    param([string]$Name, [scriptblock]$Body)
    try { & $Body; $script:Passed++; Write-Host "PASS $Name" }
    catch { $script:Failed++; Write-Host "FAIL $Name : $($_.Exception.Message)" -ForegroundColor Red }
}
function Require {
    param([bool]$Condition, [string]$Message)
    if (-not $Condition) { throw $Message }
}
function Reject {
    param([scriptblock]$Body, [string]$Pattern)
    $failure = $null
    try { & $Body | Out-Null } catch { $failure = $_.Exception.Message }
    if (-not $failure -or $failure -notmatch $Pattern) { throw "Expected rejection matching '$Pattern', got '$failure'." }
}
function New-Payload {
    $path = Join-Path $fixture.Path ([guid]::NewGuid().ToString('N'))
    $null = New-Item -ItemType Directory -Path (Join-Path $path 'Mods'), (Join-Path $path 'UserData/Fixture') -Force
    [IO.File]::WriteAllText((Join-Path $path 'Mods/Fixture.dll'), 'fixture-dll-v1')
    [IO.File]::WriteAllText((Join-Path $path 'Mods/Shared.dll'), 'fixture-shared-v1')
    [IO.File]::WriteAllText((Join-Path $path 'UserData/Fixture/mod.json'), '{"name":"Fixture","version":"1.2.3"}')
    return $path
}
function Zip-Payload {
    param([string]$Path)
    $zip = Join-Path $fixture.Path ([guid]::NewGuid().ToString('N') + '.zip')
    [IO.Compression.ZipFile]::CreateFromDirectory($Path, $zip)
    return $zip
}
function Rewrite-Manifest {
    param([string]$Path, [scriptblock]$Edit)
    $file = Join-Path $Path 'release-manifest.json'
    $manifest = Get-Content -LiteralPath $file -Raw | ConvertFrom-Json
    & $Edit $manifest
    [IO.File]::WriteAllText($file, (ConvertTo-Json $manifest -Depth 10))
}

try {
    Check 'Valid archive verifies every payload and clean source' {
        $payload = New-Payload
        $null = Write-S1ReleaseManifest $payload Fixture $source
        $archive = Assert-S1ReleaseArchive (Zip-Payload $payload) Fixture '1.2.3' $source -RequireClean
        Require ($archive.Files.Count -eq 3) 'Wrong payload count.'
    }
    Check 'DLL tampering is detected' {
        $payload = New-Payload
        $null = Write-S1ReleaseManifest $payload Fixture $source
        [IO.File]::WriteAllText((Join-Path $payload 'Mods/Fixture.dll'), 'fixture-dll-v2')
        Reject { Assert-S1ReleaseArchive (Zip-Payload $payload) Fixture '1.2.3' $source } 'SHA-256 mismatch'
    }
    Check 'Extra payload is refused' {
        $payload = New-Payload
        $null = Write-S1ReleaseManifest $payload Fixture $source
        [IO.File]::WriteAllText((Join-Path $payload 'unexpected.dll'), 'extra')
        Reject { Assert-S1ReleaseArchive (Zip-Payload $payload) Fixture '1.2.3' $source } 'undeclared'
    }
    Check 'Missing payload is refused' {
        $payload = New-Payload
        $null = Write-S1ReleaseManifest $payload Fixture $source
        Remove-Item -LiteralPath (Join-Path $payload 'Mods/Shared.dll')
        Reject { Assert-S1ReleaseArchive (Zip-Payload $payload) Fixture '1.2.3' $source } 'missing'
    }
    Check 'Filename-only legacy ZIP is refused' {
        Reject { Assert-S1ReleaseArchive (Zip-Payload (New-Payload)) Fixture '1.2.3' $source } 'no provenance'
    }
    Check 'Wrong commit is refused' {
        $payload = New-Payload
        $null = Write-S1ReleaseManifest $payload Fixture $source
        $other = [pscustomobject]@{ Head = ('c' * 40); Dirty = $false; Fingerprint = $source.Fingerprint }
        Reject { Assert-S1ReleaseArchive (Zip-Payload $payload) Fixture '1.2.3' $other } 'source identity differs'
    }
    Check 'Changed source with unchanged HEAD is refused' {
        $payload = New-Payload
        $null = Write-S1ReleaseManifest $payload Fixture $source
        $other = [pscustomobject]@{ Head = $source.Head; Dirty = $false; Fingerprint = ('c' * 64) }
        Reject { Assert-S1ReleaseArchive (Zip-Payload $payload) Fixture '1.2.3' $other } 'source identity differs'
    }
    Check 'Wrong version is refused' {
        $payload = New-Payload
        $null = Write-S1ReleaseManifest $payload Fixture $source
        Reject { Assert-S1ReleaseArchive (Zip-Payload $payload) Fixture '1.2.4' $source } 'version'
    }
    Check 'Dirty package supports local distribution but cannot publish' {
        $payload = New-Payload
        $dirty = [pscustomobject]@{ Head = $source.Head; Dirty = $true; Fingerprint = $source.Fingerprint }
        $null = Write-S1ReleaseManifest $payload Fixture $dirty
        $zip = Zip-Payload $payload
        $null = Assert-S1ReleaseArchive $zip Fixture '1.2.3' $dirty
        Reject { Assert-S1ReleaseArchive $zip Fixture '1.2.3' $dirty -RequireClean } 'clean source'
    }
    Check 'Dirty current source cannot publish a clean package' {
        $payload = New-Payload
        $null = Write-S1ReleaseManifest $payload Fixture $source
        $dirty = [pscustomobject]@{ Head = $source.Head; Dirty = $true; Fingerprint = $source.Fingerprint }
        Reject { Assert-S1ReleaseArchive (Zip-Payload $payload) Fixture '1.2.3' $dirty -RequireClean } 'clean source'
    }
    foreach ($required in @('Mods/Fixture.dll', 'Mods/Shared.dll', 'UserData/Fixture/mod.json')) {
        Check "Missing required input $required aborts packaging" {
            $payload = New-Payload
            Remove-Item -LiteralPath (Join-Path $payload $required)
            Reject { Write-S1ReleaseManifest $payload Fixture $source } 'missing'
        }
    }
    Check 'Malformed metadata aborts packaging' {
        $payload = New-Payload
        [IO.File]::WriteAllText((Join-Path $payload 'UserData/Fixture/mod.json'), '{broken')
        Reject { Write-S1ReleaseManifest $payload Fixture $source } 'metadata'
    }
    Check 'Missing version does not fall back to 1.0.0' {
        $payload = New-Payload
        [IO.File]::WriteAllText((Join-Path $payload 'UserData/Fixture/mod.json'), '{"name":"Fixture"}')
        Reject { Write-S1ReleaseManifest $payload Fixture $source } 'semantic version'
    }
    Check 'Metadata for another mod is refused' {
        $payload = New-Payload
        [IO.File]::WriteAllText((Join-Path $payload 'UserData/Fixture/mod.json'), '{"name":"Other","version":"1.2.3"}')
        Reject { Write-S1ReleaseManifest $payload Fixture $source } 'metadata'
    }
    Check 'Consistently hashed metadata must still match manifest version' {
        $payload = New-Payload
        $null = Write-S1ReleaseManifest $payload Fixture $source
        $meta = Join-Path $payload 'UserData/Fixture/mod.json'
        [IO.File]::WriteAllText($meta, '{"name":"Fixture","version":"9.9.9"}')
        Rewrite-Manifest $payload {
            param($manifest)
            $record = $manifest.Files | Where-Object Path -EQ 'UserData/Fixture/mod.json'
            $record.Sha256 = (Get-FileHash -LiteralPath $meta -Algorithm SHA256).Hash.ToLowerInvariant()
            $record.Length = (Get-Item -LiteralPath $meta).Length
        }
        Reject { Assert-S1ReleaseArchive (Zip-Payload $payload) Fixture '1.2.3' $source } 'Metadata version'
    }
    Check 'Deleting Shared from both ZIP and manifest still fails' {
        $payload = New-Payload
        $null = Write-S1ReleaseManifest $payload Fixture $source
        Remove-Item -LiteralPath (Join-Path $payload 'Mods/Shared.dll')
        Rewrite-Manifest $payload { param($manifest) $manifest.Files = @($manifest.Files | Where-Object Path -NE 'Mods/Shared.dll') }
        Reject { Assert-S1ReleaseArchive (Zip-Payload $payload) Fixture '1.2.3' $source } 'Required package'
    }
    Check 'Path traversal and case-duplicate ZIP entries are refused' {
        foreach ($badPath in @('../outside', 'mods/fixture.dll')) {
            $payload = New-Payload
            $null = Write-S1ReleaseManifest $payload Fixture $source
            $zip = Zip-Payload $payload
            $archive = [IO.Compression.ZipFile]::Open($zip, [IO.Compression.ZipArchiveMode]::Update)
            try { $null = $archive.CreateEntry($badPath) } finally { $archive.Dispose() }
            Reject { Assert-S1ReleaseArchive $zip Fixture '1.2.3' $source } 'Unsafe|Duplicate'
        }
    }
    Check 'Cleanup refuses outside paths and leaves files intact' {
        $sentinel = Join-Path $fixture.Path 'sentinel.txt'
        [IO.File]::WriteAllText($sentinel, 'keep')
        Reject { Remove-S1ReleaseStagingDirectory ([pscustomobject]@{ Parent = $fixture.Parent; Path = $fixture.Parent }) } 'outside'
        Require (Test-Path -LiteralPath $sentinel) 'Sentinel was deleted.'
    }
    Check 'Staging names are unique and independent' {
        $first = New-S1ReleaseStagingDirectory
        $second = New-S1ReleaseStagingDirectory
        try { Require ($first.Path -ne $second.Path) 'Staging collision.' }
        finally { Remove-S1ReleaseStagingDirectory $first; Remove-S1ReleaseStagingDirectory $second }
    }
    Check 'Source fingerprint includes content, untracked input and submodule state' {
        # The Git inventory is a narrow fixture seam; hashing operates on real files.
        $originalGit = (Get-Command Invoke-S1ReleaseGit).ScriptBlock
        $script:FakeDirty = ''
        $script:FakeSubmodule = ''
        $script:FakePaths = "tracked.txt`0"
        $script:FakeIgnored = ''
        function Invoke-S1ReleaseGit {
            param($Root, $Arguments)
            switch ($Arguments[0]) {
                'rev-parse' { return ('a' * 40) }
                'submodule' { return $script:FakeSubmodule }
                'status' { return $script:FakeDirty }
                'ls-files' { if ($Arguments -contains '--ignored') { return $script:FakeIgnored }; return $script:FakePaths }
                default { throw 'Unexpected fixture Git invocation.' }
            }
        }
        try {
            $root = Join-Path $fixture.Path 'source'
            $null = New-Item -ItemType Directory -Path $root
            [IO.File]::WriteAllText((Join-Path $root 'tracked.txt'), 'one')
            $first = Get-S1ReleaseSourceIdentity $root
            [IO.File]::WriteAllText((Join-Path $root 'tracked.txt'), 'two')
            $script:FakeDirty = ' M tracked.txt'
            $second = Get-S1ReleaseSourceIdentity $root
            Require ($second.Dirty -and $first.Fingerprint -ne $second.Fingerprint) 'Changed source not identified.'
            [IO.File]::WriteAllText((Join-Path $root 'untracked.txt'), 'extra')
            $script:FakePaths += "untracked.txt`0"
            $third = Get-S1ReleaseSourceIdentity $root
            Require ($third.Fingerprint -ne $second.Fingerprint) 'Untracked source omitted.'
            $script:FakeSubmodule = '-aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa ThirdParty/Missing'
            Reject { Get-S1ReleaseSourceIdentity $root } 'uninitialized'
        } finally { Set-Item Function:Invoke-S1ReleaseGit $originalGit }
    }
    Check 'Ignored SDK source and runtime assets cannot be certified as clean' {
        $originalGit = (Get-Command Invoke-S1ReleaseGit).ScriptBlock
        $script:IgnoredProbe = ''
        function Invoke-S1ReleaseGit {
            param($Root, $Arguments)
            if ($Arguments[0] -eq 'ls-files' -and $Arguments -contains '--ignored') { return $script:IgnoredProbe }
            throw 'Unexpected ignored-input fixture Git invocation.'
        }
        try {
            foreach ($path in @(
                'Source/Mods/NotesApp/src/LocalPatch.tmp.cs',
                'Source/Mods/NotesApp/src/scratch/Hidden.cs',
                'Source/Mods/NotesApp/src/scratch/GeneratorInput.txt',
                'Source/Mods/NotesApp/src/icon.png',
                'Source/Mods/NotesApp/src/Directory.Build.targets',
                'Source/Mods/Directory.Build.props',
                'Source/Mods/NotesApp/Directory.Build.props',
                'Source/Mods/NotesApp/assets/icon.png',
                'Source/Mods/NotesApp/assets/extra.bundle',
                'Source/Mods/TaxiDriver/assets/car.glb',
                'Source/Mods/WorldLook/assets/embedded/worldlook-shaders.bundle',
                'Source/Mods/WorldLook/assets/embedded/shader-build.json',
                'Source/Mods/HitmanPhone/assets/polaroid.png')) {
                $script:IgnoredProbe = $path + [char]0
                Reject { Assert-S1ReleaseIgnoredInputs $fixture.Path } 'Ignored production input'
            }
            $script:IgnoredProbe = "Source/Mods/NotesApp/src/obj/Release/AssemblyInfo.cs`0Source/Mods/NotesApp/src/bin/Release/NotesApp.dll`0Source/Mods/WorldLook/build/logs/prior/Build.cs`0"
            Assert-S1ReleaseIgnoredInputs $fixture.Path
        } finally { Set-Item Function:Invoke-S1ReleaseGit $originalGit }
    }
    Check 'Real Git ignores and info/exclude expose hidden production inputs' {
        # An unborn fixture repository needs no commit, tag, remote or user identity.
        $root = Join-Path $fixture.Path 'ignored-git'
        $null = New-Item -ItemType Directory -Path $root
        $null = Invoke-S1ReleaseGit $root @('init', '--quiet')
        [IO.File]::WriteAllText((Join-Path $root '.gitignore'), "*.tmp.cs`nscratch/`nbin/`nobj/`n")
        foreach ($path in @('Source/Mods/Fixture/src/LocalPatch.tmp.cs', 'Source/Mods/Fixture/src/scratch/Hidden.cs')) {
            $full = Join-Path $root $path
            $null = New-Item -ItemType Directory -Path (Split-Path -Parent $full) -Force
            [IO.File]::WriteAllText($full, '// compiled by default SDK globs')
            Reject { Assert-S1ReleaseIgnoredInputs $root } ([regex]::Escape($path))
            Remove-Item -LiteralPath $full
        }
        $asset = 'Source/Mods/Fixture/assets/local.png'
        [IO.File]::WriteAllText((Join-Path $root '.git/info/exclude'), $asset + "`n")
        $null = New-Item -ItemType Directory -Path (Join-Path $root 'Source/Mods/Fixture/assets') -Force
        [IO.File]::WriteAllText((Join-Path $root $asset), 'ignored icon')
        Reject { Assert-S1ReleaseIgnoredInputs $root } ([regex]::Escape($asset))
    }
} finally { Remove-S1ReleaseStagingDirectory $fixture }
Write-Host "Release fixtures: $script:Passed passed, $script:Failed failed."
if ($script:Failed) { exit 1 }
