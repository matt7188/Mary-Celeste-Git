Shader "VacuumShaders/Advanced Dissolve/TextMeshPro/Bitmap Custom Atlas" {

Properties {
	_MainTex		("Font Atlas", 2D) = "white" {}
	_FaceTex		("Font Texture", 2D) = "white" {}
	_FaceColor		("Text Color", Color) = (1,1,1,1)

	_VertexOffsetX	("Vertex OffsetX", float) = 0
	_VertexOffsetY	("Vertex OffsetY", float) = 0
	_MaskSoftnessX	("Mask SoftnessX", float) = 0
	_MaskSoftnessY	("Mask SoftnessY", float) = 0

	_ClipRect("Clip Rect", vector) = (-32767, -32767, 32767, 32767)
	_Padding		("Padding", float) = 0

	_StencilComp("Stencil Comparison", Float) = 8
	_Stencil("Stencil ID", Float) = 0
	_StencilOp("Stencil Operation", Float) = 0
	_StencilWriteMask("Stencil Write Mask", Float) = 255
	_StencilReadMask("Stencil Read Mask", Float) = 255

	_ColorMask("Color Mask", Float) = 15



	//Advanced Dissolve
	[HideInInspector] _DissolveCutoff("Dissolve", Range(0,1)) = 0.25
		
	//Mask
	[HideInInspector][KeywordEnum(None, XYZ Axis, Plane, Sphere, Box, Cylinder, Cone)]  _DissolveMask("Mak", Float) = 0
	[HideInInspector][Enum(X,0,Y,1,Z,2)]                                                _DissolveMaskAxis("Axis", Float) = 0
[HideInInspector][Enum(World,0,Local,1)]                                            _DissolveMaskSpace("Space", Float) = 0	 
	[HideInInspector]																   _DissolveMaskOffset("Offset", Float) = 0
	[HideInInspector]																   _DissolveMaskInvert("Invert", Float) = 1		
	[HideInInspector][KeywordEnum(One, Two, Three, Four)]							   _DissolveMaskCount("Count", Float) = 0		
	
	[HideInInspector]  _DissolveMaskPosition("", Vector) = (0,0,0,0)
    [HideInInspector]  _DissolveMaskNormal("", Vector) = (1,0,0,0)
    [HideInInspector]  _DissolveMaskRadius("", Float) = 1

	//Alpha Source
	[HideInInspector][KeywordEnum(Main Map Alpha, Custom Map, Two Custom Maps, Three Custom Maps)]  _DissolveAlphaSource("Alpha Source", Float) = 0
	[HideInInspector]_DissolveMap1("", 2D) = "white" { }
    [HideInInspector][UVScroll]  _DissolveMap1_Scroll("", Vector) = (0,0,0,0)
    [HideInInspector]_DissolveMap2("", 2D) = "white" { }
    [HideInInspector][UVScroll]  _DissolveMap2_Scroll("", Vector) = (0,0,0,0)
    [HideInInspector]_DissolveMap3("", 2D) = "white" { }
    [HideInInspector][UVScroll]  _DissolveMap3_Scroll("", Vector) = (0,0,0,0)

	[HideInInspector][Enum(Multiply, 0, Add, 1)]  _DissolveSourceAlphaTexturesBlend("Texture Blend", Float) = 0
	[HideInInspector]							  _DissolveNoiseStrength("Noise", Float) = 0.1
	[HideInInspector][Enum(UV0,0,UV1,1)]          _DissolveAlphaSourceTexturesUVSet("UV Set", Float) = 0

	[HideInInspector][KeywordEnum(Normal, Triplanar, Screen Space)] _DissolveMappingType("Triplanar", Float) = 0
	[HideInInspector][Enum(World,0,Local,1)]                        _DissolveTriplanarMappingSpace("Mapping", Float) = 0	
	[HideInInspector]                                               _DissolveMainMapTiling("", FLOAT) = 1	

	//Edge
	[HideInInspector]                                       _DissolveEdgeWidth("Edge Size", Range(0,1)) = 0.25
	[HideInInspector][Enum(Cutout Source,0,Main Map,1)]     _DissolveEdgeDistortionSource("Distortion Source", Float) = 0
	[HideInInspector]                                       _DissolveEdgeDistortionStrength("Distortion Strength", Range(0, 2)) = 0

	//Color
	[HideInInspector]                _DissolveEdgeColor("Edge Color", Color) = (0,1,0,1)
	[HideInInspector][PositiveFloat] _DissolveEdgeColorIntensity("Intensity", FLOAT) = 0
	[HideInInspector][Enum(Solid,0,Smooth,1, Smooth Squared,2)]      _DissolveEdgeShape("Shape", INT) = 0

	[HideInInspector][KeywordEnum(None, Gradient, Main Map, Custom)] _DissolveEdgeTextureSource("", Float) = 0
	[HideInInspector][NoScaleOffset]								 _DissolveEdgeTexture("Edge Texture", 2D) = "white" { }
	[HideInInspector][Toggle]										 _DissolveEdgeTextureReverse("Reverse", FLOAT) = 0
	[HideInInspector]												 _DissolveEdgeTexturePhaseOffset("Offset", FLOAT) = 0
	[HideInInspector]												 _DissolveEdgeTextureAlphaOffset("Offset", Range(-1, 1)) = 0	
	[HideInInspector]												 _DissolveEdgeTextureMipmap("", Range(0, 10)) = 1		
	[HideInInspector][Toggle]										 _DissolveEdgeTextureIsDynamic("", Float) = 0

	[HideInInspector][PositiveFloat] _DissolveGIMultiplier("GI Strength", Float) = 1	
		
	//Global
	[HideInInspector][KeywordEnum(None, Mask Only, Mask And Edge, All)] _DissolveGlobalControl("Global Controll", Float) = 0

	//Meta
    [HideInInspector] _Dissolve_ObjectWorldPos("", Vector) = (0,0,0,0)		
}

SubShader{

	Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
	
	Stencil
	{
		Ref[_Stencil]
		Comp[_StencilComp]
		Pass[_StencilOp]
		ReadMask[_StencilReadMask]
		WriteMask[_StencilWriteMask]
	}
	
	
	Lighting Off
	Cull [_CullMode]
	ZTest [unity_GUIZTestMode]
	ZWrite Off
	Fog { Mode Off }
	Blend SrcAlpha OneMinusSrcAlpha
	ColorMask[_ColorMask]

	Pass {
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag

		#pragma multi_compile __ UNITY_UI_CLIP_RECT
		#pragma multi_compile __ UNITY_UI_ALPHACLIP


		#include "UnityCG.cginc"


		sampler2D _MainTex;
		float4 _MainTex_ST;
		fixed _Cutoff;


#pragma shader_feature _ _ALPHATEST_ON _ALPHABLEND_ON

#pragma shader_feature _ _DISSOLVEGLOBALCONTROL_MASK_ONLY _DISSOLVEGLOBALCONTROL_MASK_AND_EDGE _DISSOLVEGLOBALCONTROL_ALL
#pragma shader_feature _ _DISSOLVEMAPPINGTYPE_TRIPLANAR _DISSOLVEMAPPINGTYPE_SCREEN_SPACE
#pragma shader_feature _ _DISSOLVEALPHASOURCE_CUSTOM_MAP _DISSOLVEALPHASOURCE_TWO_CUSTOM_MAPS _DISSOLVEALPHASOURCE_THREE_CUSTOM_MAPS
#pragma shader_feature _ _DISSOLVEMASK_XYZ_AXIS _DISSOLVEMASK_PLANE _DISSOLVEMASK_SPHERE _DISSOLVEMASK_BOX _DISSOLVEMASK_CYLINDER _DISSOLVEMASK_CONE
#pragma shader_feature _ _DISSOLVEEDGETEXTURESOURCE_GRADIENT _DISSOLVEEDGETEXTURESOURCE_MAIN_MAP _DISSOLVEEDGETEXTURESOURCE_CUSTOM
#pragma shader_feature _ _DISSOLVEMASKCOUNT_TWO _DISSOLVEMASKCOUNT_THREE _DISSOLVEMASKCOUNT_FOUR


#include "../cginc/AdvancedDissolve.cginc"


		struct appdata_t {
			float4 vertex		: POSITION;
			fixed4 color		: COLOR;
			float2 texcoord0	: TEXCOORD0;
			float2 texcoord1	: TEXCOORD1;
			float3 normal       : NORMAL;
		};

		struct v2f {
			float4	vertex		: SV_POSITION;
			fixed4	color		: COLOR;
			float2	texcoord0	: TEXCOORD0;
			float2	texcoord1	: TEXCOORD1;
			float4	mask		: TEXCOORD2;


			float3 worldPos : TEXCOORD3;
#ifdef _DISSOLVEMAPPINGTYPE_TRIPLANAR
			half3 objNormal : TEXCOORD4;
			float3 coords : TEXCOORD5;
#else
			float4 dissolveUV : TEXCOORD4;
#endif	
		};

		uniform	sampler2D 	_FaceTex;
		uniform float4		_FaceTex_ST;
		uniform	fixed4		_FaceColor;

		uniform float		_VertexOffsetX;
		uniform float		_VertexOffsetY;
		uniform float4		_ClipRect;
		uniform float		_MaskSoftnessX;
		uniform float		_MaskSoftnessY;

		float2 UnpackUV(float uv)
		{
			float2 output;
			output.x = floor(uv / 4096);
			output.y = uv - 4096 * output.x;

			return output * 0.001953125;
		}

		v2f vert (appdata_t v)
		{
			float4 vert = v.vertex;
			vert.x += _VertexOffsetX;
			vert.y += _VertexOffsetY;

			vert.xy += (vert.w * 0.5) / _ScreenParams.xy;

			float4 vPosition = UnityPixelSnap(UnityObjectToClipPos(vert));

			fixed4 faceColor = v.color;
			faceColor *= _FaceColor;

			v2f OUT;
			OUT.vertex = vPosition;
			OUT.color = faceColor;
			OUT.texcoord0 = v.texcoord0;
			OUT.texcoord1 = TRANSFORM_TEX(UnpackUV(v.texcoord1), _FaceTex);
			float2 pixelSize = vPosition.w;
			pixelSize /= abs(float2(_ScreenParams.x * UNITY_MATRIX_P[0][0], _ScreenParams.y * UNITY_MATRIX_P[1][1]));

			// Clamp _ClipRect to 16bit.
			float4 clampedRect = clamp(_ClipRect, -2e10, 2e10);
			OUT.mask = float4(vert.xy * 2 - clampedRect.xy - clampedRect.zw, 0.25 / (0.25 * half2(_MaskSoftnessX, _MaskSoftnessY) + pixelSize.xy));
			


			//VacuumShaders 
			OUT.worldPos = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1)).xyz;
#ifdef _DISSOLVEMAPPINGTYPE_TRIPLANAR
			OUT.coords = float4(v.vertex.xyz, 1);
			OUT.objNormal = lerp(UnityObjectToWorldNormal(v.normal), v.normal, VALUE_TRIPLANARMAPPINGSPACE);
#else
			DissolveVertex2Fragment(vPosition, v.texcoord0, v.texcoord1, OUT.dissolveUV);
#endif

			return OUT;
		}

		fixed4 frag (v2f IN) : SV_Target
		{

#ifdef _DISSOLVEMAPPINGTYPE_TRIPLANAR
			float4 alpha = ReadDissolveAlpha_Triplanar(IN.coords, IN.objNormal, IN.worldPos);
#else
			float4 alpha = ReadDissolveAlpha(IN.texcoord0.xy, IN.dissolveUV, IN.worldPos);
#endif				
		DoDissolveClip(alpha);



		float3 dissolveAlbedo = 0;
		float3 dissolveEmission = 0;
		float dissolveBlend = DoDissolveAlbedoEmission(alpha, dissolveAlbedo, dissolveEmission, IN.texcoord0.xy);





			fixed4 color = tex2D(_MainTex, IN.texcoord0) * tex2D(_FaceTex, IN.texcoord1) * IN.color;

		
			// Alternative implementation to UnityGet2DClipping with support for softness.
			#if UNITY_UI_CLIP_RECT
				half2 m = saturate((_ClipRect.zw - _ClipRect.xy - abs(IN.mask.xy)) * IN.mask.zw);
				color *= m.x * m.y;
			#endif

			#if UNITY_UI_ALPHACLIP
				clip(color.a - 0.001);
			#endif

				//Diffuse
				color.rgb = lerp(color.rgb, dissolveAlbedo, dissolveBlend);

				//Emission
				color.rgb += dissolveEmission * dissolveBlend;


			return color;
		}
		ENDCG
	}
}

CustomEditor "AdvancedDissolveTextMeshProGUI"
}
