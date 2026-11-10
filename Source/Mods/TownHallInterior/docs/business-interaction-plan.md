# Town Hall Business: In-Game 3D Interaction Specification (Phase 4)

## 1. Übersicht & Zielsetzung

Dieses Dokument beschreibt die Spezifikation für die In-Game-3D-Interaktionstrigger im und am Hyland Rathaus.
Nach erfolgreicher Etablierung von World-Registration (Phase 1), Kauf & Wirtschaft (Phase 2) sowie Geldwäsche-Terminal & Persistenz (Phase 3) schließt dieser Schritt die spielerseitige Interaktion im 3D-Raum ab:

1. **Eingangs- / Kauf-Trigger (Erdgeschoss / Portal)**:
   - Erkennung der Spielerposition vor dem Portal (`X: 52..56, Y: 1.0..3.5, Z: 29.5..32.5`).
   - Bei Nicht-Besitz: HUD-Aufforderung `[E] Rathaus erwerben ($250.000)`.
   - Bei Tastendruck `E`: Ausführung von `TryPurchaseTownHall(Auto)` mit sofortiger Rückmeldung via In-Game-HUD-Notification.
   - Bei Besitz: Statushinweis `Rathaus Hyland (Eigentum)`.

2. **Geldwäsche-Terminal-Trigger (Obergeschoss Arbeitsplatz 0)**:
   - Verankerung an der geometrischen Koordinate von `office-computer-0` (`X=56.4f, Y=5.5f, Z=30.25f`).
   - Erkennungsradius: <= 2.2 m.
   - Bei Nicht-Besitz: Sperr-Meldung bei Interaktion (Zugriff verweigert, Eigentumsnachweis fehlt).
   - Bei Besitz: HUD-Aufforderung `[E] Geldwäsche-Terminal nutzen`.
   - Bei Tastendruck `E`: Ermittlung von Bargeld und verfügbarer Tageskapazität, Übergabe an `TryStartLaundering` und HUD-Benachrichtigung mit Betrag und verbleibender Kapazität.

3. **Debounce- & State-Schutz**:
   - Reentrancy- und Cooldown-Schutz (0.8s) verhindert Doppelauslösungen bei Tastendruck.
   - Strikte Isolationsprüfung: Interaktionen führen nur in der freigegebenen Testumgebung (`SaveGame_2` / `TownHallTest`) zu State-Mutationen.

---

## 2. Architektur & Port-Abstraktion

```
+-------------------------------------------------------------+
|                 TownHallInterior.Mod                        |
|  - OnUpdate(): _interactionCoordinator.Tick(deltaTime)     |
|  - OnGUI(): Render active HUD prompt if present             |
+------------------------------+------------------------------+
                               |
                               v
            +-------------------------------------+
            |   TownHallInteractionCoordinator    | (Pure C# Domain)
            |  - Evaluates Player Position        |
            |  - Resolves InteractionZone         |
            |  - Manages Cooldown & Input State   |
            |  - Triggers Purchase / Laundering   |
            +------------------+------------------+
                               |
             +-----------------+-----------------+
             |                                   |
             v                                   v
+------------------------+           +------------------------+
|  ITownHallPlayerPort   |           | BusinessPurchaseManager|
|  - Position            |           | BusinessLaunderingMgr  |
|  - Key Input           |           +------------------------+
|  - Notifications       |
+------------------------+
             |
             v
+---------------------------------+
| NativeTownHallInteractionPort   | (MelonLoader / Il2Cpp)
| - Player.Local.transform        |
| - Input.GetKeyDown(KeyCode.E)   |
| - NotificationsManager.Instance |
| - Procedural TownHallIcon       |
+---------------------------------+
```

---

## 3. Akzeptanzkriterien

1. **Reine Testbarkeit**: `TownHallInteractionCoordinator` ist vollständig durch Unit Tests abgedeckt (keine UnityEngine-Hänger).
2. **Zone-Genauigkeit**: Außerhalb der Zonen wird kein Trigger aktiv; innerhalb der Zonen schalten Eingangs- und Terminal-Prompt präzise um.
3. **Sicherheits-Gate**: In unautorisierten Slots (`SaveGame_1`) schlägt jede Mutation fehl, ohne Spielabsturz.
4. **Fehlertoleranz**: Fehlende Spieler-Referenzen (`Player.Local == null`) oder native Manager führen zu sauberem Graceful Fallback.
