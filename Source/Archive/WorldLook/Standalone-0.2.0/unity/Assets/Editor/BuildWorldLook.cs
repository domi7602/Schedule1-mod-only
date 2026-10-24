using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>Editor-only shader build. This file is never compiled into the mod DLL.</summary>
public static class BuildWorldLook
{
    private const string RequiredEditor = "2022.3.62f3";
    private const string BundleName = "worldlook-shaders.bundle";
    private const string VariantPath = "Assets/BuildSettings/WorldLookVariants.shadervariants";
    private static readonly string[] ShaderPaths =
    {
        "Assets/S1Mods/ReferenceWater.shader",
        "Assets/S1Mods/WorldLookSky.shader"
    };

    [Serializable] private sealed class FileHash
    {
        public string path;
        public string sha256;
    }
    [Serializable] private sealed class ShaderContract
    {
        public string assetPath;
        public string shaderName;
        public int passCount;
        public string[] properties;
    }
    [Serializable] private sealed class Receipt
    {
        public string createdUtc;
        public string unityVersion;
        public string platform;
        public string graphicsApi;
        public string bundleSha256;
        public long bundleBytes;
        public string urpPackageVersion;
        public string corePackageVersion;
        public int requestedWaterVariants;
        public FileHash[] inputs;
        public ShaderContract[] shaders;
        public string validation;
    }

    public static void Build()
    {
        try
        {
            if (Application.unityVersion != RequiredEditor)
                throw new InvalidOperationException("Use Unity " + RequiredEditor + "; found " + Application.unityVersion);
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.StandaloneWindows64)
                throw new InvalidOperationException("Select StandaloneWindows64 (-buildTarget Win64).");

            PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.StandaloneWindows64, false);
            PlayerSettings.SetGraphicsAPIs(BuildTarget.StandaloneWindows64, new[] { GraphicsDeviceType.Direct3D11 });
            PlayerSettings.colorSpace = ColorSpace.Linear;
            ConfigureBuildPipeline();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            var sourceShaders = new List<Shader>();
            foreach (string path in ShaderPaths)
            {
                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
                Shader shader = AssetDatabase.LoadAssetAtPath<Shader>(path);
                if (shader == null) throw new FileNotFoundException("Shader source is missing: " + path);
                CheckShaderMessages(shader);
                sourceShaders.Add(shader);
            }
            CreateVariants(sourceShaders[0], sourceShaders[1]);
            AssetDatabase.SaveAssets();

            string unityRoot = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
            string root = Path.GetFullPath(Path.Combine(unityRoot, ".."));
            string output = Path.Combine(root, "assets");
            Directory.CreateDirectory(output);
            var map = new AssetBundleBuild
            {
                assetBundleName = BundleName,
                assetNames = ShaderPaths.Concat(new[] { VariantPath }).ToArray()
            };
            var manifest = BuildPipeline.BuildAssetBundles(output, new[] { map },
                BuildAssetBundleOptions.ForceRebuildAssetBundle |
                BuildAssetBundleOptions.ChunkBasedCompression |
                BuildAssetBundleOptions.StrictMode,
                BuildTarget.StandaloneWindows64);
            if (manifest == null) throw new InvalidOperationException("BuildAssetBundles returned no manifest.");
            foreach (var shader in sourceShaders) CheckShaderMessages(shader);

            string bundlePath = Path.Combine(output, BundleName);
            if (!File.Exists(bundlePath) || new FileInfo(bundlePath).Length < 32)
                throw new InvalidDataException("A valid shader bundle was not produced.");

            var contracts = new List<ShaderContract>();
            AssetBundle bundle = AssetBundle.LoadFromFile(bundlePath);
            if (bundle == null) throw new InvalidDataException("The newly built bundle could not be loaded in the Editor.");
            try
            {
                var variants = bundle.LoadAsset<ShaderVariantCollection>(VariantPath);
                if (variants == null || variants.variantCount < 9)
                    throw new InvalidDataException("The built bundle is missing the requested water/sky variant collection.");
                foreach (string path in ShaderPaths)
                {
                    Shader shader = bundle.LoadAsset<Shader>(path);
                    if (shader == null) throw new InvalidDataException("Shader missing from built bundle: " + path);
                    CheckShaderMessages(shader);
                    var material = new Material(shader);
                    try
                    {
                        if (material.passCount < 1) throw new InvalidDataException("No material pass in " + path);
                        var properties = new List<string>();
                        for (int i = 0; i < shader.GetPropertyCount(); i++) properties.Add(shader.GetPropertyName(i));
                        contracts.Add(new ShaderContract
                        {
                            assetPath = path, shaderName = shader.name,
                            passCount = material.passCount, properties = properties.ToArray()
                        });
                    }
                    finally { UnityEngine.Object.DestroyImmediate(material); }
                }
            }
            finally { bundle.Unload(true); }

