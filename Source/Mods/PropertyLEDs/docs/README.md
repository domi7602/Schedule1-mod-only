# PropertyLEDs

Room-by-room RGB LED strips along the ceiling and wall edges of your owned properties and
businesses, controlled from a dedicated phone app. Developed independently for *Schedule I*
(MelonLoader, IL2CPP, S1API). No code, models or textures from any existing LED mod are used.

> **Status: Verified, owner-confirmed 2026-10-24 (v0.1.8, location-aware initial selection and property-named power status; 115 tests pass).** The confirmation had been missing from the record and was supplied retroactively; native location/status fitting, toggle/color and the save roundtrip were not itemized. The mod builds, deploys and ships a complete settings,
> rendering, phone and diagnostics implementation. It deliberately contains **no built-in room
> coordinates**: until a room has been measured and marked verified in the layout catalog, no
> persistent LED geometry is placed there. See
> [`implementation-status.md`](implementation-status.md) for the implementation history and the
> evidence that predates the owner confirmation.

## What it does

- **Ceiling-edge LED strips** built at runtime as one merged mesh per room — no prefabs, no
  AssetBundles, no colliders, one material per room group.
- **Property-wide colour** with optional **per-room overrides**: a property default plus any number
  of rooms that deviate from it.
- **Phone app** *Property LEDs*: a property selector with larger readable labels, **Color** (rainbow colour bar, no
  degrees), **Saturation** (white to strong colour, percentage) and **Brightness** (0–5).
  **Advanced** opens the retained **Color intensity** control; every fresh app open starts collapsed.
  The white preset and on/off switch remain. The room selector, RGB preview and inheritance button
  were removed in 0.1.6; phone edits target the property default. Measured available height is distributed across
  the controls with bounded growth. Loading/failure messages appear inline only when needed;
  small screens retain scrolling. All original slider ranges and edit semantics are retained.
  The English mockup-inspired UI has been compiled, tested and compared in a rendered HTML preview
    ([visual comparison](visual-preview/README.md)). Native rendering and hit testing still need
    a game check. Only the stateful switch changes power; there is no duplicate power button.
- **Console command** `led` for measurement, preview, export and manual control (see
  [`diagnostics.md`](diagnostics.md)).
- **Saved per save slot** (`settings_slot_{n}.json`, atomic writes with `.bak`, 1 s debounce).
  Settings and layouts can never leak between slots.
- **Verified-layout gate**: unverified geometry is only ever shown through an explicit preview and
  is never installed as real room LEDs.

## Installation

1. **Requirements:** Schedule I (IL2CPP branch), MelonLoader 0.7.3, S1API 3.2.x.
2. Copy `PropertyLEDs.dll` (and the shared `Shared.dll`) into `<GameDir>\Mods\`.
3. Copy `UserData/PropertyLEDs/` into `<GameDir>\UserData\PropertyLEDs\` (metadata, optional).
4. Start the game. The mod creates its files on first use:

```text
<UserData>\PropertyLEDs\
├── settings_slot_{n}.json     per save slot: property defaults + room overrides
├── layouts.json               measured room layouts (starts empty)
└── diagnostics/               hierarchy dumps and layout candidates
```

## Usage

1. Buy a property or business, load the save and open the phone.
2. Open **Property LEDs**. The property selector lists only owned properties/businesses. On the first actual opening after loading, it selects the player's exact current owned property/business if available; otherwise it falls back to the first owned entry. No nearest-property guess is used.
3. Pick a property if needed. Manual choices survive app close/reopen within the loaded session; loading a save re-arms initial location selection. The power status names the selected property, not necessarily the room you are standing in. Phone controls edit its **Property default**; merely opening/navigating retains saved room overrides.
4. Move a slider, press **White**, or switch **LEDs ON/OFF**. Changes apply to the world
   immediately and are written to disk at most once per second.
   Open **Advanced** for **Color intensity** when needed; opening/closing this panel changes only
   the view and never resets a saved value or enables the LEDs.
5. Room-specific controls are not exposed in the current phone UI; saved overrides remain supported by the runtime.

A room only shows LEDs once its geometry has been measured and marked verified in `layouts.json`
(see [`diagnostics.md`](diagnostics.md)). Everything else stays fully configurable.

## Inheritance rules

| Action | Effect |
|---|---|
| Edit **Property default** | Applies to every room of that property and **removes all room overrides** |
| Edit a **single room** | Creates/updates an override for that room only |
| **Use property color** | Deletes the room's override; the room follows the property again |
| LEDs off | Stays off until the effective settings for that scope change again |

## Layout data format (`layouts.json`)

```json
{
  "FormatVersion": 1,
  "Rooms": [
    {
      "PropertyId": "stable property id",
      "RoomId": "stable room id",
      "DisplayName": "Living room",
      "AnchorPath": ".",            
      "Verified": false,
      "Width": 0.035,
      "Paths": [
        { "Closed": true,  "Points": [ { "X": 0, "Y": 2.4, "Z": 0 }, { "X": 4, "Y": 2.4, "Z": 0 } ] },
        { "Closed": false, "Points": [ { "X": 0, "Y": 2.4, "Z": 0 }, { "X": 0, "Y": 2.4, "Z": 3 } ] }
      ]
    }
  ]
}
```

- `AnchorPath` is relative to the owned property root; `"."` selects that root itself. Points are
  **anchor-local metres**, so layouts survive property transforms.
- `Width` is the beam cross-section side in metres (0.001–1).
- `Verified: true` means "measured and visually checked in game". Only verified rooms are rendered
  persistently; `Verified: false` is preview-only.
- Unsupported `FormatVersion`, corrupt files and duplicate `(PropertyId, RoomId)` pairs are refused
  without modifying the file.

## Known limitations

- No room layouts ship with the mod yet; every property has to be measured in game first.
- No realtime Unity lights: the strips are emissive only, so nearby surfaces are not tinted.
- Multiplayer synchronisation and foreign property mods are out of scope.
- Automatic room detection is not implemented; layouts are data-driven on purpose.

## Development

See [`diagnostics.md`](diagnostics.md) for the measurement workflow, [`core-tests.md`](core-tests.md)
for the automated test report and [`environment-baseline.md`](environment-baseline.md) for the
audited toolchain this build targets.
