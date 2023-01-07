Shader "PeerPlay/Underwater"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseScale("Noise Scale", float) = 1
        _NoiseFrequency("Noise Frequency", float) = 1
        _NoiseSpeed("Noise Speed", float) = 1
    }
        SubShader
        {
            // No culling or depth
            Cull Off ZWrite Off ZTest Always

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"
               
#include "noiseSimplex.cginc"

                uniform float _NoiseFrequency, _NoiseScale, _NoiseSpeed;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 scrPos : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.scrPos = ComputeScreenPos(o.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;

            fixed4 frag(v2f i) : COLOR
            {
                float3 spos = float3(i.scrPos.x, i.scrPos.y, 0) * _NoiseFrequency;
                spos.z += _Time.x * _NoiseSpeed;
                float noise = _NoiseScale * ((snoise(spos) + 1) / 2);
                fixed4 col = fixed4(noise,noise,noise,1);
                return col;
            }
            ENDCG
        }
    }
}
