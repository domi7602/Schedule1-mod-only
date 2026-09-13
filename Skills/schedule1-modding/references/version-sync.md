# Version Sync — Reference & Session Notes

Concrete, session-tested data for AGENTS.md ↔ Source version drift fixes. Read this **before** running `bump-version.ps1` on any mod.

## 1. Source-of-truth precedence

When a mod's version disagrees across files, the canonical source is:

```
[assembly: MelonInfo(typeof(<Mod>.Mod), "<Name>", "<X.Y.Z>", "Dominik")]
```

in `Source/Mods/<Mod>/src/Mod.cs`. MelonLoader uses this at runtime, so the
binary that ends up in `<GameDir>\Mods\` reflects *this* version — never any
`obj/`-generated `AssemblyVersion`/`FileVersion` or any `public override string
Version` property (no mod in this workspace uses that pattern). When the four
files disagree, MelonInfo wins.

`docs/mod.json` is a secondary truth for Nexus/Thunderstore metadata and
must match MelonInfo.

`AGENTS.md` matrix row is the human-facing summary. Drift here is the
"lügen in der Doku" symptom.

## 2. The Feature-Diff Gate (pre-bump checklist)

1. Read the AGENTS.md description for the mod. List every feature claim.
2. For each claim, grep the source for evidence. Useful keywords:
   - `Spine`, `Avatar`, `MiddleSpine`, `LowerSpine` → Spine Rig Alignment
   - `strap`, `buckle`, `harness`, `sternum` → harness system
   - `ObjLoader`, `TryGetOrLoad`, `\.obj` → Wavefront OBJ runtime
   - `Mannequin`, `targetRotation`, `mouseDeltaX` → 360° rotation
   - `Tier`, `tier1`, `tier2`, `tier3`, `SlotLimit` → tier-based scaling
   - `BuildableItem.Start`, `SetupPlacedStation` → placement hook
   - `SafeStorage.SaveAtomic`, `slot_{`, `OnSaveComplete` → slot-isolated save
   - `BuyFromHardwareStore`, `InjectHardwareStoreListing` → shop integration
3. Build a table: claim → file:line evidence (or "not found").
4. Decide: **bump + real CHANGELOG** OR **fix AGENTS.md back to honest state**.
   Both are valid. The wrong answer is "bump without checking" — that's how
   the drift started.

Worked example from 2026-08-24 (BackpackMod 0.1.0 → 1.0.0):
| AGENTS.md claim | Source evidence |
|---|---|
| 3D Wearable Backpacks | `BackpackVisualManager.cs:158 CreateBackpackVisualInstance` |
| Spine Rig Alignment | `BackpackVisualManager.cs:450 MiddleSpine/LowerSpine fallback` |
| Realistic Harness & Straps | `BackpackVisualManager.cs:300-317` (CHEST STERNUM CROSS-STRAP, STRAP TENSION SLIDE BUCKLES) |
| ObjLoader | `BackpackVisualManager.cs:170 ObjLoader.TryGetOrLoadBackpackMesh` |
| Storage | `BackpackStorageManager.cs` (whole file) |
| Mannequin 360 Rotation | `Patches/CharacterUIPatch.cs:78-95 mouseDeltaX * -180f` |
| All 6 claims verified → 1.0.0 honest. |

Counter-example from same session (AutoPackagingStation 0.1.0 → 0.2.0):
- AGENTS.md claimed "v0.2.1 verified 2026-08-23" — but the entire feature
  list was already in 0.1.0's CHANGELOG. No source evidence of work between
  0.1.0 and 0.2.1.
- Decision: bump to **0.2.0** (not 0.2.1) with explicit
  "documentation sync, no code changes since 0.1.0" CHANGELOG entry.
- Honest, traceable, future-proof.

## 3. `bump-version.ps1` gotchas (verified 2026-08-24)

The script is at `Tools/bump-version.ps1` and is generally solid, but it has
three concrete bugs that bite when CHANGELOG headers or version formats
deviate from the strict default.

### 3.1 Custom CHANGELOG header breaks the dedup

The script's regex `(?m)^# Changelog\s*\r?\n` does NOT match
`# Changelog - <ModName>` (e.g. `# Changelog - AutoPackagingStation`).

When this fails, the script **prepends a new `# Changelog` block above the
existing file** instead of inside it. Result: file now has two `# Changelog`
headers and a duplicate `## X.Y.Z` entry at the top.

**Fix**: read the CHANGELOG after every bump; manually delete the duplicate
block.

### 3.2 Bracketed version header breaks the dedup

The script's regex `## $NewVersion\b` does NOT match `## [X.Y.Z]`
(e.g. `## [0.2.0] - 2026-08-22`).

When this fails, the script prepends `## X.Y.Z (date) - Version bump.` even
though the bracketed entry already exists.

**Fix**: same as 3.1 — read after the bump, delete the duplicate.

### 3.3 The `- Version bump.` placeholder is a documentation lie on first release

When the prior CHANGELOG is empty or single-line (e.g.
`## 0.1.0 - Initiale Version.`), the script's prepended
`## X.Y.Z (date) - Version bump.` reads as "we built a stable prior version
that we are now bumping" — which is the opposite of the truth (the prior
version was a stub).

