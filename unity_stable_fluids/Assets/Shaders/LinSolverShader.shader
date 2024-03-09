Shader "bluebean/StableFluids/LinSolverShader"
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
			sampler2D _Right;
			sampler2D _Left;
			float _a;
			float _c;
			float4 _texelSize;

			float4 frag(v2f i) : SV_Target
			{
				float2 vL = i.uv - float2(_texelSize.x,0);
				float2 vR = i.uv + float2(_texelSize.x, 0);
				float2 vT = i.uv + float2(0, _texelSize.y);
				float2 vB = i.uv - float2(0, _texelSize.y);

				float4 b = tex2D(_Right, i.uv);
				float4 left = tex2D(_Left, vL);
				float4 right = tex2D(_Left, vR);
				float4 top = tex2D(_Left, vT);
				float4 bottom = tex2D(_Left, vB);
				float4 col = (b + _a * (left + right + top + bottom)) / (_c);
			    return col;
		    }
		ENDCG
	}
	}
}
