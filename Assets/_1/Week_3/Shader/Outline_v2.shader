Shader "Unlit/Outline_v2"
{
    Properties
    {
        _Ramp ("Ramp",2D) = "white" {}
        _OutlineColor("Outline Color",Color) = (0,0,0,0)
        _OutlineThickness ("Outline Thickness",float) = 0.01
        [Toggle(CONSTANT_THICKNESS)] _UseScroll("Constant thickness from the screen", float) = 0
    }
    
    CGINCLUDE
    #include "UnityCG.cginc"
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
    ENDCG

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            // LightMode について
            // https://light11.hatenadiary.com/entry/2022/03/15/195620
            Tags { "LightMode" = "UniversalForward" }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

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
        
        Pass
        {
            Tags { "LightMode" = "SRPDefaultUnlit" } // デフォルト
            cull Front
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma shader_feature CONSTANT_THICKNESS

            fixed4 _OutlineColor;
            float _OutlineThickness;
            
            v2f vert (appdata v)
            {
                v2f o;
                float4 tmpProjectionPos = UnityObjectToClipPos(v.vertex);
                #ifdef CONSTANT_THICKNESS
                float4 extendVertex = v.vertex + float4(v.normal,0) * _OutlineThickness * UNITY_Z_0_FAR_FROM_CLIPSPACE(tmpProjectionPos.z);
                #else
                float4 extendVertex = v.vertex + float4(v.normal,0) * _OutlineThickness;
                #endif

                o.vertex = UnityObjectToClipPos(extendVertex);
                o.normal = float3(0,0,0); // ノーマルはもういらない
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDCG
        }
    }
}
