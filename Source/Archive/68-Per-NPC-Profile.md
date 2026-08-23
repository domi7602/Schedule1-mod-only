# Profiles of the 79 NPC Character Classes

**Source Scope:** `ScheduleOne/NPCs/CharacterClasses/` as well as direct references from `Quests/`, `Map/DarkMarket.cs`, and the sewer documentation.

> Names, NPC IDs, regions, shop assortments, and daily schedules are mostly serialized as Unity scene/prefab data and are not present as literals in the decompiled C# classes. Such values are deliberately designated as **unknown** or **unspecified**. `Contract` and `DeaddropQuest` work generically with `Customer` or `DeadDrop`; they do not name individual character classes. The actual direct base classes are `Supplier`, `Dealer`, or `NPC` — not `Customer` or `DealerStaticDoor`.

## Overview

| Name | ID | Role | Region/Location |
|---|---|---|---|
| Albert | unknown | Supplier | unspecified |
| Phil | unknown | Supplier | unspecified |
| Salvador | unknown | Supplier | unspecified |
| Shirley | unknown | Supplier | unspecified |
| Benji | unknown | Dealer | South-Central Plaza; Region unknown |
| Brad | unknown | Dealer | unspecified |
| Jane | unknown | Dealer | unspecified |
| Leo | unknown | Dealer | unspecified |
| Molly | unknown | Dealer | unspecified |
| Wei | unknown | Dealer | unspecified |
| SchizoGoblin | unknown | Sewer Special | Sewers |
| SewerGoblin | unknown | Sewer Special | Sewers |
| SewerKing | unknown | Sewer Special/Boss | Sewer Office |
| Alison | unknown | Generic NPC | unspecified |
| Anna | unknown | Generic NPC/Barber | unspecified |
| Austin | unknown | Generic NPC | unspecified |
| Beth | unknown | Generic NPC | unspecified |
| Bruce | unknown | Generic NPC | unspecified |
| Carl | unknown | Generic NPC | unspecified |
| Charles | unknown | Generic NPC | unspecified |
| Chris | unknown | Generic NPC | unspecified |
| Dan | unknown | Generic NPC/Shop | unspecified |
| Dean | unknown | Generic NPC/Tattoo Artist | unspecified |
| Dennis | unknown | Generic NPC | unspecified |
| Elizabeth | unknown | Generic NPC | unspecified |
| Eugene | unknown | Generic NPC | unspecified |
| Fiona | unknown | Generic NPC/Shop | unspecified |
| Fixer | unknown | Generic NPC/Special Dialogue | unspecified |
| Frank | unknown | Generic NPC | unspecified |
| Genghis | unknown | Generic NPC | unspecified |
| George | unknown | Generic NPC | unspecified |
| Geraldine | unknown | Generic NPC | unspecified |
| Greg | unknown | Generic NPC | unspecified |
| Harold | unknown | Generic NPC | unspecified |
| Herbert | unknown | Generic NPC/Shop | unspecified |
| Igor | unknown | Generic NPC/Doorkeeper | Dark Market/Warehouse Entrance |
| Irene | unknown | Generic NPC | unspecified |
| Jack | unknown | Generic NPC | unspecified |
| Jackie | unknown | Generic NPC | unspecified |
| Javier | unknown | Generic NPC | unspecified |
| Jeff | unknown | Generic NPC | unspecified |
| Jen | unknown | Generic NPC/Key Merchant | unspecified |
| Jennifer | unknown | Generic NPC | unspecified |
| Jeremy | unknown | Generic NPC/Vehicle Dealer | Dealership; Region unknown |
| Jessi | unknown | Generic NPC | unspecified |
| Joyce | unknown | Generic NPC | unspecified |
| Karen | unknown | Generic NPC | unspecified |
| Kathy | unknown | Generic NPC | unspecified |
| Keith | unknown | Generic NPC | unspecified |
| Kelly | unknown | Generic NPC | unspecified |
| Kevin | unknown | Generic NPC | unspecified |
| Kim | unknown | Generic NPC | unspecified |
| Kyle | unknown | Generic NPC | unspecified |
| Lily | unknown | Generic NPC/Tutorial | Tutorial/Regular Schedule; Location unknown |
| Lisa | unknown | Generic NPC | unspecified |
| Louis | unknown | Generic NPC | unspecified |
| Lucy | unknown | Generic NPC | unspecified |
| Ludwig | unknown | Generic NPC | unspecified |
| Mac | unknown | Generic NPC | unspecified |
| Marco | unknown | Generic NPC/Vehicle Service | Vehicle Repair Shop; Region unknown |
| Melissa | unknown | Generic NPC | unspecified |
| Michael | unknown | Generic NPC | unspecified |
| Mick | unknown | Generic NPC/Pawnshop Keeper | Pawn Shop; Region unknown |
| Ming | unknown | Generic NPC | unspecified |
| Oscar | unknown | Generic NPC/Dark Market Shop | Dark Market/Warehouse |
| Pearl | unknown | Generic NPC | unspecified |
| Peggy | unknown | Generic NPC | unspecified |
| Peter | unknown | Generic NPC | unspecified |
| Philip | unknown | Generic NPC | unspecified |
| Randy | unknown | Generic NPC | unspecified |
| Ray | unknown | Generic NPC/Real Estate Contact | Manor reference; Region unknown |
| Sam | unknown | Generic NPC/Quest Contact | Tunnel; Region unknown |
| Sherman | unknown | Generic NPC | unspecified |
| Steve | unknown | Generic NPC/Shop | unspecified |
| Thomas | unknown | Generic NPC/Cartel Contact | Meeting/Vehicle; Region unknown |
| Tobias | unknown | Generic NPC | unspecified |
| Trent | unknown | Generic NPC | unspecified |
| Uncle Nelson | unknown | Generic NPC/Tutorial Contact | Payphone Contact; Location unknown |
| Walter | unknown | Generic NPC | unspecified |

