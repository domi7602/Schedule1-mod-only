#Requires -Version 7
<#
.SYNOPSIS
    Perform one bounded WorldLook acceptance action through the local S1MCP bridge.
.DESCRIPTION
    No keyboard injection, screenshots, deployment, saving or automatic mutation retry.
    A fresh evidence JSONL records requests, correlated responses and failures. On/Off,
    Menu, Load and Time require the owner's exact SaveName and absolute SavePath.
    Calls parameterless WorldLook.Automation methods; S1MCP 1.0.5 does not reliably
    decode call_method parameter arrays. Load uses its dedicated load_save handler.
    Menu and Load wait for actual scene/readiness changes, not a success message.
    Keep GameProcessId and GameStartedUtc from Status on subsequent actions to test
    one process. Dot-sourcing defines functions only (used by synthetic tests).
.EXAMPLE
    pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/invoke-worldlook-mcp.ps1 `
      -Action Status -EvidencePath .scratch/worldlook/status.jsonl
#>
[CmdletBinding()]
param(
    [ValidateSet('Status', 'On', 'Off', 'Menu', 'Load', 'Time')][string]$Action = 'Status',
    [string]$EvidencePath,
    [string]$SaveName,
    [string]$SavePath,
    [Nullable[int]]$Hhmm,
    [string]$GameDir,
    [Nullable[int]]$GameProcessId,
    [string]$GameStartedUtc,
    [ValidateRange(5, 120)][int]$TimeoutSeconds = 60
)

function Write-S1McpEvidence($Context, $Record) {
    $Record | ConvertTo-Json -Depth 40 -Compress | Add-Content -LiteralPath $Context.EvidencePath -Encoding utf8
}

function Get-S1McpProcess {
    $listeners = @(Get-NetTCPConnection -LocalAddress '127.0.0.1' -LocalPort 8765 -State Listen -ErrorAction Stop)
    if ($listeners.Count -ne 1) { throw 'Expected exactly one S1MCP listener on 127.0.0.1:8765.' }
    $process = Get-Process -Id $listeners[0].OwningProcess -ErrorAction Stop
    return @{ Id = $process.Id; Path = $process.Path; StartedUtc = $process.StartTime.ToUniversalTime().ToString('O') }
}

function Assert-S1McpProcess($Context, $Current) {
    if ($Current.Id -ne $Context.Process.Id -or $Current.StartedUtc -cne $Context.Process.StartedUtc -or
        -not [string]::Equals($Current.Path, $Context.GameExe, [StringComparison]::OrdinalIgnoreCase)) {
        throw 'Game process identity changed or the bridge belongs to another installation.'
    }
}

function New-S1McpTransportTimeout([string]$Message) {
    $failure = [TimeoutException]::new($Message)
    $failure.Data['S1McpTransportTimeout'] = $true
    return $failure
}

function Test-S1McpTransportTimeout([Exception]$Failure) {
    while ($null -ne $Failure) {
        if (($Failure -is [Net.Sockets.SocketException] -and $Failure.SocketErrorCode -eq [Net.Sockets.SocketError]::TimedOut) -or
            ($Failure -is [TimeoutException] -and $Failure.Data['S1McpTransportTimeout'] -eq $true)) { return $true }
        $Failure = $Failure.InnerException
    }
    return $false
}

function Read-S1McpBytes($Stream, [int]$Length, [datetime]$DeadlineUtc) {
    $bytes = [byte[]]::new($Length)
    $offset = 0
    while ($offset -lt $Length) {
        $remaining = [int][Math]::Ceiling(($DeadlineUtc - [DateTime]::UtcNow).TotalMilliseconds)
        if ($remaining -le 0) { throw (New-S1McpTransportTimeout 'MCP response deadline exceeded.') }
        if ($Stream.CanTimeout) { $Stream.ReadTimeout = $remaining }
        $count = $Stream.Read($bytes, $offset, $Length - $offset)
        if ($count -eq 0) { throw 'MCP stream closed before frame completed.' }
        $offset += $count
    }
    return ,$bytes
}

function Write-S1McpFrame($Stream, $Message) {
    $bytes = [Text.Encoding]::UTF8.GetBytes(($Message | ConvertTo-Json -Depth 12 -Compress))
    $header = [BitConverter]::GetBytes([uint32]$bytes.Length)
    $Stream.Write($header, 0, 4)
    $Stream.Write($bytes, 0, $bytes.Length)
    $Stream.Flush()
}

function Read-S1McpResponse($Stream, [int]$RequestId, [datetime]$DeadlineUtc, $Context) {
    for ($frame = 0; $frame -lt 8; $frame++) {
        $length = [BitConverter]::ToUInt32((Read-S1McpBytes $Stream 4 $DeadlineUtc), 0)
        if ($length -eq 0 -or $length -gt 1048576) { throw 'Invalid or excessive MCP response length.' }
        $response = [Text.Encoding]::UTF8.GetString((Read-S1McpBytes $Stream $length $DeadlineUtc)) | ConvertFrom-Json -AsHashtable
        $numericId = $response.id -is [int] -or $response.id -is [long]
        if ($numericId -and $response.id -lt 0 -and $response.result.type -ceq 'server_heartbeat' -and $response.result.status -ceq 'alive') {
            Write-S1McpEvidence $Context @{ notification = $response; capturedUtc = [DateTime]::UtcNow.ToString('O') }
            continue
        }
        if (-not $numericId -or $response.id -ne $RequestId) {
            Write-S1McpEvidence $Context @{ uncorrelatedResponse = $response; expectedId = $RequestId }
            throw 'MCP response ID mismatch; no retry.'
        }
        return $response
    }
    throw 'MCP response frame budget exceeded.'
}

function Invoke-S1McpRequest($Context, [string]$Method, [hashtable]$Parameters = @{}) {
    if ($Method -cnotin @('get_game_state', 'get_time_of_day', 'set_time_of_day', 'list_saves', 'load_save', 'call_method')) {
        throw 'Unsupported MCP method.'
    }
    if ($Method -eq 'call_method' -and ($Parameters.object_name -cne 'WorldLook.Automation' -or
        $Parameters.component_type -cne 'WorldLook.WorldLookAutomation' -or
        $Parameters.method_name -cnotin @('GetStatus', 'EnableLook', 'DisableLook', 'ReturnToMenu') -or $Parameters.Count -ne 3)) {
        throw 'Only the parameterless WorldLook automation endpoint is allowed.'
    }
    Assert-S1McpProcess $Context (Get-S1McpProcess)
    if ([DateTime]::UtcNow -ge $Context.DeadlineUtc) { throw 'WorldLook action deadline exceeded.' }
    $request = @{ id = [Random]::Shared.Next(10000, 1000000000); method = $Method; params = $Parameters }
    $started = [DateTime]::UtcNow.ToString('O')
    $client = [Net.Sockets.TcpClient]::new()
    try {
        $connect = $client.ConnectAsync('127.0.0.1', 8765)
        if (-not $connect.Wait(3000)) { throw (New-S1McpTransportTimeout 'MCP connection timed out.') }
        $null = $connect.GetAwaiter().GetResult()
        $stream = $client.GetStream()
        $stream.WriteTimeout = 3000
        Write-S1McpFrame $stream $request
        $deadline = [DateTime]::UtcNow.AddSeconds(8)
        if ($Context.DeadlineUtc -lt $deadline) { $deadline = $Context.DeadlineUtc }
        $response = Read-S1McpResponse $stream $request.id $deadline $Context
        Write-S1McpFrame $stream @{ id = $request.id; status = 'received' }
        Write-S1McpEvidence $Context @{ startedUtc = $started; completedUtc = [DateTime]::UtcNow.ToString('O'); process = $Context.Process; request = $request; response = $response }
        Assert-S1McpProcess $Context (Get-S1McpProcess)
        if ($response.error) { throw ('MCP server error: ' + ($response.error | ConvertTo-Json -Compress -Depth 8)) }
        if (-not $response.ContainsKey('result')) { throw 'MCP response has no result.' }
        return $response.result
    }
    catch {
        Write-S1McpEvidence $Context @{ startedUtc = $started; request = $request; clientError = $_.Exception.Message; mutationRetried = $false }
        throw
    }
    finally { $client.Dispose() }
}

function Invoke-S1WorldLookEndpoint($Context, [string]$Method = 'GetStatus') {
    $result = Invoke-S1McpRequest $Context 'call_method' @{ object_name = 'WorldLook.Automation'; component_type = 'WorldLook.WorldLookAutomation'; method_name = $Method }
    if ($result.method_name -cne $Method -or $result.return_value.type -cne 'String' -or $result.return_value.value -isnot [string]) {
        throw 'WorldLook endpoint returned no explicit JSON result (missing method or unsupported bridge).'
    }
    $status = $result.return_value.value | ConvertFrom-Json -AsHashtable
    $invalidBoolean = @('accepted', 'requestedEnabled', 'ready', 'singleplayer', 'loading', 'hostOrSingleplayer',
        'gameLoaded', 'lightingFaulted', 'waterFaulted') | Where-Object { $status[$_] -isnot [bool] }
    if ($status.schemaVersion -ne 1 -or @($invalidBoolean).Count -gt 0) {
        throw 'Unsupported or incomplete WorldLook status schema.'
    }
    if (-not $status.accepted -or $status.error) { throw ('WorldLook rejected ' + $Method + ': ' + $status.error) }
    return $status
}

function Assert-S1WorldLookSave($Status, [string]$ExpectedName, [string]$ExpectedPath) {
    if (-not $Status.ready -or $Status.scene -cne 'Main' -or -not $Status.gameLoaded -or $Status.loading -or
        -not $Status.hostOrSingleplayer -or -not $Status.singleplayer) { throw 'WorldLook requires a ready singleplayer Main scene.' }
    if ($Status.saveName -cne $ExpectedName -or -not [string]::Equals(
        ([IO.Path]::GetFullPath($Status.savePath)).TrimEnd('\', '/'), $ExpectedPath, [StringComparison]::OrdinalIgnoreCase)) {
        throw 'Loaded save identity does not match the explicitly selected save.'
    }
}

function Find-S1WorldLookSave($Saves, [string]$ExpectedName, [string]$ExpectedPath) {
    $matches = @($Saves | Where-Object {
        $_.organisationName -ceq $ExpectedName -and $_.savePath -and
        [string]::Equals(([IO.Path]::GetFullPath($_.savePath)).TrimEnd('\', '/'), $ExpectedPath, [StringComparison]::OrdinalIgnoreCase)
    })
    if ($matches.Count -ne 1 -or ($matches[0].slot_index -isnot [long] -and $matches[0].slot_index -isnot [int]) -or $matches[0].slot_index -lt 0) {
        throw 'Save selection is missing, ambiguous or has an invalid slot index.'
    }
    return [int]$matches[0].slot_index
}

function Wait-S1WorldLookStatus($Context, [scriptblock]$Predicate) {
    while ([DateTime]::UtcNow -lt $Context.DeadlineUtc) {
        $status = $null
        try { $status = Invoke-S1WorldLookEndpoint $Context }
        catch {
            if (-not (Test-S1McpTransportTimeout $_.Exception)) { throw }
            # Loading can block the main thread longer than one response window. Only
            # this read-only poll may repeat; schema/process errors still stop the action.
            Write-S1McpEvidence $Context @{ readOnlyPollTimedOutUtc = [DateTime]::UtcNow.ToString('O');
                error = $_.Exception.Message; method = 'GetStatus'; mutationRetried = $false }
        }
        if ($null -ne $status -and (& $Predicate $status)) { return $status }
        $remaining = [int][Math]::Ceiling(($Context.DeadlineUtc - [DateTime]::UtcNow).TotalMilliseconds)
        if ($remaining -gt 0) { Start-Sleep -Milliseconds ([Math]::Min(500, $remaining)) }
    }
    throw 'WorldLook state transition timed out; mutation was not retried.'
}

function Invoke-S1WorldLookAction($Context, [string]$Action, [string]$SaveName, [string]$SavePath, [Nullable[int]]$Hhmm) {
    $before = Invoke-S1WorldLookEndpoint $Context
    if ($Action -eq 'Status') { return $before }
    if ($Action -eq 'Load') {
        if ($before.scene -cne 'Menu' -or $before.loading) { throw 'Load requires the actual main menu; no automatic scene switch.' }
        $game = Invoke-S1McpRequest $Context 'get_game_state'
        $clock = Invoke-S1McpRequest $Context 'get_time_of_day'
        if ($game.scene_name -cne 'Menu' -or $clock.available -isnot [bool] -or $clock.available) { throw 'Native main-menu readiness was not confirmed.' }
        $saves = Invoke-S1McpRequest $Context 'list_saves'
        $slot = Find-S1WorldLookSave $saves.saves $SaveName $SavePath
        $started = Invoke-S1McpRequest $Context 'load_save' @{ slot_index = $slot }
        if ($started.success -isnot [bool] -or -not $started.success -or $started.slot_index -ne $slot -or
            $started.returned_to_menu -isnot [bool] -or $started.returned_to_menu) {
            throw 'Load request not confirmed; no retry.'
        }
        $after = Wait-S1WorldLookStatus $Context { param($s) $s.ready }
        Assert-S1WorldLookSave $after $SaveName $SavePath
        if ($after.requestedEnabled -ne $before.requestedEnabled) { throw 'WorldLook session state changed across reload.' }
        return $after
    }
    Assert-S1WorldLookSave $before $SaveName $SavePath
    switch ($Action) {
        { $_ -in @('On', 'Off') } {
            $enabled = $Action -eq 'On'
            $method = if ($enabled) { 'EnableLook' } else { 'DisableLook' }
            $accepted = Invoke-S1WorldLookEndpoint $Context $method
            if ($accepted.requestedEnabled -ne $enabled) { throw 'WorldLook setter returned the wrong requested state.' }
            $after = Invoke-S1WorldLookEndpoint $Context
            Assert-S1WorldLookSave $after $SaveName $SavePath
            if ($after.requestedEnabled -ne $enabled -or $after.lightingFaulted -or $after.waterFaulted) { throw 'WorldLook state/fault readback failed.' }
            return $after
        }
        'Menu' {
            $null = Invoke-S1WorldLookEndpoint $Context 'ReturnToMenu'
            $after = Wait-S1WorldLookStatus $Context { param($s) $s.scene -ceq 'Menu' -and -not $s.loading }
            $game = Invoke-S1McpRequest $Context 'get_game_state'
            $clock = Invoke-S1McpRequest $Context 'get_time_of_day'
            if ($game.scene_name -cne 'Menu' -or $clock.available -isnot [bool] -or $clock.available -or $after.requestedEnabled -ne $before.requestedEnabled) {
                throw 'Menu transition/session-state readback failed.'
            }
            return $after
        }
        'Time' {
            if ($null -eq $Hhmm -or $Hhmm -lt 0 -or $Hhmm -gt 2359 -or $Hhmm % 100 -ge 60) { throw 'Time requires a valid HHMM value.' }
            $write = Invoke-S1McpRequest $Context 'set_time_of_day' @{ hhmm = [int]$Hhmm }
            if ($write.available -isnot [bool] -or -not $write.available -or
                ($write.readback_hhmm -isnot [int] -and $write.readback_hhmm -isnot [long]) -or $write.readback_hhmm -ne $Hhmm) { throw 'Time setter readback failed; no retry.' }
            $read = Invoke-S1McpRequest $Context 'get_time_of_day'
            $targetMinutes = [int][Math]::Floor($Hhmm / 100) * 60 + $Hhmm % 100
            # Simulation continues between requests; permit at most three elapsed minutes.
            if ($read.available -isnot [bool] -or -not $read.available -or
                ($read.time_minutes -isnot [int] -and $read.time_minutes -isnot [long]) -or
                $read.time_minutes -lt 0 -or $read.time_minutes -gt 1439 -or (($read.time_minutes - $targetMinutes + 1440) % 1440) -gt 3) {
                throw 'Independent clock readback is outside the three-minute sampling window.'
            }
            $after = Invoke-S1WorldLookEndpoint $Context
            Assert-S1WorldLookSave $after $SaveName $SavePath
            return @{ before = $before; after = $after; appliedHhmm = $Hhmm; clock = $read }
        }
        default { throw 'Unsupported WorldLook action.' }
    }
}

function Invoke-S1WorldLook {
    [CmdletBinding()]
    param([string]$Action = 'Status', [string]$EvidencePath, [string]$SaveName, [string]$SavePath,
        [Nullable[int]]$Hhmm, [string]$GameDir, [Nullable[int]]$GameProcessId, [string]$GameStartedUtc, [int]$TimeoutSeconds = 60)
    if ($Action -notin @('Status', 'On', 'Off', 'Menu', 'Load', 'Time')) { throw 'Unsupported WorldLook action.' }
    if ([string]::IsNullOrWhiteSpace($EvidencePath)) { throw 'A new EvidencePath is required.' }
    if ($Action -ne 'Status') {
        if ([string]::IsNullOrWhiteSpace($SaveName) -or [string]::IsNullOrWhiteSpace($SavePath) -or
            -not [IO.Path]::IsPathFullyQualified($SavePath)) { throw 'Mutations require exact SaveName and absolute SavePath.' }
        $SavePath = ([IO.Path]::GetFullPath($SavePath)).TrimEnd('\', '/')
    }
    if (-not $GameDir) { $GameDir = if ($env:SCHEDULE1_PATH) { $env:SCHEDULE1_PATH } else { 'C:/Program Files (x86)/Steam/steamapps/common/Schedule I' } }
    $gameExe = [IO.Path]::GetFullPath((Join-Path $GameDir 'Schedule I.exe'))
    $process = Get-S1McpProcess
    $context = @{ Process = $process; GameExe = $gameExe; DeadlineUtc = [DateTime]::UtcNow.AddSeconds($TimeoutSeconds); EvidencePath = [IO.Path]::GetFullPath($EvidencePath) }
    Assert-S1McpProcess $context $process
    if (($null -ne $GameProcessId -and $process.Id -ne $GameProcessId) -or
        ($GameStartedUtc -and [DateTimeOffset]::Parse($process.StartedUtc) -ne [DateTimeOffset]::Parse($GameStartedUtc))) { throw 'The selected game session is no longer running.' }
    $null = [IO.Directory]::CreateDirectory((Split-Path $context.EvidencePath -Parent))
    $created = [IO.File]::Open($context.EvidencePath, [IO.FileMode]::CreateNew, [IO.FileAccess]::Write)
    $created.Dispose()
    Write-S1McpEvidence $context @{ action = $Action; process = $process; selectedSaveName = $SaveName; selectedSavePath = $SavePath; startedUtc = [DateTime]::UtcNow.ToString('O') }
    try {
        $status = Invoke-S1WorldLookAction $context $Action $SaveName $SavePath $Hhmm
        $result = @{ action = $Action; process = $process; evidencePath = $context.EvidencePath; completedUtc = [DateTime]::UtcNow.ToString('O'); status = $status }
        Write-S1McpEvidence $context @{ completed = $result }
        return $result
    }
    catch {
        Write-S1McpEvidence $context @{ failedUtc = [DateTime]::UtcNow.ToString('O'); error = $_.Exception.Message; mutationRetried = $false }
        throw
    }
}

if ($MyInvocation.InvocationName -ne '.') {
    $ErrorActionPreference = 'Stop'
    Invoke-S1WorldLook @PSBoundParameters | ConvertTo-Json -Depth 20
}
