// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/PostProcessingRipple"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1, 1, 1, 1)
		_WorldSpaceScannerPos("ScannerPos", Vector) = (0, 0, 0)
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

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				float4 ray : TEXCOORD1;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
				float2 uv_depth : TEXCOORD1;
				float4 interpolatedRay : TEXCOORD2;
            };
			float4 _MainTex_TexelSize;


            sampler2D _MainTex;
			sampler2D _CameraDepthTexture;
            float4 _MainTex_ST;
			float4 _Color;
			float3 _WorldSpaceScannerPos;
			float _WaveDistance;
			float _WaveSize;
			float4 _WaveColor;

            v2f vert (appdata v)
            {
                v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv.xy;
				o.uv_depth = v.uv.xy;

				o.interpolatedRay = v.ray;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				float4 col = tex2D(_MainTex, i.uv);
				//depth = tex2D(_CameraDepthTexture, i.uv).r;

				//depth = Linear01Depth(depth) * _ProjectionParams.z;

				//float waveFront = step(depth, _WaveDistance);
				//float waveTrail = smoothstep(_WaveDistance - _WaveSize, _WaveDistance, depth);
				//
				//float wave = waveFront * waveTrail;

				//col = lerp(col, _WaveColor, wave);

				float rawDepth = DecodeFloatRG(tex2D(_CameraDepthTexture, i.uv_depth));
				float linearDepth = Linear01Depth(rawDepth) * _ProjectionParams.z;
				float4 wsDir = linearDepth * i.interpolatedRay;
				float3 wsPos = _WorldSpaceCameraPos + linearDepth; //Get world space position by adding the cam pos and wsDir togheter

				float dist = distance(wsPos, _WorldSpaceScannerPos);

				float4 scanCol = float4(0, 0, 0, 0);

				if (dist < _WaveDistance && dist > _WaveDistance - _WaveSize && linearDepth < _ProjectionParams.z) {
					//float waveFront = step(dist, _WaveDistance);
					//float waveTrail = smoothstep(_WaveDistance - _WaveSize, _WaveDistance, dist);
					//float wave = waveFront * waveTrail;
					float wave = 1 - (_WaveDistance - dist) / _WaveSize;

					scanCol = 1;// lerp(col, _WaveColor, wave);
				}

				return col + scanCol;
            }
            ENDCG
        }
    }
}