# TownHall Business: Plan für Geldwäsche und OG-Terminal (Phase 3)

Stand: 2026-11-09. Status: Spezifikation und Entwurf für das Geldwäsche-Terminal im Obergeschoss.

---

## 1. Ziel und Einordnung

Gemäß dem [Fertigstellungsplan](business-completion-plan.md) schaltet der Besitz des Rathauses (Phase 2) die Geschäftsbedienung frei:
- Im Obergeschoss (OG) steht an den Bürotischen (`office-computer-0`) ein interaktiver Verwaltungscomputer.
- Vor dem Kauf ist der Computer gesperrt (`"access-denied:unowned"`).
- Nach dem Kauf kann der Spieler schmutziges Bargeld (`Dirty Cash`) zur Geldwäsche einreichen.
- Das Business nutzt die nativen Geldwäsche-Routinen der Spiel-Engine (`LaunderCapacity`, `currentLaunderTotal`, `appliedLaunderLimit`, `StartLaunderingOperation`).
- Die Kapazität beträgt standardmäßig **20.000 $** (konfigurierbar).
- Nach Ablauf der nativen Zeitspanne wird das gewaschene Geld dem Spielerkonto gutgeschrieben.

---

## 2. Wirtschaftliche & Native Regeln

Basierend auf den Engine-Dekompilaten (`S1API.BusinessWrapper`) und `schedule1-economy`:

### 2.1 Kapazitäts- und Validierungsregeln
1. **Besitzprüfung:** Ist `IsOwned == false`, ist jegliche Geldwäscheaktion blockiert.
2. **Kapazitätsprüfung:** Die Summe aus laufenden Operationen (`CurrentLaunderTotal`) und dem neuen Betrag darf die Maximalkapazität (`LaunderCapacity`) nicht überschreiten:
   $$\text{AvailableCapacity} = \text{LaunderCapacity} - \text{CurrentLaunderTotal}$$
   Liegt der gewünschte Betrag über `AvailableCapacity`, bricht der Auftrag mit `"capacity-exceeded"` ab.
3. **Bargeldprüfung:** Der Spieler muss mindestens den einzureichenden Betrag an Bargeld bei sich tragen (`GetCashBalance() >= amount`). Fehlt Bargeld, bricht der Auftrag mit `"insufficient-cash"` ab.
4. **Endliche positive Beträge:** Beträge $\le 0$ oder ungültige Fließkommazahlen (NaN/Infinity) werden sofort abgewiesen.

### 2.2 Atomare Auftragsannahme
- Der Manager zieht das eingereichte Bargeld ein (`ChangeCashBalance(-amount)`).
- Anschließend ruft er `StartLaunderingOperation(amount, minutesSinceStarted: 0)` auf.
- Scheitert der Start der nativen Operation, wird das eingezogene Bargeld **sofort kompensierend zurückerstattet** (`ChangeCashBalance(+amount)`).

### 2.3 Auszahlung & Abschluss
- Wenn die Operation abgeschlossen ist, wird der Reingewinn (nach Abzug eventueller Gebühren) auf das Bankkonto (`CreateOnlineTransaction`) oder als sauberes Bargeld ausgezahlt.
- In den Statistiken (`TownHallBusinessData`) wird `TotalLaundered += amount` und `CompletedLaunderingOperations += 1` erfasst und persistiert.

---

## 3. Architektur & Schnittstellen

### 3.1 Zustände (`LaunderingTerminalState`)
- `LockedUnowned`: Zugriff verweigert (noch nicht gekauft).
- `Ready`: Bereit zur Eingabe.
- `Validating`: Prüfung von Betrag und Kapazität.
- `CapacityExceeded`: Kapazitätsgrenze überschritten.
- `InsufficientCash`: Nicht genug Bargeld im Inventar.
- `Depositing`: Bargeld wird entnommen.
- `Processing`: Auftrag läuft in der Engine.
- `Completed`: Vorgang beendet, Auszahlung erfolgt.
- `Refunding`: Fehler beim Start, Rückerstattung läuft.
- `Refunded`: Rückerstattung abgeschlossen.
- `Faulted`: Unerwarteter Systemfehler.

### 3.2 Schnittstelle: `ILaunderingPort`
```csharp
public interface ILaunderingPort
{
    float GetLaunderCapacity();
    float GetCurrentLaunderTotal();
    float GetPlayerCash();
    bool DeductCash(float amount);
    bool RefundCash(float amount);
    bool StartNativeLaundering(float amount);
    bool PayoutCleanFunds(float amount, string label, string note);
}
```

### 3.3 Manager: `BusinessLaunderingManager`
- Kapselt alle Geschäftsregeln, Kapazitätsprüfungen, Reentranzsperren und Kompensationspfade.
- Verbindet sich mit `BusinessPurchaseManager` (Besitzstatus) und `TownHallPersistenceManager` (Statistik-Persistenz).

---

## 4. Offline-Testplan
1. **Zugriff gesperrt vor Kauf:** Bei `IsOwned == false` scheitert jeder Einzahlungsversuch sofort.
2. **Erfolgreicher Auftrag nach Kauf:** Bargeld vorhanden, Kapazität frei ➔ Bargeld abgezogen, native Operation gestartet, Status `Processing`.
3. **Kapazitätsüberschreitung:** Betrag > Restkapazität ➔ Abbruch ohne Bargeldabzug.
4. **Zu wenig Bargeld:** Spieler hat weniger Bargeld als gewünschter Betrag ➔ Abbruch ohne Abzug.
5. **Kompensierende Rückerstattung:** Bargeld abgezogen, aber Start der nativen Operation wirft Exception / schlägt fehl ➔ Bargeld wird zu 100 % erstattet.
6. **Mehrere parallele Operationen:** Addieren sich zur Gesamtsumme auf, solange unter Kapazitätslimit.
7. **Auszahlung / Abschluss:** Sauberer Übergang zu `Completed`, Statistiken aktualisiert.
