using System;
using Il2CppInterop.Runtime;
using UnityEngine;

namespace AutoPackagingStation.Entities;

/// <summary>
/// Procedural 3D Mesh and Visual Builder for the 4x4 Industrial Auto-Packaging Station.
/// Generates heavy industrial chassis, animated UV-scrolling conveyor, pneumatic compression pistons, and multi-state status LEDs.
/// </summary>
public static class AutoPackMeshBuilder
{
    private static Sprite? _cachedIconSprite;
    private static Texture2D? _cachedIconTexture;
    private static Texture2D? _cachedConveyorTexture;
    private static Material? _cachedBaseMaterial;
    private static Material? _cachedConveyorMaterial;
    private static Material? _cachedHazardMaterial;
    private static Material? _cachedPistonMaterial;

    private static Shader GetSafeShader()
    {
        Shader? s = Shader.Find("Universal Render Pipeline/Lit");
        if (s == null || s.Pointer == IntPtr.Zero)
        {
            s = Shader.Find("Standard");
        }
        if (s == null || s.Pointer == IntPtr.Zero)
        {
            s = Shader.Find("Diffuse");
        }
        if (s == null || s.Pointer == IntPtr.Zero)
        {
            s = Shader.Find("Unlit/Color");
        }
        return s!;
    }

    public static Material GetBaseMaterial()
    {
        if (_cachedBaseMaterial != null && _cachedBaseMaterial.Pointer != IntPtr.Zero) return _cachedBaseMaterial;

        var mat = new Material(GetSafeShader())
        {
            name = "AutoPack_BaseIndustrial_Mat",
            color = new Color(0.18f, 0.20f, 0.23f, 1f)
        };
        try
        {
            mat.SetFloat("_Metallic", 0.85f);
            mat.SetFloat("_Smoothness", 0.40f);
        }
        catch { }

        _cachedBaseMaterial = mat;
        return _cachedBaseMaterial;
    }

    public static Material GetConveyorMaterial()
    {
        if (_cachedConveyorMaterial != null && _cachedConveyorMaterial.Pointer != IntPtr.Zero) return _cachedConveyorMaterial;

        if (_cachedConveyorTexture == null || _cachedConveyorTexture.Pointer == IntPtr.Zero)
        {
            _cachedConveyorTexture = GenerateConveyorTexture();
        }

        var mat = new Material(GetSafeShader())
        {
            name = "AutoPack_ConveyorBelt_Mat",
            mainTexture = _cachedConveyorTexture,
            color = new Color(0.25f, 0.26f, 0.28f, 1f)
        };
        try
        {
            mat.SetFloat("_Metallic", 0.30f);
            mat.SetFloat("_Smoothness", 0.25f);
            mat.mainTextureScale = new Vector2(1f, 4f);
        }
        catch { }

        _cachedConveyorMaterial = mat;
        return _cachedConveyorMaterial;
    }

    public static Material GetHazardMaterial()
    {
        if (_cachedHazardMaterial != null && _cachedHazardMaterial.Pointer != IntPtr.Zero) return _cachedHazardMaterial;

        var mat = new Material(GetSafeShader())
        {
            name = "AutoPack_HazardTrim_Mat",
            color = new Color(0.95f, 0.75f, 0.05f, 1f)
        };
        try
        {
            mat.SetFloat("_Metallic", 0.4f);
            mat.SetFloat("_Smoothness", 0.5f);
        }
        catch { }

        _cachedHazardMaterial = mat;
        return _cachedHazardMaterial;
    }

    public static Material GetPistonMaterial()
    {
        if (_cachedPistonMaterial != null && _cachedPistonMaterial.Pointer != IntPtr.Zero) return _cachedPistonMaterial;

        var mat = new Material(GetSafeShader())
        {
            name = "AutoPack_ChromePiston_Mat",
            color = new Color(0.85f, 0.88f, 0.92f, 1f)
        };
        try
        {
            mat.SetFloat("_Metallic", 0.95f);
            mat.SetFloat("_Smoothness", 0.85f);
        }
        catch { }

        _cachedPistonMaterial = mat;
        return _cachedPistonMaterial;
    }

