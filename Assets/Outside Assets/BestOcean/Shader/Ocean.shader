

Shader "Ocean/Ocean"
{
	Properties
	{
		[NoScaleOffset] _Normals ( "Normal Map", 2D ) = "bump" {}
		_NormalsStrength("Strength", Range(0.01, 2.0)) = 0.3
		_NormalsScale("Scale", Range(0.01, 50.0)) = 1.0

		_Diffuse("Diffuse", Color) = (0.2, 0.05, 0.05, 1.0)

		_SubSurfaceColour("Colour", Color) = (0.0, 0.48, 0.36)
		_SubSurfaceBase("Base Mul", Range(0.0, 2.0)) = 0.6
		_SubSurfaceSun("Sun Mul", Range(0.0, 10.0)) = 0.8
		_SubSurfaceSunFallOff("Sun Fall-Off", Range(1.0, 16.0)) = 4.0

		[Header(Height Based Scattering)]
		[Toggle] _SubSurfaceHeightLerp("Enable", Float) = 1
		_SubSurfaceHeightMax("Height Max", Range(0.0, 50.0)) = 3.0
		_SubSurfaceHeightPower("Height Power", Range(0.01, 10.0)) = 1.0
		_SubSurfaceCrestColour("Crest Colour", Color) = (0.42, 0.69, 0.52)

		[Header(Shallow Scattering)]
		[Toggle] _SubSurfaceShallowColour("Enable", Float) = 1
		_SubSurfaceDepthMax("Depth Max", Range(0.01, 50.0)) = 3.0
		_SubSurfaceDepthPower("Depth Power", Range(0.01, 10.0)) = 1.0
		_SubSurfaceShallowCol("Shallow Colour", Color) = (0.42, 0.75, 0.69)

		[Header(Reflection Environment)]
		_FresnelPower("Fresnel Power", Range(0.0, 20.0)) = 3.0
		[NoScaleOffset] _Skybox ("Skybox", CUBE) = "" {}

		[Header(Add Directional Light)]
		[Toggle] _ComputeDirectionalLight("Enable", Float) = 1
		_DirectionalLightFallOff("Fall-Off", Range(1.0, 4096.0)) = 128.0
		_DirectionalLightBoost("Boost", Range(0.0, 512.0)) = 5.0

		[Header(Foam)]
		[NoScaleOffset] _FoamTexture ( "Texture", 2D ) = "white" {}
		_FoamScale("Scale", Range(0.01, 50.0)) = 10.0
		_FoamWhiteColor("White Foam Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_FoamBubbleColor("Bubble Foam Color", Color) = (0.64, 0.83, 0.82, 1.0)
		_FoamBubbleParallax("Bubble Foam Parallax", Range(0.0, 0.25)) = 0.05
		_ShorelineFoamMinDepth("Shoreline Foam Min Depth", Range(0.001, 5.0)) = 0.27
		_WaveFoamFeather("Wave Foam Feather", Range(0.001, 1.0)) = 0.32
		_WaveFoamBubblesCoverage("Wave Foam Bubbles Coverage", Range(0.0, 5.0)) = 0.95

		_WaveTex("_WaveTex", 2D) = "black" {}
		_Foam2Tex("_Foam2Tex", 2D) = "black" {}
		_FoamShinee("_FoamShinee", Range(0.1, 5)) = 1
		[Header(Transparency)]
		_DepthFogDensity("Density", Vector) = (0.28, 0.16, 0.24, 1.0)
		_RefractionStrength("Refraction Strength", Range(0.0, 1.0)) = 0.1

		[Header(Caustics)]
		[Toggle] _Caustics("Enable", Float) = 1
		[NoScaleOffset] _CausticsTexture ("Caustics", 2D ) = "black" {}
		_CausticsTextureScale("Scale", Range(0.0, 25.0)) = 5.0
		_CausticsTextureAverage("Texture Average Value", Range(0.0, 1.0)) = 0.07
		_CausticsStrength("Strength", Range(0.0, 10.0)) = 3.2
		_CausticsFocalDepth("Focal Depth", Range(0.0, 25.0)) = 2.0
		_CausticsDepthOfField("Depth Of Field", Range(0.01, 10.0)) = 0.33
		_CausticsDistortionScale("Distortion Scale", Range(0.01, 50.0)) = 10.0
		_CausticsDistortionStrength("Distortion Strength", Range(0.0, 0.25)) = 0.075


		[Header(Render State)]
		[Enum(CullMode)] _CullMode("Cull Mode", Int) = 2
	}

	Category
	{
		Tags {}

		SubShader
		{
			Tags { "LightMode"="ForwardBase" "Queue"="Geometry+510" "IgnoreProjector"="True" "RenderType"="Opaque" }

			GrabPass
			{
				"_BackgroundTexture"
			}

			Pass
			{
				// Culling user defined - can be inverted for under water
				Cull [_CullMode]

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#pragma multi_compile_fog

				#pragma shader_feature _COMPUTEDIRECTIONALLIGHT_ON
				#pragma shader_feature _SUBSURFACEHEIGHTLERP_ON
				#pragma shader_feature _SUBSURFACESHALLOWCOLOUR_ON
				#pragma shader_feature _CAUSTICS_ON
				#pragma shader_feature _UNDERWATER_ON


				#include "UnityCG.cginc"
				#include "Lighting.cginc"

				struct appdata_t
				{
					float4 vertex : POSITION;
					float2 texcoord: TEXCOORD0;
				};

				struct v2f
				{
					float4 vertex : SV_POSITION;
					half4 n_shadow : TEXCOORD1;
					float2 uv : TEXCOORD2;
					half4 foam_screenPos : TEXCOORD4;
					half4 lodAlpha_worldXZUndisplaced_oceanDepth : TEXCOORD5;
					float3 worldPos : TEXCOORD7;

					half4 grabPos : TEXCOORD9;
					
					UNITY_FOG_COORDS( 3 )
				};
				// Samplers and data associated with a LOD.
// _LD_Params: float4(world texel size, texture resolution, shape weight multiplier, 1 / texture resolution)
#define LOD_DATA(LODNUM) \
	uniform sampler2D _LD_Sampler_AnimatedWaves_##LODNUM; \
	uniform sampler2D _LD_Sampler_SeaFloorDepth_##LODNUM; \
	uniform sampler2D _LD_Sampler_Foam_##LODNUM; \
	uniform sampler2D _LD_Sampler_Flow_##LODNUM; \
	uniform sampler2D _LD_Sampler_DynamicWaves_##LODNUM; \
	uniform sampler2D _LD_Sampler_Shadow_##LODNUM; \
	uniform float4 _LD_Params_##LODNUM; \
	uniform float3 _LD_Pos_Scale_##LODNUM;

// Create two sets of LOD data, which have overloaded meaning depending on use:
// * the ocean surface geometry always lerps from a more detailed LOD (0) to a less detailed LOD (1)
// * simulations (persistent lod data) read last frame's data from slot 0, and any current frame data from slot 1
// * any other use that does not fall into the previous categories can use either slot and generally use slot 0
				LOD_DATA(0)
				LOD_DATA(1)
					uniform float depthMax;
#define DEPTH_OUTSCATTER_CONSTANT 0.25
				uniform float NowTime;
				uniform half3 _Diffuse;
				// MeshScaleLerp, FarNormalsWeight, LODIndex (debug), unused
				uniform float4 _InstanceData;
				uniform float3 _GeomData;
				uniform float3 _OceanCenterPosWorld;
				uniform sampler2D _Normals;
				uniform half4 _DepthFogDensity;
				uniform sampler2D _CameraDepthTexture;
				uniform half _FresnelPower;
				// Hack - due to SV_IsFrontFace occasionally coming through as true for backfaces,
				// add a param here that forces ocean to be in undrwater state. I think the root
				// cause here might be imprecision or numerical issues at ocean tile boundaries, although
				// i'm not sure why cracks are not visible in this case.
				uniform float _ForceUnderwater;

				uniform half _SubSurfaceDepthMax;
				uniform half _SubSurfaceDepthPower;
				uniform half3 _SubSurfaceShallowCol;

				uniform half3 _SubSurfaceColour;
				uniform half _SubSurfaceBase;
				uniform half _SubSurfaceSun;
				uniform half _SubSurfaceSunFallOff;
				uniform half _SubSurfaceHeightMax;
				uniform half _SubSurfaceHeightPower;
				uniform half3 _SubSurfaceCrestColour;


				uniform half _RefractionStrength;


#if _CAUSTICS_ON
				uniform sampler2D _CausticsTexture;
				uniform half _CausticsTextureScale;
				uniform half _CausticsTextureAverage;
				uniform half _CausticsStrength;
				uniform half _CausticsFocalDepth;
				uniform half _CausticsDepthOfField;
				uniform half _CausticsDistortionScale;
				uniform half _CausticsDistortionStrength;
#endif // _CAUSTICS_ON
#if _COMPUTEDIRECTIONALLIGHT_ON
				uniform half _DirectionalLightFallOff;
				uniform half _DirectionalLightBoost;
#endif

				uniform sampler2D _BackgroundTexture;
				sampler2D _WaveTex;
				sampler2D _Foam2Tex;

				



				float2 LD_WorldToUV(in float2 i_samplePos, in float2 i_centerPos, in float i_res, in float i_texelSize)
				{
					return (i_samplePos - i_centerPos) / (i_texelSize * i_res) + 0.5;
				}
				float2 LD_0_WorldToUV(in float2 i_samplePos) { return LD_WorldToUV(i_samplePos, _LD_Pos_Scale_0.xy, _LD_Params_0.y, _LD_Params_0.x); }
				float2 LD_1_WorldToUV(in float2 i_samplePos) { return LD_WorldToUV(i_samplePos, _LD_Pos_Scale_1.xy, _LD_Params_1.y, _LD_Params_1.x); }

				float2 LD_UVToWorld(in float2 i_uv, in float2 i_centerPos, in float i_res, in float i_texelSize)
				{
					return i_texelSize * i_res * (i_uv - 0.5) + i_centerPos;
				}
				float2 LD_0_UVToWorld(in float2 i_uv) { return LD_UVToWorld(i_uv, _LD_Pos_Scale_0.xy, _LD_Params_0.y, _LD_Params_0.x); }
				float2 LD_1_UVToWorld(in float2 i_uv) { return LD_UVToWorld(i_uv, _LD_Pos_Scale_1.xy, _LD_Params_1.y, _LD_Params_1.x); }
				float ComputeLodAlpha(float3 i_worldPos, float i_meshScaleAlpha)
				{
					// taxicab distance from ocean center drives LOD transitions
					float2 offsetFromCenter = float2(abs(i_worldPos.x - _OceanCenterPosWorld.x), abs(i_worldPos.z - _OceanCenterPosWorld.z));
					float taxicab_norm = max(offsetFromCenter.x, offsetFromCenter.y);

					// interpolation factor to next lod (lower density / higher sampling period)
					float lodAlpha = taxicab_norm / _LD_Pos_Scale_0.z - 1.0;

					// lod alpha is remapped to ensure patches weld together properly. patches can vary significantly in shape (with
					// strips added and removed), and this variance depends on the base density of the mesh, as this defines the strip width.
					// using .15 as black and .85 as white should work for base mesh density as low as 16.
					const float BLACK_POINT = 0.15, WHITE_POINT = 0.85;
					lodAlpha = max((lodAlpha - BLACK_POINT) / (WHITE_POINT - BLACK_POINT), 0.);

					// blend out lod0 when viewpoint gains altitude
					lodAlpha = min(lodAlpha + i_meshScaleAlpha, 1.);
					return lodAlpha;
				}
				// Sampling functions
				void SampleDisplacements(in sampler2D i_dispSampler, in float2 i_uv, in float i_wt, inout float3 io_worldPos)
				{
					const half3 disp = tex2Dlod(i_dispSampler, float4(i_uv, 0., 0.)).xyz;
					io_worldPos += i_wt * disp;
				}
#if _CAUSTICS_ON
				void ApplyCaustics(in const half3 i_view, in const half3 i_lightDir, in const float i_sceneZ, in sampler2D i_normals, inout half3 io_sceneColour)
				{
					// could sample from the screen space shadow texture to attenuate this..
					// underwater caustics - dedicated to P
					float3 camForward = mul((float3x3)unity_CameraToWorld, float3(0., 0., 1.));
					float3 scenePos = _WorldSpaceCameraPos - i_view * i_sceneZ / dot(camForward, -i_view);
					const float2 scenePosUV = LD_1_WorldToUV(scenePos.xz);
					half3 disp = 0.;
					// this gives height at displaced position, not exactly at query position.. but it helps. i cant pass this from vert shader
					// because i dont know it at scene pos.
					SampleDisplacements(_LD_Sampler_AnimatedWaves_1, scenePosUV, 1.0, disp);
					half waterHeight = _OceanCenterPosWorld.y + disp.y;
					half sceneDepth = waterHeight - scenePos.y;
					half bias = abs(sceneDepth - _CausticsFocalDepth) / _CausticsDepthOfField;
					// project along light dir, but multiply by a fudge factor reduce the angle bit - compensates for fact that in real life
					// caustics come from many directions and don't exhibit such a strong directonality
					float2 surfacePosXZ = scenePos.xz + i_lightDir.xz * sceneDepth / (4.*i_lightDir.y);
					half2 causticN = _CausticsDistortionStrength * UnpackNormal(tex2D(i_normals, surfacePosXZ / _CausticsDistortionScale)).xy;
					half4 cuv1 = half4((surfacePosXZ / _CausticsTextureScale + 1.3 *causticN + half2(0.044*NowTime + 17.16, -0.169*NowTime)), 0., bias);
					half4 cuv2 = half4((1.37*surfacePosXZ / _CausticsTextureScale + 1.77*causticN + half2(0.248*NowTime, 0.117*NowTime)), 0., bias);

					half causticsStrength = _CausticsStrength;

					io_sceneColour *= 1. + causticsStrength *
						(0.5*tex2Dbias(_CausticsTexture, cuv1).x + 0.5*tex2Dbias(_CausticsTexture, cuv2).x - _CausticsTextureAverage);
				}
#endif // _CAUSTICS_ON
				void SnapAndTransitionVertLayout(float i_meshScaleAlpha, inout float3 io_worldPos, out float o_lodAlpha)
				{
					// see comments above on _GeomData
					const float SQUARE_SIZE_2 = 2.0*_GeomData.x, SQUARE_SIZE_4 = 4.0*_GeomData.x;

					// snap the verts to the grid
					// The snap size should be twice the original size to keep the shape of the eight triangles (otherwise the edge layout changes).
					io_worldPos.xz -= frac(unity_ObjectToWorld._m03_m23 / SQUARE_SIZE_2) * SQUARE_SIZE_2; // caution - sign of frac might change in non-hlsl shaders

					// compute lod transition alpha
					o_lodAlpha = ComputeLodAlpha(io_worldPos, i_meshScaleAlpha);

					// now smoothly transition vert layouts between lod levels - move interior verts inwards towards center
					float2 m = frac(io_worldPos.xz / SQUARE_SIZE_4); // this always returns positive
					float2 offset = m - 0.5;
					// check if vert is within one square from the center point which the verts move towards
					const float minRadius = 0.26; //0.26 is 0.25 plus a small "epsilon" - should solve numerical issues
					if (abs(offset.x) < minRadius) io_worldPos.x += offset.x * o_lodAlpha * SQUARE_SIZE_4;
					if (abs(offset.y) < minRadius) io_worldPos.z += offset.y * o_lodAlpha * SQUARE_SIZE_4;
				}

				void SampleDisplacementsNormals(in sampler2D i_dispSampler, in float2 i_uv, in float i_wt, in float i_invRes, in float i_texelSize, inout float3 io_worldPos, inout half2 io_nxz)
				{
					const float4 uv = float4(i_uv, 0., 0.);

					const half3 disp = tex2Dlod(i_dispSampler, uv).xyz;
					io_worldPos += i_wt * disp;

					float3 n; {
						float3 dd = float3(i_invRes, 0.0, i_texelSize);
						half3 disp_x = dd.zyy + tex2Dlod(i_dispSampler, uv + dd.xyyy).xyz;
						half3 disp_z = dd.yyz + tex2Dlod(i_dispSampler, uv + dd.yxyy).xyz;
						n = normalize(cross(disp_z - disp, disp_x - disp));
					}
					io_nxz += i_wt * n.xz;
				}


				uniform half _NormalsStrength;
				uniform half _NormalsScale;
				uniform half ViewY;
				uniform samplerCUBE _Skybox;
				half2 SampleNormalMaps(float2 worldXZUndisplaced, float lodAlpha)
				{
					const float2 v0 = float2(0.94, 0.34), v1 = float2(-0.85, -0.53);
					const float geomSquareSize = _GeomData.x;
					float nstretch = _NormalsScale * geomSquareSize; // normals scaled with geometry
					const float spdmulL = _GeomData.y;
					half2 norm =
						UnpackNormal(tex2D(_Normals, (v0*NowTime*spdmulL + worldXZUndisplaced) / nstretch)).xy +
						UnpackNormal(tex2D(_Normals, (v1*NowTime*spdmulL + worldXZUndisplaced) / nstretch)).xy;

					// blend in next higher scale of normals to obtain continuity
					const float farNormalsWeight = _InstanceData.y;
					const half nblend = lodAlpha * farNormalsWeight;
					if (nblend > 0.001)
					{
						// next lod level
						nstretch *= 2.;
						const float spdmulH = _GeomData.z;
						norm = lerp(norm,
							UnpackNormal(tex2D(_Normals, (v0*NowTime*spdmulH + worldXZUndisplaced) / nstretch)).xy +
							UnpackNormal(tex2D(_Normals, (v1*NowTime*spdmulH + worldXZUndisplaced) / nstretch)).xy,
							nblend);
					}

					// approximate combine of normals. would be better if normals applied in local frame.
					return _NormalsStrength * norm;
				}
				void SampleFoam(in sampler2D i_oceanFoamSampler, float2 i_uv, in float i_wt, inout half io_foam)
				{
					io_foam += i_wt * tex2Dlod(i_oceanFoamSampler, float4(i_uv, 0., 0.)).x;
				}

				void SampleFlow(in sampler2D i_oceanFlowSampler, float2 i_uv, in float i_wt, inout half2 io_flow)
				{
					const float4 uv = float4(i_uv, 0., 0.);
					io_flow += i_wt * tex2Dlod(i_oceanFlowSampler, uv).xy;
				}
				void SampleSeaFloorHeightAboveBaseline(in sampler2D i_oceanDepthSampler, float2 i_uv, in float i_wt, inout half io_oceanDepth)
				{
					io_oceanDepth += i_wt * (tex2Dlod(i_oceanDepthSampler, float4(i_uv, 0., 0.)).x);
				}



				
				void ApplyNormalMapsWithFlow(float2 worldXZUndisplaced, float2 flow, float lodAlpha, inout half3 io_n)
				{
					const float half_period = 1;
					const float period = half_period * 2;
					float sample1_offset = fmod(NowTime, period);
					float sample1_weight = sample1_offset / half_period;
					if (sample1_weight > 1.0) sample1_weight = 2.0 - sample1_weight;
					float sample2_offset = fmod(NowTime + half_period, period);
					float sample2_weight = 1.0 - sample1_weight;

					// In order to prevent flow from distorting the UVs too much,
					// we fade between two samples of normal maps so that for each
					// sample the UVs can be reset
					half2 io_n_1 = SampleNormalMaps(worldXZUndisplaced - (flow * sample1_offset), lodAlpha);
					half2 io_n_2 = SampleNormalMaps(worldXZUndisplaced - (flow * sample2_offset), lodAlpha);
					io_n.xz += sample1_weight * io_n_1;
					io_n.xz += sample2_weight * io_n_2;
					io_n = normalize(io_n);
				}
				v2f vert( appdata_t v )
				{
					v2f o;

					// move to world
					o.worldPos = mul(unity_ObjectToWorld, v.vertex);

					// vertex snapping and lod transition
					float lodAlpha;
					SnapAndTransitionVertLayout(_InstanceData.x, o.worldPos, lodAlpha);
					o.lodAlpha_worldXZUndisplaced_oceanDepth.x = lodAlpha;
					o.lodAlpha_worldXZUndisplaced_oceanDepth.yz = o.worldPos.xz;
					
					// sample shape textures - always lerp between 2 LOD scales, so sample two textures
					o.n_shadow = half4(0., 0., 0., 0.);
					o.foam_screenPos.x = 0.;



					o.lodAlpha_worldXZUndisplaced_oceanDepth.w = 0.;

					// sample weights. params.z allows shape to be faded out (used on last lod to support pop-less scale transitions)
					float wt_0 = (1. - lodAlpha) * _LD_Params_0.z;
					float wt_1 = (1. - wt_0) * _LD_Params_1.z;
					// sample displacement textures, add results to current world pos / normal / foam
					const float2 worldXZBefore = o.worldPos.xz;
					const float2 uv_0 = LD_0_WorldToUV(worldXZBefore);
					o.uv = worldXZBefore.xy;
					if (wt_0 > 0.00)
					{
						
						SampleDisplacementsNormals(_LD_Sampler_AnimatedWaves_0, uv_0, wt_0, _LD_Params_0.w, _LD_Params_0.x, o.worldPos, o.n_shadow.xy);


						SampleFoam(_LD_Sampler_Foam_0, uv_0, wt_0, o.foam_screenPos.x);


						#if _SUBSURFACESHALLOWCOLOUR_ON
						SampleSeaFloorHeightAboveBaseline(_LD_Sampler_SeaFloorDepth_0, uv_0, wt_0, o.lodAlpha_worldXZUndisplaced_oceanDepth.w);
						#endif
					}
					if (wt_1 > 0.00)
					{
						const float2 uv_1 = LD_1_WorldToUV(worldXZBefore);
						SampleDisplacementsNormals(_LD_Sampler_AnimatedWaves_1, uv_1, wt_1, _LD_Params_1.w, _LD_Params_1.x, o.worldPos, o.n_shadow.xy);


						SampleFoam(_LD_Sampler_Foam_1, uv_1, wt_1, o.foam_screenPos.x);



						#if _SUBSURFACESHALLOWCOLOUR_ON
						SampleSeaFloorHeightAboveBaseline(_LD_Sampler_SeaFloorDepth_1, uv_1, wt_1, o.lodAlpha_worldXZUndisplaced_oceanDepth.w);
						#endif

					}

					// convert height above -1000m to depth below surface
					o.lodAlpha_worldXZUndisplaced_oceanDepth.w = depthMax - o.lodAlpha_worldXZUndisplaced_oceanDepth.w;

					// foam can saturate
					o.foam_screenPos.x = saturate(o.foam_screenPos.x);
					// view-projection
					o.vertex = mul(UNITY_MATRIX_VP, float4(o.worldPos, 1.));

					UNITY_TRANSFER_FOG(o, o.vertex);

					// unfortunate hoop jumping - this is inputs for refraction. depending on whether HDR is on or off, the grabbed scene
					// colours may or may not come from the backbuffer, which means they may or may not be flipped in y. use these macros
					// to get the right results, every time.
					o.grabPos = ComputeGrabScreenPos(o.vertex);
					o.foam_screenPos.yzw = ComputeScreenPos(o.vertex).xyw;
					return o;
				}

				// frag shader uniforms

				

				float3 WorldSpaceLightDir(float3 worldPos)
				{
					float3 lightDir = _WorldSpaceLightPos0.xyz;
					if (_WorldSpaceLightPos0.w > 0.)
					{
						// non-directional light - this is a position, not a direction
						lightDir = normalize(lightDir - worldPos.xyz);
					}
					return lightDir;
				}
				

				bool IsUnderwater(const bool i_isFrontFace)
				{
#if _UNDERWATER_ON
					return !i_isFrontFace || _ForceUnderwater > 0.0;
#else
					return false;
#endif
				}
				void SampleShadow(in sampler2D i_oceanShadowSampler, float2 i_uv, in float i_wt, inout fixed2 io_shadow)
				{
					io_shadow += i_wt * tex2Dlod(i_oceanShadowSampler, float4(i_uv, 0., 0.)).xy;
				}

				half3 ScatterColour(
					in const float3 i_surfaceWorldPos, in const half i_surfaceOceanDepth, in const float3 i_cameraPos,
					in const half3 i_lightDir, in const half3 i_view, in const fixed i_shadow,
					in const bool i_underWater, in const bool i_outscatterLight)
				{
					half depth;
					half waveHeight;
					half shadow = 0.;
					

					depth = i_surfaceOceanDepth;
					waveHeight = i_surfaceWorldPos.y - _OceanCenterPosWorld.y;
					shadow = i_shadow;
					

					// base colour
					half3 col = _Diffuse;
#ifdef UNITY_COLORSPACE_GAMMA
					col = pow(col, 1.5);
#endif


					{
#if _SUBSURFACESHALLOWCOLOUR_ON
						float shallowness = pow(1. - saturate(depth / _SubSurfaceDepthMax), _SubSurfaceDepthPower);
						half3 shallowCol = _SubSurfaceShallowCol;
						col = lerp(col, shallowCol, shallowness);

#endif					

#if _SUBSURFACEHEIGHTLERP_ON
						col += pow(saturate(0.5 + 2.0 * waveHeight / _SubSurfaceHeightMax), _SubSurfaceHeightPower) * _SubSurfaceCrestColour.rgb;
#endif

						// light
						// use the constant term (0th order) of SH stuff - this is the average. it seems to give the right kind of colour
						col *= half3(unity_SHAr.w, unity_SHAg.w, unity_SHAb.w);

						// Approximate subsurface scattering - add light when surface faces viewer. Use geometry normal - don't need high freqs.
						half towardsSun = pow(max(0., dot(i_lightDir, -i_view)), _SubSurfaceSunFallOff);
#ifdef UNITY_COLORSPACE_GAMMA
						_SubSurfaceColour = pow(_SubSurfaceColour, 2);
#endif
						col += (_SubSurfaceBase + _SubSurfaceSun * towardsSun) * _SubSurfaceColour.rgb * _LightColor0 * shadow;
					}

					// outscatter light - attenuate the final colour by the camera depth under the water, to approximate less
					// throughput due to light scatter as the camera gets further under water.
					if (i_outscatterLight)
					{
						half camDepth = i_surfaceWorldPos.y - _WorldSpaceCameraPos.y;
						if (i_underWater)
						{
							col *= exp(-_DepthFogDensity.xyz * camDepth * DEPTH_OUTSCATTER_CONSTANT);
						}
					}

					return col;
				}
				half3 OceanEmission(in const half3 i_view, in const half3 i_n_pixel, in const float3 i_lightDir,
					in const half4 i_grabPos, in const float i_pixelZ, in const half2 i_uvDepth, in const float i_sceneZ, in const float i_sceneZ01,
					in const half3 i_bubbleCol, in sampler2D i_normals, in sampler2D i_cameraDepths, in const bool i_underwater, in const half3 i_scatterCol)
				{
					half3 col = i_scatterCol;

					// underwater bubbles reflect in light
					col += i_bubbleCol;



					// View ray intersects geometry surface either above or below ocean surface

					const half2 uvBackground = i_grabPos.xy / i_grabPos.w;
					_RefractionStrength *= min(1, 15 / (ViewY - _OceanCenterPosWorld.y));
					half2 uvBackgroundRefract = uvBackground + _RefractionStrength * i_n_pixel.xz;
					half3 sceneColour;
					half3 alpha = 0.;
					float depthFogDistance;

					// Depth fog & caustics - only if view ray starts from above water
					if (!i_underwater)
					{
						const half2 uvDepthRefract = i_uvDepth + _RefractionStrength * i_n_pixel.xz;
						const float sceneZRefract = LinearEyeDepth(tex2D(i_cameraDepths, uvDepthRefract).x);

						// Compute depth fog alpha based on refracted position if it landed on an underwater surface, or on unrefracted depth otherwise
						if (sceneZRefract > i_pixelZ)
						{
							depthFogDistance = sceneZRefract - i_pixelZ;
						}
						else
						{
							depthFogDistance = i_sceneZ - i_pixelZ;

							// We have refracted onto a surface in front of the water. Cancel the refraction offset.
							uvBackgroundRefract = uvBackground;
						}

						sceneColour = tex2D(_BackgroundTexture, uvBackgroundRefract).rgb;
#if _CAUSTICS_ON
						ApplyCaustics(i_view, i_lightDir, i_sceneZ, i_normals, sceneColour);
#endif
					}
					else
					{
						sceneColour = tex2D(_BackgroundTexture, uvBackgroundRefract).rgb;
						depthFogDistance = i_pixelZ;
					}

					alpha = 1. - exp(-_DepthFogDensity.xyz * depthFogDistance);

					// blend from water colour to the scene colour
					col = lerp(sceneColour, col, alpha);



					return col;
				}
				void ApplyReflectionSky(half3 view, half3 n_pixel, half3 lightDir, half shadow, half4 i_screenPos, inout half3 col)
				{
					// Reflection
					half3 refl = reflect(-view, n_pixel);
					half3 skyColour;


					skyColour = texCUBE(_Skybox, refl).rgb;

#ifdef UNITY_COLORSPACE_GAMMA
					skyColour = pow(skyColour, 1.5);
#endif
					// Add primary light to boost it
#if _COMPUTEDIRECTIONALLIGHT_ON
					skyColour += pow(max(0., dot(refl, lightDir)), _DirectionalLightFallOff) * _DirectionalLightBoost * _LightColor0 * shadow;
#endif

					// Fresnel
					const float IOR_AIR = 1.0;
					const float IOR_WATER = 1.33;
					// reflectance at facing angle
					float R_0 = (IOR_AIR - IOR_WATER) / (IOR_AIR + IOR_WATER); R_0 *= R_0;
					// schlick's approximation
					float R_theta = R_0 + (1.0 - R_0) * pow(1.0 - max(dot(n_pixel, view), 0.), _FresnelPower);
					col = lerp(col, skyColour, R_theta);

				}

				



				uniform sampler2D _FoamTexture;
				uniform half _FoamScale;
				uniform float4 _FoamTexture_TexelSize;
				uniform half4 _FoamWhiteColor;
				uniform half4 _FoamBubbleColor;
				uniform half _FoamBubbleParallax;
				uniform half _ShorelineFoamMinDepth;
				uniform half _WaveFoamFeather;
				uniform half _WaveFoamBubblesCoverage;
				uniform half _WaveFoamNormalStrength;
				uniform half _WaveFoamSpecularFallOff;
				uniform half _WaveFoamSpecularBoost;
				uniform half _WaveFoamLightScale;
				uniform half2 _WindDirXZ;

				half3 AmbientLight()
				{
					return half3(unity_SHAr.w, unity_SHAg.w, unity_SHAb.w);
				}

				half WhiteFoamTexture(half i_foam, float2 i_worldXZUndisplaced)
				{
					half ft = lerp(
						tex2D(_FoamTexture, (1.25*i_worldXZUndisplaced + NowTime / 10.) / _FoamScale).r,
						tex2D(_FoamTexture, (3.00*i_worldXZUndisplaced - NowTime / 10.) / _FoamScale).r,
						0.5);

					// black point fade
					i_foam = saturate(1. - i_foam);
					return smoothstep(i_foam, i_foam + _WaveFoamFeather, ft);
				}

				void ComputeFoam(half i_foam, float2 i_worldXZUndisplaced, float2 i_worldXZ, half3 i_n, float i_pixelZ, float i_sceneZ, half3 i_view, float3 i_lightDir, half i_shadow, out half3 o_bubbleCol, out half4 o_whiteFoamCol)
				{
					half foamAmount = i_foam;
					float diffz = i_sceneZ - i_pixelZ;
					foamAmount *= saturate(diffz / _ShorelineFoamMinDepth);

					// Additive underwater foam - use same foam texture but add mip bias to blur for free
					float2 foamUVBubbles = (lerp(i_worldXZUndisplaced, i_worldXZ, 0.05) + 0.5 * NowTime * _WindDirXZ) / _FoamScale + 0.125 * i_n.xz;
					half bubbleFoamTexValue = tex2Dlod(_FoamTexture, float4(.74 * foamUVBubbles - _FoamBubbleParallax * i_view.xz / i_view.y, 0., 5.)).r;
					o_bubbleCol = (half3)bubbleFoamTexValue * _FoamBubbleColor.rgb * saturate(i_foam * _WaveFoamBubblesCoverage) * AmbientLight();

					// White foam on top, with black-point fading
					half whiteFoam = WhiteFoamTexture(foamAmount, i_worldXZUndisplaced);


					o_whiteFoamCol.rgb = _FoamWhiteColor.rgb * (AmbientLight() + _WaveFoamLightScale * _LightColor0 * i_shadow);

					o_whiteFoamCol.a = _FoamWhiteColor.a * whiteFoam;
					
				}

				void ComputeFoamWithFlow(half2 flow, half i_foam, float2 i_worldXZUndisplaced, float2 i_worldXZ, half3 i_n, float i_pixelZ, float i_sceneZ, half3 i_view, float3 i_lightDir, half i_shadow, out half3 o_bubbleCol, out half4 o_whiteFoamCol)
				{
					const float half_period = 1;
					const float period = half_period * 2;
					float sample1_offset = fmod(NowTime, period);
					float sample1_weight = sample1_offset / half_period;
					if (sample1_weight > 1.0) sample1_weight = 2.0 - sample1_weight;
					float sample2_offset = fmod(NowTime + half_period, period);
					float sample2_weight = 1.0 - sample1_weight;

					// In order to prevent flow from distorting the UVs too much,
					// we fade between two samples of normal maps so that for each
					// sample the UVs can be reset
					half3 o_bubbleCol1 = half3(0, 0, 0);
					half4 o_whiteFoamCol1 = half4(0, 0, 0, 0);
					half3 o_bubbleCol2 = half3(0, 0, 0);
					half4 o_whiteFoamCol2 = half4(0, 0, 0, 0);

					ComputeFoam(i_foam, i_worldXZUndisplaced - (flow * sample1_offset), i_worldXZ, i_n, i_pixelZ, i_sceneZ, i_view, i_lightDir, i_shadow, o_bubbleCol1, o_whiteFoamCol1);
					ComputeFoam(i_foam, i_worldXZUndisplaced - (flow * sample2_offset), i_worldXZ, i_n, i_pixelZ, i_sceneZ, i_view, i_lightDir, i_shadow, o_bubbleCol2, o_whiteFoamCol2);
					o_bubbleCol = (sample1_weight * o_bubbleCol1) + (sample2_weight * o_bubbleCol2);
					o_whiteFoamCol = (sample1_weight * o_whiteFoamCol1) + (sample2_weight * o_whiteFoamCol2);
				}


				float _FoamShinee;
				half4 frag(const v2f i, const bool i_isFrontFace : SV_IsFrontFace) : SV_Target
				{
					const bool underwater = IsUnderwater(i_isFrontFace);

					half3 view = normalize(_WorldSpaceCameraPos - i.worldPos);

					// water surface depth, and underlying scene opaque surface depth
					float pixelZ = LinearEyeDepth(i.vertex.z);
					half3 screenPos = i.foam_screenPos.yzw;
					half2 uvDepth = screenPos.xy / screenPos.z;
					float sceneZ01 = tex2D(_CameraDepthTexture, uvDepth).x;
					float sceneZ = LinearEyeDepth(sceneZ01);

					float3 lightDir = WorldSpaceLightDir(i.worldPos);
					// Soft shadow, hard shadow
					fixed2 shadow = (fixed2)1.0;

					// Normal - geom + normal mapping
					half3 n_geom = normalize(half3(i.n_shadow.x, 1., i.n_shadow.y));
					if (underwater) n_geom = -n_geom;
					half3 n_pixel = n_geom;

					n_pixel.xz += (underwater ? -1. : 1.) * SampleNormalMaps(i.lodAlpha_worldXZUndisplaced_oceanDepth.yz, i.lodAlpha_worldXZUndisplaced_oceanDepth.x);
					n_pixel = normalize(n_pixel);


					// Foam - underwater bubbles and whitefoam
					half3 bubbleCol = (half3)0.;

					half4 whiteFoamCol;
					ComputeFoam(i.foam_screenPos.x, i.lodAlpha_worldXZUndisplaced_oceanDepth.yz, i.worldPos.xz, n_pixel, pixelZ, sceneZ, view, lightDir, shadow.y, bubbleCol, whiteFoamCol);



					// Compute color of ocean - in-scattered light + refracted scene
					half3 scatterCol = ScatterColour(i.worldPos, i.lodAlpha_worldXZUndisplaced_oceanDepth.w, _WorldSpaceCameraPos, lightDir, view, shadow.x, underwater, true);

					half3 col = OceanEmission(view, n_pixel, lightDir, i.grabPos, pixelZ, uvDepth, sceneZ, sceneZ01, bubbleCol, _Normals, _CameraDepthTexture, underwater, scatterCol);

					// Light that reflects off water surface
					ApplyReflectionSky(view, n_pixel, lightDir, shadow.y, i.foam_screenPos.yzzw, col);

					// Override final result with white foam - bubbles on surface
	
					col = lerp(col, whiteFoamCol.rgb, whiteFoamCol.a);
					float diffz = sceneZ - pixelZ;
					float waveSpeed = 0;
					float wl = 100;
					float4 waveColor = tex2D(_WaveTex, float2(1 - max(diffz, 0.1) / wl + 0.273, 1));
					float depth1 = diffz;
					float depth2 = diffz * 0.9;
					half4 foam = tex2D(_Foam2Tex, i.uv * 0.2 - _Time.y * 0.05);
					float foam1 = foam.r * (1.0 - abs(depth2 * 2.0 - 1.0)) * 0.3;
					float foam2 = foam.g * (1 - abs(depth1 * 2.0 - 1.0)) * 0.2;
					col += pow(max(0, 1 - diffz) * (foam1 + foam2) * waveColor * 2, _FoamShinee);
					// Fog
					if (!underwater)
					{
						// above water - do atmospheric fog
						UNITY_APPLY_FOG(i.fogCoord, col);
					}
					else
					{
						// underwater - do depth fog
						col = lerp(col, scatterCol, 1. - exp(-_DepthFogDensity.xyz * pixelZ));
					}


					return half4(col, 1.);
				}

				ENDCG
			}
		}
	}
}
