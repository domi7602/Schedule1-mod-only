using System;
using System.Collections.Generic;

namespace GraphicsProbe;

internal sealed class RendererScanBudget
{
    private readonly int _surfaceLimit, _effectLimit;
    public int SurfaceScanned { get; private set; }
    public int EffectsScanned { get; private set; }
    public int SurfaceScanOmissions { get; private set; }
    public int EffectScanOmissions { get; private set; }
    public Dictionary<string, int> CountsByNativeType { get; } = new();

    public RendererScanBudget(int surfaceLimit, int effectLimit)
    { _surfaceLimit = Math.Max(0, surfaceLimit); _effectLimit = Math.Max(0, effectLimit); }

    public static bool IsEffect(string type) => type == "UnityEngine.ParticleSystemRenderer"
        || type == "UnityEngine.LineRenderer" || type == "UnityEngine.TrailRenderer";

    public bool TryScan(string type, out bool effect)
    {
        effect = IsEffect(type);
        if (!CountsByNativeType.ContainsKey(type) && CountsByNativeType.Count >= 32) type = "<other-native-types: key cap>";
        CountsByNativeType.TryGetValue(type, out int count);
        CountsByNativeType[type] = count + 1;
        if (effect)
        {
            if (EffectsScanned >= _effectLimit) { EffectScanOmissions++; return false; }
            EffectsScanned++;
        }
        else
        {
            if (SurfaceScanned >= _surfaceLimit) { SurfaceScanOmissions++; return false; }
            SurfaceScanned++;
        }
        return true;
    }
}
