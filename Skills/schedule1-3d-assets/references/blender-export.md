# Blender Export & Geometry Preparation for Schedule I

This reference document explains how to optimally prepare 3D models in Blender for import into *Schedule I* and how to avoid typical modeling mistakes.

---

## 1. The Coordinate & Axis Problem (Z-Up vs. Y-Up)

Blender and Unity use different coordinate systems:
* **Blender:** Right-handed system, **$+Z$ is up**, $+Y$ is forward.
* **Unity:** Left-handed system, **$+Y$ is up**, $+Z$ is forward.

### The correct Blender export standard:

1. **Freeze transformations before export:**
   * Select everything in object mode (<kbd>A</kbd>).
   * Press <kbd>Ctrl+A</kbd> $\rightarrow$ choose **Apply All Transforms** (sets rotation to $(0,0,0)$, scale to $(1,1,1)$, and origin to $(0,0,0)$).
2. **Export settings for Wavefront OBJ (`.obj`):**
   * **Forward:** `-Z Forward`
   * **Up:** `Y Up`
   * **Apply Modifiers:** `Enabled`
   * **Write Normals:** `Enabled`
   * **Include UVs:** `Enabled`
   * **Triangulate Faces:** `Optional (recommended for clean triangles)`
3. **Export settings for GLTF/GLB (`.glb`):**
   * **Format:** `glTF Binary (.glb)` (contains textures and meshes in a single compact file).
   * **Transform $\rightarrow$ +Y Up:** `Enabled`.

---

## 2. Normals & Backface Culling (The "Transparent Mesh" Problem)

By default, Unity renders triangles only from one side (*Backface Culling*) to save performance. If normals point inward, the model is invisible in-game or looks like it's been turned inside out.

### Normals check in Blender:
1. Open the **Viewport Overlays** dropdown in the top-right of the viewport.
2. Tick **Face Orientation**:
   * 🟦 **Blue:** The normal points outward (correct).
   * 🟥 **Red:** The normal points inward (invisible/black in Unity!).
3. **Repair normals:**
   * Switch to edit mode (<kbd>Tab</kbd>).
   * Select all faces (<kbd>A</kbd>).
   * Press <kbd>Shift+N</kbd> (*Recalculate Outside*).

---

## 3. Scales & Reference Dimensions for Schedule I

Schedule I uses real meters as units ($1.0 = 1\,\text{meter}$):

| Body part / element | Real dimensions in Schedule I | Recommended Blender bounding box |
|---|---|---|
| **Avatar total height** | $\approx 1.75\,\text{m}$ | $Z = 1.75\,\text{m}$ |
| **Torso width (shoulder to shoulder)** | $\approx 0.12\text{--}0.14\,\text{m}$ | $X = 0.14\,\text{m}$ |
| **Backpack Tier 1 (Small)** | W: $0.095\,\text{m}$, H: $0.145\,\text{m}$, D: $0.055\,\text{m}$ | Sits flat on upper back |
| **Backpack Tier 2 (Hiking)** | W: $0.110\,\text{m}$, H: $0.170\,\text{m}$, D: $0.068\,\text{m}$ | Sits between the shoulder blades |
| **Backpack Tier 3 (Tactical)** | W: $0.125\,\text{m}$, H: $0.195\,\text{m}$, D: $0.080\,\text{m}$ | Larger pack volume, max torso width |
| **Hand items / weapons** | Grip length: $\approx 0.12\text{--}0.15\,\text{m}$ | Set pivot point directly on the palm |

---

## 4. Origin & Pivot Point Placement

The pivot point in Blender determines how the object is rotated and attached to bones in-game:
* **For backpacks & clothing:**  
  Set the pivot point exactly on the front contact surface with the back ($Z = 0$, $Y = 0$, $X = 0$).
* **For weapons / tools:**  
  Set the pivot point exactly where the character grips the handle.
* **For furniture & ground props:**  
  Set the pivot point exactly on the bottom edge of the floor ($Z = 0$) so the object doesn't sink into the ground when placed.