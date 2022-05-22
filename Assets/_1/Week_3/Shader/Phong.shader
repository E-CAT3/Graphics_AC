Shader "Unlit/Phong"
{
    Properties
    {
        _Color ("Color",color) = (1,1,1,1)
        _ReflectionLevel("ReflectionLevel",float) = 1
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

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
                float4 worldPos : TEXCOORD0;
            };

            fixed4 _Color;
            float _ReflectionLevel;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = mul(unity_ObjectToWorld,v.normal);
                o.worldPos = mul(unity_ObjectToWorld,v.vertex); // ワールド座標へ変換
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz); // ディレクショナルライトの逆向き
                fixed3 lightCol = _LightColor0.xyz; // ディレクショナルライトの色
                float3 refVec = reflect(-lightDir, i.normal); // 反射角
                float3 toEye = normalize(_WorldSpaceCameraPos - i.worldPos); // 表面から視線へのベクトル
                float t = pow(max(0,dot(refVec, toEye)),_ReflectionLevel); 
                fixed4 col = _Color;
                col.xyz *= lightCol * t;
                
                return col;
              
            }
            ENDCG
        }
    }
    
    
}
