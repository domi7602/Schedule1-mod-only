using System.IO;
using UnityEditor;
using UnityEngine;
public static class BuildReference
{
    public static void Build()
    {
        const string shaderPath = "Assets/S1Mods/ReferenceWater.shader";
        var shader = AssetDatabase.LoadAssetAtPath<Shader>(shaderPath);
        if (shader == null) { Debug.LogError("[S1RefBuild] shader not found at " + shaderPath); EditorApplication.Exit(1); return; }
        const string matPath = "Assets/S1Mods/S1ReferenceMat.mat";
        var existing = AssetDatabase.LoadAssetAtPath<Material>(matPath);
        if (existing == null)
        {
            var mat = new Material(shader) { name = "S1ReferenceMat" };
            AssetDatabase.CreateAsset(mat, matPath);
            AssetDatabase.SaveAssets();
        }
        string outDir = Path.Combine(Application.dataPath, "..", "RefOut");
        Directory.CreateDirectory(outDir);
        var build = new AssetBundleBuild { assetBundleName = "s1reference", assetNames = new[] { shaderPath, matPath } };
        var manifest = BuildPipeline.BuildAssetBundles(outDir, new[] { build }, BuildAssetBundleOptions.ForceRebuildAssetBundle, BuildTarget.StandaloneWindows64);
        if (manifest == null) { Debug.LogError("[S1RefBuild] BuildAssetBundles returned null"); EditorApplication.Exit(2); return; }
        string bundlePath = Path.Combine(outDir, "s1reference");
        Debug.Log("[S1RefBuild] bundle built: " + bundlePath + " (" + new FileInfo(bundlePath).Length + " bytes)");
    }
    public static void Diagnose()
    {
        const string shaderPath = "Assets/S1Mods/ReferenceWater.shader";
        var shader = AssetDatabase.LoadAssetAtPath<Shader>(shaderPath);
        if (shader == null) { Debug.LogError("[S1RefDiag] shader asset not found"); EditorApplication.Exit(1); return; }
        Debug.Log("[S1RefDiag] shader=" + shader.name + " isSupported=" + shader.isSupported + " passCount=" + shader.passCount + " messageCount=" + ShaderUtil.GetShaderMessageCount(shader));
        foreach (var m in ShaderUtil.GetShaderMessages(shader))
            Debug.Log("[S1RefDiag] shader message (" + m.severity + "): " + m.message + " " + m.messageDetails + " line " + m.line);
        AssetDatabase.ImportAsset(shaderPath, ImportAssetOptions.ForceUpdate);
        Debug.Log("[S1RefDiag] after reimport: isSupported=" + shader.isSupported + " messages=" + ShaderUtil.GetShaderMessageCount(shader));
        string bundlePath = Path.Combine(Application.dataPath, "..", "RefOut", "s1reference");
        if (!File.Exists(bundlePath)) { Debug.LogWarning("[S1RefDiag] bundle not found at " + bundlePath); }
        else
        {
            var bundle = AssetBundle.LoadFromFile(bundlePath);
            var shaders = bundle != null ? bundle.LoadAllAssets<Shader>() : new Shader[0];
            foreach (var s in shaders)
            {
                Debug.Log("[S1RefDiag] bundle shader: " + s.name + " isSupported=" + s.isSupported + " passCount=" + s.passCount + " messages=" + ShaderUtil.GetShaderMessageCount(s));
                foreach (var m in ShaderUtil.GetShaderMessages(s))
                    Debug.Log("[S1RefDiag] bundle shader message (" + m.severity + "): " + m.message + " " + m.messageDetails + " line " + m.line);
            }
            var mats = bundle != null ? bundle.LoadAllAssets<Material>() : new Material[0];
            foreach (var mat in mats)
                Debug.Log("[S1RefDiag] bundle material: " + mat.name + " shader=" + (mat.shader != null ? mat.shader.name : "null") + " passCount=" + mat.passCount);
            if (bundle != null) bundle.Unload(true);
        }
        Debug.Log("[S1RefDiag] done");
        EditorApplication.Exit(0);
    }

    /// <summary>
    /// Proves how the runtime planar-reflection ABI must be bound: whether the
    /// material property sheet sees _ReflectionVP/_ReflectionAvailable even
    /// though they are declared in UnityPerMaterial but not in Properties.
    /// </summary>
    public static void ProbeBinding()
    {
        string bundlePath = Path.Combine(Application.dataPath, "..", "RefOut", "s1reference");
        if (!File.Exists(bundlePath)) { Debug.LogError("[S1RefBind] bundle missing: " + bundlePath); EditorApplication.Exit(1); return; }
        var bundle = AssetBundle.LoadFromFile(bundlePath);
        if (bundle == null) { Debug.LogError("[S1RefBind] bundle failed to load"); EditorApplication.Exit(1); return; }
        var shader = bundle.LoadAsset<Shader>("Assets/S1Mods/ReferenceWater.shader");
        if (shader == null)
        {
            var all = bundle.LoadAllAssets<Shader>();
            if (all != null && all.Length > 0) shader = all[0];
        }
        if (shader == null) { Debug.LogError("[S1RefBind] shader missing in bundle"); EditorApplication.Exit(1); return; }
        Debug.Log("[S1RefBind] shader=" + shader.name + " isSupported=" + shader.isSupported);

        var mat = new Material(shader);
        string[] names =
        {
            "_ShallowTint", "_DeepTint", "_Absorption",
            "_ReflectionTex", "_ReflectionStrength", "_ReflectionSharpness",
            "_WaveAmp", "_WaveScale", "_WaveSpeed", "_RippleStrength",
            "_SunStrength", "_SunSharpness",
            "_FoamColor", "_FoamDepth", "_FoamStrength",
            "_SkyColor", "_AmbientColor",
            "_ReflectionVP", "_ReflectionAvailable", "_ReflectionTex_ST",
        };
        foreach (var n in names)
            Debug.Log("[S1RefBind] HasProperty " + n + " = " + mat.HasProperty(n));

        var probe = Matrix4x4.TRS(new Vector3(1f, 2f, 3f), Quaternion.identity, Vector3.one);
        mat.SetMatrix("_ReflectionVP", probe);
        var back = mat.GetMatrix("_ReflectionVP");
        bool matrixMatch = back.m03 == probe.m03 && back.m13 == probe.m13 && back.m23 == probe.m23;
        Debug.Log("[S1RefBind] material SetMatrix/GetMatrix _ReflectionVP m03=" + back.m03 + " m13=" + back.m13 + " m23=" + back.m23 + " match=" + matrixMatch);

        mat.SetFloat("_ReflectionAvailable", 1f);
        Debug.Log("[S1RefBind] material SetFloat/GetFloat _ReflectionAvailable = " + mat.GetFloat("_ReflectionAvailable"));

        var tex = new RenderTexture(8, 8, 0);
        mat.SetTexture("_ReflectionTex", tex);
        Debug.Log("[S1RefBind] material SetTexture _ReflectionTex isNull=" + (mat.GetTexture("_ReflectionTex") == null));

        // Global path, for comparison with the material path above.
        Shader.SetGlobalMatrix("_ReflectionVP", probe);
        Shader.SetGlobalFloat("_ReflectionAvailable", 1f);
        Debug.Log("[S1RefBind] global SetMatrix/SetFloat written (no per-material readback exists)");

        UnityEngine.Object.DestroyImmediate(mat);
        UnityEngine.Object.DestroyImmediate(tex);
        bundle.Unload(true);
        Debug.Log("[S1RefBind] done");
        EditorApplication.Exit(0);
    }
}
