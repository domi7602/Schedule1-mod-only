using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Object = UnityEngine.Object;

// Editor-only, copied into an isolated scratch project by Run-Preview.ps1.
// No water math is duplicated here: all water pixels come from the supplied bundle.
public static class WorldLookWaterPreview
{
    private const int Width = 960, Height = 540;
    private static readonly List<Object> Owned = new List<Object>();
    private static readonly List<string> Errors = new List<string>();
    private static readonly Color Sky = new Color(0.32f, 0.42f, 0.55f, 1);
    private static readonly Color Horizon = new Color(0.42f, 0.46f, 0.50f, 1);
    internal static float FixedTime;
    internal static int ClockExecutions;

    [Serializable] private sealed class Property
    {
        public string name, type;
        public float number;
        public Color color;
    }
    [Serializable] private sealed class ImageRecord
    {
        public string file, sha256;
        public Vector3 cameraPosition, cameraTarget;
        public float seconds, minRgb, maxRgb;
        public bool water, spatiallyNonuniform;
        public int nonfinitePixels, errorMagentaPixels, changedPixelsFromDry;
    }
    [Serializable] private sealed class Receipt
    {
        public string label, createdUtc, bundlePath, bundleSha256, shaderName;
        public string fixtureSha256, unity, urp, gpu, graphicsApi, colorSpace;
        public string capture = "URP SingleCameraRequest -> ARGBFloat Linear GPU target -> RGBAFloat ReadPixels; PNG linear-to-sRGB encoding, clamped to 8 bit; no tone mapping.";
        public string scene = "Water plane y=0 (300x300); checker floor y=-1.8 (200x200); 12 submerged colored cubes; synthetic neutral daylight sky/horizon inputs, not sampled game colors.";
        public string settings = "960x540; vertical FOV=50; clip=.1..400; HDR; depth after opaques; full-resolution opaque texture; renderScale=1; MSAA=1; mipmapped/trilinear checker; no postprocessing, shadows, fog, camera jitter or auto exposure; wind=.2. Sun/wave/foam defaults retained from bundle.";
        public string limitations = "Controlled Editor GPU comparison only. Not native game lighting/camera, FPS, shadow-variant coverage, motion/flicker or owner visual acceptance. Float target detects observed nonfinite output, not every intermediate shader operation.";
        public Vector3 sunEuler;
        public Color sunColor, skyColor, horizonColor, ambientColor;
        public float sunIntensity;
        public Property[] material;
        public ImageRecord[] images;
        public bool repeatedTimeZeroPngIdentical;
        public string[] errors;
    }

