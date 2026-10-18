// Minimal isolation test shader: plain unlit color, no URP includes, no
// multi_compile. If even this comes out unsupported in the AssetBundle, the
// problem is project/build settings, not the Water shader's feature set.
Shader "S1Mods/TestUnlit"
{
    Properties
    {
        _Color ("Color", Color) = (1, 0.5, 0.1, 1)
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }
        Pass
        {
            Name "ForwardUnlit"
            Tags { "LightMode" = "UniversalForward" }
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)
            float4 _Color;
            CBUFFER_END

            struct Attributes { float4 positionOS : POSITION; };
            struct Varyings { float4 positionCS : SV_POSITION; };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                return OUT;
            }

            half4 frag() : SV_Target { return half4(_Color.rgb, 1); }
            ENDHLSL
        }
    }
}
