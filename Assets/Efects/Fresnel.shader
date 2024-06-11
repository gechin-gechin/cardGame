Shader "Unlit/Fresnel"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Color",Color)=(1,1,1,1)
        _Power("Power",Range(0.1,5))=1
        _SpecularPow ("Speclar Pow", float) = 5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderType"="Transparent" "RenderPipeline"="UniversalRenderPipeline" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

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
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float4 vertex : SV_POSITION;
                float3 viewDir : TEXCOORD5;
                float3 lightDir : TEXCOORD6;
            };

            sampler2D _MainTex;
            float4 _Color;
            float _Power;

            half4 _LightColor0;
            float _SpecularPow;

            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal=normalize(v.normal);

                float3 worldPos=mul(unity_ObjectToWorld,v.vertex).xyz;
                o.viewDir=normalize(UnityWorldSpaceViewDir(worldPos));

                float isDirectional = step(1, _WorldSpaceLightPos0.w);
                o.lightDir = normalize(_WorldSpaceLightPos0.xyz - (worldPos.xyz * isDirectional));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                col *= UNITY_LIGHTMODEL_AMBIENT.rgba;
                float3 diffuse = saturate(dot(i.normal, i.lightDir)) * _LightColor0;

                float3 reflectVec = reflect(-i.lightDir, i.normal);
                float specular = pow(saturate(dot(reflectVec, i.viewDir)), _SpecularPow);
                col*=fixed4((diffuse+specular),1.0);

                float fresnel=pow(1.0-saturate(dot(i.normal,i.viewDir)),_Power);
                float3 fcol = lerp(col.rgb,_Color.rgb,fresnel);
                col = fixed4(fcol,fresnel);
                return col;
            }
            ENDCG
        }
    }
}
