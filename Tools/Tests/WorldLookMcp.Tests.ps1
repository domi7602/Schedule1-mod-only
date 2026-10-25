#Requires -Version 7
# Synthetic streams and stubbed bridge responses only. Never connects to the game.
[CmdletBinding()]
param([string]$ScratchRoot)
$ErrorActionPreference = 'Stop'
$repo = Split-Path (Split-Path $PSScriptRoot -Parent) -Parent
. (Join-Path $repo 'Tools/invoke-worldlook-mcp.ps1')
if (-not $ScratchRoot) { $ScratchRoot = Join-Path ([IO.Path]::GetTempPath()) 's1-worldlook-mcp-tests' }
$ScratchRoot = [IO.Path]::GetFullPath($ScratchRoot)
$scratch = Join-Path $ScratchRoot ([guid]::NewGuid().ToString('N'))
$null = [IO.Directory]::CreateDirectory($scratch)
$script:passed = 0
$script:failed = 0
function Check([bool]$Condition, [string]$Message) {
    if ($Condition) { $script:passed++; Write-Host "PASS: $Message" }
    else { $script:failed++; Write-Host "FAIL: $Message" -ForegroundColor Red }
}
function Expect-Failure([scriptblock]$Action, [string]$Pattern, [string]$Message) {
    $failure = ''
    try { $null = & $Action } catch { $failure = $_.Exception.Message }
    Check ($failure -match $Pattern) "$Message ($failure)"
}
function New-Stream($Messages) {
    $stream = [IO.MemoryStream]::new()
    foreach ($message in $Messages) { Write-S1McpFrame $stream $message }
    $stream.Position = 0
    return $stream
}
$context = @{ EvidencePath = Join-Path $scratch 'frames.jsonl'; DeadlineUtc = [DateTime]::UtcNow.AddSeconds(60);
    GameExe = '/fixture/Schedule I.exe'; Process = @{ Id = 42; Path = '/fixture/Schedule I.exe'; StartedUtc = '2026-10-25T10:00:00.0000000Z' } }
