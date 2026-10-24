// S1Mods/ReferenceWater — URP water without an object-reflection camera.
//
// Scene depth supplies an eye-space thickness approximation; scene color
// supplies the refracted opaque background. Reflection is a separate surface
// contribution, composed with premultiplied alpha so clear shallows do not
// attenuate Fresnel reflection a second time. The remaining destination weight
// preserves visibility through the surface; this is not volumetric transport.
//
// The runtime supplies only live sky/horizon colors. Their weak directional
// highlight cannot reproduce buildings, clouds or scene geometry. Independent
// body opacity keeps the water visible when the bottom depth is unavailable.
//
// Four warped wave components perturb surface normals, not mesh geometry.
// Wind changes their amplitude; screen-space filtering reduces distant aliasing.
// Direct sun highlights use the URP main-light shadow and attenuation inputs.
//
// Target: D3D11 / URP 14, Unity 2022.3.62f2. Depth and opaque textures must be
// available from the game's active camera; bundle build settings alone do not
// enable them in the running game.
Shader "S1Mods/ReferenceWater"
{
    Properties
    {
        // Transmission palette (actual scene color is refracted through these).
        _ShallowTint ("Shallow Tint", Color) = (0.35, 0.75, 0.75, 1.0)
        _DeepTint    ("Deep Tint",    Color) = (0.03, 0.23, 0.38, 1.0)
        _Absorption  ("Absorption Density", Range(0.05, 4.0)) = 0.55
        _SurfaceOpacity ("Minimum Water Body Opacity", Range(0.1, 0.65)) = 0.22
        _ReflectionStrength ("Sky Highlight Strength", Range(0, 0.25)) = 0.12

        // Calm fine-wave surface detail (analytic, single evaluation).
        _WaveAmp   ("Wave Amplitude", Range(0, 0.6)) = 0.15
        _WaveScale ("Wave Detail Scale", Range(0.2, 8.0)) = 1.6
        _WaveSpeed ("Wave Speed", Range(0, 4)) = 0.8
        _RippleStrength ("Ripple Normal Strength", Range(0, 2)) = 0.55
        _WindActivity ("Wind Activity", Range(0, 1)) = 0.0

        // Controlled sun highlight (bounded; not a blown streak).
        _SunStrength ("Sun Highlight Strength", Range(0, 3)) = 0.9
        _SunSharpness ("Sun Highlight Sharpness", Range(8, 1024)) = 220.0

        // Shore foam: thickness-keyed band only.
        _FoamColor ("Foam Color", Color) = (0.90, 0.95, 0.97, 1.0)
        _FoamDepth ("Foam Water Depth", Range(0.02, 2.0)) = 0.35
        _FoamStrength ("Foam Strength", Range(0, 2)) = 0.8

        // Runtime writes current sky and horizon colors, including at night.
        _SkyColor ("Sky Zenith Fallback", Color) = (0.55, 0.70, 0.85, 1.0)
        _AmbientColor ("Sky Horizon Fallback", Color) = (0.12, 0.22, 0.30, 1.0)
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
        }

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            Blend One OneMinusSrcAlpha
            ZWrite Off
            Cull Back

            HLSLPROGRAM
            #pragma target 3.0
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma multi_compile_fragment _ _SHADOWS_SOFT

            // Transparent water samples the main light's shadow map, rather
            // than treating an opaque object's screen-space shadow as its own.
            #define _SURFACE_TYPE_TRANSPARENT 1

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareOpaqueTexture.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float4 screenPos  : TEXCOORD1;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _ShallowTint;
                float4 _DeepTint;
                float  _Absorption;
                float  _SurfaceOpacity;
                float  _ReflectionStrength;
                float  _WaveAmp;
                float  _WaveScale;
                float  _WaveSpeed;
                float  _RippleStrength;
                float  _WindActivity;
                float  _SunStrength;
                float  _SunSharpness;
                float4 _FoamColor;
                float  _FoamDepth;
                float  _FoamStrength;
                float4 _SkyColor;
                float4 _AmbientColor;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.positionCS = TransformWorldToHClip(OUT.positionWS);
                OUT.screenPos = ComputeScreenPos(OUT.positionCS);
                return OUT;
            }

            // Analytic normal field with four wave directions and a slow domain
            // warp. Pixel footprint filtering fades waves too fine for a pixel.
            float2 WaveGradient(float2 xz, float t)
            {
                float2 warp = float2(
                    sin(dot(xz, float2(0.17, 0.11)) + t * 0.07),
                    cos(dot(xz, float2(-0.09, 0.19)) - t * 0.05));
                float2 p = xz * _WaveScale + warp * 1.8;
                float2 d1 = normalize(float2(0.82, 0.57));
                float2 d2 = normalize(float2(-0.63, 0.77));
                float2 d3 = normalize(float2(0.24, -0.97));
                float2 d4 = normalize(float2(-0.91, -0.41));
                float f1 = 1.0;
                float f2 = 2.37;
                float f3 = 4.19;
                float f4 = 6.83;
                float a1 = 1.0;
                float a2 = 0.42;
                float a3 = 0.16;
                float a4 = 0.07;
                float footprint = max(length(ddx(p)), length(ddy(p)));
                float4 filter = 1.0 - smoothstep(0.5, 2.4, footprint * float4(f1, f2, f3, f4));
                float2 g = filter.x * a1 * f1 * d1 * cos(dot(p, d1) * f1 + t * _WaveSpeed * 1.0)
                         + filter.y * a2 * f2 * d2 * cos(dot(p, d2) * f2 - t * _WaveSpeed * 1.27)
                         + filter.z * a3 * f3 * d3 * cos(dot(p, d3) * f3 + t * _WaveSpeed * 0.81)
                         + filter.w * a4 * f4 * d4 * cos(dot(p, d4) * f4 - t * _WaveSpeed * 1.63);
                // Windy is a weather weight, not a velocity. Modulating phase
                // with a changing speed would jump as elapsed time increases.
                float windAmplitude = lerp(0.85, 1.35, saturate(_WindActivity));
                return g * _WaveAmp * _RippleStrength * windAmplitude;
            }

            float3 SafeWaterDirection(float3 direction)
            {
                return direction * rsqrt(max(dot(direction, direction), 1e-8));
            }

            float HasSceneDepth(float rawDepth)
            {
                // The target D3D11 depth range is 0..1. Cleared/far values do
                // not provide a bottom depth and must not create shore foam.
                return rawDepth > 1e-6 && rawDepth < (1.0 - 1e-6) ? 1.0 : 0.0;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float2 screenUV = saturate(IN.screenPos.xy / max(IN.screenPos.w, 1e-5));

                // Eye-space depth difference is an approximation to the path
                // through water. It is not a vertical depth measurement.
                float surfaceEye = LinearEyeDepth(IN.positionCS.z, _ZBufferParams);
                float sceneRaw = SampleSceneDepth(screenUV);
                float sceneEye = LinearEyeDepth(sceneRaw, _ZBufferParams);
                float depthValid = HasSceneDepth(sceneRaw) * step(surfaceEye, sceneEye);
                float shoreThickness = max(sceneEye - surfaceEye, 0.0);

                // Reject distorted samples landing on geometry in front of the
                // water. The accepted color and absorption use the same depth.
                float2 grad = WaveGradient(IN.positionWS.xz, _Time.y);
                float2 refractUV = saturate(screenUV + grad * 0.012);
                float refractedRaw = SampleSceneDepth(refractUV);
                float refractedEye = LinearEyeDepth(refractedRaw, _ZBufferParams);
                float refractionValid = HasSceneDepth(refractedRaw) * step(surfaceEye + 1e-3, refractedEye);
                refractUV = lerp(screenUV, refractUV, refractionValid);
                float thickness = max(lerp(sceneEye, refractedEye, refractionValid) - surfaceEye, 0.0);
                float3 sceneColor = SampleSceneColor(refractUV);

                // Retain the clear-shallows dye curve. Its opacity describes
                // the water-body contribution, independently of reflection.
                float absorb = 1.0 - exp(-thickness * _Absorption);
                absorb = saturate(absorb);
                // No valid bottom means deep water, not a vanished surface.
                float dye = lerp(1.0, saturate(pow(absorb, 2.2)), depthValid);
                float3 transmitTint = lerp(_ShallowTint.rgb, _DeepTint.rgb, dye);
                float3 environmentLight = max(lerp(_AmbientColor.rgb, _SkyColor.rgb, 0.35), 0.0);
                float3 deepBody = _DeepTint.rgb * environmentLight * 1.5;
                float3 transmitted = lerp(deepBody, sceneColor * transmitTint * 2.0, depthValid);

                // Perturbed normal from the same single analytic gradient.
                float3 viewDir = SafeWaterDirection(GetCameraPositionWS() - IN.positionWS);
                float3 normalWS = normalize(float3(-grad.x, 1.0, -grad.y));

                float3 reflectedDirection = reflect(-viewDir, normalWS);
                float skyElevation = smoothstep(0.0, 0.65, saturate(reflectedDirection.y));
                float3 reflection = lerp(_AmbientColor.rgb, _SkyColor.rgb, skyElevation);

                // Fresnel balance: grazing angles reflect, downward view
                // transmits (reveals bottom near shore).
                float fresnel = pow(1.0 - saturate(dot(normalWS, viewDir)), 5.0);
                fresnel = lerp(0.02, 1.0, fresnel);
                float reflectionWeight = saturate(fresnel * clamp(_ReflectionStrength, 0.0, 0.25));
                float bodyOpacity = lerp(clamp(_SurfaceOpacity, 0.1, 0.65), 0.89, dye);
                float bodyWeight = (1.0 - reflectionWeight) * bodyOpacity;
                float alpha = reflectionWeight + bodyWeight;
                float3 composed = reflection * reflectionWeight + transmitted * bodyWeight;

                // Controlled sun highlight from the URP main light.
                Light mainLight = GetMainLight(TransformWorldToShadowCoord(IN.positionWS));
                float3 lightDir = SafeWaterDirection(mainLight.direction);
                float3 halfDir = SafeWaterDirection(lightDir + viewDir);
                float spec = pow(saturate(dot(normalWS, halfDir)), _SunSharpness) * _SunStrength;
                float shadow = lerp(mainLight.shadowAttenuation, 1.0, GetMainLightShadowFade(IN.positionWS));
                float attenuation = mainLight.distanceAttenuation * shadow * saturate(dot(normalWS, lightDir));
                composed += mainLight.color * spec * attenuation * 0.6;

                // Foam uses the undistorted contact depth, so refraction does
                // not pull the shore band onto foreground objects.
                float foamBand = 1.0 - saturate(shoreThickness / max(_FoamDepth, 1e-3));
                float foamEdge = smoothstep(0.35, 0.9, foamBand + grad.x * 0.15);
                float foamOpacity = saturate(foamEdge * _FoamStrength * 0.75) * depthValid;
                // Approximate diffuse sky illumination plus the direct main
                // light keeps the unchanged foam pigment from glowing at night.
                float3 foamLighting = saturate(lerp(_AmbientColor.rgb, _SkyColor.rgb, 0.35)
                    + mainLight.color * attenuation);
                composed = composed * (1.0 - foamOpacity) + _FoamColor.rgb * foamLighting * foamOpacity;
                alpha = alpha * (1.0 - foamOpacity) + foamOpacity;

                // RGB is already premultiplied. The blend stage contributes
                // (1-alpha) of the existing scene exactly once.
                return half4(composed, alpha);
            }
            ENDHLSL
        }
    }

    FallBack Off
}
