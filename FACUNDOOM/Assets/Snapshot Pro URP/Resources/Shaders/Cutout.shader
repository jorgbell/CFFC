Shader "SnapshotProURP/Cutout"
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
			sampler2D _CutoutTex;
			float4 _BorderColor;
			int _Stretch;

			float _Zoom;
			float2 _Offset;
			float _Rotation;

            float4 frag (v2f_img i) : SV_Target
            {
				float2 UVs = (i.uv - 0.5f) / _Zoom;

				float aspect = (_Stretch == 0) ? _ScreenParams.x / _ScreenParams.y : 1.0f;
				UVs = float2(aspect * UVs.x, UVs.y);

                float sinRot = sin(_Rotation);
		        float cosRot = cos(_Rotation);

				float2x2 rotation = float2x2(cosRot, sinRot, -sinRot, cosRot);

				UVs = mul(rotation, UVs);
				UVs += float2(_Offset.x * aspect, _Offset.y) + 0.5f;

				float cutoutAlpha = tex2D(_CutoutTex, UVs).a;
				float4 col = tex2D(_MainTex, i.uv);
				return lerp(col, _BorderColor, cutoutAlpha);
            }
            ENDCG
        }
    }
}
