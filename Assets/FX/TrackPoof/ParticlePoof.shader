Shader "Unlit/ParticlePoof"
{
    Properties
    {

        _ClippingThreshold ("Clipping Threshold" , Range(0,1)) = 0.1
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
                fixed4 texcoords : TEXCOORD0;
                fixed3 stream : TEXCOORD1;
                fixed4 color : COLOR;
            };

            struct v2f
            {

                half lifetime : TEXCOORD0;
                float4 vertex : SV_POSITION;
                 fixed4 color : COLOR;
            };

            half _ClippingThreshold;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex =  mul(UNITY_MATRIX_VP, v.vertex);
                o.lifetime = v.texcoords.x;
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                half erode = i.lifetime;
                half erosionAmt = step(i.color.w,erode);
                clip(_ClippingThreshold - erosionAmt);
                return i.color;
            }
            ENDCG
        }
    }
}
