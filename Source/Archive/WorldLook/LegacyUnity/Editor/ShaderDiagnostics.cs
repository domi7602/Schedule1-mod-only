using System.IO;
using UnityEditor;
using UnityEngine;

// Batchmode diagnostics for the S1Mods/Water shader: does it actually compile
// in the editor, and what does the bundle contain?
//   Unity.exe -batchmode -quit -projectPath <proj> -executeMethod ShaderDiagnostics.Diagnose
public static class ShaderDiagnostics
{
    public static void Diagnose()
    {
        const string shaderPath = "Assets/S1Mods/Water.shader";
        var shader = AssetDatabase.LoadAssetAtPath<Shader>(shaderPath);
        if (shader == null)
        {
            Debug.LogError("[S1Diag] shader asset not found: " + shaderPath);
            EditorApplication.Exit(1);
            return;
        }

        Debug.Log($"[S1Diag] shader={shader.name} isSupported={shader.isSupported} passCount={shader.passCount} messageCount={ShaderUtil.GetShaderMessageCount(shader)}");
        var msgs = ShaderUtil.GetShaderMessages(shader);
        foreach (var m in msgs)
        {
            Debug.Log($"[S1Diag] shader message ({m.severity}): {m.message} {m.messageDetails} line {m.line}");
        }
        bool hasErrors = false;
        foreach (var m in msgs)
        {
            if (m.severity.ToString().Contains("Error")) hasErrors = true;
        }
        Debug.Log($"[S1Diag] hasErrorMessages={hasErrors}");

        // Re-import forces a fresh compile attempt so deferred/hidden errors surface.
        AssetDatabase.ImportAsset(shaderPath, ImportAssetOptions.ForceUpdate);
        var msgs2 = ShaderUtil.GetShaderMessages(shader);
        bool hasErrors2 = false;
        foreach (var m in msgs2)
        {
            if (m.severity.ToString().Contains("Error")) hasErrors2 = true;
            Debug.Log($"[S1Diag] post-reimport message ({m.severity}): {m.message} {m.messageDetails} line {m.line}");
        }
        Debug.Log($"[S1Diag] after reimport: isSupported={shader.isSupported} messages={ShaderUtil.GetShaderMessageCount(shader)} hasErrors={hasErrors2}");

        // Which graphics APIs will the Standalone target compile for?
        try
        {
            var apis = PlayerSettings.GetGraphicsAPIs(BuildTarget.StandaloneWindows64);
            Debug.Log("[S1Diag] Standalone graphics APIs: " + string.Join(", ", apis));
        }
        catch (System.Exception ex)
        {
            Debug.Log("[S1Diag] GetGraphicsAPIs failed: " + ex.Message);
        }

        // Bundle contents: load it back and inspect the embedded shader.
        InspectBundle(Path.Combine(Application.dataPath, "..", "BundleOut", "s1water"), "s1water");
        InspectBundle(Path.Combine(Application.dataPath, "..", "BundleOut", "s1test"), "s1test");
        InspectBundle(Path.Combine(Application.dataPath, "..", "BundleOut", "s1unity"), "s1unity");

        Debug.Log("[S1Diag] done");
        EditorApplication.Exit(0);
    }

    private static void InspectBundle(string bundlePath, string label)
    {
        if (!File.Exists(bundlePath))
        {
            Debug.LogWarning("[S1Diag] bundle '" + label + "' not found at " + bundlePath);
            return;
        }
        var bundle = AssetBundle.LoadFromFile(bundlePath);
        var shaders = bundle != null ? bundle.LoadAllAssets<Shader>() : new Shader[0];
        foreach (var s in shaders)
        {
            Debug.Log($"[S1Diag] {label} shader: {s.name} isSupported={s.isSupported} passCount={s.passCount} messages={ShaderUtil.GetShaderMessageCount(s)}");
            foreach (var m in ShaderUtil.GetShaderMessages(s))
            {
                Debug.Log($"[S1Diag] {label} shader message ({m.severity}): {m.message} {m.messageDetails} line {m.line}");
            }
        }
        var mats = bundle != null ? bundle.LoadAllAssets<Material>() : new Material[0];
        foreach (var mat in mats)
        {
            Debug.Log($"[S1Diag] {label} material: {mat.name} shader={mat.shader?.name} passCount={mat.passCount}");
        }
        if (bundle != null) bundle.Unload(true);
    }
}
