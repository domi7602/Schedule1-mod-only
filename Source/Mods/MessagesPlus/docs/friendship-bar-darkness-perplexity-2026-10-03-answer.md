## Kurzantwort

Mathematisch lässt sich zeigen: Bei `Image.color=(1,1,1)` und `CanvasRenderer`-Alpha=1 kann **kein** reiner Alpha-/Tint-Mechanismus (CanvasGroup, Overlay, ColorBlock-Multiply) Pixel erzeugen, die dunkler sind als der Hintergrund dahinter – die gemessenen Segmentwerte (2–34) sind aber durchgängig dunkler als Header (17,22,33) und Card (21,29,41). Das spricht stark dafür, dass die Sprite-Texel selbst dunkel sind (unverifizierbar laut DO-NOT-GUESS, aber die einzige verbleibende Erklärung); zum Aufhellen empfehle ich Max-Channel-Normalisierung (Option c) statt Weiß-Overlay oder Gamma.

## Frage 1 – Rangfolge der Mechanismen

**Mathematischer Ausschluss zuerst:** Jede Alpha-Compositing-Operation auf Basis von Weiß (Overlay, CanvasGroup-Alpha, Clipping-Backdrop) hat die Form `out = lerp(quelle, hintergrund_oder_weiß, α)`. Für `Image.color=weiß` kann das Ergebnis nie dunkler als die hellere der beiden Mischpartner sein. Da alle fünf Segmentwerte (z. B. 10,1,3) dunkler sind als sowohl Header (17,22,33) als auch die Card-Lückenfarbe (21,29,41), scheiden **Ancestor-CanvasGroup-Alpha** und **fillRect-Clipping-Backdrop** als Erklärung rechnerisch aus – vorausgesetzt Dump und Screenshot stammen aus demselben Frame/derselben Session (ungeklärte Annahme, im Log nicht garantiert).

Rangfolge (wahrscheinlichste zuerst):

1. **Sprite-Texel selbst ist dunkel** – einzige verbleibende Erklärung, die mit dem Dump konsistent ist. Unverifizierbar (Texeldaten sind laut Vorgabe game-owned), aber durch Ausschluss aller anderen Pfade die plausibelste.
2. **Selectable.ColorTint via `CrossFadeColor`** – betrifft nur `Graphic.color`, und genau dieses Feld wurde im Dump bereits als (1,1,1) gelesen. Für Background/Fill damit zum Messzeitpunkt ausgeschlossen. Bleibt ein Restrisiko nur für den Handle, falls dieser (wie im Standard-Slider-Prefab üblich) ein eigenes `Selectable` mit `targetGraphic`-Referenz trägt – das ist aber eine Annahme über das konkrete Prefab und damit ungeklärt.
3. **Material-/Shader-Tint** – vom Dump nicht erfasst (nur `Image.color` wurde gelesen, nicht `Image.material`). Beim Standard-UI-Shader ist die Multiplikation `Farbe × Textur` monoton, kann also keine Umkehr erzeugen; ein nicht-standard Shader wäre nötig, dafür gibt es keinen Hinweis.
4. **FillRect-Clipping / weitere Ancestor-CanvasGroups** – siehe oben, mathematisch ausgeschlossen, außer Dump und Screenshot sind zeitlich nicht deckungsgleich (Risiko, keine Garantie im Log).

**Zur Sub-Frage, ob ein Stock-`Slider` selbst Farben schreibt:** Nein. Ich habe den Unity-uGUI-Quellcode geprüft – `Slider` manipuliert in seiner Visuals-Aktualisierung ausschließlich `m_FillRect.anchorMin/anchorMax` bzw. `Image.fillAmount` (wenn `fillImage.type == Image.Type.Filled`) sowie die Position/Anchor des `m_HandleRect`. Farbänderungen kommen ausschließlich aus `Selectable.DoStateTransition`, das `targetGraphic.CrossFadeColor(targetColor, …)` aufruft – ein Mechanismus, den `Slider` selbst nicht besitzt (er ist nicht von `Selectable` abgeleitet; nur ein separat verdrahteter Handle-`Selectable` könnte das tun). Nebenbei eine Korrektur: Der im Report genannte Methodenname `UpdateVisualState` existiert so nicht – die tatsächliche Engine-Methode heißt `Selectable.DoStateTransition(SelectionState, bool)`, der Slider selbst nennt seine Methode `UpdateVisuals()`/`Set(...)`.

## Frage 2 – Diskriminierungs-Probe

Minimales Log-Set, günstig vor teuer sortiert, pro Segment einmal pro Konversation:

