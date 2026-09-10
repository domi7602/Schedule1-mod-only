# PhoneScroll — ThirdParty-Mod

**Version:** 1.4
**Autor:** V4LEXL (NexusMods)
**Quelle:** NexusMods — externer Download (Closed-Source)

## Zweck

PhoneScroll repariert das vanilla-Phone-HomeScreen, das **nicht für Listen gebaut wurde**: es injiziert eine `ScrollViewport`-`Mask`-Komponente um `HomeScreen/AppIcons` und scrollt die App-Icons per Mausrad.

## Verhalten (Decompile-Highlights, ilspycmd 8.2.0)

- **Hook:** Harmony-Postfix auf `HomeScreen.Start` (Priority 800 / `Priority.First`)
- **Setup:** baut `ScrollViewport` als Vollbild-RectTransform mit `Mask`, reparentiert `AppIcons` darein, setzt `localPosition.y += 120f`
- **Hardening:** jeden Frame (gedrosselt auf 1s) setzt `MaskableGraphic.maskable = true` und `Canvas.overrideSorting = false` auf allen Icon-Children — damit die Mask sauber clippt
- **Scroll:** `OnUpdate` liest `Input.mouseScrollDelta.y * ScrollSpeed` (Default **30**), klemmt auf `[0, maxOffset]`, setzt `AppIcons.localPosition.y = originalY + scrollOffset`

## Bekannte Probleme

### Vibrierendes Scrollen

`ScrollSpeed = 30` ist zu hoch für ein 30-Pixel-Mausrad-Tick. **Empfohlene Anpassung** in `<GameDir>\UserData\MelonPreferences.cfg` (Sektion `[PhoneScroll]`):

```ini
[PhoneScroll]
Enabled = True
ScrollSpeed = 5
DebugLog = False
```

5–10 ist ein guter Startwert. Niedriger = smootheres Scrollen, höhere Sprungweite pro Tick. Probiere, was sich für dich am besten anfühlt.

### Konkurrenz mit S1API-PhoneApps

PhoneScroll hört nur auf `Input.mouseScrollDelta.y` — es filtert nicht nach Fokus. Wenn ein S1API-PhoneApp offen ist (NotesApp, CalculatorApp, PotScanner, PocketShop, BankApp) und einen eigenen `ScrollRect` bedient, **reagieren beide Systeme auf denselben Mausrad-Tick**. Resultat: das S1API-PhoneApp scrollt intern, während PhoneScroll die HomeScreen-App-Icons verschiebt — gefühltes "Vibrieren".

Workaround: PhoneApp-ScrollSensitivity pro Mod niedrig setzen (siehe In-Game-Befunde aus 2026-09-10).

## Deploy

PhoneScroll wird **automatisch** via `Tools/deploy-thirdparty.ps1` (aufgerufen aus `Source/Mods/Directory.Build.targets` → `DeployThirdParty`) nach `<GameDir>\Mods\PhoneScroll.dll` kopiert. Keine manuellen Schritte nötig — ein `dotnet build` der Solution reicht.

Whitelist via `ThirdParty/.deployignore`: alles in `ThirdParty/S1API/`, `S1MAPI/`, `S1MCPServer-master/`, `ScheduleOne-Sideload/`, `ScheduleOne-Hash/`, `MoreDrugs/` ist explizit vom Deploy ausgenommen.

## Lizenz / Attribution

V4LEXL auf NexusMods. Kein Quellcode im Repo verfügbar; `PhoneScroll.decompiled.cs` (lokal im Temp-Verzeichnis bei Decompile, nicht im Repo) als Referenz für die Verhaltensanalyse. Im Repo liegt nur die `.dll` (Binary Deployment).
