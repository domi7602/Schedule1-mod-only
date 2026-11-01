#Requires -Version 7
<#
.SYNOPSIS
    Bounded live-baseline helper for the Town Hall door tests through the local S1MCP bridge.
.DESCRIPTION
    Orchestrates save hashing, Steam launch, bridge identity, read-only state reads and
    single-shot mutations (time, teleport, load) plus in-game captures. No keyboard or
    mouse injection, no deployment, no saving, no automatic mutation retry: a timed-out
    mutation stays uncertain and must be inspected before any further write. The real
    E/Escape interaction and desktop captures belong to the CUA session described in
    Source/Mods/TownHallInterior/docs/live-automation-runbook.md. Every action appends to
    one fresh JSONL evidence file per run. Dot-sourcing defines functions only.
.EXAMPLE
    pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/townhall-live-baseline.ps1 `
      -Action PreHash -EvidencePath .scratch/townhall/run.jsonl -SnapshotDir .scratch/townhall/snapshot
#>
[CmdletBinding()]
param(
    [ValidateSet('PreHash', 'PostHash', 'Launch', 'Bridge', 'State', 'Time', 'Teleport', 'Load', 'Screenshot', 'Quit', 'Collect')]
    [string]$Action = 'State',
    [string]$EvidencePath,
    [string]$SaveRoot,
    [string]$ManifestPath,
    [string]$SnapshotDir,
    [string]$SaveName,
    [string]$SavePath,
    [Nullable[int]]$Hhmm,
    [Nullable[double]]$X,
    [Nullable[double]]$Y,
    [Nullable[double]]$Z,
    [switch]$KeepY,
    [ValidateRange(1000, 15000)][int]$ScreenshotTimeoutMs = 5000,
    [string]$GameDir,
    [ValidateRange(5, 300)][int]$TimeoutSeconds = 90
)

$ErrorActionPreference = 'Stop'

function Write-RunEvidence($Context, $Record) {
    if (-not $Context -or -not $Context.EvidencePath) { return }
    $Record | ConvertTo-Json -Depth 40 -Compress | Add-Content -LiteralPath $Context.EvidencePath -Encoding utf8
}

function Get-DefaultSaveRoot {
    Join-Path $env:USERPROFILE 'AppData\LocalLow\TVGS\Schedule I\Saves\76561199109519851'
}

function Get-DefaultGameDir {
    if ($env:SCHEDULE1_PATH) { return $env:SCHEDULE1_PATH }
    'C:\Program Files (x86)\Steam\steamapps\common\Schedule I'
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
        if ($Stream.CanTimeout) { $stream.ReadTimeout = $remaining }
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
            Write-RunEvidence $Context @{ notification = $response; capturedUtc = [DateTime]::UtcNow.ToString('O') }
            continue
        }
        if (-not $numericId -or $response.id -ne $RequestId) {
            Write-RunEvidence $Context @{ uncorrelatedResponse = $response; expectedId = $RequestId }
            throw 'MCP response ID mismatch; no retry.'
        }
        return $response
    }
    throw 'MCP response frame budget exceeded.'
}

function Invoke-S1McpRequest($Context, [string]$Method, [hashtable]$Parameters = @{}) {
    $allowed = @('get_game_state', 'get_time_of_day', 'set_time_of_day', 'list_saves', 'load_save', 'teleport_player', 'capture_screenshot')
    if ($Method -cnotin $allowed) { throw "Unsupported MCP method '$Method'." }
    Assert-S1McpProcess $Context (Get-S1McpProcess)
    if ([DateTime]::UtcNow -ge $Context.DeadlineUtc) { throw 'Run deadline exceeded.' }
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
        $deadline = [DateTime]::UtcNow.AddSeconds(10)
        if ($Context.DeadlineUtc -lt $deadline) { $deadline = $Context.DeadlineUtc }
        $response = Read-S1McpResponse $stream $request.id $deadline $Context
        Write-S1McpFrame $stream @{ id = $request.id; status = 'received' }
        Write-RunEvidence $Context @{ startedUtc = $started; completedUtc = [DateTime]::UtcNow.ToString('O'); process = $Context.Process; request = $request; response = $response; mutationRetried = $false }
        Assert-S1McpProcess $Context (Get-S1McpProcess)
        if ($response.error) { throw ('MCP server error: ' + ($response.error | ConvertTo-Json -Compress -Depth 8)) }
        if (-not $response.ContainsKey('result')) { throw 'MCP response has no result.' }
        return $response.result
    }
    catch {
        Write-RunEvidence $Context @{ startedUtc = $started; request = $request; clientError = $_.Exception.Message; mutationRetried = $false }
        throw
    }
    finally { $client.Dispose() }
}

function Get-SaveTreeHashes([string]$Root) {
    $map = [ordered]@{}
    Get-ChildItem -LiteralPath $Root -Recurse -File | ForEach-Object {
        $rel = $_.FullName.Substring($Root.Length + 1) -replace '\\', '/'
        $map[$rel] = (Get-FileHash -Algorithm SHA256 -LiteralPath $_.FullName).Hash.ToLower()
    }
    return $map
}

function Invoke-PreHash($Context) {
    $root = $Context.SaveRoot
    if (-not (Test-Path -LiteralPath $root)) { throw "Save root not found: $root" }
    $manifest = [ordered]@{
        createdUtc = [DateTime]::UtcNow.ToString('O')
        root       = $root
        files      = (Get-SaveTreeHashes $root)
    }
    $manifestPath = Join-Path $Context.EvidenceDir 'save-manifest.json'
    $manifest | ConvertTo-Json -Depth 6 | Set-Content -LiteralPath $manifestPath -Encoding utf8
    if ($Context.SnapshotDir) {
        New-Item -ItemType Directory -Path $Context.SnapshotDir -Force | Out-Null
        Get-ChildItem -LiteralPath $root -Recurse -File | ForEach-Object {
            $rel = $_.FullName.Substring($root.Length + 1)
            $target = Join-Path $Context.SnapshotDir $rel
            New-Item -ItemType Directory -Path (Split-Path $target -Parent) -Force | Out-Null
            Copy-Item -LiteralPath $_.FullName -Destination $target -Force
        }
    }
    Write-RunEvidence $Context @{ action = 'PreHash'; manifestPath = $manifestPath; fileCount = $manifest.files.Count; snapshotDir = $Context.SnapshotDir }
    Write-Output "PreHash: $($manifest.files.Count) Dateien, Manifest $manifestPath"
    return $manifestPath
}

function Invoke-PostHash($Context) {
    $manifestFile = if ($Context.ManifestPath) { $Context.ManifestPath } else { Join-Path $Context.EvidenceDir 'save-manifest.json' }
    if (-not (Test-Path -LiteralPath $manifestFile)) { throw "Manifest not found: $manifestFile" }
    $manifest = Get-Content -LiteralPath $manifestFile -Raw | ConvertFrom-Json
    $root = $manifest.root
    $firstKey = @($manifest.files.PSObject.Properties | Select-Object -First 1)
    if ($firstKey.Count -ne 1) { throw 'Manifest contains no file entries.' }
    if (-not (Test-Path -LiteralPath (Join-Path $root ($firstKey[0].Name -replace '/', '\')))) {
        throw 'Manifest root and keys are inconsistent (first entry missing); fix the manifest instead of comparing.'
    }
    $changed = @(); $missing = @(); $extra = @()
    foreach ($p in $manifest.files.PSObject.Properties) {
        $file = Join-Path $root ($p.Name -replace '/', '\')
        if (-not (Test-Path -LiteralPath $file)) { $missing += $p.Name; continue }
        $hash = (Get-FileHash -Algorithm SHA256 -LiteralPath $file).Hash.ToLower()
        if ($hash -cne $p.Value) { $changed += $p.Name }
    }
    Get-ChildItem -LiteralPath $root -Recurse -File | ForEach-Object {
        $rel = $_.FullName.Substring($root.Length + 1) -replace '\\', '/'
        if ($null -eq $manifest.files.PSObject.Properties[$rel]) { $extra += $rel }
    }
    $compare = [ordered]@{
        comparedUtc = [DateTime]::UtcNow.ToString('O')
        manifest    = $manifestFile
        changed     = $changed
        missing     = $missing
        extra       = $extra
    }
    $comparePath = Join-Path $Context.EvidenceDir 'save-compare.json'
    $compare | ConvertTo-Json -Depth 6 | Set-Content -LiteralPath $comparePath -Encoding utf8
    Write-RunEvidence $Context @{ action = 'PostHash'; changed = $changed.Count; missing = $missing.Count; extra = $extra.Count; comparePath = $comparePath }
    Write-Output "PostHash: $($changed.Count) geaendert, $($missing.Count) fehlend, $($extra.Count) neu -> $comparePath"
    $changed | ForEach-Object { Write-Output "  geaendert: $_" }
    $missing | ForEach-Object { Write-Output "  fehlend: $_" }
    $extra | ForEach-Object { Write-Output "  neu: $_" }
}

function Invoke-Launch($Context) {
    $before = Get-Process -Name 'Schedule I' -ErrorAction SilentlyContinue
    if ($before) { throw 'Game already running; refuse to launch a second instance.' }
    Start-Process 'steam://rungameid/3164500'
    $deadline = [DateTime]::UtcNow.AddSeconds($Context.TimeoutSeconds)
    $proc = $null
    while ([DateTime]::UtcNow -lt $deadline) {
        $proc = Get-Process -Name 'Schedule I' -ErrorAction SilentlyContinue
        if ($proc) { break }
        Start-Sleep -Milliseconds 1000
    }
    if (-not $proc) { throw 'Game process did not appear within the timeout.' }
    Write-RunEvidence $Context @{ action = 'Launch'; processId = $proc.Id; startedUtc = $proc.StartTime.ToUniversalTime().ToString('O') }
    Write-Output "Launch: Spielprozess PID $($proc.Id), gestartet $($proc.StartTime.ToUniversalTime().ToString('O'))"
}

function Invoke-Bridge($Context) {
    $deadline = [DateTime]::UtcNow.AddSeconds($Context.TimeoutSeconds)
    $info = $null
    while ([DateTime]::UtcNow -lt $deadline) {
        try { $info = Get-S1McpProcess; break }
        catch { Start-Sleep -Milliseconds 1000 }
    }
    if (-not $info) { throw 'No single S1MCP listener appeared within the timeout.' }
    $contextProcess = @{ Id = $info.Id; Path = $info.Path; StartedUtc = $info.StartedUtc }
    $Context.Process = $contextProcess
    $state = Invoke-S1McpRequest $Context 'get_game_state'
    Write-RunEvidence $Context @{ action = 'Bridge'; process = $contextProcess; scene = $state.scene_name }
    Write-Output "Bridge: PID $($info.Id), Szene '$($state.scene_name)'"
}

function Invoke-Teleport($Context) {
    if ($null -eq $Context.X -or $null -eq $Context.Z) { throw 'Teleport requires -X and -Z.' }
    $y = $Context.Y
    if ($KeepY -or $null -eq $y) {
        $state = Invoke-S1McpRequest $Context 'get_game_state'
        $y = [double]$state.player.position.y
    }
    if ($y -lt -10 -or $y -gt 1000) { throw 'Vertical position outside the historical validation range.' }
    $before = Invoke-S1McpRequest $Context 'get_game_state'
    $result = Invoke-S1McpRequest $Context 'teleport_player' @{ position = @{ x = [double]$Context.X; y = [double]$y; z = [double]$Context.Z } }
    $after = Invoke-S1McpRequest $Context 'get_game_state'
    Write-RunEvidence $Context @{ action = 'Teleport'; target = @{ x = $Context.X; y = $y; z = $Context.Z }; result = $result; beforePosition = $before.player.position; afterPosition = $after.player.position }
    Write-Output "Teleport: Ziel ($($Context.X), $y, $($Context.Z)); danach ($($after.player.position.x), $($after.player.position.y), $($after.player.position.z))"
}

function Invoke-Load($Context) {
    if (-not $Context.SaveName -or -not $Context.SavePath) { throw 'Load requires -SaveName and -SavePath.' }
    $expected = [IO.Path]::GetFullPath($Context.SavePath).TrimEnd('\', '/')
    $state = Invoke-S1McpRequest $Context 'get_game_state'
    if ($state.scene_name -cne 'Menu') { throw 'Load requires the actual main menu; no automatic scene switch.' }
    $saves = Invoke-S1McpRequest $Context 'list_saves'
    $matches = @($saves.saves | Where-Object {
        $_.organisationName -ceq $Context.SaveName -and $_.savePath -and
        [string]::Equals(([IO.Path]::GetFullPath($_.savePath)).TrimEnd('\', '/'), $expected, [StringComparison]::OrdinalIgnoreCase)
    })
    if ($matches.Count -ne 1) { throw 'Save selection is missing or ambiguous; no retry.' }
    $slot = [int]$matches[0].slot_index
    $started = Invoke-S1McpRequest $Context 'load_save' @{ slot_index = $slot }
    if ($started.success -isnot [bool] -or -not $started.success -or $started.slot_index -ne $slot) { throw 'Load request not confirmed; no retry.' }
    $deadline = [DateTime]::UtcNow.AddSeconds($Context.TimeoutSeconds)
    $loaded = $null
    while ([DateTime]::UtcNow -lt $deadline) {
        try {
            $after = Invoke-S1McpRequest $Context 'get_game_state'
            if ($after.scene_name -ceq 'Main') { $loaded = $after; break }
        }
        catch {
            if (-not (Test-S1McpTransportTimeout $_.Exception)) { throw }
            Write-RunEvidence $Context @{ readOnlyPollTimedOutUtc = [DateTime]::UtcNow.ToString('O'); error = $_.Exception.Message; mutationRetried = $false }
        }
        Start-Sleep -Milliseconds 1000
    }
    if (-not $loaded) { throw 'Main scene did not become ready within the timeout; load outcome uncertain, no retry.' }
    Write-RunEvidence $Context @{ action = 'Load'; slot = $slot; saveName = $Context.SaveName; savePath = $expected }
    Write-Output "Load: Slot $slot ('$($Context.SaveName)') bereit in Szene '$($loaded.scene_name)'"
}

function Invoke-Quit($Context) {
    $proc = Get-Process -Name 'Schedule I' -ErrorAction SilentlyContinue
    if (-not $proc) { Write-Output 'Quit: Spiel laeuft nicht.'; return }
    $closed = $proc.CloseMainWindow()
    $deadline = [DateTime]::UtcNow.AddSeconds(30)
    while ([DateTime]::UtcNow -lt $deadline) {
        if (-not (Get-Process -Id $proc.Id -ErrorAction SilentlyContinue)) { break }
        Start-Sleep -Milliseconds 1000
    }
    $still = Get-Process -Id $proc.Id -ErrorAction SilentlyContinue
    Write-RunEvidence $Context @{ action = 'Quit'; closeAccepted = $closed; exited = (-not $still); processId = $proc.Id }
    if ($still) { throw 'Game did not exit gracefully within 30 s; do not kill blindly, inspect the session.' }
    Write-Output "Quit: Spiel sauber beendet (PID $($proc.Id))."
}

function Invoke-Collect($Context) {
    $log = Join-Path $Context.GameDir 'MelonLoader\Latest.log'
    $target = Join-Path $Context.EvidenceDir 'MelonLoader-Latest.log'
    if (Test-Path -LiteralPath $log) { Copy-Item -LiteralPath $log -Destination $target -Force }
    $shots = Join-Path $Context.GameDir 'UserData\S1MCP\Evidence\Screenshots'
    $shotFiles = @()
    if (Test-Path -LiteralPath $shots) {
        $shotFiles = @(Get-ChildItem -LiteralPath $shots -File | Sort-Object LastWriteTime)
        $shotFiles | ForEach-Object {
            Copy-Item -LiteralPath $_.FullName -Destination (Join-Path $Context.EvidenceDir $_.Name) -Force
        }
    }
    $hashes = [ordered]@{}
    Get-ChildItem -LiteralPath $Context.EvidenceDir -File | ForEach-Object {
        $hashes[$_.Name] = (Get-FileHash -Algorithm SHA256 -LiteralPath $_.FullName).Hash.ToLower()
    }
    $manifest = [ordered]@{ collectedUtc = [DateTime]::UtcNow.ToString('O'); files = $hashes }
    $manifest | ConvertTo-Json -Depth 6 | Set-Content -LiteralPath (Join-Path $Context.EvidenceDir 'evidence-manifest.json') -Encoding utf8
    Write-RunEvidence $Context @{ action = 'Collect'; logCopied = (Test-Path -LiteralPath $target); screenshots = $shotFiles.Count }
    Write-Output "Collect: Log kopiert=$(Test-Path -LiteralPath $target), Screenshots=$($shotFiles.Count), Manifest evidence-manifest.json"
}

function Invoke-TownHallBaseline {
    if (-not $EvidencePath) { throw 'EvidencePath is required.' }
    $evidenceDir = Split-Path ([IO.Path]::GetFullPath($EvidencePath)) -Parent
    New-Item -ItemType Directory -Path $evidenceDir -Force | Out-Null
    $gameDir = if ($GameDir) { $GameDir } else { Get-DefaultGameDir }
    $context = @{
        EvidencePath = [IO.Path]::GetFullPath($EvidencePath)
        EvidenceDir  = $evidenceDir
        SaveRoot     = if ($SaveRoot) { $SaveRoot } else { Get-DefaultSaveRoot }
        SnapshotDir  = if ($SnapshotDir) { [IO.Path]::GetFullPath($SnapshotDir) } else { $null }
        ManifestPath = if ($ManifestPath) { [IO.Path]::GetFullPath($ManifestPath) } else { $null }
        SaveName     = $SaveName
        SavePath     = $SavePath
        X            = $X; Y = $Y; Z = $Z; KeepY = $KeepY.IsPresent
        GameDir      = $gameDir
        GameExe      = Join-Path $gameDir 'Schedule I.exe'
        Process      = $null
        DeadlineUtc  = [DateTime]::UtcNow.AddSeconds($TimeoutSeconds)
        TimeoutSeconds = $TimeoutSeconds
    }
    Write-RunEvidence $context @{ runStartedUtc = [DateTime]::UtcNow.ToString('O'); action = $Action; gameDir = $gameDir; saveRoot = $context.SaveRoot }
    switch ($Action) {
        'PreHash' { return Invoke-PreHash $context }
        'PostHash' { return Invoke-PostHash $context }
        'Launch' { return Invoke-Launch $context }
        'Quit' { return Invoke-Quit $context }
        'Collect' { return Invoke-Collect $context }
        'Bridge' { Invoke-Bridge $context; return }
        'State' {
            Invoke-Bridge $context
            $state = Invoke-S1McpRequest $context 'get_game_state'
            $clock = Invoke-S1McpRequest $context 'get_time_of_day'
            Write-RunEvidence $context @{ action = 'State'; state = $state; clock = $clock }
            Write-Output ($state | ConvertTo-Json -Depth 8 -Compress)
            return
        }
        'Time' {
            if ($null -eq $Hhmm -or $Hhmm -lt 0 -or $Hhmm -gt 2359 -or $Hhmm % 100 -ge 60) { throw 'Time requires a valid HHMM value.' }
            Invoke-Bridge $context
            $write = Invoke-S1McpRequest $context 'set_time_of_day' @{ hhmm = [int]$Hhmm }
            if ($write.readback_hhmm -ne $Hhmm) { throw 'Time setter readback mismatch; outcome uncertain, no retry.' }
            $read = Invoke-S1McpRequest $context 'get_time_of_day'
            Write-RunEvidence $context @{ action = 'Time'; hhmm = $Hhmm; write = $write; read = $read }
            Write-Output "Time: gesetzt $Hhmm, Readback $($read.time_hhmm)"
            return
        }
        'Teleport' { Invoke-Bridge $context; Invoke-Teleport $context; return }
        'Load' { Invoke-Bridge $context; Invoke-Load $context; return }
        'Screenshot' {
            Invoke-Bridge $context
            $shot = Invoke-S1McpRequest $context 'capture_screenshot' @{ timeout_ms = $ScreenshotTimeoutMs }
            Write-RunEvidence $context @{ action = 'Screenshot'; result = $shot }
            Write-Output "Screenshot: $($shot.path)"
            return
        }
    }
}

if ($MyInvocation.InvocationName -ne '.') {
    Invoke-TownHallBaseline
}
