# logscan.py & Latest.log Triage

The canonical tool for parsing `Latest.log` is **`logscan.py`** at `C:\Users\pc\Desktop\Schedule1-mod-only\ThirdParty\S1MCPServer-master\logscan.py`. It predates S1MCP, has no game-freeze risk, and is the default choice for mod-development log triage.

---

## 1. Quick Start

```pwsh
# Default: scan <GameDir>\MelonLoader\Latest.log for errors (last 1000 lines)
pwsh "C:\Users\pc\Desktop\Schedule1-mod-only\ThirdParty\S1MCPServer-master\logscan.py"

# Just error summary
pwsh "C:\Users\pc\Desktop\Schedule1-mod-only\ThirdParty\S1MCPServer-master\logscan.py" --error-summary

# Filter to a single mod
pwsh "C:\Users\pc\Desktop\Schedule1-mod-only\ThirdParty\S1MCPServer-master\logscan.py" --mod NotesApp

# Filter by time window
pwsh "C:\Users\pc\Desktop\Schedule1-mod-only\ThirdParty\S1MCPServer-master\logscan.py" --since 14:00
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
4. Cross-reference with `[schedule1-troubleshooting/references/common-errors.md]`.

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
Remove-Item "$env:SCHEDULE1_PATH\MelonLoader\Latest.log"
# Launch game, reproduce issue, alt-F4

# Step 2: Run logscan and find the FIRST error
& "C:\Users\pc\Desktop\Schedule1-mod-only\ThirdParty\S1MCPServer-master\logscan.py" `
    --level error `
    --json | ConvertFrom-Json | Select-Object -First 1
```

The first error is the **root cause**; everything after it is cascade.

---

## 6. Performance Notes

* `logscan.py` reads the file once and applies filters in-memory. ~50 MB file = ~1-2 s.
* For a slow game where `Latest.log` grows above 100 MB during session, prefer `--tail 5000`.
* On persistent logs, archive weekly with `Compress-Archive -Path ... -DestinationPath Latest.log.<date>.gz` (optional).
