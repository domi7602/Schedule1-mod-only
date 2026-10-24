// WorldLook's procedural cloud sky on Unity's native skybox bindings.
// Original sky/cloud/celestial calculations, adapted only to UnityCG syntax.
// This remains a 2.5D appearance model, not a volumetric atmosphere.
Shader "S1Mods/WorldLookSky"
{
    Properties
    {
        // Exact declared Custom/ScheduleOneSky contract from the supplied probe.
        // Runtime preserves live values, HDR colors and the game's sun/moon maps.
        _SunSizeModifier ("Native Sun Size", Float) = 0.15
        _SunTexture ("Native Sun Texture", 2D) = "white" {}
        [HDR] _SunShine ("Native Sun Shine", Color) = (12, 11, 10, 1)
        _MoonSizeModifier ("Native Moon Size", Float) = 0.15
        _MoonTexture ("Native Moon Texture", 2D) = "white" {}
        [HDR] _MoonShine ("Native Moon Shine", Color) = (8, 8, 8, 1)
        [HDR] _StarShine ("Native Star Shine", Color) = (40, 40, 40, 1)
        _StarFadeOutStart ("Native Star Fade Out Start", Float) = 0.074
        _StarFadeOutEnd ("Native Star Fade Out End", Float) = 0.277
        _StarFadeInStart ("Native Star Fade In Start", Float) = 0.770
        _StarFadeInEnd ("Native Star Fade In End", Float) = 0.961
        _FogInfluence ("Native Horizon Haze Influence", Float) = 1

        _WLSkyTop ("Sky Zenith", Color) = (0.24, 0.40, 0.61, 1)
        _WLSkyHorizon ("Sky Horizon", Color) = (0.64, 0.71, 0.77, 1)
        _WLSkyGround ("Sky Lower Hemisphere", Color) = (0.35, 0.39, 0.43, 1)
        _WLCloudLight ("Cloud Light", Color) = (0.96, 0.97, 0.98, 1)
        _WLCloudShadow ("Cloud Shadow", Color) = (0.46, 0.51, 0.58, 1)
        _WLCloudCoverage ("Cloud Coverage", Range(0, 1)) = 0.55
        _WLCloudDensity ("Cloud Density", Range(0.05, 1.5)) = 0.75
        _WLCloudScale ("Cloud Scale", Range(0.25, 4)) = 1
        _WLCloudOpacity ("Cloud Opacity", Range(0, 1)) = 0.88
        [HideInInspector] _WLCloudOffset ("Cloud Offset", Vector) = (19.3, 47.1, 0, 0)
        [HideInInspector] _WLSunDirection ("Direction Toward Sun", Vector) = (0.4, 0.8, 0.2, 0)
        [HideInInspector] _WLMoonDirection ("Direction Toward Moon", Vector) = (-0.4, -0.8, -0.2, 0)
        [HideInInspector] _WLDaylight ("Daylight", Float) = 1
        [HideInInspector] _WLDayFraction ("Native Day Fraction", Float) = 0.5
        [HideInInspector] _WLSunVisible ("Native Sun Available", Float) = 1
        [HideInInspector] _WLMoonVisible ("Native Moon Available", Float) = 0
        [HideInInspector] _WLMeshMode ("Native Sky Mesh Mode", Float) = 0
    }

    SubShader
    {
        Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" "RenderPipeline"="UniversalPipeline" }
        Cull Off
        ZWrite Off
        ZTest LEqual

        Pass
        {
            Name "WorldLookSky"
            // Untagged, like Unity's built-in skybox passes. The native skybox
            // renderer does not use the ordinary forward-object pass selection.

            CGPROGRAM
            #pragma target 3.5
            #pragma vertex Vert
            #pragma fragment Frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"

            CBUFFER_START(UnityPerMaterial)
                float _SunSizeModifier;
                float4 _SunTexture_ST;
                float4 _SunShine;
                float _MoonSizeModifier;
                float4 _MoonTexture_ST;
                float4 _MoonShine;
                float4 _StarShine;
                float _StarFadeOutStart, _StarFadeOutEnd;
                float _StarFadeInStart, _StarFadeInEnd;
                float _FogInfluence;
                float4 _WLSkyTop, _WLSkyHorizon, _WLSkyGround;
                float4 _WLCloudLight, _WLCloudShadow;
                float _WLCloudCoverage, _WLCloudDensity, _WLCloudScale, _WLCloudOpacity;
                float4 _WLCloudOffset, _WLSunDirection, _WLMoonDirection;
                float _WLDaylight, _WLDayFraction, _WLSunVisible, _WLMoonVisible, _WLMeshMode;
            CBUFFER_END

            sampler2D _SunTexture;
            sampler2D _MoonTexture;

            struct Attributes
            {
                float4 positionOS : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 directionWS : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            Varyings Vert(Attributes input)
            {
                Varyings output;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
                output.positionCS = UnityObjectToClipPos(input.positionOS);
                float3 worldPosition = mul(unity_ObjectToWorld, float4(input.positionOS.xyz, 1.0)).xyz;
                float3 cubeDirection = mul((float3x3)unity_ObjectToWorld, input.positionOS.xyz);
                output.directionWS = lerp(cubeDirection, worldPosition - _WorldSpaceCameraPos, saturate(_WLMeshMode));
                return output;
            }

            float Hash21(float2 p)
            {
                float3 q = frac(float3(p.x, p.y, p.x) * 0.1031);
                q += dot(q, q.yzx + 33.33);
                return frac((q.x + q.y) * q.z);
            }

            float Noise(float2 p)
            {
                float2 cell = floor(p);
                float2 f = frac(p);
                float2 u = f * f * (3.0 - 2.0 * f);
                float a = Hash21(cell), b = Hash21(cell + float2(1, 0));
                float c = Hash21(cell + float2(0, 1)), d = Hash21(cell + float2(1, 1));
                return lerp(lerp(a, b, u.x), lerp(c, d, u.x), u.y);
            }

            float Fbm(float2 p)
            {
                float value = 0.0;
                float amplitude = 0.533333;
                [unroll] for (int octave = 0; octave < 4; octave++)
                {
                    value += Noise(p) * amplitude;
                    p = float2(p.x * .80 - p.y * .60, p.x * .60 + p.y * .80) * 2.03 + float2(17.1, 9.2);
                    amplitude *= .5;
                }
                return value;
            }

            float CloudBody(float field, float coverage, float density)
            {
                float threshold = lerp(.77, .26, saturate(coverage));
                float softWidth = lerp(.16, .10, saturate(density));
                return smoothstep(threshold - softWidth * .35, threshold + softWidth, field);
            }

            // Native texture mapped onto a small angular disc. The exact native
            // shader's undocumented size convention is not presumed to be radians.
            float2 DiscUV(float3 direction, float3 axis, float radius, out float mask)
            {
                float3 reference = abs(axis.y) > .97 ? float3(1, 0, 0) : float3(0, 1, 0);
                float3 right = normalize(cross(reference, axis));
                float3 up = cross(axis, right);
                float2 projected = float2(dot(direction, right), dot(direction, up)) / radius;
                float angularDistance = length(projected);
                float aa = max(fwidth(angularDistance), .02);
                mask = (1.0 - smoothstep(1.0 - aa, 1.0 + aa, angularDistance)) * step(0.0, dot(direction, axis));
                return projected * .5 + .5;
            }

            float3 Stars(float3 direction)
            {
                float2 uv = float2(atan2(direction.z, direction.x) * (1.0 / UNITY_TWO_PI) + .5,
                    asin(clamp(direction.y, -.99999, .99999)) * (1.0 / UNITY_PI) + .5);
                float2 grid = uv * float2(900.0, 450.0);
                float2 cell = floor(grid);
                float random = Hash21(cell + 41.7);
                // 'point' is a reserved HLSL geometry-shader primitive: the identifier was invalid.
                float2 cellPoint = float2(Hash21(cell + 13.4), Hash21(cell + 71.9)) * .6 + .2;
                float2 delta = frac(grid) - cellPoint;
                float star = exp(-dot(delta, delta) * 400.0) * step(.995, random);
                float fadeOut = 1.0 - smoothstep(_StarFadeOutStart, max(_StarFadeOutStart + .001, _StarFadeOutEnd), _WLDayFraction);
                float fadeIn = smoothstep(_StarFadeInStart, max(_StarFadeInStart + .001, _StarFadeInEnd), _WLDayFraction);
                float night = saturate(fadeOut + fadeIn) * (1.0 - saturate(_WLDaylight));
                return star * _StarShine.rgb * .035 * night * smoothstep(-.02, .10, direction.y);
            }

            half4 Frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                float3 direction = normalize(input.directionWS);
                float y = direction.y;
                float skyBlend = pow(saturate(y), .45);
                float3 sky = lerp(_WLSkyHorizon.rgb, _WLSkyTop.rgb, skyBlend);
                sky = lerp(sky, _WLSkyGround.rgb, 1.0 - smoothstep(-.40, 0.0, y));

                float3 sun = normalize(_WLSunDirection.xyz);
                float3 moon = normalize(_WLMoonDirection.xyz);
                float sunFacing = saturate(dot(direction, sun));
                float sunMask, moonMask;
                float2 sunUv = DiscUV(direction, sun, clamp(abs(_SunSizeModifier) * .030, .0015, .025), sunMask);
                float2 moonUv = DiscUV(direction, moon, clamp(abs(_MoonSizeModifier) * .033, .0015, .025), moonMask);
                float sunVisible = _WLSunVisible * smoothstep(-.035, .025, sun.y);
                float moonVisible = _WLMoonVisible * smoothstep(-.035, .025, moon.y) * (1.0 - _WLDaylight * .80);
                float4 sunMap = tex2D(_SunTexture, saturate(sunUv) * _SunTexture_ST.xy + _SunTexture_ST.zw);
                float4 moonMap = tex2D(_MoonTexture, saturate(moonUv) * _MoonTexture_ST.xy + _MoonTexture_ST.zw);
                sky += sunMap.rgb * sunMap.a * _SunShine.rgb * .30 * sunMask * sunVisible;
                sky += moonMap.rgb * moonMap.a * _MoonShine.rgb * .10 * moonMask * moonVisible;
                sky += min(_SunShine.rgb, 3.0) * pow(sunFacing, 550.0) * .075 * sunVisible;
                sky += Stars(direction);

                // Perspective across two distant layers makes cells compress
                // naturally toward the horizon. There is no camera-height or
                // scene-depth assumption and no per-pixel volume ray march.
                float2 projection = direction.xz / (max(y, .025) + .23);
                float2 p = projection * (1.85 * _WLCloudScale) + _WLCloudOffset.xy;
                float2 warp = float2(Noise(p * .47 + 4.1), Noise(p * .41 - 9.7)) - .5;
                float field = Fbm(p + warp * 1.1);
                float pixelFootprint = max(length(ddx(p)), length(ddy(p)));
                float detailWeight = saturate(1.0 - pixelFootprint * 8.0);
                float detail = Noise(p * 6.7 + warp * 2.2);
                field -= (1.0 - detail) * .048 * detailWeight;
                float body = CloudBody(field, _WLCloudCoverage, _WLCloudDensity);

                float2 upperP = projection * (_WLCloudScale * float2(.83, 1.38)) + _WLCloudOffset.xy * .63 + float2(63.7, 12.8);
                float upperField = Fbm(upperP);
                float upperBody = CloudBody(upperField, saturate(_WLCloudCoverage - .12), _WLCloudDensity * .6);
                float horizonFade = smoothstep(.005, .13, y);
                float opacity = saturate(_WLCloudOpacity);
                float coverageFade = smoothstep(0.0, .08, _WLCloudCoverage);
                float alpha = (1.0 - exp(-body * _WLCloudDensity * 3.1)) * opacity * horizonFade * coverageFade;
                float upperAlpha = (1.0 - exp(-upperBody * _WLCloudDensity * 1.6)) * opacity * .43 * horizonFade * coverageFade;

                // A short 2D light sample provides soft internal form. Thick
                // centers stay cool and darker, thin sun-facing rims stay light.
                float2 lightOffset = sun.xz * .38;
                float lightField = Fbm(p + warp * 1.1 + lightOffset);
                float lit = saturate(.48 + (field - lightField) * 3.4 + (1.0 - body) * .24);
                float edge = pow(1.0 - body, 2.0) * body * 4.0;
                float silver = pow(sunFacing, 18.0) * edge * .28 * sunVisible;
                float3 cloud = lerp(_WLCloudShadow.rgb, _WLCloudLight.rgb, lit);
                cloud += min(_SunShine.rgb, 2.0) * silver;
                float haze = pow(1.0 - saturate(y), 5.0) * saturate(_FogInfluence) * .45;
                cloud = lerp(cloud, _WLSkyHorizon.rgb, haze);
                float3 upperCloud = lerp(_WLCloudShadow.rgb, _WLCloudLight.rgb, .74);
                sky = lerp(sky, upperCloud, upperAlpha);
                sky = lerp(sky, cloud, alpha);
                return half4(max(sky, 0.0), 1.0);
            }
            ENDCG
        }
    }
    Fallback Off
}
