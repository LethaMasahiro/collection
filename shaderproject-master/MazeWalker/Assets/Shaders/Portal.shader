Shader "Custom/Portal"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
	{
		Tags { "RenderType" = "Opaque" }

		Pass
		{
		HLSLPROGRAM

		#pragma vertex vert
		#pragma fragment frag

		#include "UnityCG.cginc"

		struct appdata
		{

		};

		struct v2f
		{

		};

		v2f vert(appdata v)
		{
			v2f o;
			return o;
		}
	}
}
