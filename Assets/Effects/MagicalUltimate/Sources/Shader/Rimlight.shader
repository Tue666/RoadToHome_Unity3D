 Shader "MagicalFX/Rim" {
    Properties {
      _MainTex ("Texture", 2D) = "white" {}
      _MainColor ("Main Color", Color) = (1.0,1.0,1.0,1.0)
      _CutOffTex ("CutOff map", 2D) = "white" {}
      _BumpMap ("Bumpmap", 2D) = "bump" {}
      _RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
      _RimPower ("Rim Power", Range(0.5,8.0)) = 3.0
      [PerRendererData]_Cutoff ("CutOff", Range(0,1)) = 0.5

    }
    SubShader {
      Tags {"Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"}
      CGPROGRAM
      #pragma surface surf Lambert alphatest:_Cutoff


      struct Input {
          float2 uv_MainTex;
          float2 uv_BumpMap;
          float3 viewDir;
      };

      sampler2D _CutOffTex;
      sampler2D _MainTex;
      sampler2D _BumpMap;
      float4 _RimColor;
      float4 _MainColor;
      float _RimPower;

      void surf (Input IN, inout SurfaceOutput o) {
      	  float4 cut = tex2D (_CutOffTex, IN.uv_MainTex);
          o.Albedo = tex2D (_MainTex, IN.uv_MainTex) * _MainColor;
          o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
          half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
          o.Emission = _RimColor.rgb * pow (rim, _RimPower);
          o.Alpha = cut.r;
      }

      ENDCG
    } 
    FallBack "Transparent/Cutout/Diffuse"
}