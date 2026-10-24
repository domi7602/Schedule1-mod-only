#Requires -Version 7
# Local repositories, fake game files and injected subprocess failures only.
# Never accesses the real game, a network remote or the user's Git configuration.
[CmdletBinding()]
param([string]$ScratchRoot)

$ErrorActionPreference = 'Stop'
$PSNativeCommandUseErrorActionPreference = $false
$repo = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
if (-not $ScratchRoot) { $ScratchRoot = Join-Path $env:USERPROFILE '.cache/Schedule1Tools/tests' }
$scratch = Join-Path ([IO.Path]::GetFullPath($ScratchRoot)) ('ws-' + [guid]::NewGuid().ToString('N').Substring(0, 12))
$null = New-Item -ItemType Directory -Path $scratch -Force
$pwsh = (Get-Process -Id $PID).Path
$git = (Get-Command git -CommandType Application -ErrorAction Stop | Select-Object -First 1).Source
$script:passed = 0
$script:failed = 0
$script:case = 0
function Check([bool]$Condition, [string]$Name, [string]$Detail = '') {
    if ($Condition) { $script:passed++ }
    else { $script:failed++; Write-Host "FAIL: $Name`n$Detail" -ForegroundColor Red }
}
function Write-File([string]$Root, [string]$Relative, [string]$Text) {
    $path = Join-Path $Root $Relative
    $null = New-Item -ItemType Directory -Path (Split-Path $path -Parent) -Force
    [IO.File]::WriteAllText($path, $Text, [Text.UTF8Encoding]::new($false))
}
function Git([string]$Root, [string[]]$Arguments) {
    $output = & $git -c core.longpaths=true -C $Root @Arguments 2>&1 | Out-String
    if ($LASTEXITCODE -ne 0) { throw "Fixture git failed: $output" }
    return $output.Trim()
}
function New-Repo([string]$Name) {
    $root = Join-Path $scratch $Name
    $null = New-Item -ItemType Directory -Path $root -Force
    $null = Git $root @('init', '-q')
    $null = Git $root @('config', 'user.name', 'Workspace Fixture')
    $null = Git $root @('config', 'user.email', 'fixture@example.invalid')
    $null = Git $root @('config', 'core.autocrlf', 'false')
    return $root
}
function Commit-Fixture([string]$Root) {
    $null = Git $Root @('add', '--all')
    $null = Git $Root @('-c', 'core.hooksPath=no-hooks', 'commit', '-qm', 'fixture')
}
$template = '<Project><PropertyGroup><Il2CppAssembliesPath>old</Il2CppAssembliesPath><LocalIl2CppDeploymentPath>old</LocalIl2CppDeploymentPath><MelonLoaderAssembliesPath>old</MelonLoaderAssembliesPath><UserLibsPath>old</UserLibsPath><KeepSetting>true</KeepSetting></PropertyGroup></Project>'
function New-SetupRepo([string]$Name) {
    $root = New-Repo $Name
    Write-File $root 'Tools/.keep' ''
    Copy-Item -LiteralPath (Join-Path $repo 'Tools/setup-workspace.ps1') -Destination (Join-Path $root 'Tools/setup-workspace.ps1')
    Write-File $root 'ThirdParty/S1API/example.build.props' $template
    Write-File $root 'ThirdParty/S1MAPI/local.build.props.example' $template
    Write-File $root 'ThirdParty/S1API/local.build.props' 'api-before'
    Write-File $root 'ThirdParty/S1MAPI/local.build.props' 'mapi-before'
    return $root
}
function Run-Script([string]$Script, [string[]]$Arguments) {
    $script:case++
    $output = & $pwsh -NoProfile -NonInteractive -ExecutionPolicy Bypass -File $Script @Arguments 2>&1 | Out-String
    $code = $LASTEXITCODE
    Write-File $scratch ("case-$script:case.log") $output
    return [pscustomobject]@{ Code = $code; Output = $output }
}
function Run-Setup([string]$Root, [string[]]$Extra = @()) {
    return Run-Script (Join-Path $Root 'Tools/setup-workspace.ps1') (@('-GameDir', $game, '-NonInteractive') + $Extra)
}
function Assert-PropsUnchanged([string]$Root, [string]$Name) {
    Check ((Get-Content -LiteralPath (Join-Path $Root 'ThirdParty/S1API/local.build.props') -Raw) -ceq 'api-before') "$Name preserves S1API props"
    Check ((Get-Content -LiteralPath (Join-Path $Root 'ThirdParty/S1MAPI/local.build.props') -Raw) -ceq 'mapi-before') "$Name preserves S1MAPI props"
}
function Run-Laptop([string]$Target, [string]$Upstream, [string[]]$Extra = @()) {
    return Run-Script (Join-Path $repo 'Tools/new-laptop-workspace.ps1') (@('-TargetDir', $Target, '-UpstreamUrl', $Upstream, '-GameDir', $game, '-NonInteractive') + $Extra)
}