    public static Material CreateLedMaterialInstance()
    {
        var mat = new Material(GetSafeShader())
        {
            name = "AutoPack_StatusLED_Mat",
            color = new Color(0.1f, 0.8f, 0.2f, 1f)
        };
        try
        {
            mat.EnableKeyword("_EMISSION");
            mat.SetColor("_EmissionColor", new Color(0.2f, 1.0f, 0.3f, 1f) * 2.5f);
        }
        catch { }
        return mat;
    }

    private static Texture2D GenerateConveyorTexture()
    {
        int w = 128;
        int h = 128;
        var tex = new Texture2D(w, h, TextureFormat.RGBA32, false)
        {
            name = "AutoPack_ConveyorTex",
            wrapMode = TextureWrapMode.Repeat,
            filterMode = FilterMode.Bilinear
        };

        var pixels = new Color32[w * h];
        for (int y = 0; y < h; y++)
        {
            // Ribbed rubber conveyor pattern
            byte v = (byte)((y % 16 < 4) ? 40 : (y % 16 < 8) ? 75 : 55);
            byte noise = (byte)((y * 7 + 13) % 15);
            byte finalColor = (byte)Mathf.Clamp(v + noise, 0, 255);

            for (int x = 0; x < w; x++)
            {
                byte border = (x < 6 || x > w - 7) ? (byte)30 : finalColor;
                pixels[y * w + x] = new Color32(border, border, border, 255);
            }
        }

        tex.SetPixels32(pixels);
        tex.Apply(false, true);
        return tex;
    }

