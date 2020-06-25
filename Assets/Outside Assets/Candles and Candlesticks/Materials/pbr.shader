// Upgrade NOTE: commented out 'float4 unity_LightmapST', a built-in variable
// Upgrade NOTE: commented out 'sampler2D unity_Lightmap', a built-in variable
// Upgrade NOTE: commented out 'sampler2D unity_LightmapInd', a built-in variable
// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'
// Upgrade NOTE: replaced tex2D unity_Lightmap with UNITY_SAMPLE_TEX2D
// Upgrade NOTE: replaced tex2D unity_LightmapInd with UNITY_SAMPLE_TEX2D_SAMPLER

// Shader created with Shader Forge v1.04 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.04;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:3,uamb:True,mssp:True,lmpd:True,lprd:False,rprd:False,enco:False,frtr:True,vitr:True,dbil:True,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:1,culm:0,dpts:2,wrdp:True,dith:2,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:2772,x:33520,y:32641,varname:node_2772,prsc:2|diff-7253-OUT,spec-9207-OUT,gloss-3651-A,normal-3260-RGB,amdfl-3705-RGB,amspl-6773-OUT,difocc-2100-OUT,spcocc-2100-OUT;n:type:ShaderForge.SFN_Tex2d,id:8892,x:32326,y:32914,ptovrint:False,ptlb:Color (RGB) Rough (A),ptin:_ColorRGBRoughA,varname:node_8892,prsc:2,tex:f98240d04c8800f4b8aae2fdcd0597cf,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:3260,x:33235,y:33116,ptovrint:False,ptlb:Normal (RGB),ptin:_NormalRGB,varname:node_3260,prsc:2,tex:51ade82bca2082141b5371ee2421e34e,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Vector1,id:8857,x:32643,y:32377,varname:node_8857,prsc:2,v1:0.2;n:type:ShaderForge.SFN_ViewReflectionVector,id:4011,x:32227,y:32455,varname:node_4011,prsc:2;n:type:ShaderForge.SFN_Cubemap,id:3705,x:32438,y:32414,ptovrint:False,ptlb:Diffuse Cubemap,ptin:_DiffuseCubemap,varname:node_3705,prsc:2,cube:93af84dcb71e742809730565a16757d0,pvfc:0|DIR-4011-OUT;n:type:ShaderForge.SFN_Multiply,id:4722,x:32880,y:32501,cmnt:Add diff cube to base,varname:node_4722,prsc:2|A-8857-OUT,B-3705-RGB;n:type:ShaderForge.SFN_Add,id:9207,x:33196,y:32554,varname:node_9207,prsc:2|A-4722-OUT,B-4081-OUT;n:type:ShaderForge.SFN_Cubemap,id:2651,x:32438,y:32586,ptovrint:False,ptlb:Specular Cubemap,ptin:_SpecularCubemap,varname:node_2651,prsc:2,cube:a6088d78c0dbf4657b57dcb25a795fcb,pvfc:0|DIR-4011-OUT,MIP-6692-OUT;n:type:ShaderForge.SFN_Tex2d,id:3651,x:32016,y:32768,ptovrint:False,ptlb:Metal (R) AO (B) Spec (G) Rough (A),ptin:_MetalRAOBSpecGRoughA,varname:node_3651,prsc:2,tex:924fb9b92efb2584cad9a1a30280785a,ntxv:1,isnm:False;n:type:ShaderForge.SFN_Lerp,id:7253,x:32984,y:32686,cmnt:Use more or less color for diff based on metalness,varname:node_7253,prsc:2|A-8892-RGB,B-1798-OUT,T-3651-R;n:type:ShaderForge.SFN_Lerp,id:4081,x:33098,y:32886,cmnt:Desaturate spec based on metalness,varname:node_4081,prsc:2|A-8316-OUT,B-8892-RGB,T-3651-R;n:type:ShaderForge.SFN_Desaturate,id:8316,x:32857,y:32976,varname:node_8316,prsc:2|COL-8892-RGB;n:type:ShaderForge.SFN_Slider,id:9447,x:31973,y:33151,ptovrint:False,ptlb:AO Influence,ptin:_AOInfluence,varname:node_9447,prsc:2,min:0.3,cur:0.3,max:1;n:type:ShaderForge.SFN_Lerp,id:5733,x:32428,y:33099,varname:node_5733,prsc:2|A-7544-OUT,B-3651-G,T-9447-OUT;n:type:ShaderForge.SFN_Vector1,id:7544,x:32119,y:33066,cmnt:No AO,varname:node_7544,prsc:2,v1:1;n:type:ShaderForge.SFN_Lerp,id:6773,x:32710,y:32846,cmnt:Blur specular based on roughness,varname:node_6773,prsc:2|A-2651-RGB,B-3705-RGB,T-3651-A;n:type:ShaderForge.SFN_SwitchProperty,id:1325,x:32636,y:33099,ptovrint:False,ptlb:Use AO Map,ptin:_UseAOMap,varname:node_1325,prsc:2,on:True|A-7544-OUT,B-5733-OUT;n:type:ShaderForge.SFN_ConstantClamp,id:2100,x:32979,y:33085,varname:node_2100,prsc:2,min:0.5,max:1|IN-1325-OUT;n:type:ShaderForge.SFN_OneMinus,id:1798,x:32678,y:32708,varname:node_1798,prsc:2|IN-3651-R;n:type:ShaderForge.SFN_Lerp,id:6692,x:32199,y:32613,cmnt:mip spec,varname:node_6692,prsc:2|A-1562-OUT,B-8274-OUT,T-3651-A;n:type:ShaderForge.SFN_Vector1,id:1562,x:31915,y:32583,varname:node_1562,prsc:2,v1:0;n:type:ShaderForge.SFN_ValueProperty,id:8274,x:31915,y:32670,ptovrint:False,ptlb:Max Specular Mip Level,ptin:_MaxSpecularMipLevel,varname:node_8274,prsc:2,glob:False,v1:6;proporder:8892-3260-3651-1325-9447-3705-2651-8274;pass:END;sub:END;*/

