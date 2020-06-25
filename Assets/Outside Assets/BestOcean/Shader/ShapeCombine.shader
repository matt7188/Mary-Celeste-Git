Shader "Hidden/Ocean/Combine Animated Wave LODs"
{
	Properties
	{
	}

	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#pragma multi_compile __ _DYNAMIC_WAVE_SIM_ON
			#pragma multi_compile __ _FLOW_ON

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
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
			uniform float _HorizDisplace;
			uniform float _DisplaceClamp;
			uniform float NowTime;

			void Flow(out float2 offsets, out float2 weights)
			{
				const float period = 3. * _LD_Params_0.x;
				const float half_period = period / 2.;
				offsets = fmod(float2(NowTime, NowTime + half_period), period);
				weights.x = offsets.x / half_period;
				if (weights.x > 1.0) weights.x = 2.0 - weights.x;
				weights.y = 1.0 - weights.x;
			}
			float2 LD_UVToWorld(in float2 i_uv, in float2 i_centerPos, in float i_res, in float i_texelSize)
			{
				return i_texelSize * i_res * (i_uv - 0.5) + i_centerPos;
			}
			float2 LD_WorldToUV(in float2 i_samplePos, in float2 i_centerPos, in float i_res, in float i_texelSize)
			{
				return (i_samplePos - i_centerPos) / (i_texelSize * i_res) + 0.5;
			}
			float2 LD_0_UVToWorld(in float2 i_uv) { return LD_UVToWorld(i_uv, _LD_Pos_Scale_0.xy, _LD_Params_0.y, _LD_Params_0.x); }
			float2 LD_1_WorldToUV(in float2 i_samplePos) { return LD_WorldToUV(i_samplePos, _LD_Pos_Scale_1.xy, _LD_Params_1.y, _LD_Params_1.x); }
			void SampleFlow(in sampler2D i_oceanFlowSampler, float2 i_uv, in float i_wt, inout half2 io_flow)
			{
				const float4 uv = float4(i_uv, 0., 0.);
				io_flow += i_wt * tex2Dlod(i_oceanFlowSampler, uv).xy;
			}
			void SampleDisplacements(in sampler2D i_dispSampler, in float2 i_uv, in float i_wt, inout float3 io_worldPos)
			{
				const half3 disp = tex2Dlod(i_dispSampler, float4(i_uv, 0., 0.)).xyz;
				io_worldPos += i_wt * disp;
			}
			half4 frag (v2f i) : SV_Target
			{
				// go from uv out to world for the current shape texture
				const float2 worldPosXZ = LD_0_UVToWorld(i.uv);

				// sample the shape 1 texture at this world pos
				const float2 uv_1 = LD_1_WorldToUV(worldPosXZ);

				float2 flow = 0.;
				SampleFlow(_LD_Sampler_Flow_0, i.uv, 1., flow);

				float3 result = 0.;

				// this lods waves
#if _FLOW_ON
				float2 offsets, weights;
				Flow(offsets, weights);

				float2 uv_0_flow_0 = LD_0_WorldToUV(worldPosXZ - offsets[0] * flow);
				float2 uv_0_flow_1 = LD_0_WorldToUV(worldPosXZ - offsets[1] * flow);
				SampleDisplacements(_LD_Sampler_AnimatedWaves_0, uv_0_flow_0, weights[0], result);
				SampleDisplacements(_LD_Sampler_AnimatedWaves_0, uv_0_flow_1, weights[1], result);
#else
				SampleDisplacements(_LD_Sampler_AnimatedWaves_0, i.uv, 1.0, result);
#endif

				// waves to combine down from the next lod up the chain
				SampleDisplacements(_LD_Sampler_AnimatedWaves_1, uv_1, 1.0, result);

				// TODO - uncomment this define once it works in standalone builds
#if _DYNAMIC_WAVE_SIM_ON
				{
					// convert dynamic wave sim to displacements

					half waveSimY = tex2Dlod(_LD_Sampler_DynamicWaves_0, float4(i.uv, 0., 0.)).x;
					result.y += waveSimY;

					// compute displacement from gradient of water surface - discussed in issue #18 and then in issue #47
					const float2 invRes = float2(_LD_Params_0.w, 0.);
					const half waveSimY_px = tex2Dlod(_LD_Sampler_DynamicWaves_0, float4(i.uv + invRes.xy, 0., 0.)).x;
					const half waveSimY_nx = tex2Dlod(_LD_Sampler_DynamicWaves_0, float4(i.uv - invRes.xy, 0., 0.)).x;
					const half waveSimY_pz = tex2Dlod(_LD_Sampler_DynamicWaves_0, float4(i.uv + invRes.yx, 0., 0.)).x;
					const half waveSimY_nz = tex2Dlod(_LD_Sampler_DynamicWaves_0, float4(i.uv - invRes.yx, 0., 0.)).x;

					float2 dispXZ = _HorizDisplace * (float2(waveSimY_px, waveSimY_pz) - float2(waveSimY_nx, waveSimY_nz)) / (2. * _LD_Params_0.x);

					const float maxDisp = _LD_Params_0.x * _DisplaceClamp;
					dispXZ = clamp(dispXZ, -maxDisp, maxDisp);

					result.xz += dispXZ;
				}
#endif // _DYNAMIC_WAVE_SIM_

				return half4(result, 1.);
			}
			ENDCG
		}
	}
}
