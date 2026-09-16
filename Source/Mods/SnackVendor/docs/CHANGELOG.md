# Changelog


## 0.0.4 (2026-09-16) — Street-Placement-Integration
- **`SetupPlacedStation`:** Neuer Einstiegspunkt in `SnackVendorItemFactory` für Platzierungs-Pfade, die das BuiltItem-Prefab direkt instanziieren und am Vanilla `BuildableItem.Start` vorbeilaufen (HomelessMod Street-Placement; gleicher Vertrag wie AutoPacks `SetupPlacedStation`). Hängt den Controller an bzw. nutzt einen bestehenden wieder.
- **Placement-Split im Controller:** `SetupAfterPlacement` (Vanilla-Pfad) und `SetupAfterPlacementExternal` (Street-Pfad) laufen beide in `RunPlacementSetup` mit `_placementDone`-Guard — Doppel-Setup (Start-Postfix + externe Übergabe) ist ausgeschlossen.
- **Restore-GUID:** Bei der Street-Place-Übergabe wandert die persistierte Street-Item-GUID in den Controller, damit das Sidecar (`snacks_slot_{n}.json`) über Save-Restores hinweg zum selben Slot-Eintrag resolvet.
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