$names = @('GIT_CONFIG_NOSYSTEM', 'GIT_CONFIG_GLOBAL', 'GIT_CONFIG_SYSTEM', 'GIT_CONFIG_COUNT', 'GIT_DIR', 'GIT_WORK_TREE', 'GIT_INDEX_FILE', 'GIT_ALLOW_PROTOCOL', 'PATH', 'WORKSPACE_TEST_GIT', 'WORKSPACE_TEST_FAIL')
$saved = @{}
foreach ($name in $names) { $saved[$name] = [Environment]::GetEnvironmentVariable($name, 'Process') }
try {
    $env:GIT_CONFIG_NOSYSTEM = '1'
    $env:GIT_CONFIG_GLOBAL = Join-Path $scratch 'empty-global-config'
    $env:GIT_CONFIG_SYSTEM = Join-Path $scratch 'empty-system-config'
    $env:GIT_CONFIG_COUNT = $null
    $env:GIT_DIR = $null
    $env:GIT_WORK_TREE = $null
    $env:GIT_INDEX_FILE = $null
    $env:GIT_ALLOW_PROTOCOL = 'file'
    $env:WORKSPACE_TEST_GIT = $git
    $env:WORKSPACE_TEST_FAIL = $null
    $game = Join-Path $scratch 'game $cash & space'
    Write-File $game 'MelonLoader/Il2CppAssemblies/Assembly-CSharp.dll' 'fixture assembly'
    Write-File $game 'MelonLoader/net6/MelonLoader.dll' 'fixture loader'
    $gameBefore = @(Get-ChildItem -LiteralPath $game -Recurse -File | Get-FileHash | ForEach-Object Hash) -join ','

    $root = New-SetupRepo 'valid setup'
    $result = Run-Setup $root
    Check ($result.Code -eq 0) 'noninteractive setup succeeds with both templates and fake references' $result.Output
    foreach ($part in @('S1API', 'S1MAPI')) {
        $xml = [xml](Get-Content -LiteralPath (Join-Path $root "ThirdParty/$part/local.build.props") -Raw)
        Check ($xml.Project.PropertyGroup.LocalIl2CppDeploymentPath -ceq $game) "$part preserves literal dollar, ampersand and spaces in the game path"
        Check ($xml.Project.PropertyGroup.Il2CppAssembliesPath -ceq (Join-Path $game 'MelonLoader/Il2CppAssemblies')) "$part receives the resolved IL2CPP path"
        Check ($xml.Project.PropertyGroup.KeepSetting -eq 'true') "$part keeps unrelated template properties"
    }
    $before = @(Get-FileHash -LiteralPath (Join-Path $root 'ThirdParty/S1API/local.build.props'), (Join-Path $root 'ThirdParty/S1MAPI/local.build.props') | ForEach-Object Hash) -join ','
    $result = Run-Setup $root
    $after = @(Get-FileHash -LiteralPath (Join-Path $root 'ThirdParty/S1API/local.build.props'), (Join-Path $root 'ThirdParty/S1MAPI/local.build.props') | ForEach-Object Hash) -join ','
    Check ($result.Code -eq 0 -and $before -eq $after) 'repeat setup is byte-idempotent'
    $null = & $git -C $root config --local --get core.hooksPath 2>&1
    Check ($LASTEXITCODE -eq 1) 'noninteractive setup does not activate hooks implicitly'
    $result = Run-Setup $root @('-EnableHooks')
    Check ($result.Code -eq 0 -and (Git $root @('config', '--local', '--get', 'core.hooksPath')) -eq '.githooks') 'explicit hook opt-in writes only local config' $result.Output
    $null = Git $root @('config', '--local', 'core.hooksPath', 'my-existing-hooks')
    $result = Run-Setup $root @('-EnableHooks')
    Check ($result.Code -eq 0 -and (Git $root @('config', '--local', '--get', 'core.hooksPath')) -eq 'my-existing-hooks') 'existing hook choice is preserved'

    foreach ($missing in @('S1API/example.build.props', 'S1MAPI/local.build.props.example')) {
        $root = New-SetupRepo ('missing-' + ($missing -split '/')[0])
        Remove-Item -LiteralPath (Join-Path $root ('ThirdParty/' + $missing))
        $result = Run-Setup $root
        Check ($result.Code -ne 0 -and $result.Output -match 'git submodule update' -and $result.Output -match '--init' -and $result.Output -match '--recursive') "missing $missing fails with actionable initialization guidance" $result.Output
        Assert-PropsUnchanged $root 'missing template'
    }
    $root = New-SetupRepo 'invalid-template'
    Write-File $root 'ThirdParty/S1MAPI/local.build.props.example' '<Project>broken'
    $result = Run-Setup $root
    Check ($result.Code -ne 0) 'invalid second XML template fails before any writes'
    Assert-PropsUnchanged $root 'invalid template'
    $root = New-SetupRepo 'missing-property'
    Write-File $root 'ThirdParty/S1MAPI/local.build.props.example' '<Project><PropertyGroup /></Project>'
    $result = Run-Setup $root
    Check ($result.Code -ne 0 -and $result.Output -match 'IL2CPP property missing') 'unsupported template cannot silently generate wrong props'
    Assert-PropsUnchanged $root 'missing required property'
    $root = New-SetupRepo 'missing-game'
    $result = Run-Script (Join-Path $root 'Tools/setup-workspace.ps1') @('-GameDir', (Join-Path $scratch 'absent game'), '-NonInteractive')
    Check ($result.Code -ne 0 -and $result.Output -match 'Game directory not found') 'invalid explicit game path never falls back to the installed game'
    Assert-PropsUnchanged $root 'invalid game path'

    # Build local nested submodules; GIT_ALLOW_PROTOCOL=file forbids network use.
    $leaf = New-Repo 'leaf'
    Write-File $leaf 'nested.txt' 'nested submodule proof'
    Commit-Fixture $leaf
    $api = New-Repo 'api'
    Write-File $api 'example.build.props' $template
    Write-File $api '.gitignore' 'local.build.props'
    $null = Git $api @('submodule', 'add', '--', $leaf, 'Nested/Leaf')
    Commit-Fixture $api
    $mapi = New-Repo 'mapi'
    Write-File $mapi 'local.build.props.example' $template
    Write-File $mapi '.gitignore' 'local.build.props'
    Commit-Fixture $mapi
    $upstream = New-Repo 'upstream'
    Write-File $upstream 'Tools/.keep' ''
    Copy-Item -LiteralPath (Join-Path $repo 'Tools/setup-workspace.ps1') -Destination (Join-Path $upstream 'Tools/setup-workspace.ps1')
    $null = Git $upstream @('submodule', 'add', '--', $api, 'ThirdParty/S1API')
    $null = Git $upstream @('submodule', 'add', '--', $mapi, 'ThirdParty/S1MAPI')
    Commit-Fixture $upstream
    $upstreamBefore = Git $upstream @('rev-parse', 'HEAD')
    $target = Join-Path $scratch 'laptop one'
    $result = Run-Laptop $target $upstream
    Check ($result.Code -eq 0) 'recursive local clone and real setup succeed' $result.Output
    Check (Test-Path -LiteralPath (Join-Path $target 'ThirdParty/S1API/Nested/Leaf/nested.txt')) 'nested submodule is initialized by recursive clone'
    Check ((Git $target @('remote')) -eq 'upstream') 'omitted backup leaves only the honestly named source remote'
    Check ((Git $target @('status', '--porcelain', '--untracked-files=all')) -eq '') 'generated local props do not dirty the new checkout'
    Check ($result.Output -match 'not technically read-only' -and $result.Output -notmatch 'physically impossible') 'success output makes no false push-barrier claim'
    $target = Join-Path $scratch 'laptop backup'
    $result = Run-Laptop $target $upstream @('-BackupUrl', $leaf, '-EnableHooks')
    Check ($result.Code -eq 0 -and (Git $target @('remote', 'get-url', 'origin')) -eq $leaf) 'optional backup remote is added and setup completes' $result.Output
    Check ((Git $target @('config', '--local', '--get', 'core.hooksPath')) -eq '.githooks') 'explicit hook choice is forwarded to the child setup'
    $existing = Join-Path $scratch 'existing target'
    Write-File $existing 'keep.txt' 'keep this exact content'
    $result = Run-Laptop $existing $upstream
    Check ($result.Code -ne 0 -and (Get-Content -LiteralPath (Join-Path $existing 'keep.txt') -Raw) -ceq 'keep this exact content') 'existing target is refused without modifying it'
    Check (-not (Test-Path -LiteralPath (Join-Path $existing '.git'))) 'existing target never receives Git metadata'
    $result = Run-Script (Join-Path $repo 'Tools/new-laptop-workspace.ps1') @('-NonInteractive')
    Check ($result.Code -ne 0 -and $result.Output -match 'Pass -TargetDir') 'noninteractive mode never prompts for an omitted target'
    $result = Run-Laptop (Join-Path $scratch 'clone-failed') (Join-Path $scratch 'absent upstream')
    Check ($result.Code -ne 0 -and $result.Output -match 'git clone failed' -and $result.Output -notmatch '\[new-laptop-workspace\] Done') 'clone failure cannot print final success'

    $broken = New-Repo 'broken-submodule-upstream'
    Write-File $broken '.gitmodules' ("[submodule `"broken`"]`n`tpath = ThirdParty/Broken`n`turl = " + (Join-Path $scratch 'absent-submodule').Replace('\', '/') + "`n")
    $null = Git $broken @('add', '.gitmodules')
    $leafHead = Git $leaf @('rev-parse', 'HEAD')
    $null = Git $broken @('update-index', '--add', '--cacheinfo', "160000,$leafHead,ThirdParty/Broken")
    $null = Git $broken @('-c', 'core.hooksPath=no-hooks', 'commit', '-qm', 'broken local submodule fixture')
    $target = Join-Path $scratch 'submodule-failed'
    $result = Run-Laptop $target $broken
    Check ($result.Code -ne 0 -and $result.Output -match 'git clone failed' -and $result.Output -notmatch '\[new-laptop-workspace\] Done') 'recursive submodule clone failure is fatal' $result.Output
    Check (Test-Path -LiteralPath (Join-Path $target '.git')) 'failed submodule checkout is retained for diagnosis'

    # Inject failures while forwarding every other Git operation to the real binary.
    $shim = Join-Path $scratch 'shim'
    $fakeGit = @'
$a = @($args)
$stage = $env:WORKSPACE_TEST_FAIL
$fail = ($stage -eq 'rename' -and $a -contains 'rename') -or
    ($stage -eq 'inspect' -and $a -contains 'remote' -and $a -contains '-v') -or
    ($stage -eq 'backup' -and $a -contains 'remote' -and $a -contains 'add') -or
    ($stage -eq 'hooks-read' -and $a -contains 'core.hooksPath' -and $a -contains '--get') -or
    ($stage -eq 'hooks-write' -and $a -contains 'core.hooksPath' -and $a -contains '--local' -and $a -notcontains '--get')
if ($fail) { Write-Output "injected $stage failure"; exit 51 }
& $env:WORKSPACE_TEST_GIT @a
exit $LASTEXITCODE
'@
    Write-File $shim 'git.ps1' $fakeGit
    Write-File $shim 'git.cmd' ("@echo off`n`"$pwsh`" -NoProfile -ExecutionPolicy Bypass -File `"%~dp0git.ps1`" %*`nexit /b %ERRORLEVEL%`n")
    $env:PATH = $shim + [IO.Path]::PathSeparator + $saved.PATH
    foreach ($stage in @('rename', 'inspect', 'backup')) {
        $env:WORKSPACE_TEST_FAIL = $stage
        $target = Join-Path $scratch ('failure-' + $stage)
        $result = Run-Laptop $target $upstream @('-BackupUrl', $leaf)
        Check ($result.Code -ne 0 -and $result.Output -match 'exit 51' -and $result.Output -notmatch '\[new-laptop-workspace\] Done') "$stage failure propagates without success" $result.Output
        Check (-not (Test-Path -LiteralPath (Join-Path $target 'ThirdParty/S1API/local.build.props'))) "$stage failure prevents setup from writing props"
        Check (Test-Path -LiteralPath (Join-Path $target '.git')) "$stage failure retains the new checkout for diagnosis"
    }
    $root = New-SetupRepo 'hooks-query-failed'
    $env:WORKSPACE_TEST_FAIL = 'hooks-read'
    $result = Run-Setup $root
    Check ($result.Code -ne 0 -and $result.Output -match 'exit 51') 'Git config read failure is distinct from an unset hook key'
    Assert-PropsUnchanged $root 'hook query failure'
    $root = New-SetupRepo 'hooks-write-failed'
    $env:WORKSPACE_TEST_FAIL = 'hooks-write'
    $result = Run-Setup $root @('-EnableHooks')
    Check ($result.Code -ne 0 -and $result.Output -match 'Props were written' -and $result.Output -notmatch '\[setup-workspace\] Done') 'hook-write failure reports partial completion without final success'
    $env:WORKSPACE_TEST_FAIL = $null
    $env:PATH = $saved.PATH

    $badSetup = New-Repo 'bad-setup-upstream'
    Write-File $badSetup 'Tools/.keep' ''
    Copy-Item -LiteralPath (Join-Path $repo 'Tools/setup-workspace.ps1') -Destination (Join-Path $badSetup 'Tools/setup-workspace.ps1')
    Commit-Fixture $badSetup
    $target = Join-Path $scratch 'setup-failed'
    $result = Run-Laptop $target $badSetup
    Check ($result.Code -ne 0 -and $result.Output -match 'setup-workspace.ps1 failed' -and $result.Output -notmatch '\[new-laptop-workspace\] Done') 'PowerShell child failure uses its actual process exit status' $result.Output
    Check (Test-Path -LiteralPath (Join-Path $target '.git')) 'setup failure retains its checkout'
    Check ((Git $upstream @('rev-parse', 'HEAD')) -eq $upstreamBefore -and (Git $upstream @('status', '--porcelain', '--untracked-files=all')) -eq '') 'all laptop operations leave the source repository unchanged'
    $gameAfter = @(Get-ChildItem -LiteralPath $game -Recurse -File | Get-FileHash | ForEach-Object Hash) -join ','
    Check ($gameAfter -eq $gameBefore) 'all setup/laptop operations leave fake game contents unchanged'
    Check (-not (Test-Path -LiteralPath $env:GIT_CONFIG_GLOBAL) -and -not (Test-Path -LiteralPath $env:GIT_CONFIG_SYSTEM)) 'no global or system Git configuration is written'
}
finally {
    foreach ($name in $names) { [Environment]::SetEnvironmentVariable($name, $saved[$name], 'Process') }
}
Write-Host "workspace-setup: $script:passed passed, $script:failed failed. Fixtures: $scratch"
if ($script:failed) { exit 1 }
exit 0
