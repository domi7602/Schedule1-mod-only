# TownHall Business: Plan für Kauf und Besitzübertragung (Phase 2)

Stand: 2026-11-09. Status: Spezifikation und technischer Entwurf für Phase 2 (Kauf bei Ray).

---

## 1. Ziel und Rahmenbedingungen

Gemäß dem [Fertigstellungsplan](business-completion-plan.md) ist **Phase 2** der Kaufabschluss des Rathauses als Business:
- Der Spieler kann das Rathaus für einen festgelegten Kaufpreis (Standard: **150.000 $**, konfigurierbar) erwerben.
- Der Kauf erfolgt über Ray im Maklerbüro (Real Estate Office) bzw. über dessen Whiteboard/Dialog.
- Nach dem Kauf wechselt der Besitzstatus von `Unowned` auf `Owned`, das Business wird in den nativen Listen verschoben (`UnownedBusinesses` ➔ `OwnedBusinesses`) und die Verwaltung im Obergeschoss (Geldwäsche) wird freigeschaltet.
- **Transaktionsgarantie:** Genau eine bestätigte Zahlung bewirkt genau einen Besitzwechsel. Wiederholungen, Mehrfachklicks oder Fehler dürfen zu keinem doppelten Geldabzug führen.

---

## 2. Wirtschaftliche Regeln & Fehlerbehandlung (nach `schedule1-economy`)

Basierend auf den Projektregeln (`Skills/schedule1-economy/SKILL.md` und `atomic-purchase.md`) gelten strikte Vorgaben:

### 2.1 Vorab-Validierung (Pre-Validation)
Bevor auch nur ein einziger Cent bewegt wird:
1. **Kontextprüfung:** Bestätigter Einzelspieler-Weltkontext (Testspielstand `TownHallTest / SaveGame_2`).
2. **Besitzprüfung:** Das Rathaus muss unbesessen sein (`IsOwned == false`). Bereits besessene Immobilien verwerfen den Kauf sofort mit `"already-owned"`.
3. **Preisprüfung:** Der Kaufpreis muss eine endliche, positive Zahl sein (`price > 0`).
4. **Guthabenprüfung:** Das Guthaben (Bank bzw. Bargeld) muss mindestens dem Kaufpreis entsprechen. Reicht das Geld nicht aus, bricht der Vorgang mit `"insufficient-funds"` ab (0 Geldbewegungen).

### 2.2 Atomare Ausführung & Latch
- Während der Kauf läuft, sperrt ein atomarer Latch (`_inProgress`) parallele Aufrufe oder Doppelklicks ab.
- Die Zahlung wird über `IPurchasePaymentPort` ausgeführt:
  - **Online/Bank:** `CreateOnlineTransaction("Town Hall Purchase", -price, 1f, "Real Estate")`
  - **Bargeld:** `ChangeCashBalance(-price, visualizeChange: true, playCashSound: true)`
  - **Auto:** Primär Bank, Fallback auf Bargeld (oder konfigurierte Priorität).

### 2.3 Kompensierende Transaktion (Refund bei Teilversagen)
Wird das Geld erfolgreich abgezogen, scheitert jedoch die anschließende Besitzübertragung (z. B. Speicherlisten-Fehler oder Port-Exception):
- Der Manager führt sofort eine **kompensierende Rückerstattung** in gleicher Höhe auf demselben Zahlungsweg durch (`Refund`).
- Der Vorgang wird als `Refunded` (oder `Faulted`, falls auch der Refund scheitert) protokolliert, damit kein Geld ohne Gegenwert verloren geht.

### 2.4 Idempotenz
- Ein erneuter Aufruf nach erfolgreichem Kauf führt zu keiner weiteren Zahlung und gibt `"already-owned"` zurück.

---

## 3. Architektur & Schnittstellen

### 3.1 Zustände (`PurchaseState`)
- `Unowned`: Bereit zum Kauf.
- `Validating`: Prüfung von Kontext und Preis.
- `InsufficientFunds`: Guthaben zu gering.
- `DeductingFunds`: Zahlung wird verarbeitet.
- `TransferringOwnership`: Besitz wird im Spiel registriert.
- `Completed`: Kauf erfolgreich abgeschlossen, Quittung erstellt.
- `Refunding`: Fehler nach Zahlung, Rückabwicklung läuft.
- `Refunded`: Geld erfolgreich zurückgebucht.
- `Faulted`: Schwerer Fehler (z. B. unklarer Transaktionsausgang).

### 3.2 Schnittstelle: `IPurchasePaymentPort`
```csharp
public interface IPurchasePaymentPort
{
    float GetOnlineBalance();
    float GetCashBalance();
    bool DeductBank(float amount, string label, string note);
    bool DeductCash(float amount);
    bool RefundBank(float amount, string label, string note);
    bool RefundCash(float amount);
}
```

### 3.3 Manager: `BusinessPurchaseManager`
Verwaltet den Ablauf, synchronisiert mit `BusinessWorldRegistrationManager` (um das Business in die `Owned`-Listen zu verschieben) und erzeugt eine unveränderliche `PurchaseReceipt`.

---

## 4. Ray-Integration (Maklerbüro)
- Im Real Estate Office von Ray (`RE Office`) existiert das Whiteboard mit den Angeboten.
- Nach dem Kauf wird die Anzeige am Whiteboard aktualisiert (Listing entfernt oder als "Sold" markiert).
- Das Verkaufs-Schild (`ForSaleSign`) vor dem Rathaus wird entfernt oder ausgeblendet.

---

## 5. Offline-Testplan
Folgende Testfälle sichern Phase 2 offline ab:
1. **Erfolgreicher Bankkauf:** Guthaben vorhanden, Bankabbuchung erfolgreich, Besitz gewechselt, Quittung vorhanden.
2. **Erfolgreicher Barkauf:** Ausreichend Bargeld, Barabzug erfolgreich, Besitz gewechselt.
3. **Zu wenig Guthaben:** Reicht weder Bank noch Bar, bricht der Kauf ohne Abbuchung ab.
4. **Bereits besessen:** Zweiter Kaufaufruf scheitert sofort ohne Abbuchung.
5. **Doppelklick-Schutz (Reentranz):** Parallele Kaufversuche werden durch den Latch abgewiesen.
6. **Kompensierender Refund:** Gelingt die Zahlung, scheitert aber der Besitzwechsel, wird das Geld exakt zurückerstattet.
7. **Slot-Schutz:** In Dominiks Slot 1 wird der Kaufversuch sofort durch das Gate blockiert.
