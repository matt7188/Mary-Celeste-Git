// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.04 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.04;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:2,uamb:True,mssp:True,lmpd:False,lprd:False,rprd:False,enco:True,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:0,bsrc:0,bdst:1,culm:0,dpts:2,wrdp:True,dith:2,ufog:True,aust:True,igpj:False,qofs:0,qpre:1,rntp:1,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:5679,x:32896,y:32728,varname:node_5679,prsc:2|diff-7104-RGB,normal-3392-RGB,transm-1129-RGB,lwrap-4965-OUT,amdfl-613-OUT;n:type:ShaderForge.SFN_Tex2d,id:7104,x:32503,y:32491,ptovrint:False,ptlb:Base (RGB),ptin:_BaseRGB,varname:node_7104,prsc:2,tex:addac62e84d7e2f45b199c04a56da9f6,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:3392,x:32125,y:32788,ptovrint:False,ptlb:Normal (RGB),ptin:_NormalRGB,varname:node_3392,prsc:2,tex:d01864cf6c35086498696e2458bab002,ntxv:3,isnm:True;n:type:ShaderForge.SFN_Cubemap,id:5142,x:31969,y:32976,ptovrint:False,ptlb:Diffuse Cubemap,ptin:_DiffuseCubemap,varname:node_5142,prsc:2,cube:ff4380305ddafb74aac8e800a05a2fd2,pvfc:0;n:type:ShaderForge.SFN_Tex2d,id:1129,x:32346,y:32645,ptovrint:False,ptlb:Transmission (G),ptin:_TransmissionG,varname:node_1129,prsc:2,tex:c676c3266643b1c4ca34b9acbd879363,ntxv:0,isnm:False;n:type:ShaderForge.SFN_ToggleProperty,id:989,x:32430,y:33132,ptovrint:False,ptlb:Use Diffuse Environment Light,ptin:_UseDiffuseEnvironmentLight,varname:node_989,prsc:2,on:True;n:type:ShaderForge.SFN_Vector1,id:8131,x:32430,y:33047,varname:node_8131,prsc:2,v1:0;n:type:ShaderForge.SFN_Lerp,id:613,x:32688,y:32983,cmnt:Toggle diff. cube,varname:node_613,prsc:2|A-8131-OUT,B-9866-OUT,T-989-OUT;n:type:ShaderForge.SFN_Add,id:7785,x:32289,y:33132,varname:node_7785,prsc:2|A-8523-OUT,B-8074-OUT;n:type:ShaderForge.SFN_Vector1,id:8074,x:32077,y:33155,varname:node_8074,prsc:2,v1:0.1;n:type:ShaderForge.SFN_Clamp01,id:9866,x:32491,y:33183,varname:node_9866,prsc:2|IN-7785-OUT;n:type:ShaderForge.SFN_Desaturate,id:8523,x:32168,y:32976,varname:node_8523,prsc:2|COL-5142-RGB;n:type:ShaderForge.SFN_Color,id:4881,x:32346,y:32863,ptovrint:False,ptlb:Transmission Color,ptin:_TransmissionColor,varname:node_4881,prsc:2,glob:False,c1:0.9960784,c2:0.4431373,c3:0,c4:1;n:type:ShaderForge.SFN_Multiply,id:4965,x:32615,y:32768,varname:node_4965,prsc:2|A-1129-RGB,B-4881-RGB;proporder:7104-3392-1129-4881-989-5142;pass:END;sub:END;*/

Shader "Shader Forge/transm" {
    Properties {
        _BaseRGB ("Base (RGB)", 2D) = "white" {}
        _NormalRGB ("Normal (RGB)", 2D) = "bump" {}
        _TransmissionG ("Transmission (G)", 2D) = "white" {}
        _TransmissionColor ("Transmission Color", Color) = (0.9960784,0.4431373,0,1)
        [MaterialToggle] _UseDiffuseEnvironmentLight ("Use Diffuse Environment Light", Float ) = 1
        _DiffuseCubemap ("Diffuse Cubemap", Cube) = "_Skybox" {}
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
            #pragma multi_compile_fwdbase_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _BaseRGB; uniform float4 _BaseRGB_ST;
            uniform sampler2D _NormalRGB; uniform float4 _NormalRGB_ST;
            uniform samplerCUBE _DiffuseCubemap;
            uniform sampler2D _TransmissionG; uniform float4 _TransmissionG_ST;
            uniform fixed _UseDiffuseEnvironmentLight;
            uniform float4 _TransmissionColor;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
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
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float4 _TransmissionG_var = tex2D(_TransmissionG,TRANSFORM_TEX(i.uv0, _TransmissionG));
                float3 w = (_TransmissionG_var.rgb*_TransmissionColor.rgb)*0.5; // Light wrapping
                float3 NdotLWrap = NdotL * ( 1.0 - w );
                float3 forwardLight = max(float3(0.0,0.0,0.0), NdotLWrap + w );
                float3 backLight = max(float3(0.0,0.0,0.0), -NdotLWrap + w ) * _TransmissionG_var.rgb;
                float3 indirectDiffuse = float3(0,0,0);
                float3 directDiffuse = (forwardLight+backLight)/(Pi*(dot(w,float3(0.3,0.59,0.11))+1)) * attenColor;
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float node_613 = lerp(0.0,saturate((dot(texCUBE(_DiffuseCubemap,viewReflectDirection).rgb,float3(0.3,0.59,0.11))+0.1)),_UseDiffuseEnvironmentLight); // Toggle diff. cube
                indirectDiffuse += float3(node_613,node_613,node_613); // Diffuse Ambient Light
                float4 _BaseRGB_var = tex2D(_BaseRGB,TRANSFORM_TEX(i.uv0, _BaseRGB));
                float3 diffuse = (directDiffuse + indirectDiffuse) * _BaseRGB_var.rgb;
/// Final Color:
                float3 finalColor = diffuse;
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
            #pragma multi_compile_fwdadd_fullshadows
            #pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _BaseRGB; uniform float4 _BaseRGB_ST;
            uniform sampler2D _NormalRGB; uniform float4 _NormalRGB_ST;
            uniform sampler2D _TransmissionG; uniform float4 _TransmissionG_ST;
            uniform float4 _TransmissionColor;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
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
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float Pi = 3.141592654;
                float InvPi = 0.31830988618;
/////// Diffuse:
                float NdotL = dot( normalDirection, lightDirection );
                float4 _TransmissionG_var = tex2D(_TransmissionG,TRANSFORM_TEX(i.uv0, _TransmissionG));
                float3 w = (_TransmissionG_var.rgb*_TransmissionColor.rgb)*0.5; // Light wrapping
                float3 NdotLWrap = NdotL * ( 1.0 - w );
                float3 forwardLight = max(float3(0.0,0.0,0.0), NdotLWrap + w );
                float3 backLight = max(float3(0.0,0.0,0.0), -NdotLWrap + w ) * _TransmissionG_var.rgb;
                float3 directDiffuse = (forwardLight+backLight)/(Pi*(dot(w,float3(0.3,0.59,0.11))+1)) * attenColor;
                float4 _BaseRGB_var = tex2D(_BaseRGB,TRANSFORM_TEX(i.uv0, _BaseRGB));
                float3 diffuse = directDiffuse * _BaseRGB_var.rgb;
/// Final Color:
                float3 finalColor = diffuse;
                return fixed4(finalColor * 1,0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
