Shader "Game/SurfaceOutline" {
	Properties {
		_Color ("Color", Vector) = (1,1,1,1)
		_Saturation ("Saturation", Range(0, 5)) = 1
		_Glossiness ("Smoothness", Range(0, 1)) = 0.5
		_Metallic ("Metallic", Range(0, 1)) = 0
		_Emission ("Emission", Range(0, 1)) = 0
		_EmissionPow ("EmissionPow", Range(0, 10)) = 3
		_OutlineColor ("Outline Color", Vector) = (1,1,1,1)
		_Outline ("Thick of Outline", Range(0, 0.1)) = 0.02
		_Factor ("Factor", Range(0, 1)) = 0.5
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = _Color.rgb;
			o.Alpha = _Color.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
}