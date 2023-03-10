Shader "SnapshotProURP/Vortex"
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
			float2 _Center;
			float _Strength;
			float2 _Offset;

            float4 frag (v2f_img i) : SV_Target
            {
				//float2 distance = i.uv - _Center;
				//float squ_magnitude = dot(distance.xy, distance.xy);
				//float offset = squ_magnitude * _Strength;
				//float2 uv = i.uv + float2(distance.y, -distance.x) * offset + _Offset;

				float2 distance = i.uv - _Center;
				float angle = length(distance) * _Strength;
				float x = cos(angle) * distance.x - sin(angle) * distance.y;
				float y = sin(angle) * distance.x + cos(angle) * distance.y;
				float2 uv = float2(x + _Center.x + _Offset.x, y + _Center.y + _Offset.y);

				float4 col = tex2D(_MainTex, uv);
                return col;
            }
            ENDCG
        }
    }
}
