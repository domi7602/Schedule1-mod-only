# s1 — Schedule I CLI (initial project)

Local client for the **existing S1MCP bridge**, not a new MelonLoader fork and not
CLI-Anything. Python **3.11+ standard library only**; no pip install, Hermes changes,
package mutation, deployment or game launch is necessary to run the client.
WorldLook setters and optional Windows process diagnostics also use PowerShell 7.

## Run

From the repository root, in PowerShell:

```powershell
./Tools/s1-cli/s1.cmd --help
./Tools/s1-cli/s1.cmd --json doctor
./Tools/s1-cli/s1.cmd --json status
./Tools/s1-cli/s1.cmd --json capabilities
./Tools/s1-cli/s1.cmd --json time get
./Tools/s1-cli/s1.cmd --json worldlook status
./Tools/s1-cli/s1.cmd --json --timeout 30 capture
```

Equivalent, including from Git Bash: `python Tools/s1-cli/s1.py --json doctor`.
The launcher uses `python` on PATH. For a different interpreter invoke `s1.py`
with its explicit executable. An isolated venv is optional, not needed.
To use the bare command `s1` for **this PowerShell session only**:

```powershell
$env:PATH = "$((Resolve-Path Tools/s1-cli).Path);$env:PATH"
s1 --json doctor
```

No installer or persistent PATH modification is performed. Global options go
**before** the subcommand: `--json`, `--timeout SECONDS` (0.1–120, default 8),
`--port PORT` (loopback only, default 8765), `--game-dir PATH`.
Game directory defaults to `SCHEDULE1_PATH`, otherwise the normal Steam location.

## Commands and actual backend contracts

| Command | Implementation / limits |
|---|---|
| `doctor` | Local Python/pwsh/game/DLL presence, bounded `list_methods` probe, optional Windows game-process and loopback-listener identity. Missing process permissions/timeout are **unknown**, not “game closed”. Offline diagnosis retains local data and exits nonzero. |
| `status` | `get_game_state`: scene, network status, loaded mods. Its historical `game_time` field is a server **placeholder**, NOT the clock; use `time get`. Scene/network strings do not prove a ready save. |
| `capabilities` | Actual `list_methods` result. Listed endpoints prove registration only, not working effects. No invented capabilities or implicit deployment. |
| `time get` | `get_time_of_day`, native `time_hhmm`, `time_minutes`, `display`. Menu `available:false` is a successful read of unavailable clock state. No time mutation/freeze. |
| `worldlook status` | Exact parameterless `WorldLook.Automation` / `WorldLook.WorldLookAutomation.GetStatus` via `call_method`; validates the JSON status schema and Boolean fields. |
| `worldlook on/off` | Adapter to [the existing guarded driver](../invoke-worldlook-mcp.ps1). Explicit save name/path, PID/start time and new evidence file are mandatory. No arbitrary `call_method` CLI. |
| `lighting inspect --object /Exact/Hierarchy/Light` | Exactly three `get_component_property` reads on `UnityEngine.Light`: `color`, `intensity`, `enabled`. A caller-supplied **known absolute Unity hierarchy path** is required. No invented Sun path, broad scene discovery, materials getter, recursive reflection or writes. This is a selected-light sample, not all lighting/RenderSettings or visual-effect acceptance. |
| `capture` | Discover `capture_screenshot` first, then invoke it with `timeout_ms` only. `--timeout-ms` 100–15000 (default 5000). Verify the returned local PNG before success. No desktop/sprite fallback. |

Lighting uses the inspected fork's active `GameObject.Find(path)` lookup. Missing
or inactive targets fail; duplicate hierarchy paths remain a Unity ambiguity.
The CLI bounds request count and duration; it cannot preempt a stalled Unity getter.
Samples are sequential, **not an atomic frame snapshot**.

### WorldLook mutation safety

First read `doctor` for the selected listener's `Id` / `StartedUtc` and
`worldlook status` for `saveName` / `savePath`. Confirm these are the intended
installation and owner's save. Then supply those exact values (placeholders below
are deliberately not a guessed save):

```powershell
s1 --json --timeout 30 worldlook off --save-name "<exact name>" --save-path "C:/<exact native save path>" --game-process-id <PID> --game-started-utc "<StartedUtc>" --evidence "C:/<new evidence file>.jsonl"
```

