// Upgrade NOTE: replaced 'PositionFog()' with transforming position into clip space.
// Upgrade NOTE: replaced 'V2F_POS_FOG' with 'float4 pos : SV_POSITION'
// Upgrade NOTE: replaced 'glstate.matrix.mvp' with 'UNITY_MATRIX_MVP'
// Upgrade NOTE: replaced 'glstate.matrix.texture[0]' with 'UNITY_MATRIX_TEXTURE0'
// Upgrade NOTE: replaced 'samplerRECT' with 'sampler2D'
// Upgrade NOTE: replaced 'texRECTproj' with 'tex2Dproj'

// Uses geometry normals to distort the image behind, and
// an additional texture to tint the color.

Shader "FX/Refraction Distort" {
	Properties{
		_BumpAmt("Distortion", range(0,128)) = 10.0
		_BumpMap("Normal Map", 2D) = "bump" {}
		_MainTex("Tint Color (RGB)", 2D) = "white" {}
	}

		Category{

		// We must be transparent, so other objects are drawn before this one.
		Tags{ "Queue" = "Transparent" }

		// ------------------------------------------------------------------
		//  ARB fragment program

		SubShader{

		// This pass grabs the screen behind the object into a texture.
		// We can access the result in the next pass as _GrabTexture
		GrabPass{
		Name "BASE"

		Tags{ "LightMode" = "Always" }

	}

		// Main pass: Take the texture grabbed above and use the normals to perturb it
		// on to the screen
		Pass{
		Name "BASE"
		Tags{ "LightMode" = "Always" }
		Cull Off

		CGPROGRAM
		// profiles arbfp1
		// vertex vert
		// fragment frag
		// fragmentoption ARB_precision_hint_fastest 
		// fragmentoption ARB_fog_exp2

#include "UnityCG.cginc"

		sampler2D _MainTex : register(s1);
	sampler2D _GrabTexture : register(s0);

	struct v2f {
		float4 pos : SV_POSITION;
		float4 uvrefr    : TEXCOORD0;
		float2 uv         : TEXCOORD1;
		float3 normal    : TEXCOORD2;
		half3 tspace0 : TEXCOORD3; // tangent.x, bitangent.x, normal.x
		half3 tspace1 : TEXCOORD4; // tangent.y, bitangent.y, normal.y
		half3 tspace2 : TEXCOORD5; // tangent.z, bitangent.z, normal.z
	};

	uniform float _BumpAmt;

	v2f vert(appdata_base v)
	{
		v2f o;
		o.pos = UnityObjectToClipPos (v.vertex);
		o.uv = TRANSFORM_UV(1);
		o.uvrefr = mul(UNITY_MATRIX_TEXTURE0, v.vertex);

		half3 wNormal = UnityObjectToWorldNormal(input.normal);
		half3 wTangent = UnityObjectToWorldDir(input.tangent.xyz);
		// compute bitangent from cross product of normal and tangent
		half tangentSign = input.tangent.w * unity_WorldTransformParams.w;
		half3 wBitangent = cross(wNormal, wTangent) * tangentSign;
		// output the tangent space matrix
		o.tspace0 = half3(wTangent.x, wBitangent.x, wNormal.x);
		o.tspace1 = half3(wTangent.y, wBitangent.y, wNormal.y);
		o.tspace2 = half3(wTangent.z, wBitangent.z, wNormal.z);
		o.normal = mul((float3x3)UNITY_MATRIX_MVP, v.normal);

		return o;
	}

	half4 frag(v2f i) : COLOR
	{
		i.normal = normalize(i.normal);
		half2 uv_BumpMap = TRANSFORM_TEX(i.uv, _BumpMap);

		half3 tnormal = UnpackNormal(tex2D(_BumpMap, uv_BumpMap));
		// transform normal from tangent to world space
		half3 worldNormal;
		worldNormal.x = dot(i.tspace0, tnormal);
		worldNormal.y = dot(i.tspace1, tnormal);
		worldNormal.z = dot(i.tspace2, tnormal);

		half4 col = tex2D(_MainTex, i.uv.xy);

		half diffuse = saturate(dot(col.xyz, worldNormal) * 1.25);
		// Calculate refracted vector based on the surface normal.
		// This is only an approximation because we don't know the
		// thickness of the object. So just use anything that looks
		// "good enough"

		half3 refracted = i.normal * abs(i.normal);
		//half3 refracted = refract( i.normal, half3(0,0,1), 1.333 );

		// perturb coordinates of the grabbed image
		i.uvrefr.xy = refracted.xy * (i.uvrefr.w * _BumpAmt) + i.uvrefr.xy;

		half4 refr = tex2Dproj(_GrabTexture, i.uvrefr);
		return col * refr * diffuse;
	}

		ENDCG
		// Set up the textures for this pass
		SetTexture[_GrabTexture]{}    // Texture we grabbed in the pass above
		SetTexture[_MainTex]{}        // Color tint
	}
	}

		// ------------------------------------------------------------------
		// Fallback for older cards and Unity non-Pro

		SubShader{
		Blend DstColor Zero
		Pass{
		Name "BASE"
		Cull Off

		SetTexture[_MainTex]{ combine texture }
	}
	}
	}

}