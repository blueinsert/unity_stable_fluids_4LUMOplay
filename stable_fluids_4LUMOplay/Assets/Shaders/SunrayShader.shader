Shader "bluebean/StableFluids/SunrayShader"
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

			sampler2D _Source;
			float _weight;

#define ITERATIONS 16

			float4 frag(v2f i) : SV_Target
			{
				float Density = 0.3;
		        float Decay = 0.95;
				float Exposure = 0.7;

				float2 coord = i.uv;
				float2 dir = i.uv - 0.5;

				dir *= 1.0 / float(ITERATIONS) * Density;
				float illuminationDecay = 1.0;

				float color = tex2D(_Source, i.uv).r;

				for (int i = 0; i < ITERATIONS; i++)
				{
					coord -= dir;
					float col = tex2D(_Source, coord).r;
					color += col * illuminationDecay * _weight;
					illuminationDecay *= Decay;
				}

				float4 c = float4(color * Exposure, 0.0, 0.0, 1.0);
				return c;
		    }
		ENDCG
	}
	}
}
