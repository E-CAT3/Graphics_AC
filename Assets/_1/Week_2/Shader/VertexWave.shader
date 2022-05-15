Shader "Unlit/Vertexwave"
{
    Properties
    {
        _Color ("Main Color", Color) = (1,.5,.5,1)
        _Speed ("scroll", float) = 1
        _Hight ("hight", float) = 1
        _Period ("period", float) = 1
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
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            half4 _Color;
            float _Speed;
            float _Hight;
            float _Period;

            v2f vert (appdata v)
            {
                v2f o;
                v.vertex.y += sin(_Time.y * _Speed + v.vertex.x * _Period) * _Hight;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return _Color;
            }
            ENDCG
        }
    }
}
