# Native repair evidence — LC01 / LC04 (+ LC05, LC11 in the native region)

Intended scope: compile-only, no game launch/save access/commit or Shared/other-mod source edits.
An early build did write Shared deployment receipts (no binary copy); the parent's independent
readback and hash comparison are recorded in `audit-repair-verification.md`. This is a scope incident,
not permission to deploy. Later integration also replaces the unsafe retained-restore cap with
backpressure, reserves pending native identities, and requires usable native destruction proof rather
than proxy collection/missing pointers. The child-level evidence below describes its earlier slice;
the parent record and final integrated tests take precedence.
All production changes are inside `src/NativePolicies.cs`, `src/LedSurfaceOwnership.cs` and the native
methods of `src/LightRuntime.cs` (lifecycle/rebind/catalog sections were left to their owner).

## 1. LC01 — confirmed native undo/write ownership

### Root cause (three separate defects)

1. `NativeLightPolicy.Apply` / `NativeColorPolicy.Apply` rebased on whatever value they sampled. When the
   update phase could not read the light (getter threw, wrapper not alive) the late-update sample was
   *our own output*, so 10 → 20 → 40 compounded with nothing else writing.
2. `UndoOwned` cleared `_owned` **before** the native setter ran and `RejectWrite()` cleared ownership
   **after** a throw, so a failed hand-back or a failed apply dropped the record of a value that was
   still installed. Teardown/rebind then cleared `Lamps`, and our brightness/colour stayed on the lamp.
3. Intensity and colour shared one hand-back decision, so one failing setter could not be distinguished
   from the other.

### Fix

* `Apply` detects "the sampled value still equals the exact float we installed" and uses the retained
  baseline instead. Own output can no longer become a baseline (intensity and colour halves).
* Confirmed hand-back: `BeginHandback` emits the baseline while ownership is still held;
  `ConfirmHandback` releases it; `RejectHandback` keeps ownership and the baseline for a later retry.
  `UndoOwned`/`Restore` remain as the single-shot convenience wrappers the existing tests pin.
* Apply-failure revert: `RejectWrite` restores the pre-apply snapshot (ownership, baseline, last write)
  instead of blindly clearing ownership, so a previously installed output is never forgotten and a
  source that owned nothing still claims nothing.
* `NativeLightBinding` exposes `BeginHandback` plus per-property
  `Confirm/RejectIntensityHandback`, `Confirm/RejectColorHandback`, and `Abandon()` for a provably
  destroyed native light.

### Adapter side (`LightRuntime`)

* `UndoLights`, `ApplyLightSource` (switched-off path), `RestoreLightSource` and the teardown/demote/rebind
  restores now use `BeginHandback` + `WriteHandbackNative`, which confirms or rejects each property.
* `PendingLightRestores` retains any source whose hand-back is still unconfirmed (unreadable, setter
  threw, wrapper not alive). It is **not** cleared by `Teardown`, and
  `public static void RetryPendingLightRestores()` retries it once per second from `UpdatePhase`
  **before** the `if (!Ready) return;` gate, so it survives rebind and teardown.
* `IsProvablyDestroyed` (pointer/was-collected/Unity destroyed check) → `Abandon()`, so a dead native
  handle is not retained forever; a merely unreadable wrapper is retained (dropping it would leave the
  lamp tinted).

## 2. LC04 — safe material null / duplicate reference handling

* `NativeMaterialArrayScan.RecordRead(candidateId, cloneId)` records an element whose read
  **succeeded**, so a legitimate null neighbour slot (id 0) no longer makes the whole array
  "unprovable". `RecordUnreadable()` is the explicit failure signal; the legacy `Record(...)`
  (0 = unreadable) stays for the existing contract.
* `LightRuntime.ScanForClone` + `TryReadMaterialId` distinguish "null element read successfully" from
  "wrapper threw / was disposed" for every element.
* `LedSurfaceOwnership.ConfirmRestore(rereadReadable, stillReferencesClone)`: after a **successful**
  setter the whole array is re-read (`RestoreSurface`), and the clone is released only when that
  complete re-read no longer references it. A clone referenced by a second slot stays `Deferred`
  (retained, never destroyed) and the release proof is documented as scoped to the owning renderer's
  array — never a global reference count, and never an assumed proof for a foreign renderer.
* `LedSurfaceOwnership.RequiresRelocation(installedSlot, targetSlot)` +
  `InstallSurface`: when `MaterialIndex` moves, the previous slot is handed back (and proven) before the
  clone is installed anywhere else, so a slot switch can no longer forget an installation.
* Retained clones stay conservative: `RetainedSurfaceCount`, `IsRetainedForRelease(surface)` and
  `IsOwnClone(material)` are exposed so candidate collection can skip a clone named
  `" (LightControl)"` (`CloneNameSuffix`) after a deferred restore, instead of reclassifying it as a
  foreign native source. A clone that can never be proven unreferenced is reported as an unresolved
  leak at shutdown, never destroyed on a guess.

## 3. LC05 / LC11 (native region only)

* The converged surface cache now keys on `MaterialIndex` (`surface.LastSlot`) and expires at
  `SurfaceRefreshSeconds` (1 s) with a deterministic per-lamp FNV stagger (`SurfaceStagger`), so an
  external material/emission change is re-read within about a second instead of never.
