Shader "Custom/PlayerColousBIR" {
    Properties {
        _Color("Color", Color) = (1,1,1,1)
        _PlayerColor("Player Color", Color) = (1,0,0,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
        sampler2D _BumpMap;
        sampler2D _MaskMap;

        struct Input {
            float2 uv_MainTex;
            float2 uv_BumpMap;
            float2 uv_MaskMap;
        };

        fixed4 _Color;
        fixed4 _PlayerColor;

        void surf (Input IN, inout SurfaceOutputStandard o) {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;

            c.rgb = LinearToGammaSpace(c.rgb);
            c.rgb += (LinearToGammaSpace(_PlayerColor.rgb) - 0.5) *
                (1 - c.a);
            c.rgb = GammaToLinearSpace(c.rgb);

            o.Albedo = c.rgb;
            o.Metallic = 0;
            o.Smoothness = 0;
            o.Alpha = 1.0;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
