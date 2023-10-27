Shader "Game/HayStack" {
	Properties {
		_HaysTex ("_HaysTex)", 3D) = "white" {}
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_FullColor ("Full Color", Vector) = (0,1,0,0)
		_OutlineColor ("OutlineColor", Vector) = (0,0,0,1)
		_OutlineWidth ("OutlineWidth", Range(0, 0.5)) = 0.1
		_HayScale ("HayScale", Vector) = (1,1,1,0)
		_Size ("Size", Vector) = (1,1,1,0)
		_Glossiness ("Smoothness", Range(0, 1)) = 0.5
		_Metallic ("Metallic", Range(0, 1)) = 0
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Diffuse"
}