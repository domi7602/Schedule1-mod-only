# LightControl – API-Nachweise

Stand: 18. Oktober 2026. Dieses Dokument trennt **Quellbelege**, **tatsächliche Buildprüfungen** und **fehlende Spieltests**. Die Anwesenheit einer Signatur im veröffentlichten Quellstand bestätigt weder ihre Erreichbarkeit in der Installation des Nutzers noch das Verhalten einer fertigen Lichtintegration.

## 1. Geprüfte Primärquellen

| Komponente | Festgehaltener Quellstand | Beleg |
|---|---|---|
| S1API Forked | Tag `v3.2.1`; Commit `a4cbdd9e0ec54895e64fd6be0e91874ba6649c9b` | [Release](https://github.com/ifBars/S1API/releases/tag/v3.2.1), [Commit](https://github.com/ifBars/S1API/tree/a4cbdd9e0ec54895e64fd6be0e91874ba6649c9b) |
| MelonLoader | Tag `v0.7.3`; Commit `982ed9981ac5b0619648a2e79bcc02fd8861db33` | [Release](https://github.com/LavaGang/MelonLoader/releases/tag/v0.7.3), [Commit](https://github.com/LavaGang/MelonLoader/tree/982ed9981ac5b0619648a2e79bcc02fd8861db33) |
| S1APITemplate | Commit `ff93ce131518eef04a8566b62f1d3c0b8347bdfc` | [Core](https://github.com/ifBars/S1APITemplate/blob/ff93ce131518eef04a8566b62f1d3c0b8347bdfc/Core.cs), [Konstanten](https://github.com/ifBars/S1APITemplate/blob/ff93ce131518eef04a8566b62f1d3c0b8347bdfc/Utils/Constants.cs) |
| SteamNetworkLib, ausschließlich Transportkandidat | Tag `v1.6.0`; Commit `aea3c25ac200393c905c6a4ce5fca3b44e6538e3` | [Release](https://github.com/ifBars/SteamNetworkLib/releases/tag/v1.6.0), [Client-Quelle](https://github.com/ifBars/SteamNetworkLib/blob/aea3c25ac200393c905c6a4ce5fca3b44e6538e3/SteamNetworkClient.cs) |

Die S1API-Releasebeschreibung nennt Schedule I 0.4.7, zuletzt 0.4.7f12. Die dort gemeldeten API-Prüfungen sind Prüfungen des API-Projekts. Sie ersetzen keine LightControl-Abnahme. Reale Steam-Verbindungen wurden im abschließenden 3.2.1-Releasepass ausdrücklich nicht erneut getestet. Die tatsächliche Spiel-, Unity- und Renderpipelineversion dieser Nutzerinstallation liegt noch nicht vor.

Die untersuchten Textquellen wurden über den GitHub-Lesezugriff aus den oben genannten Repositories gelesen. Ein Shell-Download war mit HTTP 403 blockiert; das war eine Netzwerkantwort, keine automatische Ablehnung einer Benutzerfreigabe. Die Rohquellen liegen für diese Arbeitsphase im separaten Rechercheverzeichnis. Sie werden nicht als Spielreferenzen oder als Bestandteil einer funktionsfähigen Mod ausgegeben.

## 2. Loader-Einstieg und Diagnose

### E-ML-01 – Tatsächlich vorhandene Loader-Verträge

Quellen: [MelonBase](https://github.com/LavaGang/MelonLoader/blob/982ed9981ac5b0619648a2e79bcc02fd8861db33/MelonLoader/Melons/MelonBase.cs), [MelonMod](https://github.com/LavaGang/MelonLoader/blob/982ed9981ac5b0619648a2e79bcc02fd8861db33/MelonLoader/Melons/MelonMod.cs).

| Typ | Vorhandener Member | Verwendung / Grenze |
|---|---|---|
| `MelonLoader.MelonMod` | Öffentliche abstrakte Mod-Basisklasse | Eigene Diagnosemod kann davon erben. |
| `MelonLoader.MelonBase` | `public virtual void OnInitializeMelon()` | Initialisierung der eigenen Diagnose und Ereignisbindungen. |
| `MelonLoader.MelonBase` | `public virtual void OnLateInitializeMelon()` | Zusätzlicher Loader-Zeitpunkt; kein Spielstand-Ladeabschluss. |
| `MelonLoader.MelonBase` | `public virtual void OnDeinitializeMelon()` | Eigene Ereignisbindungen freigeben. |
| `MelonLoader.MelonBase` | `public virtual void OnApplicationQuit()` | Prozessende; nicht mit dem Verlassen eines Spielstands gleichsetzen. |
| `MelonLoader.MelonBase` | `public virtual void OnUpdate()`; `OnLateUpdate()` | Verfügbare Frame-Callbacks; keine Vorgabe für Scans pro Bild. |
| `MelonLoader.MelonMod` | `public virtual void OnSceneWasLoaded(int buildIndex, string sceneName)` | Szene geladen; nicht gleichbedeutend mit fertig geladenem Save. |
| `MelonLoader.MelonMod` | `public virtual void OnSceneWasInitialized(int buildIndex, string sceneName)` | Initialisierungsbeobachtung. |
| `MelonLoader.MelonMod` | `public virtual void OnSceneWasUnloaded(int buildIndex, string sceneName)` | Ressourcenbereinigung nach Entladen. |

### E-ML-02 – Attribute, Logging, eigene Dateien

Quellen: [MelonInfoAttribute](https://github.com/LavaGang/MelonLoader/blob/982ed9981ac5b0619648a2e79bcc02fd8861db33/MelonLoader/Attributes/MelonInfoAttribute.cs), [MelonGameAttribute](https://github.com/LavaGang/MelonLoader/blob/982ed9981ac5b0619648a2e79bcc02fd8861db33/MelonLoader/Attributes/MelonGameAttribute.cs), [MelonLogger](https://github.com/LavaGang/MelonLoader/blob/982ed9981ac5b0619648a2e79bcc02fd8861db33/MelonLoader/Utils/MelonLogger.cs), [MelonEnvironment](https://github.com/LavaGang/MelonLoader/blob/982ed9981ac5b0619648a2e79bcc02fd8861db33/MelonLoader/Utils/MelonEnvironment.cs).

- `MelonInfoAttribute(Type type, string name, string version, string author, string downloadLink = null)` ist vorhanden.
- `MelonGameAttribute(string developer = null, string name = null)` ist vorhanden. Das offizielle S1API-Modtemplate verwendet die Konstanten `TVGS` und `Schedule I`. Dies ist ein Templatebeleg; die tatsächlich vom Spiel gemeldeten Strings werden erst im Startlog bestätigt.
- `MelonBase.LoggerInstance` ist öffentlich lesbar und liefert `MelonLogger.Instance`.
- `MelonLogger.Instance.Msg(string)`, `Warning(string)`, `Error(string)` und `Error(string, Exception)` sind vorhanden.
- `MelonLoader.Utils.MelonEnvironment.UserDataDirectory` ist ein öffentlicher statischer String-Getter. Die Quelle bildet den Pfad aus `MelonBaseDirectory` und `UserData`; keine feste Steam-Installation ist erforderlich.
- `MelonEnvironment.Il2CppAssembliesDirectory` bildet den Pfad aus dem Loaderverzeichnis und `Il2CppAssemblies`.

**Buildentscheidung:** Eine reine verwaltete Diagnose kann gegen die echte `MelonLoader.dll` bauen und geladene Assemblyversionen sowie verfügbare öffentliche API-Metadaten untersuchen. Dafür sind keine erfundenen Ersatzassemblies erforderlich. Ob ein erzeugter Diagnose-Build im Spiel lädt, muss mit exakt seiner DLL und seinem Hash beobachtet werden. Der konkrete Buildstatus steht im Build- und Testbericht, nicht in dieser Quellanalyse.

**Diagnosegrenze:** Ein Typname oder eine per Reflection gefundene Methode beweist noch keine erfolgreiche Lampenintegration. Unbekannte Methoden dürfen nicht durch angenommene Signaturen aufgerufen werden. Insbesondere unterscheiden sich `System.Type`, generierte IL2CPP-Typen, Arrays und Delegaten; Unity-Quelldokumentation allein beweist deren erzeugte Wrapper-Signatur nicht.

### E-DOTNET-01 – Hashen von Assemblydateien unter .NET 6

Die festgehaltene [.NET-6.0.36-Quelle von SHA256](https://github.com/dotnet/runtime/blob/v6.0.36/src/libraries/System.Security.Cryptography.Algorithms/src/System/Security/Cryptography/SHA256.cs) enthält statische `HashData`-Überladungen für Bytearrays und Spans, keine Überladung für einen Stream. Die Diagnose verwendet deshalb eine `SHA256.Create()`-Instanz und deren `ComputeHash(Stream)` aus HashAlgorithm. So bleibt die Datei-Verarbeitung mit net6.0 vereinbar. Die Korrektur entstand aus statischer Versionsprüfung, nicht aus einem hier ausgeführten Compilerlauf.

## 3. S1API-Lebenszyklus

### E-S1-01 – Lade-, Speicher- und Szenenereignisse

Quelle: [GameLifecycle.cs](https://github.com/ifBars/S1API/blob/a4cbdd9e0ec54895e64fd6be0e91874ba6649c9b/S1API/Lifecycle/GameLifecycle.cs).

`S1API.Lifecycle.GameLifecycle` ist eine öffentliche statische Klasse. Alle folgenden Ereignisse sind `public static event Action?` ohne Argumente:

| Ereignis | Quellseitige Bindung / Bedeutung | Offene Prüfung |
|---|---|---|
| `OnPreLoad` | Weiterleitung von `LoadManager.onPreLoad` | Reihenfolge gegenüber eigenen Save-Daten und Registrierung. |
| `OnLoadComplete` | Weiterleitung von `LoadManager.onLoadComplete` nach Auflösung vorgemerkter Map-Abfragen | Registeraufbau nach vollständigem Laden, Host und Client. |
| `OnPreSceneChange` | Weiterleitung von `LoadManager.onPreSceneChange` | Mehrfacher Menüwechsel, Abbruch eines Ladevorgangs. |
| `OnSaveInfoLoaded` | Weiterleitung von `LoadManager.onSaveInfoLoaded` | Nur Metadatenereignis; kein Save-Ladeabschluss. |
| `OnSaveStart` | Weiterleitung von `SaveManager.onSaveStart`; laut Quellkommentar Host/Server | Tatsächliche Hostautorität vor eigenen Schreibvorgängen bestätigen. |
| `OnSaveComplete` | Weiterleitung von `SaveManager.onSaveComplete`; laut Quellkommentar Host/Server | Kein Beweis, dass eine fremde Moddatei vollständig gespeichert wurde. |

**Aufgelöster Dokumentationswiderspruch:** In dieser Klasse gibt es `OnLoadComplete`. Ein `OnSaveLoaded` ist im festgehaltenen `GameLifecycle.cs` nicht vorhanden. Der Schnellstartname darf nicht übernommen werden.

Die Methoden `Initialize()` und `Reset()` sind `internal`. Die Mod soll sie nicht selbst aufrufen. [S1API.cs](https://github.com/ifBars/S1API/blob/a4cbdd9e0ec54895e64fd6be0e91874ba6649c9b/S1API/S1API.cs) bindet Initialisierung und Rücksetzung an die Szene `Main`. `Reset()` entfernt die nativen Bindungen, löscht jedoch die öffentlichen verwalteten Ereignisse nicht. Eigene Abonnements benötigen weiterhin eine klare eigene Lebensdauer. Die Aufrufreihenfolge im Spiel bleibt zu testen.

## 4. Eingaben und vorhandene Interaktionen

### E-S1-02 – Spielaktionen statt physischer Tasten

Quelle: [Controls.cs](https://github.com/ifBars/S1API/blob/a4cbdd9e0ec54895e64fd6be0e91874ba6649c9b/S1API/Input/Controls.cs).

| Member | Nachgewiesener Vertrag |
|---|---|
| `S1API.Input.ButtonCode.Interact` | Enumwert 12; wird auf die entsprechende native Aktion abgebildet. |
| `S1API.Input.ButtonCode.Sprint` | Enumwert 9; wird auf die entsprechende native Aktion abgebildet. |
| `Controls.GetButton(ButtonCode button)` | `public static bool`; gehaltene Aktion. |
| `Controls.GetButtonDown(ButtonCode button)` | `public static bool`; in diesem Bild gedrückte Aktion. |
| `Controls.GetButtonUp(ButtonCode button)` | `public static bool`; in diesem Bild losgelassene Aktion. |
| `Controls.IsTyping` | `public static bool` mit Getter und Setter. Für die Lichtmod nur lesen. |
| `Controls.CurrentInputDevice` | `InputDeviceType`; enthält `KeyboardMouse` und `Gamepad`. |
| `RegisterDeviceChangedListener(Action<InputDeviceType>)` / `UnregisterDeviceChangedListener(...)` | Vorhandene öffentliche statische Listenerverwaltung. |

Diese Verträge liefern Eingaben. Sie liefern weder den anvisierten logischen Lampeneintrag noch eine automatische Unterdrückung der nativen Schaltung. `IsTyping == false` beweist auch nicht, dass kein Telefon oder Menü offen ist. Sichtprüfung, Reichweite, UI-Kontext, genau eine Ausführung und das Unterdrücken konkurrierender Aktionen bleiben Integrationsaufgaben.

### E-S1-03 – InteractionPrompt besitzt eine konkrete Grenze

Quellen: [InteractionPrompt.cs](https://github.com/ifBars/S1API/blob/a4cbdd9e0ec54895e64fd6be0e91874ba6649c9b/S1API/Interaction/InteractionPrompt.cs), [InteractionPromptRuntime.cs](https://github.com/ifBars/S1API/blob/a4cbdd9e0ec54895e64fd6be0e91874ba6649c9b/S1API/Internal/Interaction/InteractionPromptRuntime.cs).

Die öffentliche Klasse bietet `CreateBuilder(GameObject target)`, unter anderem `SetMessage(string)`, `SetRange(float)`, `SetPriority(int)`, die Ereignisse `Hovered`, `InteractionStarted`, `InteractionEnded` sowie `Remove()` und `Dispose()`.

Der Runtime-Konstruktor prüft ausdrücklich, ob das Ziel bereits eine native `InteractableObject`-Komponente besitzt, und wirft in diesem Fall eine `InvalidOperationException`. Außerdem muss am Ziel oder an dessen Kindern ein Collider vorhanden sein. Die Runtime erzeugt und besitzt ihre eigene Interaktionskomponente.

**Folgerung:** Diese API belegt die Interaktion mit dafür geeigneten eigenen Zielen. Sie belegt keine konfliktfreie Übernahme einer bereits schaltbaren Steh- oder Wandlampe. Dafür werden die tatsächliche Lampenkomponente, vorhandene Colliderzuordnung, Schaltaktion und Eingabereihenfolge benötigt. Ein zusätzliches Kindobjekt als Umgehung ist ohne Sicht-, Prioritäts- und Rückbautest ebenfalls kein nachgewiesener Integrationsweg.

## 5. Platzierung und Objektidentität

### E-S1-04 – BuildEvents

Quellen: [BuildEvents.cs](https://github.com/ifBars/S1API/blob/a4cbdd9e0ec54895e64fd6be0e91874ba6649c9b/S1API/Building/BuildEvents.cs), [BuildEventArgs.cs](https://github.com/ifBars/S1API/blob/a4cbdd9e0ec54895e64fd6be0e91874ba6649c9b/S1API/Building/BuildEventArgs.cs), [BuildingPatches.cs](https://github.com/ifBars/S1API/blob/a4cbdd9e0ec54895e64fd6be0e91874ba6649c9b/S1API/Internal/Patches/BuildingPatches.cs).

- `BuildEvents.OnGridItemCreated`, `OnSurfaceItemCreated`, `OnBuildableItemInitialized`: jeweils `public static event Action<BuildEventArgs>?`.
- `BuildEventArgs.ItemInstance`: `S1API.Items.ItemInstance`, öffentlicher Getter, interner Setter.
- `BuildEventArgs.GameObject`: `UnityEngine.GameObject`, öffentlicher Getter, interner Setter.
- `BuildEventArgs.ItemId`: `public string?`, berechnet als `ItemInstance?.Definition?.ID`.
- `BuildEventArgs.Storage`: optionaler Storage-Wrapper; kein Lampenvertrag.

Die API-Patches hängen an der Erstellung von Grid- und Surface-Items sowie an `BuildableItem.InitializeBuildableItem`. Das ist ein konkreter Quellbeleg der API-Implementierung, kein Nachweis vollständiger Ereignisabdeckung bei Save-Laden oder für alle Netzwerkrollen.

**Instanzidentität bleibt offen:** `ItemId` bezeichnet die Definition. Zwei LED-Lampen dürfen nicht denselben dauerhaften Lampenschlüssel erhalten, nur weil sie dieselbe `ItemId` besitzen. `GameObject.GetInstanceID()` oder ein Wrapperobjekt sind ebenfalls kein belegter dauerhafter Save-Schlüssel. Ein Removal-Ereignis und eine dauerhafte Lampen-ID sind durch diese drei Ereignisse nicht nachgewiesen.

## 6. Immobilien und Räume

### E-S1-05 – Öffentliche Immobilienabfragen

Quellen: [PropertyManager.cs](https://github.com/ifBars/S1API/blob/a4cbdd9e0ec54895e64fd6be0e91874ba6649c9b/S1API/Property/PropertyManager.cs), [PropertyWrapper.cs](https://github.com/ifBars/S1API/blob/a4cbdd9e0ec54895e64fd6be0e91874ba6649c9b/S1API/Property/PropertyWrapper.cs).

| Member | Quellbeleg | Grenze |
|---|---|---|
| `PropertyManager.GetAllProperties()` | `List<PropertyWrapper>` | Erst bei verfügbaren Spielmanagern abfragen. |
| `PropertyManager.GetOwnedProperties()` | `List<PropertyWrapper>` aus nativer Owned-Liste | Gemeinsamer Spielstandbesitz, keine individuelle Spielerberechtigung. |
| `PropertyManager.FindPropertyByName(string name)` | `PropertyWrapper?`; exakter Namensvergleich | Anzeigenamen nicht als alleinigen dauerhaften Schlüssel verwenden. |
| `PropertyWrapper.PropertyName` | `string` | Anzeige. |
| `PropertyWrapper.PropertyCode` | `string` | Nativer Immobiliencode; Kollisionen und Stabilität im Zielbuild prüfen. |
| `PropertyWrapper.IsOwned` | `bool`, nur Getter | Kein Beleg individueller Multiplayerrechte. |
| `PropertyWrapper.IsPointInside(Vector3 point)` | `bool`, delegiert an native Boundsprüfung | Räumlicher Fallback; kein Raum oder Schaltgruppenbeleg. |
| `PropertyWrapper.IsContentCulled` | `bool`, Getter und Setter | Für Diagnose nur lesen; fehlende Darstellung ist nicht automatisch Entfernung. |
| `PropertyWrapper.BuildableItemCount` | `int` | Nur Anzahl; keine öffentliche Lampenliste. |

`InnerProperty` ist ein interner Member. Die geprüfte öffentliche Oberfläche enthält keine allgemeine Raum-API, keine Schalter-Leuchten-Zuordnung und keinen universellen Zugriff auf native Lampenschalter. Die Mod benötigt die reale Objektzuordnung, bevor sie Räume oder Ein/Aus-Aktionen zusichert.

## 7. Telefon-App

### E-S1-06 – Öffentliche Erweiterungspunkte

Quellen: [PhoneApp.cs](https://github.com/ifBars/S1API/blob/a4cbdd9e0ec54895e64fd6be0e91874ba6649c9b/S1API/PhoneApp/PhoneApp.cs), [ExitAction.cs](https://github.com/ifBars/S1API/blob/a4cbdd9e0ec54895e64fd6be0e91874ba6649c9b/S1API/PhoneApp/ExitAction.cs), [interne Registry](https://github.com/ifBars/S1API/blob/a4cbdd9e0ec54895e64fd6be0e91874ba6649c9b/S1API/Internal/Patches/PhoneAppRegistry.cs).

`S1API.PhoneApp.PhoneApp` ist eine öffentliche abstrakte Klasse. Eine App implementiert die geschützten abstrakten String-Eigenschaften `AppName`, `AppTitle`, `IconLabel`, `IconFileName` sowie `protected abstract void OnCreatedUI(GameObject container)`.

Weitere vorhandene Member:

- `protected virtual Sprite? IconSprite` kann ein direktes Icon liefern.
- `protected virtual EOrientation Orientation`; Enum `Horizontal = 0`, `Vertical = 1`.
- `public bool IsOpen()`, `public void OpenApp()`, `public void CloseApp()`.
- `public virtual void Exit(S1API.PhoneApp.ExitAction exit)`; `ExitAction.Used` ist öffentlich les- und schreibbar.
- `protected virtual void OnPhoneClosed()`.
- `public bool SetIconSprite(Sprite sprite)`, `public bool SetIconTexture(Texture2D texture)`.

Die internen Methoden `SpawnUI` und die interne Registry sind keine direkt nutzbaren öffentlichen Registrierungs-APIs. Das vorhandene `OnCreated()` registriert die App intern; wird dieser Hook überschrieben, darf seine Basisinitialisierung nicht verlorengehen. Der dateibasierte Iconweg liest relativ zu `MelonEnvironment.ModsDirectory`. Nur tatsächlich verwendete Icondateien gehören ins Paket. Die UI-Erzeugung benötigt die passenden Unity-Referenzen.

**Offen:** Automatische Entdeckung der eigenen konkreten App, UI-Erzeugung, deutsche und englische Texte, Ressourcenfreigabe bei Szenewechsel und Konsistenz mit der direkten Bedienung wurden noch nicht im Zielspiel ausgeführt. Der Diagnose-Build ist keine Telefon-App.

## 8. Saveable und notwendige zusätzliche Absicherung

### E-S1-07 – Speicherverträge und tatsächliches Fehlerverhalten

Quellen: [Saveable.cs](https://github.com/ifBars/S1API/blob/a4cbdd9e0ec54895e64fd6be0e91874ba6649c9b/S1API/Internal/Abstraction/Saveable.cs), [SaveableField.cs](https://github.com/ifBars/S1API/blob/a4cbdd9e0ec54895e64fd6be0e91874ba6649c9b/S1API/Saveables/SaveableField.cs), [SaveableAutoRegistry.cs](https://github.com/ifBars/S1API/blob/a4cbdd9e0ec54895e64fd6be0e91874ba6649c9b/S1API/Saveables/SaveableAutoRegistry.cs), [GenericSaveables.Patches.cs](https://github.com/ifBars/S1API/blob/a4cbdd9e0ec54895e64fd6be0e91874ba6649c9b/S1API/Internal/Patches/GenericSaveables.Patches.cs).

Öffentliche Verträge:

- `S1API.Internal.Abstraction.Saveable` ist trotz seines Namespace eine öffentliche abstrakte, für Ableitung vorgesehene Basisklasse.
- `public virtual SaveableLoadOrder LoadOrder`; Standard `AfterBaseGame`.
- `protected virtual void OnLoaded()` und `protected virtual void OnSaved()`.
- `SaveableField(string saveName)` markiert Felder.
- `public static bool RequestGameSave()` fordert einen nativen Save an; der zusätzliche Bool-Overload delegiert an die parameterlose Variante. Daraus folgt keine separate unmittelbar-dauerhafte Dateigarantie.

Die generische Registry entdeckt direkte nicht abstrakte `Saveable`-Ableitungen und erstellt sie mit einem parameterlosen Konstruktor. Die Instanzen werden gecacht. Ein beliebiger selbst erzeugter zweiter Saveable ist nicht automatisch der von der Registry gespeicherte Zustand.

Der gespeicherte Pfad wird gebildet als `Modded/Saveables/<einfacher Klassenname>/<SaveableField-Name>.json` im aktiven Saveordner. Der Namespace ist nicht Bestandteil des Typordners. Ein eigener eindeutiger Klassenname verhindert daher mehr als eine eindeutige Namespacewahl allein.

**Konkrete Quellbefunde:**

1. Der Lader liest per `File.ReadAllText` und deserialisiert per Newtonsoft.Json. Fehlende Dateien werden übersprungen; bereits vorhandene Felder werden dabei nicht automatisch zurückgesetzt.
2. Der Schreiber verwendet direkt `File.WriteAllText`. Eine atomische Ersetzung oder Sicherungskopie ist in diesem Pfad nicht vorhanden.
3. Nullfelder führen zum Löschen der zugehörigen Datei.
4. Die übergeordneten Patches fangen Ausnahmen ab und loggen sie. Das bedeutet weder, dass der Datensatz korrekt geladen wurde, noch dass ein folgender Save automatisch gesperrt wird.
5. `OnSaved()` ist eine nachträgliche Benachrichtigung; es ist kein vorangestellter Schutz gegen Überschreiben beschädigter Eingaben.

**Konsequenz für die Implementierung:** Der tatsächliche Persistenzadapter muss Lesefehler und unbekannte Schemaversionen verriegeln, Hostautorität und Saveidentität prüfen sowie den vorherigen Zustand beim Sitzungswechsel kontrolliert zurücksetzen. Ein Kerncodec mit lokalen Serialisierungstests könnte seine eigenen Formatregeln belegen, aber noch nicht Dateischreiben oder dessen Anbindung an S1API. Die endgültige Lösung muss den Save-Patchablauf berücksichtigen; das bloße Ableiten von Saveable erfüllt AP-05 nicht.

## 9. Weitere geprüfte Beobachtungs- und Materialverträge

### E-S1-08 – Vorhandene Materialhilfe ersetzt keinen Lampenadapter

Die zusätzliche Quelle [MaterialHelper.cs](https://github.com/ifBars/S1API/blob/a4cbdd9e0ec54895e64fd6be0e91874ba6649c9b/S1API/Rendering/MaterialHelper.cs) bestätigt öffentliche generische Materialoperationen, darunter `ReplaceMaterials(GameObject, Func<Material, bool>, Action<Material>)`, `SetColor(Material, string, Color)` und `SetFloat(Material, string, float)`. Sie liefert keine Zuordnung einer realen Lampe zu ihren Emissionsslots und keinen universellen Emissionsparameternamen. `ReplaceMaterials` liest `renderer.materials` und erzeugt für Treffer weitere Materialinstanzen. Der Aufruf bei jedem Farbwechsel wäre daher kein geeignetes Ressourcenmodell. Eine spätere Nutzung muss Eigentum, Wiederverwendung und Freigabe der konkret erzeugten Materialien ausdrücklich behandeln.

Der Quellstand von [PlantInstance.cs](https://github.com/ifBars/S1API/blob/a4cbdd9e0ec54895e64fd6be0e91874ba6649c9b/S1API/Growing/PlantInstance.cs) enthält unter anderem lesbare `NormalizedGrowth`, `Quality` und `YieldMultiplier`. Dies sind mögliche Beobachtungswerte für einen späteren kontrollierten Gameplayvergleich, sobald ein regulärer Zugriff auf dieselbe Pflanzeninstanz hergestellt ist. Der Wrapperkonstruktor ist intern. Diese Member belegen weder einen allgemeinen Pflanzenfinder noch einen Lampensteuerungsvertrag.

## 10. Multiplayertransport

### E-NET-01 – Status von S1API

Die geprüften öffentlichen S1API-Lifecycle-, Controls-, Building-, Property-, Phone- und Saveable-Verträge stellen keinen allgemeinen Mod-Nachrichtentransport bereit. Die Projektdatei enthält native FishNet-Referenzen; deren Anwesenheit ist kein öffentlicher S1API-Nachrichtenvertrag. [Player.cs](https://github.com/ifBars/S1API/blob/a4cbdd9e0ec54895e64fd6be0e91874ba6649c9b/S1API/Entities/Player.cs) enthält `PlayerSpawned`, `LocalPlayerSpawned` und `PlayerDespawned`; daraus ergeben sich weder Mod-Erkennung noch ein serialisierbares Nachrichtenprotokoll.

### E-NET-02 – Konkret untersuchter Kandidat SteamNetworkLib 1.6.0

Quellen: [SteamNetworkClient.cs](https://github.com/ifBars/SteamNetworkLib/blob/aea3c25ac200393c905c6a4ce5fca3b44e6538e3/SteamNetworkClient.cs), [Projektdatei](https://github.com/ifBars/SteamNetworkLib/blob/aea3c25ac200393c905c6a4ce5fca3b44e6538e3/SteamNetworkLib.csproj), [Multiplayer-Vertragshinweise](https://github.com/ifBars/SteamNetworkLib/blob/aea3c25ac200393c905c6a4ce5fca3b44e6538e3/docs/multiplayer-compatibility.md).

Die separate Bibliothek hat eine öffentliche Klasse `SteamNetworkLib.SteamNetworkClient : IDisposable` mit folgenden belegten Membern:

| Member | Bedeutung / Grenze |
|---|---|
| `SteamNetworkClient()`; `bool TryInitialize()` | Initialisierung kann fehlschlagen, wenn Steam noch nicht verfügbar ist. |
| `bool IsInitialized`, `bool IsInLobby`, `bool IsHost` | Zustandsabfragen; tatsächliche Zuordnung zur Spielsession muss beobachtet werden. |
| `ulong LocalPlayerId64`, `ulong HostPlayerId64` | Verwaltete IDs. Host-ID folgt dem Besitzer der aktuellen Lobby. |
| `void SetMyData(string key, string value)` | Mögliche Veröffentlichung einer eigenen Mod-/Protokollkennung, nur bei tatsächlich gewählter Integration. |
| `Task<bool> SendMessageToPlayerAsync(CSteamID playerId, P2PMessage message)` | Gezielter P2P-Versand. Der IL2CPP-Build verwendet `Il2CppSteamworks.CSteamID`. |
| `IDisposable SubscribeMessageHandler<T>(Action<T, CSteamID> handler)` | `T : P2PMessage, new()`; löschbare Handlerbindung. |
| `void ProcessIncomingMessages()` | Muss gemäß gewählter Integration bedient werden; Pumpen und Budgets prüfen. |
| `void Dispose()` | Bibliotheksressourcen freigeben. |

Die Quellkonfiguration für IL2CPP referenziert `Il2Cppcom.rlabrecque.steamworks.net.dll` aus der jeweiligen Spiel-Interop-Installation. Der Mono-NuGet-Vertrag von Steamworks ist kein austauschbarer Ersatz.

**Noch keine LightControl-Integration:** Diese Bibliothek wird in der Diagnose nicht benötigt, nicht auf Verdacht gebündelt und nicht als getestet bezeichnet. Vor einer Übernahme fehlen der versionsgleiche Binärbezug, die Bindung an die bestehende Spiellobby, ein eigener Mod-Handshake, Protokollversionierung, gezielter Versand ausschließlich an bestätigte Modteilnehmer, Session- und Snapshotvalidierung, Updatebudgets und reale Steam-Host-/Clienttests einschließlich Vanilla-Teilnehmern. Ein universeller Broadcast an unbekannte Teilnehmer ist kein zulässiger Ersatz für diese Nachweise.

## 11. Referenzpakete und zulässige Buildgrundlage

Quellen: [S1API-Projektdatei](https://github.com/ifBars/S1API/blob/a4cbdd9e0ec54895e64fd6be0e91874ba6649c9b/S1API/S1API.csproj), [NuGet-Workflow](https://github.com/ifBars/S1API/blob/a4cbdd9e0ec54895e64fd6be0e91874ba6649c9b/.github/workflows/publish-nuget.yml), [IL2CPP-Buildworkflow](https://github.com/ifBars/S1API/blob/a4cbdd9e0ec54895e64fd6be0e91874ba6649c9b/.github/workflows/il2cpp-build-check.yml), [S1API-NuGet](https://www.nuget.org/packages/S1API.Forked/3.2.1), [MelonLoader-NuGet](https://www.nuget.org/packages/LavaGang.MelonLoader/0.7.3).

- `LavaGang.MelonLoader` 0.7.3 bietet ein echtes öffentliches net6.0-Paket. Die eigentliche Loaderinstallation bleibt separat. Eine Mod muss dessen DLL und transitive Entwicklungsdateien nicht pauschal mitliefern.
- `S1API.Forked` 3.2.1 ist öffentlich verfügbar, enthält laut Paketmetadaten netstandard2.1 und wird im festgehaltenen Workflow als `MonoMelon` gebaut. Dieses Paket ist kein Bundle generierter Schedule-I-IL2CPP-Referenzen.
- Die öffentliche S1API-Release-ZIP enthält gemäß Releaseworkflow `Mods/S1API.Mono.MelonLoader.dll`, `Mods/S1API.Il2Cpp.MelonLoader.dll` und `Plugins/S1APILoader.MelonLoader.dll`. S1API wird als separat zu installierende Voraussetzung behandelt.
- Die S1API-eigene IL2CPP-Projektdatei benötigt unter anderem `Assembly-CSharp.dll`, `Il2CppScheduleOne.Core.dll`, `Il2Cppmscorlib.dll`, passende Unity-Module und `Il2CppFishNet.Runtime.dll`. Für LightControl ist eine kleinere konkrete Teilmenge nach den tatsächlich implementierten Adaptern festzulegen; die gesamte S1API-Referenzliste ist keine pauschale Nutzeranforderung.
- Der öffentliche CI-Workflow bezieht Spielassemblies aus einem über Repository-Secrets ausgewählten und authentifizierten Assemblies-Repository. Dies ist kein öffentlich freigegebenes Referenzangebot. Es wird nicht versucht, dessen Zugriffskontrolle zu umgehen.

**Ergebnis der Untersuchung:** Ein MelonLoader-basierter Diagnose-Build ist mit öffentlicher Loaderreferenz konkret möglich. In den geprüften offiziellen Angeboten wurde kein vollständiges öffentliches, zur Nutzerinstallation passendes Schedule-I-IL2CPP-Referenzpaket nachgewiesen, das alle Lampen-, Material-, Eingabe- und Netzwerkadapter ohne weitere Spielreferenzen ermöglicht. Die kleine Diagnose und unabhängig prüfbare Kernlogik sind sinnvolle Zwischenstände. Sie ersetzen nicht das beauftragte fertig installierbare Lichtsteuerungsrelease.

### Öffentliche Assets mit vom Anbieter angegebenem SHA-256

Diese Werte stammen aus der GitHub-Release-Metadatenantwort. Sie sind nicht mit einer in dieser Quellanalyse selbst vorgenommenen Binärprüfung zu verwechseln. Ein Buildbericht muss zusätzlich den Hash der tatsächlich verwendeten lokalen Datei nennen.

| Asset | Anbieter-SHA-256 |
|---|---|
| [MelonLoader.x64.zip 0.7.3](https://github.com/LavaGang/MelonLoader/releases/download/v0.7.3/MelonLoader.x64.zip) | `5b2b2f3d1cd42b59ec886c5bdc2663edae87a0097a4f4a8f58c0965a99dda416` |
| [S1API-Forked-3.2.1.zip](https://github.com/ifBars/S1API/releases/download/v3.2.1/S1API-Forked-3.2.1.zip) | `9670a492939797b871d56ef62b6c41e3fa70357d90eccabb066ea70c9c6f5bf3` |
| [SteamNetworkLib-IL2Cpp.dll 1.6.0, nur Kandidat](https://github.com/ifBars/SteamNetworkLib/releases/download/v1.6.0/SteamNetworkLib-IL2Cpp.dll) | `244705ac80a61a79f1b6a253ab059d8763c060496b09d94de3d0f1c4fc7732a3` |

## 12. Noch zu erhebende Integrationsnachweise

| Bereich | Genau fehlender Nachweis | Nächste verwertbare Eingabe |
|---|---|---|
| Zielinstallation | Gestarteter Spielbuild, IL2CPP, Loader/API, Unity, Pipeline | Relevanter Startlog und Diagnosebericht. |
| Pflanzenlampe | Reale Komponenten, alle Lichtquellen, Materialslots, Shaderparameter, native Aktualisierung | Bericht zu einer platzierten Pflanzenlampe, Ein/Aus-Beobachtung und Screenshot. |
| Schaltbare Lampe | Interaktionsbesitzer, konkrete Schaltmethode, Modifier-Konflikt | Bericht zu Steh-/Wandlampe und beobachtete Eingabereihenfolge. |
| Weltleuchte | Dynamische oder gebackene Anteile, Identität, Culling | Bericht zu einer festen Leuchte, Tageszeit-/Entfernungstest. |
| Dauerhafte Identität | Über Save/Laden stabile Instanzkennung oder überprüfbarer Ersatz | Zwei identische Lampen vor/nach Bewegen, Aufnehmen und Reload. |
| Räume/Schaltgruppen | Tatsächlich vorhandene Schalterzuordnung | Komponenten-/Verknüpfungsbeobachtung einer Immobilie. |
| Visueller Adapter | Originalbasis, Emission, Flackern, Rückbau, Gameplayvergleich | Passende Interop-Referenzen und gezielter Spieltest. |
| Save-Anbindung | Hostschutz, Savepfad, Lesefehlerverriegelung, Schemawechsel | Zwei Testspielstände und kontrollierter beschädigter Moddatensatz. |
| Transport | Vorhandene Modteilnehmer, Lobbybindung, echte Paketzustellung | Zwei Modteilnehmer plus Test mit Vanilla-Teilnehmer. |

Bis diese Belege vorliegen, erhalten die betroffenen Anforderungen den Status offen. Keine dieser Zeilen wird durch einen abgefangenen Fehler, einen Stub oder einen lokalen Protokolltest als erfüllt markiert.
