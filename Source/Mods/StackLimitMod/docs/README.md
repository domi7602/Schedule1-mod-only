# StackLimitMod

**StackLimitMod** is a quality-of-life mod for *Schedule I* (v0.4.6f13) that allows players to customize item stack limits across player inventories, storage containers, shelves, and trunks.

---

## 🌟 Features (v0.1.0)

- **Configurable Global Stack Limit**:
  - Set the stack limit to any number between 1 and 9999 (default: `40`).
- **Comprehensive Item Discovery**:
  - Automatically updates both statically loaded `BaseItemDefinition` assets in memory and dynamic items in `Registry.Instance`.
- **Runtime Item Registration Hook**:
  - Patches `Registry.AddToRegistry` to ensure modded items or dynamically generated items immediately receive the configured stack limit.
- **Instance-Level Safety Patch**:
  - Intercepts `BaseItemInstance.get_StackLimit` via Harmony and `PatchGuard` to ensure active item slots enforce the new limits reliably.
- **SafeStorage Persistence**:
  - Uses `S1Mods.Shared.SafeStorage` for atomic writes and automatic `.bak` backups to prevent file corruption.
- **Interactive Console & Hash Terminal Bridge**:
  - Full support for the in-game developer console (`~`) and DooDesch's `hash` smartphone terminal (`#`).

---

## 💬 In-Game Commands

Commands can be run via the developer console (`~`) or DooDesch `hash` terminal (`#`):

| Command | Description |
|---|---|
| `stack` / `stack stats` | Displays current stack limit, modified item count, and active configuration. |
| `stack set <amount>` | Sets the new stack limit (1–9999), saves to disk, and applies to all items immediately. |
| `stack reload` | Reloads configuration from `UserData/StackLimitMod/config.json` and reapplies. |
| `stack help` | Displays available subcommands and usage information. |

*Note:* `stacklimit` can also be used as an alias for `stack`.

---

## ⚙️ Configuration

Configuration is stored in `UserData/StackLimitMod/config.json`:

```json
{
  "StackLimit": 40,
  "OverrideNonStackable": true,
  "ExcludedItemIds": [],
  "LogModifications": true
}
```

### Configuration Options:
- **`StackLimit`** (int, default: `40`): The target maximum stack size for items (1–9999).
- **`OverrideNonStackable`** (bool, default: `true`): When `true`, items with an original stack limit of `1` are also increased to the new limit. When `false`, single-count/non-stackable items remain unchanged.
- **`ExcludedItemIds`** (string list, default: `[]`): List of item ID strings that should never have their stack limits modified.
- **`LogModifications`** (bool, default: `true`): Logs informational messages during item stack limit application.

---

## 🔧 Technical Details & Architecture

- **Engine:** Unity 2022.3 (IL2CPP 64-bit)
- **Mod Loader:** MelonLoader 0.7.3 (`net6.0`)
- **Harmony Patching:** Secured via `S1Mods.Shared.PatchGuard` for graceful degradation.
- **Savegame Safety:** Non-destructive in-memory definitions update, respecting vanilla savegame formats.
