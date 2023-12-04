Shader "Custom/GrassBendShader"
{
    Properties
    {
        _BendAmount("Bend Amount", Range(0, 1)) = 0.5
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        CGPROGRAM
        #pragma surface surf Lambert

        struct Input
        {
            float2 uv_MainTex;
        };

        float _BendAmount;

        void surf(Input IN, inout SurfaceOutput o)
        {
            // Calculate bending based on UV coordinates
            float bendFactor = tex2D(_MainTex, IN.uv_MainTex).r * _BendAmount;

            // Apply vertex displacement
            float3 displacedPos = UnityObjectToClipPos(IN.worldPos + float4(0, bendFactor, 0, 0));

            // Output final position
            o.Alpha = o.Alpha * tex2D(_MainTex, IN.uv_MainTex).a;
            o.Normal = UnityObjectToWorldNormal(o.Normal);
            o.Emission = o.Emission;
            o.Specular = o.Specular;
            o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;

            // Set output vertex position
            o.Pos = displacedPos.xyz;
        }
        ENDCG
    }

    FallBack "Diffuse"
}
