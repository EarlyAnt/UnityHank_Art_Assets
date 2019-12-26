// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Shader created with Shader Forge v1.02 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.02;sub:START;pass:START;ps:flbk:,lico:1,lgpr:1,nrmq:1,limd:1,uamb:True,mssp:True,lmpd:False,lprd:False,rprd:False,enco:False,frtr:True,vitr:True,dbil:False,rmgx:True,rpth:0,hqsc:True,hqlp:False,tesm:0,blpr:2,bsrc:0,bdst:0,culm:2,dpts:2,wrdp:False,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,ofsf:0,ofsu:0,f2p0:False;n:type:ShaderForge.SFN_Final,id:5041,x:32719,y:32712,varname:node_5041,prsc:2|emission-1094-OUT;n:type:ShaderForge.SFN_Tex2d,id:3348,x:32051,y:32708,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:node_3348,prsc:2,tex:97b2f5496e54f43d4b127d62d986f1ca,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Color,id:9160,x:32025,y:32975,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_9160,prsc:2,glob:False,c1:1,c2:0.732353,c3:0.3308824,c4:1;n:type:ShaderForge.SFN_Slider,id:1357,x:32233,y:33069,ptovrint:False,ptlb:Strength_Color,ptin:_Strength_Color,varname:node_1357,prsc:2,min:0,cur:1.52004,max:3;n:type:ShaderForge.SFN_Multiply,id:1094,x:32431,y:32694,varname:node_1094,prsc:2|A-3348-RGB,B-9160-RGB,C-1357-OUT,D-3348-A;proporder:3348-9160-1357;pass:END;sub:END;*/

Shader "Shader Forge/Single" {
	Properties{
		_MainTex("MainTex", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
		_Strength_Color("Strength_Color", Range(0, 6)) = 3
	}
		SubShader{
			Tags {
				"IgnoreProjector" = "True"
				"Queue" = "Transparent+100"
				"RenderType" = "Transparent"
			}
			Pass {
				Name "ForwardBase"
				Tags {
					"LightMode" = "ForwardBase"
				}
				Blend One One
				Cull Off
				ZWrite Off

				Fog {Mode Off}
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#define UNITY_PASS_FORWARDBASE
				#include "UnityCG.cginc"
				#pragma multi_compile_fwdbase
				#pragma exclude_renderers xbox360 ps3 flash d3d11_9x 
				#pragma target 3.0
				uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
				uniform float4 _Color;
				uniform float _Strength_Color;
				struct VertexInput {
					float4 vertex : POSITION;
					float2 texcoord0 : TEXCOORD0;
					fixed4 color : COLOR;
				};
				struct VertexOutput {
					float4 pos : SV_POSITION;
					float2 uv0 : TEXCOORD0;
					fixed4 color : COLOR;
				};
				VertexOutput vert(VertexInput v) {
					VertexOutput o;
					o.uv0 = v.texcoord0;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.color = v.color;
					return o;
				}
				fixed4 frag(VertexOutput i) : COLOR {
					/////// Vectors:
					////// Lighting:
					////// Emissive:
									float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
									float3 emissive = (_MainTex_var.rgb*_Color.rgb*_Strength_Color*_MainTex_var.a*i.color.rgb);
									float3 finalColor = emissive;
									return fixed4(finalColor,1)*i.color.a;
								}
								ENDCG
							}
		}
			FallBack "XW/Mobile/Diffuse"
			CustomEditor "ShaderForgeMaterialInspector"
}
