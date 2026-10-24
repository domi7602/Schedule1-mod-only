#Requires -Version 7
# Synthetic filesystem fixtures only. No real WorldLook cache or backup is mutated.
[CmdletBinding()]
param([string]$ScratchRoot)

$ErrorActionPreference = 'Stop'
$repo = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
$tool = Join-Path $repo 'Tools/clean-unity-caches.ps1'
if (-not $ScratchRoot) { $ScratchRoot = Join-Path $env:USERPROFILE '.cache/s1-cleanup' }
$scratch = [IO.Path]::GetFullPath($ScratchRoot).TrimEnd('\', '/')
$repoFull = [IO.Path]::GetFullPath($repo).TrimEnd('\', '/')
if ($scratch -eq $repoFull -or $scratch.StartsWith($repoFull + [IO.Path]::DirectorySeparatorChar, [StringComparison]::OrdinalIgnoreCase)) {
    throw 'Cleanup fixtures must live outside the repository.'
}
$scratch = Join-Path $scratch ('uc-' + [guid]::NewGuid().ToString('N').Substring(0, 10))
$null = New-Item -ItemType Directory -Path $scratch -Force
$script:passed = 0
$script:failed = 0
$run = 'Source/Mods/WorldLook/build/logs/20261022-174444-897-367f1db0/unity/Library'
$preview = 'Source/Mods/WorldLook/build/logs/p-3dc9460c/Library'

function Check([bool]$Condition, [string]$Message) {
    if ($Condition) { $script:passed++; Write-Host "PASS: $Message" }
    else { $script:failed++; Write-Host "FAIL: $Message" -ForegroundColor Red }
}

function Write-File([string]$Root, [string]$Path, [string]$Content = 'fixture') {
    $full = Join-Path $Root $Path
    $null = New-Item -ItemType Directory -Path (Split-Path $full -Parent) -Force
    [IO.File]::WriteAllText($full, $Content)
}

function New-Fixture([string]$Name) {
    $root = Join-Path $scratch $Name
    Write-File $root 'AGENTS.md'
    Write-File $root 'Source/Mods/WorldLook/build/Build.ps1'
    Write-File $root 'Source/Mods/WorldLook/src/WorldLook.csproj'
    Write-File $root 'Source/Mods/WorldLook/src/code.cs' 'must stay'
    $null = New-Item -ItemType Directory -Path (Join-Path $root 'Tools')
    Copy-Item -LiteralPath $tool -Destination (Join-Path $root 'Tools/clean-unity-caches.ps1')
    return $root
}

function New-Caches([string]$Root, [string]$Library) {
    foreach ($name in @('Artifacts', 'BurstCache', 'PackageCache', 'ShaderCache', 'TempArtifacts')) {
        Write-File $Root "$Library/$name/nested/cache.bin" '1234567'
    }
    foreach ($name in @('ArtifactDB', 'ArtifactDB-lock', 'SourceAssetDB', 'SourceAssetDB-lock', 'ShaderCache.db')) {
        Write-File $Root "$Library/$name" '123'
    }
}

function Expect-Failure([scriptblock]$Action, [string]$Pattern, [string]$Message) {
    $text = ''
    try { $null = & $Action }
    catch { $text = $_.Exception.Message }
    Check ($text -match $Pattern) "$Message ($text)"
}

function Latest-Receipt([string]$Root) {
    $path = Get-ChildItem -LiteralPath (Join-Path $Root '.scratch/unity-cache-cleanup') -Recurse -Filter receipt.json | Sort-Object FullName | Select-Object -Last 1
    Get-Content -LiteralPath $path.FullName -Raw | ConvertFrom-Json
}

Write-Host "Synthetic cleanup fixtures: $scratch"
$fx = New-Fixture 'allowlist'
New-Caches $fx $run
New-Caches $fx $preview
$protected = @(
    "$run/Bee/build.traceevents", "$run/BuildPlayerData/report.json", "$run/ScriptAssemblies/Assembly.dll",
    "$run/APIUpdater/report.txt", "$run/PackageManager/cache.dat", "$run/BuildReport.json", "$run/root.traceevents",
    'Source/Mods/WorldLook/unity/Library/Artifacts/current.bin',
    'Source/Mods/WorldLook/build/logs/not-a-run/Library/Artifacts/keep.bin',
    'Source/Mods/WorldLook/build/logs/20261399-174444-897-367f1db0/unity/Library/Artifacts/keep.bin',
    'Source/Mods/WorldLook/build/logs/20261022-174444-897/unity/Library/Artifacts/keep.bin',
    'Source/Mods/WorldLook/build/logs/20261022-174444-897-367f1db0/run.json',
    '.backups/WorldLook/saved/Library/Artifacts/backup.bin', '.scratch/prior-run/receipt.json'
)
foreach ($path in $protected) { Write-File $fx $path 'evidence remains exactly' }
$protectedHashes = @{}
foreach ($path in $protected) { $protectedHashes[$path] = (Get-FileHash -LiteralPath (Join-Path $fx $path)).Hash }
. (Join-Path $fx 'Tools/clean-unity-caches.ps1')
function Get-S1CacheActiveProcesses { @() }
$result = Invoke-S1UnityCacheCleanup
Check ($result.Status -eq 'Preview' -and $result.Targets.Count -eq 20) 'default preview recognizes only both supported run layouts'
Check ($result.Files -eq 20 -and $result.Bytes -eq 100) 'preview reports exact file count and byte sum'
Check ((Test-Path -LiteralPath (Join-Path $fx "$run/Artifacts/nested/cache.bin")) -and -not (Test-Path -LiteralPath (Join-Path $fx '.scratch/unity-cache-cleanup'))) 'preview neither deletes nor writes receipts'
$whatIf = Invoke-S1UnityCacheCleanup -Apply -WhatIf
Check ($whatIf.Status -eq 'NotApplied' -and -not (Test-Path -LiteralPath (Join-Path $fx '.scratch/unity-cache-cleanup'))) 'ShouldProcess WhatIf performs no writes'
$applied = Invoke-S1UnityCacheCleanup -Apply -Confirm:$false
Check ($applied.Status -eq 'Completed' -and -not (Test-Path -LiteralPath (Join-Path $fx "$run/Artifacts"))) 'Apply removes allowlisted cache roots'
Check (@($result.Targets | Where-Object { Test-Path -LiteralPath $_.Path }).Count -eq 0) 'all twenty planned cache entries removed'
Check ((Test-Path -LiteralPath (Join-Path $fx $run)) -and (Test-Path -LiteralPath (Join-Path $fx $preview))) 'Library and run roots are retained'
$unchanged = $true
foreach ($path in $protected) {
    if (-not (Test-Path -LiteralPath (Join-Path $fx $path)) -or (Get-FileHash -LiteralPath (Join-Path $fx $path)).Hash -ne $protectedHashes[$path]) { $unchanged = $false }
}
Check $unchanged 'current Library, root files, reports, traces, backups and unknown runs are byte-identical'
$receipt = Latest-Receipt $fx
Check ($receipt.Status -eq 'Completed' -and $receipt.ProtectedCheck -eq 'Passed' -and $receipt.ProtectedDifferences.Count -eq 0) 'receipt confirms protected-file hashes'
Check ($receipt.CompletedTargetBytes -eq 100 -and $receipt.CompletedTargetFiles -eq 20) 'receipt records only successfully completed targets'
$again = Invoke-S1UnityCacheCleanup -Apply -Confirm:$false
Check ($again.Status -eq 'NoChanges' -and $again.Targets.Count -eq 0) 'missing caches are an idempotent no-op'

$fx = New-Fixture 'active'
New-Caches $fx $run
. (Join-Path $fx 'Tools/clean-unity-caches.ps1')
function Get-S1CacheActiveProcesses { [pscustomobject]@{ ProcessName = 'Unity' } }
Expect-Failure { Invoke-S1UnityCacheCleanup -Apply -Confirm:$false } 'Active Unity/game' 'active Unity blocks before any mutation'
Check ((Test-Path -LiteralPath (Join-Path $fx "$run/Artifacts")) -and -not (Test-Path -LiteralPath (Join-Path $fx '.scratch'))) 'active-process block keeps every cache and creates no receipt'

$fx = New-Fixture 'wrong-type'
Write-File $fx "$run/Artifacts" 'not a directory'
. (Join-Path $fx 'Tools/clean-unity-caches.ps1')
function Get-S1CacheActiveProcesses { @() }
Expect-Failure { Invoke-S1UnityCacheCleanup -Apply -Confirm:$false } 'Unexpected cache entry type' 'cache file/directory type mismatch fails closed'
Check (Test-Path -LiteralPath (Join-Path $fx "$run/Artifacts")) 'type mismatch is not deleted'

foreach ($reportName in @('unity.log', 'last.buildreport', 'build.traceevents', 'crash.dmp', 'editor.crash', 'shader-build.json', 'build-info.json', 'preview.json')) {
    $fx = New-Fixture ('evidence-' + $reportName.Replace('.', '-'))
    New-Caches $fx $run
    Write-File $fx "$preview/Artifacts/nested/$reportName" 'diagnostic evidence'
    . (Join-Path $fx 'Tools/clean-unity-caches.ps1')
    function Get-S1CacheActiveProcesses { @() }
    Expect-Failure { Invoke-S1UnityCacheCleanup -Apply -Confirm:$false } 'Diagnostic/report file inside cache' "candidate evidence $reportName aborts complete preflight"
    Check ((Test-Path -LiteralPath (Join-Path $fx "$run/Artifacts/nested/cache.bin")) -and
        (Test-Path -LiteralPath (Join-Path $fx "$preview/Artifacts/nested/$reportName")) -and
        -not (Test-Path -LiteralPath (Join-Path $fx '.scratch'))) "evidence $reportName and the earlier ordinary cache are retained"
}

$fx = New-Fixture 'partial'
New-Caches $fx $run
. (Join-Path $fx 'Tools/clean-unity-caches.ps1')
function Get-S1CacheActiveProcesses { @() }
$script:realRemove = (Get-Command Remove-S1CacheEntry).ScriptBlock
$script:removeCalls = 0
function Remove-S1CacheEntry {
    param($Root, $Path, $Directory)
    $script:removeCalls++
    if ($script:removeCalls -eq 2) { throw 'synthetic locked cache entry' }
    & $script:realRemove $Root $Path $Directory
}
Expect-Failure { Invoke-S1UnityCacheCleanup -Apply -Confirm:$false } 'partial removal is possible, no rollback' 'mid-target failure is explicitly partial without a rollback claim'
$receipt = Latest-Receipt $fx
Check ($receipt.Status -eq 'Failed' -and $receipt.Targets[0].Status -eq 'FailedPartialPossible') 'failure receipt retains target progress'
Check ($receipt.CompletedTargetBytes -eq 0 -and $receipt.ProtectedCheck -eq 'Passed') 'partial target is excluded from completed totals and evidence still checked'
Check ((Test-Path -LiteralPath (Join-Path $fx "$run/Artifacts")) -and -not (Test-Path -LiteralPath (Join-Path $fx "$run/Artifacts/nested/cache.bin")) -and (Test-Path -LiteralPath (Join-Path $fx "$run/BurstCache"))) 'partial fixture distinguishes actual removal from untouched later targets'

$fx = New-Fixture 'protected-change'
New-Caches $fx $run
. (Join-Path $fx 'Tools/clean-unity-caches.ps1')
function Get-S1CacheActiveProcesses { @() }
$script:realRemove = (Get-Command Remove-S1CacheEntry).ScriptBlock
$script:changed = $false
function Remove-S1CacheEntry {
    param($Root, $Path, $Directory)
    & $script:realRemove $Root $Path $Directory
    if (-not $script:changed) { [IO.File]::WriteAllText((Join-Path $Root 'Source/Mods/WorldLook/src/code.cs'), 'concurrent edit'); $script:changed = $true }
}
Expect-Failure { Invoke-S1UnityCacheCleanup -Apply -Confirm:$false } 'preservation mismatch' 'protected-file change prevents success'
$receipt = Latest-Receipt $fx
Check ($receipt.ProtectedCheck -eq 'Mismatch' -and $receipt.ProtectedDifferences.Count -eq 1) 'receipt identifies a changed protected file'

$fx = New-Fixture 'plan-changed'
New-Caches $fx $run
. (Join-Path $fx 'Tools/clean-unity-caches.ps1')
function Get-S1CacheActiveProcesses { @() }
$script:realSnapshot = (Get-Command Get-S1CacheProtectedSnapshot).ScriptBlock
function Get-S1CacheProtectedSnapshot {
    param($Root, $Targets)
    & $script:realSnapshot $Root $Targets
    [IO.File]::WriteAllText((Join-Path $Targets[0].Path 'new-cache.bin'), 'new input during preflight')
}
Expect-Failure { Invoke-S1UnityCacheCleanup -Apply -Confirm:$false } 'changed during preflight; nothing removed' 'plan changes while protected files are hashed fail before deletion'
Check ((Test-Path -LiteralPath (Join-Path $fx "$run/Artifacts/nested/cache.bin")) -and -not (Test-Path -LiteralPath (Join-Path $fx '.scratch'))) 'changed-plan failure creates no receipt or cleanup mutation'

$fx = New-Fixture 'process-started'
New-Caches $fx $run
. (Join-Path $fx 'Tools/clean-unity-caches.ps1')
$script:processChecks = 0
function Get-S1CacheActiveProcesses {
    $script:processChecks++
    if ($script:processChecks -ge 3) { [pscustomobject]@{ ProcessName = 'UnityShaderCompiler' } }
}
Expect-Failure { Invoke-S1UnityCacheCleanup -Apply -Confirm:$false } 'Active Unity/game' 'new Unity process is checked again before removal'
$receipt = Latest-Receipt $fx
Check ($receipt.Status -eq 'Failed' -and $receipt.CompletedTargetFiles -eq 0 -and $receipt.ProtectedCheck -eq 'Passed') 'late process block records zero completed removals and retained evidence'

if ($IsWindows) {
    $fx = New-Fixture 'reparse'
    New-Caches $fx $run
    Write-File $fx 'outside/keep.txt' 'outside evidence'
    $junction = Join-Path $fx "$preview/Artifacts"
    $null = New-Item -ItemType Directory -Path (Split-Path $junction -Parent) -Force
    $null = New-Item -ItemType Junction -Path $junction -Target (Join-Path $fx 'outside')
    . (Join-Path $fx 'Tools/clean-unity-caches.ps1')
    function Get-S1CacheActiveProcesses { @() }
    Expect-Failure { Invoke-S1UnityCacheCleanup -Apply -Confirm:$false } 'Reparse' 'all candidates preflight before the first deletion'
    Check ((Test-Path -LiteralPath (Join-Path $fx "$run/Artifacts/nested/cache.bin")) -and (Test-Path -LiteralPath (Join-Path $fx 'outside/keep.txt'))) 'later target junction cannot delete first target or outside contents'
    [IO.Directory]::Delete($junction, $false)

    $fx = New-Fixture 'nested-reparse'
    New-Caches $fx $run
    Write-File $fx 'outside/keep.txt'
    $junction = Join-Path $fx "$run/Artifacts/nested/link"
    $null = New-Item -ItemType Junction -Path $junction -Target (Join-Path $fx 'outside')
    . (Join-Path $fx 'Tools/clean-unity-caches.ps1')
    function Get-S1CacheActiveProcesses { @() }
    Expect-Failure { Invoke-S1UnityCacheCleanup -Apply -Confirm:$false } 'Reparse child' 'nested cache junction is rejected before traversal'
    Check (Test-Path -LiteralPath (Join-Path $fx 'outside/keep.txt')) 'nested link target remains untouched'
    [IO.Directory]::Delete($junction, $false)

    $fx = New-Fixture 'receipt-reparse'
    New-Caches $fx $run
    $null = New-Item -ItemType Directory -Path (Join-Path $fx 'outside')
    $junction = Join-Path $fx '.scratch'
    $null = New-Item -ItemType Junction -Path $junction -Target (Join-Path $fx 'outside')
    . (Join-Path $fx 'Tools/clean-unity-caches.ps1')
    function Get-S1CacheActiveProcesses { @() }
    Expect-Failure { Invoke-S1UnityCacheCleanup -Apply -Confirm:$false } 'Reparse path' 'receipt path cannot redirect writes through a junction'
    Check ((Test-Path -LiteralPath (Join-Path $fx "$run/Artifacts")) -and @(Get-ChildItem -LiteralPath (Join-Path $fx 'outside')).Count -eq 0) 'receipt-path refusal makes no deletion or redirected write'
    [IO.Directory]::Delete($junction, $false)

    $fx = New-Fixture 'protected-reparse'
    New-Caches $fx $run
    Write-File $fx 'outside/keep.txt'
    $junction = Join-Path $fx 'Source/Mods/WorldLook/unity'
    $null = New-Item -ItemType Junction -Path $junction -Target (Join-Path $fx 'outside')
    . (Join-Path $fx 'Tools/clean-unity-caches.ps1')
    function Get-S1CacheActiveProcesses { @() }
    Expect-Failure { Invoke-S1UnityCacheCleanup -Apply -Confirm:$false } 'Reparse child' 'protected-tree junction is rejected before any cleanup'
    Check ((Test-Path -LiteralPath (Join-Path $fx "$run/Artifacts")) -and (Test-Path -LiteralPath (Join-Path $fx 'outside/keep.txt'))) 'protected-tree preflight prevents partial cleanup'
    [IO.Directory]::Delete($junction, $false)
}
else { Write-Host 'Windows junction fixtures are not applicable on this platform.' }

$fx = New-Fixture 'missing-marker'
Remove-Item -LiteralPath (Join-Path $fx 'Source/Mods/WorldLook/src/WorldLook.csproj')
New-Caches $fx $run
. (Join-Path $fx 'Tools/clean-unity-caches.ps1')
function Get-S1CacheActiveProcesses { @() }
Expect-Failure { Invoke-S1UnityCacheCleanup -Apply -Confirm:$false } 'does not exist|Cannot find path' 'missing repository marker refuses arbitrary copied directory'
Check (Test-Path -LiteralPath (Join-Path $fx "$run/Artifacts")) 'invalid root retains its contents'
Expect-Failure { Assert-S1CachePlainPath $fx (Join-Path $scratch 'allowlist/AGENTS.md') } 'outside the repository' 'containment rejects sibling-prefix paths'

$cliOutput = & pwsh -NoProfile -NonInteractive -ExecutionPolicy Bypass -File (Join-Path $fx 'Tools/clean-unity-caches.ps1') -Root $scratch 2>&1 | Out-String
Check ($LASTEXITCODE -ne 0 -and $cliOutput -match 'parameter.*Root') 'CLI exposes no arbitrary deletion-root parameter'

Write-Host ("Cleanup fixtures: {0} passed, {1} failed. Retained at {2}" -f $script:passed, $script:failed, $scratch)
if ($script:failed) { exit 1 }
exit 0
