# Changelog

## 0.0.9 (2026-09-17)
- **Station BoxCollider & [E]-Interaktion gefixt:**
  - **Physischer Station-BoxCollider:** `SnackVendorController.EnsureStationCollider()` konfiguriert einen `BoxCollider` (`0.95m × 1.85m × 0.72m`, Center `Y = 0.925m`) direkt auf dem Station-GameObject (`gameObject`). Behebt das Problem, dass die Station nach dem Entfernen der GLB-Collider nur den 10cm flachen Boden-Collider des geklonten `dryingrack` besaß und Spieler-Raycasts auf Augenhöhe durch den Automaten hindurch in die Wand gingen.
  - **Native [E]-Interaktion & Blickfeld-Erkennung:** `SnackVendorController.Update()` prüft bei Blickkontakt im 3m-Radius (Raycast + Blickwinkel-Fallback) auf die Interaktionstaste `[E]` und toggelt das `SnackVendorPanel`.
  - **HUD-Interaktions-Prompt:** Bei Blickkontakt erscheint zentriert am unteren Bildschirmrand `<color=#f39c12>[E]</color>  <b>Snack-Automat</b>` (perfekt oberhalb des RMB-Abbau-Balkens gestapelt).
  - **Panel [E]- & [ESC]-Schließen:** Das Panel schließt sich nun wahlweise über die `[ESC]`-Taste oder erneutes Drücken von `[E]`.
  - **Zentraler Abbau & Rest-Snack-Refund:** `RefundStockToPlayer()` und `RefundStockAndDismantle()` legen verbliebene Zutaten sowie die Station sicher ins Spieler-Inventar zurück, unregistrieren den Automaten aus allen Managern und säubern den Sidecar-Eintrag.



## 0.0.8 (2026-09-17)
- **Standalone Outdoor-Placement (unabhängig von HomelessMod):**
  - **Strikt isoliert auf SnackVendor:** Die Outdoor-Platzierung außerhalb gekaufter Immobilien ist hart auf `snackvendor` gegatet. Andere Möbel/Items bleiben im Vanilla-Bausystem unverändert auf Grundstücke beschränkt.
  - **Eigenständiges Bau-Subsystem (`SnackVendor.World.Outdoor`):**
    - `SnackVendorBuildPatches`: Interzeptiert `BuildUpdate_Grid.CheckIntersections` (Postfix) & `Place` (Prefix).
    - `SnackVendorGroundAssistant`: 0-Allocation Raycast- und Bodenerkennung (Hangneigung ≤ 45°, Reichweite 0,4–6,0 m, Multi-Corner Terrain-Höhe, `OverlapBoxNonAlloc` Hindernisprüfung).
    - `SnackVendorOutdoorManager`: Eigener Virtual World Root (`SnackVendor_OutdoorRoot`, `DontDestroyOnLoad`) und persistente Save-Slot-Dateien `UserData/SnackVendor/outdoor_stations_slot_{n}.json` via `SafeStorage.SaveAtomic` (.bak-gesichert).
    - `SnackVendorGuardPatches`: Schützt platzierte Straßen-Automaten vor `BuildableItem.Start`-Property-Abstürzen, Innenraum-Culling (`SetCulled`) und GridManager-Auto-Destruction (`GridItem.Destroy`).
  - **Pack-Up (Dismantle):**
    - `SnackVendorOutdoorInteractable`: 0,4 s RMB gedrückt halten mit visuellem HUD-Fortschrittsbalken packt den Automaten ein.
    - `SnackVendorPanel`: Zusätzlicher `[ 📦 Pack Up Station ]` Button im UI-Panel für bequemen Abbau per Klick.
    - **Automatischer Stock-Refund:** Verbleibende Snacks im Automaten werden beim Abbau zusammen mit dem Automaten (1x `snackvendor`) automatisch ins Spieler-Inventar zurückgelegt.
  - **Nahtlose Koexistenz mit HomelessMod:** `HomelessModInterop` prüft zur Laufzeit, ob HomelessMod aktiv ist; es entstehen keine Doppel-Platzierungen oder Datei-Konflikte.


