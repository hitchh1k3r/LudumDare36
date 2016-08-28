Shader "Hidden/HalfBlend" {

	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}

	SubShader {
		Pass {
      Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				uniform sampler2D _MainTex;

        struct appdata {
          float4 vertex : POSITION;
          float4 uv : TEXCOORD0;
        };

        struct v2f {
          float4 vertex : POSITION;
          float2 uv : TEXCOORD0;
        };

        v2f vert (appdata v) {
          v2f o;
          o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
          o.uv = v.uv;
          return o;
        }

				fixed4 frag(v2f i) : SV_Target {
					half4 c = tex2D(_MainTex, i.uv);
					if(c.r == 0 && c.g == 0 && c.b == 0)
					{
						discard;
					}
					return half4(c.rgb, 0.5);
				}
			ENDCG
		}
	}

}