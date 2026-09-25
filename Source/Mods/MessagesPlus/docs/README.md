# MessagesPlus

**Version 0.1.1** — Clear All + Trash/Restore for the vanilla Messages app (SMS) on the in-game phone.

MessagesPlus is a **patch-only mod**: it enhances the *existing* vanilla `MessagesApp` via Harmony patches and injects its UI into the app's own page. It does **not** register a new PhoneApp or add a homescreen icon.

## Features

- **Clear All** button in the Messages toolbar (top-right) → confirmation popup → every visible conversation moves to the trash (hidden via `MSGConversation.SetEntryVisibility(false)`).
- **Trash section** ("Trash") as a collapsible panel at the bottom of the conversation list, showing all deleted threads with a per-thread restore button.
- **Restore** per thread → `SetEntryVisibility(true)` + `MoveToTop()` + `RepositionEntries()` — the thread is back at the top of the inbox.
- **Empty trash** → permanent removal from the Messages app, behind a **second**, stronger confirmation.
- **Slot-isolated persistence**: the trash is saved via `S1Mods.Shared.SafeStorage.SaveAtomic` to `UserData/MessagesPlus/trash_slot_{n}.json` (atomic write + `.bak` backup, never `slot_-1`).

## Installation

1. Requires **MelonLoader 0.7.3+** and **S1API** (see the repository root README).
2. Copy `MessagesPlus.dll` into `<GameDir>\Mods\`.
3. Optionally copy `mod.json` into `<GameDir>\UserData\MessagesPlus\`.
4. Launch the game. The Messages app shows the new toolbar buttons on its home page.

## Usage

1. Open the phone → **Messages** app.
2. **[Clear All]** (top-right) moves all visible threads to the trash (confirm once).
3. Open the **[Trash (N)]** section at the bottom (toggle with the trash icon in the toolbar or the section header).
4. **[↩]** restores a thread to the top of the inbox.
5. **[Empty trash]** permanently removes all trashed threads (confirm twice — the second dialog warns that this cannot be undone).

## Multiplayer

All trash mutations (Clear All, Restore, Empty trash) are **host-only**. Multiplayer clients see a read-only trash: the mutation buttons are disabled and mutations are ignored with a log warning to prevent save desyncs.

## Persistence details

- File: `UserData/MessagesPlus/trash_slot_{n}.json` (`SafeStorage.SaveAtomic`, `.bak` recovery).
- Save-load timing: loaded on `GameLifecycle.OnSaveInfoLoaded`, re-applied to the conversation list after the vanilla `MessagesApp.Loaded` hook.
- Permanently deleted ("purged") threads are recorded in the same file and re-removed after every save load and from a throttled (1 s) re-apply, so they stay gone across sessions — even when the purged thread receives new messages (the native `MSGConversation` object stays alive and vanilla callbacks can otherwise re-show the entry). The native conversation object and the game's own save entry are deliberately left intact — destroying vanilla saveables/UI objects causes IL2CPP crashes in vanilla callbacks.

## In-game test notes

- **Purge resurrection (v0.1.1):** move a thread to the trash, empty the trash, then have that contact send a new message. The thread must NOT reappear in the inbox (the throttled re-apply suppresses it); check the log for the re-apply warnings.

## Limitations (Phase 1)

- Individual per-thread delete (long-press) is not included yet — Phase 1 covers Clear All only.
- Toolbar/section placement is anchored to the vanilla home page (top-right / bottom); exact pixel offsets may need fine-tuning after in-game verification.
- Phase 2 (toast + sound + unread badge) and Phase 3 (configurable background) are prepared in the config schema (`MessagesPlusConfig`) but not yet active.

## Config

`UserData/MessagesPlus/MessagesPlus.cfg` (MelonPreferences, category `MessagesPlus`):

| Key | Default | Purpose |
|---|---|---|
| `BackgroundColor1` / `BackgroundColor2` | `#101318` / `#1C2230` | App background gradient (Phase 3) |
| `ToastEnabled` | `true` | Toast popups on new messages (Phase 2) |
| `SoundEnabled` | `true` | Sound effects (Phase 2) |

## Build

```powershell
$env:SCHEDULE1_PATH = 'C:\Program Files (x86)\Steam\steamapps\common\Schedule I'
dotnet build Source\Mods\MessagesPlus\src\MessagesPlus.csproj -c Release
```

The DLL auto-deploys to `<GameDir>\Mods\` via `Directory.Build.targets`.
