using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace GraphicsProbe;

internal static class EnvironmentReader
{
    public static ObservedRecord Settings()
    {
        var row = new ObservedRecord();
        row.Read("applicationVersion", () => ReportPolicy.Text(Application.version));
        row.Read("unityVersion", () => ReportPolicy.Text(Application.unityVersion));
        row.Read("productName", () => ReportPolicy.Text(Application.productName));
        row.Read("graphicsDeviceName", () => ReportPolicy.Text(SystemInfo.graphicsDeviceName));
        row.Read("graphicsDeviceVendor", () => ReportPolicy.Text(SystemInfo.graphicsDeviceVendor));
        row.Read("graphicsDeviceApi", () => SystemInfo.graphicsDeviceType.ToString());
        row.Read("graphicsDeviceVersion", () => ReportPolicy.Text(SystemInfo.graphicsDeviceVersion));
        row.Read("graphicsMemoryMB", () => SystemInfo.graphicsMemorySize);
        row.Read("graphicsShaderLevel", () => SystemInfo.graphicsShaderLevel);
        row.Read("activeColorSpace", () => QualitySettings.activeColorSpace.ToString());
        row.Read("screenWidth", () => Screen.width);
        row.Read("screenHeight", () => Screen.height);
        row.Read("displayResolution", () => ReportPolicy.Text(Screen.currentResolution.ToString()));
        row.Read("fullScreenMode", () => Screen.fullScreenMode.ToString());
        row.Read("qualityIndex", () => QualitySettings.GetQualityLevel());
        row.Read("qualityName", () =>
        {
            int index = QualitySettings.GetQualityLevel();
            var names = QualitySettings.names;
            return index >= 0 && index < names.Length ? ReportPolicy.Text(names[index]) : "<out-of-range>";
        });
        row.Read("qualityAntiAliasing", () => QualitySettings.antiAliasing);
        row.Read("vSyncCount", () => QualitySettings.vSyncCount);
        row.Read("currentRuntimePipelineType", () =>
        {
            // Installed interop exposes a direct static field read, not a pipeline-creation accessor.
            var pipeline = RenderPipelineManager.s_CurrentPipeline;
            return pipeline == null ? null : ObjectReader.NativeType(pipeline);
        });
        row.Read("currentPipelineAsset", () => Pipeline(GraphicsSettings.currentRenderPipeline));
        row.Read("qualityPipelineOverride", () => Pipeline(QualitySettings.renderPipeline));
        row.Read("defaultPipelineAsset", () => Pipeline(GraphicsSettings.defaultRenderPipeline));
        row["pipelineInterpretation"] = "Asset types and settings as observed; null current asset is consistent with built-in rendering, not proof of active passes. Renderer/pass resolution intentionally omitted.";
        return row;
    }

