Shader "CAGraphicsAcademy/SpecularMapSample"
{
    Properties
    {
        _MainTex ("Main Tex", 2D) = "white" {}
        _SpecularTex ("Specular Tex", 2D) = "white" {}
        _SpecularLevel("Specular Level", Range(1, 100)) = 0
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
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1; 
                float3 worldPos : TEXCOORD2;    
            };

            sampler2D _MainTex;
            sampler2D _SpecularTex;
            float4 _MainTex_ST;
            float _SpecularLevel;

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex); //頂点をMVP行列変換
                o.normal = UnityObjectToWorldNormal(v.normal); //モデル座標系の法線をワールド座標系に変換
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz; //モデル座標系の頂点をワールド座標系に変換
                o.uv = TRANSFORM_TEX(v.uv, _MainTex); //テクスチャスケールとオフセットを加味
                return o;
            }

            float3 CalcLambertDiffuse(float3 normal); //ランバート拡散反射の計算はinline化し、フラグメントシェーダー外に配置
            float3 CalcPhongSpecular(float3 pos, float3 normal, float2 uv); //フォン鏡面反射の計算はinline化し、フラグメントシェーダー外に配置

            fixed4 frag (v2f i) : SV_Target
            {
                float3 diffuseLig = CalcLambertDiffuse(i.normal); //ランバート拡散反射を計算
                float3 specLig = CalcPhongSpecular(i.worldPos, i.normal, i.uv); //フォン鏡面反射を計算
                float3 lig = diffuseLig + specLig; //ランバートとフォンを加算したライティング値

                float4 finalColor = tex2D(_MainTex, i.uv); //メインテクスチャから色をサンプリング
                finalColor.rgb *= lig; //先に計算したライティング値を乗算

                return finalColor;
            }

            inline float3 CalcLambertDiffuse(float3 normal)
            {
                float3 ligDirection = normalize(_WorldSpaceLightPos0.xyz); //ディレクショナルライトの向きを取得
                fixed3 ligColor = _LightColor0.xyz; //ライトのカラーを取得
                return max(0.0f, dot(normal, ligDirection)) * ligColor; //内積から光が当たっているかどうかを計算する
            }

            inline float3 CalcPhongSpecular(float3 pos, float3 normal, float2 uv)
            {
                float3 ligDirection = normalize(_WorldSpaceLightPos0.xyz); //ディレクショナルライトの向きを取得
                fixed3 ligColor = _LightColor0.xyz; //ライトのカラーを取得

                float3 toEye = normalize(_WorldSpaceCameraPos - pos); //視線ベクトルを計算

                float3 refVec = reflect(-ligDirection, normal); //反射ベクトルを計算
                float t = max(0.0f, dot(refVec, toEye)); //反射ベクトルと視線ベクトル
                float mask = tex2D(_SpecularTex, uv).r; //スペキュラマップをサンプリングし、mask値を取得
                //mask = 1 - mask; //もしスペキュラマップの白黒が逆なら、maskを反転させるする

                float3 specularLig = ligColor * mask * pow(t, _SpecularLevel); //ライトカラー、マスク、スペキュラレベルで最終的なカラーを計算
                return specularLig;
            }
            ENDCG
        }
    }
}