# Changelog

## 0.1.0
- Erstes Gerüst als No-Deploy-Kandidat: blockiert `BlowupRV` und `SetRVDestroyed` im
  Einzelspieler, verschiebt den Originalzettel mit Welterhalt an die RV-Wurzel und
  gleicht den Zustand nach dem Laden im laufenden Investigate/Read-Fenster ab.
- Eigener Harmony-Eigentümer mit expliziter Registrierung, Besitzermetadaten-Prüfung
  und Rollback; keine Harmony-Attribute.
- Tests für Adapterzweige mit minimalen Attrappen. Kein Spielstart, kein Deploy.
- `RV._exploded` wird beim Blockieren gesetzt: `RV.BlowUp()` prüft in der f12
  `GameAssembly.dll` das Bool bei Offset `0x289` und kehrt bei `true` sofort zurück.
  Damit sind auch nicht gehookte Aufrufer neutralisiert. `IsDestroyed` bleibt `false`.
- Offline: 21 Tests bestanden, No-Deploy-Build 0 Warnungen/0 Fehler, Quick 7/7.
- Zweiter Kandidat verengt den Eingriff: keine Quest-Eintragstitel mehr, keine
  Sichtbarkeits- oder `IsDestroyed`-Schreibvorgänge, keine periodische Zustandslogik.
  Scheitert die Notiz-Vorbereitung, bleibt die native Zerstörung aktiv, statt die Quest
  in eine Sackgasse zu führen.
- `_exploded` ist nachweislich nicht persistiert und wird von keiner anderen RV-Methode
  gelesen; nur `BlowUp` prüft und setzt Offset `0x289`.
- **In-Game-Verify 2026-11-12, DLL `5CC97AEE…`:** Dominik las den Originalzettel am
  intakten Wohnmobil. Log 17:38–17:41: geladen, Hooks 2/2, Notiz nach Load an die Wurzel,
  17:41:23 Zerstörung blockiert. Screenshot: Quest „Read the note“, Zettel am Boden,
  Tag 0 / 9:42, 375 Dollar. Save/Reload danach und Folgeauftrag nicht geprüft.