            var hashes = new List<FileHash>();
            foreach (string path in ShaderPaths)
                hashes.Add(HashFile(root, Path.Combine(unityRoot, path)));
            hashes.Add(HashFile(root, Path.Combine(unityRoot, "Assets/Editor/BuildWorldLook.cs")));
            hashes.Add(HashFile(root, Path.Combine(unityRoot, "Packages/manifest.json")));
            string packageLock = Path.Combine(unityRoot, "Packages/packages-lock.json");
            if (File.Exists(packageLock)) hashes.Add(HashFile(root, packageLock));
            hashes.Add(HashFile(root, Path.Combine(unityRoot, "ProjectSettings/ProjectVersion.txt")));
            hashes.Add(HashFile(root, Path.Combine(unityRoot, VariantPath)));

            var urp = UnityEditor.PackageManager.PackageInfo.FindForAssembly(typeof(UniversalRenderPipelineAsset).Assembly);
            var core = UnityEditor.PackageManager.PackageInfo.FindForAssembly(typeof(Volume).Assembly);
            if (urp == null || !urp.version.StartsWith("14.", StringComparison.Ordinal))
                throw new InvalidOperationException("The shader project must resolve URP 14.x.");

            var receipt = new Receipt
            {
                createdUtc = DateTime.UtcNow.ToString("O"),
                unityVersion = Application.unityVersion,
                platform = "StandaloneWindows64",
                graphicsApi = "Direct3D11",
                bundleSha256 = Sha256(bundlePath),
                bundleBytes = new FileInfo(bundlePath).Length,
                urpPackageVersion = urp.version,
                corePackageVersion = core == null ? "unknown" : core.version,
                requestedWaterVariants = 8,
                inputs = hashes.ToArray(),
                shaders = contracts.ToArray(),
                validation = "Editor bundle build, shader messages and load/pass contracts checked. No game visual or performance validation."
            };
            File.WriteAllText(Path.Combine(output, "shader-build.json"), JsonUtility.ToJson(receipt, true));
            Debug.Log("[WorldLookBuild] SUCCESS: " + bundlePath + " sha256=" + receipt.bundleSha256);
            if (Application.isBatchMode) EditorApplication.Exit(0);
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            if (Application.isBatchMode) EditorApplication.Exit(1);
            else throw;
        }
    }

    private static void ConfigureBuildPipeline()
    {
        // This is an asset-bundle build fixture, not a replacement for the game's
        // URP asset. It is not placed in the bundle or applied by the mod.
        const string folder = "Assets/BuildSettings";
        const string rendererPath = folder + "/WorldLookBuildRenderer.asset";
        const string pipelinePath = folder + "/WorldLookBuildPipeline.asset";
        if (!AssetDatabase.IsValidFolder(folder)) AssetDatabase.CreateFolder("Assets", "BuildSettings");
        var renderer = AssetDatabase.LoadAssetAtPath<UniversalRendererData>(rendererPath);
        if (renderer == null)
        {
            renderer = ScriptableObject.CreateInstance<UniversalRendererData>();
            AssetDatabase.CreateAsset(renderer, rendererPath);
        }
        var pipeline = AssetDatabase.LoadAssetAtPath<UniversalRenderPipelineAsset>(pipelinePath);
        if (pipeline == null)
        {
            pipeline = UniversalRenderPipelineAsset.Create(renderer);
            AssetDatabase.CreateAsset(pipeline, pipelinePath);
        }
        pipeline.supportsCameraDepthTexture = true;
        pipeline.supportsCameraOpaqueTexture = true;
        pipeline.supportsHDR = true;
        // URP 14 exposes these support flags as getters. Set their serialized
        // Editor fields and verify the result through the public getters.
        var serializedPipeline = new SerializedObject(pipeline);
        SetBoolean(serializedPipeline, "m_MainLightShadowsSupported", true);
        SetBoolean(serializedPipeline, "m_SoftShadowsSupported", true);
        serializedPipeline.ApplyModifiedPropertiesWithoutUndo();
        pipeline.shadowCascadeCount = 4;
        if (!pipeline.supportsMainLightShadows || !pipeline.supportsSoftShadows)
            throw new InvalidOperationException("The build fixture could not enable main-light and soft-shadow variants.");

        // Keep the screen-shadow branch eligible too. This renderer feature
        // belongs only to the build fixture, never to the distributed bundle.
        Type screenShadowType = TypeCache.GetTypesDerivedFrom<ScriptableRendererFeature>()
            .SingleOrDefault(type => type.Name == "ScreenSpaceShadows" &&
                type.Assembly == typeof(UniversalRenderPipelineAsset).Assembly);
        if (screenShadowType == null)
            throw new InvalidOperationException("URP 14 screen-space shadow feature type is unavailable.");
        if (!renderer.rendererFeatures.Any(feature => feature != null && feature.GetType() == screenShadowType))
        {
            var feature = ScriptableObject.CreateInstance(screenShadowType) as ScriptableRendererFeature;
            if (feature == null) throw new InvalidOperationException("URP 14 screen-space shadow feature is unavailable.");
            feature.name = "WorldLookBuildScreenShadows";
            feature.SetActive(true);
            AssetDatabase.AddObjectToAsset(feature, renderer);
            renderer.rendererFeatures.Add(feature);
        }
        GraphicsSettings.defaultRenderPipeline = pipeline;
        QualitySettings.renderPipeline = pipeline;
        ConfigureGlobalStripping(folder);
        EditorUtility.SetDirty(renderer);
        EditorUtility.SetDirty(pipeline);
    }

    private static void ConfigureGlobalStripping(string folder)
    {
        const string typeName = "UnityEngine.Rendering.Universal.UniversalRenderPipelineGlobalSettings";
        RenderPipelineGlobalSettings settings = GraphicsSettings.GetSettingsForRenderPipeline<UniversalRenderPipeline>();
        if (settings == null)
        {
            string path = folder + "/WorldLookBuildGlobalSettings.asset";
            settings = AssetDatabase.LoadAssetAtPath<RenderPipelineGlobalSettings>(path);
            if (settings == null)
            {
                Type type = typeof(UniversalRenderPipelineAsset).Assembly.GetType(typeName, true);
                settings = ScriptableObject.CreateInstance(type) as RenderPipelineGlobalSettings;
                if (settings == null) throw new InvalidOperationException("Could not create URP 14 global build settings.");
                AssetDatabase.CreateAsset(settings, path);
            }
            GraphicsSettings.RegisterRenderPipelineSettings<UniversalRenderPipeline>(settings);
        }
        if (settings.GetType().FullName != typeName)
            throw new InvalidOperationException("Unexpected global pipeline settings type: " + settings.GetType().FullName);
        var serialized = new SerializedObject(settings);
        // Keeping disabled variants and enabling the features in the fixture
        // are both required; the SVC alone is not a stripping override.
        SetBoolean(serialized, "m_StripUnusedVariants", false);
        serialized.ApplyModifiedPropertiesWithoutUndo();
        serialized.Update();
        if (serialized.FindProperty("m_StripUnusedVariants").boolValue)
            throw new InvalidOperationException("URP unused-variant stripping remained enabled.");
        EditorUtility.SetDirty(settings);
    }

    private static void SetBoolean(SerializedObject target, string name, bool value)
    {
        var property = target.FindProperty(name);
        if (property == null || property.propertyType != SerializedPropertyType.Boolean)
            throw new InvalidOperationException("Required URP 14 build setting is missing: " + name);
        property.boolValue = value;
    }

    private static void CreateVariants(Shader water, Shader sky)
    {
        var collection = AssetDatabase.LoadAssetAtPath<ShaderVariantCollection>(VariantPath);
        if (collection == null)
        {
            collection = new ShaderVariantCollection();
            AssetDatabase.CreateAsset(collection, VariantPath);
        }
        collection.Clear();
        string[] shadows = { "", "_MAIN_LIGHT_SHADOWS", "_MAIN_LIGHT_SHADOWS_CASCADE", "_MAIN_LIGHT_SHADOWS_SCREEN" };
        foreach (string shadow in shadows)
        {
            for (int soft = 0; soft < 2; soft++)
            {
                var keywords = new List<string>();
                if (shadow.Length != 0) keywords.Add(shadow);
                if (soft != 0) keywords.Add("_SHADOWS_SOFT");
                var variant = new ShaderVariantCollection.ShaderVariant(water, PassType.ScriptableRenderPipeline, keywords.ToArray());
                if (!collection.Add(variant)) throw new InvalidOperationException("Duplicate or rejected water variant.");
            }
        }
        if (!collection.Add(new ShaderVariantCollection.ShaderVariant(sky, PassType.Normal)))
            throw new InvalidOperationException("The native skybox Normal-pass variant was rejected.");
        EditorUtility.SetDirty(collection);
    }

    private static void CheckShaderMessages(Shader shader)
    {
        foreach (var message in ShaderUtil.GetShaderMessages(shader))
        {
            if (message.severity.ToString() == "Error")
                throw new InvalidOperationException(shader.name + ": " + message.message + " line " + message.line);
            Debug.Log("[WorldLookBuild] " + shader.name + " " + message.severity + ": " + message.message);
        }
    }

    private static string Sha256(string path)
    {
        using (var stream = File.OpenRead(path))
        using (var algorithm = SHA256.Create())
            return BitConverter.ToString(algorithm.ComputeHash(stream)).Replace("-", "").ToLowerInvariant();
    }

    private static FileHash HashFile(string root, string path) => new FileHash
    {
        path = path.Substring(root.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar).Replace('\\', '/'),
        sha256 = Sha256(path)
    };
}