$deadline = [DateTime]::UtcNow.AddSeconds(3)
$stream = New-Stream @(@{ id = -1; result = @{ type = 'server_heartbeat'; status = 'alive' } }, @{ id = 123; result = @{ available = $true } })
$response = Read-S1McpResponse $stream 123 $deadline $context
Check ($response.id -eq 123 -and $response.result.available) 'heartbeat is skipped, correlated response retained'
Check ((Get-Content -LiteralPath $context.EvidencePath -Raw) -match 'server_heartbeat') 'heartbeat preserved in evidence'
$stream.Dispose()
$stream = New-Stream @(@{ id = 124; result = @{} })
Expect-Failure { Read-S1McpResponse $stream 123 ([DateTime]::UtcNow.AddSeconds(3)) $context } 'ID mismatch' 'uncorrelated response is rejected'
$stream.Dispose()
Check ((Get-Content -LiteralPath $context.EvidencePath -Raw) -match 'uncorrelatedResponse') 'wrong response preserved for diagnosis'
$stream = New-Stream @(@{ id = '123'; result = @{} })
Expect-Failure { Read-S1McpResponse $stream 123 ([DateTime]::UtcNow.AddSeconds(3)) $context } 'ID mismatch' 'string response ID cannot silently coerce to the request ID'
$stream.Dispose()
$stream = New-Stream @(@{ id = -2; result = @{ type = 'server_heartbeat'; status = 'unknown' } })
Expect-Failure { Read-S1McpResponse $stream 123 ([DateTime]::UtcNow.AddSeconds(3)) $context } 'ID mismatch' 'only exact heartbeat notification is skipped'
$stream.Dispose()
$stream = New-Stream @(1..8 | ForEach-Object { @{ id = -1; result = @{ type = 'server_heartbeat'; status = 'alive' } } })
Expect-Failure { Read-S1McpResponse $stream 123 ([DateTime]::UtcNow.AddSeconds(3)) $context } 'frame budget' 'endless heartbeats have a bounded frame budget'
$stream.Dispose()
foreach ($length in @(0, 1048577)) {
    $stream = [IO.MemoryStream]::new([BitConverter]::GetBytes([uint32]$length))
    Expect-Failure { Read-S1McpResponse $stream 123 ([DateTime]::UtcNow.AddSeconds(3)) $context } 'length' "invalid frame length $length is rejected before payload allocation"
    $stream.Dispose()
}
$stream = [IO.MemoryStream]::new([byte[]]@(1, 2))
Expect-Failure { Read-S1McpBytes $stream 4 ([DateTime]::UtcNow.AddSeconds(3)) } 'closed before' 'truncated frame fails instead of returning partial bytes'
$stream.Position = 0
Expect-Failure { Read-S1McpBytes $stream 1 ([DateTime]::UtcNow.AddSeconds(-1)) } 'deadline' 'expired response deadline blocks reads'
$stream.Dispose()
Expect-Failure { Assert-S1McpProcess $context @{ Id=43; Path=$context.GameExe; StartedUtc=$context.Process.StartedUtc } } 'identity changed' 'PID change is rejected'
Expect-Failure { Assert-S1McpProcess $context @{ Id=42; Path=$context.GameExe; StartedUtc='2026-10-25T11:00:00.0000000Z' } } 'identity changed' 'PID reuse with a different process start is rejected'
Expect-Failure { Assert-S1McpProcess $context @{ Id=42; Path='/other/Schedule I.exe'; StartedUtc=$context.Process.StartedUtc } } 'another installation' 'same-named process from another game installation is rejected'
Expect-Failure { Invoke-S1McpRequest $context 'execute_code' @{} } 'Unsupported MCP method' 'arbitrary MCP execution is blocked before transport'
Expect-Failure { Invoke-S1McpRequest $context 'call_method' @{object_name='WorldLook.Automation'; component_type='WorldLook.WorldLookAutomation'; method_name='Destroy'} } 'parameterless WorldLook' 'arbitrary component method is blocked before transport'
Expect-Failure { Invoke-S1McpRequest $context 'call_method' @{object_name='Other'; component_type='WorldLook.WorldLookAutomation'; method_name='GetStatus'} } 'parameterless WorldLook' 'another Unity object is blocked before transport'
Expect-Failure { Invoke-S1McpRequest $context 'call_method' @{object_name='WorldLook.Automation'; component_type='WorldLook.WorldLookAutomation'; method_name='EnableLook'; parameters=@($true)} } 'parameterless WorldLook' 'parameter arrays are blocked before transport'

$savePath = Join-Path $scratch 'SaveGame_1'
function New-Status([bool]$Enabled = $true, [string]$Scene = 'Main') {
    return @{ schemaVersion=1; action='GetStatus'; accepted=$true; changed=$false; error=$null;
        ready=($Scene -eq 'Main'); requestedEnabled=$Enabled; singleplayer=($Scene -eq 'Main');
        hostOrSingleplayer=($Scene -eq 'Main'); gameLoaded=($Scene -eq 'Main'); loading=$false;
        scene=$Scene; saveName='Fixture'; savePath=$savePath; lightingFaulted=$false; waterFaulted=$false }
}
$status = New-Status
Assert-S1WorldLookSave $status 'Fixture' $savePath
Check $true 'exact ready singleplayer save is accepted'
foreach ($key in @('ready', 'singleplayer', 'hostOrSingleplayer', 'gameLoaded')) {
    $invalid = New-Status
    $invalid[$key] = $false
    Expect-Failure { Assert-S1WorldLookSave $invalid 'Fixture' $savePath } 'ready singleplayer' "$key false blocks mutation"
}
$invalid = New-Status
$invalid.loading = $true
Expect-Failure { Assert-S1WorldLookSave $invalid 'Fixture' $savePath } 'ready singleplayer' 'loading blocks mutation'
Expect-Failure { Assert-S1WorldLookSave $status 'Other' $savePath } 'save identity' 'save name mismatch blocks mutation'
Expect-Failure { Assert-S1WorldLookSave $status 'Fixture' (Join-Path $scratch 'SaveGame_2') } 'save identity' 'save path mismatch blocks mutation'
$save = @{ slot_index=0; organisationName='Fixture'; savePath=$savePath }
Check ((Find-S1WorldLookSave @($save) 'Fixture' $savePath) -eq 0) 'save selection keeps zero-based slot zero'
Expect-Failure { Find-S1WorldLookSave @($save, $save) 'Fixture' $savePath } 'ambiguous' 'duplicate save identity blocks load'
Expect-Failure { Find-S1WorldLookSave @() 'Fixture' $savePath } 'missing' 'missing save identity blocks load'
$invalidSave = $save.Clone()
$invalidSave.slot_index = '0'
Expect-Failure { Find-S1WorldLookSave @($invalidSave) 'Fixture' $savePath } 'invalid slot' 'string slot cannot silently coerce to zero'

