Shader "Custom/SnowFalling"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("MainTex", 2D) = "white" {}
        
        _Size("Size", Float) = 1
    }
    SubShader
    {
        Tags 
        { 
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }

        Pass 
        {
            //ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma geometry geom
            
            #include "UnityCG.cginc"

            struct SnowParticle
            {
                float3 pos;
                float3 vel;
            };
            //Buffer containing all the particles
            StructuredBuffer<SnowParticle> particleBuffer;

            struct v2g
            {
                float4 vertex : POSITION;
                float4 color : TEXCOORD1;
            };

            struct g2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : TEXCOORD1;
            };

            float4 _Color;
            sampler2D _MainTex;
            float _Size;

            float3 playerPos;

            v2g vert(uint instance_id : SV_InstanceID)
            {
                v2g o;
                o.vertex = float4(particleBuffer[instance_id].pos, 1);
                o.color = _Color;
                return o;
            }

            [maxvertexcount(4)]
            void geom(point v2g IN[1], inout TriangleStream<g2f> stream)
            {
                v2g i = IN[0];

                float3 dir = normalize(_WorldSpaceCameraPos - i.vertex.xyz);
                float3 right = normalize(cross(float3(0, 1, 0), dir)) * _Size * 0.25;
                float3 up = normalize(cross(dir, right)) * _Size * 0.25;

                g2f o;
                o.color = i.color;

                //get uv coordinates for every edge (4 times)

                o.pos = UnityWorldToClipPos(i.vertex - right - up);
                o.uv = float2(0,0);
                stream.Append(o);

                o.pos = UnityWorldToClipPos(i.vertex + right - up);
                o.uv = float2(0,1);
                stream.Append(o);

                o.pos = UnityWorldToClipPos(i.vertex - right + up);
                o.uv = float2(1,0);
                stream.Append(o);

                o.pos = UnityWorldToClipPos(i.vertex + right + up);
                o.uv = float2(1,1);
                stream.Append(o);
            }

            float4 frag(g2f i) : COLOR
            {
                // return float4(i.color.xyz, 1);
                fixed4 col = tex2D(_MainTex, i.uv);
                if (col.a < 0.1) {
                    discard;
                }
                return col;
            }

            ENDHLSL
        
        }
    }
    FallBack "Diffuse"
}
