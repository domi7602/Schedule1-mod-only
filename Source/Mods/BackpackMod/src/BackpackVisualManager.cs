using Il2CppScheduleOne.AvatarFramework;
using Il2CppScheduleOne.Clothing;
using Il2CppScheduleOne.DevUtilities;
using Il2CppScheduleOne.PlayerScripts;
using Il2CppScheduleOne.UI;
using UnityEngine;
using System;

namespace BackpackMod
{
    public static class BackpackVisualManager
    {
        private static GameObject? _currentWorldVisualObj;
        private static GameObject? _currentMenuVisualObj;
        private static string? _currentActiveTier;
        private static Avatar? _cachedWorldAvatar;
        private static float _lastAvatarSearchTime = -10f;

        public static void UpdateVisuals(AssetBundle? bundle = null)
        {
            try
            {
                var playerMovement = PlayerSingleton<PlayerMovement>.Instance;
                var pc = playerMovement != null && playerMovement.Pointer != IntPtr.Zero
                    ? playerMovement.GetComponent<PlayerClothing>()
                    : null;

                // 1. STRICT EQUIPPED CHECK: ONLY show backpack if equipped in Clothing Slot 10
                string? activeTier = null;
                var wornSlot = Patches.PlayerClothingPatch.BackpackSlot;

                if (wornSlot == null && pc != null && pc.ClothingSlots != null && pc.ClothingSlots.ContainsKey((EClothingSlot)10))
                {
                    wornSlot = pc.ClothingSlots[(EClothingSlot)10];
                }

                if (wornSlot != null && wornSlot.ItemInstance != null && wornSlot.ItemInstance.Definition != null)
                {
                    string wornId = wornSlot.ItemInstance.Definition.ID;
                    if (wornId == "backpack_t3") activeTier = "Tier3_Backpack";
                    else if (wornId == "backpack_t2") activeTier = "Tier2_Backpack";
                    else if (wornId == "backpack_t1") activeTier = "Tier1_Backpack";
                }

                // If nothing is equipped in slot 10, activeTier remains null

                // If tier changed or unequipped, clear old visuals
                if (activeTier != _currentActiveTier)
                {
                    Clear();
                    _currentActiveTier = activeTier;
                }

                if (activeTier == null) return;

                // A. Update World Player Visual — robust avatar/bone resolution for first-person & V (third-person)
                if (playerMovement != null && playerMovement.Pointer != IntPtr.Zero)
                {
                    var avatar = FindWorldAvatar(playerMovement);
                    Transform attachBone = FindBackBone(playerMovement.transform, avatar);
                    bool isFallbackRoot = attachBone == playerMovement.transform;

                    if (_currentWorldVisualObj == null && attachBone != null)
                    {
                        // Don't create if we only have root fallback and avatar still missing — retry next frame instead of spawning at feet
                        if (isFallbackRoot && avatar == null)
                        {
                            Mod.Log?.Msg($"[Backpack] World avatar not yet resolved — retry next frame (tried PlayerMovement children + global scan).");
                        }
                        else
                        {
                            // For world visuals force Default layer (0) so backpack is visible in both first-person and V third-person
                            // (inheriting Player layer 8/9 makes it culled by first-person camera)
                            _currentWorldVisualObj = CreateBackpackVisualInstance(attachBone, playerMovement.transform, activeTier, forceDefaultLayer: true);
                        }
                    }
                }

                // B. Update Character Screen Mannequin Visual
                var charDisplay = Singleton<CharacterDisplay>.Instance;
                if (charDisplay != null && charDisplay.Pointer != IntPtr.Zero && charDisplay.IsOpen)
                {
                    Avatar? menuAvatar = charDisplay.Avatar ?? charDisplay.ParentAvatar;
                    Transform attachBone = FindBackBone(charDisplay.transform, menuAvatar);

                    if (_currentMenuVisualObj == null && attachBone != null)
                    {
                        _currentMenuVisualObj = CreateBackpackVisualInstance(attachBone, charDisplay.transform, activeTier, forceDefaultLayer: false);
                    }
                }
                else if (charDisplay != null && !charDisplay.IsOpen && _currentMenuVisualObj != null)
                {
                    UnityEngine.Object.Destroy(_currentMenuVisualObj);
                    _currentMenuVisualObj = null;
                }
            }
            catch (Exception ex)
            {
                Mod.Log?.Error($"Error in BackpackVisualManager.UpdateVisuals: {ex.Message}");
            }
        }

