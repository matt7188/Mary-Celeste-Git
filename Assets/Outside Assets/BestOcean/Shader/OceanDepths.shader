

// Renders ocean depth - signed distance from sea level to sea floor
Shader "Ocean/Ocean Depth From Geometry"
{
	Properties
	{
	}

	Category
	{
		Tags { "Queue" = "Geometry" }

		SubShader
		{
			Pass
			{
				Name "BASE"
				Tags { "LightMode" = "Always" }
				BlendOp Max

				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_fog
				#include "UnityCG.cginc"
		
				struct appdata_t {
					float4 vertex : POSITION;
				};

				struct v2f {
					float4 vertex : SV_POSITION;
					float depth : TEXCOORD0;
				};
				uniform float3 _OceanCenterPosWorld;
				uniform float depthMax;
				v2f vert( appdata_t v )
				{
					v2f o;
					o.vertex = UnityObjectToClipPos( v.vertex );

					float altitude = mul(unity_ObjectToWorld, v.vertex).y;

					// Depth is altitude above 1000m below sea level. This is because '0' needs to signify deep water.
					// I originally used a simple bias in the depth texture but it would still produce shallow water outside
					// the biggest LOD texture where the depth would evaluate to 0 in the ocean vert shader, so i've transformed
					// 0 to mean deep below the surface.
					o.depth = altitude - (_OceanCenterPosWorld.y - depthMax);
					return o;
				}

				float frag (v2f i) : SV_Target
				{
					return i.depth;
				}

				ENDCG
			}
		}
	}
}
