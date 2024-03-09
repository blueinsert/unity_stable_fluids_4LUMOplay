Shader "bluebean/StableFluids/SunrayMaskShader"
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

			float4 frag(v2f i) : SV_Target
			{
				float4 c = tex2D(_Source, i.uv);
				float br = max(c.r, max(c.g, c.b));
				float a = 1.0 - min(max(br*20.0, 0.0), 0.8);
				return float4(a, 0.0, 0.0, 1.0);
		    }
		ENDCG
	}
	}
}