1. `Image.color` (habt ihr schon) – wenn bereits weiß, sind reine Graphic-Color-Schreibungen (ii, iii) für diesen Frame ausgeschlossen.
2. `canvasRenderer.GetColor()` – das ist der final an die GPU gesendete Wert; ein laufender `CrossFadeColor`-Tween kann zwischenzeitlich hier einen anderen Wert liefern als das `Graphic.color`-Backing-Field. Weicht dieser Wert von `Image.color` ab → Indiz für (ii), einen aktiven oder hängengebliebenen Selectable-Transition.
3. Eigene Buchführung: ein `HashSet<IntPtr>`/Dictionary in `AppTheme`, das bei jedem `TintGraphic`/`SetDark`-Aufruf den `.Pointer` des Ziel-Graphic einträgt. Nachschlagen, ob dieser konkrete Pointer je getintet wurde – das ist der einzige eindeutige Nachweis für (iii), ohne Rückschluss aus Engine-Zustand.
4. `Selectable.colors` (ColorBlock) plus `selectable.targetGraphic.Pointer`-Vergleich gegen Background/Fill/Handle, gesucht über `GetComponentInParent<Selectable>()` bis max. 3 Ebenen hoch. Normal-/Highlighted-Color dunkel und `targetGraphic.Pointer` passt → Konfigurationsnachweis für (ii).
5. `Image.type` und `fillAmount` – nur zur korrekten Interpretation (verhindert Verwechslung von „Fill ist kurz" mit „Fill ist dunkel").
6. Erst wenn 1–4 keine Erklärung liefern: Texel-Sample je Segmentmitte via temporärer `RenderTexture` (`Graphics.Blit` von `sprite.texture` unter Berücksichtigung von `sprite.textureRect`/`textureRectOffset`, dann `ReadPixels` + `GetPixel` auf einer kleinen `Texture2D`). Das ist der einzige direkte Beweis für (i).

IL2CPP-Fallstricke:

- Jede Property einzeln try/catch-geschützt lesen (bereits euer Pattern) – native Objekte können zwischen Reads disposed werden, besonders bei Konversationswechsel.
- Identitätsvergleich über `.Pointer`, nicht `==`/`Equals` auf Wrapper-Objekten (so wie in Snippet B bereits korrekt gemacht) – IL2CPP-Wrapper können bei Neuerzeugung inkonsistente Objektidentität haben.
- `sprite.texture` ist bei atlas-gepackten Sprites standardmäßig `isReadable=false` – `GetPixels()` direkt wirft; der RenderTexture-Blit-Umweg aus Finding 2(b)/(c) ist zwingend, nicht optional.
- `RenderTexture.ReadPixels` muss nach einem tatsächlich gerenderten Frame erfolgen; synchron innerhalb eines Harmony-Patches ausgeführt kann es zu GPU-Sync-Stalls führen – auf nächsten Frame verschieben (Coroutine/`WaitForEndOfFrame` oder einen Scheduler-Callback), nicht inline im Patch.
- Temporäre `RenderTexture` sofort nach dem Lesen freigeben (`RenderTexture.ReleaseTemporary`) – IL2CPP-GC räumt native Render-Targets nicht zuverlässig zeitnah auf, bekannte Leak-Quelle in MelonLoader-Mods.

## Frage 3 – Aufhellungsqualität

Für Pixel (10,1,3)/255 = (0.0392, 0.0039, 0.0118), Zielmaximum ≈ 0.35:

| Methode | Formel | Ergebnis für (10,1,3) | Farbtonerhalt | Kontrast Segment↔Segment |
|---|---|---|---|---|
| (a) Weiß-Overlay | lerp(px, weiß, k) | k≈0,324 nötig → (89,83,85)/255 ≈ Grau | Scheitert: Sättigung bricht fast vollständig ein, Rot wird fast neutral | Overlay liegt gleichmäßig über gefüllt/leer, kein eigener Kontrast möglich |
| (b) Gamma-Lift | px^(1/γ), γ≈2,2 | (60,22,40)/255 | Mittel: Kanalreihenfolge bleibt, Verhältnisse driften leicht | Ordnung zwischen Segmenten bleibt erhalten, leicht komprimiert |
| (c) Max-Channel-Normalisierung | px·(target/max(px)) | (89,9,27)/255 | Exakt: Verhältnis 10:1:3 bleibt erhalten | Alle Segmente auf gleiche Spitzenhelligkeit – gut bei reinem Hue-Code, kann intendierte Helligkeitsunterschiede glätten |

Der explizite Zahlenfehler bei (a): um das Rotsegment überhaupt sichtbar aufzuhellen (k≈0,32), wird Grün von 0,0039 auf 0,326 und Blau von 0,0118 auf 0,332 angehoben – praktisch auf dasselbe Niveau wie Rot (0,350). Das Ergebnis ist nahezu neutralgrau statt rot; genau das verletzt die Vorgabe „rot muss rot bleiben".

