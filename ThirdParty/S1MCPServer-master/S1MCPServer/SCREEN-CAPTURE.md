# Direct final-frame screenshot (local fork, unreleased)

## Endpoint

```json
{"id":12345,"method":"capture_screenshot","params":{"timeout_ms":5000}}
```

`params` may be empty. The only accepted property is `timeout_ms`, an integer from
100 to 15000 inclusive (default 5000). No caller-selected path, filename, scale,
camera or UI toggle. Native screen size is limited to 8192 per edge and 16,777,216
pixels; PNG bytes are limited to 64 MiB. The endpoint is listed by `list_methods`.

Success is sent only after end-of-frame capture, PNG encoding, file close and size
readback. Result fields:

- `schema_version: 1`, `success: true`
- `path`: actual absolute unique PNG path under
  `<Game>/UserData/S1MCP/Evidence/Screenshots/capture-<GUID>.png`
- `width`, `height`, `bytes`, Unity `frame`
- `started_utc`, `captured_utc`, `completed_utc`, monotonic `elapsed_ms`
- `includes_ui: true`, `capture_source: "Unity.ScreenCapture/end-of-frame"`

`captured_utc` is sampled immediately before Unity's readback, not at disk-save time.
The final rendered game framebuffer includes Unity UI, unlike a camera-only render.
It does not capture foreground desktop applications. OS overlays/cursor inclusion is
not guaranteed. No save, clock, camera, WorldLook or water state is changed.

Errors: `-32602` invalid parameters; `-32001` capture already busy/draining;
`-32002` timeout; `-32000` capture/encoding/filesystem/shutdown failure.
Concurrent captures are refused. A timed-out native operation cannot be forcibly
interrupted; its guard remains until it drains, so background/minimized rendering
cannot accumulate coroutines. A subsequent end-of-frame discards canceled work.
Shutdown stops an owned pending coroutine. Textures are destroyed on the Unity
thread in `finally`. File I/O runs on a worker, with cancellation/partial-file cleanup.
Evidence ancestors are checked for reparse points; network paths are refused.
No retention/deletion policy is applied to successful evidence.

## Existing local transport

The bridge uses the inspected `Tools/invoke-worldlook-mcp.ps1` framing, correlated
response reader and acknowledgment helpers. **Do not change that script's WorldLook
method allowlist.** Its low-level helpers can be dot-sourced for this new method:

```powershell
. ./Tools/invoke-worldlook-mcp.ps1
$process = Get-S1McpProcess
$game = 'C:/Program Files (x86)/Steam/steamapps/common/Schedule I'
$context = @{
    Process = $process
    GameExe = [IO.Path]::GetFullPath((Join-Path $game 'Schedule I.exe'))
    EvidencePath = [IO.Path]::GetFullPath('./screenshot-request.jsonl')
}
Assert-S1McpProcess $context (Get-S1McpProcess)
# Use a fresh evidence filename. This throws instead of overwriting existing evidence.
$created = [IO.File]::Open($context.EvidencePath, [IO.FileMode]::CreateNew)
$created.Dispose()
$client = [Net.Sockets.TcpClient]::new()
try {
    if (-not $client.ConnectAsync('127.0.0.1', 8765).Wait(3000)) { throw 'Connect timeout' }
    $stream = $client.GetStream()
    $stream.WriteTimeout = 3000
    $id = [Random]::Shared.Next(10000, 1000000000)
    $request = @{id=$id; method='capture_screenshot'; params=@{timeout_ms=5000}}
    Write-S1McpFrame $stream $request
    $response = Read-S1McpResponse $stream $id ([DateTime]::UtcNow.AddSeconds(8)) $context
    Write-S1McpFrame $stream @{id=$id; status='received'}
    Assert-S1McpProcess $context (Get-S1McpProcess)
    Write-S1McpEvidence $context @{process=$process; request=$request; response=$response}
    if ($response.error) { throw ($response.error | ConvertTo-Json -Compress) }
    $response.result | ConvertTo-Json
    Get-FileHash -LiteralPath $response.result.path -Algorithm SHA256
} finally { $client.Dispose() }
```

