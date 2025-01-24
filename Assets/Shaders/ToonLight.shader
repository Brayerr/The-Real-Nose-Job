Shader "Unlit/ToonLight"
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
  

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
                float3 normal : NORMAL;
                
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL;
                float3 worldLightDir : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                float3 worldNormal = mul(v.normal.xyz,(float3x3)unity_WorldToObject);
                o.normal = worldNormal;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldLightDir = _WorldSpaceLightPos0;
               o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return dot(i.worldLightDir,i.normal);
                return fixed4(i.normal,1);
                fixed4 col = i.color;
                return col;
            }
            ENDCG
        }
    }
}