## Supplier

### Albert

- **Full Name / ID:** Albert; surname and NPC ID unknown.
- **Role:** Supplier.
- **Inheritance:** `Albert extends Supplier`; `Supplier extends NPC`.
- **Location/Region:** Unspecified in C# code.
- **Quest Relation:** No direct class reference from `Quests/`; `Contract`/`DeaddropQuest` do not name Albert.
- **Inventory/Sales:** Assortment and prices are not defined in class code; Supplier shop data is serialized.
- **Peculiarities:** Pure Supplier specialization without additional character logic.

### Phil

- **Full Name / ID:** Phil; surname and NPC ID unknown.
- **Role:** Supplier.
- **Inheritance:** `Phil extends Supplier`; `Supplier extends NPC`.
- **Location/Region:** Unspecified in C# code.
- **Quest Relation:** Indirect relation to mushroom cultivation; no direct reference from a quest class.
- **Inventory/Sales:** Specific assortment unspecified; dialogue offers "How do I grow shrooms?".
- **Peculiarities:** Triggers the dialogue chain `grow_shrooms_instructions`; upon unlock `TemperatureSystemEnabled` is activated.

### Salvador

- **Full Name / ID:** Salvador; surname and NPC ID unknown.
- **Role:** Supplier.
- **Inheritance:** `Salvador extends Supplier`; `Supplier extends NPC`.
- **Location/Region:** Unspecified in C# code.
- **Quest Relation:** No direct class reference from `Quests/`.
- **Inventory/Sales:** Assortment, items, and prices unspecified in class code.
- **Peculiarities:** No additional logic beyond inherited Supplier functionality.

### Shirley

- **Full Name / ID:** Shirley; surname and NPC ID unknown.
- **Role:** Supplier.
- **Inheritance:** `Shirley extends Supplier`; `Supplier extends NPC`.
- **Location/Region:** Unspecified in C# code.
- **Quest Relation:** Dead drop orders utilize Supplier logic; `DeaddropQuest` does not bind Shirley directly.
- **Inventory/Sales:** Specific assortment unspecified; orders run via `PhoneShopInterface.CartEntry`.
- **Peculiarities:** Each confirmed dead drop order increments `ShirleyDeaddropOrders`.

## Dealer

### Benji