    public static void Run()
    {
        int exitCode = 1;
        AssetBundle bundle = null;
        string output = Argument("-previewOutput");
        var receipt = new Receipt { label = Argument("-previewLabel"), createdUtc = DateTime.UtcNow.ToString("O") };
        var images = new List<ImageRecord>();
        Application.logMessageReceived += CaptureError;
        try
        {
            if (!Application.isBatchMode || Application.unityVersion != "2022.3.62f3")
                throw new InvalidOperationException("Use the isolated batch-mode Unity 2022.3.62f3 runner.");
            receipt.unity = Application.unityVersion;
            receipt.urp = UnityEditor.PackageManager.PackageInfo.FindForAssembly(typeof(UniversalRenderPipeline).Assembly).version;
            receipt.graphicsApi = SystemInfo.graphicsDeviceType.ToString();
            receipt.gpu = SystemInfo.graphicsDeviceName;
            if (receipt.urp != "14.0.12" || SystemInfo.graphicsDeviceType != GraphicsDeviceType.Direct3D11 ||
                !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBFloat))
                throw new InvalidOperationException("URP 14.0.12 / D3D11 / float GPU target required.");
            PlayerSettings.colorSpace = ColorSpace.Linear;
            receipt.colorSpace = QualitySettings.activeColorSpace.ToString();
            if (QualitySettings.activeColorSpace != ColorSpace.Linear) throw new InvalidOperationException("Linear color space required.");
            ShaderUtil.allowAsyncCompilation = false;
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            ConfigurePipeline();

            receipt.bundlePath = Argument("-previewBundle");
            receipt.bundleSha256 = Hash(receipt.bundlePath);
            receipt.fixtureSha256 = Hash(Path.Combine(Application.dataPath, "Editor/WorldLookWaterPreview.cs"));
            bundle = AssetBundle.LoadFromFile(receipt.bundlePath);
            if (bundle == null) throw new InvalidDataException("Cannot load bundle.");
            var shader = bundle.LoadAsset<Shader>("assets/s1mods/referencewater.shader");
            if (shader == null || shader.name != "S1Mods/ReferenceWater" || !shader.isSupported)
                throw new InvalidDataException("Supported bundled ReferenceWater shader required.");
            receipt.shaderName = shader.name;
            var water = Keep(new Material(shader));
            water.SetFloat("_Absorption", .10f);
            water.SetFloat("_RippleStrength", .28f);
            water.SetFloat("_SurfaceOpacity", .22f);
            water.SetFloat("_ReflectionStrength", .12f);
            water.SetFloat("_WindActivity", .2f);
            water.SetColor("_ShallowTint", new Color(.46f, .52f, .55f, 1));
            water.SetColor("_SkyColor", Sky);
            water.SetColor("_AmbientColor", Horizon);
            receipt.material = Enumerable.Range(0, shader.GetPropertyCount()).Select(i =>
            {
                string name = shader.GetPropertyName(i);
                bool color = shader.GetPropertyType(i) == ShaderPropertyType.Color;
                return new Property { name = name, type = shader.GetPropertyType(i).ToString(),
                    number = color ? 0 : water.GetFloat(name), color = color ? water.GetColor(name) : Color.clear };
            }).ToArray();

            var waterRenderer = Primitive("Water", PrimitiveType.Plane, new Vector3(0, 0, 60), new Vector3(30, 1, 30), water);
            BuildFloor();
            var sun = Keep(new GameObject("Fixed sun")).AddComponent<Light>();
            sun.type = LightType.Directional;
            sun.transform.rotation = Quaternion.Euler(20, 180, 0);
            sun.color = new Color(1, .97f, .92f, 1);
            sun.intensity = 1.2f;
            sun.shadows = LightShadows.None;
            RenderSettings.sun = sun;
            RenderSettings.skybox = null;
            RenderSettings.fog = false;
            RenderSettings.ambientMode = AmbientMode.Flat;
            RenderSettings.ambientLight = new Color(.32f, .36f, .40f, 1);
            receipt.sunEuler = sun.transform.eulerAngles;
            receipt.sunColor = sun.color;
            receipt.sunIntensity = sun.intensity;
            receipt.skyColor = Sky;
            receipt.horizonColor = Horizon;
            receipt.ambientColor = RenderSettings.ambientLight;

            var camera = Keep(new GameObject("Capture camera")).AddComponent<Camera>();
            camera.enabled = false;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = Horizon;
            camera.fieldOfView = 50;
            camera.aspect = (float)Width / Height;
            camera.nearClipPlane = .1f;
            camera.farClipPlane = 400;
            camera.allowHDR = true;
            camera.allowMSAA = false;
            var data = camera.GetUniversalAdditionalCameraData();
            data.requiresDepthTexture = true;
            data.requiresColorTexture = true;
            data.renderPostProcessing = false;
            data.antialiasing = AntialiasingMode.None;
            var target = Keep(new RenderTexture(Width, Height, 24, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear));
            if (!target.Create()) throw new InvalidOperationException("GPU target allocation failed.");
            for (int view = 0; view < 2; view++)
            {
                string prefix = view == 0 ? "near" : "far";
                camera.transform.position = view == 0 ? new Vector3(0, 3, -8) : new Vector3(0, 16, -48);
                Vector3 aim = view == 0 ? new Vector3(0, 0, 7) : new Vector3(0, 0, 32);
                camera.transform.LookAt(aim);
                waterRenderer.enabled = false;
                ImageRecord dry = Capture(camera, target, output, prefix + "-dry", aim, 0, false);
                images.Add(dry);
                waterRenderer.enabled = true;
                foreach (float seconds in new[] { 0f, 7.5f, 31f })
                {
                    string suffix = seconds == 0 ? "t0" : seconds == 7.5f ? "t7p5" : "t31";
                    ImageRecord image = Capture(camera, target, output, prefix + "-" + suffix, aim, seconds, true);
                    image.changedPixelsFromDry = CountDifferentPixels(Path.Combine(output, dry.file), Path.Combine(output, image.file));
                    images.Add(image);
                    if (image.changedPixelsFromDry == 0) throw new InvalidDataException("Water made no visible pixel contribution.");
                }
                // Repeated captures prove that Editor wall-clock time did not leak into these pixels.
                ImageRecord repeated = Capture(camera, target, output, prefix + "-t0-repeat", aim, 0, true);
                images.Add(repeated);
                if (repeated.sha256 != images.First(i => i.file == prefix + "-t0.png").sha256)
                    throw new InvalidDataException("Fixed-time repeat was not pixel-identical: " + prefix);
            }
            receipt.repeatedTimeZeroPngIdentical = true;
            foreach (var message in ShaderUtil.GetShaderMessages(shader))
                if (message.severity.ToString() == "Error") Errors.Add(message.message);
            if (Errors.Count != 0) throw new InvalidDataException("Editor rendering logged errors.");
            if (Hash(receipt.bundlePath) != receipt.bundleSha256) throw new InvalidDataException("Input bundle changed during capture.");
            exitCode = 0;
        }
        catch (Exception ex) { Errors.Add(ex.ToString()); Debug.LogException(ex); }
        finally
        {
            receipt.images = images.ToArray();
            receipt.errors = Errors.Distinct().ToArray();
            Directory.CreateDirectory(output);
            File.WriteAllText(Path.Combine(output, "preview.json"), JsonUtility.ToJson(receipt, true));
            Application.logMessageReceived -= CaptureError;
            GraphicsSettings.defaultRenderPipeline = null;
            QualitySettings.renderPipeline = null;
            for (int i = Owned.Count - 1; i >= 0; i--) if (Owned[i] != null) Object.DestroyImmediate(Owned[i]);
            if (bundle != null) bundle.Unload(true);
            Debug.Log("[WorldLookPreview] " + (exitCode == 0 ? "PASS " : "FAIL ") + output);
            EditorApplication.Exit(exitCode);
        }
    }

    private static void ConfigurePipeline()
    {
        var renderer = Keep(ScriptableObject.CreateInstance<UniversalRendererData>());
        renderer.copyDepthMode = CopyDepthMode.AfterOpaques;
        renderer.rendererFeatures.Add(Keep(ScriptableObject.CreateInstance<WorldLookPreviewClock>()));
        var pipeline = Keep(UniversalRenderPipelineAsset.Create(renderer));
        pipeline.supportsCameraDepthTexture = true;
        pipeline.supportsCameraOpaqueTexture = true;
        pipeline.supportsHDR = true;
        pipeline.msaaSampleCount = 1;
        pipeline.renderScale = 1;
        var serialized = new SerializedObject(pipeline);
        serialized.FindProperty("m_OpaqueDownsampling").intValue = (int)Downsampling.None;
        serialized.ApplyModifiedPropertiesWithoutUndo();
        GraphicsSettings.defaultRenderPipeline = pipeline;
        QualitySettings.renderPipeline = pipeline;
    }

    private static void BuildFloor()
    {
        var lit = Shader.Find("Universal Render Pipeline/Lit");
        if (lit == null || !lit.isSupported) throw new InvalidOperationException("URP Lit unavailable.");
        var floor = Keep(new Material(lit));
        floor.SetFloat("_Smoothness", 0);
        var checker = Keep(new Texture2D(64, 64, TextureFormat.RGBA32, true));
        checker.SetPixels(Enumerable.Range(0, 64 * 64).Select(i =>
            ((i % 64 / 32) + (i / 64 / 32)) % 2 == 0 ? Color.gray : Color.white).ToArray());
        checker.filterMode = FilterMode.Trilinear;
        checker.anisoLevel = 4;
        checker.wrapMode = TextureWrapMode.Repeat;
        checker.Apply();
        floor.SetTexture("_BaseMap", checker);
        floor.SetTextureScale("_BaseMap", new Vector2(50, 50));
        floor.SetColor("_BaseColor", new Color(.55f, .55f, .55f, 1));
        Primitive("Submerged checker floor", PrimitiveType.Plane, new Vector3(0, -1.8f, 50), new Vector3(20, 1, 20), floor);
        Color[] colors = { new Color(.7f, .18f, .12f), new Color(.12f, .5f, .2f), new Color(.13f, .28f, .7f), new Color(.7f, .6f, .17f) };
        for (int i = 0; i < colors.Length; i++)
        {
            var material = Keep(new Material(lit));
            material.SetColor("_BaseColor", colors[i]);
            material.SetFloat("_Smoothness", 0);
            for (int row = 0; row < 3; row++)
                Primitive("Submerged color " + i + "/" + row, PrimitiveType.Cube,
                    new Vector3((i - 1.5f) * 3, -1.15f, 2 + row * 12), new Vector3(1.6f, .9f, 2.2f), material);
        }
    }

    private static Renderer Primitive(string name, PrimitiveType type, Vector3 position, Vector3 scale, Material material)
    {
        var go = Keep(GameObject.CreatePrimitive(type));
        go.name = name;
        go.transform.position = position;
        go.transform.localScale = scale;
        Object.DestroyImmediate(go.GetComponent<Collider>());
        var renderer = go.GetComponent<Renderer>();
        renderer.sharedMaterial = material;
        renderer.shadowCastingMode = ShadowCastingMode.Off;
        return renderer;
    }

    private static ImageRecord Capture(Camera camera, RenderTexture target, string output, string name, Vector3 aim, float seconds, bool water)
    {
        FixedTime = seconds;
        int before = ClockExecutions;
        RenderPipeline.SubmitRenderRequest(camera, new UniversalRenderPipeline.SingleCameraRequest { destination = target });
        if (ClockExecutions != before + 1) throw new InvalidOperationException("Fixed-time render pass did not execute exactly once.");
        var image = new ImageRecord { file = name + ".png", cameraPosition = camera.transform.position, cameraTarget = aim,
            seconds = seconds, water = water, minRgb = float.MaxValue, maxRgb = float.MinValue };
        RenderTexture previous = RenderTexture.active;
        var pixels = new Texture2D(Width, Height, TextureFormat.RGBAFloat, false, true);
        var png = new Texture2D(Width, Height, TextureFormat.RGBA32, false, false);
        try
        {
            RenderTexture.active = target;
            pixels.ReadPixels(new Rect(0, 0, Width, Height), 0, 0, false);
            Color[] values = pixels.GetPixels();
            for (int i = 0; i < values.Length; i++)
            {
                Color c = values[i];
                if (float.IsNaN(c.r) || float.IsInfinity(c.r) || float.IsNaN(c.g) || float.IsInfinity(c.g) ||
                    float.IsNaN(c.b) || float.IsInfinity(c.b) || float.IsNaN(c.a) || float.IsInfinity(c.a)) image.nonfinitePixels++;
                if (c.r >= .99f && c.b >= .99f && c.g <= .01f) image.errorMagentaPixels++;
                image.minRgb = Mathf.Min(image.minRgb, Mathf.Min(c.r, Mathf.Min(c.g, c.b)));
                image.maxRgb = Mathf.Max(image.maxRgb, Mathf.Max(c.r, Mathf.Max(c.g, c.b)));
                if (i > 0 && c != values[0]) image.spatiallyNonuniform = true;
            }
            // Encoding real readback pixels is not a CPU implementation of the water shader.
            png.SetPixels(values.Select(c => new Color(c.gamma.r, c.gamma.g, c.gamma.b, 1)).ToArray());
            png.Apply();
            string path = Path.Combine(output, image.file);
            File.WriteAllBytes(path, png.EncodeToPNG());
            image.sha256 = Hash(path);
            if (image.nonfinitePixels != 0 || image.errorMagentaPixels != 0 || !image.spatiallyNonuniform)
                throw new InvalidDataException("Nonfinite, error-magenta or uniform GPU output: " + image.file);
            return image;
        }
        finally { RenderTexture.active = previous; Object.DestroyImmediate(pixels); Object.DestroyImmediate(png); }
    }

    private static int CountDifferentPixels(string first, string second)
    {
        var a = new Texture2D(2, 2);
        var b = new Texture2D(2, 2);
        try
        {
            if (!a.LoadImage(File.ReadAllBytes(first)) || !b.LoadImage(File.ReadAllBytes(second))) throw new InvalidDataException("PNG readback failed.");
            Color32[] left = a.GetPixels32(), right = b.GetPixels32();
            return left.Where((color, i) => !color.Equals(right[i])).Count();
        }
        finally { Object.DestroyImmediate(a); Object.DestroyImmediate(b); }
    }

    private static T Keep<T>(T value) where T : Object { Owned.Add(value); return value; }
    private static void CaptureError(string message, string stack, LogType type)
    {
        if (type == LogType.Error || type == LogType.Exception || type == LogType.Assert) Errors.Add(message);
    }
    private static string Argument(string name)
    {
        string[] args = Environment.GetCommandLineArgs();
        int index = Array.IndexOf(args, name);
        if (index < 0 || index + 1 >= args.Length) throw new ArgumentException("Missing " + name);
        return args[index + 1];
    }
    private static string Hash(string path)
    {
        using (var stream = File.OpenRead(path))
        using (var sha = SHA256.Create()) return BitConverter.ToString(sha.ComputeHash(stream)).Replace("-", "").ToLowerInvariant();
    }
}

