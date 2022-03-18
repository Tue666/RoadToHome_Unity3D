﻿Shader "MagicalFX/Distortion" {
Properties {
	_MainTex ("Texture", 2D) = "white" {}
	_DispMap ("Displacement Map (RG)", 2D) = "white" {}
	_DispScrollSpeedX  ("Map Scroll Speed X", Float) = 0
	_DispScrollSpeedY  ("Map Scroll Speed Y", Float) = 0
	_StrengthX  ("Displacement Strength X", Float) = 1
	_StrengthY  ("Displacement Strength Y", Float) = -1
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

			uniform float4 _DispMap_ST;
			uniform sampler2D _DispMap;
			uniform sampler2D _MainTex;
			uniform half _DispScrollSpeedY;
			uniform half _DispScrollSpeedX;
			uniform half _StrengthX;
			uniform half _StrengthY;

			struct appdata_t {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord: TEXCOORD0;
				float2 param : TEXCOORD1;
				float3 normal : NORMAL;
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

			sampler2D _GrabTexture;

			half4 frag( v2f i ) : COLOR
			{
				half2 mapoft = half2(_Time.y*_DispScrollSpeedX, _Time.y*_DispScrollSpeedY);
				half4 offsetColor = tex2D(_DispMap, i.uvmain + mapoft);
				half4 mainColor = tex2D(_MainTex, i.uvmain);
				  
				half oftX =  offsetColor.r * _StrengthX * i.param.x;
				half oftY =  offsetColor.g * _StrengthY * i.param.x;

				i.uvgrab.x += oftX;
				i.uvgrab.y += oftY;

				half4 col = tex2Dproj( _GrabTexture, UNITY_PROJ_COORD(i.uvgrab));
				col *= mainColor;
				col.a = i.color.a;
				return col;
			}

			ENDCG
			}

		}
	}
}
