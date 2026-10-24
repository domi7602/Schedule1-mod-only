#Requires -Version 7
[CmdletBinding()]
param([string]$ScratchRoot)

$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
$tool = Join-Path $repoRoot 'Tools/verify-clean-checkout.ps1'
if (-not $ScratchRoot) { $ScratchRoot = Join-Path $env:LOCALAPPDATA 'hermes/cache/scratch' }
$scratch = [IO.Path]::GetFullPath($ScratchRoot)
$sourceFull = [IO.Path]::GetFullPath($repoRoot).TrimEnd('\', '/')
if ($scratch -eq $sourceFull -or $scratch.StartsWith($sourceFull + [IO.Path]::DirectorySeparatorChar, [StringComparison]::OrdinalIgnoreCase)) {
    throw 'Fixture scratch must be outside the repository.'
}
# Keep room for the helper's own unique checkout directory when this suite is
# nested under verify/tool-fixtures. Git's -C still has a Windows path-length
# limit even with core.longpaths enabled for object/file operations.
$scratch = Join-Path $scratch ('cct-' + [guid]::NewGuid().ToString('N').Substring(0, 12))
$null = New-Item -ItemType Directory -Path $scratch -Force
$script:passed = 0
$script:failed = 0

function Check([bool]$Condition, [string]$Message) {
    if ($Condition) { $script:passed++ }
    else { $script:failed++; Write-Host "FAIL: $Message" }
}

function Write-File([string]$Root, [string]$Path, [string]$Text) {
    $full = Join-Path $Root $Path
    $null = New-Item -ItemType Directory -Path (Split-Path $full -Parent) -Force
    [IO.File]::WriteAllText($full, $Text, [Text.UTF8Encoding]::new($false))
}

function Git([string]$Root, [string[]]$Arguments) {
    $gitExecutable = (Get-Command git -CommandType Application -ErrorAction Stop).Source
    $result = & $gitExecutable -c core.longpaths=true -C $Root @Arguments 2>&1 | Out-String
    if ($LASTEXITCODE -ne 0) { throw $result }
    return $result.Trim()
}

$fakeVerifier = @'
param([string]$Mode, [string]$ScratchRoot, [string]$DotnetPath)
$root = Split-Path $PSScriptRoot -Parent
if ((Get-Content -Raw -LiteralPath (Join-Path $root 'tracked.txt')) -ne 'working content') { exit 31 }
if (-not (Test-Path -LiteralPath (Join-Path $root 'new ünicode file.txt'))) { exit 32 }
if (Test-Path -LiteralPath (Join-Path $root 'deleted.txt')) { exit 33 }
if (Test-Path -LiteralPath (Join-Path $root 'local.build.props')) { exit 34 }
if (([IO.File]::ReadAllBytes((Join-Path $root 'data.bin')) -join ',') -ne '0,255,17,0') { exit 35 }
if ((Get-Content -Raw -LiteralPath (Join-Path $root 'staged.txt')) -ne 'staged new file') { exit 36 }
if ($env:S1_SNAPSHOT_FIXTURE_MUTATE -eq 'true') { Set-Content -LiteralPath (Join-Path $root 'tracked.txt') -Value 'changed by verifier' }
if ($env:S1_SNAPSHOT_FIXTURE_FAIL -eq 'true') { exit 23 }
Write-Host "fixture verify: $Mode"
exit 0
'@

function New-Fixture([string]$Name) {
    $root = Join-Path $scratch $Name
    $null = New-Item -ItemType Directory -Path (Join-Path $root 'Tools') -Force
    $null = Git $root @('init', '-q')
    $null = Git $root @('config', 'user.name', 'Snapshot Fixture')
    $null = Git $root @('config', 'user.email', 'fixture@example.invalid')
    $null = Git $root @('config', 'core.autocrlf', 'false')
    $null = Git $root @('config', 'core.hooksPath', 'no-hooks')
    Copy-Item -LiteralPath $tool -Destination (Join-Path $root 'Tools/verify-clean-checkout.ps1')
    Write-File $root 'Tools/verify.ps1' $fakeVerifier
    Write-File $root '.gitignore' "local.build.props`n"
    Write-File $root 'tracked.txt' 'committed content'
    Write-File $root 'deleted.txt' 'deleted after commit'
    [IO.File]::WriteAllBytes((Join-Path $root 'data.bin'), [byte[]](1, 2, 3))
    $null = Git $root @('add', '.')
    $null = Git $root @('commit', '-qm', 'Synthetic fixture baseline')
    Write-File $root 'tracked.txt' 'staged content'
    $null = Git $root @('add', 'tracked.txt')
    Write-File $root 'tracked.txt' 'working content'
    Remove-Item -LiteralPath (Join-Path $root 'deleted.txt')
    Write-File $root 'new ünicode file.txt' 'untracked is part of this candidate'
    Write-File $root 'staged.txt' 'staged new file'
    $null = Git $root @('add', 'staged.txt')
    Write-File $root 'local.build.props' 'must not be copied'
    [IO.File]::WriteAllBytes((Join-Path $root 'data.bin'), [byte[]](0, 255, 17, 0))
    return $root
}

function Run-Fixture([string]$Root, [string[]]$Extra = @(),
    [ValidateSet('Explicit', 'Profile', 'Local')][string]$ScratchLocation = 'Explicit') {
    $savedProfile = $env:USERPROFILE
    $savedLocal = $env:LOCALAPPDATA
    try {
        $profile = Join-Path $scratch ('p-' + (Split-Path $Root -Leaf))
        $local = Join-Path $scratch ('l-' + (Split-Path $Root -Leaf))
        $env:USERPROFILE = if ($ScratchLocation -eq 'Local') { $null } else { $profile }
        $env:LOCALAPPDATA = $local
        $outRoot = switch ($ScratchLocation) {
            'Explicit' { Join-Path $scratch ((Split-Path $Root -Leaf) + '-runs') }
            'Profile' { Join-Path $profile '.cache/Schedule1Tools/clean-checkouts' }
            'Local' { Join-Path $local 'Schedule1Tools/clean-checkouts' }
        }
        $arguments = @('-NoProfile', '-NonInteractive', '-ExecutionPolicy', 'Bypass', '-File', (Join-Path $Root 'Tools/verify-clean-checkout.ps1')) + $Extra
        if ($ScratchLocation -eq 'Explicit') { $arguments += @('-ScratchRoot', $outRoot) }
        $output = & pwsh @arguments 2>&1 | Out-String
        $code = $LASTEXITCODE
        $manifestFiles = @(Get-ChildItem -LiteralPath $outRoot -Recurse -Filter 'snapshot.json' -ErrorAction SilentlyContinue)
        $manifest = if ($manifestFiles.Count -gt 0) { Get-Content -Raw -LiteralPath $manifestFiles[-1].FullName | ConvertFrom-Json } else { $null }
        return [pscustomobject]@{ code = $code; output = $output; manifest = $manifest; artifacts = $outRoot; profile = $profile; local = $local }
    }
    finally {
        $env:USERPROFILE = $savedProfile
        $env:LOCALAPPDATA = $savedLocal
    }
}

$root = New-Fixture 'current-files'
$statusBefore = Git $root @('status', '--porcelain', '--untracked-files=all')
$headBefore = Git $root @('rev-parse', 'HEAD')
$indexBefore = (Get-FileHash -LiteralPath (Join-Path $root '.git/index')).Hash
$result = Run-Fixture $root
Check ($result.code -eq 0) "working-tree snapshot verifies: $($result.output)"
Check ($result.manifest.status -eq 'Passed') 'manifest records observed success'
Check ($result.manifest.verificationExitCode -eq 0) 'manifest records verifier exit code'
Check ($result.manifest.sourceDigest -match '^[a-f0-9]{64}$') 'candidate has SHA256 identity'
Check ($result.manifest.head -eq $headBefore) 'manifest records original HEAD'
Check (($result.manifest.files.path -contains 'new ünicode file.txt')) 'untracked Unicode/space filename captured'
Check (($result.manifest.files.path -contains 'staged.txt')) 'staged new file captured'
Check (($result.manifest.files.path -notcontains 'deleted.txt')) 'working-tree deletion captured'
Check (($result.manifest.files.path -notcontains 'local.build.props')) 'ignored machine state excluded'
Check ((Git $root @('status', '--porcelain', '--untracked-files=all')) -eq $statusBefore) 'source working state unchanged'
Check ((Get-FileHash -LiteralPath (Join-Path $root '.git/index')).Hash -eq $indexBefore) 'source index unchanged'
Check ((Git $root @('rev-parse', 'HEAD')) -eq $headBefore) 'source HEAD unchanged'
Check ((Get-Content -Raw -LiteralPath (Join-Path $result.manifest.checkout 'tracked.txt')) -eq 'working content') 'working contents win over staged and committed contents'
Check (-not (Test-Path -LiteralPath (Join-Path $result.profile '.cache/Schedule1Tools/clean-checkouts')) -and -not (Test-Path -LiteralPath (Join-Path $result.local 'Schedule1Tools/clean-checkouts'))) 'explicit snapshot scratch leaves both default destinations untouched'

$root = New-Fixture 'dp'
$result = Run-Fixture $root -ScratchLocation Profile
Check ($result.code -eq 0 -and $result.manifest.status -eq 'Passed' -and (Test-Path -LiteralPath $result.artifacts)) "default snapshot verifies under USERPROFILE/.cache/Schedule1Tools/clean-checkouts: $($result.output)"
Check (-not (Test-Path -LiteralPath (Join-Path $result.local 'Schedule1Tools/clean-checkouts'))) 'snapshot USERPROFILE default never writes available LOCALAPPDATA'
$root = New-Fixture 'dl'
$result = Run-Fixture $root -ScratchLocation Local
Check ($result.code -eq 0 -and $result.manifest.status -eq 'Passed' -and (Test-Path -LiteralPath $result.artifacts)) "missing USERPROFILE uses LOCALAPPDATA/Schedule1Tools/clean-checkouts: $($result.output)"

$root = New-Fixture 'staged-executable-mode'
$null = Git $root @('update-index', '--chmod=+x', 'tracked.txt')
$indexBefore = (Get-FileHash -LiteralPath (Join-Path $root '.git/index')).Hash
$result = Run-Fixture $root
Check ($result.code -eq 0 -and $result.manifest.status -eq 'Passed') "staged mode snapshot verifies: $($result.output)"
Check (($result.manifest.files | Where-Object path -eq 'tracked.txt').mode -eq '100755') 'manifest and candidate identity include executable mode'
Check ((Git $result.manifest.checkout @('ls-files', '--stage', '--', 'tracked.txt')) -match '^100755 ') 'checkout preserves staged executable mode instead of HEAD mode'
Check ((Get-FileHash -LiteralPath (Join-Path $root '.git/index')).Hash -eq $indexBefore) 'mode reconstruction never changes the source index'

$root = New-Fixture 'forced-ignored-file'
Write-File $root '.gitignore' "local.build.props`nforced-ignored.txt`n"
Write-File $root 'forced-ignored.txt' 'explicitly tracked despite ignore rule'
$null = Git $root @('add', '--force', 'forced-ignored.txt')
$result = Run-Fixture $root
Check ($result.code -eq 0 -and $result.manifest.status -eq 'Passed') "new forced ignored file verifies: $($result.output)"
Check ((Git $result.manifest.checkout @('ls-files', '--', 'forced-ignored.txt')) -eq 'forced-ignored.txt') 'explicitly tracked ignored file is also tracked in the candidate'
Check (-not (Test-Path -LiteralPath (Join-Path $result.manifest.checkout 'local.build.props'))) 'forcing candidate index entries does not copy ignored local state'

$root = New-Fixture 'removed-gitlink'
# A valid synthetic gitlink points to an earlier local commit. The submodule is
# uninitialized and then staged for removal, so no remote or file transport runs.
$moduleSha = Git $root @('rev-parse', 'HEAD')
Write-File $root '.gitmodules' "[submodule `"Removed`"]`n`tpath = ThirdParty/Removed`n`turl = https://example.invalid/never-used.git`n"
$null = Git $root @('add', '.gitmodules')
$null = Git $root @('update-index', '--add', '--cacheinfo', "160000,$moduleSha,ThirdParty/Removed")
$null = Git $root @('commit', '-qm', 'Synthetic gitlink baseline')
$null = Git $root @('update-index', '--force-remove', 'ThirdParty/Removed')
Write-File $root '.gitmodules' ''
$result = Run-Fixture $root
Check ($result.code -eq 0 -and $result.manifest.status -eq 'Passed') "removed gitlink verifies without resurrecting HEAD dependency: $($result.output)"
Check ([string]::IsNullOrEmpty((Git $result.manifest.checkout @('ls-files', '--stage', '--', 'ThirdParty/Removed')))) 'deleted gitlink is absent from the candidate index'
Check ($result.manifest.submodules.Count -eq 0) 'manifest contains only current gitlinks'

$root = New-Fixture 'failing-verifier'
$savedFail = $env:S1_SNAPSHOT_FIXTURE_FAIL
try {
    $env:S1_SNAPSHOT_FIXTURE_FAIL = 'true'
    $result = Run-Fixture $root
    Check ($result.code -ne 0) 'failed verifier fails outer command'
    Check ($result.manifest.status -eq 'Failed' -and $result.manifest.verificationExitCode -eq 23) 'failed verifier evidence retained'
}
finally { $env:S1_SNAPSHOT_FIXTURE_FAIL = $savedFail }

$root = New-Fixture 'mutating-verifier'
$savedMutate = $env:S1_SNAPSHOT_FIXTURE_MUTATE
try {
    $env:S1_SNAPSHOT_FIXTURE_MUTATE = 'true'
    $result = Run-Fixture $root
    Check ($result.code -ne 0 -and $result.output -match 'changed snapshot source files') 'verifier source mutation cannot produce false green'
}
finally { $env:S1_SNAPSHOT_FIXTURE_MUTATE = $savedMutate }

$root = New-Fixture 'capture-only'
$result = Run-Fixture $root @('-SnapshotOnly')
Check ($result.code -eq 0 -and $result.manifest.status -eq 'Captured') 'snapshot-only is captured, never passed'
Check ($null -eq $result.manifest.verificationExitCode) 'snapshot-only does not claim tests ran'

$root = New-Fixture 'unsafe-destination'
$output = & pwsh -NoProfile -ExecutionPolicy Bypass -File (Join-Path $root 'Tools/verify-clean-checkout.ps1') -ScratchRoot (Join-Path $root 'nested') 2>&1 | Out-String
Check ($LASTEXITCODE -ne 0 -and $output -match 'outside the source repository') 'nested scratch rejected'
Check (-not (Test-Path -LiteralPath (Join-Path $root 'nested'))) 'unsafe destination is not created'

$root = New-Fixture 'deep-junction-destination'
$junction = Join-Path $scratch 'external-junction'
$null = New-Item -ItemType Junction -Path $junction -Target $root
$redirectedScratch = Join-Path $junction 'not-yet-created/runs'
$output = & pwsh -NoProfile -ExecutionPolicy Bypass -File (Join-Path $root 'Tools/verify-clean-checkout.ps1') -ScratchRoot $redirectedScratch -SnapshotOnly 2>&1 | Out-String
Check ($LASTEXITCODE -ne 0 -and $output -match 'Reparse points/symlinks') 'junction above the immediate scratch parent is rejected'
Check (-not (Test-Path -LiteralPath (Join-Path $root 'not-yet-created'))) 'deep scratch junction causes no writes in the source'

$root = New-Fixture 'dirty-submodule'
$module = Join-Path $root 'ThirdParty/Fixture'
$null = New-Item -ItemType Directory -Path $module -Force
$null = Git $module @('init', '-q')
$null = Git $module @('config', 'user.name', 'Snapshot Fixture')
$null = Git $module @('config', 'user.email', 'fixture@example.invalid')
$null = Git $module @('config', 'core.hooksPath', 'no-hooks')
Write-File $module 'tracked.txt' 'baseline'
$null = Git $module @('add', '.')
$null = Git $module @('commit', '-qm', 'Synthetic submodule baseline')
$sha = Git $module @('rev-parse', 'HEAD')
$null = Git $root @('update-index', '--add', '--cacheinfo', "160000,$sha,ThirdParty/Fixture")
Write-File $module 'tracked.txt' 'dirty submodule contents'
$result = Run-Fixture $root
Check ($result.code -ne 0 -and $result.output -match 'Submodule has uncommitted files') 'dirty submodule rejected rather than silently omitted'

# Git's configured ignore rule must not conceal dirty nested dependencies.
$null = Git $module @('checkout', '--', 'tracked.txt')
$nested = Join-Path $module 'Nested'
$null = New-Item -ItemType Directory -Path $nested -Force
$null = Git $nested @('init', '-q')
$null = Git $nested @('config', 'user.name', 'Snapshot Fixture')
$null = Git $nested @('config', 'user.email', 'fixture@example.invalid')
$null = Git $nested @('config', 'core.hooksPath', 'no-hooks')
Write-File $nested 'nested.txt' 'clean nested baseline'
$null = Git $nested @('add', '.')
$null = Git $nested @('commit', '-qm', 'Synthetic nested baseline')
$nestedSha = Git $nested @('rev-parse', 'HEAD')
Write-File $module '.gitmodules' "[submodule `"Nested`"]`n`tpath = Nested`n`turl = https://example.invalid/never-used.git`n`tignore = all`n"
$null = Git $module @('add', '.gitmodules')
$null = Git $module @('update-index', '--add', '--cacheinfo', "160000,$nestedSha,Nested")
$null = Git $module @('commit', '-qm', 'Synthetic ignored nested submodule')
Write-File $nested 'nested.txt' 'dirty nested contents hidden by ignore=all'
Check ([string]::IsNullOrEmpty((Git $module @('status', '--porcelain', '--untracked-files=all')))) 'fixture demonstrates that default Git status hides nested dirtiness'
$result = Run-Fixture $root
Check ($result.code -ne 0 -and $result.output -match 'Submodule has uncommitted files') 'explicit submodule status overrides ignore=all and rejects nested dirtiness'

Write-Host "clean-checkout.Tests: $script:passed passed; $script:failed failed. Artifacts: $scratch"
if ($script:failed -gt 0) { exit 1 }
exit 0
