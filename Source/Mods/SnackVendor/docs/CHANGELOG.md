# Changelog

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