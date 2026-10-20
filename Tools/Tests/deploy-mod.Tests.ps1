# Regression tests for Tools/deploy-mod.ps1 (complete per-project deploy plan,
# receipt, bounded in-process rollback).
#
# Self-contained harness (no Pester, no external modules) in the same style as
# Tools/Tests/deploy.Tests.ps1. Every install is a disposable fake under the
# Hermes scratch directory: no real game files are read or written.

$ErrorActionPreference = 'Stop'

$repo = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
$scriptPath = Join-Path $repo 'Tools\deploy-mod.ps1'
$targetsPath = Join-Path $repo 'Source\Mods\Directory.Build.targets'
$scratch = Join-Path $env:LOCALAPPDATA 'hermes/cache/scratch'
$root = Join-Path $scratch ('s1-deploy-mod-' + [guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $root -Force | Out-Null

$script:Passed = 0
$script:Failed = 0
$script:Skipped = 0

function Assert-True {
    param([bool]$Condition, [string]$Name)
    if ($Condition) {
        Write-Host "PASS  $Name" -ForegroundColor Green
        $script:Passed++
    }
    else {
        Write-Host "FAIL  $Name" -ForegroundColor Red
        $script:Failed++
    }
}

function Assert-Equal {
    param($Expected, $Actual, [string]$Name)
    Assert-True ($Expected -eq $Actual) "$Name (expected '$Expected', got '$Actual')"
}

function Skip-Test {
    param([string]$Name, [string]$Reason)
    Write-Host "SKIP  $Name ($Reason)" -ForegroundColor Yellow
    $script:Skipped++
}

function New-FakeInstall {
    param([string]$Name)
    $game = Join-Path $root $Name
    New-Item -ItemType Directory -Path (Join-Path $game 'MelonLoader\Il2CppAssemblies') -Force | Out-Null
    New-Item -ItemType Directory -Path (Join-Path $game 'Mods') -Force | Out-Null
    Set-Content -LiteralPath (Join-Path $game 'MelonLoader\Il2CppAssemblies\Assembly-CSharp.dll') -Value 'fake-probe'
    return $game
}

function New-FakeProject {
    param(
        [string]$Name,
        [string]$Tag = 'a',
        [switch]$NoAssets,
        [switch]$NoPdb,
        [switch]$NoJson
    )

    $projRoot = Join-Path $root ("proj-$Name-$Tag-" + [guid]::NewGuid().ToString('N').Substring(0, 6))
    $src = Join-Path $projRoot 'src'
    $assets = Join-Path $projRoot 'assets'
    $docs = Join-Path $projRoot 'docs'
    New-Item -ItemType Directory -Path $src -Force | Out-Null
    New-Item -ItemType Directory -Path $docs -Force | Out-Null

    $dll = Join-Path $src "$Name.dll"
    [IO.File]::WriteAllText($dll, "dll-$Name-$Tag")
    $pdb = Join-Path $src "$Name.pdb"
    if (-not $NoPdb) { [IO.File]::WriteAllText($pdb, "pdb-$Name-$Tag") }

    $json = Join-Path $docs 'mod.json'
    if (-not $NoJson) { [IO.File]::WriteAllText($json, "{`"name`":`"$Name`",`"version`":`"1.0.0`"}") }

    if (-not $NoAssets) {
        New-Item -ItemType Directory -Path $assets -Force | Out-Null
        [IO.File]::WriteAllText((Join-Path $assets "$($Name.ToLowerInvariant())_icon.png"), "icon-$Name-$Tag")
        [IO.File]::WriteAllText((Join-Path $assets 'pack.bundle'), "bundle-$Name-$Tag")
        [IO.File]::WriteAllText((Join-Path $assets "$($Name.ToLowerInvariant()).glb"), "glb-$Name-$Tag")
        [IO.File]::WriteAllText((Join-Path $src 'src_icon.png'), "srcicon-$Name-$Tag")
    }

    return [pscustomobject]@{
        Name       = $Name
        ProjectDir = $src
        Dll        = $dll
        Pdb        = $pdb
        Assets     = $assets
        ModJson    = $json
    }
}

function Invoke-DeployFixture {
    param(
        [Parameter(Mandatory)]$Fixture,
        [Parameter(Mandatory)][string]$GameDir,
        [string]$Configuration = 'Release',
        [int]$MutexTimeoutSeconds = 20
    )

    return Invoke-S1ModDeploy -GameDir $GameDir -ProjectName $Fixture.Name `
        -ProjectDir $Fixture.ProjectDir -TargetPath $Fixture.Dll -PdbPath $Fixture.Pdb `
        -AssetsDir $Fixture.Assets -ModJsonPath $Fixture.ModJson `
        -Configuration $Configuration -MutexTimeoutSeconds $MutexTimeoutSeconds
}

function Get-HashOf {
    param([string]$Path)
    if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) { return $null }
    return (Get-FileHash -LiteralPath $Path -Algorithm SHA256).Hash
}

function Get-CodeFromException {
    param($Exception)
    if ($null -ne $Exception.Data -and $Exception.Data.Contains('S1ExitCode')) {
        return [int]$Exception.Data['S1ExitCode']
    }
    return -1
}

function Invoke-Child {
    param([string[]]$Arguments)
    $output = & pwsh -NoProfile -NonInteractive -File $scriptPath @Arguments 2>&1
    return [pscustomobject]@{ ExitCode = $LASTEXITCODE; Output = ($output | Out-String) }
}

Write-Host "Scratch root: $root" -ForegroundColor Cyan

# Dot-source: defines helpers and must not deploy anything by itself.
. $scriptPath
$originalCopy = (Get-Command Copy-S1DeployTargetFile).ScriptBlock
$originalRestore = (Get-Command Restore-S1DeployTargetFile).ScriptBlock
$originalProcessProvider = (Get-Command Get-S1GameProcesses).ScriptBlock

# ------------------------------------------------- 1. complete plan + receipt
$game1 = New-FakeInstall 'game-full'
$fx1 = New-FakeProject -Name 'FullMod' -Tag 'v1'
$result1 = Invoke-DeployFixture -Fixture $fx1 -GameDir $game1

$expectedFiles = [ordered]@{
    'Mods\FullMod.dll'                 = $fx1.Dll
    "Mods\$($fx1.Name.ToLowerInvariant())_icon.png" = (Join-Path $fx1.Assets "$($fx1.Name.ToLowerInvariant())_icon.png")
    'Mods\src_icon.png'                = (Join-Path $fx1.ProjectDir 'src_icon.png')
    'Mods\pack.bundle'                 = (Join-Path $fx1.Assets 'pack.bundle')
    "Mods\FullMod\fullmod.glb"         = (Join-Path $fx1.Assets 'fullmod.glb')
    'UserData\FullMod\FullMod.pdb'     = $fx1.Pdb
    'UserData\FullMod\mod.json'        = $fx1.ModJson
}

$allLanded = $true
foreach ($key in $expectedFiles.Keys) {
    $dest = Join-Path $game1 $key
    if ((Get-HashOf $dest) -ne (Get-HashOf $expectedFiles[$key])) { $allLanded = $false }
}
Assert-True $allLanded 'deploy: DLL, PDB, mod.json, icons, bundle and GLB all land at the conventional paths with matching SHA256'
Assert-Equal 'Deployed' $result1.Status 'deploy: success status'
Assert-Equal 7 $result1.Copied 'deploy: all seven plan files copied on first run'
Assert-Equal 0 $result1.Unchanged 'deploy: first run has no unchanged files'

$receiptPath = Join-Path $game1 'UserData\.deploy\receipts\FullMod.json'
Assert-True (Test-Path -LiteralPath $receiptPath -PathType Leaf) 'receipt: written under <GameDir>\UserData\.deploy\receipts\<Mod>.json'
$receipt = Get-Content -LiteralPath $receiptPath -Raw | ConvertFrom-Json
Assert-Equal 'Deployed' $receipt.Status 'receipt: status Deployed'
Assert-Equal 'FullMod' $receipt.Project 'receipt: project name'
Assert-Equal 'Release' $receipt.Configuration 'receipt: configuration'
Assert-Equal ([IO.Path]::TrimEndingDirectorySeparator([IO.Path]::GetFullPath($game1))) $receipt.GameDir 'receipt: effective GameDir recorded exactly'
Assert-True ($receipt.GitRevision -match '^[0-9a-fA-F]{40}$') 'receipt: git revision recorded (full SHA)'
Assert-True ($receipt.GitDirty -is [bool]) 'receipt: git dirty state recorded as a boolean'
Assert-Equal 7 @($receipt.Entries).Count 'receipt: one entry per planned file'

$entryDll = @($receipt.Entries | Where-Object Kind -eq 'dll')[0]
Assert-Equal ([IO.Path]::GetFullPath($fx1.Dll)) $entryDll.Source 'receipt: exact absolute source path'
Assert-Equal (Join-Path $game1 'Mods\FullMod.dll') $entryDll.Destination 'receipt: exact absolute destination path'
Assert-Equal (Get-HashOf $fx1.Dll) $entryDll.SourceSha256 'receipt: source SHA256'
Assert-Equal (Get-HashOf (Join-Path $game1 'Mods\FullMod.dll')) $entryDll.DestinationSha256 'receipt: destination SHA256'
$entryGlb = @($receipt.Entries | Where-Object Kind -eq 'glb')[0]
Assert-Equal 'Mods\FullMod\fullmod.glb' $entryGlb.DestinationRelative 'receipt: GLB destination relative path recorded'
$entryJson = @($receipt.Entries | Where-Object Kind -eq 'metadata')[0]
Assert-Equal (Join-Path $game1 'UserData\FullMod\mod.json') $entryJson.Destination 'receipt: metadata destination recorded'

# ------------------------------------------------------- 2. idempotent re-run
$result2 = Invoke-DeployFixture -Fixture $fx1 -GameDir $game1
$receipt2 = Get-Content -LiteralPath $receiptPath -Raw | ConvertFrom-Json
Assert-Equal 0 $result2.Copied 'deploy: second run copies nothing'
Assert-Equal 7 $result2.Unchanged 'deploy: second run reports every file unchanged'
Assert-True ($null -eq $receipt2.BackupId) 'deploy: all-unchanged run creates no backup'

# ----------------------------------------- 3. changed file backup + receipt
[IO.File]::WriteAllText($fx1.Dll, 'dll-FullMod-v2')
$result3 = Invoke-DeployFixture -Fixture $fx1 -GameDir $game1
$receipt3 = Get-Content -LiteralPath $receiptPath -Raw | ConvertFrom-Json
Assert-Equal 1 $result3.Copied 'deploy: only the changed DLL is copied'
Assert-Equal 6 $result3.Unchanged 'deploy: unchanged siblings are not rewritten'
Assert-True ($null -ne $receipt3.BackupId) 'deploy: changed run records a backup id'
$backupFiles = @(Get-ChildItem -LiteralPath $receipt3.BackupDir -File | Where-Object Name -ne 'manifest.json')
Assert-Equal 1 $backupFiles.Count 'backup: only the overwritten target is backed up'
Assert-Equal (Get-HashOf $fx1.Dll) (Get-HashOf (Join-Path $game1 'Mods\FullMod.dll')) 'deploy: updated DLL hash matches source'
$backupManifest = Get-Content -LiteralPath (Join-Path $receipt3.BackupDir 'manifest.json') -Raw | ConvertFrom-Json
Assert-Equal 'Mods\FullMod.dll' @($backupManifest.Entries)[0].DestinationRelative 'backup: manifest records the exact relative target'

# ------------------------------------------------ 4. rollback on copy failure
$game4 = New-FakeInstall 'game-rollback'
$fx4 = New-FakeProject -Name 'RollMod' -Tag 'new'
New-Item -ItemType Directory -Path (Join-Path $game4 'UserData\RollMod') -Force | Out-Null
$oldDll = Join-Path $game4 'Mods\RollMod.dll'
$oldBundle = Join-Path $game4 'Mods\pack.bundle'
[IO.File]::WriteAllText($oldDll, 'old-dll')
[IO.File]::WriteAllText($oldBundle, 'old-bundle')
$oldDllHash = Get-HashOf $oldDll
$oldBundleHash = Get-HashOf $oldBundle

function Copy-S1DeployTargetFile {
    param([string]$Source, [string]$Destination)
    if ($Destination -like '*pack.bundle') { throw 'synthetic copy failure (bundle)' }
    Copy-Item -LiteralPath $Source -Destination $Destination -Force -ErrorAction Stop
}

$rollbackError = $null
try { Invoke-DeployFixture -Fixture $fx4 -GameDir $game4 | Out-Null }
catch { $rollbackError = $_ }
Set-Item -Path Function:\Copy-S1DeployTargetFile -Value $originalCopy

Assert-True ($null -ne $rollbackError) 'rollback: copy failure surfaces as an error'
Assert-True ($rollbackError.Exception.Message -match 'Rollback') 'rollback: error message reports the rollback outcome'
Assert-Equal $oldDllHash (Get-HashOf $oldDll) 'rollback: overwritten DLL restored byte-identically from backup'
Assert-Equal $oldBundleHash (Get-HashOf $oldBundle) 'rollback: overwritten bundle restored byte-identically'
Assert-True (-not (Test-Path -LiteralPath (Join-Path $game4 'Mods\src_icon.png'))) 'rollback: newly created icon removed'
Assert-True (-not (Test-Path -LiteralPath (Join-Path $game4 'Mods\RollMod\rollmod.glb'))) 'rollback: newly created GLB removed'
$failReceiptPath = Join-Path $game4 'UserData\.deploy\receipts\RollMod.json'
$failReceipt = Get-Content -LiteralPath $failReceiptPath -Raw | ConvertFrom-Json
Assert-Equal 'RolledBack' $failReceipt.Status 'rollback: receipt status RolledBack'
Assert-True ($failReceipt.Rollback.Attempted -eq $true -and $failReceipt.Rollback.Restored -ge 2) 'rollback: receipt records attempted restore of touched targets'
Assert-Equal 0 @($failReceipt.Rollback.Failures).Count 'rollback: clean rollback reports no failures'

# ------------------------------------- 5. partial rollback failure reported
$game5 = New-FakeInstall 'game-partial'
$fx5 = New-FakeProject -Name 'PartialMod' -Tag 'new'
New-Item -ItemType Directory -Path (Join-Path $game5 'UserData\PartialMod') -Force | Out-Null
$partialDll = Join-Path $game5 'Mods\PartialMod.dll'
[IO.File]::WriteAllText($partialDll, 'old-partial-dll')
$partialOldHash = Get-HashOf $partialDll

function Copy-S1DeployTargetFile {
    param([string]$Source, [string]$Destination)
    if ($Destination -like '*pack.bundle') { throw 'synthetic copy failure (bundle)' }
    Copy-Item -LiteralPath $Source -Destination $Destination -Force -ErrorAction Stop
}
function Restore-S1DeployTargetFile {
    param([string]$Source, [string]$Destination)
    if ($Destination -like '*PartialMod.dll') { throw 'synthetic restore failure (dll locked)' }
    Copy-Item -LiteralPath $Source -Destination $Destination -Force -ErrorAction Stop
}

$partialError = $null
try { Invoke-DeployFixture -Fixture $fx5 -GameDir $game5 | Out-Null }
catch { $partialError = $_ }
Set-Item -Path Function:\Copy-S1DeployTargetFile -Value $originalCopy
Set-Item -Path Function:\Restore-S1DeployTargetFile -Value $originalRestore

Assert-True ($null -ne $partialError) 'partial rollback: deployment fails'
Assert-True ($partialError.Exception.Message -match 'Partial rollback') 'partial rollback: error says the install is not back to its previous state'
Assert-True ($partialError.Exception.Message -match 'PartialMod.dll') 'partial rollback: failing target path reported'
$partialReceipt = Get-Content -LiteralPath (Join-Path $game5 'UserData\.deploy\receipts\PartialMod.json') -Raw | ConvertFrom-Json
Assert-Equal 'RollbackFailed' $partialReceipt.Status 'partial rollback: receipt status RollbackFailed'
Assert-True (@($partialReceipt.Rollback.Failures).Count -ge 1) 'partial rollback: receipt lists the failed restore'
Assert-True ((Get-HashOf $partialDll) -ne $partialOldHash) 'partial rollback: truthfully leaves the unrestorable file at the new content'
Assert-True ((Test-Path (Join-Path $partialReceipt.BackupDir 'manifest.json'))) 'partial rollback: verified backup retained for manual recovery'

# -------------------------------------- 6. ambiguous duplicate destinations
$game6 = New-FakeInstall 'game-dup'
$fx6 = New-FakeProject -Name 'DupMod' -Tag 'dup' -NoAssets
New-Item -ItemType Directory -Path $fx6.Assets -Force | Out-Null
[IO.File]::WriteAllText((Join-Path $fx6.ProjectDir 'same.png'), 'from-src')
[IO.File]::WriteAllText((Join-Path $fx6.Assets 'same.png'), 'from-assets')
$dupError = $null
try { Invoke-DeployFixture -Fixture $fx6 -GameDir $game6 | Out-Null } catch { $dupError = $_ }
Assert-True ($null -ne $dupError -and $dupError.Exception.Message -match 'Ambiguous duplicate destination') 'plan: same-name icons from project dir and assets dir rejected'
Assert-Equal 0 @(Get-ChildItem -LiteralPath (Join-Path $game6 'Mods') -File).Count 'plan: duplicate rejection copies nothing'

$game6b = New-FakeInstall 'game-dup-case'
$fx6b = New-FakeProject -Name 'DupCase' -Tag 'dup' -NoAssets
New-Item -ItemType Directory -Path $fx6b.Assets -Force | Out-Null
[IO.File]::WriteAllText((Join-Path $fx6b.ProjectDir 'ICON.png'), 'upper')
[IO.File]::WriteAllText((Join-Path $fx6b.Assets 'icon.png'), 'lower')
$dupCaseError = $null
try { Invoke-DeployFixture -Fixture $fx6b -GameDir $game6b | Out-Null } catch { $dupCaseError = $_ }
Assert-True ($null -ne $dupCaseError -and $dupCaseError.Exception.Message -match 'Ambiguous duplicate destination') 'plan: case-insensitive duplicate destinations rejected'

# --------------------------------- 7. directory / reparse destination guards
$game7 = New-FakeInstall 'game-dir-target'
$fx7 = New-FakeProject -Name 'DirMod' -Tag 'dir'
New-Item -ItemType Directory -Path (Join-Path $game7 'Mods\DirMod.dll') -Force | Out-Null
$dirError = $null
try { Invoke-DeployFixture -Fixture $fx7 -GameDir $game7 | Out-Null } catch { $dirError = $_ }
Assert-True ($null -ne $dirError -and $dirError.Exception.Message -match 'exists as a directory') 'safety: directory occupying a file destination rejected'
Assert-True (Test-Path (Join-Path $game7 'Mods\DirMod.dll') -PathType Container) 'safety: offending directory left untouched'
Assert-True (-not (Test-Path (Join-Path $game7 'UserData\DirMod\mod.json'))) 'safety: no other file written after rejection'

$game7b = New-FakeInstall 'game-link-target'
$fx7b = New-FakeProject -Name 'LinkMod' -Tag 'link' -NoAssets
$linkTarget = Join-Path $root 'outside-link-target.txt'
[IO.File]::WriteAllText($linkTarget, 'outside')
$linkCreated = $false
try {
    New-Item -ItemType SymbolicLink -Path (Join-Path $game7b 'Mods\LinkMod.dll') -Target $linkTarget -ErrorAction Stop | Out-Null
    $linkCreated = $true
}
catch { }
if ($linkCreated) {
    $linkError = $null
    try { Invoke-DeployFixture -Fixture $fx7b -GameDir $game7b | Out-Null } catch { $linkError = $_ }
    Assert-True ($null -ne $linkError -and $linkError.Exception.Message -match 'reparse') 'safety: reparse-point destination rejected'
    Assert-Equal 'outside' ([IO.File]::ReadAllText($linkTarget)) 'safety: reparse target was not written through'
}
else {
    Skip-Test 'safety: reparse-point destination rejected' 'symlink creation not permitted on this host'
}

# ------------------------------------------------- 8. backup is not recursive
$game8 = New-FakeInstall 'game-backup-scope'
$fx8 = New-FakeProject -Name 'ScopeMod' -Tag 'scope'
New-Item -ItemType Directory -Path (Join-Path $game8 'UserData\ScopeMod') -Force | Out-Null
$configPath = Join-Path $game8 'UserData\ScopeMod\config.json'
$savePath = Join-Path $game8 'UserData\ScopeMod\save.json'
[IO.File]::WriteAllText($configPath, '{"user":"config"}')
[IO.File]::WriteAllText($savePath, '{"user":"save"}')
$configHash = Get-HashOf $configPath
$saveHash = Get-HashOf $savePath

$firstScope = Invoke-DeployFixture -Fixture $fx8 -GameDir $game8
$firstScopeReceipt = Get-Content -LiteralPath (Join-Path $game8 'UserData\.deploy\receipts\ScopeMod.json') -Raw | ConvertFrom-Json
Assert-True ($null -eq $firstScopeReceipt.BackupDir) 'backup scope: first run with no overwritten targets creates no backup directory'

[IO.File]::WriteAllText($fx8.Dll, 'dll-ScopeMod-v2')
$null = Invoke-DeployFixture -Fixture $fx8 -GameDir $game8
$scopeReceipt = Get-Content -LiteralPath (Join-Path $game8 'UserData\.deploy\receipts\ScopeMod.json') -Raw | ConvertFrom-Json
$backupNames = @(Get-ChildItem -LiteralPath $scopeReceipt.BackupDir -Recurse -File | ForEach-Object Name)
Assert-Equal $configHash (Get-HashOf $configPath) 'backup scope: existing config.json untouched'
Assert-Equal $saveHash (Get-HashOf $savePath) 'backup scope: existing save.json untouched'
Assert-True (-not ($backupNames -contains 'config.json') -and -not ($backupNames -contains 'save.json')) 'backup scope: saves/configs never copied into the backup'
Assert-Equal 0 @(Get-ChildItem -LiteralPath $game8 -Recurse -File -Filter 'save.json' |
        Where-Object FullName -notlike '*UserData\ScopeMod\save.json').Count 'backup scope: no stray save copies in the install'

# ------------------------------------------------------------- 9. mutex gate
$game9 = New-FakeInstall 'game-mutex'
$fx9 = New-FakeProject -Name 'MutexMod' -Tag 'mutex'
$mutexName = Get-S1DeployMutexName -GameDir $game9
$heldMutex = [System.Threading.Mutex]::new($false, $mutexName)
[void]$heldMutex.WaitOne()
try {
    $mutexRun = Invoke-Child -Arguments @('-GameDir', $game9, '-ProjectName', 'MutexMod',
        '-ProjectDir', $fx9.ProjectDir, '-TargetPath', $fx9.Dll, '-PdbPath', $fx9.Pdb,
        '-AssetsDir', $fx9.Assets, '-ModJsonPath', $fx9.ModJson,
        '-Configuration', 'Release', '-MutexTimeoutSeconds', '1')
}
finally {
    $heldMutex.ReleaseMutex()
    $heldMutex.Dispose()
}
Assert-True ($mutexRun.ExitCode -ne 0 -and $mutexRun.Output -match 'mutex') 'mutex: concurrent deploy times out and fails'
Assert-Equal 0 @(Get-ChildItem -LiteralPath (Join-Path $game9 'Mods') -File).Count 'mutex: timeout copies nothing'

# ------------------------------------------------ 10. running-game guard
$game10 = New-FakeInstall 'game-running'
$fx10 = New-FakeProject -Name 'RunMod' -Tag 'run'
function Get-S1GameProcesses { @([pscustomobject]@{ ProcessName = 'Schedule I'; Id = 999 }) }
$runningCode = -2
try { Invoke-DeployFixture -Fixture $fx10 -GameDir $game10 | Out-Null }
catch { $runningCode = Get-CodeFromException $_.Exception }
Set-Item -Path Function:\Get-S1GameProcesses -Value $originalProcessProvider
Assert-Equal 4 $runningCode 'preflight: running game rejected before any write (exit code 4)'
Assert-Equal 0 @(Get-ChildItem -LiteralPath (Join-Path $game10 'Mods') -File).Count 'preflight: running game leaves Mods untouched'

# --------------------------------------------------------- 11. NoDeploy paths
$game11b = New-FakeInstall 'game-nodeploy-env'
$fx11b = New-FakeProject -Name 'NoDeployEnvMod' -Tag 'nd'
$env:S1NoDeploy = 'true'
try { $null = Invoke-DeployFixture -Fixture $fx11b -GameDir $game11b } finally { Remove-Item Env:\S1NoDeploy -ErrorAction SilentlyContinue }
Assert-Equal 0 @(Get-ChildItem -LiteralPath (Join-Path $game11b 'Mods') -File).Count 'NoDeploy: S1NoDeploy=true writes nothing'
Assert-True (-not (Test-Path (Join-Path $game11b 'UserData\.deploy'))) 'NoDeploy: no receipt/backup directories created'

$game11c = New-FakeInstall 'game-nodeploy-switch'
$fx11c = New-FakeProject -Name 'NoDeploySwitchMod' -Tag 'nd'
$null = Invoke-S1ModDeploy -GameDir $game11c -ProjectName $fx11c.Name -ProjectDir $fx11c.ProjectDir `
    -TargetPath $fx11c.Dll -PdbPath $fx11c.Pdb -AssetsDir $fx11c.Assets -ModJsonPath $fx11c.ModJson -NoDeploy
Assert-Equal 0 @(Get-ChildItem -LiteralPath (Join-Path $game11c 'Mods') -File).Count 'NoDeploy: -NoDeploy switch writes nothing'

# ------------------------------------------------ 12. unsafe project name
$game12 = New-FakeInstall 'game-unsafe'
$fx12 = New-FakeProject -Name 'UnsafeMod' -Tag 'unsafe' -NoAssets
$unsafeError = $null
try {
    Invoke-S1ModDeploy -GameDir $game12 -ProjectName '..\Escape' -ProjectDir $fx12.ProjectDir `
        -TargetPath $fx12.Dll -PdbPath $fx12.Pdb -AssetsDir $fx12.Assets -ModJsonPath $fx12.ModJson | Out-Null
}
catch { $unsafeError = $_ }
Assert-True ($null -ne $unsafeError -and $unsafeError.Exception.Message -match 'not a safe') 'safety: path-traversal project name rejected'
Assert-Equal 0 @(Get-ChildItem -LiteralPath $game12 -Recurse -File -Force |
        Where-Object { $_.FullName -like '*Escape*' }).Count 'safety: traversal name wrote nothing'

# ------------------------------------- 13. preflight failure + CLI exit codes
$cliOk = Invoke-Child -Arguments @('-GameDir', $game1, '-ProjectName', 'FullMod',
    '-ProjectDir', $fx1.ProjectDir, '-TargetPath', $fx1.Dll, '-PdbPath', $fx1.Pdb,
    '-AssetsDir', $fx1.Assets, '-ModJsonPath', $fx1.ModJson, '-Configuration', 'Release')
Assert-Equal 0 $cliOk.ExitCode 'CLI: successful deploy exits 0'
Assert-True ($cliOk.Output -match 'Unchanged 7') 'CLI: reports unchanged count on re-run'

$missingGame = Join-Path $root 'no-such-install'
$cliBad = Invoke-Child -Arguments @('-GameDir', $missingGame, '-ProjectName', 'FullMod',
    '-ProjectDir', $fx1.ProjectDir, '-TargetPath', $fx1.Dll, '-Configuration', 'Release')
Assert-Equal 3 $cliBad.ExitCode 'CLI: missing install fails with preflight exit code 3'

# ---------------------------------------- 14. Shared deploy + thirdparty kept
$sharedDir = Join-Path $root 'proj-shared\src'
New-Item -ItemType Directory -Path $sharedDir -Force | Out-Null
$sharedDll = Join-Path $sharedDir 'Shared.dll'
[IO.File]::WriteAllText($sharedDll, 'shared-dll')
$game14 = New-FakeInstall 'game-shared'
$sharedResult = Invoke-S1ModDeploy -GameDir $game14 -ProjectName 'Shared' -ProjectDir $sharedDir `
    -TargetPath $sharedDll -PdbPath $null -AssetsDir (Join-Path $root 'no-assets') `
    -ModJsonPath (Join-Path $root 'no-docs\mod.json') -Configuration Release
Assert-Equal 1 $sharedResult.Copied 'Shared: a DLL-only project deploys exactly one file'
Assert-True (Test-Path (Join-Path $game14 'Mods\Shared.dll')) 'Shared: DLL lands in Mods'
Assert-True (-not (Test-Path (Join-Path $game14 'UserData\Shared'))) 'Shared: no empty metadata folder for a project without assets'

$targetsText = Get-Content -LiteralPath $targetsPath -Raw
Assert-True ($targetsText -match 'DeployThirdParty' -and $targetsText -match 'deploy-thirdparty\.ps1') 'targets: third-party deploy stays a separate target'
Assert-True ($targetsText -notmatch '<Copy') 'targets: no ordinary MSBuild Copy flow remains'
Assert-True ($targetsText -match 'ProjectName&quot; == &quot;Shared' -or $targetsText -match "MSBuildProjectName\)' == 'Shared") 'targets: DeployShared still selected only for Shared'
Assert-True ($targetsText -match "MSBuildProjectName\)' != 'Shared") 'targets: DeployMod excludes Shared so it deploys once'
Assert-True ($targetsText -match 'deploy-mod\.ps1') 'targets: DeployMod routes through deploy-mod.ps1'
Assert-True ($targetsText -match 'UserData\\\.deploy') 'targets: receipt/backup location documented in targets'

# ------------------------------------- 15. real MSBuild import (quoted paths)
$dotnet = (Get-Command dotnet -ErrorAction SilentlyContinue).Source
if ($dotnet) {
    $msModRoot = Join-Path $root 'ms project\MsBuildMod'
    $msProjDir = Join-Path $msModRoot 'src'
    New-Item -ItemType Directory -Path $msProjDir -Force | Out-Null
    New-Item -ItemType Directory -Path (Join-Path $msModRoot 'assets') -Force | Out-Null
    New-Item -ItemType Directory -Path (Join-Path $msModRoot 'docs') -Force | Out-Null
    $msDll = Join-Path $msProjDir 'MsBuildMod.dll'
    [IO.File]::WriteAllText($msDll, 'msbuild-dll')
    [IO.File]::WriteAllText((Join-Path $msProjDir 'MsBuildMod.pdb'), 'msbuild-pdb')
    [IO.File]::WriteAllText((Join-Path $msProjDir 'msbuild_icon.png'), 'msbuild-icon')
    [IO.File]::WriteAllText((Join-Path $msModRoot 'assets\msbuild.bundle'), 'msbuild-bundle')
    [IO.File]::WriteAllText((Join-Path $msModRoot 'assets\msbuild.glb'), 'msbuild-glb')
    [IO.File]::WriteAllText((Join-Path $msModRoot 'docs\mod.json'), '{"name":"MsBuildMod"}')

    $imports = [System.Security.SecurityElement]::Escape($targetsPath)
    $msProj = Join-Path $msProjDir 'MsBuildMod.proj'
    $msProjText = @"
<Project>
  <Import Project="$imports" />
</Project>
"@
    [IO.File]::WriteAllText($msProj, $msProjText)

    $msGame = New-FakeInstall 'game msbuild with spaces'
    & $dotnet msbuild $msProj -t:DeployMod "-p:GameDir=$msGame" "-p:TargetPath=$msDll" "-p:Configuration=Release" -nologo 2>&1 | Out-Null
    $msExit = $LASTEXITCODE
    Assert-Equal 0 $msExit 'MSBuild: DeployMod target succeeds with paths containing spaces'
    Assert-Equal (Get-HashOf $msDll) (Get-HashOf (Join-Path $msGame 'Mods\MsBuildMod.dll')) 'MSBuild: DLL deployed to Mods'
    Assert-Equal (Get-HashOf (Join-Path $msProjDir 'MsBuildMod.pdb')) (Get-HashOf (Join-Path $msGame 'UserData\MsBuildMod\MsBuildMod.pdb')) 'MSBuild: PDB deployed to UserData\<Mod>'
    Assert-Equal (Get-HashOf (Join-Path $msModRoot 'docs\mod.json')) (Get-HashOf (Join-Path $msGame 'UserData\MsBuildMod\mod.json')) 'MSBuild: mod.json deployed to UserData\<Mod>'
    Assert-True (Test-Path (Join-Path $msGame 'Mods\msbuild_icon.png')) 'MSBuild: project-dir icon deployed to Mods'
    Assert-True (Test-Path (Join-Path $msGame 'Mods\msbuild.bundle')) 'MSBuild: bundle deployed to Mods'
    Assert-True (Test-Path (Join-Path $msGame 'Mods\MsBuildMod\msbuild.glb')) 'MSBuild: GLB deployed to Mods\<Mod>'
    Assert-True (Test-Path (Join-Path $msGame 'UserData\.deploy\receipts\MsBuildMod.json')) 'MSBuild: receipt written for the provider build'

    $msGameNoDeploy = New-FakeInstall 'game msbuild nodeploy'
    & $dotnet msbuild $msProj -t:DeployMod "-p:GameDir=$msGameNoDeploy" "-p:TargetPath=$msDll" "-p:S1NoDeploy=true" -nologo 2>&1 | Out-Null
    Assert-Equal 0 $LASTEXITCODE 'MSBuild: S1NoDeploy=true still succeeds'
    Assert-Equal 0 @(Get-ChildItem -LiteralPath (Join-Path $msGameNoDeploy 'Mods') -File).Count 'MSBuild: S1NoDeploy=true copies nothing'
}
else {
    Skip-Test 'MSBuild: DeployMod target end-to-end' 'dotnet not available'
}

# ------------------------------------------- 16. shared mutex prefix scheme
$mutexName16 = Get-S1DeployMutexName -GameDir (Join-Path $root 'any-install')
Assert-True ($mutexName16 -match '^Schedule1Deploy_[0-9a-f]{16}$') "mutex: fixed Schedule1Deploy_ prefix + first 8 SHA256 bytes (got '$mutexName16')"

# --------------------------------- 17. rollback guards (direct unit checks)
function New-RollbackEntry {
    param(
        [string]$Destination,
        [string]$Relative,
        [bool]$ExistedBefore,
        [string]$BackupPath,
        [string]$OriginalSha256,
        [string]$SourceSha256,
        [string]$State = 'Copied'
    )
    return [pscustomobject]@{
        Kind                = 'dll'
        Source              = $null
        SourceSha256        = $SourceSha256
        DestinationRelative = $Relative
        Destination         = $Destination
        ExistedBefore       = $ExistedBefore
        BackupPath          = $BackupPath
        OriginalSha256      = $OriginalSha256
        DestinationSha256   = $null
        Action              = 'Copied'
        State               = $State
    }
}

# 17a: a corrupted backup must be detected BEFORE restoring.
$game17 = New-FakeInstall 'game-backup-corrupt'
$dest17 = Join-Path $game17 'Mods\CorruptMod.dll'
[IO.File]::WriteAllText($dest17, 'new-content')
$backupRoot17 = Join-Path $game17 'UserData\.deploy\backups\attempt-17'
New-Item -ItemType Directory -Path $backupRoot17 -Force | Out-Null
$backup17 = Join-Path $backupRoot17 '000-CorruptMod.dll'
[IO.File]::WriteAllText($backup17, 'tampered-backup')
$original17File = Join-Path $root 'original-17.bin'
[IO.File]::WriteAllText($original17File, 'real-old-content')
$entry17 = New-RollbackEntry -Destination $dest17 -Relative 'Mods\CorruptMod.dll' -ExistedBefore $true `
    -BackupPath $backup17 -OriginalSha256 (Get-HashOf $original17File) -SourceSha256 (Get-HashOf $dest17)
$rb17 = Invoke-S1DeployRollback -Entries @($entry17) -GameRoot $game17 -BackupDir $backupRoot17
Assert-Equal 1 @($rb17.Failures).Count 'rollback corruption: tampered backup reported as a failure'
Assert-Equal 'RollbackFailed' $entry17.State 'rollback corruption: entry state RollbackFailed'
Assert-Equal 'new-content' ([IO.File]::ReadAllText($dest17)) 'rollback corruption: destination not restored from an unverified backup'
Assert-True ($rb17.Failures[0].Error -match 'Backup hash mismatch') 'rollback corruption: error names the backup hash mismatch'

# 17b: a backup outside this run's backup directory must be refused.
$game17b = New-FakeInstall 'game-backup-outside'
$dest17b = Join-Path $game17b 'Mods\OutsideMod.dll'
[IO.File]::WriteAllText($dest17b, 'new-content-b')
$outsideBackup17b = Join-Path $game17b 'Mods\stolen-backup.dll'
[IO.File]::WriteAllText($outsideBackup17b, 'old-content-b')
$realBackupRoot17b = Join-Path $game17b 'UserData\.deploy\backups\attempt-17b'
New-Item -ItemType Directory -Path $realBackupRoot17b -Force | Out-Null
$entry17b = New-RollbackEntry -Destination $dest17b -Relative 'Mods\OutsideMod.dll' -ExistedBefore $true `
    -BackupPath $outsideBackup17b -OriginalSha256 (Get-HashOf $outsideBackup17b) -SourceSha256 (Get-HashOf $dest17b)
$rb17b = Invoke-S1DeployRollback -Entries @($entry17b) -GameRoot $game17b -BackupDir $realBackupRoot17b
Assert-Equal 1 @($rb17b.Failures).Count 'rollback scope: backup outside the run backup directory refused'
Assert-True ($rb17b.Failures[0].Error -match 'backup directory') 'rollback scope: error says the backup is outside the run directory'
Assert-Equal 'new-content-b' ([IO.File]::ReadAllText($dest17b)) 'rollback scope: destination left untouched'

# 17c: reparse point at the destination must not be written through.
$game17c = New-FakeInstall 'game-restore-reparse'
$outside17c = Join-Path $root 'outside-17c.txt'
[IO.File]::WriteAllText($outside17c, 'outside-content')
$dest17c = Join-Path $game17c 'Mods\ReparseMod.dll'
$link17c = $false
try {
    New-Item -ItemType SymbolicLink -Path $dest17c -Target $outside17c -ErrorAction Stop | Out-Null
    $link17c = $true
}
catch { }
if ($link17c) {
    $entry17c = New-RollbackEntry -Destination $dest17c -Relative 'Mods\ReparseMod.dll' -ExistedBefore $false `
        -BackupPath $null -OriginalSha256 $null -SourceSha256 (Get-HashOf $fx1.Dll)
    $rb17c = Invoke-S1DeployRollback -Entries @($entry17c) -GameRoot $game17c -BackupDir $null
    Assert-Equal 1 @($rb17c.Failures).Count 'rollback reparse: reparse destination refused'
    Assert-True ($rb17c.Failures[0].Error -match 'reparse') 'rollback reparse: error names the reparse point'
    Assert-Equal 'outside-content' ([IO.File]::ReadAllText($outside17c)) 'rollback reparse: symlink target not written through'
    Assert-Equal 'RollbackFailed' $entry17c.State 'rollback reparse: entry state RollbackFailed'
}
else {
    Skip-Test 'rollback reparse: reparse destination refused' 'symlink creation not permitted on this host'
}

# 17d: absent new files are not counted as removed.
$game17d = New-FakeInstall 'game-remove-absent'
$entry17d = New-RollbackEntry -Destination (Join-Path $game17d 'Mods\AbsentMod.dll') -Relative 'Mods\AbsentMod.dll' `
    -ExistedBefore $false -BackupPath $null -OriginalSha256 $null -SourceSha256 (Get-HashOf $fx1.Dll)
$rb17d = Invoke-S1DeployRollback -Entries @($entry17d) -GameRoot $game17d -BackupDir $null
Assert-Equal 0 $rb17d.Removed 'rollback removed: absent new file not counted as removed'
Assert-Equal 0 @($rb17d.Failures).Count 'rollback removed: absent new file is not a failure'
Assert-Equal 'NotAttempted' $entry17d.State 'rollback removed: absent new file state NotAttempted'

# 17e: a new destination holding foreign content must not be deleted.
$game17e = New-FakeInstall 'game-remove-foreign'
$dest17e = Join-Path $game17e 'Mods\ForeignMod.dll'
[IO.File]::WriteAllText($dest17e, 'foreign-content')
$entry17e = New-RollbackEntry -Destination $dest17e -Relative 'Mods\ForeignMod.dll' -ExistedBefore $false `
    -BackupPath $null -OriginalSha256 $null -SourceSha256 (Get-HashOf $fx1.Dll)
$rb17e = Invoke-S1DeployRollback -Entries @($entry17e) -GameRoot $game17e -BackupDir $null
Assert-Equal 0 $rb17e.Removed 'rollback removed: foreign content not counted as removed'
Assert-Equal 1 @($rb17e.Failures).Count 'rollback removed: foreign content reported'
Assert-Equal 'foreign-content' ([IO.File]::ReadAllText($dest17e)) 'rollback removed: foreign content left in place'

# ------------------------------- 18. staged copy corruption leaves target intact
$game18 = New-FakeInstall 'game-stage-corrupt'
$fx18 = New-FakeProject -Name 'StageMod' -Tag 'new'
$dest18 = Join-Path $game18 'Mods\StageMod.dll'
[IO.File]::WriteAllText($dest18, 'old-stage-dll')
$stageHash18 = Get-HashOf $dest18
function Copy-S1DeployTargetFile {
    param([string]$Source, [string]$Destination)
    [IO.File]::WriteAllText($Destination, 'corrupt-staged-bytes')
}
$stageError = $null
try { Invoke-DeployFixture -Fixture $fx18 -GameDir $game18 | Out-Null } catch { $stageError = $_ }
Set-Item -Path Function:\Copy-S1DeployTargetFile -Value $originalCopy
Assert-True ($null -ne $stageError -and $stageError.Exception.Message -match 'hash mismatch') 'staging: corrupted staged copy detected before replacement'
Assert-Equal $stageHash18 (Get-HashOf $dest18) 'staging: destination still holds the previous bytes'
Assert-Equal 0 @(Get-ChildItem -LiteralPath (Join-Path $game18 'Mods') -Force -Filter '.s1deploy-tmp-*').Count 'staging: no temp staging files left behind'

# ------------------------------ 19. stale prior-owned icon reported, not deleted
$game19 = New-FakeInstall 'game-stale'
$fx19a = New-FakeProject -Name 'StaleMod' -Tag 'v1'
$null = Invoke-DeployFixture -Fixture $fx19a -GameDir $game19
$staleIconRel = 'Mods\stalemod_icon.png'
$staleSrcIconRel = 'Mods\src_icon.png'
$staleIcon = Join-Path $game19 $staleIconRel
$staleSrcIcon = Join-Path $game19 $staleSrcIconRel
Assert-True ((Test-Path -LiteralPath $staleIcon) -and (Test-Path -LiteralPath $staleSrcIcon)) 'stale: v1 icons deployed'

$fx19b = New-FakeProject -Name 'StaleMod' -Tag 'v2'
Remove-Item -LiteralPath (Join-Path $fx19b.Assets 'stalemod_icon.png') -Force
Remove-Item -LiteralPath (Join-Path $fx19b.ProjectDir 'src_icon.png') -Force
$warnings19 = @()
$result19 = Invoke-S1ModDeploy -GameDir $game19 -ProjectName $fx19b.Name `
    -ProjectDir $fx19b.ProjectDir -TargetPath $fx19b.Dll -PdbPath $fx19b.Pdb `
    -AssetsDir $fx19b.Assets -ModJsonPath $fx19b.ModJson -Configuration Release -WarningVariable warnings19
$receipt19 = Get-Content -LiteralPath (Join-Path $game19 'UserData\.deploy\receipts\StaleMod.json') -Raw | ConvertFrom-Json
$stale19 = @($receipt19.Stale)
Assert-Equal 2 $stale19.Count 'stale: both prior-owned icons reported'
Assert-True (($stale19.DestinationRelative -contains $staleIconRel) -and ($stale19.DestinationRelative -contains $staleSrcIconRel)) 'stale: report names the exact relative paths'
Assert-True ((Test-Path -LiteralPath $staleIcon) -and (Test-Path -LiteralPath $staleSrcIcon)) 'stale: icons left in place (no automatic deletion)'
Assert-True ((@($warnings19) -join ' ') -match 'Stale') 'stale: warning emitted'
Assert-True (-not [string]::IsNullOrWhiteSpace($receipt19.AttemptId)) 'receipt: attempt id recorded'
$attemptReceipts19 = @(Get-ChildItem -LiteralPath (Join-Path $game19 'UserData\.deploy\receipts') -File -Filter "StaleMod.$($receipt19.AttemptId).json")
Assert-True ($attemptReceipts19.Count -eq 1) 'receipt: attempt-specific receipt persisted alongside latest'
Assert-Equal ([IO.Path]::GetFullPath($attemptReceipts19[0].FullName)) ([IO.Path]::GetFullPath($receipt19.AttemptReceiptPath)) 'receipt: attempt receipt path recorded'
$attemptReceipt19 = Get-Content -LiteralPath $receipt19.AttemptReceiptPath -Raw | ConvertFrom-Json
Assert-Equal 'Deployed' $attemptReceipt19.Status 'receipt: attempt receipt is a deploy record'
$null = Invoke-DeployFixture -Fixture $fx19b -GameDir $game19
$repeat19 = Get-Content -LiteralPath (Join-Path $game19 'UserData\.deploy\receipts\StaleMod.json') -Raw | ConvertFrom-Json
Assert-Equal 2 @($repeat19.Stale).Count 'history: stale icons survive an unchanged successful rerun'

# --------------------------- 20. failure receipt states and recomputed hashes
$game20 = New-FakeInstall 'game-fail-states'
$fx20 = New-FakeProject -Name 'FailMod' -Tag 'new'
New-Item -ItemType Directory -Path (Join-Path $game20 'UserData\FailMod') -Force | Out-Null
$old20 = Join-Path $game20 'Mods\FailMod.dll'
[IO.File]::WriteAllText($old20, 'old-fail-dll')
function Copy-S1DeployTargetFile {
    param([string]$Source, [string]$Destination)
    if ($Destination -like '*pack.bundle') { throw 'synthetic copy failure (bundle)' }
    Copy-Item -LiteralPath $Source -Destination $Destination -Force -ErrorAction Stop
}
try { Invoke-DeployFixture -Fixture $fx20 -GameDir $game20 | Out-Null } catch { }
Set-Item -Path Function:\Copy-S1DeployTargetFile -Value $originalCopy
$fr20 = Get-Content -LiteralPath (Join-Path $game20 'UserData\.deploy\receipts\FailMod.json') -Raw | ConvertFrom-Json
$allowedStates = @('Pending', 'Attempted', 'Copied', 'Restored', 'Removed', 'NotAttempted', 'RollbackFailed')
Assert-True (@($fr20.Entries | Where-Object { $allowedStates -notcontains $_.State }).Count -eq 0) 'failure receipt: every entry has a truthful state'
Assert-True ($fr20.Rollback.Attempted -eq $true) 'failure receipt: attempted is true when targets were touched'
$frDll20 = @($fr20.Entries | Where-Object Kind -eq 'dll')[0]
Assert-Equal 'Restored' $frDll20.State 'failure receipt: restored DLL state'
Assert-Equal (Get-HashOf $old20) $frDll20.DestinationSha256 'failure receipt: destination SHA recomputed after rollback'
$frMeta20 = @($fr20.Entries | Where-Object Kind -eq 'metadata')[0]
Assert-True ($null -eq $frMeta20.DestinationSha256) 'failure receipt: absent destination SHA is null'
$frGlb20 = @($fr20.Entries | Where-Object Kind -eq 'glb')[0]
Assert-Equal 'NotAttempted' $frGlb20.State 'failure receipt: never-touched entry NotAttempted'
Assert-True ((Test-Path -LiteralPath $fr20.AttemptReceiptPath)) 'failure receipt: attempt-specific failure receipt persisted'

# ----------------------- 21. ownership history survives a failure + no-op rerun
$game21 = New-FakeInstall 'game-history'
$fx21a = New-FakeProject -Name 'HistoryMod' -Tag 'v1'
$null = Invoke-DeployFixture -Fixture $fx21a -GameDir $game21
$fx21b = New-FakeProject -Name 'HistoryMod' -Tag 'v2'
Remove-Item -LiteralPath (Join-Path $fx21b.Assets 'historymod_icon.png') -Force
Remove-Item -LiteralPath (Join-Path $fx21b.ProjectDir 'src_icon.png') -Force
function Copy-S1DeployTargetFile {
    param([string]$Source, [string]$Destination)
    if ($Destination -like '*pack.bundle') { throw 'synthetic copy failure (bundle)' }
    Copy-Item -LiteralPath $Source -Destination $Destination -Force -ErrorAction Stop
}
try { Invoke-DeployFixture -Fixture $fx21b -GameDir $game21 | Out-Null } catch { }
Set-Item -Path Function:\Copy-S1DeployTargetFile -Value $originalCopy
$warnings21 = @()
$null = Invoke-S1ModDeploy -GameDir $game21 -ProjectName $fx21b.Name `
    -ProjectDir $fx21b.ProjectDir -TargetPath $fx21b.Dll -PdbPath $fx21b.Pdb `
    -AssetsDir $fx21b.Assets -ModJsonPath $fx21b.ModJson -Configuration Release -WarningVariable warnings21
$receipt21 = Get-Content -LiteralPath (Join-Path $game21 'UserData\.deploy\receipts\HistoryMod.json') -Raw | ConvertFrom-Json
Assert-Equal 2 @($receipt21.Stale).Count 'history: stale icons still reported after an intervening failed run'
Assert-True ((@($warnings21) -join ' ') -match 'Stale') 'history: stale warning still emitted after a failed run'

# --------------------------- 22. receipt I/O failure rolls back, stale note
$game22 = New-FakeInstall 'game-receipt-io'
$fx22 = New-FakeProject -Name 'ReceiptMod' -Tag 'v1'
$null = Invoke-DeployFixture -Fixture $fx22 -GameDir $game22
$dest22 = Join-Path $game22 'Mods\ReceiptMod.dll'
$oldHash22 = Get-HashOf $dest22
[IO.File]::WriteAllText($fx22.Dll, 'dll-ReceiptMod-v2')
$script:origWriteJson = (Get-Command Write-S1JsonAtomic).ScriptBlock
function Write-S1JsonAtomic {
    param([string]$Path, $Object)
    if ($Path -like '*\receipts\*') { throw 'synthetic receipt IO failure' }
    & $script:origWriteJson -Path $Path -Object $Object
}
$receiptIoError = $null
try { Invoke-DeployFixture -Fixture $fx22 -GameDir $game22 | Out-Null } catch { $receiptIoError = $_ }
Set-Item -Path Function:\Write-S1JsonAtomic -Value $script:origWriteJson
Assert-True ($null -ne $receiptIoError) 'receipt I/O: failure surfaces'
Assert-True ($receiptIoError.Exception.Message -match 'receipt') 'receipt I/O: error names the receipt'
Assert-True ($receiptIoError.Exception.Message -match 'historical') 'receipt I/O: stale previous receipt called out as historical'
Assert-Equal $oldHash22 (Get-HashOf $dest22) 'receipt I/O: new code not retained after the failure (DLL rolled back)'

# ---------------------------------------------------------------- summary
Write-Host ''
Write-Host "Passed: $script:Passed | Failed: $script:Failed | Skipped: $script:Skipped" -ForegroundColor Cyan
if ($script:Failed -gt 0) { exit 1 }
exit 0