# Replace the transport seam only after framing tests; no sockets are opened anywhere.
function Reset-Responses($Items) {
    $script:responses = [Collections.Generic.Queue[object]]::new()
    foreach ($item in $Items) { $script:responses.Enqueue($item) }
    $script:requests = [Collections.Generic.List[object]]::new()
}
function Endpoint($Status, [string]$Method = 'GetStatus') {
    return @{ method_name=$Method; return_value=@{ type='String'; value=($Status | ConvertTo-Json -Compress -Depth 10) } }
}
function Invoke-S1McpRequest($Context, [string]$Method, [hashtable]$Parameters = @{}) {
    $script:requests.Add(@{ method=$Method; params=$Parameters })
    if ($script:responses.Count -eq 0) { throw 'Unexpected extra request (possible retry).' }
    $next = $script:responses.Dequeue()
    if ($next -is [string] -or $next -is [Exception]) { throw $next }
    if ($next -is [scriptblock]) { return (& $next) }
    return $next
}

Reset-Responses @(@{ method_name='DisableLook'; return_value=@{ type='null'; value='null' } })
Expect-Failure { Invoke-S1WorldLookEndpoint $context 'DisableLook' } 'no explicit JSON' 'silent null is not a successful mutation'
$rejected = New-Status
$rejected.accepted = $false
$rejected.error = 'loading'
Reset-Responses @((Endpoint $rejected 'DisableLook'))
Expect-Failure { Invoke-S1WorldLookEndpoint $context 'DisableLook' } 'rejected.*loading' 'endpoint rejection is preserved'
$invalid = New-Status
$invalid.requestedEnabled = 'false'
Reset-Responses @((Endpoint $invalid))
Expect-Failure { Invoke-S1WorldLookEndpoint $context } 'schema' 'string boolean is rejected rather than treated as true'

$off = New-Status $false
Reset-Responses @((Endpoint (New-Status)), (Endpoint $off 'DisableLook'), (Endpoint $off))
$result = Invoke-S1WorldLookAction $context 'Off' 'Fixture' $savePath $null
Check (-not $result.requestedEnabled -and $script:requests.Count -eq 3) 'OFF uses one explicit mutation and independent status readback'
Check ($script:requests[1].params.method_name -eq 'DisableLook' -and -not $script:requests[1].params.ContainsKey('parameters')) 'mutation uses the parameterless bridge-compatible wrapper'
Reset-Responses @((Endpoint $off), (Endpoint $off 'DisableLook'), (Endpoint $off))
$null = Invoke-S1WorldLookAction $context 'Off' 'Fixture' $savePath $null
Check ($script:requests.Count -eq 3) 'already OFF still verifies an idempotent explicit setter'
Reset-Responses @((Endpoint (New-Status)), (Endpoint $off 'DisableLook'), (Endpoint (New-Status)))
Expect-Failure { Invoke-S1WorldLookAction $context 'Off' 'Fixture' $savePath $null } 'readback failed' 'accepted setter followed by wrong state is rejected'
Reset-Responses @((Endpoint (New-Status)), 'unknown outcome timeout')
Expect-Failure { Invoke-S1WorldLookAction $context 'Off' 'Fixture' $savePath $null } 'unknown outcome' 'mutation timeout is propagated'
Check ($script:requests.Count -eq 2) 'unknown outcome does not retry mutation'

