Shader "Hidden/Aura" {
  Properties {
    _MainTex ("Cutout Source", 2D) = "white" {}
    _Color ("Aura Color", Color) = (1,1,1,1)
  }
  SubShader {
    Tags { "Queue"="Transparent" "RenderType"="Transparent" }
    LOD 200

    Pass {
      Tags { "LightMode" = "Always" }

      Fog { Mode Off }
      ZWrite Off
      Cull Back
      Lighting Off
      Blend One Zero

      CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #pragma fragmentoption ARB_precision_hint_fastest
        #include "UnityCG.cginc"

        uniform sampler2D_float _CameraDepthTexture;
				uniform sampler2D _MainTex;
        uniform fixed4 _Color;

        struct appdata {
          float4 vertex : POSITION;
          float2 uv : TEXCOORD0;
        };

        struct v2f {
          float4 vertex : POSITION;
          float2 uv : TEXCOORD0;
          float4 projPos : TEXCOORD1;
        };

        v2f vert (appdata v) {
          v2f o;
          o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
          o.projPos = ComputeScreenPos(o.vertex);
          o.uv = v.uv;
          return o;
        }

        fixed4 frag (v2f i) : COLOR {
          return half4(_Color.rgb, 1);
        }
      ENDCG
    }
  }
}