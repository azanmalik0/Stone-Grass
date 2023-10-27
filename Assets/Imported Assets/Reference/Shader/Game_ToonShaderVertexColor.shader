Shader "Game/ToonShaderVertexColor" {
	Properties {
		_Color ("Main Color", Vector) = (1,1,1,1)
		_Saturation ("Saturation", Range(0, 5)) = 1
		[Toggle] _HasSaturation ("HasSaturation", Float) = 0
		_OutlineColor ("Outline Color", Vector) = (1,1,1,1)
		_Outline ("Thick of Outline", Range(0, 0.1)) = 0.02
		_Factor ("Factor", Range(0, 1)) = 0.5
		_ToonEffect ("Toon Effect", Range(0, 1)) = 0.5
		_Steps ("Steps of toon", Range(0, 9)) = 3
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
}