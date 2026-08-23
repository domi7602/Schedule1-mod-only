# Universal Render Pipeline (URP) & Material-System in Schedule I

Dieses Dokument beschreibt die Shader-Architektur von *Schedule I* (v0.4.6f13), wie man Texturen und Materialien zur Laufzeit korrekt zuweist und wie man typische Renderfehler (pinke Shader, fehlender Glanz, Material-Leaks) vermeidet.

---

## 1. Warum Shader pink werden (*The Pink Shader Problem*)

In Unity 2022.3 LTS nutzt *Schedule I* die **Universal Render Pipeline (URP)**.
* Wenn eine Mod versucht, den alten Built-in Shader `"Standard"` oder `"Legacy Shaders/Diffuse"` zu verwenden, kann die URP-Render-Pipeline diesen nicht interpretieren und rendert das Objekt in grellem **Magenta/Pink**.

### Die korrekte Shader-Auflösung in C#:

```csharp
public static Shader GetUrpLitShader()
{
    // 1. Primärer URP Lit Shader
    Shader shader = Shader.Find("Universal Render Pipeline/Lit");
    if (shader != null) return shader;

    // 2. Fallbacks für Unlit oder Vanilla Asset Shader
    shader = Shader.Find("Universal Render Pipeline/Unlit");
    if (shader != null) return shader;

    shader = Shader.Find("Universal Render Pipeline/Simple Lit");
    if (shader != null) return shader;

    // 3. Letzter Notfall-Fallback
    return Shader.Find("Standard");
}
```

---

## 2. PBR Material Properties in URP

Ein URP Lit Material steuert sein Aussehen über standardisierte Shader-Properties:

| Property Name | Datentyp | Beschreibung |
|---|---|---|
| `_BaseColor` | `Color` (RGBA) | Hauptfarbe / Tönung des Materials (z. B. `Color(0.2f, 0.5f, 0.2f, 1f)` für Olivgrün). |
| `_BaseMap` | `Texture2D` | Diffuse / Albedo-Texturkarte. |
| `_Metallic` | `float` (0.0 bis 1.0) | 0.0 = Nicht-Metallisch (Stoff, Holz, Plastik), 1.0 = Vollmetall (Schnallen, Stahl). |
| `_Smoothness` | `float` (0.0 bis 1.0) | 0.0 = Matt / Rau, 1.0 = Hochglanz / Spiegelnd. Für Stoff empfiehlt sich $0{,}1\text{--}0{,}3$. |
| `_BumpMap` | `Texture2D` | Normal Map für realistische Oberflächendetails (z. B. Fäden, Falten, Rillen). |
| `_EmissionColor` | `Color` (HDR) | Leuchtfarbe für LED-Elemente, Bildschirme oder leuchtende Streifen. |

### C#-Code-Beispiel für ein stoffartiges Rucksack-Material mit Metallschnallen:

```csharp
Material fabricMat = new Material(GetUrpLitShader());
fabricMat.name = "Backpack_Fabric_Olive";
fabricMat.SetColor("_BaseColor", new Color(0.18f, 0.26f, 0.16f, 1f)); // Olivgrün
fabricMat.SetFloat("_Metallic", 0.0f);   // Stoff ist nicht metallisch
fabricMat.SetFloat("_Smoothness", 0.18f); // Matt / leicht angeraut

Material metalMat = new Material(GetUrpLitShader());
metalMat.name = "Backpack_Buckle_Steel";
metalMat.SetColor("_BaseColor", new Color(0.85f, 0.85f, 0.88f, 1f)); // Silber
metalMat.SetFloat("_Metallic", 0.85f);   // Hoher Metallwert
metalMat.SetFloat("_Smoothness", 0.75f); // Hoher Glanzwert
```

---

## 3. Dynamische Texturen zur Laufzeit laden

Wenn eine Mod eigene `.png`- oder `.jpg`-Bilder aus dem Dateisystem lädt:

```csharp
public static Texture2D LoadTextureFromFile(string filePath)
{
    if (!File.Exists(filePath)) return null;

    byte[] fileData = File.ReadAllBytes(filePath);
    Texture2D texture = new Texture2D(2, 2, TextureFormat.RGBA32, true);
    
    // Unity ImageConversion lädt PNG/JPG in die Textur
    if (ImageConversion.LoadImage(texture, fileData))
    {
        texture.filterMode = FilterMode.Bilinear;
        texture.wrapMode = TextureWrapMode.Repeat;
        return texture;
    }
    return null;
}
```

---

## 4. Speicher-Lecks verhindern (`sharedMaterial` vs `material`)

* **Gefahr:** Ein Aufruf von `renderer.material` erzeugt **jedes Mal eine neue Instanzkopie** des Materials im Grafikspeicher (VRAM). Bei wiederholten Aufrufen in Update-Schleifen führt das zu massiven Memory-Leaks und FPS-Einbrüchen!
* **Die Lösung:**
  * Für reine Farbanpassungen oder Lesezugriffe immer `renderer.sharedMaterial` verwenden.
  * Materialien einmalig zentral erzeugen, in statischen Dictionaries oder Feldern cachen und mehreren Renderern zuweisen.
