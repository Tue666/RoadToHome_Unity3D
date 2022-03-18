Shader "MagicalFX/SimpleGlass" {
	Properties {
		_MainTex ("Texture", 2D) = "black" {}
		_BumpMap ("Bumpmap", 2D) = "bump" {}
	 	_RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
     	_RimPower ("Rim Power", Range(0.5,8.0)) = 3.0
     	[PerRendererData]_Alpha ("Alpha", Range(0.0,1.0)) = 1.0
	}
	SubShader {

			Cull front
			Name "BASE"
			Tags { "Queue"="Transparent+99" "RenderType"="Transparent" }
			Blend One One 

 			Cull back

 			CGPROGRAM
      		#pragma surface surf Lambert
      		struct Input {
          		float3 viewDir;
          		float2 uv_BumpMap;
      		};

      		uniform sampler2D _BumpMap;
      		uniform sampler2D _MainTex;

      		float4 _RimColor;
      		float _RimPower;
      		float _Alpha;

      		void surf (Input IN, inout SurfaceOutput o) {
      			o.Albedo = tex2D (_MainTex, IN.uv_BumpMap);
      			o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_BumpMap));
          		half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
          		o.Emission = _RimColor.rgb * pow (rim, _RimPower) * _Alpha;
      		}

      		ENDCG
	}
	FallBack "Transparent/Diffuse"
}

