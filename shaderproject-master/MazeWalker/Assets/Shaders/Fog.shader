Shader "Custom/Fog"
{
    Properties
    {
        _FogColor ("Fog Color", Color) = (1, 1, 1, 1)
        _FogStrength ("Fog Strength", Range(0, 1)) = 0.1
        _NoiseScale ("Noise Scale", Float) = 1
    }
    SubShader
    {
        Tags {
            "RenderType"="Transparent"
            "Queue" = "Transparent+2"
        }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            #include "noiseSimplex.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 worldPos : TEXCOORD1;
                float distance : TEXCOORD2;
                float2 screenPos : TEXCOORD3;
                float depth : TEXCOORD4;
            };

            float _FogStrength;
            float4 _FogColor;
            float _NoiseScale;

            sampler2D _CameraDepthNormalsTexture;
            sampler2D _CameraDepthTexture;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.distance = -UnityObjectToViewPos(v.vertex).z;

                //Screen pos and depth calculations
                o.screenPos = ((o.vertex.xy / o.vertex.w) + 1) / 2; //Clip pos to screen pos conversion
                //Invert the screen pos (dx/opengl)
                o.screenPos.y = 1 - o.screenPos.y;
                //Calculate the vertex depth in view space (!= clip space)
                o.depth = -mul(UNITY_MATRIX_MV, v.vertex).z * _ProjectionParams.w; 
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float noise = snoise(i.worldPos * _NoiseScale) * 0.5 + 0.5;
                float3 c = _FogColor;

                //Calculate intersection values
                float screenDepth = DecodeFloatRG (tex2D(_CameraDepthNormalsTexture, i.screenPos).zw); //Sample the depth texture (Separate from normal texture)
                float diff = screenDepth - i.depth; //Depth difference
                float intersect = smoothstep(0, _ProjectionParams.w, diff); //Scale distance based on far plane

                noise = -cos(noise * 3.1415) * 0.5 + 0.5;
                return float4(c.r, c.g, c.b, _FogStrength * noise * intersect);
            }
            ENDCG
        }
    }
}
