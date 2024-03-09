Shader "bluebean/StableFluids/PressGradientSubtractShader"
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

			sampler2D _Velocity;
			sampler2D _Pressure;

			float4 _texelSize;

			float4 frag(v2f i) : SV_Target
			{
				float2 vL = i.uv - float2(_texelSize.x,0);
				float2 vR = i.uv + float2(_texelSize.x, 0);
				float2 vT = i.uv + float2(0, _texelSize.y);
				float2 vB = i.uv - float2(0, _texelSize.y);

				float left = tex2D(_Pressure, vL).r;
				float right = tex2D(_Pressure, vR).r;
				float top = tex2D(_Pressure, vT).r;
				float bottom = tex2D(_Pressure, vB).r;
				float2 velocity = tex2D(_Velocity, i.uv).rg;
				velocity.xy -= float2(right - left, top - bottom)*0.5;
				//velocity.xy -= float2(right - left, top - bottom);
				return float4(velocity, 0, 1);
		    }
		ENDCG
	}
	}
}