        private static Avatar? FindWorldAvatar(PlayerMovement pm)
        {
            if (_cachedWorldAvatar != null && _cachedWorldAvatar.Pointer != IntPtr.Zero && !_cachedWorldAvatar.WasCollected)
                return _cachedWorldAvatar;

            if (Time.unscaledTime - _lastAvatarSearchTime < 0.5f) return _cachedWorldAvatar;
            _lastAvatarSearchTime = Time.unscaledTime;

            // 1. Direct child of PlayerMovement (most common)
            try
            {
                var a = pm.GetComponentInChildren<Avatar>(true);
                if (a != null && a.Pointer != IntPtr.Zero && !a.WasCollected) { _cachedWorldAvatar = a; return a; }
            }
            catch { }

            // 2. Via Player singleton (Schedule I has Player.Local)
            try
            {
                var player = Il2CppScheduleOne.PlayerScripts.Player.Local;
                if (player != null && player.Pointer != IntPtr.Zero && !player.WasCollected)
                {
                    var a2 = player.GetComponentInChildren<Avatar>(true);
                    if (a2 != null && a2.Pointer != IntPtr.Zero && !a2.WasCollected) { _cachedWorldAvatar = a2; return a2; }
                }
            }
            catch { }

            // 3. Global scan — pick Avatar that is descendant of PlayerMovement or closest to it (throttled 0.5s)
            try
            {
                var all = Resources.FindObjectsOfTypeAll<Avatar>();
                if (all != null && all.Length > 0)
                {
                    Avatar? best = null;
                    float bestDist = float.MaxValue;
                    for (int i = 0; i < all.Length; i++)
                    {
                        var av = all[i];
                        if (av == null || av.Pointer == IntPtr.Zero || av.WasCollected) continue;
                        if (av.transform == null) continue;
                        // Prefer child of PlayerMovement
                        if (av.transform.IsChildOf(pm.transform)) { _cachedWorldAvatar = av; return av; }
                        try
                        {
                            float d = Vector3.Distance(av.transform.position, pm.transform.position);
                            if (d < bestDist && d < 2.0f)
                            {
                                bestDist = d;
                                best = av;
                            }
                        }
                        catch { }
                    }
                    if (best != null) { _cachedWorldAvatar = best; return best; }
                }
            }
            catch { }

            return _cachedWorldAvatar;
        }

