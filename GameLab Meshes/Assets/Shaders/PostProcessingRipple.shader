Shader "Custom/PostProcessingRipple"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1, 1, 1, 1)
		_WaveDistance("WaveDistance", Range(20, 1000)) = 30
		_WaveSize("WaveSize", Range(0, 20)) = 10
		_WaveColor("WaveColor", Color) = (1, 1, 1, 1)
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
			sampler2D _CameraDepthTexture;
            float4 _MainTex_ST;
			float4 _Color;
			float _WaveDistance;
			float _WaveSize;
			float4 _WaveColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float depth;
                // sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				depth = tex2D(_CameraDepthTexture, i.uv).r;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);

				depth = Linear01Depth(depth) * _ProjectionParams.z;

				float waveFront = step(depth, _WaveDistance);
				float waveTrail = smoothstep(_WaveDistance - _WaveSize, _WaveDistance, depth);
				
				float wave = waveFront * waveTrail;

				col = lerp(col, _WaveColor, wave);
				return col;
            }
            ENDCG
        }
    }
}
