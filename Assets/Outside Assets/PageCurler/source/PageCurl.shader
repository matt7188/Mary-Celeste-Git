Shader "Custom/PageCurl"
{
	Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Theta ("Theta", Range(0,90)) = 0.0
		_Apex ("Apex", float) = 0.0
		_SinTheta ("SinTheta", float) = 1.0
		_CosTheta ("CosTheta", float) = 1.0
		_ScaleX ("Scale X", float) = 1.0
		_ScaleY ("Scale Y", float) = 1.0
		_InvertSign ("Invert Sign", float) = 1.0
		_ConeSide ("Cone Side Direction", float) = 1.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		Offset -1, -1
		
		CGPROGRAM
		#pragma surface surf Lambert vertex:vert

		sampler2D _MainTex;
		fixed4 _Color;
		float _Theta;
		float _Apex;
		float _SinTheta;
		float _CosTheta;
		float _ScaleX;
		float _ScaleY;
		float _InvertSign;
		float _ConeSide;
		
		struct Input {
			float2 uv_MainTex;
		};

		float3 projectVertexInCone(float2 pPos, float sinTheta, float cosTheta, float apex)
		{
			float secondTerm = pPos.y - apex;
			float R = sqrt(pPos.x*pPos.x + secondTerm*secondTerm);
			float r = R * sinTheta;
			float beta = asin(pPos.x/R) / sinTheta;

			float3 vertexProj;
			vertexProj.x = r*sin(beta);
			vertexProj.z = R + apex - r*(1- cos(beta))*sinTheta;
			vertexProj.y = _ConeSide*r*(1-cos(beta))*cosTheta;

			return vertexProj;
		}

		void vert (inout appdata_full v, out Input o)
		{
			float offset = step(0, -_InvertSign);
			float2 curVertex = float2(offset + _InvertSign*v.texcoord.x, v.texcoord.y);		
			curVertex *= 10 * float2(_ScaleX, _ScaleY);
			
			float2 xNeighbour = curVertex + float2(0.05, 0.0);
			float2 zNeighbour = curVertex + float2(0.0, 0.05);

			float3 v0 = projectVertexInCone(curVertex, _SinTheta, _CosTheta, _Apex);
			
			v.vertex.xyz = v0;
			v.vertex.x *= 1/_ScaleX;	//It will be scaled again by Unity
			v.vertex.z *= 1/_ScaleY;
			//Project a couple of neighbouring points so that we can reconstruct the normal
			float3 xProj = projectVertexInCone(xNeighbour, _SinTheta, _CosTheta, _Apex);
			float3 zProj = projectVertexInCone(zNeighbour, _SinTheta, _CosTheta, _Apex);
			float3 v10 = xProj - v0;
			float3 v20 = zProj - v0;
			float3 vNormal = cross(v20, v10);
			v.normal = normalize(vNormal)*_InvertSign;
			o.uv_MainTex = v.texcoord.xy;
		}

		void surf (Input IN, inout SurfaceOutput o) 
		{
			//Basic diffuse shading
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