Run PowerShell explicitly (`pwsh -NoProfile -ExecutionPolicy Bypass`). No connector
exposing this new method was available during implementation. The snippet above is
an inspected-transport usage example, **not a completed live test**. After success,
open/decode the returned PNG independently and compare its dimensions to the result.
Do not substitute a sprite image or a previous screenshot.

The server deadline starts when the main-thread handler runs; queue starvation is
bounded by the client's transport timeout. Unity/native calls are not preemptible.
A transport timeout does not establish no disk effect; inspect the evidence folder
before retrying. A narrow race at completion versus timeout can leave a completed
file without a success response. Disk/path checks do not defend against a concurrent
hostile filesystem administrator replacing ancestors between validation and write.

## Build/deploy safety

This fork's historical post-build target can **forcibly kill the game**. Do not use
its plain build/deploy path. `S1NoDeploy=true` alone is not honored by that target.
Use both flags below, only while the game is closed:

```powershell
if (Get-Process -Name 'Schedule I' -ErrorAction SilentlyContinue) { throw 'Game running' }
dotnet build ThirdParty/S1MCPServer-master/S1MCPServer/S1MCPServer.csproj `
  -c 'Release IL2CPP' -p:S1NoDeploy=true -p:RunPostBuildEvent=Never
```

Deploy the built DLL through `Tools/deploy-mod.ps1` with absolute `-ProjectDir`,
`-TargetPath`, `-ProjectName S1MCPServer` and `-Configuration 'Release IL2CPP'`.
The helper checks the game is closed, backs up an existing destination, stages/hashes
and emits a receipt. Never copy the build output's reference DLLs into Mods.
Launch only `steam://rungameid/3164500`. Menu capture is enough; no save loading needed.

## Verification record — 2026-10-26

- Inspected live installed ScreenCaptureModule and ImageConversionModule using
  ilspycmd 9.1.0.7988. Typed APIs: `CaptureScreenshotAsTexture(int)` returns `Texture2D`;
  `EncodeToPNG(Texture2D)` returns `Il2CppStructArray<byte>`; managed copy via `ToArray()`.
- Latest pre-existing loader log reported game `0.4.7f12`, Unity `2022.3.62f2`,
  MelonLoader `0.7.3`, Il2CppInterop `1.5.1-ci.845`.
- RED/GREEN: deferred publication/unique PNG, concurrent guard/timeout/late drain,
  invalid dimensions/deadlines/PNG, real Windows junction refusal, end-of-frame
  scheduling/resource cleanup, parameter rejection and pending shutdown. Production
  pure coordinator and handler are linked into tests; Unity/Melon/response queue
  boundaries are substitutes, **not proof of live rendering**.
- `dotnet test Source/Tests/S1MCP.Tests/S1MCP.Tests.csproj`: **62 passed**, 0 failed.
- Release IL2CPP no-deploy compile: **0 errors, 180 existing-file warnings**.
  Built DLL SHA256: `153AD83DE5C611C1617845136B14BE4129C92DE51F61FA1CAD25401EB0C99E88`.
- Important execution error: game was initially closed but later started externally
  (PID 3404, local start 19:03:53). A shell command used `;` after the process guard,
  so its failing guard did not stop the following no-deploy test/build. No copy, kill
  or launch occurred. Future checks must fail closed (`&&` or one PowerShell script).
- At 19:08 local the game remained running. `Mods/S1MCPServer-IL2CPP.dll` was absent
  and no localhost:8765 listener was present. Therefore there was **no installed DLL
  to back up/hash**, no deployment, and **no real screenshot/PNG path to report**.
- Independent review, isolated clean-checkout verification, closed-game rebuild,
  authorized deployment and live PNG readback remain pending. WorldLook, water,
  saves, Blender, unrelated working changes, versions and Git history were untouched.
