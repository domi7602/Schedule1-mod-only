# Regression tests for bounded build/deploy safety.
# Runs entirely in a scratch fake install; never touches the real game and
# never copies a real ThirdParty DLL.

$ErrorActionPreference = 'Stop'
$repo = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
$preflightPath = Join-Path $repo 'Tools\check-deploy-preflight.ps1'
$deployPath = Join-Path $repo 'Tools\deploy-thirdparty.ps1'
$buildAllPath = Join-Path $repo 'Tools\build-all.ps1'

$scratch = Join-Path $env:LOCALAPPDATA 'hermes/cache/scratch'
$root = Join-Path $scratch ('s1-deploy-regression-' + [guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $root -Force | Out-Null

$script:Passed = 0
$script:Failed = 0
function Assert-True {
    param([bool]$Condition, [string]$Name, [string]$Detail = '')
    if ($Condition) { Write-Host "PASS  $Name" -ForegroundColor Green; $script:Passed++ }
    else {
        Write-Host "FAIL  $Name" -ForegroundColor Red
        # Child output is the only clue when an assertion fails on a machine that
        # differs from the author's (CI runner, clean box). Print the tail only.
        if ($Detail) {
            $tail = @($Detail -split "`r?`n" | Where-Object { $_ -ne '' } | Select-Object -Last 12)
            foreach ($line in $tail) { Write-Host "      $line" -ForegroundColor DarkGray }
        }
        $script:Failed++
    }
}

function New-FakeInstall {
    param([string]$Name, [switch]$NoProbe)
    $game = Join-Path $root $Name
    New-Item -ItemType Directory -Path (Join-Path $game 'MelonLoader\Il2CppAssemblies') -Force | Out-Null
    New-Item -ItemType Directory -Path (Join-Path $game 'Mods') -Force | Out-Null
    if (-not $NoProbe) {
        Set-Content -LiteralPath (Join-Path $game 'MelonLoader\Il2CppAssemblies\Assembly-CSharp.dll') -Value 'fake-probe'
    }
    return $game
}

function New-FakeThirdParty {
    param([string]$Name)
    $tp = Join-Path $root $Name
    New-Item -ItemType Directory -Path (Join-Path $tp 'a') -Force | Out-Null
    New-Item -ItemType Directory -Path (Join-Path $tp 'b') -Force | Out-Null
    [IO.File]::WriteAllBytes((Join-Path $tp 'a\ModA.dll'), [Text.Encoding]::UTF8.GetBytes('fake-mod-a'))
    [IO.File]::WriteAllBytes((Join-Path $tp 'b\ModB.dll'), [Text.Encoding]::UTF8.GetBytes('fake-mod-b'))
    New-Item -ItemType Directory -Path (Join-Path $tp 'dup1') -Force | Out-Null
    New-Item -ItemType Directory -Path (Join-Path $tp 'dup2') -Force | Out-Null
    [IO.File]::WriteAllBytes((Join-Path $tp 'dup1\Dup.dll'), [Text.Encoding]::UTF8.GetBytes('dup-one'))
    [IO.File]::WriteAllBytes((Join-Path $tp 'dup2\Dup.dll'), [Text.Encoding]::UTF8.GetBytes('dup-two'))
    return $tp
}

function Set-Allowlist {
    param([string]$ThirdPartyRoot, [string[]]$Lines, [string]$Name = '.deployallow')
    $path = Join-Path $ThirdPartyRoot $Name
    Set-Content -LiteralPath $path -Value $Lines -Encoding utf8
    return $path
}

function Invoke-Child {
    param([string]$Script, [string[]]$Arguments)
    $output = & pwsh -NoProfile -NonInteractive -File $Script @Arguments 2>&1
    return [pscustomobject]@{ ExitCode = $LASTEXITCODE; Output = ($output | Out-String) }
}

function Get-CodeFromException {
    param($Exception)
    if ($null -ne $Exception.Data -and $Exception.Data.Contains('S1ExitCode')) {
        return [int]$Exception.Data['S1ExitCode']
    }
    return -1
}

Write-Host "Scratch root: $root" -ForegroundColor Cyan

# ---------------------------------------------------------------- preflight
. $preflightPath

$game1 = New-FakeInstall 'game1'
$game2 = New-FakeInstall 'game2'

$env:SCHEDULE1_PATH = Join-Path $root 'does-not-exist'
$ctx = Invoke-S1DeployPreflight -GameDir $game1 -Operation Mod
Assert-True ($ctx.GameDir -eq [IO.Path]::GetFullPath($game1)) 'preflight: explicit GameDir overrides SCHEDULE1_PATH'
Assert-True ($ctx.ModsDir -eq (Join-Path $game1 'Mods')) 'preflight: ModsDir derived from override'
$ctxProject = Invoke-S1DeployPreflight -GameDir $game1 -Operation 'HitmanPhone'
Assert-True ($ctxProject.Operation -eq 'HitmanPhone') 'preflight: arbitrary MSBuild project label accepted'

$relativeCode = -2
try { Invoke-S1DeployPreflight -GameDir 'relative\game' | Out-Null } catch { $relativeCode = Get-CodeFromException $_.Exception }
Assert-True ($relativeCode -eq 2) 'preflight: relative GameDir rejected (code 2)'

$repoCode = -2
try { Invoke-S1DeployPreflight -GameDir $repo | Out-Null } catch { $repoCode = Get-CodeFromException $_.Exception }
Assert-True ($repoCode -eq 2) 'preflight: repo root rejected (code 2)'

$missing = Join-Path $root 'missing-game'
$missingCode = -2
try { Invoke-S1DeployPreflight -GameDir $missing | Out-Null } catch { $missingCode = Get-CodeFromException $_.Exception }
Assert-True ($missingCode -eq 3) 'preflight: missing install rejected (code 3)'

$noProbe = New-FakeInstall 'no-probe' -NoProbe
$noProbeCode = -2
try { Invoke-S1DeployPreflight -GameDir $noProbe | Out-Null } catch { $noProbeCode = Get-CodeFromException $_.Exception }
Assert-True ($noProbeCode -eq 3) 'preflight: missing MelonLoader probe rejected (code 3)'

$originalProvider = (Get-Command Get-S1GameProcesses).ScriptBlock
function Get-S1GameProcesses { @([pscustomobject]@{ ProcessName = 'Schedule I'; Id = 4242 }) }
$runningCode = -2
try { Invoke-S1DeployPreflight -GameDir $game1 | Out-Null } catch { $runningCode = Get-CodeFromException $_.Exception }
Assert-True ($runningCode -eq 4) 'preflight: running game rejected (code 4)'
Set-Item -Path Function:\Get-S1GameProcesses -Value $originalProvider

$savedGetProcess = (Get-Command Get-Process).ScriptBlock
function Get-Process { throw 'synthetic enumeration failure' }
$unknownCode = -2
try { Invoke-S1DeployPreflight -GameDir $game1 | Out-Null } catch { $unknownCode = Get-CodeFromException $_.Exception }
Assert-True ($unknownCode -eq 5) 'preflight: process enumeration failure is fail-closed (code 5)'
Set-Item -Path Function:\Get-S1GameProcesses -Value $originalProvider
Remove-Item -Path Function:\Get-Process -ErrorAction SilentlyContinue

$env:SCHEDULE1_PATH = $game2
$ctx2 = Invoke-S1DeployPreflight -Operation Mod
Assert-True ($ctx2.GameDir -eq [IO.Path]::GetFullPath($game2)) 'preflight: SCHEDULE1_PATH fallback used when GameDir omitted'
Remove-Item Env:\SCHEDULE1_PATH -ErrorAction SilentlyContinue

$preflightJson = Invoke-Child -Script $preflightPath -Arguments @('-GameDir', $game1, '-Operation', 'Build', '-Json')
$jsonObj = $null
try { $jsonObj = $preflightJson.Output | ConvertFrom-Json } catch { }
Assert-True ($preflightJson.ExitCode -eq 0 -and $null -ne $jsonObj -and $jsonObj.Status -eq 'Ok') 'preflight CLI: JSON success (exit 0)'
$preflightBad = Invoke-Child -Script $preflightPath -Arguments @('-GameDir', $missing, '-Json')
$badObj = $null
try { $badObj = $preflightBad.Output | ConvertFrom-Json } catch { }
Assert-True ($preflightBad.ExitCode -eq 3 -and $null -ne $badObj -and $badObj.ExitCode -eq 3) 'preflight CLI: JSON failure reports exit 3'

# ------------------------------------------------------------ third-party
. $deployPath

$game3 = New-FakeInstall 'game3'
$tp = New-FakeThirdParty 'thirdparty'

$missingAllow = Invoke-Child -Script $deployPath -Arguments @('-GameDir', $game3, '-ThirdPartyRoot', $tp, '-AllowlistPath', (Join-Path $tp 'absent.deployallow'))
Assert-True ($missingAllow.ExitCode -ne 0) 'deploy: missing allowlist fails (no implicit deploy-all)'
Assert-True (-not (Test-Path (Join-Path $game3 'Mods\ModA.dll'))) 'deploy: missing allowlist copied nothing'

$emptyAllowPath = Set-Allowlist -ThirdPartyRoot $tp -Lines @('# empty', '')
$empty = Invoke-Child -Script $deployPath -Arguments @('-GameDir', $game3, '-ThirdPartyRoot', $tp, '-AllowlistPath', $emptyAllowPath)
Assert-True ($empty.ExitCode -eq 0) 'deploy: empty allowlist succeeds'
Assert-True (@(Get-ChildItem (Join-Path $game3 'Mods') -File).Count -eq 0) 'deploy: empty allowlist deploys nothing'

# Never deploy actual third-party binaries, even if the allowlist changes later.
$shippedRules = @(Get-Content (Join-Path $repo 'ThirdParty/.deployallow') | Where-Object { $_.Trim() -and -not $_.Trim().StartsWith('#') })
Assert-True ($shippedRules.Count -eq 0) 'deploy: shipped allowlist requires deliberate opt-in'

$oneAllowPath = Set-Allowlist -ThirdPartyRoot $tp -Lines @('a/ModA.dll')
$one = Invoke-Child -Script $deployPath -Arguments @('-GameDir', $game3, '-ThirdPartyRoot', $tp, '-AllowlistPath', $oneAllowPath)
$sourceHash = (Get-FileHash (Join-Path $tp 'a\ModA.dll') -Algorithm SHA256).Hash
$destHash = if (Test-Path (Join-Path $game3 'Mods\ModA.dll')) { (Get-FileHash (Join-Path $game3 'Mods\ModA.dll') -Algorithm SHA256).Hash } else { '' }
Assert-True ($one.ExitCode -eq 0 -and $destHash -eq $sourceHash) 'deploy: explicit allowlist copies one DLL and hashes match'
Assert-True (-not (Test-Path (Join-Path $game3 'Mods\ModB.dll'))) 'deploy: non-allowlisted DLL not copied'

$oneAgain = Invoke-Child -Script $deployPath -Arguments @('-GameDir', $game3, '-ThirdPartyRoot', $tp, '-AllowlistPath', $oneAllowPath)
Assert-True ($oneAgain.ExitCode -eq 0 -and $oneAgain.Output -match 'same\s+a/ModA.dll') 'deploy: idempotent re-run reports unchanged'

$dupAllowPath = Set-Allowlist -ThirdPartyRoot $tp -Lines @('**/Dup.dll')
$dup = Invoke-Child -Script $deployPath -Arguments @('-GameDir', $game3, '-ThirdPartyRoot', $tp, '-AllowlistPath', $dupAllowPath)
Assert-True ($dup.ExitCode -ne 0 -and $dup.Output -match 'Ambiguous') 'deploy: duplicate destination names rejected'
Assert-True (-not (Test-Path (Join-Path $game3 'Mods\Dup.dll'))) 'deploy: ambiguous target copied nothing'

$badPatternPath = Set-Allowlist -ThirdPartyRoot $tp -Lines @('../escape.dll')
$badPattern = Invoke-Child -Script $deployPath -Arguments @('-GameDir', $game3, '-ThirdPartyRoot', $tp, '-AllowlistPath', $badPatternPath)
Assert-True ($badPattern.ExitCode -ne 0) 'deploy: traversal allowlist pattern rejected'

# Restore the single-DLL allowlist (the shared fixture file was reused above).
$oneAllowPath = Set-Allowlist -ThirdPartyRoot $tp -Lines @('a/ModA.dll')

$game4 = New-FakeInstall 'game4'
$badGamePath = Join-Path $root 'not-a-game'
New-Item -ItemType Directory -Path $badGamePath -Force | Out-Null
$badGame = Invoke-Child -Script $deployPath -Arguments @('-GameDir', $badGamePath, '-ThirdPartyRoot', $tp, '-AllowlistPath', $oneAllowPath)
Assert-True ($badGame.ExitCode -eq 3 -and $badGame.Output -match 'preflight|install') 'deploy: missing game install fails closed with preflight exit 3'
Assert-True (@(Get-ChildItem (Join-Path $game4 'Mods') -File).Count -eq 0) 'deploy: bad game target left untouched'

# Explicit -GameDir must win over a bogus SCHEDULE1_PATH.
$env:SCHEDULE1_PATH = $badGamePath
$override = Invoke-Child -Script $deployPath -Arguments @('-GameDir', $game4, '-ThirdPartyRoot', $tp, '-AllowlistPath', $oneAllowPath)
$overrideHash = if (Test-Path (Join-Path $game4 'Mods\ModA.dll')) { (Get-FileHash (Join-Path $game4 'Mods\ModA.dll') -Algorithm SHA256).Hash } else { '' }
Assert-True ($override.ExitCode -eq 0 -and $overrideHash -eq $sourceHash) 'deploy: passed GameDir overrides SCHEDULE1_PATH'
Remove-Item Env:\SCHEDULE1_PATH -ErrorAction SilentlyContinue

# Game-running guard inside the deploy helper (fake provider, no real process).
$game5 = New-FakeInstall 'game5'
function Get-S1GameProcesses { @([pscustomobject]@{ ProcessName = 'Schedule I'; Id = 777 }) }
$runningDeployCode = -2
try {
    Invoke-S1ThirdPartyDeploy -GameDir $game5 -ThirdPartyRoot $tp -AllowlistPath $oneAllowPath -MutexTimeoutSeconds 5 | Out-Null
}
catch { $runningDeployCode = Get-CodeFromException $_.Exception }
Assert-True ($runningDeployCode -eq 4) 'deploy: running-game guard rejects before any copy'
Assert-True (@(Get-ChildItem (Join-Path $game5 'Mods') -File).Count -eq 0) 'deploy: running-game guard copied nothing'
Set-Item -Path Function:\Get-S1GameProcesses -Value $originalProvider

# Mutex timeout must fail, not report a skipped-but-successful deployment.
# Hold the shared (mod + third-party) mutex for this exact GameDir.
$game6 = New-FakeInstall 'game6'
$sharedMutexName = Get-S1ThirdPartyMutexName -GameDir $game6
$heldMutex = [System.Threading.Mutex]::new($false, $sharedMutexName)
[void]$heldMutex.WaitOne()
try {
    $mutexResult = Invoke-Child -Script $deployPath -Arguments @('-GameDir', $game6, '-ThirdPartyRoot', $tp, '-AllowlistPath', $oneAllowPath, '-MutexTimeoutSeconds', '1')
}
finally {
    $heldMutex.ReleaseMutex()
    $heldMutex.Dispose()
}
Assert-True ($mutexResult.ExitCode -ne 0 -and $mutexResult.Output -match 'mutex') 'deploy: shared mutex timeout fails instead of skipping with success'
Assert-True (@(Get-ChildItem (Join-Path $game6 'Mods') -File).Count -eq 0) 'deploy: mutex timeout copied nothing'

# NoDeploy semantics.
$game7 = New-FakeInstall 'game7'
$noDeploySwitch = Invoke-Child -Script $deployPath -Arguments @('-GameDir', $game7, '-ThirdPartyRoot', $tp, '-AllowlistPath', $oneAllowPath, '-NoDeploy')
Assert-True ($noDeploySwitch.ExitCode -eq 0 -and @(Get-ChildItem (Join-Path $game7 'Mods') -File).Count -eq 0) 'deploy: -NoDeploy skips all copies'

$env:S1NoDeploy = 'true'
$noDeployEnv = Invoke-Child -Script $deployPath -Arguments @('-GameDir', $game7, '-ThirdPartyRoot', $tp, '-AllowlistPath', $oneAllowPath)
Remove-Item Env:\S1NoDeploy -ErrorAction SilentlyContinue
Assert-True ($noDeployEnv.ExitCode -eq 0 -and @(Get-ChildItem (Join-Path $game7 'Mods') -File).Count -eq 0) 'deploy: S1NoDeploy=true env skips all copies'

# ------------------------------------------------ allowlist pattern accuracy
# A typo next to a valid pattern must fail naming the bad pattern and must not
# copy the valid one (selection fails before any install write).
$game8 = New-FakeInstall 'game8'
$typoAllowPath = Set-Allowlist -ThirdPartyRoot $tp -Lines @('a/ModA.dll', 'a/ModA-Typo.dll') -Name 'typo.deployallow'
$typo = Invoke-Child -Script $deployPath -Arguments @('-GameDir', $game8, '-ThirdPartyRoot', $tp, '-AllowlistPath', $typoAllowPath)
Assert-True ($typo.ExitCode -ne 0 -and $typo.Output -match 'a/ModA-Typo\.dll') 'deploy: unmatched allowlist pattern fails and names the pattern'
Assert-True (@(Get-ChildItem (Join-Path $game8 'Mods') -File).Count -eq 0) 'deploy: unmatched pattern copied nothing (not even the valid pattern)'

# ------------------------------------------------------ reserved mod DLL names
# Source/Mods/Shared.csproj reserves Shared.dll even with identical bytes.
$tpReserved = Join-Path $root 'thirdparty-reserved'
New-Item -ItemType Directory -Path (Join-Path $tpReserved 'shared') -Force | Out-Null
[IO.File]::WriteAllBytes((Join-Path $tpReserved 'shared\Shared.dll'), [Text.Encoding]::UTF8.GetBytes('fake-shared'))
[IO.File]::WriteAllBytes((Join-Path $tpReserved 'shared\NotReserved.dll'), [Text.Encoding]::UTF8.GetBytes('fake-not-reserved'))
$game9 = New-FakeInstall 'game9'
$reservedAllowPath = Set-Allowlist -ThirdPartyRoot $tpReserved -Lines @('shared/Shared.dll') -Name 'reserved.deployallow'
$reserved = Invoke-Child -Script $deployPath -Arguments @('-GameDir', $game9, '-ThirdPartyRoot', $tpReserved, '-AllowlistPath', $reservedAllowPath)
Assert-True ($reserved.ExitCode -ne 0 -and $reserved.Output -match 'reserved by a workspace mod' -and $reserved.Output -match 'Shared\.dll') 'deploy: workspace mod DLL name collision fails even when bytes match'
Assert-True (-not (Test-Path (Join-Path $game9 'Mods\Shared.dll'))) 'deploy: reserved-name collision copied nothing'

# ------------------------------------- existing DLLs are never overwritten
# Foreign shared.dll already in Mods with different bytes: refuse, no partial
# copy of the other selected DLL.
$game10 = New-FakeInstall 'game10'
$foreignBytes = [Text.Encoding]::UTF8.GetBytes('owner-managed-foreign')
[IO.File]::WriteAllBytes((Join-Path $game10 'Mods\ModA.dll'), $foreignBytes)
$foreignHashBefore = (Get-FileHash (Join-Path $game10 'Mods\ModA.dll') -Algorithm SHA256).Hash
$twoAllowPath = Set-Allowlist -ThirdPartyRoot $tp -Lines @('a/ModA.dll', 'b/ModB.dll') -Name 'foreign.deployallow'
$foreign = Invoke-Child -Script $deployPath -Arguments @('-GameDir', $game10, '-ThirdPartyRoot', $tp, '-AllowlistPath', $twoAllowPath)
$foreignHashAfter = (Get-FileHash (Join-Path $game10 'Mods\ModA.dll') -Algorithm SHA256).Hash
Assert-True ($foreign.ExitCode -ne 0 -and $foreign.Output -match 'owner-managed|manual action') 'deploy: existing foreign DLL is refused with manual-update guidance'
Assert-True ($foreignHashBefore -eq $foreignHashAfter) 'deploy: foreign DLL bytes untouched'
Assert-True (-not (Test-Path (Join-Path $game10 'Mods\ModB.dll'))) 'deploy: refused plan copies nothing (no partial deployment)'

# Changed third-party DLL in place: refuse and preserve its bytes.
$game11 = New-FakeInstall 'game11'
[IO.File]::WriteAllBytes((Join-Path $game11 'Mods\ModB.dll'), [Text.Encoding]::UTF8.GetBytes('stale-thirdparty'))
$staleHashBefore = (Get-FileHash (Join-Path $game11 'Mods\ModB.dll') -Algorithm SHA256).Hash
$onlyBAllowPath = Set-Allowlist -ThirdPartyRoot $tp -Lines @('b/ModB.dll') -Name 'changed.deployallow'
$changed = Invoke-Child -Script $deployPath -Arguments @('-GameDir', $game11, '-ThirdPartyRoot', $tp, '-AllowlistPath', $onlyBAllowPath)
$staleHashAfter = (Get-FileHash (Join-Path $game11 'Mods\ModB.dll') -Algorithm SHA256).Hash
Assert-True ($changed.ExitCode -ne 0 -and $changed.Output -match 'different content') 'deploy: changed existing third-party DLL is refused'
Assert-True ($staleHashBefore -eq $staleHashAfter) 'deploy: changed third-party DLL bytes untouched'

# Byte-identical existing DLL: clean no-op.
$game12 = New-FakeInstall 'game12'
Copy-Item -LiteralPath (Join-Path $tp 'a\ModA.dll') -Destination (Join-Path $game12 'Mods\ModA.dll')
$sameAllowPath = Set-Allowlist -ThirdPartyRoot $tp -Lines @('a/ModA.dll') -Name 'same.deployallow'
$same = Invoke-Child -Script $deployPath -Arguments @('-GameDir', $game12, '-ThirdPartyRoot', $tp, '-AllowlistPath', $sameAllowPath)
Assert-True ($same.ExitCode -eq 0 -and $same.Output -match 'same\s+a/ModA.dll') 'deploy: byte-identical existing DLL is a no-op'
Assert-True (@(Get-ChildItem (Join-Path $game12 'Mods') -File).Count -eq 1) 'deploy: no-op left no staging files behind'

# Empty allowlist must not uninstall previously copied DLLs, and must say so.
$emptyAgainPath = Set-Allowlist -ThirdPartyRoot $tp -Lines @('# empty') -Name 'empty.deployallow'
$emptyAgain = Invoke-Child -Script $deployPath -Arguments @('-GameDir', $game12, '-ThirdPartyRoot', $tp, '-AllowlistPath', $emptyAgainPath)
Assert-True ($emptyAgain.ExitCode -eq 0 -and (Test-Path (Join-Path $game12 'Mods\ModA.dll'))) 'deploy: empty allowlist leaves previously copied DLLs in place'
Assert-True ($emptyAgain.Output -match 'does NOT uninstall|not uninstall') 'deploy: empty allowlist warns it is not an uninstall'

# ------------------------------------------------- shared mutex contract
# Same name deploy-mod.ps1 uses: prefix + first 8 SHA256 bytes, lowercase hex.
function Get-ExpectedSharedMutexName {
    param([string]$Path)
    $normalized = [IO.Path]::TrimEndingDirectorySeparator([IO.Path]::GetFullPath($Path)).ToLowerInvariant()
    $sha = [Security.Cryptography.SHA256]::Create()
    try { $bytes = $sha.ComputeHash([Text.Encoding]::UTF8.GetBytes($normalized)) }
    finally { $sha.Dispose() }
    return 'Schedule1Deploy_' + (-join ($bytes[0..7] | ForEach-Object { $_.ToString('x2') }))
}
$game13 = New-FakeInstall 'game13'
$expectedName = Get-ExpectedSharedMutexName $game13
Assert-True ((Get-S1ThirdPartyMutexName -GameDir $game13) -eq $expectedName) 'deploy: third-party mutex matches Schedule1Deploy_ + first8 SHA256 hex'
Assert-True ((Get-S1ThirdPartyMutexName -GameDir ($game13 + [IO.Path]::DirectorySeparatorChar)) -eq $expectedName) 'deploy: mutex name trims trailing separator'
Assert-True ((Get-S1ThirdPartyMutexName -GameDir $game13.ToUpperInvariant()) -eq $expectedName) 'deploy: mutex name is case-insensitive (lowercased before hash)'

# Agreement with deploy-mod: only assert once deploy-mod has landed the shared
# prefix; otherwise report a skip so this suite can run during the transition.
$deployModPath = Join-Path $repo 'Tools/deploy-mod.ps1'
$modPrefix = $null
if (Test-Path -LiteralPath $deployModPath -PathType Leaf) {
    $prefixLine = Select-String -LiteralPath $deployModPath -Pattern "S1DeployMutexPrefix\s*=\s*'([^']+)'" | Select-Object -First 1
    if ($null -ne $prefixLine) { $modPrefix = $prefixLine.Matches[0].Groups[1].Value }
}
if ($modPrefix -eq 'Schedule1Deploy_') {
    # Run deploy-mod's own function in an isolated child process (never
    # dot-sourced into this suite) and compare the produced mutex name.
    $modMutexName = (& pwsh -NoProfile -NonInteractive -Command ". '$deployModPath'; Get-S1DeployMutexName -GameDir '$game13'" 2>&1 | Out-String).Trim()
    Assert-True ($modMutexName -eq $expectedName) 'deploy: mutex name agrees with deploy-mod.ps1 for the same GameDir'
}
else {
    Write-Host "SKIP  deploy: deploy-mod.ps1 still uses prefix '$modPrefix' (shared-prefix agreement test waits for that change)" -ForegroundColor Yellow
}

# -------------------------------------------------------------- build-all
. $buildAllPath

$savedEnv = $env:S1NoDeploy
$env:S1NoDeploy = $null
Assert-True ((Get-S1EffectiveNoDeploy -NoDeploySwitch $false -EnvironmentValue $null) -eq $false) 'build-all: auto-deploy is the default'
Assert-True ((Get-S1EffectiveNoDeploy -NoDeploySwitch $true -EnvironmentValue $null) -eq $true) 'build-all: -NoDeploy disables deploy'
Assert-True ((Get-S1EffectiveNoDeploy -NoDeploySwitch $false -EnvironmentValue 'true') -eq $true) 'build-all: existing S1NoDeploy=true is respected'
Assert-True ((Get-S1EffectiveNoDeploy -NoDeploySwitch $true -EnvironmentValue 'false') -eq $true) 'build-all: -NoDeploy wins over env=false'
$env:S1NoDeploy = $savedEnv

$argsNoDeploy = New-S1BuildArguments -Solution 'C:\repo\S1Mods.sln' -Config 'Release' -NoDeploy $true -GameDir 'D:\Games\Schedule I'
Assert-True ($argsNoDeploy[0] -eq 'build' -and $argsNoDeploy[1] -eq 'C:\repo\S1Mods.sln') 'build-all: passes dotnet build verb and solution'
Assert-True ($argsNoDeploy -contains '-p:S1NoDeploy=true') 'build-all: passes effective no-deploy to MSBuild'
Assert-True ($argsNoDeploy -contains '-p:GameDir=D:\Games\Schedule I') 'build-all: passes explicit GameDir to MSBuild'
$argsDeploy = New-S1BuildArguments -Solution 'C:\repo\S1Mods.sln' -Config 'Debug' -NoDeploy $false -GameDir ''
Assert-True (($argsDeploy -contains '-p:S1NoDeploy=false') -and -not ($argsDeploy -match '^-p:GameDir=')) 'build-all: deploy default without GameDir override'

$dotnet = Resolve-S1DotnetPath -Requested $null
$sdks = Assert-S1DotnetSdk -DotnetPath $dotnet
Assert-True (@($sdks).Count -gt 0) 'build-all: dotnet SDK fallback/preflight resolves an SDK'

# End-to-end build-all in a throwaway repo with an empty solution. This still
# exercises the real script (SDK resolution, preflight, MSBuild property) but
# never builds the actual S1Mods.sln.
$fakeRepo = Join-Path $root 'fake-repo'
New-Item -ItemType Directory -Path (Join-Path $fakeRepo 'Tools') -Force | Out-Null
New-Item -ItemType Directory -Path (Join-Path $fakeRepo 'Source\Mods') -Force | Out-Null
Copy-Item -LiteralPath $buildAllPath -Destination (Join-Path $fakeRepo 'Tools\build-all.ps1')
Copy-Item -LiteralPath $preflightPath -Destination (Join-Path $fakeRepo 'Tools\check-deploy-preflight.ps1')
& $dotnet new sln -n S1Mods -o (Join-Path $fakeRepo 'Source\Mods') --force 2>&1 | Out-Null
$fakeBuildAll = Join-Path $fakeRepo 'Tools\build-all.ps1'
$fakeSln = Join-Path $fakeRepo 'Source\Mods\S1Mods.sln'
# SDK 10 defaults `dotnet new sln` to the .slnx template, so a runner with a newer
# default SDK creates S1Mods.slnx and every e2e assertion below fails on a missing
# file. Ask for the classic format explicitly; SDK 8 rejects the switch, in which
# case its (already classic) output from the call above is correct.
if (-not (Test-Path -LiteralPath $fakeSln)) {
    & $dotnet new sln -n S1Mods -o (Join-Path $fakeRepo 'Source\Mods') --force --format sln 2>&1 | Out-Null
}
# Precondition: without the scratch solution every e2e assertion below fails for
# the same uninteresting reason, so assert it on its own.
Assert-True (Test-Path -LiteralPath $fakeSln) 'build-all e2e: fake solution created' -Detail ((& $dotnet --version 2>&1 | Out-String) + "sln: $fakeSln")

$buildNoDeploy = Invoke-Child -Script $fakeBuildAll -Arguments @('-NoDeploy')
Assert-True ($buildNoDeploy.ExitCode -eq 0 -and $buildNoDeploy.Output -match 'disabled \(NoDeploy\)') 'build-all e2e: -NoDeploy builds without deploy' -Detail $buildNoDeploy.Output

$fakeBuildGame = New-FakeInstall 'build-game'
$buildDefault = Invoke-Child -Script $fakeBuildAll -Arguments @('-GameDir', $fakeBuildGame)
Assert-True ($buildDefault.ExitCode -eq 0 -and $buildDefault.Output -match 'enabled \(default\)') 'build-all e2e: auto-deploy default still builds' -Detail $buildDefault.Output

$buildBadGame = Invoke-Child -Script $fakeBuildAll -Arguments @('-GameDir', (Join-Path $root 'not-a-game'))
Assert-True ($buildBadGame.ExitCode -ne 0 -and $buildBadGame.Output -match 'preflight failed') 'build-all e2e: preflight failure aborts before build' -Detail $buildBadGame.Output

# Real MSBuild import: destination overrides must not escape the validated root.
$targetGame = New-FakeInstall 'target-game'
$targetProject = Join-Path $root 'BoundTarget.proj'
$targetSource = Join-Path $root 'BoundTarget.dll'
[IO.File]::WriteAllText($targetSource, 'fixture-only')
$targetsFile = [System.Security.SecurityElement]::Escape((Join-Path $repo 'Source/Mods/Directory.Build.targets'))
[IO.File]::WriteAllText($targetProject, "<Project><Import Project=`"$targetsFile`" /></Project>")
$wrongMods = Join-Path $root 'wrong-mods'
$wrongData = Join-Path $root 'wrong-data'
& $dotnet msbuild $targetProject -t:DeployMod "-p:GameDir=$targetGame" "-p:TargetPath=$targetSource" "-p:ModsDir=$wrongMods" "-p:UserDataDir=$wrongData" -nologo
Assert-True ($LASTEXITCODE -eq 0) 'targets: real deploy target accepts fake install'
Assert-True (Test-Path (Join-Path $targetGame 'Mods/BoundTarget.dll')) 'targets: DLL lands under checked GameDir'
Assert-True (-not (Test-Path $wrongMods) -and -not (Test-Path $wrongData)) 'targets: independent destination overrides cannot redirect writes'

# ---------------------------------------------------------------- summary
Write-Host ''
Write-Host "Passed: $script:Passed | Failed: $script:Failed" -ForegroundColor Cyan
if ($script:Failed -gt 0) { exit 1 }
exit 0
