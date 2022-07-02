Shader "Unlit/CubeMap"
{
    Properties
    {
        _CubeMap ("CubeMap",Cube) = ""{}
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
            // #include "Lighting.cginc"
            // #include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normal : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };

            UNITY_DECLARE_TEXCUBE(_CubeMap);

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                // o.normal = mul(unity_ObjectToWorld,v.normal);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld,v.vertex).xyz; // ワールド座標へ変換
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // float3 lightDir = normalize(_WorldSpaceLightPos0.xyz); // ディレクショナルライトの逆向き
                float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos); //視線ベクトルを計算
                float3 refVec = reflect(-viewDir, i.normal); // 反射角
            
                fixed4 col = UNITY_SAMPLE_TEXCUBE(_CubeMap, refVec); //キューブマップからサンプリング
                return col;
            }
            ENDCG
        }
    }
    
    
}
