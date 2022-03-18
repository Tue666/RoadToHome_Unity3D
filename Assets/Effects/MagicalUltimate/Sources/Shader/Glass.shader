Shader "MagicalFX/Glass" {
Properties {
	_MainTex ("Texture", 2D) = "black" {}
	_BumpMap ("Bumpmap", 2D) = "bump" {}
	_DispMap ("Displacement Map (RG)", 2D) = "white" {}
	 _MainColor ("Main Color", Color) = (0.26,0.19,0.16,0.0)
	_DispScrollSpeedX  ("Map Scroll Speed X", Float) = 0
	_DispScrollSpeedY  ("Map Scroll Speed Y", Float) = 0
	_StrengthX  ("Displacement Strength X", Float) = 1
	_StrengthY  ("Displacement Strength Y", Float) = -1
	 _RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)
     _RimPower ("Rim Power", Range(0.5,8.0)) = 3.0
     [PerRendererData]_Alpha ("Alpha", Range(0.0,1.0)) = 1.0
}


Category {
	Tags { "Queue"="Transparent+99" "RenderType"="Transparent" }
	Blend SrcAlpha OneMinusSrcAlpha
	Cull Off Lighting Off ZWrite Off ZTest Always
	
	BindChannels {
		Bind "Color", color
		Bind "Vertex", vertex
		Bind "TexCoord", texcoord
	}

	SubShader {


      	GrabPass {						
			Name "BASE"
			Tags { "LightMode" = "Always" }
 		}

		Pass {
			
			Name "BASE"
			Tags { "LightMode" = "Always" }
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"


			uniform half _StrengthX;
			uniform half _StrengthY;
			uniform float4 _DispMap_ST;
			uniform sampler2D _DispMap;
			uniform half _DispScrollSpeedY;
			uniform half _DispScrollSpeedX;


			struct appdata_t {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord: TEXCOORD0;
				float2 param : TEXCOORD1;
			};

			struct v2f {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 uvmain : TEXCOORD0;
				float2 param : TEXCOORD1;
				float4 uvgrab : TEXCOORD2;
			};

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);

				#if UNITY_UV_STARTS_AT_TOP
					float scale = -1.0;
				#else
					float scale = 1.0;
				#endif

				o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y*scale) + o.vertex.w) * 0.5;
				o.uvgrab.zw = o.vertex.zw;
				o.uvmain = TRANSFORM_TEX( v.texcoord, _DispMap );
				o.color = v.color;
				o.param = v.param;
				return o;
			}
			sampler2D _BumpMap;
			sampler2D _GrabTexture;
			float4 _MainColor;
			float _Alpha;

			half4 frag( v2f i ) : COLOR
			{
				half2 mapoft = half2(_Time.y*_DispScrollSpeedX, _Time.y*_DispScrollSpeedY);
				half4 bump = tex2D(_BumpMap, i.uvmain);
				half4 offsetColor = tex2D(_DispMap, i.uvmain + mapoft);

				half oftX =  offsetColor.r * _StrengthX * i.param.x;
				half oftY =  offsetColor.g * _StrengthY * i.param.x;

				i.uvgrab.x += oftX;
				i.uvgrab.y += oftY;

				half4 col = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
				col *= _MainColor;
				col.a = i.color.a * _Alpha;
				return col;
			}

			ENDCG
			}

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
	}
}
