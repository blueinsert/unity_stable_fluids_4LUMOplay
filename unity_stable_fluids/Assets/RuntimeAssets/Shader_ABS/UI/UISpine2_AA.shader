// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "ProjectL/UISpine2_AA"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255

		_ColorMask ("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}
		
		Stencil
		{
			Ref [_Stencil]
			Comp [_StencilComp]
			Pass [_StencilOp] 
			ReadMask [_StencilReadMask]
			WriteMask [_StencilWriteMask]
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest [unity_GUIZTestMode]
		Blend One OneMinusSrcAlpha
		ColorMask [_ColorMask]

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			#include "UnityUI.cginc"

			#pragma multi_compile __ UNITY_UI_ALPHACLIP
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				half2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
			};
			
			fixed4 _Color;
			fixed4 _TextureSampleAdd;
			float4 _ClipRect;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.worldPosition = IN.vertex;
				OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);
				OUT.texcoord = IN.texcoord;						
				OUT.color = IN.color * _Color;
				return OUT;
			}

			sampler2D _MainTex;
			float4 _MainTex_TexelSize;
						
			float4 AASampleTexture(sampler2D tex, float2 texcoord, float2 offsetMax)
			{
				// 计算周围4个点的UV偏移，0.25是实验值
				float2 offset = fwidth(texcoord) * 0.25;
				
				// 限制UV偏移，避免取样到其他部位的颜色
				offset = clamp(offset, 0, offsetMax);
				
				// 取周围四个点的颜色并计算平均值
				float4 color1 = tex2D(tex, texcoord + float2(-offset.x, -offset.y));
				float4 color2 = tex2D(tex, texcoord + float2(offset.x, -offset.y));
				float4 color3 = tex2D(tex, texcoord + float2(-offset.x, offset.y));
				float4 color4 = tex2D(tex, texcoord + float2(offset.x, offset.y));
				
				return (color1 + color2 + color3 + color4) / 4;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				//half4 color = tex2D(_MainTex, IN.texcoord);
				half4 color = AASampleTexture(_MainTex, IN.texcoord, _MainTex_TexelSize.xy * 2);
				
				color.rgb *= color.a;
				color = (color + _TextureSampleAdd) * IN.color;
				color.rgba *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
				
				#ifdef UNITY_UI_ALPHACLIP
				clip (color.a - 0.001);
				#endif

				return color;
			}
		ENDCG
		}
	}
}
