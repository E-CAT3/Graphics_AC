Shader "Unlit/NormalMap"
{
    Properties
    {
        _SpecularColor ("SpecularColor",color) = (1,1,1,1)
        _SpecularMaskMap ("SpecularColor",2D) = "white" {}
        _SpecularIntencity ("SpecularIntencity",float) = 1
        
        _DiffuseColor ("DiffuseColor",color) = (1,1,1,1)
        _ReflectionLevel("ReflectionLevel",float) = 1
        
        [Normal] _NormalMap ("Normalmap",2D) = "bump" {}

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
                half3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                half4 tangent : TANGENT;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                half3 normal : TEXCOORD2;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                half3 tangent : TEXCOORD3;
                half3 binormal : TEXCOORD4;
            };

            fixed4 _SpecularColor;
            sampler2D _SpecularMaskMap;
            float4 _SpecularMaskMap_ST;
            float _SpecularIntencity;
            fixed4 _DiffuseColor;
            float _ReflectionLevel;
            sampler2D _NormalMap;

            inline fixed4 CalcPhone(float3 lightDir, float3 normal, float2 uv, float3 worldPos, fixed3 lightCol)
            {
                float3 toEye = normalize(_WorldSpaceCameraPos - worldPos);
                float3 refVec = reflect(-lightDir, normal); // 反射角
                float t = pow(max(0,dot(refVec, toEye)),_ReflectionLevel);
                fixed4 specColor = tex2D(_SpecularMaskMap,uv).r * _SpecularColor;
                specColor.rgb *= lightCol * t * _SpecularIntencity;
                return specColor;
            }

            inline fixed4 CalcDiffuse(float3 lightDir, float3 normal, fixed3 lightCol)
            {
                fixed4 diffuseColor = max(0,dot(lightDir,normal));
                diffuseColor *= fixed4(lightCol,1) * _DiffuseColor;
                return diffuseColor;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                // o.normal = mul(unity_ObjectToWorld,v.normal);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.worldPos = mul(unity_ObjectToWorld,v.vertex).xyz; // ワールド座標へ変換
                o.uv = TRANSFORM_TEX(v.uv, _SpecularMaskMap);;
                o.tangent = normalize(mul(unity_ObjectToWorld,v.tangent));
                o.binormal = cross(o.normal,o.tangent);
                o.binormal = normalize(mul(unity_ObjectToWorld,o.binormal));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz); // ディレクショナルライトの逆向き
                fixed3 lightCol = _LightColor0.xyz; // ディレクショナルライトの色

                // float3 refVec = reflect(-lightDir, i.normal); // 反射角
                // float3 aN = dot(_WorldSpaceLightPos0.xyz,i.normal);
                // float3 refVec = -_WorldSpaceLightPos0.xyz + 2*aN*i.normal; // 反射角reflect未使用

                half3 normalmap = UnpackNormal(tex2D(_NormalMap,i.uv));
                float3 normal = (i.tangent * normalmap.x) + (i.binormal * normalmap.y) + (i.normal * normalmap.z);

                fixed4 specularColor = CalcPhone(lightDir, normal, i.uv, i.worldPos, lightCol);
                fixed4 diffuseColor = CalcDiffuse(lightDir, normal, lightCol);
                
                fixed4 col = diffuseColor + specularColor;
                return col;
            }
            
            ENDCG
        }
    }
    
    
}