    /// <summary>
    /// Builds the full 4x4 industrial station visual hierarchy and links parts to the controller.
    /// </summary>
    public static void BuildStationVisuals(GameObject root, AutoPackStationController controller)
    {
        if (root == null || root.Pointer == IntPtr.Zero) return;

        Transform? existingRoot = root.transform.Find("AutoPack_VisualRoot");
        GameObject visualRoot;
        if (existingRoot != null && existingRoot.Pointer != IntPtr.Zero)
        {
            visualRoot = existingRoot.gameObject;
        }
        else
        {
            visualRoot = new GameObject("AutoPack_VisualRoot");
            visualRoot.transform.SetParent(root.transform, false);
        }

        string modelPath = System.IO.Path.Combine(MelonLoader.Utils.MelonEnvironment.ModsDirectory, "AutoPackagingStation", "model.glb");
        if (System.IO.File.Exists(modelPath))
        {
            try 
            {
                var glbData = System.IO.File.ReadAllBytes(modelPath);
                var glbModel = S1MAPI.Gltf.GltfLoader.LoadGlb(glbData);
                if (glbModel != null && glbModel.Pointer != IntPtr.Zero)
                {
                    glbModel.transform.SetParent(visualRoot.transform, false);
                    
                    // Strip colliders if any on glb meshes, as setup handles them on root
                    var colliders = glbModel.GetComponentsInChildren<Collider>();
                    for (int i = 0; i < colliders.Length; i++) {
                        GameObject.Destroy(colliders[i]);
                    }

                    // Apply URP Lit shader to all GLB renderers to prevent pink materials
                    var urpShader = GetSafeShader();
                    var renderers = glbModel.GetComponentsInChildren<Renderer>();
                    for (int i = 0; i < renderers.Length; i++) {
                        var renderer = renderers[i];
                        if (renderer.material != null) {
                            renderer.material.shader = urpShader;
                        }
                    }
                    
                    // Attempt to find animation nodes by name, fallback to procedural if missing
                    var conveyorBelt = glbModel.transform.Find("AutoPack_ConveyorBelt") ?? glbModel.transform.Find("Station_ConveyorBelt");
                    if (conveyorBelt != null && conveyorBelt.Pointer != IntPtr.Zero) {
                        var renderer = conveyorBelt.GetComponent<Renderer>();
                        if (renderer != null && renderer.Pointer != IntPtr.Zero) controller.RegisterConveyorVisual(renderer, renderer.material);
                    }
                    var piston = glbModel.transform.Find("AutoPack_PneumaticPress_Head") ?? glbModel.transform.Find("PneumaticPress_Head");
                    if (piston != null && piston.Pointer != IntPtr.Zero) controller.RegisterPistonVisual(piston);
                    var led = glbModel.transform.Find("AutoPack_Status_LED_Beacon") ?? glbModel.transform.Find("Status_LED_Beacon");
                    if (led != null && led.Pointer != IntPtr.Zero) {
                        var renderer = led.GetComponent<Renderer>();
                        var light = led.GetComponent<Light>();
                        if (renderer != null && renderer.Pointer != IntPtr.Zero && light != null && light.Pointer != IntPtr.Zero) controller.RegisterLedVisual(renderer, renderer.material, light);
                    }

                    MelonLoader.MelonLogger.Msg("Loaded custom AutoPackagingStation GLB model successfully.");
                    return; // Skip procedural generation
                }
            }
            catch (System.Exception ex)
            {
                MelonLoader.MelonLogger.Warning($"Failed to load GLB model, falling back to procedural meshes. Error: {ex}");
            }
        }

        try
        {
            // 1. Base Platform (2m x 2m x 0.2m industrial chassis)
            var baseGo = GameObject.CreatePrimitive(PrimitiveType.Cube);
            baseGo.name = "AutoPack_BasePlatform";
            baseGo.transform.SetParent(visualRoot.transform, false);
            baseGo.transform.localPosition = new Vector3(0f, 0.10f, 0f);
            baseGo.transform.localScale = new Vector3(2.0f, 0.20f, 2.0f);
            StripCollider(baseGo);
            SetMaterial(baseGo, GetBaseMaterial());

            // Hazard Trims (Front & Back)
            var trimFront = GameObject.CreatePrimitive(PrimitiveType.Cube);
            trimFront.name = "AutoPack_HazardTrim_Front";
            trimFront.transform.SetParent(visualRoot.transform, false);
            trimFront.transform.localPosition = new Vector3(0f, 0.21f, 0.95f);
            trimFront.transform.localScale = new Vector3(2.0f, 0.04f, 0.10f);
            StripCollider(trimFront);
            SetMaterial(trimFront, GetHazardMaterial());

            var trimBack = GameObject.CreatePrimitive(PrimitiveType.Cube);
            trimBack.name = "AutoPack_HazardTrim_Back";
            trimBack.transform.SetParent(visualRoot.transform, false);
            trimBack.transform.localPosition = new Vector3(0f, 0.21f, -0.95f);
            trimBack.transform.localScale = new Vector3(2.0f, 0.04f, 0.10f);
            StripCollider(trimBack);
            SetMaterial(trimBack, GetHazardMaterial());

            // 2. Corner Structural Posts (4 Steel Pillars)
            float[] postX = { -0.92f, 0.92f, -0.92f, 0.92f };
            float[] postZ = { -0.92f, -0.92f, 0.92f, 0.92f };
            for (int i = 0; i < 4; i++)
            {
                var post = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                post.name = $"AutoPack_Pillar_{i + 1}";
                post.transform.SetParent(visualRoot.transform, false);
                post.transform.localPosition = new Vector3(postX[i], 0.70f, postZ[i]);
                post.transform.localScale = new Vector3(0.12f, 0.60f, 0.12f);
                StripCollider(post);
                SetMaterial(post, GetBaseMaterial());
            }

            // 3. Conveyor Bed & Belt
            var conveyorGo = GameObject.CreatePrimitive(PrimitiveType.Cube);
            conveyorGo.name = "AutoPack_ConveyorBelt";
            conveyorGo.transform.SetParent(visualRoot.transform, false);
            conveyorGo.transform.localPosition = new Vector3(0f, 0.45f, 0f);
            conveyorGo.transform.localScale = new Vector3(0.90f, 0.10f, 1.80f);
            StripCollider(conveyorGo);

            var conveyorRenderer = conveyorGo.GetComponent<Renderer>();
            var conveyorMat = new Material(GetConveyorMaterial());
            conveyorRenderer.material = conveyorMat;

            // Register conveyor renderer & material with controller
            controller.RegisterConveyorVisual(conveyorRenderer, conveyorMat);

            // Conveyor Guard Rails (Left & Right)
            var railL = GameObject.CreatePrimitive(PrimitiveType.Cube);
            railL.name = "AutoPack_ConveyorRail_L";
            railL.transform.SetParent(visualRoot.transform, false);
            railL.transform.localPosition = new Vector3(-0.48f, 0.55f, 0f);
            railL.transform.localScale = new Vector3(0.06f, 0.18f, 1.80f);
            StripCollider(railL);
            SetMaterial(railL, GetHazardMaterial());

            var railR = GameObject.CreatePrimitive(PrimitiveType.Cube);
            railR.name = "AutoPack_ConveyorRail_R";
            railR.transform.SetParent(visualRoot.transform, false);
            railR.transform.localPosition = new Vector3(0.48f, 0.55f, 0f);
            railR.transform.localScale = new Vector3(0.06f, 0.18f, 1.80f);
            StripCollider(railR);
            SetMaterial(railR, GetHazardMaterial());

            // 3b. Packaging Kessel (vessel) — centered on the conveyor, the press stamp dips into this.
            //     Sits on top of the conveyor belt (conveyor top is y=0.50).
            //     Inner Ø 0.40m, outer Ø 0.50m, height 0.30m → top rim at y=0.80.
            var kettleBody = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            kettleBody.name = "AutoPack_Packaging_Kessel";
            kettleBody.transform.SetParent(visualRoot.transform, false);
            kettleBody.transform.localPosition = new Vector3(0f, 0.65f, 0f);
            kettleBody.transform.localScale = new Vector3(0.50f, 0.30f, 0.50f);
            StripCollider(kettleBody);
            SetMaterial(kettleBody, GetBaseMaterial());

            // Kessel rim — slightly larger ring at the top for visual contrast
            var kettleRim = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            kettleRim.name = "AutoPack_Packaging_Kessel_Rim";
            kettleRim.transform.SetParent(visualRoot.transform, false);
            kettleRim.transform.localPosition = new Vector3(0f, 0.82f, 0f);
            kettleRim.transform.localScale = new Vector3(0.54f, 0.04f, 0.54f);
            StripCollider(kettleRim);
            SetMaterial(kettleRim, GetHazardMaterial());

            // 4. Overhead Press Arch & Pneumatics
            var archTop = GameObject.CreatePrimitive(PrimitiveType.Cube);
            archTop.name = "AutoPack_OverheadArch_Top";
            archTop.transform.SetParent(visualRoot.transform, false);
            archTop.transform.localPosition = new Vector3(0f, 1.35f, 0f);
            archTop.transform.localScale = new Vector3(1.30f, 0.18f, 0.60f);
            StripCollider(archTop);
            SetMaterial(archTop, GetBaseMaterial());

            var archLegL = GameObject.CreatePrimitive(PrimitiveType.Cube);
            archLegL.name = "AutoPack_OverheadArch_LegL";
            archLegL.transform.SetParent(visualRoot.transform, false);
            archLegL.transform.localPosition = new Vector3(-0.60f, 0.90f, 0f);
            archLegL.transform.localScale = new Vector3(0.12f, 0.80f, 0.50f);
            StripCollider(archLegL);
            SetMaterial(archLegL, GetBaseMaterial());

            var archLegR = GameObject.CreatePrimitive(PrimitiveType.Cube);
            archLegR.name = "AutoPack_OverheadArch_LegR";
            archLegR.transform.SetParent(visualRoot.transform, false);
            archLegR.transform.localPosition = new Vector3(0.60f, 0.90f, 0f);
            archLegR.transform.localScale = new Vector3(0.12f, 0.80f, 0.50f);
            StripCollider(archLegR);
            SetMaterial(archLegR, GetBaseMaterial());

            // Twin Pneumatic Pistons (Moving Press Head)
            // Initial Y=1.10 so the stamp clears the Kessel rim (y=0.80) when raised;
            // AnimateCycle drives localPosition.y between 1.10 (raised) and 0.80 (pressed, dipping into the Kessel).
            var pistonRoot = new GameObject("AutoPack_PneumaticPress_Head");
            pistonRoot.transform.SetParent(visualRoot.transform, false);
            pistonRoot.transform.localPosition = new Vector3(0f, 1.10f, 0f);

            var pistonCyl1 = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            pistonCyl1.name = "AutoPack_PistonCylinder_1";
            pistonCyl1.transform.SetParent(pistonRoot.transform, false);
            pistonCyl1.transform.localPosition = new Vector3(-0.25f, 0.25f, 0f);
            pistonCyl1.transform.localScale = new Vector3(0.10f, 0.18f, 0.10f);
            StripCollider(pistonCyl1);
            SetMaterial(pistonCyl1, GetPistonMaterial());

            var pistonCyl2 = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            pistonCyl2.name = "AutoPack_PistonCylinder_2";
            pistonCyl2.transform.SetParent(pistonRoot.transform, false);
            pistonCyl2.transform.localPosition = new Vector3(0.25f, 0.25f, 0f);
            pistonCyl2.transform.localScale = new Vector3(0.10f, 0.18f, 0.10f);
            StripCollider(pistonCyl2);
            SetMaterial(pistonCyl2, GetPistonMaterial());

            // Center press rod — connects the two pistons to the stamp plate
            var pressRod = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            pressRod.name = "AutoPack_PressRod";
            pressRod.transform.SetParent(pistonRoot.transform, false);
            pressRod.transform.localPosition = new Vector3(0f, 0.10f, 0f);
            pressRod.transform.localScale = new Vector3(0.06f, 0.10f, 0.06f); // Ø0.12, H=0.20
            StripCollider(pressRod);
            SetMaterial(pressRod, GetPistonMaterial());

            // Cylindrical stamp plate — fits inside the Kessel opening (Ø0.40m).
            // Was a flat rectangular plate (0.75×0.45) that didn't match the round vessel.
            var stampPlate = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            stampPlate.name = "AutoPack_PistonStampPlate";
            stampPlate.transform.SetParent(pistonRoot.transform, false);
            stampPlate.transform.localPosition = new Vector3(0f, -0.05f, 0f);
            stampPlate.transform.localScale = new Vector3(0.38f, 0.05f, 0.38f); // Ø0.38, H=0.10
            StripCollider(stampPlate);
            SetMaterial(stampPlate, GetPistonMaterial());

            controller.RegisterPistonVisual(pistonRoot.transform);

            // 5. Intake Hopper (Rear - Z < 0) & Output Chute (Front - Z > 0)
            var intakeHopper = GameObject.CreatePrimitive(PrimitiveType.Cube);
            intakeHopper.name = "AutoPack_Intake_Hopper";
            intakeHopper.transform.SetParent(visualRoot.transform, false);
            intakeHopper.transform.localPosition = new Vector3(0f, 0.65f, -0.75f);
            intakeHopper.transform.localRotation = Quaternion.Euler(-15f, 0f, 0f);
            intakeHopper.transform.localScale = new Vector3(0.70f, 0.15f, 0.35f);
            StripCollider(intakeHopper);
            SetMaterial(intakeHopper, GetBaseMaterial());

            var outputChute = GameObject.CreatePrimitive(PrimitiveType.Cube);
            outputChute.name = "AutoPack_Output_Chute";
            outputChute.transform.SetParent(visualRoot.transform, false);
            outputChute.transform.localPosition = new Vector3(0f, 0.35f, 0.80f);
            outputChute.transform.localRotation = Quaternion.Euler(18f, 0f, 0f);
            outputChute.transform.localScale = new Vector3(0.70f, 0.12f, 0.40f);
            StripCollider(outputChute);
            SetMaterial(outputChute, GetBaseMaterial());

            // 6. Multi-State Status LED Beacon & Light
            var ledGo = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            ledGo.name = "AutoPack_Status_LED_Beacon";
            ledGo.transform.SetParent(visualRoot.transform, false);
            ledGo.transform.localPosition = new Vector3(0f, 1.48f, 0f);
            ledGo.transform.localScale = new Vector3(0.18f, 0.18f, 0.18f);
            StripCollider(ledGo);

            var ledRenderer = ledGo.GetComponent<Renderer>();
            var ledMat = CreateLedMaterialInstance();
            ledRenderer.material = ledMat;

            var ledLight = ledGo.AddComponent(Il2CppType.Of<Light>()).Cast<Light>();
            ledLight.type = LightType.Point;
            ledLight.range = 3.0f;
            ledLight.intensity = 1.2f;
            ledLight.color = Color.green;

            controller.RegisterLedVisual(ledRenderer, ledMat, ledLight);

            // 7. Control Console Terminal (Side-mounted)
            var consoleGo = GameObject.CreatePrimitive(PrimitiveType.Cube);
            consoleGo.name = "AutoPack_ControlConsole_Terminal";
            consoleGo.transform.SetParent(visualRoot.transform, false);
            consoleGo.transform.localPosition = new Vector3(0.85f, 0.80f, 0f);
            consoleGo.transform.localRotation = Quaternion.Euler(0f, 0f, -20f);
            consoleGo.transform.localScale = new Vector3(0.12f, 0.40f, 0.35f);
            StripCollider(consoleGo);
            SetMaterial(consoleGo, GetBaseMaterial());
        }
        catch (Exception ex)
        {
            Mod.Log.Error($"Failed to build station visuals: {ex}");
        }
    }

