# TownHall Business: Plan für Persistenz und Rekonstruktion (Option B)

## Implementierter Stand vom 9. November 2026

Die Business-Erweiterung ist auf den lokalen Einzelspielerstand
`TownHallTest / SaveGame_2` und die geprüfte native Spielassembly von
`0.4.7f12` begrenzt. Nach dem Ladeereignis wartet sie auf den fertigen Host
und das geladene Inventar. Besitz und Geldwäscheaufträge liegen in der
Zusatzdatei der Mod. Der laufende Zustand wird bei Kauf, Einzahlung und
Auszahlung zunächst im Arbeitsspeicher geändert.

Beim bestätigten Start von `Save(string)` friert die Mod den Rathauszustand
samt Bankguthaben, Bargeld und Spielzeit ein. Sie legt eine vorbereitete
Datei im eigenen UserData-Ordner an. Nach Save-Abschluss prüft sie Ziel,
Fehlerflag und leere Warteschlange sowie die Geld- und Zeitwerte aus den
geschriebenen Dateien. Erst dann veröffentlicht sie den Rathausstand unter
dem Fingerabdruck der nativen Dateien. Der Fingerabdruck umfasst
`Game.json`, `Metadata.json`, `Money.json`, `Players/Player_0/Inventory.json`
und `Time.json`. Der vollständige Slotpfad trennt die Archive.
Beim Laden wählt der Fingerabdruck genau den dazugehörigen Rathausstand.

Der leere Ausgangsstand wird vor der ersten Geldbuchung an den bestehenden
Spielstand gebunden. Ein abgebrochener Save behält den bisherigen Checkpoint
und die vorbereitete Datei. Bei einem unbekannten Fingerabdruck, beschädigten
Daten oder unklarer Abbuchung sperrt die Mod die betroffenen Transaktionen.
Der native Save bleibt unter Kontrolle des Spiels. Ein unterbrochener
Mehrdateien-Save kann eine manuelle Zuordnung oder Wiederherstellung benötigen.

### Vorhandene Rathausdaten übernehmen

Das alte Schema 1 speicherte Kauf und Einzahlungen unabhängig vom Spielsave.
Bei einem bereits bezahlten Altstand müssen Besitz, Bankabbuchung und offene
Bargeldeinzahlungen zum gewählten nativen Spielstand passen. Die beiden
Hashes in der Mod-Logmeldung identifizieren die Dateien für diesen Abgleich.
Sie geben allein keine Auskunft über die Buchungen.

Erst nach bestätigter Zuordnung kann neben der alten
`UserData/TownHallInterior/Saves/TownHallTest_SaveGame_2/townhall_business.json`
eine `legacy-adoption.json` mit den beiden ermittelten SHA-256-Werten abgelegt
werden.

```json
{
  "NativeFingerprint": "<SHA-256-Fingerabdruck des bestätigten Spielstands>",
  "LegacySha256": "<SHA-256 der bestätigten alten Rathausdatei>"
}
```

Beim nächsten Laden erstellt die Mod den zugeordneten Checkpoint im Schema 2.
Sie rechnet verbleibende Zeit anteilig in Spielminuten um und hält fällige
Altaufträge mit möglicher früherer Auszahlung zur Klärung fest. Alte
Haupt- und Sicherungsdatei bleiben als Belege erhalten.

## Historischer Entwurf

Die folgenden Abschnitte beschreiben den früheren Entwurf vom 9. November
und den damaligen Stand vor der Bindung an native Checkpoints.

Stand: 2026-11-09. Status: Spezifikation für Speichern und Laden des TownHall-Business-Zustands.

---

## 1. Ziel und Anforderungen

Für einen lückenlosen Spielverlauf muss der Status des Rathauses (Besitz, Kaufdaten, Geldwäsche-Statistiken) über Spielsitzungen, Neuladen und Spielneustarts hinweg zuverlässig erhalten bleiben:
- Nach dem Kauf durch den Spieler darf das Rathaus beim nächsten Spielstart nicht wieder als "Unowned" erscheinen.
- Daten müssen **slot-isoliert** gespeichert werden, um Verwechslungen oder Datenübertrag zwischen Spielständen (insbesondere Dominiks Slot 1 und Testslot 2) technisch auszuschließen.
- Speichervorgänge müssen **atomar und ausfallsicher** sein (Schutz vor Dateikorruption bei Spielabstürzen).
- Ein Ladefehler darf **niemals** als erfolgreicher leerer Spielstand interpretiert werden oder bestehende Daten überschreiben.

