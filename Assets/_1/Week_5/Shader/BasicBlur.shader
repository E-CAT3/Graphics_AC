Shader "Unlit/BasicBlur"
{
    Properties
    {
        [HideInInspector] _MainTex ("Texture", 2D) = "white" {}
        [HideInInspector] _BlurLevel ( "BlurLevel", float) = 1
    }
    SubShader
    {
        Cull Off
        ZTest Always
        ZWrite Off
        
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
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _BlurLevel;
            float2 _Resolution;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float offsetU = _BlurLevel / _Resolution.x;
                float offsetV = _BlurLevel / _Resolution.y;

                float4 color = tex2D(_MainTex,i.uv);

                color += tex2D(_MainTex,i.uv + float2(-offsetU, offsetV));  // 左上
                color += tex2D(_MainTex,i.uv + float2(0.0, offsetV));       // 上
                color += tex2D(_MainTex,i.uv + float2(offsetU, offsetV));   // 右上

                color += tex2D(_MainTex,i.uv + float2(0.0, -offsetV));      // 左
                color += tex2D(_MainTex,i.uv + float2(0.0, offsetV));       // 右

                color += tex2D(_MainTex,i.uv + float2(-offsetU, -offsetV)); // 左下
                color += tex2D(_MainTex,i.uv + float2(0.0, -offsetV));      //　下
                color += tex2D(_MainTex,i.uv + float2(offsetU, -offsetV));  //　右下

                return color / 9.0f;
            }
            ENDCG
        }
    }
}
