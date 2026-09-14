# SnackVendor — Spec v0.1 (Entwurf, 2026-09-14)

> Eigene Vending-Maschine wie die Cuke-Maschine, aber der Spieler füllt sie selbst
> mit Zutaten aus dem Gas-Markt. NPCs kaufen daraus; der Verkaufspreis ist der
> Vanilla-Marktpreis der Zutat und wird dem Spieler als Cash gutgeschrieben.

## Nutzer-Entscheidungen (2026-09-14, verbindlich)

| Frage | Entscheidung |
|---|---|
| Platzierung | **Platzierbar** wie AutoPackagingStation (Buildable-Item, überall) |
| Konsum | **Kein Spieler-Konsum.** Nur NPCs kaufen; Item wandert ins NPC-Inventar |
| Preis | **Vanilla-Gas-Markt-Preis** der Zutat (wie ein Snack-Automat) |
| Nutzer | NPCs kaufen → **passives Einkommen** für den Spieler |
| Eigene Entnahme | **Kostenlos** (eigner Stock) |

## Asset-Pipeline: 3D-Modell (2026-09-14, gebaut)

- **Blender headless:** `"C:\Program Files\Blender Foundation\Blender 5.2\blender.exe"
  --background --python assets/build_snackvendor_model.py` erzeugt
  `assets/SnackVendor_model.glb` (+ Preview-Render).
- **Modell:** Low-Poly-Vending-Maschine im Cuke-Stil, 0.95×1.85×0.72 m:
  roter Korpus, Glasfront mit 4 Regalböden + bunten Snack-Boxen/Dosen
  (deterministischer Seed), Chrom-Türrahmen, Bedienpanel (Display, 3×3-Keypad,
  Münzeinwurf), Ausgabefach, Sockel/Füße, gelbe Logo-Streifen.
- **Referenz-Empties im GLB** (für Weg-A-Integration): `ItemSpawnPoint`,
  `AccessPoint`, `CashSpawnPoint` — die Transforms, die die Vanilla
  `VendingMachine`-Komponente erwartet.
- **Runtime-Load (geplant, AutoPack-Muster):** `Mods/SnackVendor/model.glb` via
  `S1MAPI.Gltf.GltfLoader.LoadGlb(bytes)` → Collider strippen → URP-Lit-Shader
  auf alle Renderer (sonst pink) → unter Visual-Root parenten. GLB liegt schon
  am Zielort; C#-Loader folgt mit Feature-Schritt 2.
- **Pitfall:** `bpy.path.abspath("//...")` löst im `--background`-Modus ohne
  .blend-Datei nach `C:\` auf — Ausgabepfade absolut angeben oder Datei nach
  dem Run zurückverschieben.

## Verifizierte Vanilla-Bausteine (game v0.4.6f13, decompiled proxies)

- `Il2CppScheduleOne.ObjectScripts.VendingMachine` — Referenz für Kauf-Flow
  (`Interacted → SendPurchase` ServerRpc → `PurchaseRoutine` ObserversRpc →
  `DropItem`), statische `AllMachines`-Liste, GUID-Persistenz (`GetSaveData`/`Load`),
  Break/Repair, `COST` static. **Hat KEIN Stock-System** — Cuke-Stock ist unendlich.
- `NPCSignal_UseVendingMachine` (`Il2CppScheduleOne.NPCs.Schedules`) — Vanilla-NPC-KI
  kauft an Maschinen: `GetTargetMachine()`, `MachineOverride`, `TargetMachine`,
  `WalkCallback`. Findet Maschinen über `VendingMachine.AllMachines`.
- `NPC.Inventory` (`NPCInventory`, `Il2CppScheduleOne.NPCs.Framework.Inventory`) —
  NPC-Inventar existiert; gekaufte Items können hinein wandern.
- `AdditiveDefinition` (Zutaten): trägt nur Mixing-Stats (`QualityChange`,
  `YieldMultiplier`, `InstantGrowth`) + `BasePurchasePrice` via
  `StorableItemDefinition`. **Keine Konsum-Effekte in Vanilla** — für Spieler-
  Konsum wäre Neubau nötig (bewusst NICHT Scope).
- S1API: `S1API.Entities.NPC`, `S1API.Money`, `BuildableItemDefinitionBuilder`
  (AutoPack nutzt letzteren für platzierbare Station).

## Architektur (Schichten wie AutoPackagingStation)

```
Mod.cs                     Melon-Einstieg, Config-Init, Harmony-Patches
SnackItemFilter.cs         HardFilter: nur EItemCategory.Ingredient in Stock-Slots
SnackVendorController.cs   MonoBehaviour auf dem platzierbaren GO:
                           native ItemSlots (Stock), GUID, Interaction-Hooks
SnackVendorStore.cs        Sidecar-Persistenz snacks_slot_{n}.json (slot_-1-Guard,
                           .bak-Schutz) — Pattern: AutoPackStore/BusinessIncome
SnackPurchaseEngine.cs     NPC-Kauf: Signal abgreifen/Attraktion, Preis =
                           BasePurchasePrice, Item → NPC.Inventory, Cash → Spieler
