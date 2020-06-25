
// Persistent foam sim

Shader "Hidden/Ocean/Simulation/Update Foam"
{
	Properties {
	}

	Category
	{
		// Base simulation runs first on geometry queue, no blending.
		// Any interactions will additively render later in the transparent queue.
		Tags { "Queue" = "Geometry" }

		SubShader {
			Pass {

				Name "BASE"
				Tags{ "LightMode" = "Always" }

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_fog

				#include "UnityCG.cginc"

				struct appdata_t {
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f {
					float4 vertex : SV_POSITION;
					float4 uv_uv_lastframe : TEXCOORD0;
					float2 worldXZ : TEXCOORD1;
				};
#define LOD_DATA(LODNUM) \
	uniform sampler2D _LD_Sampler_AnimatedWaves_##LODNUM; \
	uniform sampler2D _LD_Sampler_SeaFloorDepth_##LODNUM; \
	uniform sampler2D _LD_Sampler_Foam_##LODNUM; \
	uniform sampler2D _LD_Sampler_Flow_##LODNUM; \
	uniform sampler2D _LD_Sampler_DynamicWaves_##LODNUM; \
	uniform sampler2D _LD_Sampler_Shadow_##LODNUM; \
	uniform float4 _LD_Params_##LODNUM; \
	uniform float3 _LD_Pos_Scale_##LODNUM;
				LOD_DATA(0)
					LOD_DATA(1)
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
				
				// It seems that unity_DeltaTime.x is always >= 0.005! So Crest adds its own dts
				uniform float _SimDeltaTime;
				uniform float _SimDeltaTimePrev;

				// Compute current uv, and uv for the last frame to allow a sim to move around in the world but keep
				// its data stationary, without smudged or blurred data.
				void ComputeUVs(in float2 worldXZ, in float2 vertexXY, out float2 uv_lastframe, out float2 uv)
				{
					// uv for target data - always simply 0-1 so take from geometry
					uv = vertexXY;
					uv.y = -uv.y;
					uv.xy = 0.5*uv.xy + 0.5;

					// uv for source data - use bound data to compute
					uv_lastframe = LD_0_WorldToUV(worldXZ);
				}

				
				v2f vert(appdata_t v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);

					// lod data 1 is current frame, compute world pos from quad uv
					o.worldXZ = LD_1_UVToWorld(v.uv);

					ComputeUVs(o.worldXZ, o.vertex.xy, o.uv_uv_lastframe.zw, o.uv_uv_lastframe.xy);

					return o;
				}

				// respects the gui option to freeze time
				uniform half _FoamFadeRate;
				uniform half _WaveFoamStrength;
				uniform half _WaveFoamCoverage;
				uniform half _ShorelineFoamMaxDepth;
				uniform half _ShorelineFoamStrength;
				uniform float depthMax;
				half frag(v2f i) : SV_Target
				{
					float4 uv = float4(i.uv_uv_lastframe.xy, 0., 0.);
					float4 uv_lastframe = float4(i.uv_uv_lastframe.zw, 0., 0.);
					// #if _FLOW_ON
					half4 velocity = half4(tex2Dlod(_LD_Sampler_Flow_1, uv).xy, 0., 0.);
					half foam = tex2Dlod(_LD_Sampler_Foam_0, uv_lastframe
						- ((_SimDeltaTime * _LD_Params_0.w) * velocity)
					).x;
					// #else
					// // sampler will clamp the uv currently
					// half foam = tex2Dlod(_LD_Sampler_Foam_0, uv_lastframe).x;
					// #endif
					half2 r = abs(uv_lastframe.xy - 0.5);
					if (max(r.x, r.y) > 0.5 - _LD_Params_0.w)
					{
						// no border wrap mode for RTs in unity it seems, so make any off-texture reads 0 manually
						foam = 0.;
					}

					// fade
					foam *= max(0.0, 1.0 - _FoamFadeRate * _SimDeltaTime);

					// sample displacement texture and generate foam from it
					const float3 dd = float3(_LD_Params_1.w, 0.0, _LD_Params_1.x);
					half3 s = tex2Dlod(_LD_Sampler_AnimatedWaves_1, uv).xyz;
					half3 sx = tex2Dlod(_LD_Sampler_AnimatedWaves_1, uv + dd.xyyy).xyz;
					half3 sz = tex2Dlod(_LD_Sampler_AnimatedWaves_1, uv + dd.yxyy).xyz;
					float3 disp = s.xyz;
					float3 disp_x = dd.zyy + sx.xyz;
					float3 disp_z = dd.yyz + sz.xyz;
					// The determinant of the displacement Jacobian is a good measure for turbulence:
					// > 1: Stretch
					// < 1: Squash
					// < 0: Overlap
					float4 du = float4(disp_x.xz, disp_z.xz) - disp.xzxz;
					float det = (du.x * du.w - du.y * du.z) / (_LD_Params_1.x * _LD_Params_1.x);
					foam += 5. * _SimDeltaTime * _WaveFoamStrength * saturate(_WaveFoamCoverage - det);

					// add foam in shallow water. use the displaced position to ensure we add foam where world objects are.
					float4 uv_1_displaced = float4(LD_1_WorldToUV(i.worldXZ + disp.xz), 0., 1.);
					float signedOceanDepth = depthMax - tex2Dlod(_LD_Sampler_SeaFloorDepth_1, uv_1_displaced).x + disp.y;
					foam += _ShorelineFoamStrength * _SimDeltaTime * saturate(1. - signedOceanDepth / _ShorelineFoamMaxDepth);

					return foam;
				}
				ENDCG
			}
		}
	}
}