- **Full Name / ID:** Benji; surname and NPC ID unknown.
- **Role:** Dealer.
- **Inheritance:** `Benji extends Dealer`; `Dealer extends NPC` and implements `IItemSlotOwner`.
- **Location/Region:** South-Central Plaza according to existing NPC documentation; EMapRegion unknown.
- **Quest Relation:** Recruitment can trigger further serialized flows via `onRecruitmentRequested`; no quest class names Benji directly.
- **Inventory/Items:** Counts `WeedInstance` quantities in dealer inventory; additionally his cash balance is tracked.
- **Peculiarities:** Writes recommendation, weed quantity, cash, and customer count to variables; hides potential dealer PoI.

### Brad

- **Full Name / ID:** Brad; surname and NPC ID unknown.
- **Role:** Dealer.
- **Inheritance:** `Brad extends Dealer`; `Dealer extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference from `Quests/`; generic dealer assignment to contracts is possible.
- **Inventory/Items:** Not individually specified; uses inherited dealer inventory.
- **Peculiarities:** No additional logic; availability and customers are controlled by `Dealer`/serialized data.

### Jane

- **Full Name / ID:** Jane; surname and NPC ID unknown.
- **Role:** Dealer, not Generic NPC.
- **Inheritance:** `Jane extends Dealer`; `Dealer extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference from `Quests/`; can be deployed via generic contract system.
- **Inventory/Items:** Not individually specified; inherited dealer inventory.
- **Peculiarities:** No additional character logic in `Jane.cs`.

### Leo

- **Full Name / ID:** Leo; surname and NPC ID unknown.
- **Role:** Dealer, not Generic NPC.
- **Inheritance:** `Leo extends Dealer`; `Dealer extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference from `Quests/`.
- **Inventory/Items:** Not individually specified; inherited dealer inventory.
- **Peculiarities:** No additional character logic in `Leo.cs`.

### Molly

- **Full Name / ID:** Molly; surname and NPC ID unknown.
- **Role:** Dealer.
- **Inheritance:** `Molly extends Dealer`; `Dealer extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference from `Quests/`.
- **Inventory/Items:** Not individually specified; inherited dealer inventory.
- **Peculiarities:** No additional character logic in `Molly.cs`.

### Wei

- **Full Name / ID:** Wei; additional name components and NPC ID unknown.
- **Role:** Dealer.
- **Inheritance:** `Wei extends Dealer`; `Dealer extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference from `Quests/`.
- **Inventory/Items:** Not individually specified; inherited dealer inventory.
- **Peculiarities:** No additional character logic in `Wei.cs`.

## Sewer Special

### SchizoGoblin

- **Full Name / ID:** SchizoGoblin; NPC ID unknown.
- **Role:** Sewer Special; no sewer boss in present code.
- **Inheritance:** `SchizoGoblin extends NPC`.
- **Location/Region:** Sewers; specific position unspecified.
- **Quest Relation:** No direct reference from `Quests/`.
- **Inventory/Items:** No item access in class code.
- **Peculiarities:** Bound to a target player and only visible on NPC layer to them; `Activate()` is empty.

### SewerGoblin

- **Full Name / ID:** SewerGoblin; NPC ID unknown.
- **Role:** Sewer Special/Enemy.
- **Inheritance:** `SewerGoblin extends NPC`.
- **Location/Region:** Sewers; uses a `SewerHidingBuilding` as retreat point.
- **Quest Relation:** No direct quest class reference; belongs to sewer system.
- **Inventory/Items:** A serialized `PacifyItem` can pacify him; during retrieval he takes an item from player inventory.
- **Peculiarities:** States Inactive, Attacking, Retrieving, and Retreating; deployment at earliest after 12 hours, then minute-by-minute random check.
- **Peculiarities:** Retreats on low health or hit chance; target must be inside sewers.

### SewerKing

- **Full Name / ID:** SewerKing; NPC ID unknown.
- **Role:** Sewer Special/Boss.
- **Inheritance:** `SewerKing extends NPC`.
- **Location/Region:** Guards the `SewerOffice` in the sewers.
- **Quest Relation:** No direct reference from `Quests/`; progress persisted in sewer system as `HasSewerKingBeenDefeated`.
- **Inventory/Items:** No items or sales in class code.
- **Peculiarities:** Server-side attacks players whose `CurrentProperty` is the Sewer Office; no special boss phases.

## Generic NPCs

### Alison

- **Full Name / ID:** Alison; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Alison extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Anna

- **Full Name / ID:** Anna; surname and ID unknown.
- **Role:** Generic NPC with barber dialogue function.
- **Inheritance:** `Anna extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** No item access; haircut selection integrated into dialogue.
- **Peculiarities:** `HairCutChoiceValid` currently always allows first dialogue choice.

