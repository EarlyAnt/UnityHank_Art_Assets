Shader "Shader Forge/EffCartoon_simple"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		 _light_Color ("light_Color", Range(0.5, 5)) = 3
	}
	SubShader
	{
		Tags { "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent" }
		LOD 100

		Pass
		{
			Cull Off
            ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha //One One
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
			sampler2D _MainTex;
			float4 _MainTex_ST;
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.vertexColor = v.vertexColor;
				o.uv = v.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv, _MainTex));
				//return _MainTex_var;
				return fixed4(i.vertexColor.rgb*_MainTex_var.rgb*_light_Color,_MainTex_var.a*i.vertexColor.a);
			}
			ENDCG
		}
	}
}
