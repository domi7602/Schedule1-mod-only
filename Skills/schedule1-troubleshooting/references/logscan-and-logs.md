# Latest.log Triage & Diagnostics

The canonical method for inspecting runtime diagnostics in Schedule I is querying `<GameDir>\MelonLoader\Latest.log`. These native PowerShell commands provide fast, zero-dependency log triage without game-freeze risks.

---

## 1. Quick Start (PowerShell One-Liners)

```pwsh
$log = "$env:SCHEDULE1_PATH\MelonLoader\Latest.log"

# Default: scan Latest.log for errors and warnings (last 200 lines)
Get-Content $log -Tail 200 | Select-String -Pattern '\[ERROR\]|\[WARNING\]|Exception|WasCollected'

# Filter to a specific mod (e.g. NotesApp)
Get-Content $log -Tail 500 | Select-String -Pattern '\[NotesApp\]'

# Check for IL2CPP GC collection issues or stale pointers
Get-Content $log | Select-String -Pattern 'WasCollected|Pointer == IntPtr\.Zero|NullReferenceException'

# Find the very FIRST error in the log (root cause of startup crashes)
Get-Content $log | Select-String -Pattern '\[ERROR\]|Exception' | Select-Object -First 3
```

---

## 2. Common Flags

| Flag | Effect |
|------|--------|
| `--error-summary` | Aggregate error categories with counts |
| `--mod <name>` | Filter log lines containing `<name>` |
| `--since HH:MM` | Only lines after this wall-clock time (today) |
| `--level error\|warn\|info` | Filter by log level |
| `--tail <N>` | Only the last N lines (faster on huge logs) |
| `--regex <pattern>` | Custom line-level grep (case-insensitive) |
| `--json` | Machine-readable output (for scripts) |
| `--no-color` | Plain text (for piping) |

---

## 3. Log Anatomy (What Each Block Means)

`Latest.log` is structured as follows, roughly:

```
[<HH:MM:SS.mmm>] [ModName] Info/Warning/Error: <message>
[<HH:MM:SS.mmm>] [stack-trace-prefix-line]
[<HH:MM:SS.mmm>]   at ...
[<HH:MM:SS.mmm>]   at ...
...
```

**Pillars to scan for:**
- `[MelonLoader]` banner lines = loader-level diagnostics (broken dep, IL2CPP interp issues).
- `[<ModName>]` lines = mod-emitted messages (e.g. `[NotesApp]`).
- `Exception:` / `at …` blocks = .NET exception stack trace.
- `Harmony` lines = patch install/remove diagnostics.
- Lines without prefix = MelonLoader internal messages.

---

## 4. Common Spike Patterns (What to Look For)

### 4.1. Mod Init Failure (Mod never starts)
```
[<time>] [ModName] Error: <message>
[<time>] Exception: ... StackOverflowException / MissingMethodException / TypeInitializationException
```
**Cause:** `OnInitializeMelon` threw before logging "Ready". Steps:
1. Reproduce.
2. Open `Latest.log` from cold boot (delete it, restart game).
3. Find the **first** error belonging to the mod — ignore everything later.
4. Cross-reference with `[common-errors.md](common-errors.md)`.

### 4.2. Patch Silent No-Op
```
[<time>] [ModName] Info: Patched: <Type>.<Method> (Prefix: 1, Postfix: 0)
[<time>] (no further logs about the patched method)
```
**Cause:** Patch installed but never invoked. Most likely:
- IL2CPP method inlining — patch the caller.
- `__instance == null` early return.
- Scene/lifecycle condition.

### 4.3. Save-Load Desync
```
[<time>] [S1API] Error: GameLifecycle.OnLoadComplete fired before ...
[<time>] [ModName] Error: Property.OwnedProperties.Count == 0
[<time>] Exception: NullReferenceException at …
```
**Cause:** Hooked the wrong lifecycle. Use `GameLifecycle.OnSaveInfoLoaded` instead of `OnGameplaySceneLoaded`.

### 4.4. Native AV Crash (0xc0000005)
```
Windows Error: 0xc0000005 (Access Violation)
(no managed stack trace in log)
```
**Cause:** Mod class volume too high (>5-10 MB) → IL2CPPInterop class init failure → native AV.
**Fix:** Reduce `RegisterTypeInIl2Cpp` count, defer init, split into multiple mods.

### 4.5. IL2CPP Interop Failure (0x80131506)
```
Windows Error: 0x80131506 (CLR Fatal Error)
Fatal error in GC
```
**Cause:** S1API pre-init assembly-loading crash (often NPC-registration; confirmed 2026-08-03 for 3.1.3/3.1.6). Suspect S1API version mismatch.

---

## 5. Worked Example: Catch the First Error

```pwsh
# Step 1: Cold-start capture
Remove-Item "$env:SCHEDULE1_PATH\MelonLoader\Latest.log" -ErrorAction SilentlyContinue
# Launch game, reproduce issue, Alt+F4

# Step 2: Find the FIRST error in the log
Get-Content "$env:SCHEDULE1_PATH\MelonLoader\Latest.log" | `
    Select-String -Pattern '\[ERROR\]|Exception' | `
    Select-Object -First 1
```

The first error is almost always the **root cause**; subsequent errors are usually secondary cascades caused by uninitialized singletons or broken state.

---

## 6. Performance Notes

* Streaming `Latest.log` with `Get-Content -Tail 200` is instantaneous even on huge log files (>100 MB).
* To follow the log in real time during a gameplay session:
  ```pwsh
  Get-Content "$env:SCHEDULE1_PATH\MelonLoader\Latest.log" -Wait -Tail 50
  ```

