# Sprach- und Konfigurationsressourcen

**Status: Ressourcen vorbereitet; produktive UI-/Konfigurationsanbindung offen.**

`assets/lang` enthält Deutsch, Englisch, Französisch, Spanisch, Portugiesisch,
Italienisch, Polnisch und Russisch. Jede Datei enthält dieselben 42 Schlüssel.
`translationReview: pending` bedeutet: Noch keine vollständige sprachliche und
visuelle Abnahme im Spiel. Die portugiesische Fassung orientiert sich an
europäischem Portugiesisch. Farbnamen sind Bedienbezeichnungen; die tatsächliche
Farbabbildung liegt zentral im Kern.

Die geplante Auflösung lautet: explizite Sprachwahl, sonst Systemsprache;
unbekannte Sprache fällt auf Englisch zurück. Ein fehlender oder ungültiger
Übersetzungsschlüssel soll auf den englischen Schlüssel zurückfallen und einmal
pro Sitzung diagnostiziert werden. Formatplatzhalter müssen vor Verwendung
zwischen Ziel- und Standardsprache verglichen werden.

`assets/config.defaults.json` enthält Vorgaben für die spätere Laufzeitbindung.
Die Datei wird aktuell von keinem installierbaren Hauptmodul geladen. Sie ist
keine Behauptung, dass Konfiguration, Deaktivierung oder Telefonsteuerung bereits
im Spiel funktionieren.

| Einstellung | Vorgabe | Zulässiger Bereich / Wirkung |
|---|---|---|
| enabled | true | Eigene visuelle Eingriffe aktivierbar; Rückbau erst mit Adaptern implementierbar |
| language | auto | auto, de, en, fr, es, pt, it, pl, ru |
| phoneAppEnabled | true | Sichtbarkeit der künftigen Telefon-App |
| includeWorldLights | true | Registrierungsfilter für geeignete Weltleuchten |
| interactionRangeMeters | 4 | 1 bis 10 Meter; tatsächliche Sicht-/Reichweitenprüfung steht aus |
| rainbowEnabled | true | Farbzyklus soll Regenbogen enthalten |
| rainbowCyclesPerMinute | 6 | 0,5 bis 60; Vorgabe entspricht 10 Sekunden je Zyklus |
| discoBpm | 120 | 30 bis 150; Startparameter werden vom maßgeblichen Host übernommen |

Die Ressourcenvorgaben sind Projektentscheidungen in Anlehnung an die
[Funktionsbeschreibung der Referenz](https://www.nexusmods.com/schedule1/mods/2883).
Die Mod übernimmt daraus keine fremden Konfigurationssektionen, eigenen
Dateiidentitäten oder Speicherdaten.