// Test-only pass inserted after URP's camera/time setup and immediately before transparents.
public sealed class WorldLookPreviewClock : ScriptableRendererFeature
{
    private sealed class ClockPass : ScriptableRenderPass
    {
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            float t = WorldLookWaterPreview.FixedTime;
            var cmd = CommandBufferPool.Get("WorldLook preview fixed time");
            cmd.SetGlobalVector("_Time", new Vector4(t / 20, t, t * 2, t * 3));
            cmd.SetGlobalVector("_SinTime", new Vector4(Mathf.Sin(t / 8), Mathf.Sin(t / 4), Mathf.Sin(t / 2), Mathf.Sin(t)));
            cmd.SetGlobalVector("_CosTime", new Vector4(Mathf.Cos(t / 8), Mathf.Cos(t / 4), Mathf.Cos(t / 2), Mathf.Cos(t)));
            cmd.SetGlobalVector("unity_DeltaTime", new Vector4(1f / 60, 60, 1f / 60, 60));
            cmd.SetGlobalVector("_TimeParameters", new Vector4(t, Mathf.Sin(t), Mathf.Cos(t), 0));
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
            WorldLookWaterPreview.ClockExecutions++;
        }
    }
    private ClockPass pass;
    public override void Create() { pass = new ClockPass { renderPassEvent = (RenderPassEvent)((int)RenderPassEvent.BeforeRenderingTransparents - 1) }; }
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData) { renderer.EnqueuePass(pass); }
}