    private static object? Pipeline(RenderPipelineAsset asset)
    {
        if (asset == null) return null;
        var row = new ObservedRecord();
        row.Read("name", () => ReportPolicy.Text(asset.name));
        row.Read("nativeType", () => ObjectReader.NativeType(asset));
        row.Read("urpSerializedConfiguration", () =>
        {
            var urp = asset.TryCast<UniversalRenderPipelineAsset>();
            if (urp == null) return null;
            var u = new ObservedRecord();
            u.Read("requireDepthTexture", () => urp.m_RequireDepthTexture);
            u.Read("requireOpaqueTexture", () => urp.m_RequireOpaqueTexture);
            u.Read("opaqueDownsampling", () => urp.m_OpaqueDownsampling.ToString());
            u.Read("supportsHDR", () => urp.m_SupportsHDR);
            u.Read("msaa", () => urp.m_MSAA.ToString());
            u.Read("renderScale", () => urp.m_RenderScale);
            u.Read("defaultRendererIndex", () => urp.m_DefaultRendererIndex);
            u.Read("rendererType", () => urp.m_RendererType.ToString());
            u.Read("shadowDistance", () => urp.m_ShadowDistance);
            u.Read("shadowCascadeCount", () => urp.m_ShadowCascadeCount);
            u.Read("supportsMainLightShadows", () => urp.m_MainLightShadowsSupported);
            u.Read("supportsAdditionalLightShadows", () => urp.m_AdditionalLightShadowsSupported);
            u.Read("useSRPBatcher", () => urp.m_UseSRPBatcher);
            u.Read("supportsDynamicBatching", () => urp.m_SupportsDynamicBatching);
            u.Read("rendererDataDescriptors", () =>
            {
                var list = urp.m_RendererDataList;
                int count = list == null ? 0 : list.Length;
                int scanned = Math.Min(count, 16);
                var entries = new List<ObservedRecord>();
                for (int i = 0; i < scanned; i++)
                {
                    int index = i;
                    var entry = new ObservedRecord { ["index"] = index };
                    entry.Read("nativeType", () => list![index] == null ? "<missing>" : ObjectReader.NativeType(list![index]));
                    entry.Read("name", () => list![index] == null ? null : ReportPolicy.Text(list![index].name));
                    entries.Add(entry);
                }
                return new { totalObserved = count, scanned, kept = entries.Count, truncated = count > scanned, items = entries };
            });
            u["passAvailability"] = "unresolved: no renderer construction, renderer resolution or feature/pass execution";
            return u;
        });
        return row;
    }

    public static ObservedRecord Environment()
    {
        var row = new ObservedRecord();
        row.Read("skyboxSharedMaterial", () => ObjectReader.MaterialDetails(RenderSettings.skybox, includeProperties: true));
        row.Read("fog", () => RenderSettings.fog);
        row.Read("fogMode", () => RenderSettings.fogMode.ToString());
        row.Read("fogColor", () => ObjectReader.ColorValue(RenderSettings.fogColor));
        row.Read("fogDensity", () => RenderSettings.fogDensity);
        row.Read("fogStartDistance", () => RenderSettings.fogStartDistance);
        row.Read("fogEndDistance", () => RenderSettings.fogEndDistance);
        row.Read("ambientMode", () => RenderSettings.ambientMode.ToString());
        row.Read("ambientIntensity", () => RenderSettings.ambientIntensity);
        row.Read("ambientLight", () => ObjectReader.ColorValue(RenderSettings.ambientLight));
        row.Read("ambientSkyColor", () => ObjectReader.ColorValue(RenderSettings.ambientSkyColor));
        row.Read("ambientEquatorColor", () => ObjectReader.ColorValue(RenderSettings.ambientEquatorColor));
        row.Read("ambientGroundColor", () => ObjectReader.ColorValue(RenderSettings.ambientGroundColor));
        row.Read("sun", () =>
        {
            var sun = RenderSettings.sun;
            return sun == null ? null : ObjectReader.LightDetails(sun);
        });
        row.Read("inGameTime", () =>
        {
            var timeManager = Il2CppScheduleOne.GameTime.TimeManager.Instance;
            if (!S1Mods.Shared.NetworkGuard.IsAlive(timeManager)) return null;
            int current = timeManager.CurrentTime;
            var t = new ObservedRecord();
            t.Read("currentTime", () => current);
            t.Read("currentDay", () => timeManager.CurrentDay.ToString());
            t.Read("elapsedDays", () => timeManager.ElapsedDays);
            t.Read("displayTime", () => ReportPolicy.Text(Il2CppScheduleOne.GameTime.TimeManager.GetMinutesToDisplayTime(current)));
            return t;
        });
        row.Read("bakedLightProbesObjectType", () => LightmapSettings.lightProbes == null ? null : ObjectReader.NativeType(LightmapSettings.lightProbes));
        row["limitations"] = "No light-probe positions/coefficients, GPU buffers or rendered pixels read. Volume descriptors are separate, not evaluated blends. Shader property values and light values are point-in-time observations; the day/night cycle may rewrite them.";
        return row;
    }
}
