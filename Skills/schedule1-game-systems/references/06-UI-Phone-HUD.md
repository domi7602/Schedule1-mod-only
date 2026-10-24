# UI/Phone/HUD (Schedule I)

> Evidence: inherited notes from the supplied Skills.zip. The reported 2026-10-08 static scan was not rerun for this package. Treat symbols, values and behavior as lookup candidates until checked against the target build; [scope and workflow](README.md).
> UNVERIFIED against the installed runtime. Static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check: 30/31 identifier-shaped tokens resolve (0 documented as absent). Unresolved identifiers are listed at the end of this file. Runtime behaviour is not covered by this sweep.

## Phone (3D Worldspace UI)
- `Phone.cs`: 3D model, `GraphicRaycaster`, 2 orientations (Vertical/Horizontal)
- Two canvases: `HomeScreen` (icons, time) + `AppsCanvas` (app container)
- Static variable: `Phone.ActiveApp`

## App System
- Generic base: `App<T> : PlayerSingleton<T>`
- Each app registers automatically in `OnStartClient()` → `Apps.Add(this)`
- App icons via `HomeScreen.GenerateAppIcon<T>()` (keyboard shortcuts 1-9)

### Available Apps
| App | Class | Description |
|-----|-------|-------------|
| ProductManager | `ProductManagerApp` | Manage products, prices, favorites |
| Messages | `MessagesApp` | SMS conversations |
| DealerManagement | `DealerManagementApp` | Manage dealers |
| Contacts | `ContactsApp` | NPC contacts, region map |
| Map | `MapApp` | Map with POIs |
| Delivery | `DeliveryApp` | Order deliveries |
| Journal | `JournalApp` | Quest journal |
| CallInterface | `CallInterface` | Phone call UI |
| PhoneShopInterface | `PhoneShopInterface` | In-app shop |

## ProductManagerApp Details
- Left side: Product list **grouped by DrugType** (Marijuana, Meth, Coke, Shrooms, MDMA, Heroin) + favorites container
- Right side: `ProductAppDetailPanel` with:
  - Price input field (InputField, $1–$999, rounded)
  - Suggested Price (calculated market value)
  - Listed toggle (enable/disable for sale)
  - Name + Description ("???" if undiscovered)
  - Effect properties
  - Recipe view (ingredients + mixer + output)
  - Addiction degree slider (0-100%)
- **NOT** in the app: Quality, stock level, revenue statistics, deals/deliveries

## HUD (ScreenSpace Overlay)
- `Singleton<HUD>`:
  - Crosshair, Hotbar (8 slots), Cash/Online balance
  - Quest HUD, Crime status, Notifications
  - Sleep/Curfew prompts
  - Weapon reticle (`ReticleController`)

## UIScreenManager (Stack-Based)
- `PersistentSingleton<UIScreenManager>`
- `Stack<UIScreenInfo>` (LIFO)
- Back/Escape pops top screen
- Popup system for modal dialogues (`OpenPopupScreen()`)

## Tooltips/Interaction
- `TooltipManager`: Raycasts across all canvases, displays `Tooltip` component
- `InteractionCanvas`: World interaction prompts ("Press E to use")
- `InputPromptsCanvas`: Context-sensitive key bindings/prompts

---

## Unresolved identifiers in the imported 2026-10-08 record

The imported scan did not resolve the following tokens within its recorded search scope. These may be placeholders, attributes from another assembly or drift candidates. Search the actual declaring assembly and version before concluding that an API was removed.

- `InputPromptsCanvas`
