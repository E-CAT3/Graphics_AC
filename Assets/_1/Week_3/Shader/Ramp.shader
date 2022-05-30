Shader "Unlit/Ramp"
{
    Properties
    {
        _Ramp ("Ramp",2D) = "white" {}
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
            };

            sampler2D _Ramp;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = mul(unity_ObjectToWorld,v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz); // ライトの角度
                fixed4 col = tex2D(_Ramp, (dot(lightDir,i.normal)+1)/2); // rampテクスチャで色を付ける
                
                return col;
            }
            ENDCG
        }
    }
}
