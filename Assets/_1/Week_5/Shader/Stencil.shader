/*

Shader "Unlit/GrabShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            sampler2D _CameraOpaqueTexture; //追加
            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_CameraOpaqueTexture, i.uv);//追加
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
*/



Shader "Unlit/Stencil"
{
    Properties
    {
        _MixColor ("MixColor", Color) = (1,1,1,1)
        _ShiftLevel ("Shift", Range(0.0, 1.0)) = 0
        _RimLevel ("Rim", Range(0.0, 1.0)) = 0
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" }
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
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 screenPosition : TEXCOORD1;
                float3 nromal : TEXCOORD2;
                float3 worldPos : TEXCOORD3;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            // sampler2D _GrabPassTexture;
            // sampler2D _CameraOpaqueTexture;
            sampler2D _CameraOpaqueTexture; //追加
            float _ShiftLevel;
            float _RimLevel;
            float4 _MixColor; 
   
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.screenPosition = ComputeScreenPos(o.vertex);
                o.nromal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 toEye = normalize(_WorldSpaceCameraPos - i.worldPos);
                float rim = dot(i.nromal, toEye);
                rim = pow(rim,_RimLevel);

                // float4 color = tex2Dproj(_CameraOpaqueTexture, i.screenPosition);

                //リム強度が低いほどサンプリング位置をシフトさせない
                float4 color = tex2Dproj(_CameraOpaqueTexture, i.screenPosition + (1 - rim) * _ShiftLevel);
                color += _MixColor * rim;
                return color;
            }
            ENDCG
        }
    }
}
