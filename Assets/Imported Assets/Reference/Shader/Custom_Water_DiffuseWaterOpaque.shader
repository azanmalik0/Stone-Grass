Shader "Custom/Water/DiffuseWaterOpaque" {
	Properties {
		_WaterColor ("Water color", Vector) = (1,1,1,1)
		_WaterTex ("Water texture", 2D) = "white" {}
		_Tiling ("Water tiling", Vector) = (1,1,1,1)
		_TextureVisibility ("Texture visibility", Range(0, 1)) = 1
		[Space(20)] _DistTex ("Distortion", 2D) = "white" {}
		_DistTiling ("Distortion tiling", Vector) = (1,1,1,1)
		[Space(20)] _WaterHeight ("Water height", Float) = 0
		[Space(20)] _MoveDirection ("Direction", Vector) = (0,0,0,0)
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = 1;
		}
		ENDCG
	}
}