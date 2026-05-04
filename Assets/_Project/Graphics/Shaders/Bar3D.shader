Shader "Custom/Bar3D"
{
    Properties
    {
        _Fill("Fill Amount", Range(0,1)) = 1
        _FillColor("Fill Color", Color) = (0,1,0,1)
        _EmptyColor("Empty Color", Color) = (1,0,0,1)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }

        // === Pass для URP ===
        Pass
        {
            Name "URP_Unlit"
            Tags { "LightMode"="SRPDefaultUnlit" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            float _Fill;
            float4 _FillColor;
            float4 _EmptyColor;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(IN);
                float filled = step(IN.uv.x, _Fill);
                return lerp(_EmptyColor, _FillColor, filled);
            }
            ENDHLSL
        }

        // === Fallback Pass для Built-in pipeline (и Android GLES2) ===
        Pass
        {
            Name "Builtin_Unlit"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float _Fill;
            float4 _FillColor;
            float4 _EmptyColor;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float filled = step(i.uv.x, _Fill);
                return lerp(_EmptyColor, _FillColor, filled);
            }
            ENDCG
        }
    }

    // Fallback на стандартный Unlit (если всё остальное не сработает)
    Fallback "Unlit/Color"
}
