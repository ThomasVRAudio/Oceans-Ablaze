Shader "Custom/LandShader"
{
    Properties
    {
        [Header(Colors)]
        _WaterColor("Water Color", Color) = (1,1,1,1)
        _SandColor("Sand Color", Color) = (1,1,1,1)
        _GrassColor("Grass Color", Color) = (1,1,1,1)
        _DarkGrassColor("Dark Grass Color", Color) = (1,1,1,1)
        _MountainColor("Mountain Color", Color) = (1,1,1,1)
        _DarkMountainColor("Dark Mountain Color", Color) = (1,1,1,1)
        _PeakColor("Peak Color", Color) = (1,1,1,1)

        _MaxHeight("Max Height", float) = 2.0

        [Header(Levels)]
        _Level0("Level0", float) = 0.0
        _Level1("Level1", float) = 0.0
        _Level2("Level2", float) = 0.0
        _Level3("Level3", float) = 0.0
        _Level4("Level4", float) = 0.0
        _Level5("Level5", float) = 0.0
           
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM
        #pragma surface surf Standard

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        float4 _WaterColor;
        float4 _SandColor;
        float4 _GrassColor;
        float4 _DarkGrassColor;
        float4 _MountainColor;
        float4 _DarkMountainColor;
        float4 _PeakColor;
        float _MaxHeight;

        float _Level0;
        float _Level1;
        float _Level2;
        float _Level3;
        float _Level4;
        float _Level5;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float4 color;

            if (IN.worldPos.y > _MaxHeight * _Level5)
                color = _PeakColor;
            else if (IN.worldPos.y > _MaxHeight * _Level4)
                color = _DarkMountainColor;
            else if (IN.worldPos.y > _MaxHeight * _Level3)
                color = _MountainColor;
            else if (IN.worldPos.y > _MaxHeight * _Level2)
                color = _DarkGrassColor;
            else if (IN.worldPos.y > _MaxHeight * _Level1)
                color = _GrassColor;
            else if (IN.worldPos.y > _MaxHeight * _Level0)
                color = _SandColor;
            else
                color = _WaterColor;

            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