### Austin

- **Full Name / ID:** Austin; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Austin extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Beth

- **Full Name / ID:** Beth; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Beth extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Bruce

- **Full Name / ID:** Bruce; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Bruce extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Carl

- **Full Name / ID:** Carl; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Carl extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Charles

- **Full Name / ID:** Charles; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Charles extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Chris

- **Full Name / ID:** Chris; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Chris extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Dan

- **Full Name / ID:** Dan; surname and ID unknown.
- **Role:** Generic NPC with shop.
- **Inheritance:** `Dan extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** First completed purchase sets `Dan_Greeting_Done` and triggers serialized greeting event.
- **Inventory/Items:** Sells via `ShopInterface`; assortment not specified in C# code.
- **Peculiarities:** Plays voice-over and random closing lines after orders.

### Dean

- **Full Name / ID:** Dean; surname and ID unknown.
- **Role:** Generic NPC with tattoo dialogue.
- **Inheritance:** `Dean extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** No item access; tattoo selection runs via dialogue.
- **Peculiarities:** `TattooChoiceValid` currently always allows first dialogue choice.

### Dennis

- **Full Name / ID:** Dennis; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Dennis extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Elizabeth

- **Full Name / ID:** Elizabeth; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Elizabeth extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Eugene

- **Full Name / ID:** Eugene; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Eugene extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Fiona

- **Full Name / ID:** Fiona; surname and ID unknown.
- **Role:** Generic NPC with shop.
- **Inheritance:** `Fiona extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Sells via `ShopInterface`; assortment not in class code.
- **Peculiarities:** Shop selection always valid; thanks player via voice-over and random line.

### Fixer

- **Full Name / ID:** Fixer; real name and ID unknown.
- **Role:** Generic NPC with special dialogue.
- **Inheritance:** `Fixer extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct quest class reference; greeting progress stored via `FixerGreeted`.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** Uses an overriding `GreetingDialogue` until first conversation.

### Frank

- **Full Name / ID:** Frank; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Frank extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Genghis

- **Full Name / ID:** Genghis; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Genghis extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### George

- **Full Name / ID:** George; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `George extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Geraldine

- **Full Name / ID:** Geraldine; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Geraldine extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Greg

- **Full Name / ID:** Greg; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Greg extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Harold

- **Full Name / ID:** Harold; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Harold extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Herbert

- **Full Name / ID:** Herbert; surname and ID unknown.
- **Role:** Generic NPC with shop.
- **Inheritance:** `Herbert extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Sells via `ShopInterface`; assortment not in class code.
- **Peculiarities:** Closing dialogue replaces `formal_address` with formal player salutation.

### Igor

- **Full Name / ID:** Igor; surname and ID unknown.
- **Role:** Generic NPC/Doorkeeper.
- **Inheritance:** `Igor extends NPC`.
- **Location/Region:** At main entrance of Dark Market/Warehouse; Region unspecified.
- **Quest Relation:** `DarkMarket` disables Igor when setting `WarehouseUnlocked`.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** Indirectly controls locked access prior to Warehouse unlock.

### Irene

- **Full Name / ID:** Irene; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Irene extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Jack

- **Full Name / ID:** Jack; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Jack extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Jackie

- **Full Name / ID:** Jackie; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Jackie extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Javier

- **Full Name / ID:** Javier; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Javier extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Jeff

- **Full Name / ID:** Jeff; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Jeff extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Jen

