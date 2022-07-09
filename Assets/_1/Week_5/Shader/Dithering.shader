Shader "Unlit/Dithering"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DitherLevel ( "DitherLevel", int) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
                float4 screenPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            int _DitherLevel;

            static const int pattern[16] = { // staticがないと読み込めないらしい
                 0,  8,  2, 10,
                12,  4, 14,  6,
                 3, 11,  1,  9,
                15,  7, 13,  5
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 viewPortPos = i.screenPos.xy / i.screenPos.w; // wで除算し、スクリーンでの投影座標を取得
                float2 screenPosInPixel = viewPortPos.xy * _ScreenParams.xy; // 0～1をスクリーンピクセルに変換

                // ディザリング用UV作成
                int ditherUV_x = (int)fmod(screenPosInPixel.x, 4.0f); // パターンサイズが4x4なので4で割る
                int ditherUV_y = (int)fmod(screenPosInPixel.y, 4.0f); // パターンサイズが4x4なので4で割る
                float dither = pattern[ditherUV_x + ditherUV_y + 4];
                clip(dither - _DitherLevel);
                float4 color = tex2D(_MainTex,i.uv);
                return color;
            }
            ENDCG
        }
    }
}
