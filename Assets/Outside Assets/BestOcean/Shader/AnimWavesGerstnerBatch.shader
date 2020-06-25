// A batch of Gerstner components
Shader "Ocean/Gerstner"
{
	Properties
	{
		_NumInBatch("_NumInBatch", float) = 0
	}

	Category
	{
		Tags{ "Queue" = "Transparent" }

		SubShader
		{
			Pass
			{
				Name "BASE"
				Tags { "LightMode" = "Always" }
				Blend One One
			
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_fog
				#include "UnityCG.cginc"

				#define TWOPI 6.283185

				struct appdata_t {
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
					half color : COLOR0;
				};

				struct v2f {
					float4 vertex : SV_POSITION;
					float3 worldPos_wt : TEXCOORD0;
					float2 uv : TEXCOORD1;
				};

				// IMPORTANT - this mirrors the constant with the same name in ShapeGerstnerBatched.cs, both must be updated together!
				#define BATCH_SIZE 32
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

	LOD_DATA(0)
	LOD_DATA(1)
				float2 LD_UVToWorld(in float2 i_uv, in float2 i_centerPos, in float i_res, in float i_texelSize)
				{
					return i_texelSize * i_res * (i_uv - 0.5) + i_centerPos;
				}
				float2 LD_0_UVToWorld(in float2 i_uv) { return LD_UVToWorld(i_uv, _LD_Pos_Scale_0.xy, _LD_Params_0.y, _LD_Params_0.x); }
				v2f vert( appdata_t v )
				{
					v2f o;
					o.vertex = float4(v.vertex.x, -v.vertex.y, 0., .5);

					float2 worldXZ = LD_0_UVToWorld(v.uv);

					o.worldPos_wt.xy = worldXZ;
					o.worldPos_wt.z = v.color.x;

					o.uv = v.uv;

					return o;
				}

				uniform float NowTime;
				uniform half _Chop;
				uniform half _Gravity;
				uniform half4 _Wavelengths[BATCH_SIZE / 4];
				uniform half4 _Amplitudes[BATCH_SIZE / 4];
				uniform half4 _Angles[BATCH_SIZE / 4];
				uniform half4 _Phases[BATCH_SIZE / 4];
				uniform half4 _ChopScales[BATCH_SIZE / 4];
				uniform half4 _GravityScales[BATCH_SIZE / 4];
				// how many samples we want in one wave. trade quality for perf.
				uniform float _TexelsPerWave;
				uniform float _MaxWavelength;
				uniform float _ViewerAltitudeLevelAlpha;
				uniform float _GridSize;

				float MinWavelengthForCurrentOrthoCamera()
				{
					return _GridSize * _TexelsPerWave;
				}


				float ComputeWaveSpeed(float wavelength, float g)
				{
					// wave speed of deep sea ocean waves: https://en.wikipedia.org/wiki/Wind_wave
					// https://en.wikipedia.org/wiki/Dispersion_(water_waves)#Wave_propagation_and_dispersion
					//float g = 9.81; float k = 2. * 3.141593 / wavelength; float cp = sqrt(g / k); return cp;
					const float one_over_2pi = 0.15915494;
					return sqrt(wavelength*g*one_over_2pi);
				}
				uniform float depthMax;
				half4 frag (v2f i) : SV_Target
				{
					const half minWavelength = MinWavelengthForCurrentOrthoCamera();
			
					// sample ocean depth (this render target should 1:1 match depth texture, so UVs are trivial)
					const half depth = depthMax - tex2D(_LD_Sampler_SeaFloorDepth_0, i.uv).x;
					half3 result = (half3)0.;

					// unrolling this loop once helped SM Issue Utilization and some other stats, but the GPU time is already very low so leaving this for now
					for (uint vi = 0; vi < BATCH_SIZE / 4; vi++)
					{
						[unroll]
						for (uint ei = 0; ei < 4; ei++)
						{
							if (_Wavelengths[vi][ei] == 0.)
							{
								return half4(i.worldPos_wt.z * result, 0.);
							}

							// weight
							half wt = 1;
							half depth_wt = saturate(depth / (1.5 * _Wavelengths[vi][ei])) * min(depth, 1);
							// leave a little bit - always keep 10% of amplitude
							wt *= .05 + .95 * depth_wt;

							// wave speed
							half C = ComputeWaveSpeed(_Wavelengths[vi][ei], _Gravity * _GravityScales[vi][ei]);
							// direction
							half2 D = half2(cos(_Angles[vi][ei]), sin(_Angles[vi][ei]));
							// wave number
							half k = TWOPI / _Wavelengths[vi][ei];
							// spatial location
							half x = dot(D, i.worldPos_wt.xy);

							half3 result_i = wt * _Amplitudes[vi][ei];
							result_i.y *= cos(k*(x + C * NowTime) + _Phases[vi][ei]);
							result_i.xz *= -_Chop * _ChopScales[vi][ei] * D * sin(k*(x + C * NowTime) + _Phases[vi][ei]);
							result += result_i;
						}
					}

					return half4(i.worldPos_wt.z * result, 0.);
				}

				ENDCG
			}
		}
	}
}