---

## 2. Speicher-Architektur (Zwei-Säulen-Modell)

Basierend auf den Projektregeln (`Skills/schedule1-persistence/SKILL.md`) nutzen wir zwei aufeinander abgestimmte Persistenzwege:

### 2.1 Säule 1: Native Engine-Persistenz (`ISaveable`)
- Die TownHall-Business-Instanz ist über `NativeWorldPort` beim nativen `SaveManager` als `ISaveable` registriert.
- Bei regulärem `SaveManager.Save(folder)` serialisiert die Engine den Business-Zustand direkt in den Spielstand-Ordner (`SaveGame_2/`).
- Beim Laden wird die Instanz über die native Deserialisierung in `Properties` und `Businesses` wiederhergestellt.

### 2.2 Säule 2: Mod-Eigene Sidecar-Persistenz (`SafeStorage`)
- Als robuster, versionsunabhängiger Backup- und Metadaten-Speicher führt der Mod eine Sidecar-Datei:
  `UserData/TownHallInterior/Saves/<Organisation>_<Slot>/townhall_business.json`
- **Schutzmechanismen:**
  - Atomares Schreiben über temporäre Dateien (`.tmp` ➔ Atomic Replace).
  - Automatisches Backup (`.bak`) vor Überschreiben.
  - Strenger Slot-Filter: Wird an den bestätigten Ladekontext gebunden. Ein Schreib- oder Ladeversuch in Slot 1 wird strikt abgewiesen.

---

## 3. Datenmodell (`TownHallBusinessData`)

```json
{
  "schemaVersion": 1,
  "isOwned": true,
  "purchasePrice": 150000.0,
  "purchaseTimestampUtc": "2026-11-09T06:00:00Z",
  "paymentMode": "Bank",
  "totalLaundered": 0.0,
  "completedLaunderingOperations": 0
}
```

---

## 4. Lebenszyklus & Synchronisation

1. **Beim Laden der Welt (`OnSceneWasLoaded` bei `"Main"`):**
   - Der Manager prüft über `ConstructionGate.Check` den Ladekontext.
   - Die gespeicherten Daten werden über `TownHallPersistenceManager.Load` eingelesen.
   - Ist das Rathaus laut Speicherstand besessen (`isOwned == true`), wird `BusinessPurchaseManager.RestoreOwnership(true)` aufgerufen und das Business in den `Owned`-Listen angemeldet.
2. **Nach dem Kauf (Phase 2):**
   - Bei erfolgreichem Kaufabschluss wird der aktualisierte Zustand (`isOwned = true`, Quittungsdaten) sofort im Sidecar persistiert.
3. **Beim regulären Speichern:**
   - Vor jedem Speichern friert der Manager einen unveränderlichen Daten-Snapshot ein und schreibt ihn atomar weg.
4. **Beim Verlassen der Welt (`OnSceneWasUnloaded`):**
   - Der flüchtige Speicherzustand wird deregistriert; die persistenten Dateien bleiben auf der Festplatte unverändert geschützt.

---

## 5. Offline-Testplan
1. **Serialisierungs-Roundtrip:** Save ➔ Load reproduziert exakt alle Felder (Besitz, Preis, Zeitstempel, Summen).
2. **Slot-Schutz (Dominik Slot 1):** Lade- und Speichervorgänge werden für Slot 1 sofort fail-closed abgewiesen.
3. **Ausfallsicherheit bei korrupter Primärdatei:** Ist `townhall_business.json` beschädigt, wird automatisch die gültige `.bak`-Datei geladen.
4. **Schutz vor Datenverlust:** Wenn weder Primär- noch Backupdatei lesbar sind, wird ein Fehler gemeldet und die Datei nicht mit leeren Daten überschrieben.
5. **Standardzustand bei neuer Welt:** Fehlt die Datei vollständig, wird ein gültiger Unowned-Ausgangszustand bereitgestellt.