SnackVendorFactory.cs      BuildableItemDefinitionBuilder + Mesh (AutoPack-Clone)
```

### NPC-Anziehung — Entscheidung: Cuke-Klon-Architektur (2026-09-14, ersetzt Weg A/B)

Vanilla-Scheduling-Analyse (decompiled, verifiziert):
- Jeder NPC hat `NPCScheduleManager` (via `ScheduleBehaviour`): actions-Liste
  (datengetrieben aus NPC-Prefab-Assets!), `OnMinPass`-Takt, `ShouldStart()` pro
  Action, `priority`/`StartTime`, Curfew-Listen (`EnabledDuringCurfew`/`NoCurfew`).
- `NPCSignal_UseVendingMachine` wählt Ziel LAUFZEIT: `MachineOverride`-Feld +
  `GetTargetMachine()`; NPCs finden Maschinen über statische
  `VendingMachine.AllMachines`.
- `NPCManager.NPCRegistry` = statische `List<NPC>` aller NPCs (Fallback, falls
  der Klon-Pfad trotzdem scheitert: Radius-Scan, BusinessIncome-Pattern).

**Verworfen: `AddComponent<VendingMachine>` auf platziertes GridItem.** FishNet
sammelt NetworkBehaviours beim Spawn — nachträglich hinzugefügte Komponenten
bekommen keine RPC-Registrierung; `SendPurchase` (ServerRpc) würde still
fehlschlagen. Kein Spike-Wert: architektonisch nahe Sackgasse.

**Cuke-Klon-Architektur (neu, verbindlich):**
1. **GridItem 1×1 bleibt der Anker** (Baumenü, Placement, Overlap-Prevention,
   Vanilla-Positions-Persistenz) — AutoPack-Pattern (`CloneFrom(pot|storage)`
   statt `packagingstation`, falls Footprint-Korrektur nötig:
   `CoordinateFootprintTilePairs`/`OriginFootprint` auf 1×1 setzen).
2. Beim Platzieren (`BuildableItem.Start`-Postfix): **echte Cuke-Maschine aus
   der Szene klonen** (`Instantiate` einer Vanilla-Instanz aus
   `VendingMachine.AllMachines` als Vorlage) → FishNet-Objekt mit komplett
   verdrahteten RPCs, `InteractableObject`, Collider, Lights, Sounds,
   `PhysicsDamageable` — `Awake()` des Klons registriert sich selbst in
   `AllMachines`.
3. **Mesh-Tausch:** Originale Renderer am Klon deaktivieren, unser GLB
   (`Mods/SnackVendor/model.glb`, mit `ItemSpawnPoint`/`AccessPoint`/
   `CashSpawnPoint`-Empties) drunter parenten, Komponenten-Referenzen
   (`ItemSpawnPoint`, `AccessPoint`, `CashSpawnPoint`, ggf. `DoorMesh`) auf
   unsere Empties umlenken. GLB-Load: S1MAPI.Gltf + URP-Shader-Fix.
4. Klon an Grid-Position ausrichten; Visual des GridItems bleibt unsichtbar
   (AutoPack `HideBaseRenderers`-Pattern).
5. Host spawnt den Klon über FishNet (Runtime-Spawn bewährt — vanilla
   `DropItem` spawnt bei jedem Kauf NetworkObject-Pickups).
6. Harmony-Patch auf `SendPurchase`/`PurchaseRoutine`/`DropItem`: nur bei
   Instanzen mit unserem Marker (`SnackVendorMarker`-Komponente) → Kauf gegen
   unseren Stock (Zutat aus Slot → `NPC.Inventory`, Cash =
   `BasePurchasePrice` an Spieler). Vanilla-Maschinen unberührt.
7. Save/Load: Position = GridItem (vanilla). Stock-Sidecar (`snacks_slot_{n}`
   .json) hängt an der Klon-GUID; beim Laden Klon neu instanziieren +
   verlinken (GUID-Stabilität über `BakedGUID` des Klons sichern).

**Spike-Gate (S1MCP) bleibt, aber enger gefasst:** (a) Klon einer Szene-
VendingMachine zur Runtime erzeugt eine funktionierende, kaufbare Maschine?
(b) Wie verhält sich `OnDestroy` des Klons (AllMachines-Deregistrierung) beim
Zerstören/Abräumen des GridItems? (c) BakedGUID-Stabilität über Save/Load.

### Money-Flow-Regeln (aus game-mod-persistence Skill)

- Verkauf buchen: erst Stock-Slot dekrementieren + snapshot, dann Cash gutschreiben;
  Persistenz-Sidecar dirty-flaggen; beim Laden Marker-Check (crash window).
- Preis-Quelle: `StorableItemDefinition.BasePurchasePrice` (vanilla Gas-Markt),
  KEIN eigener Preis — Nutzer-Entscheidung.
- Multiplayer: Host-Guards auf jedem mutierenden Pfad (Deposit/Extract/Kauf/
  Save-Flush), Pattern aus schedule1-il2cpp-sorting-patterns Skill.

## Nicht-Scope (explizit)

- Spieler-Konsum von Zutaten (kein Vanilla-Pfad, Nutzer hat abgelehnt)
- Feste Spawn-Punkte in der Stadt
- Co-op-Kauf durch andere Spieler (nur Host-Welt relevant)

## Offene Punkte / Build-Reihenfolge

1. [x] Scaffold baut grün (Mod.cs-Gerüst, csproj, mod.json) ← erledigt 2026-09-14
2. [x] 3D-Modell (Blender headless → GLB + Ref-Empties, liegt in Mods/SnackVendor/) ← erledigt
3. [ ] GridItem 1×1 registrieren + platzierbar (AutoPack-Klon: CloneFrom, Ghost, Icon)
4. [ ] Cuke-Klon-Spawn beim Platzieren: BuildableItem.Start-Postfix → VendingMachine-
       Klon + Mesh-Tausch + Ref-Umlenkung + Marker — **S1MCP-Spike (a)/(b)/(c)**
5. [ ] Stock-Slots (HardFilter Ingredient) + Deposit/Extract-UI + Sidecar-Persistenz
6. [ ] Kauf-Interception (Harmony auf SendPurchase) + Cash-Auszahlung (Money-Flow-
       Marker) + NPC.Inventory-Transfer
7. [ ] UI-Politur, Multiplayer-Host-Guards, Version-Bump (4 Stellen + AGENTS.md-Freigabe)