**Empfehlung:** (c), weil es als reine Skalarmultiplikation die Kanalverhältnisse exakt invariant lässt – das ist mathematisch der einzige der drei Ansätze mit garantiertem Farbton-/Sättigungserhalt, nicht nur Annäherung. Formel-Vorschlag:

```
scale = clamp(targetMaxChannel / max(r, g, b, ε), 1.0, scaleCap)
rgb' = rgb * scale
```

mit `ε≈1/255` (verhindert Division durch 0 bei reinem Schwarz – solche Pixel sollten unverändert/transparent bleiben statt skaliert zu werden), `targetMaxChannel` deutlich über dem Max-Kanal von Header (0,133) und Card (0,165) gewählt, z. B. 0,45–0,55, und `scaleCap` (z. B. 20–30×), damit Anti-Aliasing-Randpixel mit sehr niedrigem Alpha nicht zu Halo-Artefakten aufgeblasen werden – idealerweise den Skalierungsfaktor zusätzlich mit dem Pixel-Alpha dämpfen.

## Frage 4 – Sprite-Replacement-Mechanik

Für geometrische Identität bei `Image.Type.Filled` (fillAmount-gesteuert) oder `Sliced` muss die Ersatz-Sprite exakt erhalten:

- **rect**-Dimensionen (Pixelgröße innerhalb der Textur) – bestimmt die native Größe und bei `Filled` die Fill-Achsen-Geometrie proportional.
- **textureRect/textureRectOffset** – bei getrimmten Atlas-Sprites verschiebt ein falscher Offset den Fill-Nullpunkt relativ zum Handle.
- **pivot** – falscher Pivot verschiebt die generierte Mesh-Geometrie, besonders kritisch bei `fillOrigin`.
- **pixelsPerUnit** – beeinflusst native Größe und bei `Sliced` die Rand-Skalierung.
- **border** (9-Slice-Ränder) – bei `Sliced` zwingend identisch, sonst verzerren Ecken/Kanten.
- **Mesh-Typ** (`Tight` vs `FullRect`) – für `Image` praktisch irrelevant, da `Image` für Simple/Filled/Sliced immer eine prozedurale Quad-/9-Slice-/Fill-Mesh aus den Rect-Metadaten erzeugt, nicht aus dem Sprite-Mesh selbst (das betrifft nur `SpriteRenderer`). Das ist eine Entwarnung gegenüber einer möglichen Überbesorgnis im Vorschlag.

Probleme bei Atlas-Textur-Wholesale-Blit: `sprite.texture` ist die **ganze Atlas-Seite**, nicht nur die eigene Grafik – ein naiver Blit kopiert fremde Nachbar-Sprites mit; es muss exakt der `textureRect`-Teilbereich geblittet werden. Zusätzlich droht Alpha-Fringe durch bilineares Sampling über die Sprite-Grenze hinweg in Nachbar-Sprites hinein (verstärkt sich nach dem Aufhellen), Filterungs-Mismatch an den Kanten, und ein sRGB/Linear-Mismatch zwischen Quelltextur und `RenderTexture`-Readback kann unbeabsichtigt zusätzlich zum eigentlichen Gamma-/Normalisierungsschritt die Helligkeit verzerren.

**Risikoärmere Alternative:** ein eigenes `Image.material` mit einem simplen Aufhell-Shader (z. B. Screen-Blend `col.rgb = 1-(1-col.rgb)*(1-tint.rgb)`), der die komplette Sprite-/Border-/Pivot-/Trim-Bürokratie umgeht, weil Mesh und Fill-Geometrie unberührt bleiben und nur der Fragment-Output verändert wird. Kosten in einem IL2CPP-Mod ohne eigene Shader-Assets: HLSL/ShaderLab kann im IL2CPP-Player nicht zur Laufzeit kompiliert werden – nötig ist entweder ein vorab in einem zur Ziel-Unity-Version und Grafik-API passenden Editor gebautes `AssetBundle` mit dem Shader (Versions-/API-Kompatibilitätsrisiko, sonst „pink shader"-Fehler), oder Wiederverwendung eines im Spiel bereits vorhandenen Shaders via `Shader.Find` – beides ein echter Zusatzaufwand gegenüber dem texelkopierenden Ansatz, der kein neues Asset benötigt.

---
[Quota] Used 1 Pro Search query (claude_sonnet) | Pro: 56 left | Research: 20 left | WARNING: Pro quota running low — prefer pplx_smart_query(intent='quick') or pplx_sonar for simple lookups

[Conversation ID: b61a7dcc-49af-4da5-a826-63c781b978cb]
