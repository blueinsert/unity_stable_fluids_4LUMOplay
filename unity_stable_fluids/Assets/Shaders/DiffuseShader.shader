Shader "bluebean/StableFluids/DiffuseShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
		SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

	struct v2f
	{
		float2 uv : TEXCOORD0;
		float4 vertex : SV_POSITION;
	};

	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = v.uv;
		return o;
	}

			sampler2D _MainTex;
			float4 _texelSize;
			sampler2D _Source;
			float _a;

			float4 frag(v2f i) : SV_Target
			{
				//_resolution.x = 512;
			//_resolution.y = 512;
				float2 vL = i.uv - float2(_texelSize.x,0);
				float2 vR = i.uv + float2(_texelSize.x, 0);
				float2 vT = i.uv + float2(0, _texelSize.y);
				float2 vB = i.uv - float2(0, _texelSize.y);
				//float4 c = tex2Dlod(_MainTex, float4(i.uv,0,0));
				float4 c = tex2D(_Source,i.uv);
				//explicit time intergration for diffuse
				float4 l = tex2D(_Source, vL);
				float4 r = tex2D(_Source, vR);
				float4 t = tex2D(_Source, vT);
				float4 b = tex2D(_Source, vB);
				//float4 result = (1.0 - 4.0 * _a) * c + _a * (l + r + t + b);
				float4 result = (c + (l + r + t + b) * _a) / (1 + 4.0 * _a);
				//result = float4(_resolution.x, 0, 0, 1);
                return result;
		}
		ENDCG
	}
	}
}