$menu = New-Status $false 'Menu'
$menuAccepted = $menu.Clone()
Reset-Responses @((Endpoint $off), (Endpoint $menuAccepted 'ReturnToMenu'), (Endpoint $menu), @{scene_name='Menu'}, @{available=$false})
$result = Invoke-S1WorldLookAction $context 'Menu' 'Fixture' $savePath $null
Check ($result.scene -eq 'Menu' -and -not $result.requestedEnabled) 'menu confirms endpoint, native scene and absent clock while preserving OFF'
Reset-Responses @((Endpoint $menu), @{scene_name='Menu'}, @{available=$false}, @{saves=@($save)}, @{success=$true;slot_index=0;returned_to_menu=$false}, (Endpoint $off))
$result = Invoke-S1WorldLookAction $context 'Load' 'Fixture' $savePath $null
Check ($result.ready -and -not $result.requestedEnabled) 'load verifies actual ready save and retained OFF'
Check (@($script:requests | Where-Object method -eq 'load_save').Count -eq 1) 'load is dispatched once'
Reset-Responses @((Endpoint $menu), @{scene_name='Menu'}, @{available=$false}, @{saves=@($save)}, @{success=$true;slot_index=0;returned_to_menu=$false}, (Endpoint (New-Status)))
Expect-Failure { Invoke-S1WorldLookAction $context 'Load' 'Fixture' $savePath $null } 'changed across reload' 'reload cannot reset OFF to ON unnoticed'
Reset-Responses @((Endpoint $off))
Expect-Failure { Invoke-S1WorldLookAction $context 'Load' 'Fixture' $savePath $null } 'actual main menu' 'load cannot silently switch from gameplay to menu'
Reset-Responses @((Endpoint $menu), @{scene_name='Menu'}, @{available='false'})
Expect-Failure { Invoke-S1WorldLookAction $context 'Load' 'Fixture' $savePath $null } 'readiness' 'string clock availability cannot authorize loading'
Reset-Responses @((Endpoint $menu), @{scene_name='Menu'}, @{available=$false}, @{saves=@($save)}, @{success='true';slot_index=0;returned_to_menu=$false})
Expect-Failure { Invoke-S1WorldLookAction $context 'Load' 'Fixture' $savePath $null } 'not confirmed' 'string success flag cannot confirm loading'

