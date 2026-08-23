# Configuration (Schedule I)

## Core Classes

| Class | Purpose |
|-------|---------|
| `ConfigurationService` | Configuration management service |
| `ConfigurationServiceNetworker` | Network sync for settings/configuration |
| `Configuration` | Main configuration data class |
| `BaseConfiguration` | Base configuration class |

## Settings Categories

- `GameSettings`: General game settings
- `InputSettings`: Key bindings and controls
- `GraphicsSettings`: Graphics options
- `AudioSettings`: Audio settings (wrapper)
- `DisplaySettings`: Display options
- `OtherSettings`: Miscellaneous settings

## Settings UI

- `GameSettingsWindow`: Settings menu window
- Numerous UI controls: `SettingsSlider`, `SettingsToggle`, `SettingsDropdown`
- `Keybinder`: Rebinding key UI
- `ResolutionDropdown`, `QualityDropdown`, `DisplayModeDropdown`

## Modding Configuration

- Custom configurations via `ModConfig<T>` in Shared
- MelonLoader `UserData/<Mod>.cfg` pattern
- No direct modification of the game configuration required
