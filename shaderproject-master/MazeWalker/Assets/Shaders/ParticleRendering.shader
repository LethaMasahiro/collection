Shader "Custom/ParticleRendering"
{
    Properties
    {
        _SlowColor ("Slow Color", Color) = (1, 0, 0)
        _FastColor ("Fast Color", Color) = (0, 1, 0)
        _SpeedScale ("Speed Scale", Float) = 50

        _Size ("Size", Float) = 1
    }

    SubShader
    {
        Tags {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
        }


        Pass 
        {
            ZWrite Off
            Blend One One
            // Cull Off

            HLSLPROGRAM
            
            #pragma vertex vert
            #pragma fragment frag
            #pragma geometry geom
            
            #include "UnityCG.cginc"
            
            //The same particle data we're using in the compute shader
            struct Particle
            {
                float3 pos;
                float3 vel;
                float3 targetVel;
                float3 targetOffset;
            };
            
            //Buffer containing all the particles
            StructuredBuffer<Particle> particleBuffer;
            
            struct v2g
            {
                float4 vertex : POSITION;
                float4 color : TEXCOORD0;
                float3 targetVel : TEXCOORD1;
            };
            
            struct g2f
            {
                float4 vertex : SV_POSITION;
                float4 color : TEXCOORD0;
            };
            
            //ToDo: Properties
            float4 _SlowColor;
            float4 _FastColor;
            float _SpeedScale;
            float _Size;
            
            v2g vert(uint instance_id : SV_InstanceID)
            {
                v2g o;
                o.vertex = float4(particleBuffer[instance_id].pos, 1);
                o.color = lerp(_SlowColor, _FastColor, length(particleBuffer[instance_id].vel) / _SpeedScale);
                o.targetVel = particleBuffer[instance_id].targetVel;
                return o;
            }

#define RENDER_TARGET_VEL 1
#if RENDER_TARGET_VEL
            [maxvertexcount(2)]
            void geom(point v2g IN[1], inout LineStream<g2f> stream)
            {
                v2g i = IN[0];

                g2f o;
                o.color = i.color;

                o.vertex = UnityWorldToClipPos(i.vertex);
                stream.Append(o);

                o.color.a = 0;
                o.vertex = UnityWorldToClipPos(i.vertex + i.targetVel * _Size);
                stream.Append(o);
            }

#else

            [maxvertexcount(4)]
            void geom(point v2g IN[1], inout TriangleStream<g2f> stream)
            {
                v2g i = IN[0];

                float3 dir = normalize(_WorldSpaceCameraPos - i.vertex.xyz);
                float3 right = normalize(cross(float3(0, 1, 0), dir)) * _Size;
                float3 up = normalize(cross(dir, right)) * _Size;


                g2f o;
                o.color = i.color;

                o.vertex = UnityWorldToClipPos(i.vertex - right - up);
                stream.Append(o);

                o.vertex = UnityWorldToClipPos(i.vertex + right - up);
                stream.Append(o);

                o.vertex = UnityWorldToClipPos(i.vertex - right + up);
                stream.Append(o);

                o.vertex = UnityWorldToClipPos(i.vertex + right + up);
                stream.Append(o);
            }

#endif

            float4 frag(g2f i) : COLOR
            {
                // return float4(i.color.xyz, 1);
                return float4(i.color.xyz, 1) * i.color.a;
            }
            
            ENDHLSL
        }
    }
}
