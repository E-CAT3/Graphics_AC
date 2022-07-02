Shader "CAGraphicsAcademy/CubemapSample"
{
    Properties
    {
        _Cube("Cube", CUBE) = "" {} //キューブマップを使用するにはCUBEに設定する
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
                float3 normal : Normal;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float3 normal : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            UNITY_DECLARE_TEXCUBE(_Cube); //キューブマップとして使用することを宣言

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex); //頂点をMVP行列変換
                o.uv = TRANSFORM_TEX(v.uv, _MainTex); //テクスチャスケールとオフセットを加味
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz; //頂点座標をワールド座標系に変換
                o.normal = UnityObjectToWorldNormal(v.normal); //法線をワールド座標系に変換
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos); //視線ベクトルを計算
                float3 refDir = reflect(-viewDir, i.normal); //反射ベクトルを計算

                fixed4 col = UNITY_SAMPLE_TEXCUBE(_Cube, refDir); //キューブマップからサンプリング
                return col;
            }
            ENDCG
        }
    }
}