Reset-Responses @((Endpoint $off), @{available=$true;readback_hhmm=1800}, @{available=$true;time_hhmm=1802;time_minutes=1082}, (Endpoint $off))
$result = Invoke-S1WorldLookAction $context 'Time' 'Fixture' $savePath 1800
Check ($result.appliedHhmm -eq 1800 -and $result.clock.time_minutes -eq 1082 -and $result.after.savePath -eq $savePath) 'time checks explicit HHMM, independent clock and unchanged save'
Reset-Responses @((Endpoint $off), @{available=$true;readback_hhmm=1800}, @{available=$true;time_minutes=1100})
Expect-Failure { Invoke-S1WorldLookAction $context 'Time' 'Fixture' $savePath 1800 } 'sampling window' 'unrelated clock readback is rejected'
foreach ($invalidMinutes in @(-360, 2520, '1080')) {
    Reset-Responses @((Endpoint $off), @{available=$true;readback_hhmm=1800}, @{available=$true;time_minutes=$invalidMinutes})
    Expect-Failure { Invoke-S1WorldLookAction $context 'Time' 'Fixture' $savePath 1800 } 'sampling window' "invalid minute value $invalidMinutes cannot pass modulo comparison"
}
Reset-Responses @((Endpoint $off), @{available='true';readback_hhmm=1800})
Expect-Failure { Invoke-S1WorldLookAction $context 'Time' 'Fixture' $savePath 1800 } 'setter readback' 'string availability cannot confirm a time write'
Reset-Responses @((Endpoint $off), @{available=$true;readback_hhmm=1800}, @{available=$true;time_minutes=1080}, (Endpoint $menu))
Expect-Failure { Invoke-S1WorldLookAction $context 'Time' 'Fixture' $savePath 1800 } 'ready singleplayer' 'scene change during clock verification is rejected'
Reset-Responses @((Endpoint $off))
Expect-Failure { Invoke-S1WorldLookAction $context 'Time' 'Fixture' $savePath 1860 } 'valid HHMM' 'invalid HHMM fails before time mutation'
Check ($script:requests.Count -eq 1) 'invalid HHMM performs no time write'

$expired = $context.Clone()
$expired.DeadlineUtc = [DateTime]::UtcNow.AddSeconds(-1)
Reset-Responses @()
Expect-Failure { Wait-S1WorldLookStatus $expired { $false } } 'timed out' 'transition deadline terminates without a mutation retry'
Check ($script:requests.Count -eq 0) 'expired transition does not issue further requests'

$socketTimeout = [IO.IOException]::new('synthetic read failure', [Net.Sockets.SocketException]::new(10060))
Check (Test-S1McpTransportTimeout $socketTimeout) 'nested socket timeout is recognized without message matching'
Check (-not (Test-S1McpTransportTimeout ([TimeoutException]::new('not a transport timeout')))) 'unmarked timeout does not authorize another poll'
Check (-not (Test-S1McpTransportTimeout ([IO.IOException]::new('synthetic reset', [Net.Sockets.SocketException]::new(10054))))) 'connection reset is not retried as a timeout'
$pollContext = $context.Clone()
$pollContext.DeadlineUtc = [DateTime]::UtcNow.AddSeconds(5)
Reset-Responses @($socketTimeout, (Endpoint $off))
$result = Wait-S1WorldLookStatus $pollContext { param($s) $s.ready }
Check ($result.ready -and $script:requests.Count -eq 2) 'confirmed status read timeout is followed by a fresh successful read'
Check (@($script:requests | Where-Object { $_.method -ne 'call_method' -or $_.params.method_name -ne 'GetStatus' }).Count -eq 0) 'timeout recovery dispatches only GetStatus'
Check ((Get-Content -LiteralPath $context.EvidencePath -Raw) -match 'readOnlyPollTimedOutUtc') 'read timeout remains in diagnostic evidence'
Reset-Responses @((Endpoint $menu), @{scene_name='Menu'}, @{available=$false}, @{saves=@($save)}, @{success=$true;slot_index=0;returned_to_menu=$false}, $socketTimeout, (Endpoint $off))
$null = Invoke-S1WorldLookAction $pollContext 'Load' 'Fixture' $savePath $null
Check (@($script:requests | Where-Object method -eq 'load_save').Count -eq 1) 'load timeout recovery does not repeat load_save'
Reset-Responses @((Endpoint (New-Status)), $socketTimeout)
Expect-Failure { Invoke-S1WorldLookAction $pollContext 'Off' 'Fixture' $savePath $null } 'synthetic read failure' 'the same transport timeout on a mutation is propagated'
Check ($script:requests.Count -eq 2) 'timed-out mutation is never repeated'
Reset-Responses @((Endpoint @{schemaVersion=1;accepted=$true}))
Expect-Failure { Wait-S1WorldLookStatus $pollContext { $true } } 'schema' 'status schema failure is never retried'
Check ($script:requests.Count -eq 1) 'schema failure stops after one status request'
Reset-Responses @('Game process identity changed or the bridge belongs to another installation.')
Expect-Failure { Wait-S1WorldLookStatus $pollContext { $true } } 'identity changed' 'process identity failure is never retried'
Check ($script:requests.Count -eq 1) 'process failure stops after one status request'
$shortContext = $context.Clone()
$shortContext.DeadlineUtc = [DateTime]::UtcNow.AddSeconds(5)
Reset-Responses @({
    $shortContext.DeadlineUtc = [DateTime]::UtcNow.AddMilliseconds(-1)
    throw (New-S1McpTransportTimeout 'MCP response deadline exceeded.')
})
Expect-Failure { Wait-S1WorldLookStatus $shortContext { $true } } 'state transition timed out' 'transport timeout cannot extend the global transition deadline'
Check ($script:requests.Count -eq 1) 'expired global deadline prevents another read attempt'

