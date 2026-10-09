# System Reference: Schedule I

Analysis of 64 gameplay systems, based on the decompiled C# source code (AssetRipper export, 22 GB, ~1855 classes in `Il2CppScheduleOne`).

> **Index:** [`_index.md`](_index.md) is the **single authoritative index** (with depth ratings + redirect stubs). This README keeps only workflow + source provenance.

> verified: file set consolidated 2026-10-05 (7 redirect stubs, one category table). Decompiles regenerated 2026-10-05 against the fresh proxies (ilspycmd, project mode) — spot-check set passed.

## Workflow Guidelines for Agents

1. Locate the target system in **[`_index.md`](_index.md)** (respect the Depth rating: 🔵 files are class registers only — go to the decompile for real signatures).
2. Read the respective markdown file.
3. Locate core classes directly in `GameReferences/decompiled/Assembly-CSharp/Il2CppScheduleOne/<System>/`.
4. For S1API wrappers, check `ThirdParty/S1API/S1API/` first — prefer the wrapper.
5. Verify live signatures against `$env:SCHEDULE1_PATH\MelonLoader\Il2CppAssemblies\` via `ilspycmd` (scoped `DOTNET_ROOT`).
6. For IL2CPP/Harmony safety patterns: `Skills/schedule1-modding/SKILL.md` + `Skills/schedule1-troubleshooting/SKILL.md`.

## Source

- AssetRipper export of the game's `Assembly-CSharp` (~1855 C# classes from the `Il2CppScheduleOne` namespace).
- Local decompiles: `GameReferences/decompiled/Assembly-CSharp/Il2CppScheduleOne/` (see `GameReferences/README.md` for regeneration).
- File set: 64 numbered systems (01–64, no gaps) + `_index.md` + this README. 26 is a known-bug analysis, not a system (intentional).
