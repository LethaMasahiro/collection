Shader "Custom/Wall"
{
    Properties
    {
        // _PortalDepthTex ("_PortalDepthTex", 2D) = "white" {}
        _MainTex ("Main Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags {
            "RenderType" = "Opaque"
            "Queue" = "Geometry"
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            
            sampler2D _PortalDepthTex;
            sampler2D _MainTex;
            int _UseDepth;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.vertex.xy / _ScreenParams.xy;

                float4 tex = tex2D(_PortalDepthTex, uv);

                if (_UseDepth != 0 && i.vertex.z > tex.z) {
                    discard;
                }
                
                float angle = max(0.1, dot(normalize(_WorldSpaceLightPos0), i.normal));

                float4 main = tex2D(_MainTex, i.uv);
                float4 col = angle * main;

                // UNITY_APPLY_FOG(i.fogCoord, col);

                float d = i.vertex.z;

                // return lerp(col, float4(1, 1, 1, 1), d);

                return col;
            }
            ENDCG
        }
    }
}