- **Full Name / ID:** Jen; surname and ID unknown.
- **Role:** Generic NPC/Key Merchant.
- **Inheritance:** `Jen extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** Part of sewer access, but without direct reference from a quest class.
- **Inventory/Items:** Sells a Sewer Key at relation 3.0 according to sewer system; no item literal in `Jen.cs`.
- **Peculiarities:** One of three access pathways to the sewers.

### Jennifer

- **Full Name / ID:** Jennifer; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Jennifer extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Jeremy

- **Full Name / ID:** Jeremy; surname and ID unknown.
- **Role:** Generic NPC/Vehicle Dealer.
- **Inheritance:** `Jeremy extends NPC`.
- **Location/Region:** References a `Dealership`; Region unspecified.
- **Quest Relation:** No direct quest class reference; first contact stored via `JeremyGreeted`.
- **Inventory/Items:** Sells vehicles from serialized `DealershipListing` entries; code, name, and price originate from `VehicleManager`.
- **Peculiarities:** Custom greeting dialogue until first conversation.

### Jessi

- **Full Name / ID:** Jessi; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Jessi extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Joyce

- **Full Name / ID:** Joyce; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Joyce extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Karen

- **Full Name / ID:** Karen; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Karen extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Kathy

- **Full Name / ID:** Kathy; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Kathy extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Keith

- **Full Name / ID:** Keith; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Keith extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Kelly

- **Full Name / ID:** Kelly; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Kelly extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Kevin

- **Full Name / ID:** Kevin; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Kevin extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Kim

- **Full Name / ID:** Kim; additional name components and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Kim extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Kyle

- **Full Name / ID:** Kyle; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Kyle extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Lily

- **Full Name / ID:** Lily; surname and ID unknown.
- **Role:** Generic NPC with tutorial schedule.
- **Inheritance:** `Lily extends NPC`.
- **Location/Region:** Possesses separate `TutorialScheduleGroup` and `RegularScheduleGroup`; locations not named in C# code.
- **Quest Relation:** Tutorial conditions are serialized; upon relationship unlock `Lily_Unlocked` is set.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** Daily routine can alternate between tutorial and regular group.

### Lisa

- **Full Name / ID:** Lisa; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Lisa extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Louis

- **Full Name / ID:** Louis; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Louis extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Lucy

- **Full Name / ID:** Lucy; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Lucy extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Ludwig

- **Full Name / ID:** Ludwig; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Ludwig extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Mac

- **Full Name / ID:** Mac; additional name components and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Mac extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Marco

- **Full Name / ID:** Marco; surname and ID unknown.
- **Role:** Generic NPC/Vehicle Service.
- **Inheritance:** `Marco extends NPC`.
- **Location/Region:** Vehicle repair shop with recovery point and parking detector; Region unspecified.
- **Quest Relation:** No direct quest class reference; first contact stored as `MarcoGreeted`.
- **Inventory/Items:** No merchandise list; sells vehicle repainting via `VehicleModMenu`.
- **Peculiarities:** Can recover the last driven, unoccupied vehicle and repaint a correctly parked vehicle.

### Melissa

- **Full Name / ID:** Melissa; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Melissa extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Michael

- **Full Name / ID:** Michael; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Michael extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Mick

- **Full Name / ID:** Mick; surname and ID unknown.
- **Role:** Generic NPC/Pawnshop Keeper.
- **Inheritance:** `Mick extends NPC`.
- **Location/Region:** Pawn Shop; Region unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Pawn trading via dialogue; specific accepted items are not defined in `Mick.cs`.
- **Peculiarities:** Refuses business as long as `PawnShopAngeredToday` is true.

### Ming

- **Full Name / ID:** Ming; additional name components and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Ming extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No identifiable shop or quest function in evaluated code.

### Oscar

- **Full Name / ID:** Oscar; surname and ID unknown.
- **Role:** Generic NPC/Dark Market Shop.
- **Inheritance:** `Oscar extends NPC`.
- **Location/Region:** Dark Market/Warehouse; EMapRegion unspecified.
- **Quest Relation:** `DarkMarket.SetUnlocked` calls his delivery unlock upon `WarehouseUnlocked`; also according to sewer doc Oscar gives hint for world key.
- **Inventory/Items:** Sells via `ShopInterface`; assortment not in C# code. After unlock shop becomes available in Delivery app.
- **Peculiarities:** Custom one-time greeting dialogue via `OscarGreeted`; Dark Market only opens within time window and not during pursuit.

### Pearl

- **Full Name / ID:** Pearl; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Pearl extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Peggy

- **Full Name / ID:** Peggy; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Peggy extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Peter

- **Full Name / ID:** Peter; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Peter extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inheritance:** `Peter extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Philip

