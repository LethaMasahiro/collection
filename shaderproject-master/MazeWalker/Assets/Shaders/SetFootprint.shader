Shader "Custom/SetFootprint"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FootprintSize ("Footprint Size", Range(0, 1)) = 1
        _FootprintDecay ("Footprint Decay", Range(0, 5)) = 2
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float2 _PlayerPos;
            float _FootprintSize;
            float _FootprintDecay;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                float dist = length(i.worldPos - _PlayerPos);

                if (dist < _FootprintSize * 0.01) {
                    float f = (100 * dist / _FootprintSize );
                    return pow(f, _FootprintDecay) * col;
                }

                return col;
            }
            ENDCG
        }
    }
}