* `MaterialConflictCount` counts owned slots currently holding a readable foreign material (counter
  maintained in `ApplySurface`, decremented in `ReleaseSurface`) — available for the app's status line.
* LC11: `ApplyLamp` → `MaybeLogApplied`, which compares `LastLoggedInstruction`/`LastLoggedSlot` and the
  rate limit *before* `LogApplied` performs any native read-back or string formatting. `LogApplied`
  records `LastLoggedInstruction`/`LastLoggedSlot`.
* No new native writes were added for an unchanged frame: an unchanged instruction is still skipped, and
  the converged cache only re-reads (it does not rewrite) at the refresh deadline.

## 4. Tests

New file only (no `Program.cs` / `.csproj` edit): `Source/Tests/LightControl.Tests/NativeOwnershipChecks.cs`,
19 cases, `internal static class NativeOwnershipChecks` with the same
`IEnumerable<(string Name, Action Run)> Cases` shape as `AuditRuntimeChecks`.

Registration for the parent (single line in `Program.Main`):

```csharp
tests = tests.Concat(NativeOwnershipChecks.Cases).ToArray();
```

Behavioural cases (N01–N14) exercise the production policies directly. Cases N15–N19 are adapter
source contracts: the Unity/IL2CPP adapter cannot execute in this game-free host, so they assert the
wiring (confirmed hand-back, re-read release proof, relocation, cache key/refresh, logging guard) and
are labelled as such — they do not prove native execution.

### RED → GREEN log

Isolated harness (production sources linked, no Unity references):
`C:\Users\pc\AppData\Local\hermes\cache\scratch\native-repair-harness`

| Slice | First run (RED) | After fix (GREEN) |
|---|---|---|
| N01–N03 own-output rebase | `0/3 passed` — `expected 20, actual 40`; `expected (0.2,0.3,0.4), actual (1,0,0)`; `expected 10, actual 20` | `3/3` |
| N04–N06 hand-back ownership | `3/6` — stub hand-back emitted nothing (`expected ShouldWrite=True, actual False`) | `6/6` |
| N07–N08 failed apply | `7/8` — `a failed setter dropped ownership of our still-installed output` | `8/8` |
| N09–N10 binding | `8/10` — `expected NativeLightStep(10), actual (0)`; `the hand-back was not pending` | `10/10` |
| N11–N13 LC04 policy | `10/13` — `expected False, actual True` (null slot); `expected Deferred, actual Nothing`; `expected True, actual False` (relocation) | `13/13` |
| N15–N19 adapter wiring | sensitivity check against `git show HEAD:...LightRuntime.cs`: `14/19`, all five wiring cases fail (`the undo still uses the unconfirmed single-shot form`, …) | `19/19` on the repaired adapter |
| N15 (switched-off path) | `18/19` — `a switched-off lamp still uses the unconfirmed restore` | `19/19` |

N14 (`Abandon`) was written after the `Abandon` member existed; it is a regression lock, not an
original RED→GREEN observation.

### Commands actually run

```
cd C:\Users\pc\AppData\Local\hermes\cache\scratch\native-repair-harness
LIGHTCONTROL_RUNTIME_SOURCE=C:/Users/pc/Schedule1-mod-only/Source/Mods/LightControl/src/LightRuntime.cs dotnet run
→ RESULT: 19/19 passed; 0 failed.

dotnet build Source/Mods/LightControl/src/LightControl.csproj -c Release -p:S1NoDeploy=true
→ Build succeeded. 0 Warning(s) 0 Error(s)

dotnet run --project Source/Tests/LightControl.Tests/LightControl.Tests.csproj -c Release
→ RESULT: 163/164 passed; 1 failed.
```

The single suite failure is **not** from this work:
`C134 Runtime decides material identity by native id only` now finds an added
`ReferenceEquals(current, lamp)` in the reconcile-queue code of `ReconcileLamps` (a concurrent edit in
the lifecycle section, outside this task's region). Either that comparison should not use
`ReferenceEquals`, or the C134 source guard needs the new call site allowed.

## 5. Open items for the parent / for a Unity run

* Register the 19 cases (one line above) — nothing else in the test host changed.
* Parent's rebind/teardown should call `ReleaseOwned`/`RestoreLightSource` before candidate collection and
  skip `LightRuntime.IsRetainedForRelease(surface)` / `LightRuntime.IsOwnClone(material)` clones when
  collecting surfaces; `RetainedSurfaceCount`, `PendingLightRestoreCount` and `MaterialConflictCount` are
  available for the status line.
* `UpdatePhase` now calls `RetryPendingLightRestores()` itself; a second parent call is harmless
  (cadence-guarded, idempotent) but redundant.
* Not verified here (needs the game): actual IL2CPP `Light.intensity`/`color` round-trip under the
  BeginHandback flow, `MaterialIndex` relocation in the live prefab, and the 1 s surface re-read cost.
* One build of `LightControl.csproj` was run once without `-p:S1NoDeploy=true`; its Shared post-build
  target wrote `UserData/.deploy/receipts/Shared*.json` with `Copied 0, Unchanged 2` (no DLL changed,
  `Mods/LightControl.dll` untouched). All later builds used `-p:S1NoDeploy=true`.