Shader "Shader Forge/pbr" {
    Properties {
        _ColorRGBRoughA ("Color (RGB) Rough (A)", 2D) = "white" {}
        _NormalRGB ("Normal (RGB)", 2D) = "bump" {}
        _MetalRAOBSpecGRoughA ("Metal (R) AO (B) Spec (G) Rough (A)", 2D) = "gray" {}
        [MaterialToggle] _UseAOMap ("Use AO Map", Float ) = 1
        _AOInfluence ("AO Influence", Range(0.3, 1)) = 0.3
        _DiffuseCubemap ("Diffuse Cubemap", Cube) = "_Skybox" {}
        _SpecularCubemap ("Specular Cubemap", Cube) = "_Skybox" {}
        _MaxSpecularMipLevel ("Max Specular Mip Level", Float ) = 6
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "ForwardBase"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            #pragma glsl
            #ifndef LIGHTMAP_OFF
                // float4 unity_LightmapST;
                // sampler2D unity_Lightmap;
                #ifndef DIRLIGHTMAP_OFF
                    // sampler2D unity_LightmapInd;
                #endif
            #endif
            uniform sampler2D _ColorRGBRoughA; uniform float4 _ColorRGBRoughA_ST;
            uniform sampler2D _NormalRGB; uniform float4 _NormalRGB_ST;
            uniform samplerCUBE _DiffuseCubemap;
            uniform samplerCUBE _SpecularCubemap;
            uniform sampler2D _MetalRAOBSpecGRoughA; uniform float4 _MetalRAOBSpecGRoughA_ST;
            uniform float _AOInfluence;
            uniform fixed _UseAOMap;
            uniform float _MaxSpecularMipLevel;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 binormalDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                #ifndef LIGHTMAP_OFF
                    float2 uvLM : TEXCOORD7;
                #endif
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(unity_ObjectToWorld, float4(v.normal,0)).xyz;
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex);
                #ifndef LIGHTMAP_OFF
                    o.uvLM = v.texcoord1 * unity_LightmapST.xy + unity_LightmapST.zw;
                #endif
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _NormalRGB_var = UnpackNormal(tex2D(_NormalRGB,TRANSFORM_TEX(i.uv0, _NormalRGB)));
                float3 normalLocal = _NormalRGB_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                #ifndef LIGHTMAP_OFF
                    float4 lmtex = UNITY_SAMPLE_TEX2D(unity_Lightmap,i.uvLM);
                    #ifndef DIRLIGHTMAP_OFF
                        float3 lightmap = DecodeLightmap(lmtex);
                        float3 scalePerBasisVector = DecodeLightmap(UNITY_SAMPLE_TEX2D_SAMPLER(unity_LightmapInd,unity_Lightmap,i.uvLM));
                        UNITY_DIRBASIS
                        half3 normalInRnmBasis = saturate (mul (unity_DirBasis, normalLocal));
                        lightmap *= dot (normalInRnmBasis, scalePerBasisVector);
                    #else
                        float3 lightmap = DecodeLightmap(lmtex);
                    #endif
                #endif
                #ifndef LIGHTMAP_OFF
                    #ifdef DIRLIGHTMAP_OFF
                        float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                    #else
                        float3 lightDirection = normalize (scalePerBasisVector.x * unity_DirBasis[0] + scalePerBasisVector.y * unity_DirBasis[1] + scalePerBasisVector.z * unity_DirBasis[2]);
                        lightDirection = mul(lightDirection,tangentTransform); // Tangent to world
                    #endif
                #else
                    float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                #endif
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i)*2;
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float4 _MetalRAOBSpecGRoughA_var = tex2D(_MetalRAOBSpecGRoughA,TRANSFORM_TEX(i.uv0, _MetalRAOBSpecGRoughA));
                float gloss = _MetalRAOBSpecGRoughA_var.a;
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float node_7544 = 1.0; // No AO
                float node_2100 = clamp(lerp( node_7544, lerp(node_7544,_MetalRAOBSpecGRoughA_var.g,_AOInfluence), _UseAOMap ),0.5,1);
                float3 specularAO = float3(node_2100,node_2100,node_2100);
                float4 _DiffuseCubemap_var = texCUBE(_DiffuseCubemap,viewReflectDirection);
                float4 _ColorRGBRoughA_var = tex2D(_ColorRGBRoughA,TRANSFORM_TEX(i.uv0, _ColorRGBRoughA));
                float node_8316 = dot(_ColorRGBRoughA_var.rgb,float3(0.3,0.59,0.11));
                float3 specularColor = ((0.2*_DiffuseCubemap_var.rgb)+lerp(float3(node_8316,node_8316,node_8316),_ColorRGBRoughA_var.rgb,_MetalRAOBSpecGRoughA_var.r));
                float specularMonochrome = dot(specularColor,float3(0.3,0.59,0.11));
                float HdotL = max(0.0,dot(halfDirection,lightDirection));
                float3 fresnelTerm = specularColor + ( 1.0 - specularColor ) * pow((1.0 - HdotL),5);
                float NdotV = max(0.0,dot( normalDirection, viewDirection ));
                float3 fresnelTermAmb = specularColor + ( 1.0 - specularColor ) * ( pow((1.0 - NdotV),5) / (4-3*gloss) );
                float alpha = 1.0 / ( sqrt( (Pi/4.0) * specPow + Pi/2.0 ) );
                float visTerm = ( NdotL * ( 1.0 - alpha ) + alpha ) * ( NdotV * ( 1.0 - alpha ) + alpha );
                visTerm = 1.0 / visTerm;
                float normTerm = (specPow + 8.0 ) / (8.0 * Pi);
                #if !defined(LIGHTMAP_OFF) && defined(DIRLIGHTMAP_OFF)
                    float3 directSpecular = float3(0,0,0);
                #else
                    float3 directSpecular = 1 * pow(max(0,dot(halfDirection,normalDirection)),specPow)*fresnelTerm*visTerm*normTerm;
                #endif
                float3 indirectSpecular = (0 + lerp(texCUBElod(_SpecularCubemap,float4(viewReflectDirection,lerp(0.0,_MaxSpecularMipLevel,_MetalRAOBSpecGRoughA_var.a))).rgb,_DiffuseCubemap_var.rgb,_MetalRAOBSpecGRoughA_var.a)) * specularAO * fresnelTermAmb;
                float3 specular = (directSpecular + indirectSpecular) * specularColor;
                #ifndef LIGHTMAP_OFF
                    #ifndef DIRLIGHTMAP_OFF
                        specular *= lightmap;
                    #else
                        specular *= (floor(attenuation) * _LightColor0.xyz);
                    #endif
                #else
                    specular *= (floor(attenuation) * _LightColor0.xyz);
                #endif
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 indirectDiffuse = float3(0,0,0);
                #ifndef LIGHTMAP_OFF
                    float3 directDiffuse = float3(0,0,0);
                #else
                    float3 directDiffuse = max( 0.0, NdotL)*InvPi * attenColor;
                #endif
                #ifndef LIGHTMAP_OFF
                    #ifdef SHADOWS_SCREEN
                        #if (defined(SHADER_API_GLES) || defined(SHADER_API_GLES3)) && defined(SHADER_API_MOBILE)
                            directDiffuse += min(lightmap.rgb, attenuation);
                        #else
                            directDiffuse += max(min(lightmap.rgb,attenuation*lmtex.rgb), lightmap.rgb*attenuation*0.5);
                        #endif
                    #else
                        directDiffuse += lightmap.rgb;
                    #endif
                #endif
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb*2; // Ambient Light
                indirectDiffuse += _DiffuseCubemap_var.rgb; // Diffuse Ambient Light
                indirectDiffuse *= float3(node_2100,node_2100,node_2100); // Diffuse AO
                float node_1798 = (1.0 - _MetalRAOBSpecGRoughA_var.r);
                float3 diffuse = (directDiffuse + indirectDiffuse) * lerp(_ColorRGBRoughA_var.rgb,float3(node_1798,node_1798,node_1798),_MetalRAOBSpecGRoughA_var.r);
                diffuse *= 1-specularMonochrome;
