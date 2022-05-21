Shader "Unlit/FlipBook_v3"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        [Toggle(Y_INVERSE)] _UseScroll("Y Inverse", float) = 0
        _ScrollSpeed ("Scroll Speed", float) = 1.0
        [ShowAsVector2Drawer] _Tile ("Tile",Vector) = (2.0,2.0,0.0,0.0)
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
            #pragma shader_feature Y_INVERSE

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _ScrollSpeed;
            float4 _Tile;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                i.uv.x *= 1/_Tile.x;
                i.uv.y *= 1/_Tile.y;

                #ifdef Y_INVERSE
                    i.uv.y += fmod(floor(-_Time.y * _ScrollSpeed),_Tile.y) / _Tile.y;
                #else
                    i.uv.y += fmod(floor(_Time.y * _ScrollSpeed),_Tile.y) / _Tile.y;
                #endif
                
                i.uv.x += fmod(floor(_Time.y * _ScrollSpeed * _Tile.x),_Tile.x) / _Tile.x;

                fixed4 col = tex2D(_MainTex, i.uv);
                
                return col;
            }
            ENDCG
        }
    }
}
