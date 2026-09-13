# Rigging & Bone-Attachment in Schedule I

Dieses Dokument beschreibt, wie 3D-Ausrüstung, Kleidung, Rucksäcke und Hand-Objekte an das Skelett des *Schedule I* Spieler-Avatars gebunden werden.

---

## 1. Die Skelett-Hierarchie des Schedule I Humanoid Avatars

Der Spieler-Avatar in *Schedule I* nutzt eine standardisierte Humanoid-Knochenstruktur:

```text
PlayerRoot
└── Model / Armature
    └── Hips
        ├── Spine
        │   └── Spine1
        │       └── Spine2                  <-- Anheftpunkt für Rucksäcke & Westen
        │           ├── Neck
        │           │   └── Head            <-- Anheftpunkt für Hüte, Masken, Brillen
        │           ├── LeftShoulder
        │           │   └── LeftArm -> LeftForeArm -> LeftHand
        │           └── RightShoulder
        │               └── RightArm -> RightForeArm -> RightHand  <-- Hand-Items & Waffen
        ├── LeftUpLeg -> LeftLeg -> LeftFoot
        └── RightUpLeg -> RightLeg -> RightFoot
```

---

## 2. Knochen sicher zur Laufzeit finden

Da Bone-Pfade im Mesh manchmal variieren (z. B. durch Avatar-Customization), verwendet man eine rekursive Suche:

```csharp
public static Transform FindBoneRecursive(Transform current, string boneName)
{
    if (current.name.Equals(boneName, StringComparison.OrdinalIgnoreCase))
        return current;

    for (int i = 0; i < current.childCount; i++)
    {
        Transform found = FindBoneRecursive(current.GetChild(i), boneName);
        if (found != null) return found;
    }
    return null;
}
```

### Anheften eines Rucksacks an `Spine2`:

```csharp
Transform spine2 = FindBoneRecursive(playerTransform, "Spine2");
if (spine2 != null)
{
    GameObject backpack = GameObject.Instantiate(backpackPrefab);
    backpack.transform.SetParent(spine2, false);
    
    // Position exakt zwischen den Schulterblättern
    backpack.transform.localPosition = new Vector3(0f, -0.020f, -0.095f);
    backpack.transform.localRotation = Quaternion.identity;
    backpack.transform.localScale = Vector3.one;
}
```

---

## 3. Die Zero-Collider-Sicherheitsregel (*Kritisch!*)

### Das Problem:
Wenn ein an den Spieler geheftetes 3D-Objekt (z. B. ein Rucksack oder ein Hut) noch aktive `BoxCollider`, `MeshCollider` oder `CapsuleCollider` enthält:
1. Blockiert es die Raycasts der Kamera $\rightarrow$ Der Spieler kann keine Türen mehr öffnen, keine NPCs ansprechen und keine Gegenstände aufheben.
2. Der Mauszeiger im Charakter- oder Inventar-Menü bleibt am eigenen Rucksack hängen.
3. Die Physik-Engine (CharacterController) kollidiert mit dem eigenen Rucksack und der Spieler fliegt durch den Boden oder bleibt in Türen stecken.

### Die dauerhafte Lösung:
Beim Instanziieren von Kleidung oder Rucksäcken **sofort alle Collider rekursiv entfernen**:

```csharp
public static void StripAllColliders(GameObject obj)
{
    Collider[] colliders = obj.GetComponentsInChildren<Collider>(true);
    for (int i = 0; i < colliders.Length; i++)
    {
        UnityEngine.Object.Destroy(colliders[i]);
    }
}
```

---

## 4. 360° Mannequin-Inspektion im Charakter-Menü

Im Charakter-Menü (<kbd>Tab</kbd>) rendert das Spiel eine UI-Puppe (*Mannequin*). Um dem Spieler die Möglichkeit zu geben, neue Ausrüstung von allen Seiten zu betrachten:

* **Maus-Drag-Rotation:** Bei gedrückter rechter Maustaste (<kbd>Hold RMB</kbd>) die Y-Achse des Mannequin-Transforms anhand von `Input.GetAxis("Mouse X")` rotieren.
* **Tastatur-Tasten:** Zusätzliche Rotation über <kbd>Q</kbd> (Links) und <kbd>E</kbd> (Rechts).
* **Automatischer Reset:** Beim Schließen des Charakter-Menüs die Rotation wieder sanft auf `Quaternion.identity` zurücksetzen.
