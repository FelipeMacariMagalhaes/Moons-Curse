Shader "Custom/VignetteJason"
{
    Properties
    {
        _Color("Color", Color) = (0,0,0,1)
        _Cutoff("Cutoff", Range(0,1)) = 0
        _Smoothness("Smoothness", Range(0.01, 1)) = 0.2
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Overlay" }
        LOD 100
        Cull Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            fixed4 _Color;
            float _Cutoff;
            float _Smoothness;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv * 2 - 1; // transforma 0-1 em -1 a 1
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float dist = length(i.uv);
                float alpha = smoothstep(_Cutoff, _Cutoff + _Smoothness, dist);
                return fixed4(_Color.rgb, alpha);
            }
            ENDCG
        }
    }
}