Use `on` for explicit ON. The existing driver checks listener process/executable
and start time before/after requests; ready singleplayer Main scene and save
identity before mutation and during independent readback; requested state and
fault flags after mutation. The adapter requires port 8765. Evidence is CreateNew,
never overwritten. Correlated driver requests/readbacks live in that JSONL;
the outer `requests` list is empty for delegated setters. A timeout/failure is
**not proof of no effect**. No mutation is retried automatically. Read status and
evidence before any further action. ON/OFF changes session state, not saved config.
The process/save check and setter cannot be made atomic across the existing bridge;
retain the driver's transition/readback safeguards and do not switch saves mid-action.

### Capture verification

The endpoint is implemented in the local fork but may not be installed. See its
[actual capture contract](../../ThirdParty/S1MCPServer-master/S1MCPServer/SCREEN-CAPTURE.md).
The CLI requires an absolute local PNG in the selected game's
`UserData/S1MCP/Evidence/Screenshots/` directory, rejects symlink/junction ancestors,
checks metadata, signature, chunk CRCs, dimensions and bounded zlib pixel stream,
and decodes every RGB/RGBA8 non-interlaced scanline/filter. Output includes file and
decoded-pixel SHA256. Other PNG formats are explicitly unsupported rather than
claimed verified. Limits match the endpoint: 8192 per edge, 16,777,216 pixels, 64 MiB.
The CLI reads the actual server file; it does not overwrite/copy evidence.

Use a larger whole-command timeout for high-resolution PNG decoding. A client
capture timeout can still leave a completed server file: inspect the evidence
folder before retrying. Local filesystem calls cannot be forcibly preempted and
path validation is not a security boundary against a hostile local administrator.
A decoded image is not proof of a perceptual golden-hour change. No matched-camera
comparison or verified time-freeze mechanism is implemented.

## Output and exits

`--json` produces one JSON object on stdout (except explicit `--help`, which is
human-readable). Without it, the same object is indented. Stable schema v1:

```json
{"schema_version":1,"ok":true,"command":"time","data":{"available":false,"reason":"..."},"error":null,"requests":[]}
```

The example is a **schema illustration**, not a live result. Real direct bridge
requests contain random numeric IDs, exact request parameters and correlated
responses in `requests`. Successful replies and server errors are acknowledged.
Transport: little-endian 4-byte UTF-8 JSON length prefix; max 1 MiB response frame,
max 8 frames including recognized negative-ID heartbeats, fresh connection per
request, one monotonic whole-command deadline, no automatic retries. Protocol
mismatch stops immediately. There is no remote host option/authentication layer;
use the trusted local bridge and avoid simultaneous clients competing for it.

| Exit | Meaning |
|---|---|
| 0 | Requested read/verification completed (inspect `available`, readiness, etc.) |
| 2 | Usage or missing explicit mutation identity |
| 3 | Bridge/dependency unavailable |
| 4 | Deadline; effect may be unknown |
| 5 | Invalid/mismatched protocol or result schema |
| 6 | Unsupported endpoint or explicit bridge rejection |
| 7 | Guarded WorldLook driver failure (diagnostic stderr/evidence retained) |
| 8 | PNG evidence validation failure |

`data` can contain partial diagnosis on failure; only top-level `ok` is completion.
Normal launch/load/save/time-setting/arbitrary execution commands do not exist.
Capabilities output may list unsafe server endpoints; that does not expose them
as CLI commands. JSON/evidence can contain local paths and save names; review before sharing.

## Tests

```powershell
python -m unittest discover -s Tools/s1-cli/tests -v
pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/Tests/WorldLookMcp.Tests.ps1
```

Tests launch the real CLI as subprocesses against synthetic **actual loopback TCP**
servers, checking little-endian fragmentation, IDs, acknowledgements, heartbeat,
invalid frames, deadlines, errors, safe dispatch, Windows launcher, doctor and
PNG decoding/corruption. Only the setter's process boundary is substituted; the
second command exercises the existing driver's 81 synthetic process/save/readback
checks. None proves Unity or in-game rendering. The new Python suite is standalone;
it is not yet wired into the repository's PowerShell Pure gate.

## Local verification / remaining limits

Initial RED/GREEN cycles were run for CLI help, correlated status, capability/time
reads, doctor, WorldLook, selected light inspection, guarded setter adapter,
capture verification, nonfinite JSON rejection, process diagnosis and launcher.
All final results and real local probes are recorded in `VERIFICATION.md`.
No DLL/source change, deployment, launch, save load, save/time mutation, package
installation, commit or push is part of this project. Other working edits remain
untouched. Live endpoint acceptance still requires a separately authorized bridge
installation and running game; unavailable results are not replaced by fixtures.
