# Blender Export & Geometrie-Vorbereitung für Schedule I

Dieses Referenzdokument erklärt, wie man 3D-Modelle in Blender optimal für den Import in *Schedule I* vorbereitet und typische Modellierungsfehler vermeidet.

---

## 1. Das Koordinaten- & Achsen-Problem (Z-Up vs. Y-Up)

Blender und Unity verwenden unterschiedliche Koordinatensysteme:
* **Blender:** Rechtshändiges System, **$+Z$ ist Oben**, $+Y$ ist Vorne.
* **Unity:** Linkshändiges System, **$+Y$ ist Oben**, $+Z$ ist Vorne.

### Der richtige Blender-Export-Standard:

1. **Transformations-Freeze vor dem Export:**
   * Im Objektmodus alles markieren (<kbd>A</kbd>).
   * <kbd>Ctrl+A</kbd> drücken $\rightarrow$ **Apply All Transforms** auswählen (Setzt Rotation auf $(0,0,0)$, Scale auf $(1,1,1)$ und Origin auf $(0,0,0)$).
2. **Export-Einstellungen für Wavefront OBJ (`.obj`):**
   * **Forward:** `-Z Forward`
   * **Up:** `Y Up`
   * **Apply Modifiers:** `Aktiviert`
   * **Write Normals:** `Aktiviert`
   * **Include UVs:** `Aktiviert`
   * **Triangulate Faces:** `Optional (empfohlen für saubere Triangles)`
3. **Export-Einstellungen für GLTF/GLB (`.glb`):**
   * **Format:** `glTF Binary (.glb)` (enthält Texturen und Meshes in einer einzelnen kompakten Datei).
   * **Transform $\rightarrow$ +Y Up:** `Aktiviert`.

---

## 2. Normalen & Backface-Culling (Das "Durchsichtige Mesh"-Problem)

In Unity werden Dreiecke standardmäßig nur von einer Seite gerendert (*Backface Culling*), um Rechenleistung zu sparen. Wenn Normalen nach innen zeigen, ist das Modell im Spiel unsichtbar oder sieht von innen nach außen gestülpt aus.

### Normalen-Check in Blender:
1. Oben rechts im Viewport das Dropdown **Viewport Overlays** öffnen.
2. Den Haken bei **Face Orientation** setzen:
   * 🟦 **Blau:** Die Normale zeigt nach außen (korrekt).
   * 🟥 **Rot:** Die Normale zeigt nach innen (wird in Unity unsichtbar/schwarz!).
3. **Normalen reparieren:**
   * In den Editiermodus wechseln (<kbd>Tab</kbd>).
   * Alle Flächen markieren (<kbd>A</kbd>).
   * <kbd>Shift+N</kbd> drücken (*Recalculate Outside*).

---

## 3. Maßstäbe & Referenz-Dimensionen für Schedule I

Schedule I verwendet reale Meter als Maßeinheit ($1{,}0 = 1\,\text{Meter}$):

| Körperteil / Element | Reale Maße in Schedule I | Empfohlene Blender-Bounding-Box |
|---|---|---|
| **Avatar Gesamthöhe** | $\approx 1{,}75\,\text{m}$ | $Z = 1{,}75\,\text{m}$ |
| **Torso-Breite (Schulter zu Schulter)** | $\approx 0{,}12\text{--}0{,}14\,\text{m}$ | $X = 0{,}14\,\text{m}$ |
| **Rucksack Tier 1 (Small)** | B: $0{,}095\,\text{m}$, H: $0{,}145\,\text{m}$, T: $0{,}055\,\text{m}$ | Liegt flach auf oberem Rücken |
| **Rucksack Tier 2 (Hiking)** | B: $0{,}110\,\text{m}$, H: $0{,}170\,\text{m}$, T: $0{,}068\,\text{m}$ | Sitzt zwischen den Schulterblättern |
| **Rucksack Tier 3 (Tactical)** | B: $0{,}125\,\text{m}$, H: $0{,}195\,\text{m}$, T: $0{,}080\,\text{m}$ | Größeres Packvolumen, max. Torsobreite |
| **Hand-Items / Waffen** | Grifflänge: $\approx 0{,}12\text{--}0{,}15\,\text{m}$ | Pivot-Punkt direkt auf die Handfläche setzen |

---

## 4. Origin & Pivot-Punkt-Platzierung

Der Pivot-Punkt in Blender bestimmt, wie das Objekt im Spiel rotiert und an Knochen befestigt wird:
* **Bei Rucksäcken & Kleidung:**  
  Setze den Pivot-Punkt genau auf die vordere Kontaktfläche mit dem Rücken ($Z = 0$, $Y = 0$, $X = 0$).
* **Bei Waffen / Werkzeugen:**  
  Setze den Pivot-Punkt genau auf die Stelle, an der die Spielfigur den Griff umschließt.
* **Bei Möbeln & Boden-Props:**  
  Setze den Pivot-Punkt exakt auf die Unterkante des Bodens ($Z = 0$), damit das Objekt beim Platzieren nicht im Boden versinkt.