        private static GameObject CreateBackpackVisualInstance(Transform bone, Transform root, string tierName, bool forceDefaultLayer = false)
        {
            var rootVisual = new GameObject($"Backpack_Worn_{tierName}");
            rootVisual.transform.SetParent(bone, false);

            // Rig bone scale compensation
            Vector3 boneScale = bone.lossyScale;
            float bx = Mathf.Abs(boneScale.x) > 0.0001f ? Mathf.Abs(boneScale.x) : 1f;
            float by = Mathf.Abs(boneScale.y) > 0.0001f ? Mathf.Abs(boneScale.y) : 1f;
            float bz = Mathf.Abs(boneScale.z) > 0.0001f ? Mathf.Abs(boneScale.z) : 1f;

            // 1. Check if user provided a custom 3D model from Blender (.obj)
            var customMesh = ObjLoader.TryGetOrLoadBackpackMesh(tierName);
            if (customMesh != null)
            {
                var customObj = new GameObject("CustomBlenderMesh");
                customObj.transform.SetParent(rootVisual.transform, false);
                customObj.transform.localPosition = Vector3.zero;
                customObj.transform.localRotation = Quaternion.identity;
                customObj.transform.localScale = new Vector3(1f / bx, 1f / by, 1f / bz);

                var mf = customObj.AddComponent<MeshFilter>();
                mf.sharedMesh = customMesh;

                var mr = customObj.AddComponent<MeshRenderer>();
                ApplyMaterial(customObj, root, GetTierMainColor(tierName), false);

                int layer = forceDefaultLayer ? 0 : bone.gameObject.layer;
                SetLayerRecursively(rootVisual, layer);
                Mod.Log?.Msg($"Successfully mounted custom Blender mesh for '{tierName}' on '{bone.name}' (layer {layer}{(forceDefaultLayer?" forced Default":"")}).");
                return rootVisual;
            }

            // 2. Default Realistic Multi-Part Visual (Main Pack + Straps + Buckles + Pockets)
            Vector3 mainSize;
            Vector3 pocketSize;
            Vector3 mainOffset;
            Color mainColor = GetTierMainColor(tierName);
            Color strapColor;
            Color buckleColor = new Color(0.85f, 0.85f, 0.90f); // Metallic chrome/silver for buckles

            switch (tierName)
            {
                case "Tier1_Backpack": // Small Everyday Pack (Navy Blue / Slate Straps)
                    mainSize = new Vector3(0.095f, 0.145f, 0.055f);
                    pocketSize = new Vector3(0.070f, 0.065f, 0.020f);
                    mainOffset = new Vector3(0f, -0.015f, -0.060f);
                    strapColor = new Color(0.14f, 0.16f, 0.22f);
                    break;

                case "Tier2_Backpack": // Hiking Daypack (Forest Green / Dark Olive Straps)
                    mainSize = new Vector3(0.110f, 0.170f, 0.068f);
                    pocketSize = new Vector3(0.080f, 0.080f, 0.024f);
                    mainOffset = new Vector3(0f, -0.020f, -0.065f);
                    strapColor = new Color(0.16f, 0.22f, 0.15f);
                    break;

                case "Tier3_Backpack": // Tactical / Heavy Duffle (Crimson / Tactical Black Straps)
                default:
                    mainSize = new Vector3(0.125f, 0.195f, 0.080f);
                    pocketSize = new Vector3(0.090f, 0.090f, 0.028f);
                    mainOffset = new Vector3(0f, -0.025f, -0.070f);
                    strapColor = new Color(0.10f, 0.10f, 0.12f);
                    break;
            }

            // Apply inverse bone scale to root visual position
            rootVisual.transform.localPosition = new Vector3(mainOffset.x / bx, mainOffset.y / by, mainOffset.z / bz);
            rootVisual.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            rootVisual.transform.localScale = Vector3.one;

            // A. MAIN PACK BODY (On upper back)
            var mainBody = CreateSubPart(rootVisual, "MainCompartment",
                new Vector3(mainSize.x / bx, mainSize.y / by, mainSize.z / bz),
                Vector3.zero,
                Quaternion.identity);
            ApplyMaterial(mainBody, root, mainColor, false);

            // B. FRONT ZIPPER POCKET (Outer pocket on the rear)
            var frontPocket = CreateSubPart(rootVisual, "FrontPocket",
                new Vector3(pocketSize.x / bx, pocketSize.y / by, pocketSize.z / bz),
                new Vector3(0f, (-mainSize.y * 0.18f) / by, (-mainSize.z * 0.52f) / bz),
                Quaternion.identity);
            ApplyMaterial(frontPocket, root, strapColor, false);

            // C. TOP GRAB HANDLE
            var topHandle = CreateSubPart(rootVisual, "TopHandle",
                new Vector3((mainSize.x * 0.32f) / bx, 0.012f / by, 0.016f / bz),
                new Vector3(0f, (mainSize.y * 0.52f) / by, 0f),
                Quaternion.identity);
            ApplyMaterial(topHandle, root, strapColor, false);

            // D. LEFT & RIGHT SIDE POCKETS (Water bottle / utility slots)
            float sideWidth = 0.012f;
            float sideHeight = mainSize.y * 0.40f;
            float sideDepth = mainSize.z * 0.65f;

            var leftSidePocket = CreateSubPart(rootVisual, "LeftSidePocket",
                new Vector3(sideWidth / bx, sideHeight / by, sideDepth / bz),
                new Vector3((mainSize.x * 0.50f + sideWidth * 0.45f) / bx, (-mainSize.y * 0.18f) / by, 0f),
                Quaternion.identity);
            ApplyMaterial(leftSidePocket, root, strapColor, false);

            var rightSidePocket = CreateSubPart(rootVisual, "RightSidePocket",
                new Vector3(sideWidth / bx, sideHeight / by, sideDepth / bz),
                new Vector3((-mainSize.x * 0.50f - sideWidth * 0.45f) / bx, (-mainSize.y * 0.18f) / by, 0f),
                Quaternion.identity);
            ApplyMaterial(rightSidePocket, root, strapColor, false);

            // E. SHOULDER HARNESS & STRAPS (Wrap over the shoulders down to the chest)
            float shoulderSpacing = 0.034f; // Tailored distance from spine to shoulder strap center
            float strapThick = 0.008f;
            float strapWidth = 0.016f;

            // 1. Left Shoulder Arch (Curves over left shoulder: Back -> Top -> Front)
            var leftShoulderTop = CreateSubPart(rootVisual, "LeftShoulderArch",
                new Vector3(strapWidth / bx, strapThick / by, 0.090f / bz),
                new Vector3(shoulderSpacing / bx, (mainSize.y * 0.46f) / by, 0.020f / bz),
                Quaternion.Euler(-12f, 0f, 0f));
            ApplyMaterial(leftShoulderTop, root, strapColor, false);

            // 2. Left Front Chest Strap (Runs down along the left chest)
            var leftChestStrap = CreateSubPart(rootVisual, "LeftChestStrap",
                new Vector3(strapWidth / bx, 0.110f / by, strapThick / bz),
                new Vector3(shoulderSpacing / bx, (mainSize.y * 0.10f) / by, 0.055f / bz),
                Quaternion.Euler(5f, 0f, -2f));
            ApplyMaterial(leftChestStrap, root, strapColor, false);

            // 3. Right Shoulder Arch (Curves over right shoulder)
            var rightShoulderTop = CreateSubPart(rootVisual, "RightShoulderArch",
                new Vector3(strapWidth / bx, strapThick / by, 0.090f / bz),
                new Vector3(-shoulderSpacing / bx, (mainSize.y * 0.46f) / by, 0.020f / bz),
                Quaternion.Euler(-12f, 0f, 0f));
            ApplyMaterial(rightShoulderTop, root, strapColor, false);

            // 4. Right Front Chest Strap (Runs down along the right chest)
            var rightChestStrap = CreateSubPart(rootVisual, "RightChestStrap",
                new Vector3(strapWidth / bx, 0.110f / by, strapThick / bz),
                new Vector3(-shoulderSpacing / bx, (mainSize.y * 0.10f) / by, 0.055f / bz),
                Quaternion.Euler(5f, 0f, 2f));
            ApplyMaterial(rightChestStrap, root, strapColor, false);

            // F. CHEST STERNUM CROSS-STRAP & CENTER BUCKLE (Connects left and right shoulder straps)
            float sternumY = (mainSize.y * 0.14f) / by;
            float sternumZ = 0.058f / bz;

            var sternumStrap = CreateSubPart(rootVisual, "SternumCrossStrap",
                new Vector3((shoulderSpacing * 2.0f) / bx, 0.010f / by, 0.006f / bz),
                new Vector3(0f, sternumY, sternumZ),
                Quaternion.identity);
            ApplyMaterial(sternumStrap, root, strapColor, false);

            // Center Sternum Clasp / Buckle (Metallic center clip)
            var sternumBuckle = CreateSubPart(rootVisual, "SternumBuckleClasp",
                new Vector3(0.016f / bx, 0.012f / by, 0.009f / bz),
                new Vector3(0f, sternumY, sternumZ + (0.003f / bz)),
                Quaternion.identity);
            ApplyMaterial(sternumBuckle, root, buckleColor, true);

            // G. STRAP TENSION SLIDE BUCKLES (Metal buckles on left and right lower straps)
            float lowerBuckleY = (mainSize.y * 0.02f) / by;
            float lowerBuckleZ = 0.058f / bz;

            var leftSlideBuckle = CreateSubPart(rootVisual, "LeftSlideBuckle",
                new Vector3(0.018f / bx, 0.008f / by, 0.008f / bz),
                new Vector3(shoulderSpacing / bx, lowerBuckleY, lowerBuckleZ),
                Quaternion.identity);
            ApplyMaterial(leftSlideBuckle, root, buckleColor, true);

            var rightSlideBuckle = CreateSubPart(rootVisual, "RightSlideBuckle",
                new Vector3(0.018f / bx, 0.008f / by, 0.008f / bz),
                new Vector3(-shoulderSpacing / bx, lowerBuckleY, lowerBuckleZ),
                Quaternion.identity);
            ApplyMaterial(rightSlideBuckle, root, buckleColor, true);

            int finalLayer = forceDefaultLayer ? 0 : bone.gameObject.layer;
            SetLayerRecursively(rootVisual, finalLayer);

            Mod.Log?.Msg($"Created realistic 3D backpack harness on '{bone.name}' (layer: {finalLayer}{(forceDefaultLayer?" forced Default":"")}, tier: {tierName})");
            return rootVisual;
        }

