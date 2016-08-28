Shader "Hidden/AuraOverlay" {

	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}

	SubShader {
		Pass {
      Blend One Zero

			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				uniform sampler2D _MainTex;

        struct appdata {
          float4 vertex : POSITION;
          float4 uv : TEXCOORD0;
        };

        struct v2f {
          float4 vertex : POSITION;
          float2 uv : TEXCOORD0;

          float4 vtf0_1 : TEXCOORD1;
          float4 vtf2_3 : TEXCOORD2;
          float4 vtf4_5 : TEXCOORD3;
          float4 vtf6_7 : TEXCOORD4;
          float4 vtf8_9 : TEXCOORD5;
          float4 vtf10_11 : TEXCOORD6;
          float4 vtf12_13 : TEXCOORD7;
        };

        v2f vert (appdata v) {
          v2f o;
          o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
          o.uv = v.uv;

          float blurSize = 0.4;

          o.vtf0_1   = float4(o.uv + blurSize * float2(-0.028, 0.0), o.uv + blurSize * float2(-0.024, 0.0));
			    o.vtf2_3   = float4(o.uv + blurSize * float2(-0.020, 0.0), o.uv + blurSize * float2(-0.016, 0.0));
			    o.vtf4_5   = float4(o.uv + blurSize * float2(-0.012, 0.0), o.uv + blurSize * float2(-0.008, 0.0));
			    o.vtf6_7   = float4(o.uv + blurSize * float2(-0.004, 0.0), o.uv + blurSize * float2( 0.004, 0.0));
			    o.vtf8_9   = float4(o.uv + blurSize * float2( 0.008, 0.0), o.uv + blurSize * float2( 0.012, 0.0));
			    o.vtf10_11 = float4(o.uv + blurSize * float2( 0.016, 0.0), o.uv + blurSize * float2( 0.020, 0.0));
			    o.vtf12_13 = float4(o.uv + blurSize * float2( 0.024, 0.0), o.uv + blurSize * float2( 0.028, 0.0));
          return o;
        }

				fixed4 frag(v2f i) : SV_Target {
					half4 t = tex2D(_MainTex, i.uv);
					if(t.a == 1)
					{
						return t;
					}

					half4 c = half4(0, 0, 0, t.a);
			    c.rgb += (tex2D(_MainTex, i.vtf0_1.xy) * 0.0044299121055113265).rgb;
			    c.rgb += (tex2D(_MainTex, i.vtf0_1.zw) * 0.00895781211794).rgb;
			    c.rgb += (tex2D(_MainTex, i.vtf2_3.xy) * 0.0215963866053).rgb;
			    c.rgb += (tex2D(_MainTex, i.vtf2_3.zw) * 0.0443683338718).rgb;
			    c.rgb += (tex2D(_MainTex, i.vtf4_5.xy) * 0.0776744219933).rgb;
			    c.rgb += (tex2D(_MainTex, i.vtf4_5.zw) * 0.115876621105).rgb;
			    c.rgb += (tex2D(_MainTex, i.vtf6_7.xy) * 0.147308056121).rgb;
			    c.rgb += (t * 0.159576912161).rgb;
			    c.rgb += (tex2D(_MainTex, i.vtf6_7.zw) * 0.147308056121).rgb;
			    c.rgb += (tex2D(_MainTex, i.vtf8_9.xy) * 0.115876621105).rgb;
			    c.rgb += (tex2D(_MainTex, i.vtf8_9.zw) * 0.0776744219933).rgb;
			    c.rgb += (tex2D(_MainTex, i.vtf10_11.xy) * 0.0443683338718).rgb;
			    c.rgb += (tex2D(_MainTex, i.vtf10_11.zw) * 0.0215963866053).rgb;
			    c.rgb += (tex2D(_MainTex, i.vtf12_13.xy) * 0.00895781211794).rgb;
			    c.rgb += (tex2D(_MainTex, i.vtf12_13.zw) * 0.0044299121055113265).rgb;

					return half4(c.rgba);
				}
			ENDCG
		}

		Pass {
      Blend OneMinusDstColor One
      // Blend One One

			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"

				uniform sampler2D _MainTex;

        struct appdata {
          float4 vertex : POSITION;
          float4 uv : TEXCOORD0;
        };

        struct v2f {
          float4 vertex : POSITION;
          float2 uv : TEXCOORD0;

          float4 vtf0_1 : TEXCOORD1;
          float4 vtf2_3 : TEXCOORD2;
          float4 vtf4_5 : TEXCOORD3;
          float4 vtf6_7 : TEXCOORD4;
          float4 vtf8_9 : TEXCOORD5;
          float4 vtf10_11 : TEXCOORD6;
          float4 vtf12_13 : TEXCOORD7;
        };

        v2f vert (appdata v) {
          v2f o;
          o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
          o.uv = v.uv;

          float blurSize = 0.4;

          o.vtf0_1   = float4(o.uv + blurSize * float2(0.0, -0.028), o.uv + blurSize * float2(0.0, -0.024));
			    o.vtf2_3   = float4(o.uv + blurSize * float2(0.0, -0.020), o.uv + blurSize * float2(0.0, -0.016));
			    o.vtf4_5   = float4(o.uv + blurSize * float2(0.0, -0.012), o.uv + blurSize * float2(0.0, -0.008));
			    o.vtf6_7   = float4(o.uv + blurSize * float2(0.0, -0.004), o.uv + blurSize * float2(0.0,  0.004));
			    o.vtf8_9   = float4(o.uv + blurSize * float2(0.0,  0.008), o.uv + blurSize * float2(0.0,  0.012));
			    o.vtf10_11 = float4(o.uv + blurSize * float2(0.0,  0.016), o.uv + blurSize * float2(0.0,  0.020));
			    o.vtf12_13 = float4(o.uv + blurSize * float2(0.0,  0.024), o.uv + blurSize * float2(0.0,  0.028));
          return o;
        }

				fixed4 frag(v2f i) : SV_Target {
					half4 t = tex2D(_MainTex, i.uv);
					if(t.a == 1)
					{
						discard;
					}

					half4 c = half4(0, 0, 0, t.a);
			    c.rgb += (tex2D(_MainTex, i.vtf0_1.xy) * 0.0044299121055113265).rgb;
			    c.rgb += (tex2D(_MainTex, i.vtf0_1.zw) * 0.00895781211794).rgb;
			    c.rgb += (tex2D(_MainTex, i.vtf2_3.xy) * 0.0215963866053).rgb;
			    c.rgb += (tex2D(_MainTex, i.vtf2_3.zw) * 0.0443683338718).rgb;
			    c.rgb += (tex2D(_MainTex, i.vtf4_5.xy) * 0.0776744219933).rgb;
			    c.rgb += (tex2D(_MainTex, i.vtf4_5.zw) * 0.115876621105).rgb;
			    c.rgb += (tex2D(_MainTex, i.vtf6_7.xy) * 0.147308056121).rgb;
			    c.rgb += (t * 0.159576912161).rgb;
			    c.rgb += (tex2D(_MainTex, i.vtf6_7.zw) * 0.147308056121).rgb;
			    c.rgb += (tex2D(_MainTex, i.vtf8_9.xy) * 0.115876621105).rgb;
			    c.rgb += (tex2D(_MainTex, i.vtf8_9.zw) * 0.0776744219933).rgb;
			    c.rgb += (tex2D(_MainTex, i.vtf10_11.xy) * 0.0443683338718).rgb;
			    c.rgb += (tex2D(_MainTex, i.vtf10_11.zw) * 0.0215963866053).rgb;
			    c.rgb += (tex2D(_MainTex, i.vtf12_13.xy) * 0.00895781211794).rgb;
			    c.rgb += (tex2D(_MainTex, i.vtf12_13.zw) * 0.0044299121055113265).rgb;

					return half4(c.rgba);
				}
			ENDCG
		}
	}

}