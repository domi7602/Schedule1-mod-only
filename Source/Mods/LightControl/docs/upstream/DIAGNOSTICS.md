# Gezielte Laufzeitdiagnose

**Status: Quelltext vorhanden, C#-Build und Laufzeitbeobachtung offen.**
Der Helfer ist ein eigenes Artefakt namens `LightControl.Diagnostics.dll`.
Er bietet keine Lichtsteuerung und darf nicht als fertige Mod präsentiert werden.

## Was sich bereits ohne Diagnose-Modul sammeln lässt

`tools/COLLECT_DIAGNOSTICS.cmd` ermittelt nach Eingabe des Spielordners eine
gezielte Dateiliste und Versionshinweise. Dafür ist kein eigener C#-Build nötig.
Das ist die erste benötigte Rückmeldung. Die folgenden Schritte werden erst
nach Sichtung dieser Informationen relevant.

## Beobachtungsumfang des Diagnosequelltexts

Der einzige direkte Fremdverweis ist die echte `MelonLoader.dll` für net6 aus
Version 0.7.3. Die verwendeten Melon-Attribute, Initialisierungs-, Update- und
Szenencallbacks sowie `MelonEnvironment.UserDataDirectory` sind im Quellstand
`982ed9981ac5b0619648a2e79bcc02fd8861db33` belegt. Die Metadaten `TVGS` und
`Schedule I` stammen aus dem offiziellen S1APITemplate; die konkrete
Zielinstallation muss dieselben Angaben bestätigen.

Der Helfer legt nur seinen eigenen Diagnoseordner und eine Kurzanleitung an.
Eine leere Datei `UserData/LightControl.Diagnostics/CAPTURE.request` fordert
genau eine Aufnahme an. Die Dateiabfrage erfolgt maximal einmal pro Sekunde.
Objektsuche und Vertragsinspektion laufen ausschließlich bei dieser Anforderung.
Es gibt keine automatische Vollsuche pro Bild oder nach jedem Szenenwechsel.

Eine Aufnahme enthält:

- Version, Produktname und Unity-Version, soweit die öffentlichen Getter erreichbar sind.
- Den beobachteten Typ des aktuellen Renderpipeline-Assets; ein fehlender Wert wird nicht als bestätigte Pipeline ausgegeben.
- Version und SHA-256 ausgewählter geladener Referenzassemblies, ohne ihre Dateipfade.
- Tatsächlich verfügbare öffentliche Member ausgewählter S1API-/Unity-Typen.
- Bis zu 256 aktive Unity-Lichtkomponenten mit Farbe, Intensität, Reichweite, Schaltzustand und begrenztem Hierarchiehinweis.
- Explizite Fehl-/Nichtverfügbarkeitsmeldungen jeder fehlgeschlagenen Probe.

Die dokumentierte generische Suche `UnityEngine.Object.FindObjectsOfType`
wird nur verwendet, wenn die erwartete öffentliche Überladung tatsächlich
reflektierbar ist. Ob die resultierenden IL2CPP-Wrapper hier über
`System.Collections.IEnumerable` zugänglich sind, ist eine offene Laufzeitprüfung.
Bei fehlender Unterstützung wird kein alternativer unbestätigter Spielhook benutzt.

## Grenzen der Aussage

Die Aufnahme der ersten 256 Lichtkomponenten begrenzt die Ausgabe. Die einmalige
Unity-Suche selbst kann mehr Objekte enumerieren; dafür ist kein Performancebudget
einer produktiven Mod nachgewiesen. Inaktive, entladene und rein emissive Leuchten
sind damit nicht vollständig abgedeckt. Die Probe erfasst noch keine Materialplätze,
Shaderparameter, Schalter-Leuchten-Beziehungen oder nachgewiesenen nativen Instanz-IDs.

`GetInstanceID` und ein Transformpfad sind ausschließlich Diagnosehinweise. Sie
werden nie als dauerhafte Kennung oder Netzwerkkennung in den Kern übernommen.
Die öffentliche Existenz eines Members beweist keine sichere Patchreihenfolge,
Gameplayneutralität, Nutzerberechtigung oder Replikation.

## Spätere lokale Ausführung

1. Den Helfer mit dem in `BUILD.md` beschriebenen Diagnostics-Modus und der echten Loader-Referenz bauen.
2. Buildbericht und erzeugte Diagnose-DLL sichern; ihr Hash gehört zur Beobachtung.
3. Nur diese Diagnose-DLL in die Testinstallation unter `Mods` kopieren.
4. Einen kopierten Testspielstand laden. Eine Pflanzenlampe, eine schaltbare Lampe und eine feste Weltleuchte für die gezielte spätere Untersuchung bereithalten.
5. Die leere Anforderungsdatei anlegen. Den neuen JSON-Bericht im Diagnoseordner prüfen.
6. Beobachtungsbericht, Diagnose-DLL-Hash und tatsächliche Spielversion zuordnen.
7. Helfer durch Entfernen seiner DLL aus `Mods` deaktivieren. Der eigene Diagnoseordner kann danach gelöscht werden.

Diese Anleitung ist ein geplanter Testablauf, kein Protokoll einer bereits
erfolgreichen Installation. Es werden keine Unity-Werte gesetzt, Harmony-Patches
installiert, Speicherstände verändert oder Netzwerkpakete gesendet.

## Quellen

- [MelonBase 0.7.3](https://github.com/LavaGang/MelonLoader/blob/982ed9981ac5b0619648a2e79bcc02fd8861db33/MelonLoader/Melons/MelonBase.cs)
- [MelonMod 0.7.3](https://github.com/LavaGang/MelonLoader/blob/982ed9981ac5b0619648a2e79bcc02fd8861db33/MelonLoader/Melons/MelonMod.cs)
- [MelonEnvironment 0.7.3](https://github.com/LavaGang/MelonLoader/blob/982ed9981ac5b0619648a2e79bcc02fd8861db33/MelonLoader/Utils/MelonEnvironment.cs)
- [Unity 2022.3: FindObjectsOfType](https://docs.unity3d.com/2022.3/Documentation/ScriptReference/Object.FindObjectsOfType.html)
- [Unity 2022.3: currentRenderPipeline](https://docs.unity3d.com/2022.3/Documentation/ScriptReference/Rendering.GraphicsSettings-currentRenderPipeline.html)