        private static Color GetTierMainColor(string tier)
        {
            return tier switch
            {
                "Tier1_Backpack" => new Color(0.18f, 0.38f, 0.72f), // Royal Blue
                "Tier2_Backpack" => new Color(0.20f, 0.55f, 0.30f), // Olive Forest Green
                "Tier3_Backpack" => new Color(0.65f, 0.15f, 0.15f), // Crimson Tactical
                _ => Color.gray
            };
        }

        private static GameObject CreateSubPart(GameObject parent, string name, Vector3 localScale, Vector3 localPos, Quaternion localRot)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.name = name;
            go.transform.SetParent(parent.transform, false);
            go.transform.localScale = localScale;
            go.transform.localPosition = localPos;
            go.transform.localRotation = localRot;

            var col = go.GetComponent<Collider>();
            if (col != null)
            {
                UnityEngine.Object.Destroy(col);
            }

            return go;
        }

        private static void ApplyMaterial(GameObject obj, Transform root, Color color, bool isMetallic)
        {
            try
            {
                Shader? targetShader = null;

                // 1. Try finding shader from active root avatar renderers
                var renderersOnRoot = root.GetComponentsInChildren<Renderer>(true);
                foreach (var pr in renderersOnRoot)
                {
                    if (pr == null || pr.Pointer == IntPtr.Zero || pr.WasCollected) continue;
                    try
                    {
                        var sharedMat = pr.sharedMaterial;
                        if (sharedMat != null && sharedMat.Pointer != IntPtr.Zero && !sharedMat.WasCollected && sharedMat.shader != null)
                        {
                            string sName = sharedMat.shader.name;
                            if (!string.IsNullOrEmpty(sName) && !sName.Contains("Error") && !sName.Contains("InternalError"))
                            {
                                targetShader = sharedMat.shader;
                                break;
                            }
                        }
                    }
                    catch { }
                }

                // 2. Fallback to Universal Render Pipeline/Lit or Standard
                targetShader ??= Shader.Find("Universal Render Pipeline/Lit")
                                ?? Shader.Find("Universal Render Pipeline/Simple Lit")
                                ?? Shader.Find("Standard");

                var r = obj.GetComponent<Renderer>();
                if (r != null)
                {
                    r.enabled = true;
                    r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                    r.receiveShadows = true;

                    if (targetShader != null)
                    {
                        var mat = new Material(targetShader);
                        if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", color);
                        if (mat.HasProperty("_Color")) mat.SetColor("_Color", color);

                        if (isMetallic)
                        {
                            if (mat.HasProperty("_Smoothness")) mat.SetFloat("_Smoothness", 0.75f);
                            if (mat.HasProperty("_Metallic")) mat.SetFloat("_Metallic", 0.85f);
                        }
                        else
                        {
                            if (mat.HasProperty("_Smoothness")) mat.SetFloat("_Smoothness", 0.25f);
                            if (mat.HasProperty("_Metallic")) mat.SetFloat("_Metallic", 0.05f);
                        }

                        r.sharedMaterial = mat;
                    }
                }
            }
            catch (Exception ex)
            {
                Mod.Log?.Error($"Error in ApplyMaterial: {ex.Message}");
            }
        }

