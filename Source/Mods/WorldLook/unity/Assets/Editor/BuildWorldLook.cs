using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

/// <summary>Water-only adaptation of the validated standalone builder; never compiled into the mod DLL.</summary>
public static class BuildWorldLook
{
    private const string RequiredEditor = "2022.3.62f3";
    private const string RequiredPackage = "14.0.12";
    private const string BundleName = "worldlook-shaders.bundle";
    private const string ShaderPath = "Assets/S1Mods/ReferenceWater.shader";
    private const string VariantPath = "Assets/BuildSettings/WorldLookVariants.shadervariants";
    private static readonly string[] RequiredProperties =
    {
        "_ShallowTint", "_DeepTint", "_Absorption", "_SurfaceOpacity", "_ReflectionStrength",
        "_WaveAmp", "_WaveScale", "_WaveSpeed", "_RippleStrength", "_WindActivity",
        "_SunStrength", "_SunSharpness", "_FoamColor", "_FoamDepth", "_FoamStrength", "_SkyColor", "_AmbientColor"
    };

    [Serializable] private sealed class FileHash
    {
        public string path;
        public string sha256;
    }
    [Serializable] private sealed class SourceManifest
    {
        public FileHash[] inputs;
    }
    [Serializable] private sealed class ShaderContract
    {
        public string assetPath;
        public string shaderName;
        public int passCount;
        public bool editorSupported;
        public string[] properties;
    }
    [Serializable] private sealed class Receipt
    {
        public int schemaVersion = 1;
        public string createdUtc;
        public string unityVersion;
        public string platform;
        public string graphicsApi;
        public string colorSpace;
        public string bundleSha256;
        public long bundleBytes;
        public string urpPackageVersion;
        public string corePackageVersion;
        public int requestedWaterVariants;
        public int loadedWaterVariants;
        public string[] bundleAssets;
        public FileHash[] inputs;
        public FileHash[] generatedFixture;
        public string resolvedPackageLockSha256;
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

            // The PowerShell driver supplies an isolated project copy. Generated
            // pipeline assets and PlayerSettings stay in that copy, not in source.
            string root = RequiredArgument("-worldLookRoot");
            string output = RequiredArgument("-worldLookOutput");
            string inputManifest = RequiredArgument("-worldLookInputs");
            string unityRoot = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
            var source = JsonUtility.FromJson<SourceManifest>(File.ReadAllText(inputManifest));
            if (source == null || source.inputs == null || source.inputs.Length == 0)
                throw new InvalidDataException("No source provenance was supplied.");
            CheckSourceHashes(root, source.inputs);
            foreach (var input in source.inputs.Where(file =>
                file.path.StartsWith("unity/Assets/", StringComparison.Ordinal) || file.path == "unity/Packages/manifest.json"))
            {
                if (Sha256(Path.Combine(unityRoot, input.path.Substring("unity/".Length))) != input.sha256)
                    throw new InvalidDataException("Build-copy input differs from source: " + input.path);
            }
            if (File.Exists(Path.Combine(unityRoot, "Assets/S1Mods/WorldLookSky.shader")))
                throw new InvalidDataException("The water build must not include a custom sky source.");

            var urp = UnityEditor.PackageManager.PackageInfo.FindForAssembly(typeof(UniversalRenderPipelineAsset).Assembly);
            var core = UnityEditor.PackageManager.PackageInfo.FindForAssembly(typeof(Volume).Assembly);
            if (urp == null || core == null || urp.version != RequiredPackage || core.version != RequiredPackage)
                throw new InvalidOperationException("The pinned project/editor must resolve URP and Core " + RequiredPackage + ".");

            PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.StandaloneWindows64, false);
            PlayerSettings.SetGraphicsAPIs(BuildTarget.StandaloneWindows64, new[] { GraphicsDeviceType.Direct3D11 });
            PlayerSettings.colorSpace = ColorSpace.Linear;
            ConfigureBuildPipeline();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            AssetDatabase.ImportAsset(ShaderPath, ImportAssetOptions.ForceUpdate);
            Shader sourceShader = AssetDatabase.LoadAssetAtPath<Shader>(ShaderPath);
            if (sourceShader == null) throw new FileNotFoundException("Shader source is missing: " + ShaderPath);
            CheckShaderMessages(sourceShader);
            CreateVariants(sourceShader);
            AssetDatabase.SaveAssets();

