// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "MagicalFX/AnimationUV" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_MainColor ("Main Color", Color) = (1,1,1,1)
		_BumpMap ("Bumpmap", 2D) = "bump" {}
	 	_RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
     	_RimPower ("Rim Power", Range(0.5,8.0)) = 3.0
     	_Alpha ("Alpha", Range(0.0,1.0)) = 1.0
		_DispScrollSpeedX  ("Speed X", Float) = 0
		_DispScrollSpeedY  ("Speed Y", Float) = 0
		_StrengthX  ("Direction X", Float) = 1
		_StrengthY  ("Direction Y", Float) = -1
	}
	SubShader {

			Name "BASE"
			Tags { "Queue"="Transparent" "RenderType"="Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha
 			Cull back Lighting On ZWrite off

 			CGPROGRAM
      		#pragma surface surf Lambert alpha
      		struct Input {
          		float3 viewDir;
          		float2 uv_MainTex;
      		};

      		uniform half _DispScrollSpeedY;
			uniform half _DispScrollSpeedX;
			uniform half _StrengthX;
			uniform half _StrengthY;
      		uniform sampler2D _BumpMap;
      		uniform sampler2D _MainTex;
      		float4 _MainColor;
      		float4 _RimColor;
      		float _RimPower;
      		float _Alpha;
      		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
			UNITY_INSTANCING_BUFFER_END(Props)

      		void surf (Input IN, inout SurfaceOutput o) {
      			half2 mapoft = half2(_Time.y*_DispScrollSpeedX, _Time.y*_DispScrollSpeedY);
      			half4 col = tex2D (_MainTex, IN.uv_MainTex + mapoft);
      			o.Albedo = col * _MainColor;
      			o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_MainTex + mapoft));
          		half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));
          		o.Emission = _RimColor.rgb * pow (rim, _RimPower);
          		o.Alpha = col.a * _Alpha;
      		}

      		ENDCG
	}
	FallBack "Transparent/Diffuse"
}
