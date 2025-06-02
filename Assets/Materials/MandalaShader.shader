Shader "Custom/MandalaShader"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (0.2, 0.4, 0.8, 1)
        _AccentColor ("Accent Color", Color) = (0.3, 0.5, 0.9, 0.7)
        _PatternIntensity ("Pattern Intensity", Range(0,5)) = 1
        _PatternScale ("Pattern Scale", Range(0,10)) = 3
        _PatternSpeed ("Pattern Speed", Range(0,5)) = 1
        _GlowIntensity ("Glow Intensity", Range(0,1)) = 0.5
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Lighting Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            fixed4 _BaseColor;
            fixed4 _AccentColor;
            float _PatternIntensity;
            float _PatternScale;
            float _PatternSpeed;
            float _GlowIntensity;
            float4 _Time; // Built-in Unity variable

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.vertex.xy;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float r = length(i.uv);
                float angle = atan2(i.uv.y, i.uv.x);

                float t = _Time.y;
                float pattern = sin(_PatternScale * angle + _PatternSpeed * t);
                pattern *= cos(_PatternScale * r - _PatternSpeed * t);
                pattern *= _PatternIntensity;

                fixed4 color = lerp(_BaseColor, _AccentColor, pattern);
                color.a *= _GlowIntensity + 0.5;
                return color;
            }
            ENDCG
        }
    }
    FallBack "Unlit/Color"
}