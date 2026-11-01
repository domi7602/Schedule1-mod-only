# TownHallInterior live automation runbook

This runbook automates the native door baseline and later staging smoke checks through
three channels: the PowerShell runner `Tools/townhall-live-baseline.ps1` (orchestration,
hashing, launch, bridge transport, evidence), the in-game S1MCP bridge (state reads and
bounded single-shot mutations), and a separate CUA session (real Windows input and
desktop captures). The agent in the orchestrating session reads the returned PNGs and
classifies the dialogs. No channel may deploy, save or retry a mutation on its own.

## Preconditions

- Steam is offline (owner-set) and the Steam cloud conflict for app 3164500 stays
  unresolved until the owner decides; the runner never touches Steam cloud state.
- The session uses the owner-declared test slot `SaveGame_1` ("Dominik") only.
- Vanilla runs have **no** `TownHallInterior.dll` installed; staging runs need an
  explicit owner go and are a separate phase.
- The game is closed before `Launch`; the runner refuses a second instance.

## Evidence layout

One folder per run under
`.backups/townhall-modal-save-20261028/live-auto-<UTC-stamp>/` containing `run.jsonl`
(requests, correlated responses, failures, `mutationRetried=false` on every write),
`save-manifest.json`, `save-compare.json`, copied captures, `MelonLoader-Latest.log`
and `evidence-manifest.json` with SHA-256 of every evidence file.

## Sequence

All commands run from the repository root with explicit PowerShell:

1. `pwsh -NoProfile -ExecutionPolicy Bypass -File Tools/townhall-live-baseline.ps1 -Action PreHash -EvidencePath <run>/run.jsonl -SnapshotDir <run>/save-snapshot`
2. `-Action Launch` (steam:// launch, waits for the process)
3. `-Action Bridge -TimeoutSeconds 120` (single listener on 127.0.0.1:8765 owned by the
   game exe; readiness via `get_game_state`)
4. From the menu: `-Action Load -SaveName Dominik -SavePath <Saves>\76561199109519851\SaveGame_1`
   (unique name+path match, slot echo, Main-scene readiness; no retry). Skip when the
   session resumes an already loaded world.
5. `-Action Teleport -X 52.8 -Z 31.0 -KeepY` — approach point west of the door
   (door at x≈54.3, z≈31). The vertical coordinate is reused from the live player read;
   verify arrival with a capture; a fall or clip aborts the run (documented, no retry).
6. Optional `-Action Time -Hhmm <value>` to reach a zero-NPC window. Corroborate with
   state/NPC reads; an empty read does **not** prove absence.
7. CUA handoff (separate session, steps below): aim the door center, press `E` once,
   wait ~1 s. Orchestrator: `-Action Screenshot` plus the CUA desktop capture.
8. Agent vision classifies the capture: `Knock` prompt/dialog only, NPC selection list,
   nothing, or anything else (abort and preserve).
9. CUA presses `Escape`; orchestrator captures again; expect the closed state.
10. Repeat 5–9 for each NPC condition actually reachable in the session (0 / 1 / 2+);
    record which conditions were observed, never claim unobserved ones.
11. `-Action Quit` (graceful window close; a non-exiting game is inspected, not killed),
    `-Action PostHash -ManifestPath <run>/save-manifest.json`, `-Action Collect`.

## CUA handoff contract

The CUA session reads this runbook and performs only: menu navigation by save name when
`Load` is unavailable, mouse aim on the door center derived from the latest capture,
single `E` and `Escape` presses, and desktop window captures paired with UTC time and
file hash. Known CUA limits (dropped F13–F15 virtual-key codes) do not affect E/Escape.
The CUA session must not type into consoles, open Steam overlays or resolve cloud
conflicts.

## Abort rules

Stop and preserve evidence on: MCP response-ID mismatch, bridge/process identity
change, any timeout during a mutation (outcome unknown — inspect before another
write), teleport fall/clip, a dialog that matches no expected class, foreground
windows obscuring the capture, save hashes changing beyond the documented expectation
(the 2026-10-28 manual baseline wrote nothing), or any Steam cloud prompt.

## What a run proves — and what it does not

A completed run proves: the real E/Escape input path at the door, the dialog class per
observed NPC condition, save-tree integrity across the session, and bridge/transport
behavior. It does **not** prove: NPC absence from reads alone, gameplay acceptance of
`teleport_player`/`set_time_of_day` (helper mutations with readback only), any
TownHallInterior component behavior in vanilla runs, or multiplayer/authority paths.
Staging runs additionally assert the log lines of the installed hook and that native
behavior is unchanged; they still prove no room publication until gates 3–5 pass.
