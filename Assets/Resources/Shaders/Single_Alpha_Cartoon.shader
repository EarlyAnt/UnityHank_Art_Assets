Shader "Shader Forge/Single_Alpha_Cartoon"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		//_Mask("Mask",2D)="white"{}
		_light_Color ("light_Color", Range(0.5, 5)) = 1.538462
		_dark_Color ("dark_Color", Range(0, 1)) = 0.2831191
	}
	SubShader
	{
		Tags { "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent" }
		Pass
		{
			Cull Off
            ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 vertexColor : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 vertexColor : COLOR;
			};

			uniform float _light_Color;
			uniform float _dark_Color;
			sampler2D _MainTex;//,_Mask;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.vertexColor = v.vertexColor;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				//float4 _Texture_main = tex2D(_MainTex,i.uv);
				//float4 _Texture_mask = tex2D(_Mask,i.uv);
				float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv, _MainTex));
				//return _MainTex_var;
				return fixed4(i.vertexColor.rgb*_MainTex_var.rgb,_MainTex_var.a*i.vertexColor.a);
				
			}
			ENDCG
		}
	}
}
