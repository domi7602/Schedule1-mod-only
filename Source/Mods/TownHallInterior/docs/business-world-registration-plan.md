# TownHall Business: Plan zur Weltregistrierung und nativen Initialisierung (Phase 1)

Stand: 2026-11-09. Status: Spezifikation und technischer Entwurf für die Weltregistrierung.

---

## 1. Kontext und Ausgangslage

Am **6. November 2026** wurden im Rahmen von Phase 1 vier diagnostische Live-Proben auf dem Testspielstand `TownHallTest / SaveGame_2` erfolgreich abgeschlossen:
1. **ISaveable-Speicherprobe (`38bd7da3…`):** Synchroner No-IO-Save-Pfad mit Completion und atomarem Rückbau verifiziert.
2. **Aktive Untertyp-Probe (`c07ada69…`):** Eigener no-base IL2CPP-Untertyp mit vollständigen Unity-Lifecycle-Callbacks beobachtet.
3. **NetworkObject-Bindungsprobe (`beb088f2…`):** Gegenseitige Bindung von Business- und NetworkObject-Komponenten samt deterministischem Abbau bestätigt.
4. **Kapazitätsgebundene Datenprobe (`b449c1c1…`):** Listener-Snapshot (`size=889`, `capacity=1024`), Datenaufbau, `SetProperty`-Rückverweise und native Abfragen (`CurrentLaunderTotal=0`, `AppliedLaunderLimit=0`, `Inside=true`, `Outside=false`) verifiziert.

Nach jedem Live-Lauf wurde der Normalbuild (`614b81fd…`) wiederhergestellt. Alle **1.166 Unit-Tests** in `TownHallInterior.Tests` sind grün.

**Offene Lücke:** Die bisherigen Proben liefen isoliert in einer temporären Diagnoseschale und bauten sich nach wenigen Frames selbst wieder ab. Das Rathaus existiert im laufenden Spiel noch nicht als dauerhaft registriertes, natives Business-Objekt in den globalen Spiel-Listen (`Business.Businesses`, `Property.Properties`) und im `SaveManager`.

---

## 2. Technische Anforderungen an die Weltregistrierung

Aus dem nativen Maschinencode-Audit (`native-init-findings.json`) gehen folgende Kernanforderungen hervor:

### 2.1 Vermeidung von "Blind Native Awake"
Der originale native Konstruktor `Property.Awake_UserLogic` hängt das Objekt sofort in `Properties` und `UnownedProperties`, dereferenziert danach jedoch hart die untergeordneten GameObjects (`BoundingBox`, `POI`, `ForSaleSign` mit `Name`- und `Price`-TextMeshPro-Komponenten) sowie `EmployeeIdlePoints`. Fehlt eines dieser Elemente, stürzt das Spiel mit einer NullReferenceException ab und hinterlässt korrupten Teilzustand.

**Lösungsansatz:**
- Die Instanzierung erfolgt entweder über ein vollständig vorstrukturiertes Prefab-Hierarchy-Template mit allen benötigten Child-Komponenten ODER
- über den bereits bewährten expliziten Komponentenaufbau unseres Subtyps, der alle Pflichtfelder vor der Publikation initialisiert.

### 2.2 Hierarchie und Namensräume
- **Welt-Hierarchie:** Gemäß nativer Konvention unter `@Businesses/HylandTownHall`.
- **PropertyName:** `"Hyland Town Hall"`
- **PropertyCode:** `"hyland_town_hall"`
- **Zugeordnete Bounds:** BoxCollider auf Trigger-Layer (`Invisible`), der den Innenraum des Rathauses umschließt.

### 2.3 Registrierung in nativen Spielsystemen
Sobald die Szene geladen und die Welt initialisiert ist, werden folgende Referenzen sauber gesetzt:
1. `Business.Businesses.Add(instance)`
2. `Business.UnownedBusinesses.Add(instance)` (sofern `IsOwned == false`) bzw. `Business.OwnedBusinesses.Add(instance)` (wenn gekauft)
3. `Property.Properties.Add(instance)`
4. `Property.UnownedProperties.Add(instance)` bzw. `Property.OwnedProperties.Add(instance)`
5. `SaveManager.RegisterSaveable(instance)` (für die Speicherung)

### 2.4 Weltwechsel- und Abbau-Vertrag (World Exit & Slot Switching)
Beim Entladen der Welt (Menü-Rückkehr, Slot-Wechsel oder Spielende) muss das Objekt:
- Seine Registrierungen aus allen statischen nativen Listen (`Business.Businesses`, `Property.Properties`, `SaveManager.Saveables`) rückstandslos austragen.
- Sicherstellen, dass keine verwaisten Zeiger oder Leichen im Folge-Slot (insbesondere Slot 1) verbleiben.
- Eventuelle TimeManager- und MoneyManager-Abonnements sauber abmelden.

---

## 3. Schrittweiser Umsetzungsplan (Phase 1 Abschluss)

### Schritt 1: Offline-Architektur & World-Registration-Port (C#)
- Definition von `IBusinessWorldPort` zur Entkopplung der nativen Welt- und Listenoperationen von der Modlogik.
- Implementierung der Zustandsmaschine für `WorldRegistrationState`:
  - `Unregistered` -> `Prepared` -> `Registered` -> `Deregistered`.
- Vorbereitung der Unit-Tests zur Verifikation aller Registrierungs- und Deregistrierungs-Pfade (Normalablauf, doppelter Aufruf, unerwarteter Abbruch).

### Schritt 2: Integration in den Scene/World-Load-Lifecycle
- Erkennung des korrekten Ladezeitpunkts (wenn die Stadt-Szene geladen ist und Spiel-Manager bereitstehen).
- Absicherung gegen Ausführung im falschen Spielstand (Gate auf Testslot `TownHallTest / SaveGame_2`).

### Schritt 3: Speicherung & Rekonstruktion (Save / Reload)
- Erprobung der `ISaveable`-Routinen im registrierten Zustand: Werden Daten beim echten Speichern ins SaveGame geschrieben?
- Rekonstruktion beim Laden: Übernahme des Besitzstatus aus dem Spielstand.

---

## 4. Sicherheitsregeln

1. **Strikter Schutz von Slot 1 (`Dominik / SaveGame_1`):** Keine Testroutinen laufen außerhalb von Slot 2.
2. **Fail-Closed bei Fehlern:** Tritt beim Aufbau ein Fehler auf, wird keine unvollständige Registrierung in globalen Listen hinterlassen.
3. **Kein automatisches Deployment / Spielstart:** Änderungen werden offline gegen Tests gebaut (`-p:S1NoDeploy=true`); Live-Tests erfolgen nur nach Freigabe durch Dominik.
