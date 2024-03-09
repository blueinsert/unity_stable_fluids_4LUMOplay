Shader "bluebean/StableFluids/DivergenceShader"
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
			float4 _texelSize;

			float4 frag(v2f i) : SV_Target
			{
				float2 vL = i.uv - float2(_texelSize.x,0);
				float2 vR = i.uv + float2(_texelSize.x, 0);
				float2 vT = i.uv + float2(0, _texelSize.y);
				float2 vB = i.uv - float2(0, _texelSize.y);

				float2 center = tex2D(_Source, i.uv).xy;
				float left = tex2D(_Source, vL).x;
				float right = tex2D(_Source, vR).x;
				float top = tex2D(_Source, vT).y;
				float bottom = tex2D(_Source, vB).y;
				if (vL.x < 0)
					left = -center.x;
				if (vR.x > 1.0)
					right = -center.x;
				if (vT.y > 1.0)
					top = -center.y;
				if (vB.y < 0)
					bottom = -center.y;

				/*if (vL.x < 0)
					left = 0;
				if (vR.x > 1.0)
					right = 0;
				if (vT.y > 1.0)
					top = 0;
				if (vB.y < 0)
					bottom = 0;*/
				/*if (vL.x < 0)
					left = center.x;
				if (vR.x > 1.0)
					right = center.x;
				if (vT.y > 1.0)
					top = center.y;
				if (vB.y < 0)
					bottom = center.y;*/
				float div = 0.5 * (right - left + top - bottom);
				float4 col = float4(div, 0, 0, 1.0);
			    return col;
		    }
		ENDCG
	}
	}
}