/// Final Color:
                float3 finalColor = diffuse + specular;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
        Pass {
            Name "ForwardAdd"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            Fog { Color (0,0,0,0) }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #include "Lighting.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            #pragma glsl
            uniform sampler2D _ColorRGBRoughA; uniform float4 _ColorRGBRoughA_ST;
            uniform sampler2D _NormalRGB; uniform float4 _NormalRGB_ST;
            uniform samplerCUBE _DiffuseCubemap;
            uniform sampler2D _MetalRAOBSpecGRoughA; uniform float4 _MetalRAOBSpecGRoughA_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 binormalDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = mul(unity_ObjectToWorld, float4(v.normal,0)).xyz;
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.binormalDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.binormalDir, i.normalDir);
/////// Vectors:
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 _NormalRGB_var = UnpackNormal(tex2D(_NormalRGB,TRANSFORM_TEX(i.uv0, _NormalRGB)));
                float3 normalLocal = _NormalRGB_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform )); // Perturbed normals
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i)*2;
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
///////// Gloss:
                float4 _MetalRAOBSpecGRoughA_var = tex2D(_MetalRAOBSpecGRoughA,TRANSFORM_TEX(i.uv0, _MetalRAOBSpecGRoughA));
                float gloss = _MetalRAOBSpecGRoughA_var.a;
                float specPow = exp2( gloss * 10.0+1.0);