        private static void SetLayerRecursively(GameObject obj, int layer)
        {
            if (obj == null) return;
            obj.layer = layer;
            foreach (Transform child in obj.transform)
            {
                if (child != null)
                {
                    SetLayerRecursively(child.gameObject, layer);
                }
            }
        }

        private static Transform FindBackBone(Transform root, Avatar? avatar)
        {
            if (avatar != null && avatar.MiddleSpine != null && avatar.MiddleSpine.Pointer != IntPtr.Zero) return avatar.MiddleSpine;
            if (avatar != null && avatar.LowerSpine != null && avatar.LowerSpine.Pointer != IntPtr.Zero) return avatar.LowerSpine;
            if (avatar != null && avatar.BodyContainer != null && avatar.BodyContainer.Pointer != IntPtr.Zero) return avatar.BodyContainer;
            if (avatar != null && avatar.Armature != null && avatar.Armature.Pointer != IntPtr.Zero)
            {
                // Fallback: search under Armature hierarchy for spine
                try
                {
                    var armChildren = avatar.Armature.GetComponentsInChildren<Transform>(true);
                    Transform? armFallback = null;
                    foreach (var t in armChildren)
                    {
                        if (t == null || t.Pointer == IntPtr.Zero) continue;
                        string name = t.name.ToLower();
                        if (name.Contains("spine2") || name.Contains("chest") || name.Contains("upperchest"))
                            return t;
                        if (name.Contains("spine") || name.Contains("spine1"))
                            armFallback = t;
                    }
                    if (armFallback != null) return armFallback;
                }
                catch { }
            }

            Transform? fallbackSpine = null;
            try
            {
                var transforms = root.GetComponentsInChildren<Transform>(true);
                if (transforms != null)
                {
                    for (int i = 0; i < transforms.Length; i++)
                    {
                        var t = transforms[i];
                        if (t == null || t.Pointer == IntPtr.Zero) continue;
                        string name = t.name.ToLower();
                        if (name.Contains("spine2") || name.Contains("chest") || name.Contains("upperchest"))
                        {
                            return t;
                        }
                        if (name.Contains("spine") || name.Contains("spine1"))
                        {
                            fallbackSpine = t;
                        }
                    }
                }
            }
            catch { }

            return fallbackSpine ?? root;
        }

        public static void Clear()
        {
            if (_currentWorldVisualObj != null)
            {
                UnityEngine.Object.Destroy(_currentWorldVisualObj);
                _currentWorldVisualObj = null;
            }
            if (_currentMenuVisualObj != null)
            {
                UnityEngine.Object.Destroy(_currentMenuVisualObj);
                _currentMenuVisualObj = null;
            }
            _currentActiveTier = null;
            _cachedWorldAvatar = null;
            _lastAvatarSearchTime = -10f;
        }
    }
}