    private static void StripCollider(GameObject go)
    {
        var col = go.GetComponent<Collider>();
        if (col != null)
        {
            GameObject.Destroy(col);
        }
    }

    private static void SetMaterial(GameObject go, Material mat)
    {
        var r = go.GetComponent<Renderer>();
        if (r != null)
        {
            r.sharedMaterial = mat;
        }
    }

    /// <summary>
    /// Generates a crisp procedural 128x128 UI icon for the Auto-Packaging Station.
    /// </summary>
    public static Sprite GetOrCreateIconSprite()
    {
        if (_cachedIconSprite != null && _cachedIconSprite.Pointer != IntPtr.Zero) return _cachedIconSprite;

        int size = 128;
        _cachedIconTexture = new Texture2D(size, size, TextureFormat.RGBA32, false)
        {
            name = "AutoPackagingStation_IconTex",
            filterMode = FilterMode.Bilinear
        };

        var pixels = new Color32[size * size];
        Color32 bg = new(24, 28, 34, 255);
        Color32 border = new(55, 65, 80, 255);
        Color32 gold = new(240, 185, 30, 255);
        Color32 steel = new(130, 145, 160, 255);
        Color32 darkSteel = new(60, 70, 80, 255);
        Color32 greenLed = new(40, 230, 80, 255);
        Color32 boxColor = new(180, 130, 80, 255);

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                int idx = y * size + x;
                float nx = (x - size / 2f) / (size / 2f);
                float ny = (y - size / 2f) / (size / 2f);

                // Rounded background card
                if (Mathf.Abs(nx) > 0.90f || Mathf.Abs(ny) > 0.90f)
                {
                    pixels[idx] = new Color32(0, 0, 0, 0); // Transparent outer
                    continue;
                }

                // Border frame
                if (Mathf.Abs(nx) > 0.82f || Mathf.Abs(ny) > 0.82f)
                {
                    pixels[idx] = border;
                    continue;
                }

                // Default dark background
                Color32 col = bg;

                // Base platform at bottom (y: 20 to 35)
                if (y >= 20 && y <= 35 && x >= 24 && x <= 104)
                {
                    col = darkSteel;
                    if (y >= 32) col = gold; // Hazard stripe
                }

                // Conveyor Bed (y: 36 to 52, x: 28 to 100)
                if (y >= 36 && y <= 52 && x >= 28 && x <= 100)
                {
                    byte v = (byte)((x % 12 < 4) ? 45 : 70);
                    col = new Color32(v, v, v, 255);
                }

                // Packaged crate/box on conveyor (y: 53 to 75, x: 48 to 80)
                if (y >= 53 && y <= 75 && x >= 48 && x <= 80)
                {
                    col = boxColor;
                    if (x == 64 || y == 64) col = gold; // Tape seal
                }

                // Overhead Press Gantry (y: 80 to 105)
                if (y >= 80 && y <= 105)
                {
                    // Vertical legs
                    if ((x >= 28 && x <= 38) || (x >= 90 && x <= 100))
                    {
                        col = steel;
                    }
                    // Top beam
                    if (y >= 96 && y <= 105 && x >= 28 && x <= 100)
                    {
                        col = steel;
                    }
                    // Pneumatic piston shaft (y: 76 to 95, x: 60 to 68)
                    if (y >= 76 && y <= 95 && x >= 60 && x <= 68)
                    {
                        col = new Color32(210, 225, 240, 255); // Shiny chrome
                    }
                }

                // Status LED beacon on top (y: 106 to 116, x: 60 to 68)
                if (y >= 106 && y <= 116 && x >= 60 && x <= 68)
                {
                    float ledDist = Vector2.Distance(new Vector2(x, y), new Vector2(64, 111));
                    if (ledDist <= 5f) col = greenLed;
                }

                pixels[idx] = col;
            }
        }

        _cachedIconTexture.SetPixels32(pixels);
        _cachedIconTexture.Apply(false, true);

        _cachedIconSprite = Sprite.Create(_cachedIconTexture, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), 100f);
        _cachedIconSprite.name = "AutoPackagingStation_IconSprite";
        return _cachedIconSprite;
    }
}
