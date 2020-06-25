Shader "Unlit/TestSahder"
{
	Properties
	{
		[HideInInspector]_MainTex ("Main Texture", 2D) = "white" {}
		[HideInInspector]_Color("KLUER", Color) = (1,1,1,1)
		_GradeR("Red Grading", Range(-1,1)) = 0
		_GradeG("Green Grading", Range(-1,1)) = 0
		_GradeB("Blue Grading", Range(-1,1)) = 0
		_SaturateR("Red Saturation", Range(0,2)) = 1
		_SaturateG("Green Saturation", Range(0,2)) = 1
		_SaturateB("Blue Saturation", Range(0,2)) = 1
		[Gamma]_Gamma("Gamma", Range(0,2)) = 1
		_DitherScale("Dither Scale", Range(0,2)) = 1
		_DitherRatio("Dither Ratio", range(0,128)) = 64
		_DitherAlpha("Dither Alpha", Range(0,2)) = 1
		_BlurFactor ("Blur Factor", Range(0, .1)) = 1.0
	}

    SubShader
    {
        //Tags { "RenderType"="Opaque" }
		//Tags {"Queue" = "Transparent"}
        LOD 100

        Pass
        {
            CGPROGRAM
			// Upgrade NOTE: excluded shader from DX11, OpenGL ES 2.0 because it uses unsized arrays
			//#pragma exclude_renderers d3d11 gles
			// Upgrade NOTE: excluded shader from DX11, OpenGL ES 2.0 because it uses unsized arrays
			//#pragma exclude_renderers d3d11 gles
            #pragma vertex vert
            #pragma fragment frag
			#pragma target 3.0

            #include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;

			};

            // note: no SV_POSITION in this struct
            struct v2f {
                float2 uv : TEXCOORD0;
            };

            v2f vert (
                float4 vertex : POSITION, // vertex position input
                float2 uv : TEXCOORD0, // texture coordinate input
                out float4 outpos : SV_POSITION // clip space position output
                )
            {
                v2f o;
                o.uv = uv;
                outpos = UnityObjectToClipPos(vertex);
                return o;
            }

			//Long line of variables getting made
            sampler2D _MainTex;
            float4 _MainTex_ST;
			float4 _Color; 
			float _GradeR;
			float _GradeG;
			float _GradeB;
			float _SaturateR;
			float _SaturateG;
			float _SaturateB;
			float _Gamma;
			float _DitherScale;
			int _DitherRatio;
			sampler2D _GrabTexture;
            float4 _GrabTexture_TexelSize;
            float _BlurFactor;
			float _DitherAlpha;
			//const int indexMatr[16];// = int[16](0,8,2,10,12,4,14,6,3,11,1,9,15,7,13,5);
			static float a[5] = {3.4, 4.2, 5.0, 5.2, 1.1};
			const int indexMatrix4x4[4][4] = {
				{0,  8,  2,  10},
				{12, 4,  14, 6},
				{3,  11, 1,  9},
				{15, 7,  13, 5}
			};

			//Dithers whatever float value
			float find_closest(int x, int y, float c0)
			{
				int dither[8][8] = {
				{ 0, 32, 8, 40, 2, 34, 10, 42}, /* 8x8 Bayer ordered dithering */
				{48, 16, 56, 24, 50, 18, 58, 26}, /* pattern. Each input pixel */
				{12, 44, 4, 36, 14, 46, 6, 38}, /* is scaled to the 0..63 range */
				{60, 28, 52, 20, 62, 30, 54, 22}, /* before looking in this table */
				{ 3, 35, 11, 43, 1, 33, 9, 41}, /* to determine the action. */
				{51, 19, 59, 27, 49, 17, 57, 25},
				{15, 47, 7, 39, 13, 45, 5, 37},
				{63, 31, 55, 23, 61, 29, 53, 21} }; 

				float limit = 0.0;
				if(x < 8)
				{
				limit = (dither[x][y]+1)/float(_DitherRatio);
				}
				if(c0 < limit)
				return 0.0;
				return 1.0;
			}
			
            fixed4 frag (v2f i, UNITY_VPOS_TYPE screenPos : VPOS) : SV_Target
            {
				fixed4 col = tex2D(_MainTex, i.uv);
				float4 EndColor = col;// = float4(col,col,col,1);
				EndColor.r = EndColor.r + _GradeR;
				EndColor.g = EndColor.g + _GradeG;
				EndColor.b = EndColor.b + _GradeB;
				EndColor.r = EndColor.r * _SaturateR;
				EndColor.g = EndColor.g * _SaturateG;
				EndColor.b = EndColor.b * _SaturateB;
				EndColor.rgb = EndColor.rgb * _Gamma;


				/*
				//screenPos.xy = floor(screenPos.xy * 0.25) * .5;
                float checker = -frac(screenPos.r + screenPos.g);

				if (checker<0){
					EndColor.r = screenPos.x;// EndColor.r * _SaturateR;
					
				}else{
					EndColor.b = screenPos.y;// EndColor.b * _SaturateB;
				}
				*/
				float4 lum = float4(0.299, 0.587, 0.114, 0);
				float grayscale = dot(col, lum);
				float3 rgb = tex2D(_MainTex, _ScreenParams.xy).rgb;


				//EndColor = grayscale;

				float2 ScalePos = _ScreenParams.xy * _DitherScale;
				int x = int(fmod(ScalePos.x, 8));
				int y = int(fmod(ScalePos.y, 8));

				float GreyFinal = find_closest(x, y, grayscale);
				//EndColor.r = find_closest(x, y, EndColor.r);
				//EndColor.g = find_closest(x, y, EndColor.g);
				//EndColor.b = find_closest(x, y, EndColor.b);
				float4 greh = float4(0, 0, 0, 0);

				float GreyyFinal;
				float grayyscale;
                //the fragment shader
				//calculate aspect ratio
				float invAspect = _ScreenParams.y / _ScreenParams.x;
				//iterate over blur samples
				for(float index = 0; index < 10; index++){
					//get uv coordinate of sample
					float2 uvx = i.uv + float2((index/9 - 0.5) * _BlurFactor * invAspect, 0);
					float2 uvy = i.uv + float2(0, (index/9 - 0.5) * _BlurFactor * invAspect);
					//uv.x = i.uvx + (index/9 - 0.5) * _Factor * invAspect;
					//uv.y = i.uvy + (index/9 - 0.5) * _Factor;
					//add color at position to color
					col = tex2D(_MainTex, uvx);
					grayyscale = dot(col, lum);
					GreyyFinal += find_closest(x, y, grayyscale);
					col = tex2D(_MainTex, uvy);
					grayyscale = dot(col, lum);
					GreyyFinal += find_closest(x, y, grayyscale);
					//greh.rg += GreyFinal * uv;

				}
				//divide the sum of values by the amount of samples
				GreyyFinal = GreyyFinal / 20;
				EndColor.rgb = EndColor.rgb + (EndColor.rgb * (GreyyFinal * _DitherAlpha));
				return EndColor;


				//EndColor.rgb = EndColor.rgb * GreyFinal;
				//return EndColor;

				/*
				if (grayscale>.45){
					EndColor.rgb = grayscale.r;
				}
				else{
					EndColor.rgb = GreyFinal.r;
				}
				//EndColor.rgb = EndColor.rgb * .9;

				*/

				/*
				int x = int(fmod(screenPos.x, 4));
				int y = int(fmod(screenPos.y, 4));
				int mat = indexMatrix4x4[(x + y * 4)] / 16.0;
				float closestColor = (col < 0.5) ? 0 : 1;
				float secondClosestColor = 1 - closestColor;
				float d = indexValue(mat);
				float distance = abs(closestColor - col);
				EndColor = (distance < d) ? closestColor : secondClosestColor;
				*/


				//EndColor = .5;
				//fixed
				//float3(dither(col),dither(col),dither(col));
				//EndColor = float4(float3(dither(col, screenPos),dither(col, screenPos),dither(col, screenPos)), 1);

				//fixed4 cool = tex2D(_MainTex, i.uv);
				//float4 cool = float4(i.uv.r,i.uv.g,1,1);
				//cool.rgb = 1 - cool.rgb;
				//float4 cool = float4(0,i.uv.g,i.uv.r,1);
				//return cool;
				//return float4(1,1,1,1);
                // sample the texture
                //fixed4 col = tex2D(_MainTex, i.uv);
				//fixed4 col = fixed4(1,1,1,1);
				//col.rgb = col.rgb * _Color.rgb; 
				


                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);


				
                
            }

            ENDCG
        }
    }
}