## 0.0.7 (2026-09-17)
- **Blueprint-Ghost Y-Offset & GLB-Vorschau gefixt:** Im Blueprint-/Ghost-Modus versank der Automat um die halbe Höhe (0,925 m) im Boden. Ursache: `PrimitiveType.Cube` zentriert seinen Pivot bei `(0, 0, 0)` (Y reicht von -0,925 m bis +0,925 m), wodurch die untere Hälfte unter die Grid-Ebene geriet.
  - **GLB-Ghost-Vorschau:** `BuildOrLoadGhostPrefab` lädt nun primär das echte GLB-Modell (`SnackVendor_model.glb`) via `S1MAPI.GltfLoader` (Collider entfernt, Basis bei Y = 0) – im Blueprint ist nun exakt das reale Vending-Machine-Modell bündig auf dem Boden sichtbar.
  - **Cube-Fallback korrigiert:** Sollte das GLB fehlen/fehlschlagen, wird der Fallback-Proxy-Cube um `+0.925m` nach oben versetzt, sodass die Unterkante plan auf dem Boden aufliegt.
  - **Frische Instanzen:** Ghost-Factory erzeugt jedes Mal eine frische Instanz, um zerstörte GameObject-Referenzen nach Bauabbruch/Platzierung zu verhindern.


