Shader "lym/reflect" {
	Properties {
		_Color("Color Tint",Color) = (1,1,1,1)
		_NoiseTex("Noise Tex", 2D) = "white" {}
		_Intensity("intensity", float) = 0.1
		_XSpeed("Flow Speed", float) = -0.2
	}
	SubShader {
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		Pass{
			Tags{ "LightMode" = "ForwardBase" }
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include"Lighting.cginc"

			fixed4 _Color;
			sampler2D _reflect;//渲染纹理
			sampler2D _NoiseTex;//噪声纹理
			float4 _NoiseTex_ST;
			uniform float _Intensity;
			uniform float _XSpeed;

			struct a2v {
				float4 vertex:POSITION;
				float4 uv:TEXCOORD0;
			};
			struct v2f {
				float4 pos:SV_POSITION;
				float2 uv:TEXCOORD0; 
				float2 screenuv:TEXCOORD1;
			};
			v2f vert(a2v i) {
				v2f o;
				o.pos = UnityObjectToClipPos(i.vertex);
				o.uv = TRANSFORM_TEX(i.uv, _NoiseTex);
				o.screenuv = ((o.pos.xy/o.pos.w)+1)*0.5;
				return o;
			}
			fixed4 frag(v2f i):SV_Target{
				fixed4 noise_col = tex2D(_NoiseTex, i.screenuv + fixed2(_Time.y*_XSpeed, 0));
				fixed uOffset = noise_col.g;
				fixed vOffset = noise_col.g;
				fixed4 col = tex2D(_reflect, i.screenuv + _Intensity*fixed2(uOffset, vOffset));
				return col*_Color;
			}
			ENDCG
		}
	}
	FallBack "Transparent/VertexLit"
}