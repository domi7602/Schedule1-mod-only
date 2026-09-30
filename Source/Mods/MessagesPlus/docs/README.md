# MessagesPlus

**Version 0.3.0** — inbox hygiene for the vanilla Messages app (SMS) on the in-game phone: Clear All + Clear Read, live name search, category filter chips, unread counter, and the one-time legacy restore.

MessagesPlus is a **patch-only mod**: it enhances the *existing* vanilla `MessagesApp` via Harmony patches and injects its UI into the app's own page. It does **not** register a new PhoneApp or add a homescreen icon.

## Features

The injected **toolbar** (top-right of the Messages home page) has three rows:

1. **🔎 Search** — live contact-name filter (case-insensitive substring) + **🔢 "N unread"** counter (visible conversations with `Read == false` — vanilla has no per-message read state).
2. **🏷️ Filter chips** — `[All][Customer][Dealer][Supplier]`: show only threads whose `MSGConversation.Categories` contain the selection. Search and filter combine (AND).
3. **🧹 [Clear Read]** + **🗑️ [Clear All]** — both with a confirmation dialog ("… Supplier and dealer threads are kept.").

**Clear All** hides every visible **customer** conversation via `MSGConversation.SetEntryVisibility(false)`. **Clear Read** does the same, but only for conversations whose `MSGConversation.Read` flag is `true` (unknown read state = kept).

- **Customer-only:** supplier and dealer threads and threads with unknown/empty categories are never touched (conservative filter: `Categories` contains Customer AND neither Supplier nor Dealer — null/empty categories mean non-customer).
- **View-only search/filter (`InboxView`):** the search/filter view only toggles entry GameObjects and never mutates save state. It only restores hides it owns (rows hidden by vanilla are left alone) and re-applies itself from a throttled 1 s tick while active (vanilla can re-show entries on its own events). Zero cost with the default view. Search/filter reset when the app closes.
- **One-time legacy restore:** threads deleted/hidden by MessagesPlus v0.1.x are un-hidden again on the next save load (they are still hidden inside the game save — `MSGConversationData.IsHidden`): every `Trashed`/`Purged` record from the old `UserData/MessagesPlus/trash_*.json[.bak]` files plus every hidden supplier/dealer thread (safety net). Processed legacy files are renamed to `*.restored`.
- **No trash** since v0.2.0: no trash UI, no per-entry delete buttons, no trash persistence.

## Installation

1. Requires **MelonLoader 0.7.3+** and **S1API** (see the repository root README).
2. Copy `MessagesPlus.dll` into `<GameDir>\Mods\`.
3. Optionally copy `mod.json` into `<GameDir>\UserData\MessagesPlus\`.
4. Launch the game. The Messages app shows the MessagesPlus toolbar on its home page.

## Usage

1. Open the phone → **Messages** app.
2. **Search / filter chips** — narrow the inbox view (view-only, resets on close).
3. **[Clear Read]** — confirm once; all read customer threads are hidden, unread ones stay.
4. **[Clear All]** — confirm once; all customer threads are hidden from the inbox, supplier and dealer threads stay.
5. Hidden threads stay hidden across sessions (the game persists the visibility). To bring v0.1.x-deleted threads back, just load the save once with this version — the legacy restore un-hides them automatically.

## Multiplayer

Clear All, Clear Read and the legacy restore are **host-only**. Multiplayer clients see disabled mutation buttons; mutations are ignored with a log warning to prevent save desyncs. Search, filter and the unread counter are view-only and stay active for clients.

## Legacy restore details

- Runs on `GameLifecycle.OnSaveInfoLoaded` and after the vanilla `MessagesApp.Loaded` hook (idempotent; deferred while the conversation lists are still empty so no record can be lost).
- Pass (a): `UserData/MessagesPlus/trash_*.json` + `trash_*.json.bak` (all slots) — every `Trashed`/`Purged` record is matched (Id → Index+Name → name-only fallback for `Index < 0` records) and its conversation un-hidden; each processed file is renamed to `<name>.restored`.
- Pass (b) safety net: every hidden Supplier/Dealer conversation in `MessagesApp.Conversations`/`ActiveConversations` is un-hidden. Customer threads are never un-hidden here (they stay hidden on purpose).
- Both passes are host-only (they mutate save state).

## In-game test notes

- **Search/filter (v0.3.0):** type a contact name → only matching rows stay; clear the field → all rows return. Switch chips → same. Close and reopen the app → view is reset. Watch for row gaps after filtering (would mean `RepositionEntries` ignores inactive entries — report it).
- **Clear Read (v0.3.0):** with ≥1 read and ≥1 unread customer thread: only the read ones vanish; supplier/dealer threads remain.
- **Unread counter (v0.3.0):** matches the number of unread inbox threads; updates within ~1 s.
- **Legacy restore (v0.2.0):** with the v0.1.x `trash_slot_1.json` present and a save containing a trashed thread: load the save → the thread is visible again and `trash_slot_1.json.restored` exists in `UserData/MessagesPlus/`.
- **Customer-only Clear All:** after Clear All only customer threads vanish from the inbox; supplier/dealer threads remain.

## Limitations

- Toolbar placement is anchored to the vanilla home page (top-right, ~340×132 Dp); exact pixel offsets may need fine-tuning after in-game verification.
- The vanilla app ships its own category filter internals (`MessagesApp.FilterByCategory`/`CategoryButtons`); MessagesPlus deliberately uses its own row-visibility engine instead (unknown int semantics, and it must compose with the search). If the vanilla category buttons turn out to be visible in-game too, the chips can be dropped.
- Unread counting is per conversation (the `Read` flag lives on `MSGConversationData` per thread); per-message read states do not exist in vanilla.
- Phase 2 (toast + sound) and Phase 3 (configurable background) are prepared in the config schema (`MessagesPlusConfig`) but not yet active.

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
