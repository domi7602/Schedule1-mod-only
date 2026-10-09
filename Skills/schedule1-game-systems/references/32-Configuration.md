# Configuration (Schedule I)
> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 75/76 identifier-shaped tokens resolve (0 documented as absent). Unresolved identifiers are listed at the end of this file. Runtime behaviour is not covered by this sweep.

## Core Classes

| Class | Verified base type | Purpose |
|-------|--------------------|---------|
| `ConfigurationService` | `PersistentSingleton<ConfigurationService>` (`ScheduleOne.Configuration`) | Central registry of all `BaseConfiguration` assets |
| `ConfigurationServiceNetworker` | `NetworkBehaviour` | Host→client settings sync (FishNet Target RPC) |
| `BaseConfiguration` | `ScriptableObject` | Config asset base; carries `OnConfigurationChanged` |
| `Configuration<T>` | `BaseConfiguration where T : Settings` | Generic wrapper with `DefaultSettings` / `PlatformDefaultSettings` — the underscored spellings are not members |
| `Settings` | `PersistentSingleton<Settings>` (`ScheduleOne.DevUtilities`) | Applies/writes the real engine settings |

Concrete `Configuration<T>` examples verified: `SFXConfiguration : Configuration<SFXSettings>` (Audio), `EquipConfiguration : Configuration<EquipSettings>` (Equipping). So the same pattern is used across game systems.

## ConfigurationService API (verified)
- `Configurations` property → all registered `BaseConfiguration` assets
- `TryGetConfiguration<T>(out T)` (generic) / `TryGetConfiguration(string name, out BaseConfiguration)`
- `GetConfigurationAndListenForChanges(Action<BaseConfiguration> callback)` — subscribe to change notifications
- `UnsubscribeFromConfigurationChanges(Action<BaseConfiguration> callback)`
- `ResetConfigurations()` (private)
- `BaseConfiguration.OnConfigurationChanged` — `Il2CppSystem.Action<BaseConfiguration>` field; raised by config instances when a setting changes

## Settings Categories (DevUtilities)
Data classes are plain `[Serializable]` classes; `Settings` singleton applies them:
| Data class | Applied by | Reloaded by |
|-----------|------------|-------------|
| `DisplaySettings` | `ApplyDisplaySettings` (+ `MoveMainWindowTo`, `ConfirmDisplaySettings` UI) | — (no `ReloadDisplaySettings` exists; changes flow through `WriteDisplaySettings`) |
| `GraphicsSettings` | `ApplyGraphicsSettings` | `ReloadGraphicsSettings` |
| `AudioSettings` | `ApplyAudioSettings` | `ReloadAudioSettings` |
| `InputSettings` | `ApplyInputSettings`, `RestoreDefaultKeyboardBindings`, `RestoreDefaultGamepadBindings` | `ReloadInputSettings` |
| `OtherSettings` | `ApplyOtherSettings` | `ReloadOtherSettings` |
| `GamepadSettings` (+ `HapticSettings`) | `ApplyGamepadSettings` | `ReloadGamepadSettings` |
- `GameSettings` data class holds e.g. `ConsoleEnabled`, `UseRandomizedMixMaps`
- `Settings` also has `GetActionControlPath(string actionName)` for input display strings

## Events
On `Settings` (verified):
| Event | Kind | Trigger |
|-------|------|---------|
| `onDisplaySettingsApplied` | `Action` event (add/remove verified) | after display settings applied |
| `onQualitySettingsChanged` | `Action` event (add/remove verified) | quality preset change |
| `onInputsApplied` | delegate field (accessible directly) | after input bindings applied |
| `onUnappliedDisplayIndexChanged` | delegate field (accessible directly) | pending display change before confirmation |

## Network Sync
- `ConfigurationServiceNetworker`: on `OnSpawnServer(NetworkConnection)` and `OnConfigChanged(BaseConfiguration)` the host sends `ApplySettingsJson(NetworkConnection, string, string)` (Target RPC) so client settings JSON matches host.

## Save Participation
- Configurations are `ScriptableObject` assets; **no** `ISaveable` implementation in this namespace (grep verified). Persistence is via the `Write*Settings` methods on `Settings` (file-based), not via the save system.

## Settings UI
- `GameSettingsWindow` (`ScheduleOne.UI.Settings`), `SettingsScreen` (`ScheduleOne.UI.MainMenu`), controls: `SettingsSlider`, `SettingsToggle`, `SettingsDropdown`, `ConfirmDisplaySettings`, `RebindActionUI` (DevUtilities, input rebinding UI)

## Hook Points (Harmony)
1. `ConfigurationService.GetConfigurationAndListenForChanges(...)` — usually **no patch needed**: call it directly to observe any game configuration.
2. `Settings.ApplyDisplaySettings(DisplaySettings)` / `ApplyGraphicsSettings(...)` — postfix to enforce mod settings (e.g., clamp/override after the game applies its own).
3. `ConfigurationServiceNetworker.ApplySettingsJson` (Target RPC) — patch to intercept/filter synced settings in multiplayer.

## Modding Configuration (Mods)
- **No `ModConfig<T>` class exists in S1API** (only appears inside a doc-comment example in `S1API.Internal.Abstraction.Saveable` — do not reference it as an API).
- Standard pattern: MelonLoader `MelonPreferences.CreateCategory(...)` / `CreateEntry<T>(...)` → `UserData/<Mod>.cfg`; S1API itself does this internally via `S1APIPreferences`.
- Game settings themselves: prefer `ConfigurationService.TryGetConfiguration<T>` + `Settings.Apply*/Write*` instead of editing config files behind the game's back.

---

---

---

---

---

---

---

---

## Unresolved identifiers (f12 static check 2026-10-08)

These documented identifiers were not found in the f12 game assemblies, the checked-in S1API/S1MAPI source, or the workspace source. Treat them as drift candidates and re-derive them from the current decompiles before relying on this document.

- `ReloadDisplaySettings`
