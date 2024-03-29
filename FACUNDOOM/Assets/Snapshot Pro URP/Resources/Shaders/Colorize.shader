﻿Shader "SnapshotProURP/Colorize"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
			CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _TintColor;

            float4 frag (v2f_img i) : SV_Target
            {
				float4 col = tex2D(_MainTex, i.uv);
                return lerp(col, col * _TintColor, _TintColor.a);
            }
            ENDCG
        }
    }
}