            Directory.CreateDirectory(output);
            var map = new AssetBundleBuild
            {
                assetBundleName = BundleName,
                assetNames = new[] { ShaderPath, VariantPath }
            };
            var manifest = BuildPipeline.BuildAssetBundles(output, new[] { map },
                BuildAssetBundleOptions.ForceRebuildAssetBundle |
                BuildAssetBundleOptions.ChunkBasedCompression |
                BuildAssetBundleOptions.StrictMode,
                BuildTarget.StandaloneWindows64);
            if (manifest == null) throw new InvalidOperationException("BuildAssetBundles returned no manifest.");
            CheckShaderMessages(sourceShader);

            string bundlePath = Path.Combine(output, BundleName);
            if (!File.Exists(bundlePath) || new FileInfo(bundlePath).Length < 32)
                throw new InvalidDataException("A valid shader bundle was not produced.");

            ShaderContract contract;
            string[] bundleAssets;
            int loadedVariants;
            AssetBundle bundle = AssetBundle.LoadFromFile(bundlePath);
            if (bundle == null) throw new InvalidDataException("The newly built bundle could not be loaded in the Editor.");
            try
            {
                bundleAssets = bundle.GetAllAssetNames().OrderBy(path => path, StringComparer.Ordinal).ToArray();
                var expectedAssets = map.assetNames.Select(path => path.ToLowerInvariant()).OrderBy(path => path, StringComparer.Ordinal);
                if (!bundleAssets.SequenceEqual(expectedAssets))
                    throw new InvalidDataException("Unexpected asset in the water-only bundle.");
                Shader[] shaders = bundle.LoadAllAssets<Shader>();
                Shader shader = bundle.LoadAsset<Shader>(ShaderPath);
                if (shaders.Length != 1 || shader == null || shader.name != "S1Mods/ReferenceWater")
                    throw new InvalidDataException("Exactly one ReferenceWater shader must be present.");
                CheckShaderMessages(shader);
                if (!shader.isSupported) throw new InvalidDataException("The loaded shader is unsupported in this Editor.");
                var variants = bundle.LoadAsset<ShaderVariantCollection>(VariantPath);
                CheckWaterVariants(variants, shader, sourceShader);
                loadedVariants = variants.variantCount;
                var material = new Material(shader);
                try
                {
                    if (material.passCount < 1) throw new InvalidDataException("No water material pass in the built bundle.");
                    var properties = Enumerable.Range(0, shader.GetPropertyCount()).Select(shader.GetPropertyName).ToArray();
                    if (properties.Length != RequiredProperties.Length || RequiredProperties.Any(name => !material.HasProperty(name)))
                        throw new InvalidDataException("The built water shader does not expose all 17 required material properties.");
                    contract = new ShaderContract
                    {
                        assetPath = ShaderPath, shaderName = shader.name, passCount = material.passCount,
                        editorSupported = shader.isSupported, properties = properties
                    };
                }
                finally { UnityEngine.Object.DestroyImmediate(material); }
            }
            finally { bundle.Unload(true); }

