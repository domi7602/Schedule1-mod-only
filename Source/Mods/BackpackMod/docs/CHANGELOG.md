# Changelog


## 1.2.2 (2026-09-12) — Bug-Audit-Fixes Runde 4 (Audit 2026-09-12)
- **HUD-Sort-Button Raycast-Schutz:** Nach dem Klonen des Vanilla-Close-Buttons setzen wir `img.raycastTarget = false` auf der Image-Hülle (Button selbst bleibt raycast-fähig). Verhindert, dass der Sort-Button bei seiner Anchor-Position (-24, 150) Drag-/Click-Events auf den darunter liegenden Slot-Grid schluckt.

## 1.2.1 (2026-09-12) — Bug-Audit-Fixes Runde 3 (Audit 2026-09-12)
- ObjLoader: `LoadMeshFromObj` prueft jetzt die Dateigroesse (50 MB Cap) vor `File.ReadAllLines` und bricht die Parse-Loop bei >250 000 Vertices ab. Schuetzt vor Frame-Spikes bei versehentlich riesigen oder korrumpierten OBJ-Files im `UserData/BackpackMod/models/`.
- `BackpackDefinitions` Listing-Snapshot-Dump jetzt hinter `#if DEBUG` (war pro Load aktiv und flutete das MelonLoader-Log in Release).

## 1.2.0 (2026-09-12) — Bug-Audit-Fixes (Audit 2026-09-12)
- **Sort-Commit atomar (HIGH):** Vor dem ersten Write werden alle Instances via `ItemInstance.GetCopy(quantity)` vorbereitet (erhaelt Qualitaet/Packaging/Instanz-State komplett). Schlaegt eine Vorbereitung fehl, wird vor JEDEM Write abgebrochen — kein Verlust, keine Duplikation.
- **Clipboard-Slot-Schutz (MEDIUM):** Player-Inventory-Sort filtert jetzt per Referenzvergleich gegen `inv.clipboardSlot`/`inv.cashSlot`; Hotbar 0-7 ist die einzige Sortzone. Vanilla-UI-Validierung der Spezialslots wird nicht mehr umgangen.
- **Overflow-Downgrade-Quality+Packaging (HIGH):** Beim Tier-Downgrade wird die Live-Instanz via `GetCopy` zurueckgegeben (kein Default-Reset mehr). Overflow-Items, die nicht ins Inventar passen, werden in `backpack_overflow_slot_{n}.json` persistiert und beim naechsten Backpack-Oeffnen automatisch zurueckgegeben.
- **Cross-Save-Protect (MEDIUM):** `ResetCache` savet nicht mehr vor dem Zerstoeren (OnPreLoad-Cross-Save-Vermutung); die Main-Unload-Pfad-Kette `OnSceneWasUnloaded → ResetForSceneUnload → SaveStorage` ist explizit.
- Doku: Item-Beschreibungs-Text „Drop it to access its inventory" widerspricht dem realen ToggleStorage-Flow (Info-Notiz; keine Aenderung).

## 1.1.0 (2026-09-12)
- **B1 Sort (QoL Spec v1.1.0, button-only nach User-Entscheidung):** Ein-Klick-Sortierung fuer Backpack (StorageEntity), Player-Inventory (Hotbar 0-8, Cash-Slot ausgenommen) und geoeffnete Vanilla-Storage-Container. Keine Hotkeys — ausschliesslich Buttons.
- "Sort"-Button im StorageMenu (geklont vom Vanilla-Close-Button, inheriting Styling) sortiert den geoeffneten Container.
- "Sort Inventory"-Button im GameplayMenu-Character-Screen (Inventar offen) sortiert das Player-Inventory.
- Sort-Algorithmus: Merge nach ID+Qualitaet+Packaging bis StackLimit (respektiert StackLimitMod), Ordnung Kategorie → Qualitaet (absteigend) → Name; atomarer Plan-then-Commit (kein Itemverlust bei Kapazitaetsmangel).
- Neue Klassen: `BackpackInventorySorter` (Sort-Engine, Snapshot-Semantik), `SortUIInjector` (beide Buttons, idempotent, GameplayMenu-Sync via OnOpen/OnClose/SetScreen-Postfixes).
- StorageMenu.Open/Close + GameplayMenu.OnOpen/OnClose/SetScreen Harmony-Patches via PatchGuard; Scene-Unload-Reset fuer Button-Caches.
- Fix-3.1-Pattern auf Sort-Instanzen: Qualitaet + Packaging bleiben beim Umsortieren erhalten (GetDefaultInstance + Quality/SetPackaging Restore).

## 1.0.2 (2026-09-11)
- slot_-1-Guard: kein slot_-1-File mehr im Menue; Save bei unaufgeloestem Slot geskippt.
- ClothingItemUIPatch: Message-Fallback nur noch fuer Il2CppException (kein Swallow fremder Bugs).

## 1.0.1 (2026-09-10)
- Bone-Cache plus 2s-Throttle (Fix Per-Frame-Scan und Log-Spam).
- BackpackSlot-Reset bei Scene-Unload mit Pointer-Validierung; Material-Cache; Index-Loop ueber ItemDictionary; Empty-JSON-Guard.

## 1.0.0 (2026-08-24)
- First public release: 3D Wearable Backpacks with Spine Rig Alignment on the player avatar.
- ⌨ **B-Hotkey** toggles backpack storage from anywhere in the game.
- Realistic Harness system with chest sternum cross-strap, shoulder straps, and metal buckles via `BackpackVisualManager.cs`.
- Tier system: backpack_t1 / backpack_t2 / backpack_t3 with per-tier color palettes.
- `ObjLoader` runtime: zero-dependency Wavefront OBJ loader for custom `.obj` files under `UserData/BackpackMod/models/`.
- `backpacks.bundle` AssetBundle pipeline for prefab-based backpack meshes.
- Tier-based storage scaling (T1 small, T2 hiking, T3 tactical) via `BackpackStorageManager`.
- 360° mannequin inspection rotation in the Character menu (RMB-drag, Q/E keys).
- ClothingSlot 10 binding: backpack renders only when actively equipped.

## 0.1.0 (2026-08-21)
- Initiale Version — BackpackMod-Grundgerüst, Bundle-Loader, F8-Debug-Spawn.
