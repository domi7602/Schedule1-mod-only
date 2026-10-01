# Troubleshooting (Players)

This page covers the most common installation problems. For developer-level diagnostics (Harmony patch failures, save-load timing, IL2CPP crashes) see [`docs/pitfalls.md`](pitfalls.md) and the [`schedule1-troubleshooting`](../Skills/schedule1-troubleshooting/SKILL.md) skill.

## Where is the log?

MelonLoader writes everything to

```text
<GameDir>\MelonLoader\Latest.log
```

Mods from this repository log with a `[<ModName>]` prefix (for example `[NotesApp]`). Search the log for the mod name first, then for `ERROR` or `Exception`.

## Checklist before reporting a bug

1. **MelonLoader 0.7.3** is installed and the game starts with the MelonLoader console.
2. **S1API** is present: `Mods\S1API.Il2Cpp.MelonLoader.dll` and `Plugins\S1APILoader.dll`. Every mod here requires it.
3. **`Shared.dll`** is present in `Mods\`. It is the common library (`S1Mods.Shared`) used by all mods in this repository and ships inside each release ZIP. Do not delete it when removing a single mod.
4. **S1MAPI** (`UserLibs\S1MAPI_Il2Cpp.dll`) is installed if you use `AutoPackagingStation` or `TaxiDriver`.
5. The ZIP contents were copied to the right places: only `.dll` and `.png` files in `Mods\`, `mod.json` and `.pdb` in `UserData\<ModName>\`.
6. The game version matches the one the mod was verified on — see [`docs/compatibility.md`](compatibility.md).

## Common symptoms

| Symptom | Likely cause | Fix |
|---|---|---|
| Mod does not appear in the MelonLoader console at startup | DLL not in `Mods\`, or the ZIP was extracted into a sub-folder | Make sure `<GameDir>\Mods\<ModName>.dll` exists directly in `Mods\`. |
| `FileNotFoundException` / `Could not load file or assembly 'Shared'` | `Shared.dll` missing from `Mods\` | Copy `Shared.dll` from the release ZIP into `Mods\`. |
| `Could not load file or assembly 'S1API'` | S1API not installed or outdated | Install S1API 3.2.x (see [Compatibility](compatibility.md)). |
| `Could not load file or assembly 'S1MAPI'` | S1MAPI missing | Install S1MAPI into `UserLibs\` (only `AutoPackagingStation` and `TaxiDriver` need it). |
| `BadImageFormatException` lines at startup | A non-mod file (for example `mod.json`, `.pdb` or a reference assembly) was placed in `Mods\` | Move `mod.json` / `.pdb` to `UserData\<ModName>\`; remove stray DLLs from `Mods\`. |
| A phone app opens but shows an empty / transparent screen | Known IL2CPP lifecycle issue when a mod destroys UI on phone close | Update to the latest mod version; report with the log if it persists. |
| Mod settings reset or a file named `..._slot_-1.json` appears | Save slot was not yet known when the mod saved | Fixed in current versions of all mods; delete the `slot_-1` file and update the mod. |
| Harmony warning `PatchGuard: ... skipped` in the log | The game update changed a method the mod patches | The mod degrades gracefully; check [`docs/compatibility.md`](compatibility.md) and report the warning. |

## Where mods store their data

Mods keep configuration and save-slot data under `UserData\<ModName>\`, for example `UserData\NotesApp\notes_slot_1.json`. Files are written atomically with a `.bak` backup; if a file is corrupted the mod falls back to the backup on load.

## Reporting a bug

Open an issue with the [bug report template](https://github.com/domi7602/Schedule1-mod-only/issues/new/choose) and include:

- mod name and version (from `UserData\<ModName>\mod.json` or the MelonLoader console),
- game version (shown in the MelonLoader console at startup),
- the relevant part of `MelonLoader\Latest.log`,
- steps to reproduce.
