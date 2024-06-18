Shader "Custom/6.3. Dissolve" {
    Properties {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)

        _DissolveGlowColor ("Dissolve glow colour", Color) = (1,1,1,1)
        _DissolveGlowOffset ("Dissolve offset", Range(0,1)) = 0.0

        _PlayerPos ("_PlayerPos", Vector) = (0, 0, 0, 0)
        _MaxDistance ("Max Distance", Range(0, 10)) = 5
    }
    SubShader {
        Tags { "RenderType"="Opaque"}
        // Cull Off

        CGPROGRAM

        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard addshadow
        #pragma target 5.0
        #include "noiseSimplex.cginc"
        
        sampler2D _MainTex;

        struct Input {
            float3 worldPos;
            float2 uv_MainTex;
        };

        fixed4 _Color;
        float3 _PlayerPos;

        half _DissolveGlowOffset;
        fixed4 _DissolveGlowColor;
        float _MaxDistance;

        void surf (Input IN, inout SurfaceOutputStandard o) 
        {
            float noise = snoise(float3(IN.worldPos * 1.0f)) * 0.5 + 0.5;
            float dissolve = 0.0f;

            float distance = length(_PlayerPos - IN.worldPos);
            dissolve = (1 - distance / _MaxDistance) + 0.1;
            clip(noise - dissolve);

            o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb * _Color.rgb;
            if(noise - dissolve < _DissolveGlowOffset && noise > _DissolveGlowOffset)
            {
                o.Emission = _DissolveGlowColor.rgb;
                o.Albedo = _DissolveGlowColor.rgb;
            }
        }

        ENDCG
    }
    FallBack "Diffuse"
}

