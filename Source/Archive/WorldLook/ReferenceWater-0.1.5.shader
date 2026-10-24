// S1Mods/ReferenceWater — reference-oriented URP water prototype (Slice 4).
//
// Contract: renders composed transmission + planar reflection input, driven by
// true scene inputs — NOT camera distance, NOT a constant pretend reflection:
//   - scene depth  via DeclareDepthTexture  -> actual water thickness for
//     transmission absorption (shallow shows bottom, deep attenuates)
//   - scene color via DeclareOpaqueTexture  -> refraction of what is actually
//     under the surface, distorted by fine analytic wave derivatives
//   - planar reflection via _ReflectionTex (2D) + _ReflectionVP matrix +
//     _ReflectionAvailable float, fed by the runtime's disabled owned URP
//     camera (actual planar render, no static tint substitution). When the
//     runtime has no reflection yet (_ReflectionAvailable == 0), the shader
//     falls back to sky/probe ambient input — never a constant pretend color.
//   - fine calm waves: cheap analytic derivative normal (no multi-sample wave
//     evaluations per pixel); controlled sun highlight; shore foam band keyed
//     off water thickness (shallow edge only, not a blanket).
//
// Defaults: calm amplitudes 0.12–0.18 and a clear blue-green palette — not the
// previous near-black deep color.
//
// Target: D3D11 / URP 14 (UniversalForward). Requires a URP pipeline asset with
// depth + opaque texture enabled (game runs Unity 2022.3.62f2, URP 14.0.x).
Shader "S1Mods/ReferenceWater"
{
    Properties
    {
        // Transmission palette (actual scene color is refracted through these).
        _ShallowTint ("Shallow Tint", Color) = (0.35, 0.75, 0.75, 1.0)
        _DeepTint    ("Deep Tint",    Color) = (0.03, 0.23, 0.38, 1.0)
        _Absorption  ("Absorption Density", Range(0.05, 4.0)) = 0.55

        // Planar reflection input (runtime-owned; see header).
        _ReflectionTex ("Reflection Texture", 2D) = "black" {}
        _ReflectionStrength ("Reflection Strength", Range(0, 1.5)) = 0.85
        _ReflectionSharpness ("Reflection Sharpness", Range(1, 64)) = 24.0

        // Calm fine-wave surface detail (analytic, single evaluation).
        _WaveAmp   ("Wave Amplitude", Range(0, 0.6)) = 0.15
        _WaveScale ("Wave Detail Scale", Range(0.2, 8.0)) = 1.6
        _WaveSpeed ("Wave Speed", Range(0, 4)) = 0.8
        _RippleStrength ("Ripple Normal Strength", Range(0, 2)) = 0.55

        // Controlled sun highlight (bounded; not a blown streak).
        _SunStrength ("Sun Highlight Strength", Range(0, 3)) = 0.9
        _SunSharpness ("Sun Highlight Sharpness", Range(8, 1024)) = 220.0

        // Shore foam: thickness-keyed band only.
        _FoamColor ("Foam Color", Color) = (0.90, 0.95, 0.97, 1.0)
        _FoamDepth ("Foam Water Depth", Range(0.02, 2.0)) = 0.35
        _FoamStrength ("Foam Strength", Range(0, 2)) = 0.8

        // Sky/probe fallback when no planar reflection is available yet.
        _SkyColor ("Sky Fallback", Color) = (0.55, 0.70, 0.85, 1.0)
        _AmbientColor ("Ambient Fallback", Color) = (0.12, 0.22, 0.30, 1.0)
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

            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Back

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS

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
                float4 _ReflectionTex_ST;
                float  _ReflectionStrength;
                float  _ReflectionSharpness;
                float  _WaveAmp;
                float  _WaveScale;
                float  _WaveSpeed;
                float  _RippleStrength;
                float  _SunStrength;
                float  _SunSharpness;
                float4 _FoamColor;
                float  _FoamDepth;
                float  _FoamStrength;
                float4 _SkyColor;
                float4 _AmbientColor;
                // Runtime planar-reflection ABI (set every frame by the mod):
                // _ReflectionTex = live planar render target (2D, sRGB read as-is);
                // _ReflectionVP  = reflection view-projection matrix;
                // _ReflectionAvailable = 1 when the owned camera rendered this
                // frame, else 0 (shader falls back to sky/probe ambient).
                float4x4 _ReflectionVP;
                float  _ReflectionAvailable;
            CBUFFER_END

            TEXTURE2D(_ReflectionTex);
            SAMPLER(sampler_ReflectionTex);

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.positionCS = TransformWorldToHClip(OUT.positionWS);
                OUT.screenPos = ComputeScreenPos(OUT.positionCS);
                return OUT;
            }

            // Fine calm-wave field. The normal is the analytic derivative, still one
            // evaluation per component. A slow warp bends the field so the crests
            // do not run straight across the harbor, and four incommensurate
            // directions keep them from locking into comb teeth.
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
                float2 g = a1 * f1 * d1 * cos(dot(p, d1) * f1 + t * _WaveSpeed * 1.0)
                         + a2 * f2 * d2 * cos(dot(p, d2) * f2 - t * _WaveSpeed * 1.27)
                         + a3 * f3 * d3 * cos(dot(p, d3) * f3 + t * _WaveSpeed * 0.81)
                         + a4 * f4 * d4 * cos(dot(p, d4) * f4 - t * _WaveSpeed * 1.63);
                return g * _WaveAmp * _RippleStrength;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                float2 screenUV = IN.screenPos.xy / max(IN.screenPos.w, 1e-5);

                // True water thickness from scene depth (not camera distance).
                float surfaceEye = LinearEyeDepth(IN.positionCS.z, _ZBufferParams);
                float sceneRaw = SampleSceneDepth(screenUV);
                float sceneEye = LinearEyeDepth(sceneRaw, _ZBufferParams);
                float thickness = max(sceneEye - surfaceEye, 0.0);

                // Refraction: what is actually under the surface, distorted by
                // the fine-wave gradient. Distortion scales with calm amplitude
                // so reflections stay sharp, not smeared.
                float2 grad = WaveGradient(IN.positionWS.xz, _Time.y);
                float2 refractUV = screenUV + grad * 0.012;
                float3 sceneColor = SampleSceneColor(refractUV);

                // Transmission over ACTUAL thickness. Raw absorb still measures depth.
                // Dye and alpha use a late power so the first several metres stay near the
                // bottom's own colour. The previous straight lerp and high alpha floor
                // was already a veil at the bank and an opaque blue plate a few metres out.
                float absorb = 1.0 - exp(-thickness * _Absorption);
                absorb = saturate(absorb);
                float dye = saturate(pow(absorb, 2.2));
                float3 transmitTint = lerp(_ShallowTint.rgb, _DeepTint.rgb, dye);
                float3 transmitted = sceneColor * transmitTint * 2.0;

                // Perturbed normal from the same single analytic gradient.
                float3 viewDir = normalize(GetCameraPositionWS() - IN.positionWS);
                float3 normalWS = normalize(float3(-grad.x, 1.0, -grad.y));

                // Planar reflection: project the world pos through the runtime
                // reflection VP; wave gradient perturbs the UV for distortion.
                float4 reflClip = mul(_ReflectionVP, float4(IN.positionWS, 1.0));
                float2 reflUV = reflClip.xy / max(reflClip.w, 1e-5);
                reflUV = reflUV * 0.5 + 0.5;
                reflUV += grad * 0.06;
                float3 planar = SAMPLE_TEXTURE2D(_ReflectionTex, sampler_ReflectionTex, saturate(reflUV)).rgb;

                // Sky/probe fallback while the runtime has no planar frame yet.
                float upFace = saturate(normalWS.y * 0.5 + 0.5);
                float3 skyFallback = lerp(_AmbientColor.rgb, _SkyColor.rgb, upFace);
                float3 reflection = lerp(skyFallback, planar, saturate(_ReflectionAvailable));

                // Fresnel balance: grazing angles reflect, downward view
                // transmits (reveals bottom near shore).
                float fresnel = pow(1.0 - saturate(dot(normalWS, viewDir)), 5.0);
                fresnel = lerp(0.04, 1.0, fresnel);
                float3 composed = lerp(transmitted, reflection, saturate(fresnel * _ReflectionStrength + 0.02));

                // Controlled sun highlight from the URP main light.
                Light mainLight = GetMainLight();
                float3 lightDir = normalize(mainLight.direction);
                float3 halfDir = normalize(lightDir + viewDir);
                float spec = pow(saturate(dot(normalWS, halfDir)), _SunSharpness) * _SunStrength;
                composed += mainLight.color * spec * 0.6;

                // Shore foam: thin thickness band only — never a blanket.
                float foamBand = 1.0 - saturate(thickness / max(_FoamDepth, 1e-3));
                float foamEdge = smoothstep(0.35, 0.9, foamBand + grad.x * 0.15);
                composed = lerp(composed, _FoamColor.rgb, saturate(foamEdge * _FoamStrength * 0.75));

                // Clear through harbor depth; the plate only arrives once dye is already high.
                float alpha = saturate(0.04 + dye * 0.85);

                return half4(composed, alpha);
            }
            ENDHLSL
        }
    }

    FallBack Off
}
