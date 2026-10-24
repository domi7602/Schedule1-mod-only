# LightControl LC12 persistence safety — evidence

> Parent integration supersedes the child-level absence proof below: `DirectoryNotFoundException`
> alone is not absence. The persistent disk tests reproduce an unreachable-drive write-gate opening
> (RED) and verify the repaired full-ancestor listing (GREEN). The final host links the real adapter,
> real Shared writer and logger, with only loader environment/logging and native slot-probe shims.
> See `audit-repair-verification.md`; old concurrent build/test failures below are historical.

## Scope

Owner-approved, local-only work: LightControl source, tests and documentation plus compile-only
builds. No deployment, game launch, live-save access, commit, push, dependency change, `Shared`
edit, schema change or version bump. Runtime/UI files were not touched by this task.

Baseline before LC12: 151/151 console-host checks (the host has since grown through concurrent
work; see "Repository suite" below) and a clean Release mod build.

## Defects repaired

| # | Defect | Repair |
|---|---|---|
| D1 | `LightPersistence.Load` used `File.Exists` as proof of absence. `File.Exists` returns `false` for unreadable/unreachable paths, so unknown bytes were reported as "missing data" and the write gate opened over them. | Load now uses read outcomes (`PersistenceReadOutcome`): `Present`, `Unreadable`, or `Absent` only after the parent directory was actually listed. |
| D2 | A corrupt main file was parsed directly and blocked the session; the `.bak` file was never validated or used, even when it was the only good copy. | A main that was read but fails lamp-data validation falls back to the backup only after the backup passes its own full validation (schema, save id, every lamp). |
| D3 | A valid backup could mask intact-but-not-ours data (unknown schema or foreign save id), replacing it silently. | A readable main with an unknown schema or a foreign save id blocks even when the backup is valid; the backup is not even read in that case. Unreadable main also blocks. |
| D4 | A backup-recovered session would have written via `SafeStorage.SaveTextAtomic`, whose `.bak` rotation copies the corrupt main over the one good backup. | After a successful recovery the adapter writes the main file through `.tmp` + `File.Move` **without** rotating `.bak`; the validated backup stays intact. All files are kept on failure. |
| D5 | Save I/O and the native slot probe could surface as exceptions from the adapter. | `Save` wraps the slot probe, serialization and write in guards; every failure returns `false` with a warning. |

Also added: `LightPersistence.LastLoadResult` (`PersistenceLoadResult` with `Error`/`Detail`) and
`LoadedFromBackup`; the existing `bool Load(...)` / `bool Save(...)` signatures are unchanged.
`PersistenceError` and the save schema are unchanged.

## Load decision table (pure, `PersistenceSlotPolicy.PlanLoad`)

| main read | main classification | backup | result |
|---|---|---|---|
| Present | valid | not read | load main |
| Present | unknown schema | not read | **block** `UnsupportedSchema`, data retained |
| Present | foreign save id | not read | **block** `WrongSave`, data retained |
| Present | corrupt JSON / invalid content | valid | **load backup** (fully validated) |
| Present | corrupt / invalid | foreign save | **block** `WrongSave` |
| Present | corrupt / invalid | unknown schema | **block** `UnsupportedSchema` |
| Present | corrupt / invalid | absent | **block** `InvalidJson` |
| Present | corrupt / invalid | unreadable | **block** `InvalidState` |
| Unreadable | — | not read | **block** `InvalidState` (absence unproven) |
| Absent (proven) | — | valid | **load backup** |
| Absent (proven) | — | unreadable | **block** `InvalidState` |
| Absent (proven) | — | corrupt | **block** `InvalidJson` |
| Absent (proven) | — | absent (proven) | confirm missing, gate `Ready` |

The plan performs no I/O and changes no gate; load never repairs, renames or rewrites a file.
`JsonStatePersistence.Inspect(json, saveId)` performs the classification without touching the
store or the gate, and `LoadJson` re-validates against its own session/save id.

### Recovery write policy (chosen over read-only)

After a backup recovery the session stays writable, but the first save uses
`LightPersistence.PreserveBackup`: `.tmp` → `File.Move(temp, main, overwrite)` with **no** `.bak`
copy, because the only rotation source would be the corrupt main. The corrupt main therefore
stays byte-identical on disk until that save succeeds; the good backup is never overwritten. If
that write fails, every file is kept, `Save` returns `false` and the next autosave retries. This
was chosen over a permanent read-only recovered session because it preserves the owner's changes
while still guaranteeing the validated backup survives the first save. If the protected write is
not possible (permissions), the session remains recovered in memory and the backup is untouched.

## RED → GREEN evidence (executed)

Harness (scratch, not shipped): `C:\Users\pc\AppData\Local\hermes\cache\scratch\lc12-*`.
Projects link the **real** `LightPersistence.cs`, `PersistenceSlotPolicy.cs`,
`JsonStatePersistence.cs` and model sources; only MelonLoader/Il2Cpp dependencies are replaced by
shims (`ModLogger`, `SafeStorage.GetUserDataPath`/`SaveTextAtomic`, `SaveSlots`). The shim's
`SaveTextAtomic` mirrors the real ordering (`.tmp` → best-effort `.bak` copy → overwrite `Move`,
no fsync) and can inject an I/O fault.

### 1. Adapter probe, same assertions before and after (`lc12-red-before` → `lc12-red`)

