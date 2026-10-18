// S1Mods/Water — a self-contained URP water shader for the Schedule I graphics
// overhaul (Slice 3). It replaces the StylizedWater2 material on the ocean mesh
// with a fresher look: layered directional waves, depth-based colour falloff,
// crest foam, Blinn-Phong specular and a Fresnel sky tint. Built as an
// AssetBundle and loaded by the WaterShader mod at runtime.
//
// The game runs Unity 2022.3.62 (URP 14). Keep the shader on the URP forward
// path ("LightMode" = "UniversalForward") and avoid shader-graph dependencies
// so the compiled bundle stays portable between editor patch versions.
Shader "S1Mods/Water"
{
    Properties
    {
        _ShallowColor ("Shallow Color", Color) = (0.12, 0.55, 0.55, 0.85)
        _DeepColor    ("Deep Color",    Color) = (0.02, 0.12, 0.22, 0.95)
        _FoamColor    ("Foam Color",    Color) = (0.92, 0.97, 1.00, 1.00)

        _WaveHeight    ("Wave Height",    Range(0, 2))    = 0.18
        _WaveFrequency ("Wave Frequency", Range(0, 10))   = 2.2
        _WaveSpeed     ("Wave Speed",     Range(0, 4))    = 1.1
        _WaveCount     ("Wave Layers",    Range(1, 6))    = 4

        _FoamWidth   ("Foam Width",   Range(0, 2))   = 0.22
        _FoamStrength("Foam Strength",Range(0, 2))   = 1.0

        _DepthFade   ("Depth Fade",   Range(0.1, 10)) = 2.5
        _SpecPower   ("Specular Power", Range(8, 512)) = 128
        _SpecStrength("Specular Strength", Range(0, 2)) = 0.8
        _FresnelPower("Fresnel Power", Range(0.1, 8)) = 3.5
        _FresnelTint ("Sky Tint", Color) = (0.75, 0.85, 1.0, 1.0)
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
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS   : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float3 normalWS   : TEXCOORD1;
                float waveHeight  : TEXCOORD2;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _ShallowColor;
                float4 _DeepColor;
                float4 _FoamColor;
                float  _WaveHeight;
                float  _WaveFrequency;
                float  _WaveSpeed;
                float  _WaveCount;
                float  _FoamWidth;
                float  _FoamStrength;
                float  _DepthFade;
                float  _SpecPower;
                float  _SpecStrength;
                float  _FresnelPower;
                float4 _FresnelTint;
            CBUFFER_END

            // Layered directional waves: sum of sine bands, cheap and stable.
            // Returns the height offset; waveHeight (the 0..1 crest estimate) is
            // used downstream for foam.
            float SampleWaves(float2 xz, out float crest)
            {
                float height = 0.0;
                crest = 0.0;
                float amp = 1.0;
                float freq = _WaveFrequency;
                float2 dir = normalize(float2(1.0, 0.35));

                int layers = (int)_WaveCount;
                for (int i = 0; i < 6; i++)
                {
                    if (i >= layers) break;
                    float phase = dot(xz, dir) * freq + _Time.y * _WaveSpeed * (1.0 + 0.2 * i);
                    float w = sin(phase);
                    height += w * amp * _WaveHeight / (1.0 + i);
                    crest += saturate(w) * amp;
                    amp *= 0.62;
                    freq *= 1.7;
                    dir = float2(dir.x * 0.7 - dir.y * 0.7, dir.x * 0.7 + dir.y * 0.7);
                }
                return height;
            }

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                float crest;
                float h = SampleWaves(IN.positionOS.xz, crest);

                float3 posOS = IN.positionOS.xyz;
                posOS.y += h;

                OUT.positionWS = TransformObjectToWorld(posOS);
                OUT.positionCS = TransformWorldToHClip(OUT.positionWS);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.waveHeight = crest;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // Approximate a wave-perturbed normal from the analytic height.
                float crestA, crestB;
                float eps = 0.15;
                float h  = SampleWaves(IN.positionWS.xz, crestA);
                float hx = SampleWaves(IN.positionWS.xz + float2(eps, 0), crestB);
                float hz = SampleWaves(IN.positionWS.xz + float2(0, eps), crestB);
                float3 normalWS = normalize(float3(-(hx - h) / eps, 1.0, -(hz - h) / eps));

                // Depth-based colour falloff (view depth, cheap proxy: distance
                // to camera in world space scaled by the fade parameter).
                float viewDist = length(IN.positionWS - GetCameraPositionWS());
                float depthFac = saturate(viewDist / _DepthFade);
                float3 baseColor = lerp(_ShallowColor.rgb, _DeepColor.rgb, depthFac);

                // Lighting: URP main light with Blinn-Phong specular.
                Light mainLight = GetMainLight();
                float3 lightDir = normalize(mainLight.direction);
                float3 viewDir = normalize(GetCameraPositionWS() - IN.positionWS);
                float3 halfDir = normalize(lightDir + viewDir);

                float ndl = saturate(dot(normalWS, lightDir));
                float3 diffuse = baseColor * (ndl * mainLight.color + 0.25);

                float spec = pow(saturate(dot(normalWS, halfDir)), _SpecPower) * _SpecStrength;
                float3 specular = mainLight.color * spec;

                // Fresnel sky tint toward the horizon.
                float fresnel = pow(1.0 - saturate(dot(normalWS, viewDir)), _FresnelPower);
                float3 color = lerp(diffuse + specular, _FresnelTint.rgb, fresnel * 0.5);

                // Crest foam: high crest values near the wave tops.
                float foam = saturate((crestA - 0.55) / max(_FoamWidth, 0.001)) * _FoamStrength;
                color = lerp(color, _FoamColor.rgb, saturate(foam));

                return half4(color, lerp(_ShallowColor.a, _DeepColor.a, depthFac));
            }
            ENDHLSL
        }
    }

    FallBack Off
}