- **Full Name / ID:** Philip; surname and ID unknown.
- **Role:** Generic NPC; not to be equated with Supplier Phil.
- **Inheritance:** `Philip extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** Separate class alongside `Phil`; no additional character logic.

### Randy

- **Full Name / ID:** Randy; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Randy extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Ray

- **Full Name / ID:** Ray; surname and ID unknown.
- **Role:** Generic NPC/Real Estate Contact.
- **Inheritance:** `Ray extends NPC`.
- **Location/Region:** Connected to `Manor` property; own whereabouts unspecified.
- **Quest Relation:** Intro conditions are rank, by default 21 game days and 15,000 net worth; no quest class names Ray directly.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** Sends dialogue chain `manor_rebuilt` when manor reconstruction is complete; greeting and intro status are stored.

### Sam

- **Full Name / ID:** Sam; surname and ID unknown.
- **Role:** Generic NPC/Quest Contact.
- **Inheritance:** `Sam extends NPC`.
- **Location/Region:** Tunnel reference; Region unspecified.
- **Quest Relation:** `Quest_DefeatCartel` calls `SendTunnelDugMessage()`.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** Sends messaging dialogue chain `tunnel_dug` with a short delay.

### Sherman

- **Full Name / ID:** Sherman; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Sherman extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Steve

- **Full Name / ID:** Steve; surname and ID unknown.
- **Role:** Generic NPC with shop.
- **Inheritance:** `Steve extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Sells via `ShopInterface`; assortment not in class code.
- **Peculiarities:** Thanks player after orders via voice-over and random closing line.

### Thomas

- **Full Name / ID:** Thomas; surname and ID unknown.
- **Role:** Generic NPC/Cartel Contact.
- **Inheritance:** `Thomas extends NPC`.
- **Location/Region:** Uses a meeting location or meeting vehicle in quests; specific region unknown.
- **Quest Relation:** Direct relation to `Quest_UnfavourableAgreements` and `Quest_TheDeepEnd`; sends `thomas_intro` message chain.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** Can end an agreement with the Benzies; this irreversibly sets cartel status to `Hostile`.

### Tobias

- **Full Name / ID:** Tobias; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Tobias extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Trent

- **Full Name / ID:** Trent; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Trent extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

### Uncle Nelson

- **Full Name / ID:** Uncle Nelson; additional name and NPC ID unknown.
- **Role:** Generic NPC/Tutorial Contact.
- **Inheritance:** `UncleNelson extends NPC`.
- **Location/Region:** Contacts player via mobile phone and refers to a payphone; own location unknown.
- **Quest Relation:** Direct relation to `Quest_GettingStarted` and `Quest_WelcomeToHylandPoint`.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** Sends a one-time initial, initially unknown/unread message; has a differing demo version.

### Walter

- **Full Name / ID:** Walter; surname and ID unknown.
- **Role:** Generic NPC.
- **Inheritance:** `Walter extends NPC`.
- **Location/Region:** Unspecified.
- **Quest Relation:** No direct class reference found.
- **Inventory/Items:** Unspecified.
- **Peculiarities:** No character-specific logic beyond network initialization.

## Classification of Open Data

- **79/79 classes documented:** 4 Suppliers, 6 Dealers, 3 Sewer Specials, and 66 direct `NPC` subclasses.
- **NPC IDs and Surnames:** Not present as values in the investigated C# files; they likely reside in Unity prefabs/scenes.
- **Regions and Schedules:** The directory `NPCs/Schedules/` contains only the generic schedule system, no per-character named schedule files. Specific schedule groups are serialized object references.
- **Quest Coverage:** Direct C# references exist for Uncle Nelson, Thomas, and Sam. Oscar, Igor, Lily, Ray, Shirley, Phil, and Sewer NPCs have systemic or variable-based progress references.
- **Assortments:** The `ShopInterface`/Supplier/Dealer offers are serialized; in character code only Benji's weed inventory, Shirley's shopping cart type, and the functional types of special shops are explicitly visible.
