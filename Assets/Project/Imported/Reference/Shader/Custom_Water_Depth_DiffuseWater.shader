Shader "Custom/Water/Depth/DiffuseWater" {
	Properties {
		_Color ("Color", Vector) = (1,1,1,1)
		_MainTex ("Texture", 2D) = "white" {}
		[Space(20)] _WaterColor ("Water color", Vector) = (1,1,1,1)
		_WaterTex ("Water texture", 2D) = "white" {}
		_Tiling ("Water tiling", Vector) = (1,1,1,1)
		_TextureVisibility ("Texture visibility", Range(0, 1)) = 1
		[Space(20)] _DistTex ("Distortion", 2D) = "white" {}
		_DistTiling ("Distortion tiling", Vector) = (1,1,1,1)
		[Space(20)] _WaterHeight ("Water height", Float) = 0
		_WaterDeep ("Water deep", Float) = 0
		_WaterDepth ("Water depth param", Range(0, 0.1)) = 0
		_WaterMinAlpha ("Water min alpha", Range(0, 1)) = 0
		[Space(20)] _BorderColor ("Border color", Vector) = (1,1,1,1)
		_BorderWidth ("Border width", Range(0, 1)) = 0
		[Space(20)] _MoveDirection ("Direction", Vector) = (0,0,0,0)
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType"="Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		sampler2D _MainTex;
		fixed4 _Color;
		struct Input
		{
			float2 uv_MainTex;
		};
		
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
}