The `before` project links a code-verbatim copy of the pre-repair adapter; both runs use valid
fixtures (schema 1, `slot-0`, one lamp with a legal kind/color/intensity).

Before (RED, valid fixture):

```
FAIL RED-1 Directory instead of file must not be treated as missing  -> load result: expected 'False', got 'True'
FAIL RED-2 Corrupt main must recover from a validated backup         -> load result: expected 'True', got 'False'
FAIL RED-3 A save after recovery must keep the good backup           -> load failed
PASS RED-4 Locked main must block rather than confirm missing        (already correct before)
1/4 RED probes behaved as expected (all must FAIL before the fix).
```

After (GREEN, identical assertions against the repaired adapter):

```
PASS RED-1 ... PASS RED-2 ... PASS RED-3 ... PASS RED-4 ...
4/4 RED probes behaved as expected
```

RED-1 creates a **directory** at the lamp-file path: `File.Exists` is `false`, reading throws
`UnauthorizedAccessException`. This is the executed reproduction of "File.Exists false does not
mean missing"; it stands in for an ACL/locked/unreachable path that cannot be produced without
changing machine permissions.

### 2. Adapter regression suite (`lc12-persistence`, 14/14)

Covers: directory-as-file blocks; corrupt main recovers; `LastLoadResult`/`LoadedFromBackup`
preserved; save after recovery keeps `.bak` byte-identical and writes the recovered lamp; unknown
schema and foreign save-id mains block despite a valid backup; corrupt main with foreign backup
blocks; proven absence (both parent directories listed) confirms missing and enables writes;
locked main blocks and is kept; unsupported-schema backup blocks; stale slot refuses to write;
injected write failure and throwing slot probe return `false` without throwing; load leaves main,
`.corrupt` and `.tmp` untouched.

### 3. Repository helper (`Source/Tests/LightControl.Tests/PersistenceRepairChecks.cs`, 15/15)

Pure `PlanLoad`/`Inspect` cases plus one supplementary source contract (`File.Exists(` is absent
from the adapter; plan/last-result/backup-preservation markers present). `LC12-15` is a
regression guard for the existing C62 contract: an explicit `ReportReadFailure()` after a
successful load must block the gate. Sensitivity check performed: re-adding a "keep Ready" guard
to `ReportLoadFailure` made LC12-15 fail (`Expected 'Blocked', got 'Ready'`) and removing it made
it pass. (LC12-15 is a guard pinned after the parent reported C62 failing, not part of the
original RED.)

### Registration for the console host (parent integration; `Program.cs`/csproj untouched)

```csharp
foreach (var (name, run) in PersistenceRepairChecks.Cases())
    tests.Add((name, run));
```

`Cases()` is self-contained: no package, no file writes, and the source-contract case resolves
the repository by walking up from `AppContext.BaseDirectory` (or `LC12_REPO`).

## Repository suite and build

```
dotnet run --project Source/Tests/LightControl.Tests/LightControl.Tests.csproj -c Release \
  -p:S1NoDeploy=true -p:BuildProjectReferences=false
```

- `C41`, `C42`, `C43`, `C61`, `C62` all PASS. C62 initially failed when `ReportLoadFailure`
  retained a `Ready` gate; the guard was removed and the failure is now covered by LC12-15.
- The host is being extended concurrently, so its total moves. The last run during this task was
  **160/164**; every failure (`C99`, `C120`, `C134`, `C149`) is UI/runtime work owned by other
  tasks, and C62 is green. Persistence-related C41/C42/C43/C61/C62 all pass.

```
dotnet build Source/Mods/LightControl/src/LightControl.csproj -c Release --no-incremental \
  -p:S1NoDeploy=true -p:BuildProjectReferences=false
-> Build succeeded. 0 Warning(s), 0 Error(s)
```

That clean build was captured after the LC12 source changes and before the concurrent
runtime/native edits landed. At hand-off `LightRuntime.cs` is mid-edit by another task and every
current compile error references `LightRuntime.cs` only (18 errors, none in
`LightPersistence.cs`, `PersistenceSlotPolicy.cs` or `JsonStatePersistence.cs`); those three
files also compile cleanly in the linked test host and in the scratch harnesses.

## Shared contracts inspected, not changed

- `SafeStorage.GetUserDataPath` = `MelonEnvironment.UserDataDirectory/<ModName>/<file>`; no player
  or world identity is added. A different world in the same numeric slot is still indistinguishable
  in this format; no invented world id or migration was added.
- `SaveSlots.GetActiveSlotNumber` returns `>= 0` or `-1` (probe errors are already caught there);
  the adapter still requires the captured slot to equal the active slot, including `-1`.
- `SafeStorage.SaveTextAtomic` = `.tmp` → best-effort `.bak` copy → overwrite `Move`, failures
  swallowed, **no fsync**. Its name is not proof of crash durability, and the `.bak` copy can fail
  without vetoing the replace. The adapter therefore validates contents itself and does not claim
  durability beyond the file operations performed.
- `SaveSlots`, `SafeStorage` and every other `Shared` file are unchanged.

## Evidence limits

- The adapter harness shims `SafeStorage`/`SaveSlots`/`ModLogger`; it does not prove the real
  MelonLoader path, the native save pipeline or the game's own autosave timing.
- No game session, PhoneApp or save directory was touched; nothing was deployed.
- The directory-as-file case is an executed stand-in for an inaccessible path, not an ACL change.
- No crash/fsync durability claim is made; `.tmp`/`.bak` semantics are exactly those documented
  above.
