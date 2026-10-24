using System.IO;
using UnityEditor;
using UnityEngine;

// Builds the S1Mods/Water shader + its default material into an AssetBundle the
// WaterShader mod loads at runtime. Run from the CLI:
//   Unity.exe -batchmode -quit -projectPath <proj> -executeMethod BuildBundles.Build
public static class BuildBundles
{
    public static void Build()
    {
        const string shaderPath = "Assets/S1Mods/Water.shader";
        var shader = AssetDatabase.LoadAssetAtPath<Shader>(shaderPath);
        if (shader == null)
        {
            Debug.LogError("[S1WaterBuild] shader not found at " + shaderPath);
            EditorApplication.Exit(1);
            return;
        }

        // A default material so the mod can swap it in without authoring one.
        const string matPath = "Assets/S1Mods/S1WaterMat.mat";
        var existing = AssetDatabase.LoadAssetAtPath<Material>(matPath);
        if (existing == null)
        {
            var mat = new Material(shader) { name = "S1WaterMat" };
            AssetDatabase.CreateAsset(mat, matPath);
            AssetDatabase.SaveAssets();
        }

        string outDir = Path.Combine(Application.dataPath, "..", "BundleOut");
        Directory.CreateDirectory(outDir);

        var build = new AssetBundleBuild
        {
            assetBundleName = "s1water",
            assetNames = new[] { shaderPath, matPath }
        };

        // Isolation bundle: minimal unlit shader. If THIS comes out unsupported
        // in the bundle too, the failure is project/build settings, not the
        // Water shader's feature set (2026-10-17 pink-water investigation).
        const string testShaderPath = "Assets/S1Mods/TestUnlit.shader";
        const string testMatPath = "Assets/S1Mods/S1TestMat.mat";
        var testShader = AssetDatabase.LoadAssetAtPath<Shader>(testShaderPath);
        if (testShader != null)
        {
            var existingTest = AssetDatabase.LoadAssetAtPath<Material>(testMatPath);
            if (existingTest == null)
            {
                var mat = new Material(testShader) { name = "S1TestMat" };
                AssetDatabase.CreateAsset(mat, testMatPath);
                AssetDatabase.SaveAssets();
            }
        }
        var testBuild = new AssetBundleBuild
        {
            assetBundleName = "s1test",
            assetNames = new[] { testShaderPath, testMatPath }
        };

        // Control bundle: a material using one of Unity's OWN built-in shaders.
        // If even Unity's shader comes out unsupported, the bundle build pipeline
        // itself is broken (2026-10-17 pink-water investigation).
        var builtinShader = Shader.Find("Universal Render Pipeline/Lit");
        AssetBundleBuild unityBuild = default;
        if (builtinShader != null)
        {
            const string unityMatPath = "Assets/S1Mods/S1UnityMat.mat";
            var existingUnity = AssetDatabase.LoadAssetAtPath<Material>(unityMatPath);
            if (existingUnity == null)
            {
                var mat = new Material(builtinShader) { name = "S1UnityMat" };
                AssetDatabase.CreateAsset(mat, unityMatPath);
                AssetDatabase.SaveAssets();
            }
            unityBuild = new AssetBundleBuild
            {
                assetBundleName = "s1unity",
                assetNames = new[] { unityMatPath }
            };
            Debug.Log("[S1WaterBuild] control bundle with built-in URP Lit shader: " + builtinShader.name);
        }
        else
        {
            Debug.LogWarning("[S1WaterBuild] Shader.Find(URP/Lit) returned null — control bundle skipped");
        }

        var manifest = BuildPipeline.BuildAssetBundles(
            outDir,
            unityBuild.assetBundleName != null
                ? new[] { build, testBuild, unityBuild }
                : new[] { build, testBuild },
            // ForceRebuildAssetBundle: without it BuildAssetBundles skips bundles
            // whose inputs are unchanged (cache hit) and keeps the old output on
            // disk — which left an f3-built bundle in place when rebuilding with
            // a different editor patch version (2026-10-17 pink-water incident).
            BuildAssetBundleOptions.ForceRebuildAssetBundle,
            BuildTarget.StandaloneWindows64);

        if (manifest == null)
        {
            Debug.LogError("[S1WaterBuild] BuildAssetBundles returned null");
            EditorApplication.Exit(2);
            return;
        }

        string bundlePath = Path.Combine(outDir, "s1water");
        Debug.Log("[S1WaterBuild] bundle built: " + bundlePath +
                  " (" + new FileInfo(bundlePath).Length + " bytes)");
    }
}