## 0.0.6 (2026-09-16)
- **Repo-Fix (mit entdeckt):** `NPCSignalPatches.cs` und `SnackVendorPanel.cs` waren durch einen `.gitignore`-Bug (`Mods/` ohne Root-Anker matchte auch `Source/Mods/`) nie committed — ein frischer Clone konnte SnackVendor nicht bauen. Beide Dateien sind jetzt im Repo.
- **Interacted_Prefix fail-closed:** Der stille `catch { return true; }` ließ bei internem Fehlern die Vanilla-Zahl-UI auf eigenen Maschinen aufgehen (Spieler könnte eigenen Stock "zurückkaufen"). Jetzt: Warn-Log + Vanilla-UI bleibt für unsere Maschinen blockiert; Vanilla-Maschinen unverändert.
- **Persist-Doppel-Backup entfernt:** `SnackVendorStore.Save` machte eigenes `.bak`-Copy + toten `.tmp`-Cleanup, obwohl `SafeStorage.SaveAtomic` beides intern macht — doppelter Backup-Schreibvorgang pro NPC-Kauf (Hot Path) eliminiert.
- **Panel-Log-Flut gedrosselt:** `SnackVendorPanel.Draw` loggte einen persistierenden Fehler pro Frame; jetzt max. 1 Warn pro Panel-Open (Reset bei Toggle). Doppelte `_open == null`-Checks in `Update`/`Draw` zusammengeführt.
- **KNOWN GAP (MP) dokumentiert:** Deposit/Extract laufen client-lokal (lokaler Stock + lokale Sidecar-Datei), während der NPC-Kauf nur host-seitig dekrementiert → Stock-Desync im Multiplayer möglich. Als bewusste Spike-Lücke in `SnackVendorPanel`/`VendingMachinePatches` dokumentiert; Singleplayer ist nicht betroffen.
- **Unsichtbare Station behoben (GLB-Mesh-Hiding-Fix 2026-09-17):** Die platzierte Station war komplett unsichtbar. Ursachen: 1) `SwapMesh` hängte den GLB-Mesh an `Clone.transform` (den animierten Vanilla-Klon). 2) Der `HideBaseRenderers`-Loop rief `IsChildOf` auf ungemappten IL2CPP-GLTF-Nodes auf, scheiterte und setzte `forceRenderingOff=true` auf allen 59 GLB-Meshes. 3) `sm.shader = sh` überschrieb die von `S1MAPI.GltfLoader` bereits korrekt initialisierten URP-Lit-PBR-Materialien. Fix: GLB wird direkt an `transform` als `SnackVendor_VisualRoot` gehängt, alle Kind-Knoten erhalten das `SnackVendor_`-Präfix, `sm.shader`-Mutation entfernt, Klon-Renderer werden gezielt am Klon abgeschaltet und der Root-Loop schont alle `SnackVendor`-Renderer.
- **RMB-PackUp über HomelessMod (In-Game-Report 2026-09-17):** Station war an der Straße nicht mit RMB einpackbar („won't fit in inventory") — als Custom-Station bekam sie kein `OutdoorItemInteractable`, dadurch feuerte der Vanilla-PackUp des Rack-Präfabs ins Leere. Koordinierter Fix in HomelessMod v0.1.12: `snackvendor` erhält in Place- und Restore-Pfad das generische `OutdoorItemInteractable` (Refund 1x via `GetDefaultInstance` → `AddItemToInventory` + `UnregisterStreetItem` + Destroy); `autopackagingstation` bleibt ausgeschlossen (eigenes F-Key-PackUp).
- **Sidecar-Orphan-Prune:** `PersistAllStationSlots` entfernt beim SaveComplete Sidecar-Einträge ohne lebenden Controller — aus gepackte Stationen hinterlassen keinen wiedererweckbaren Stock-Eintrag mehr (GUID-Wechsel bei Neuplatzierung).



## 0.0.5 (2026-09-16) — Spike-Completion (GLB, NPC-Inventory, Panel)
- **GLB-Mesh via S1MAPI:** `SwapMesh` lädt `S1MAPI.Gltf.GltfLoader.LoadGlb(bytes)` (gleiche Pipeline wie AutoPackagingStation): Kollidatoren entfernt, URP-Shader-Fix via `sharedMaterial` (H12-Muster, keine Material-Klone), Vanilla-Klon-Renderer erst nach erfolgreichem Load deaktiviert. Fallback bei Load-Fehler: Vanilla-Mesh bleibt sichtbar. GLB wird jetzt auto-deployt (`assets/*.glb` → `Mods\SnackVendor\` via `Directory.Build.targets`); Runtime akzeptiert `SnackVendor_model.glb` und Legacy `model.glb`.
- **NPC-Inventory-Credit:** Neuer `NPCSignalPatches.Purchase_Prefix` (PatchGuard-registriert) captured den kaufenden NPC (`GetComponentInParent<NPC>()`, host-gated) in einer 60-s-Pending-Map; `SendPurchase_Prefix` überträgt `GetDefaultInstance(1)` → `NPC.Inventory.InsertItem`. Fehler blockieren nie den Cash-Pfad. Config: `CreditNpcInventory` (default an).
- **Deposit/Extract-Panel:** `VendingMachine.Interacted`-Prefix öffnet auf eigenen Maschinen ein IMGUI-Panel (HomelessMod-Pattern: gecachte GUIStyles, gedämpfte Skalierung, cursor-frei solange offen) statt der Vanilla-Pay-UI. Einlagern (+1/+5/All) mit `GetCopy`-Rollback bei vollem Station-Slot; Herausnehmen mit 1-Unit-Kapazitäts-Probe und Stock-Refund bei Add-Fail. Auto-Close bei Distanz/ESC/Szenenwechsel/zerstörter Station. Config: `PanelRange` (3,5 m).
- **Stock-Identität auf Registry-IDs umgestellt:** Der alte numerische Scan (`GetItem("1".."1023")`) matchte nie echte Item-IDs (IDs sind Strings wie `"cuke"`) — Stock-Slots, Preis-/Namensauflösung und Allowed-Set laufen jetzt über String-IDs; das Allowed-Set wird aus Shop-Listings abgeleitet (Gas-Markt bevorzugt, `AdditiveDefinition`-Filter, 30-s-Cache). Legacy-Sidecar-Zeilen mit numerischen IDs werden beim Restore verworfen (geloggt). `MaxSlots` hängt jetzt an `MaxIngredientSlots` (Config).
- **Nebenbefund gefixt:** `ThirdParty/Archive/**` fehlte in `ThirdParty/.deployignore` — jeder Build deployte das deprecatete Reference-Assembly `Hash.dll` nach `Mods\` (BadImageFormatException-Quelle, Cleanup 2026-09-10). Ignore-Regel ergänzt, DLL aus `Mods\` entfernt.
- Hinweis: In-Game-Verify (Spike-Gate + neue Features) steht noch aus — siehe Verifikationstabelle in der README des Mods.

## 0.0.4 (2026-09-16) — Street-Placement-Integration
- **`SetupPlacedStation`:** Neuer Einstiegspunkt in `SnackVendorItemFactory` für Platzierungs-Pfade, die das BuiltItem-Prefab direkt instanziieren und am Vanilla `BuildableItem.Start` vorbeilaufen (HomelessMod Street-Placement; gleicher Vertrag wie AutoPacks `SetupPlacedStation`). Hängt den Controller an bzw. nutzt einen bestehenden wieder.
- **Placement-Split im Controller:** `SetupAfterPlacement` (Vanilla-Pfad) und `SetupAfterPlacementExternal` (Street-Pfad) laufen beide in `RunPlacementSetup` mit `_placementDone`-Guard — Doppel-Setup (Start-Postfix + externe Übergabe) ist ausgeschlossen.
- **Restore-GUID:** Bei der Street-Place-Übergabe wandert die persistierte Street-Item-GUID in den Controller, damit das Sidecar (`snacks_slot_{n}.json`) über Save-Restores hinweg zum selben Slot-Eintrag resolvet.
- **Klon-Mesh sichtbar lassen:** `SwapMesh` deaktivierte bislang die Klon-Renderer, obwohl die GLB→Mesh-Konvertierung noch ein Stub ist — Ergebnis wäre ein unsichtbarer Automat gewesen. Renderer bleiben an, bis das echte GLB-Routing landet (Follow-up).
- **Diagnose:** Der Start-Postfix loggt jetzt (Debug) bei Nicht-SnackVendor-Items — "Postfix nie gefeuert" vs. "falsches Item" ist im Log unterscheidbar. Kosmetik: hardcoded "0.0.1" im Init-Log-String entfernt.
- Hinweis: In-Game-Verify des Spike-Gates (Klon in `VendingMachine.AllMachines`, NPC-Routing, Cash-Credit) steht noch aus.

## 0.0.3 (2026-09-15) — Audit-Patch (vor 0.1.0)
Sicherheits- und Cleanup-Audit aus dem Repo-Review 2026-09-15. Vor den
offenen 0.1.0-TODOs (GLB, Shop-Listing, UI, NPC-Inventory) eingeschoben,
damit MP-Sicherheit und Memory-Leaks schon vor dem Player-facing-Release
stehen.

- **H1 Host-Authority für `SendPurchase`:** `NetworkGuard.IsHostOrSingleplayer()`
  als Pre-Gate in `VendingMachinePatches.SendPurchase_Prefix` —
  verhindert Cash-Credit-Duplikation auf MP-Clients. Die anderen drei
  Prefixes (`PurchaseRoutine`/`DropItem`/`DropCash`) bleiben reine
  "block vanilla"-Prefixes ohne Geld-Bewegung und brauchen kein Gate.
- **H2 PatchGuard-Migration:** `PatchClassProcessor` durch fünf
  explizite `PatchGuard.TryPatch`-Calls ersetzt (BuildableItem.Start
  Postfix + 4× VendingMachine Prefixes). PatchGuard-Statistik
  (`PatchGuard.Report(Log)`) zeigt jetzt Signature-Drift statt stille
  tote Station.
- **H3 `SnackVendorController.OnDestroy`:** Trackt jetzt die in
  `SwapMesh` allokierten `Material`-Instanzen + das GLB-Mesh-GameObject
  in `_ownedMaterials`/`_glbMeshGo` und zerstört sie explizit beim
  Station-Abbau. Idempotent via `_destroyed`-Guard.
- **M8 `BuildOrLoadGhostPrefab.Dispose()`:** Zerstört Icon-Sprite +
  Texture, Ghost-Material und Ghost-GameObject; wird in
  `Mod.OnDeinitializeMelon` aufgerufen (Hot-Reload-/App-Quit-Sicherheit).


# Changelog - SnackVendor

## 0.0.2-mvp (2026-09-14) — MVP-Spike
Erster integrierter Build (Build OK, In-Game-Verify offen):

- **Wired:** Buildable 1x1 (`snackvendor`) via S1API `BuildableItemDefinitionBuilder`; Placement via `BuildableItem.Start`-Postfix; Vanilla-Cuke-VendingMachine-Klon pro Station; marker-guarded Harmony-Prefixes (`SendPurchase`, `PurchaseRoutine`, `DropItem`, `DropCash`); Sidecar-Persistenz `snacks_slot_{n}.json` (Sentinel-Guard) via `SnackVendorStore`; prozeduraler Proxy-Ghost + 64x80-Icon.
- **Offen (TODO vor 0.1.0):** In-Game-Verifikation des NPC-Kauf-Routings, Cash-Credit an den Spieler (derzeit nur Log), GLB-Mesh via `S1MAPI.GltfLoader`, Hardware-Shop-Listing (`S1API.Internal.Shops.ShopIntegration`), Deposit/Extract-UI-Panel, Ingredient-Credit in `NPC.Inventory` via `NPCSignal_UseVendingMachine`.
- Details + Verifikations-Tabelle: `README.md` im Mod-Ordner; Design-Entscheidungen: `docs/SPEC.md`.