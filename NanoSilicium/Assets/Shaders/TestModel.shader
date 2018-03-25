﻿Shader "Unlit/TestModel"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white"{}
		_speed("Speed", Float) = 1.0
		_turn("turnDirection", Float) = 0.0
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _position;
			float _turn;
			float _speed;
			
			v2f vert (appdata v)
			{
				v2f o;
				float4 pos = _position - v.vertex;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				o.vertex.x += sin(_SinTime * 10 + o.vertex.y * 2) / 5;
				o.vertex.y += sin(_SinTime * 10 + o.vertex.x * 2) / 5;
				if (_turn > 0)
					o.vertex.x += abs(pos.y);
				else if (_turn < 0)
					o.vertex.x -= abs(pos.y);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}