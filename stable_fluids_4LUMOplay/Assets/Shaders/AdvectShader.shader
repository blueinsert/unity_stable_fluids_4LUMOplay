Shader "bluebean/StableFluids/AdvectShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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

            sampler2D _velocity;
            sampler2D _source;
            float _dt;
            float4 _texelSize;

            float4 frag(v2f i) : SV_Target
            {
                float2 texelSize = _texelSize.xy;

                //3rd order Runge-Kutta
                float2 v1 = tex2D(_velocity, i.uv).rg * texelSize;
                float2 p1 = i.uv - 0.5 * _dt * v1 ;
                float2 v2 = tex2D(_velocity, p1).rg * texelSize;
                float2 p2 = i.uv - 0.75 * _dt * v2;
                float2 v3 = tex2D(_velocity, p2).rg * texelSize;
                float2 p = i.uv - _dt * (v1 * 2.0 / 9.0 + v2 / 3.0 + v3 * 4.0 / 9.0);

                //p = i.uv - _dt * v1;

                float4 col = tex2D(_source, p);
                return col;
            }

            ENDCG
        }
    }
}
