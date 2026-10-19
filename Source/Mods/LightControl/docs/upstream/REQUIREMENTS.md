# Verbindlicher Anforderungskatalog

Stand: 18. Oktober 2026; Entwicklungsstand 0.1.0-dev.1.

**Keine Pflichtanforderung gilt bereits als vollständig im Spiel abgenommen.** „Kern vorhanden“ bedeutet: eigener C#-Quelltext ist implementiert und statisch geprüft, aber mangels Compiler noch nicht gebaut oder durch C#-Tests ausgeführt. „Quellvertrag belegt“ bedeutet: der konkrete externe Member wurde in der festgehaltenen Quelle gefunden. Ein Quellvertrag ist kein Laufzeitnachweis.

Der vollständige Auftrag in `IMPLEMENTATION_BRIEF.txt` bleibt maßgeblich. Optionale Funktionen nach 1.0 wurden nicht zu Pflichtfunktionen gemacht; der 1.0-Umfang wurde nicht verkleinert. Die R-Nummern unten sind interne Nachverfolgungskennungen, die T-Nummern die unveränderten Abnahmetests des Auftrags.

| ID | Pflichtanforderung | Implementierungsstelle / offene Zuständigkeit | Prüffall | Tatsächlicher Status |
|---|---|---|---|---|
| R001 | Eigenständiger Quelltext, Kennung und Namespace | Projektdateien / LightControl.Core / LightControl.Diagnostics | Review | Quelltext vorhanden |
| R002 | Windows/IL2CPP, konkrete Spiel-, Loader-, S1API- und Unity-Version bestätigen | docs/COMPATIBILITY.md / AP-00 | T01 | Installation offen |
| R003 | Echte Release-DLL und installierbare endgültige ZIP | Produktive Mod noch nicht vorhanden / AP-01, AP-09–11 | T01, T18 | Offen |
| R004 | Lokale Referenzpfade und Laufzeitabhängigkeiten eindeutig trennen | Diagnostics.csproj / tools/build.ps1 / spätere Modreferenzen | T01, T18 | Teilweise vorbereitet |
| R005 | Frischer Build, Fehlerabbruch, keine alte DLL übernehmen | tools/build.ps1 | Buildprüfung, T01 | Skript vorhanden; PowerShell-Lauf offen |
| R006 | Logische Leuchte mit mehreren Komponenten und Emissionsflächen | LampRegistration / geplanter nativer Adapter | T04 | Registermetadaten im Kern; Adapter offen |
| R007 | Dauerhafte native Instanz-ID, Itemdefinition kein Instanzschlüssel | PersistentLampId / LampIdentity / geplanter Identitätsadapter | T07 | Kern vorhanden; native ID offen |
| R008 | Weltleuchten-ID mit Kollisionserkennung und Versionsstrategie | PersistentLampId / geplanter Weltlocator | T07, T08 | Vertragsform vorhanden; Wiedererkennung offen |
| R009 | Register nach Ladeabschluss und bei Platzierung ergänzen | LightStateStore / GameLifecycle- und BuildEvents-Anbindung | T08, T09 | Kern vorhanden; Ereignisanbindung offen |
| R010 | Bewegen, Entladen und echte Entfernung unterscheiden | LightStateStore / geplanter Lebenszyklusadapter | T07–T09 | Kern vorhanden; Spielnachweis offen |
| R011 | Ampeln, Fahrzeuge, Taschenlampe sowie gehaltene/eingelagerte Objekte ausschließen | Geplante Lampenadapter/Registrierungsfilter | T04, T08 | Offen |
| R012 | Original, Weiß, Rot, Orange, Gelb, Grün, Cyan, Blau, Lila, Rosa, Regenbogen in dieser Reihenfolge | ColorMode / LampBaseState.NextColor / VisualResolver | Kerntests, T02, T03 | Kern vorhanden |
| R013 | Intensitäten 10/25/50/75/100/150/200 Prozent | IntensityLevel / LampBaseState / VisualInstruction | Kerntests, T05 | Kern vorhanden |
| R014 | Aktuellen unveränderten Spielwert skalieren, keine kumulative Anwendung | VisualInstruction / geplanter nativer Basiswertadapter | Kerntests, T05 | Rechenvertrag vorhanden; Abgriff offen |
| R015 | Originalstellung, native Schaltung und Flackern erhalten | VisualResolver / geplanter Lichtadapter | T03, T05, T16 | Einflussmodell vorhanden; Spielwirkung offen |
| R016 | Lichtfarbe und Materialemission gemeinsam, Texturen/Originalintensitäten erhalten | Geplante Licht-/Materialadapter, Shaderprüfung | T04, T05 | Offen |
| R017 | Wachstum und Versorgung durch visuelle Änderung unverändert | Keine Gameplayänderung im Kern / künftiger Vergleichslauf | T06 | Spielnachweis offen |
| R018 | Interaktion an Spielaktionen binden, genau einmal je Tastendruck, Neubelegung | Geplanter Eingabeadapter mit belegten Controls-Verträgen | T02 | Offen |
| R019 | Sprintmodifikator bei schaltbaren Lampen; native Schaltung und Aufnehmen erhalten | Geplanter Interaktionsadapter | T02, T08 | Offen |
| R020 | Menüs, Telefon, Texteingabe, Sichtblockaden und Reichweite beachten | Geplante Ziel-/Kontextprüfung | T02 | Offen |
| R021 | Verschiedene Collider derselben Leuchte auf dieselbe Instanz abbilden | Geplanter Register-/Zieladapter | T02, T07 | Offen |
| R022 | Lampenwerte am richtigen Spielstand mit eigenem Datenformat speichern | JsonStatePersistence / geplanter S1API-Persistenzadapter | Kerntests, T07, T09 | Daten-API vorhanden; Datei-/Saveanbindung offen |
| R023 | Schema validieren, unbekannte Zukunftsversion nicht überschreiben | JsonStatePersistence | Kerntests, T15, T16 | Kern vorhanden |
| R024 | Schreibsperre nach Lesefehler; fehlende Daten von Lesefehler trennen | JsonStatePersistence / späterer Speicheradapter | Kerntests, T15 | Kern vorhanden; I/O-Vertrag offen |
| R025 | Vollständiges Schreiben, Wiederherstellung und Sicherung | Geplanter Dateispeicheradapter nach geprüftem Saveablauf | T15 | Offen |
| R026 | Nur Mod-Host schreibt gemeinsame Lampendaten | SessionMode / JsonStatePersistence / ExportPersisted | Kerntests, T12, T13 | Kern vorhanden; Hostableitung offen |
| R027 | Vorgemerkte Zustände für noch nicht vorhandene Objekte erhalten | LightStateStore / JsonStatePersistence | Kerntests, T07–T09 | Kern vorhanden |
| R028 | Echte Neuplatzierung beginnt mit Standard; fortbestehende Instanz behält Zustand | LightStateStore / nativer Identitäts- und Entfernungsadapter | Kerntests, T08 | Kern vorhanden; Unterscheidung im Spiel offen |
| R029 | Spielstände/Sitzungen sauber trennen, nichts lokal an späteren Mod-Host übertragen | LightStateStore.BeginSession / Sitzungsrollen | Kerntests, T09, T13 | Kern vorhanden |
| R030 | Eigene Telefon-App, dieselbe Zustandsverwaltung wie Direktbedienung | Geplante S1API.PhoneApp-Ableitung | T10 | Quellvertrag belegt; UI offen |
| R031 | Raumgruppen aus echten Schalterbeziehungen, nachvollziehbare Sammelgruppe | Geplanter Zuordnungsadapter / LampSelection | T11 | Kernselektion vorhanden; Zuordnung offen |
| R032 | Ein/Aus ausschließlich über tatsächlich unterstützte native Aktionen | Geplanter nativer Schaltadapter | T05, T10, T13 | Offen |
| R033 | Unbesessene Immobilien sichtbar und regulär gesperrt, keine erfundenen Individualrechte | Geplanter Immobilien-/Berechtigungsadapter | T11 | Property-Verträge belegt; Integration offen |
| R034 | Gemischte Gruppenwerte in der Oberfläche darstellen | Geplanter UI-Zustandsleser | T10 | Offen |
| R035 | Fernsteuerung sowie geschlossene App ohne vollständige Daueraktualisierung | Register / geplanter Telefonadapter | T10, T17 | Offen |
| R036 | Alle 14 verpflichtenden Lampenfamilien konkret abdecken | Geplante Adapter; Abdeckungstabelle unten | T04 | 0 von 14 im Spiel geprüft |
| R037 | Mod-Erkennung, Protokollversion, nur bestätigte Modteilnehmer adressieren | Protocol.cs / noch auszuwählender Transport | T12, T13 | Datenmodell vorhanden; Erkennung/Transport offen |
| R038 | Host prüft Sender, Ziel, Werte, Sitzung und Berechtigung | LightStateStore / ClientCommand / CommandAuthorizationContext | Kerntests, T12 | Prüfmodell vorhanden; echte Senderidentität offen |
| R039 | Gruppenbefehle atomar für die aktuellen Mitglieder | LightStateStore / eingefrorene Zielauswahl | Kerntests, T11, T12 | Kern vorhanden |
| R040 | Snapshot bei Beitritt, Revisionen, Rennen und veraltete Nachrichten behandeln | StateSnapshot / StateDelta / LightStateStore | Kerntests, T12 | Kern vorhanden; Transport-/Beitrittstest offen |
| R041 | Ohne Mod-Host deutlich angezeigter rein lokaler Sitzungsmodus | SessionMode.Local / geplanter Mod-Handshake und UI | Kerntests, T13 | Kern vorhanden; Erkennung/Anzeige offen |
| R042 | Vanilla-Teilnehmer können mitspielen, keine zusätzliche Farbdarstellung zusichern | Geplanter Transport und Vanilla-Sitzung | T13 | Offen |
| R043 | Regenbogen und Disco aus gemeinsamer Zeitbasis/Stabilwert, kein Farbpaket je Bild | VisualResolver / DiscoEffect / geplanter Zeitabgleich | Kerntests, T12, T14 | Kern vorhanden; Zeitsynchronisierung offen |
| R044 | Grundstücksdisco überlagert Weltdisco; mehrere Grundstücke unabhängig | EffectState / LightStateStore | Kerntests, T14 | Kern vorhanden |
| R045 | Farbänderung während Disco bleibt Grundzustand; Helligkeit sofort wirksam | LampBaseState / VisualResolver | Kerntests, T14 | Kern vorhanden |
| R046 | Ausgeschaltete Leuchten bleiben bei Effekten aus | VisualInstruction ohne Einschaltaktion / nativer Adapter | T05, T14 | Vertragsmodell vorhanden; Spieltest offen |
| R047 | Neue Leuchten Standard plus aktiver Effekt; spätestens 5 Sekunden Bereichsabgleich | LightStateStore / geplanter Ereignis-/Nachkontrolladapter | Kerntests, T08, T14 | Kernregel vorhanden; Erkennungsfrist offen |
| R048 | Frühere Gruppenänderung nicht automatisch auf neue Mitglieder anwenden | LightStateStore / momentane Gruppenzielmenge | Kerntests, T08, T11 | Kern vorhanden |
| R049 | Temporäre Effekte nicht dauerhaft speichern; keine Saves während Animation | JsonStatePersistence / Effekttrennung | Kerntests, T14, T17 | Datenmodell vorhanden; Savefrequenz im Spiel offen |
| R050 | Konfiguration, gültige Grenzen, Deaktivierung und Rückbau | assets/config.defaults.json / VisualResolver / geplanter Konfigurationsadapter | Kerntests, T16 | Vorgaben vorhanden; Laufzeitbindung offen |
| R051 | Deutsch/Englisch plus Französisch, Spanisch, Portugiesisch, Italienisch, Polnisch, Russisch | assets/lang/*.json | Strukturprüfung, UI-/Sprachabnahme | 42 Schlüssel je Sprache; Abnahme offen |
| R052 | Eigene Materialien/Events freigeben, keine Lecks über Ladezyklen | Geplante nativen Adapter; Diagnose deaktiviert eigene Abfrage | T16, T17 | Produktiver Ressourcenzyklus offen |
| R053 | Dokumentierte Hauptthread-/GPU-Budgets, keine Vollsuche oder Materialerzeugung je Bild | Geplante Profilingläufe und Adapter | T17 / Performance | Keine Messung |
| R054 | Alle Pflichtprüfungen mit Voraussetzungen, Ergebnis, Version und DLL-Hash führen | docs/ACCEPTANCE.md / docs/TESTREPORT.md | T01–T18 | Katalog vorhanden; Spielprüfungen offen |
| R055 | Geprüften DLL-Hash einfrieren, Paketierung baut nicht neu | tools/build.ps1 / tools/package.py | Pakettests, T18 | Werkzeuge vorhanden; echte DLL offen |
| R056 | Ausdrückliche Allowlist, keine Spiel-DLLs, private Daten oder Entwicklungsreste | tools/package.py / Paketmanifest | Pakettests | Paketlogik lokal geprüft |
| R057 | Prüfsummen, sichere Archivpfade, frisches Entpacken und Inhaltsvergleich | tools/package.py verify / eigene Prüfläufe | Pakettests, T18 | Entwicklungsarchiv prüfbar; Installation offen |
| R058 | T18 separat an endgültigen ZIP-Hash binden, ZIP danach unverändert | tools/package.py verify-release / docs/ACCEPTANCE.md | T18 | Verfahren vorhanden; T18 offen |
| R059 | Deutsche Installation/Update/Deinstallation, Kompatibilität, Buildinfo, Lizenzen, Download | README_DE.txt / docs / Paketablauf | T16, T18 | Entwicklungsanleitung vorhanden; Releaseanleitung folgt geprüftem Ladeverhalten |

## Abdeckung der Lampenfamilien

Eine Leuchte wird erst nach geprüfter Komponenten-, Material-, Identitäts- und Schaltzuordnung als unterstützt markiert. Bloßes Auffinden einer Unity-Light-Komponente reicht nicht aus. Jede Zeile benötigt mindestens ein konkretes Exemplar und T04/T05, bei platzierbaren Leuchten zusätzlich T07/T08.

| Familie | Bestätigtes Exemplar | Adapter | Farb-/Emissionstest | Status |
|---|---|---|---|---|
| Halogenpflanzenlampe | Fehlt | Offen | Nicht ausgeführt | Nicht freigegeben |
| LED-Pflanzenlampe | Fehlt | Offen | Nicht ausgeführt | Nicht freigegeben |
| Vollspektrumpflanzenlampe | Fehlt | Offen | Nicht ausgeführt | Nicht freigegeben |
| Zeltbeleuchtung | Fehlt | Offen | Nicht ausgeführt | Nicht freigegeben |
| Stehlampe | Fehlt | Offen | Nicht ausgeführt | Nicht freigegeben |
| Wandlampe | Fehlt | Offen | Nicht ausgeführt | Nicht freigegeben |
| Deckenleuchte | Fehlt | Offen | Nicht ausgeführt | Nicht freigegeben |
| Neonröhre | Fehlt | Offen | Nicht ausgeführt | Nicht freigegeben |
| Hängeleuchte | Fehlt | Offen | Nicht ausgeführt | Nicht freigegeben |
| Laterne | Fehlt | Offen | Nicht ausgeführt | Nicht freigegeben |
| Straßenlampe | Fehlt | Offen | Nicht ausgeführt | Nicht freigegeben |
| Strahler | Fehlt | Offen | Nicht ausgeführt | Nicht freigegeben |
| Leuchtreklame | Fehlt | Offen | Nicht ausgeführt | Nicht freigegeben |
| Automatenbeleuchtung | Fehlt | Offen | Nicht ausgeführt | Nicht freigegeben |

## Verbleibende Integrationsarbeit

Die jeweils genannten Adapter sind noch zu implementierende Bestandteile, keine bereits vorhandenen leeren Klassen. Das Projekt enthält bewusst keine produktive `LightControl.Mod`-Assembly, die durch Stubs einen erfolgreichen Funktionsbuild vortäuschen könnte. Der Diagnosehelfer liefert ausschließlich Beobachtungsdaten.

