---
name: schedule1-mod-reversing
description: >-
  Reverse-engineering runbook for third-party Schedule I mod DLLs.
  Use when a foreign mod ZIP must be understood — "kannst du die Mod dekompilieren?",
  "was macht dieser Mod?", before adopting a pattern from someone else's mod, before
  installing an unknown DLL, or when deciding whether a foreign mod still works after
  a game update. Covers extraction, ilspycmd decompilation, hook inventory, the
  compatibility check against GameReferences, and the red-flag checklist.
  Keywords: decompile, ilspycmd, reverse engineering, third-party mod, foreign DLL,
  hook inventory, Harmony patch inventory, GameReferences, compatibility check,
  RPC hash, obfuscation, mod zip.
---

> Runtime and dependency details are maintained in workspace [AGENTS.md](../../AGENTS.md). Verification notes in this skill describe evidence scope; they do not imply current-runtime verification.

# Schedule I — Reverse-Engineering Third-Party Mods

Turning an unknown `*.dll` in a ZIP into a defensible statement about what it does, what it
hooks, and whether it still works on the current runtime. The procedure is mechanical; the
value is in the **hook inventory** and the **compatibility check**, not in reading every file.

This skill contains **no redistributable code**. Decompiled sources stay local, are never
committed, never shipped, and never installed back into the game without Dominik's explicit go.

---

## 1. When to Use

- A mod ZIP shows up and the question is "what is this / can we decompile it".
- A pattern from someone else's mod should be adopted (economy math, item injection, UI).
- An unknown `Mods\*.dll` is in the live game folder or is about to be installed.
- A foreign mod is suspected of breaking after a game update.
- Documentation or a feature claim needs verification ("the README says X — does the code do X?").

**Don't use for:** our own mods (→ `schedule1-modding`, `schedule1-troubleshooting`), lifecycle
event ordering (→ `schedule1-lifecycle-verify`), or crash triage (→ `schedule1-troubleshooting`).

## 2. The Six Steps

```
1. Inventory      → unzip into a durable folder, list every entry
2. Decompile      → ilspycmd -p into src/, note TFM + reference set
3. Triage         → which files matter, what is the architecture
4. Hook inventory → grep every patch target; this is the real "what does it do"
5. Compat check   → does each anchor exist in GameReferences/decompiled?
6. Report         → identify / architecture / mechanics / hooks / compat / risks / reusable
```

Details, exact commands and interpretation rules: [`references/reversing-runbook.md`](references/reversing-runbook.md).

## 3. Decision Tree

```
ZIP arrives
│
├─ Single DLL, small (< 200 KB)  → plain C# mod, decompile is readable end-to-end
├─ Single DLL, large (> 1 MB)    → check embedded resources first (assembly.GetManifestResourceNames);
│                                  big mods ship icons/sounds/designs, not necessarily more logic
└─ ZIP with Mods\ prefix         → extract as-is, preserve the subfolder (mod may rely on it)

First look at the csproj ilspycmd generates
│
├─ TargetFramework net472               → older MelonLoader generation, raw IL2CPP + Harmony
├─ TargetFramework netcoreapp6.0        → current generation, Il2CppInterop
├─ using Il2CppScheduleOne.*            → Il2CppInterop proxies (namespaces carry the Il2Cpp prefix)
├─ using ScheduleOne.* (no prefix)      → S1API facade (S1API.* types appear in the reference set)
└─ Reference set (MelonLoader / 0Harmony / Assembly-CSharp / FishNet / S1API) → tells you the
   generation, whether it is singleplayer-only, and which framework it assumed

Obfuscated?
├─ Readable class + method + string names → no obfuscation, full analysis is fine
└─ Names like a.b.c / control-flow flattening → stop; report "obfuscated, analysis not viable
   without deobfuscation", do not sink time into it
```

## 4. Quick Reference — the Two Checks That Matter

**Hook inventory** (this is what the mod actually does, regardless of its README):

```bash
grep -nE "Patch\(typeof|AccessTools\.(Method|PropertyGetter|Field)|HarmonyPatch\(|TargetMethod" src/**/*.cs
```

**Compatibility against the current runtime** (the step that turns "should work" into a finding):

```bash
cd /c/Users/pc/Schedule1-mod-only/GameReferences/decompiled
check() { printf "%-58s " "$1"; grep -rqE "$2" --include="*.cs" . && echo OK || echo MISSING; }
check "PlayerHealth.TakeDamage (RPC hash)" "TakeDamage_3505310624"
check "ShopListing.Price"                  "class ShopListing"
check "GoonPool.goons"                     "NativeFieldInfoPtr_goons"
```

Interpretation:
- **Every anchor present** → the mod will initialise on this runtime. It says nothing about
  *behaviour*; that needs a real session.
- **A hash-suffixed RPC missing** → the game update moved it; anything gated on it throws at
  `OnInitializeMelon` or silently stops firing.
- **Dynamically patched by method name** (no fixed type) → a MISSING anchor is not a crash:
  the mod quietly does less. Report which capability is lost.

## 5. Red Flags — read these before trusting the analysis

Full catalogue with real examples: [`references/red-flags.md`](references/red-flags.md).

| Flag | Why it matters |
|---|---|
| `catch (Exception) { }` around a whole tick | Failures are invisible; the mod can be half-dead with a clean log |
| `[Conditional("…DIAGNOSTICS")]` logging | On a release build every log call is **removed from the DLL** |
| Patches a compiler-generated type (`…ObObObUnique`, `_d__27`) | Name is regenerated by the next build → first hook to break |
| `AccessTools.Field(typeof(X), "privateName")` | Field rename silently disables the guard |
| Direct `RpcLogic___…` invocation | Bypasses FishNet; harmless only if the mod is singleplayer-gated |
| Postfix on a per-frame UI method | Dauerlast; measure before adopting |
| State outside the save with no `.bak` | Crash mid-write = corrupt file, often swallowed |
| Save-slot detection by guessed reflection | Falls back to a shared bucket → state bleeds across saves |
| `HttpClient` / `UnityWebRequest` | Check the host: user-supplied URL is fine, hardcoded telemetry is not |
| Third-party DLL already in `Mods\` | Check before analysis; the "not installed" answer is part of the report |

## 6. Output Shape

Every analysis answers these, in this order (see [`references/case-notes.md`](references/case-notes.md)
for three worked examples):

1. **Identity** — path, size, hash, TFM, reference set, obfuscation, installed yes/no.
2. **Architecture** — file/namespace table with line counts; which layer is pure logic vs. Unity.
3. **Mechanics** — the actual rules (formulas, seeds, thresholds), not marketing.
4. **Hook inventory** — table of every patch target + what it does.
5. **Compatibility** — anchor → present/missing, against `GameReferences/decompiled`.
6. **Risks** — red flags found, with file:line.
7. **Reusable patterns** — what is worth adopting, and what deliberately is not.
8. **Open checks** — only questions that need a real game session.

## 7. Boundaries

- **Local only.** Decompiled output lives under `Documents/Schedule1-Mod-Reversing/<Mod>-<Version>/`
  (`src/`, the original ZIP, `ANALYSIS.md`). Never in the repo, never in `Release/`, never in chat
  as code blocks of meaningful size.
- **Never redistribute** extracted game code or foreign mod source.
- **Never install** a foreign DLL into `Mods\` as part of an analysis — that is a separate,
  explicit decision by Dominik, and it needs the game closed and a deploy receipt.
- **Report, don't fix.** Bugs in someone else's mod are findings, not repair jobs, unless asked.