**Fix**: before running the script, manually write the new `## X.Y.Z (date)`
header with real feature bullets. The script's dedup check then matches
`## X.Y.Z\b` (no brackets) and skips prepending. Worked example for
BackpackMod 0.1.0 → 1.0.0 in this session:

```markdown
# Changelog

## 1.0.0 (2026-08-24)
- First public release: 3D Wearable Backpacks with Spine Rig Alignment ...
- Realistic Harness system (chest sternum cross-strap, shoulder straps, metal buckles) ...
- Tier system: backpack_t1 / backpack_t2 / backpack_t3 with per-tier color palettes.
- `ObjLoader` runtime: zero-dependency Wavefront OBJ loader ...
- `backpacks.bundle` AssetBundle pipeline for prefab-based backpack meshes.
- Tier-based storage scaling (T1 small, T2 hiking, T3 tactical) ...
- 360° mannequin inspection rotation in the Character menu (RMB-drag, Q/E keys).
- ClothingSlot 10 binding: backpack renders only when actively equipped.
- B hotkey toggles backpack storage.

## 0.1.0 (2026-08-21)
- Initiale Version.
```

Then `pwsh Tools/bump-version.ps1 -Mod BackpackMod -Version 1.0.0` correctly
detected the existing `## 1.0.0` header and only updated the other three
files (`Mod.cs MelonInfo`, `mod.json`, `AGENTS.md matrix row`).

## 4. Post-bump verification checklist

1. `grep -n "X.Y.Z" Source/Mods/<Mod>/src/Mod.cs` → should match MelonInfo.
2. `cat Source/Mods/<Mod>/docs/mod.json` → `version` field matches.
3. `head -20 Source/Mods/<Mod>/docs/CHANGELOG.md` → no duplicate `# Changelog`
   headers, no duplicate `## X.Y.Z` entries, no leftover `- Version bump.`
   placeholder.
4. `grep -n "<Mod>" AGENTS.md` → matrix row says `vX.Y.Z`.
5. Optional: `dotnet build Source/Mods/<Mod>/src/<Mod>.csproj -c Release` →
   rebuild + redeploy to `<GameDir>\Mods\` if you want the new version live.

If any of these fails, fix it before committing — the bump is not "done" until
all four agree.

## 5. The ripgrep bracket-trap (tool-side, not skill-side)

When using `search_files` for patterns containing `(` or `[`, the underlying
ripgrep fails with "unclosed group/character class". Workaround that worked
this session:

- Drop the special char when possible (`MelonInfo` instead of `MelonInfo(`).
- Add `file_glob="*.cs"` to narrow scope first, then grep inside.
- For complex patterns, escape with `\(` (didn't fully test).

This is a Hermes-tool quirk, not a workspace pattern. Mentioned here so the
next agent doesn't lose 3 tool calls re-trying the same failing regex.