////// Specular:
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float4 _DiffuseCubemap_var = texCUBE(_DiffuseCubemap,viewReflectDirection);
                float4 _ColorRGBRoughA_var = tex2D(_ColorRGBRoughA,TRANSFORM_TEX(i.uv0, _ColorRGBRoughA));
                float node_8316 = dot(_ColorRGBRoughA_var.rgb,float3(0.3,0.59,0.11));
                float3 specularColor = ((0.2*_DiffuseCubemap_var.rgb)+lerp(float3(node_8316,node_8316,node_8316),_ColorRGBRoughA_var.rgb,_MetalRAOBSpecGRoughA_var.r));
                float specularMonochrome = dot(specularColor,float3(0.3,0.59,0.11));
                float HdotL = max(0.0,dot(halfDirection,lightDirection));
                float3 fresnelTerm = specularColor + ( 1.0 - specularColor ) * pow((1.0 - HdotL),5);
                float NdotV = max(0.0,dot( normalDirection, viewDirection ));
                float alpha = 1.0 / ( sqrt( (Pi/4.0) * specPow + Pi/2.0 ) );
                float visTerm = ( NdotL * ( 1.0 - alpha ) + alpha ) * ( NdotV * ( 1.0 - alpha ) + alpha );
                visTerm = 1.0 / visTerm;
                float normTerm = (specPow + 8.0 ) / (8.0 * Pi);
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*fresnelTerm*visTerm*normTerm;
                float3 specular = directSpecular * specularColor;
/////// Diffuse:
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL)*InvPi * attenColor;
                float node_1798 = (1.0 - _MetalRAOBSpecGRoughA_var.r);
                float3 diffuse = directDiffuse * lerp(_ColorRGBRoughA_var.rgb,float3(node_1798,node_1798,node_1798),_MetalRAOBSpecGRoughA_var.r);
                diffuse *= 1-specularMonochrome;
/// Final Color:
                float3 finalColor = diffuse + specular;
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
