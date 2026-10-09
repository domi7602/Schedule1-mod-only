# StackLimitMod

**StackLimitMod** is a quality-of-life mod for *Schedule I* (v0.4.7, IL2CPP) that allows players to customize item stack limits across player inventories, storage containers, shelves, and trunks.

---

## 🌟 Features (v0.1.7)

- **Configurable Global Stack Limit**:
  - Set the stack limit to any number between 1 and 9999 (default: `40`).
- **Comprehensive Item Discovery**:
  - Automatically updates both statically loaded `BaseItemDefinition` assets in memory and dynamic items in `Registry.Instance`.
- **Runtime Item Registration Hook**:
  - Patches `Registry.AddToRegistry` to ensure modded items or dynamically generated items immediately receive the configured stack limit.
- **Instance-Level Safety Patch**:
  - Intercepts `BaseItemInstance.get_StackLimit` via Harmony and `PatchGuard` to ensure active item slots enforce the new limits reliably.
- **Category-Safe Scope (`AgricultureOnly`, default on)**:
  - The limit is applied strictly to agriculture/farming items (soil, seeds, packaging, additives/fertilizers, mushroom spores/spawns, harvested crops) and mixing/cooking ingredients. Weapons, ammunition, clothing, and cash are always protected.
- **MixGuard**:
  - Blocks starting a second mix on a mixing station that is already mixing and shows a rate-limited *"Mixing already running"* notification instead of failing silently.
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
| `stack set ag <true\|false>` | Toggles Agriculture-Only mode (agriculture + ingredients; weapons & ammo stay protected). |
| `stack check <itemId>` | Diagnoses why a specific item is (not) stack-limited. |
| `stack report` | Summarizes the last apply run (who was skipped and why). |
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
  "AgricultureOnly": true,
  "ExcludedItemIds": [],
  "LogModifications": true,
  "LogDecisions": false
}
```

### Configuration Options:
- **`StackLimit`** (int, default: `40`): The target maximum stack size for items (1–9999).
- **`OverrideNonStackable`** (bool, default: `true`): When `true`, items with an original stack limit of `1` are also increased to the new limit. When `false`, single-count/non-stackable items remain unchanged.
- **`AgricultureOnly`** (bool, default: `true`): When `true`, stack limits are applied strictly to agriculture/farming items and mixing/cooking ingredients. Weapons, ammunition, clothing, and cash are always protected regardless of this flag.
- **`ExcludedItemIds`** (string list, default: `[]`): List of item ID strings that should never have their stack limits modified.
- **`LogModifications`** (bool, default: `true`): Logs informational messages during item stack limit application.
- **`LogDecisions`** (bool, default: `false`): Diagnostic hot-path logging — records every uncached eligibility decision (bounded to 512 entries) into `UserData/StackLimitMod/apply_report.json`. Enable while diagnosing a specific item, disable afterwards.

---

## 🔧 Technical Details & Architecture

- **Engine:** Unity 2022.3 (IL2CPP 64-bit)
- **Mod Loader:** MelonLoader 0.7.3 (`net6.0`)
- **Harmony Patching:** Secured via `S1Mods.Shared.PatchGuard` for graceful degradation.
- **Savegame Safety:** Non-destructive in-memory definitions update, respecting vanilla savegame formats.
