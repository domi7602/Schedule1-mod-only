# Rigging & Bone Attachment in Schedule I
> verified: static identifier sweep 2026-10-08 against the freshly regenerated f12 decompile (`GameReferences/decompiled/Assembly-CSharp`), replacing the earlier decompile-generation check — 7 of 7 identifier-shaped tokens resolve (0 documented as absent; 0 lowercase parameter tokens are out of scope). Static coverage only; runtime behaviour still needs an in-game session.


This document describes how 3D gear, clothing, backpacks, and hand-held objects are bound to the skeleton of the *Schedule I* player avatar.

---

## 1. The Skeleton Hierarchy of the Schedule I Humanoid Avatar

The player avatar in *Schedule I* uses a standardized humanoid bone structure:

```text
PlayerRoot
└── Model / Armature
    └── Hips
        ├── Spine
        │   └── Spine1
        │       └── Spine2                  <-- Attachment point for backpacks & vests
        │           ├── Neck
        │           │   └── Head            <-- Attachment point for hats, masks, glasses
        │           ├── LeftShoulder
        │           │   └── LeftArm -> LeftForeArm -> LeftHand
        │           └── RightShoulder
        │               └── RightArm -> RightForeArm -> RightHand  <-- Hand items & weapons
        ├── LeftUpLeg -> LeftLeg -> LeftFoot
        └── RightUpLeg -> RightLeg -> RightFoot
```

---

## 2. Safely Finding Bones at Runtime

Because bone paths in the mesh sometimes vary (e.g. due to avatar customization), use a recursive search:

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

### Attaching a backpack to `Spine2`:

```csharp
Transform spine2 = FindBoneRecursive(playerTransform, "Spine2");
if (spine2 != null)
{
    GameObject backpack = GameObject.Instantiate(backpackPrefab);
    backpack.transform.SetParent(spine2, false);
    
    // Position exactly between the shoulder blades
    backpack.transform.localPosition = new Vector3(0f, -0.020f, -0.095f);
    backpack.transform.localRotation = Quaternion.identity;
    backpack.transform.localScale = Vector3.one;
}
```

---

## 3. The Zero-Collider Safety Rule (*Critical!*)

### The Problem:
When a 3D object attached to the player (e.g. a backpack or a hat) still contains active `BoxCollider`, `MeshCollider`, or `CapsuleCollider` components:
1. It blocks the camera raycasts → the player can no longer open doors, talk to NPCs, or pick up items.
2. The cursor in the character or inventory menu gets stuck on the player's own backpack.
3. The physics engine (CharacterController) collides with the player's own backpack and the player flies through the floor or gets stuck in doors.

### The Permanent Solution:
When instantiating clothing or backpacks, **immediately remove all colliders recursively**:

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

## 4. 360° Mannequin Inspection in the Character Menu

In the character menu (<kbd>Tab</kbd>) the game renders a UI doll (*mannequin*). To give the player the ability to inspect new gear from all sides:

* **Mouse-drag rotation:** With the right mouse button held (<kbd>Hold RMB</kbd>), rotate the mannequin transform's Y axis based on `Input.GetAxis("Mouse X")`.
* **Keyboard keys:** Additional rotation via <kbd>Q</kbd> (left) and <kbd>E</kbd> (right).
* **Automatic reset:** When the character menu is closed, smoothly reset the rotation back to `Quaternion.identity`.

---

---