            CheckSourceHashes(root, source.inputs);
            var fixture = Directory.GetFiles(Path.Combine(unityRoot, "Assets/BuildSettings"))
                .OrderBy(path => path, StringComparer.Ordinal).Select(path => HashFile(unityRoot, path)).ToArray();
            var receipt = new Receipt
            {
                createdUtc = DateTime.UtcNow.ToString("O"),
                unityVersion = Application.unityVersion,
                platform = "StandaloneWindows64",
                graphicsApi = "Direct3D11",
                colorSpace = "Linear",
                bundleSha256 = Sha256(bundlePath),
                bundleBytes = new FileInfo(bundlePath).Length,
                urpPackageVersion = urp.version,
                corePackageVersion = core.version,
                requestedWaterVariants = 8,
                loadedWaterVariants = loadedVariants,
                bundleAssets = bundleAssets,
                inputs = source.inputs,
                generatedFixture = fixture,
                resolvedPackageLockSha256 = Sha256(Path.Combine(unityRoot, "Packages/packages-lock.json")),
                shaders = new[] { contract },
                validation = "Editor bundle rebuilt, loaded, shader messages/pass/17-property contracts and all 8 SVC entries checked. SVC presence is not native GPU-variant execution or game visual/performance validation."
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

    private static string RequiredArgument(string name)
    {
        string[] arguments = Environment.GetCommandLineArgs();
        int index = Array.IndexOf(arguments, name);
        if (index < 0 || index + 1 >= arguments.Length || string.IsNullOrWhiteSpace(arguments[index + 1]))
            throw new ArgumentException("Use build/Build.ps1; missing " + name);
        return Path.GetFullPath(arguments[index + 1]);
    }

    private static void CheckSourceHashes(string root, FileHash[] inputs)
    {
        foreach (var input in inputs)
        {
            string path = Path.GetFullPath(Path.Combine(root, input.path));
            if (!path.StartsWith(root.TrimEnd(Path.DirectorySeparatorChar) + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) ||
                Sha256(path) != input.sha256)
                throw new InvalidDataException("Source changed during shader build: " + input.path);
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

    private static IEnumerable<ShaderVariantCollection.ShaderVariant> WaterVariants(Shader water)
    {
        string[] shadows = { "", "_MAIN_LIGHT_SHADOWS", "_MAIN_LIGHT_SHADOWS_CASCADE", "_MAIN_LIGHT_SHADOWS_SCREEN" };
        foreach (string shadow in shadows)
        {
            for (int soft = 0; soft < 2; soft++)
            {
                var keywords = new List<string>();
                if (shadow.Length != 0) keywords.Add(shadow);
                if (soft != 0) keywords.Add("_SHADOWS_SOFT");
                yield return new ShaderVariantCollection.ShaderVariant(water, PassType.ScriptableRenderPipeline, keywords.ToArray());
            }
        }
    }

    private static void CreateVariants(Shader water)
    {
        var collection = AssetDatabase.LoadAssetAtPath<ShaderVariantCollection>(VariantPath);
        if (collection == null)
        {
            collection = new ShaderVariantCollection();
            AssetDatabase.CreateAsset(collection, VariantPath);
        }
        collection.Clear();
        foreach (var variant in WaterVariants(water))
            if (!collection.Add(variant)) throw new InvalidOperationException("Duplicate or rejected water variant.");
        CheckWaterVariants(collection, water, water);
        EditorUtility.SetDirty(collection);
    }

    private static void CheckWaterVariants(ShaderVariantCollection collection, Shader water, Shader sourceShader)
    {
        if (collection == null || collection.shaderCount != 1 || collection.variantCount != 8)
            throw new InvalidDataException("The collection must contain exactly the 8 requested water variants.");
        foreach (var sourceVariant in WaterVariants(sourceShader))
        {
            // The constructor validates against Editor snippets, which a loaded
            // player-bundle shader does not have. Validate the descriptor against
            // its source first, then query membership using the loaded shader's
            // identity through the public struct field. This does not add variants.
            var loadedVariant = sourceVariant;
            loadedVariant.shader = water;
            if (!collection.Contains(loadedVariant))
                throw new InvalidDataException("A requested water variant is missing from the collection.");
        }
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