# Verify top-level session selection and append-only evidence without touching a port.
$fixtureGame = Join-Path $scratch 'game'
$script:fixtureProcess = @{ Id=42; Path=(Join-Path $fixtureGame 'Schedule I.exe'); StartedUtc='2026-10-25T10:00:00.0000000Z' }
function Get-S1McpProcess { return $script:fixtureProcess }
$evidence = Join-Path $scratch 'status.jsonl'
Reset-Responses @((Endpoint (New-Status)))
$result = Invoke-S1WorldLook -Action Status -EvidencePath $evidence -GameDir $fixtureGame -GameProcessId 42 -GameStartedUtc '2026-10-25T10:00:00Z'
Check ($result.process.Id -eq 42 -and (Test-Path -LiteralPath $evidence)) 'top-level result returns pinned session and fresh evidence path'
$originalHash = (Get-FileHash -LiteralPath $evidence).Hash
Expect-Failure { Invoke-S1WorldLook -Action Status -EvidencePath $evidence -GameDir $fixtureGame } 'exist|already' 'existing evidence cannot be overwritten'
Check ((Get-FileHash -LiteralPath $evidence).Hash -eq $originalHash) 'existing evidence bytes remain unchanged'
Expect-Failure { Invoke-S1WorldLook -Action Status -EvidencePath (Join-Path $scratch 'wrong-pid.jsonl') -GameDir $fixtureGame -GameProcessId 43 } 'no longer running' 'caller-specified old PID is rejected before requests'
Expect-Failure { Invoke-S1WorldLook -Action Status -EvidencePath (Join-Path $scratch 'wrong-start.jsonl') -GameDir $fixtureGame -GameStartedUtc '2026-10-25T11:00:00Z' } 'no longer running' 'caller-specified old start time is rejected before requests'
Check (-not (Test-Path -LiteralPath (Join-Path $scratch 'wrong-pid.jsonl')) -and -not (Test-Path -LiteralPath (Join-Path $scratch 'wrong-start.jsonl'))) 'session mismatch creates no false acceptance artifact'
Expect-Failure { Invoke-S1WorldLook -Action Off -EvidencePath (Join-Path $scratch 'missing-save.jsonl') -GameDir $fixtureGame -SaveName Fixture } 'SaveName and absolute SavePath' 'missing save path fails with an explicit mutation guard'
Reset-Responses @('synthetic unknown response')
$failurePath = Join-Path $scratch 'failure.jsonl'
Expect-Failure { Invoke-S1WorldLook -Action Status -EvidencePath $failurePath -GameDir $fixtureGame } 'synthetic unknown' 'top-level failure is propagated'
Check ((Get-Content -LiteralPath $failurePath -Raw) -match 'failedUtc' -and $script:requests.Count -eq 1) 'failed action retains diagnostic evidence and does not retry'

Write-Host "WorldLook MCP synthetic checks: $script:passed passed, $script:failed failed. Fixtures: $scratch"
if ($script:failed -gt 0) { exit 1 }
exit 0
