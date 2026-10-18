using System;
using System.Collections.Generic;

namespace WaterShader;

/// <summary>
/// Unity-free decisions and math for the reference water reflection path.
/// Row-major 4x4 arrays keep the reflection math testable without UnityEngine.
/// The runtime consumes only MirrorPoint/MirrorDirection; ReflectionMatrix is
/// the matrix those two are derived from and exists to pin that relationship.
/// </summary>
internal static class ReferenceWaterPolicy
{
    /// <summary>Shader properties the reference shader contract requires.</summary>
    internal static readonly string[] RequiredShaderProperties =
    {
        "_ShallowTint", "_DeepTint", "_Absorption",
        "_ReflectionTex", "_ReflectionStrength", "_ReflectionSharpness",
        "_WaveAmp", "_WaveScale", "_WaveSpeed", "_RippleStrength",
        "_SunStrength", "_SunSharpness",
        "_FoamColor", "_FoamDepth", "_FoamStrength",
        "_SkyColor", "_AmbientColor",
    };

    /// <summary>
    /// Reflection across the horizontal plane y = waterY maps (x, y, z) to
    /// (x, 2*waterY - y, z). Returned as a row-major 4x4.
    /// </summary>
    internal static float[] ReflectionMatrix(float waterY)
    {
        return new[]
        {
            1f, 0f, 0f, 0f,
            0f, -1f, 0f, 2f * waterY,
            0f, 0f, 1f, 0f,
            0f, 0f, 0f, 1f,
        };
    }

    /// <summary>Mirror a world point across y = waterY.</summary>
    internal static (float x, float y, float z) MirrorPoint(float waterY, float x, float y, float z)
    {
        return (x, 2f * waterY - y, z);
    }

    /// <summary>Mirror a direction across y = waterY (only the y component flips).</summary>
    internal static (float x, float y, float z) MirrorDirection(float x, float y, float z)
    {
        return (x, -y, z);
    }

    /// <summary>Clamp the reflection render-target size to the supported budget.</summary>
    internal static (int width, int height) ReflectionSize(int requestedWidth, int requestedHeight, int maxDimension)
    {
        int max = Math.Clamp(maxDimension, 128, 1024);
        int width = Math.Clamp(requestedWidth, 16, max);
        int height = Math.Clamp(requestedHeight, 16, max);
        return (width, height);
    }

    /// <summary>Decide whether a camera should trigger the owned reflection render.</summary>
    internal static bool ShouldRenderReflection(
        bool reentrancyGateOpen,
        bool cameraIsOwned,
        bool cameraIsReflection,
        bool cameraIsOverlay,
        bool cameraIsPreview,
        bool cameraIsGame,
        bool materialBound)
    {
        return reentrancyGateOpen
               && !cameraIsOwned
               && !cameraIsReflection
               && !cameraIsOverlay
               && !cameraIsPreview
               && cameraIsGame
               && materialBound;
    }

    /// <summary>Decide whether the reference shader may replace the clone's shader.</summary>
    internal static bool CanApplyReference(bool bundleLoaded, bool shaderSupported, bool hasRequiredProperties)
    {
        return bundleLoaded && shaderSupported && hasRequiredProperties;
    }

    /// <summary>List contract property names the supplied probe reports as absent.</summary>
    internal static string[] MissingRequiredProperties(Func<string, bool> hasProperty)
    {
        if (hasProperty == null) throw new ArgumentNullException(nameof(hasProperty));

        var missing = new List<string>();
        foreach (string name in RequiredShaderProperties)
        {
            if (!hasProperty(name)) missing.Add(name);
        }

        return missing.ToArray();
    }

    /// <summary>Decide whether reflection inputs may be bound this frame.</summary>
    internal static bool ShouldBindReflection(bool materialBound, bool reflectionTextureReady)
    {
        return materialBound && reflectionTextureReady;
    }

    /// <summary>
    /// Scale a source resolution down to a bounded budget while preserving its
    /// aspect ratio. Unlike <see cref="ReflectionSize"/> this keeps the shape of
    /// the frame, which a per-axis clamp would distort at high resolutions.
    /// </summary>
    internal static (int width, int height) ReflectionSizeForAspect(int sourceWidth, int sourceHeight, int maxDimension)
    {
        int max = Math.Clamp(maxDimension, 128, 1024);
        if (sourceWidth <= 0 || sourceHeight <= 0) return (max, max);

        int longSide = Math.Max(sourceWidth, sourceHeight);
        float scale = Math.Min(1f, max / (float)longSide);
        int width = Math.Max(16, (int)Math.Round(sourceWidth * scale));
        int height = Math.Max(16, (int)Math.Round(sourceHeight * scale));
        return (width, height);
    }
}
