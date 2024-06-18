// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'



Shader "Custom/Snow" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _NormalMap("Normal", 2D) = "normal" {}
        _FootprintTexture("_FootprintTexture", 2D) = "white" {}

        _SnowHeight("Snow Height", Range(0, 10)) = 1
        _SnowLevel("Snow level", Range(0, 1)) = 1
        _SnowColor("Snow color", Color) = (1.0,1.0,1.0,1.0)
        _SnowDirection("Snow direction", Vector) = (0,1,0)

        _SnowChangeWidth("Snow Change Width", Range(0, 1)) = 0.5
        _SnowChangeHeight("Snow Change Height", Range(0, 1)) = 0.5
    }
    
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 300

        // Pass {
            CGPROGRAM
// // Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct appdata members worldPosition)
// #pragma exclude_renderers d3d11

            // #pragma vertex vert
            // #pragma fragment frag
            // tessellate:tessFixed
            #pragma surface surf BlinnPhong addshadow fullforwardshadows vertex:disp nolightmap
            #pragma target 4.6

            // #include "UnityCG.cginc"
            
            fixed4 _Color;
            sampler2D _MainTex;
            float3 playerPos;

            half _Glossiness;
            half _Metallic;

            sampler2D _NormalMap;
            float _SnowLevel;
            float _SnowHeight;
            float4 _SnowColor;
            float4 _SnowDirection;

            sampler2D _FootprintTexture;

            float _Tess;

            float _SnowChangeHeight;
            float _SnowChangeWidth;


            sampler2D _DispTex;
            float _Displacement;

            struct Input {
                float2 uv_MainTex;
                float2 uv_NormalMap;
                float3 worldPos;
                float3 test;
                float4 vertex;

                INTERNAL_DATA 
            };

            struct appdata 
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord : TEXCOORD0;
            };

            float4 tessFixed()
            {
                return _Tess;
            }

            void disp (inout appdata v, out Input o)
            {
                UNITY_INITIALIZE_OUTPUT(Input, o);
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex);

                float4 footprints = tex2Dlod(_FootprintTexture, float4(worldPos.xz * 0.01, 0, 0));
                v.vertex += float4(v.normal * _SnowHeight * footprints.x, 0);
                // v.vertex += float4(v.normal * _SnowHeight, 0);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.vertex /= o.vertex.w;
            }


            void surf(Input i, inout SurfaceOutput o) {
                float4 footprints = tex2Dlod(_FootprintTexture, float4(i.worldPos.xz * 0.01, 0, 0));
                o.Normal = UnpackNormal(tex2D( _NormalMap, i.uv_NormalMap));

                float4 tex = tex2D(_MainTex, i.uv_MainTex);

                float width = _SnowChangeWidth;
                float h = _SnowChangeHeight;

                if (footprints.x > h + width) {
                    o.Albedo = 1;
                }
                else if (footprints.x > h) {
                    o.Albedo = lerp(tex, float4(1, 1, 1, 1), (footprints.x - h) / width);
                } else {
                    o.Albedo = tex;
                }

                
            }

            ENDCG
        // }
    }
    FallBack "Diffuse"
}
