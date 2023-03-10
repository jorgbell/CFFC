Shader "SnapshotProURP/BasicDither"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
		//_NoiseTex("Noise Texture", 2D) = "white" {}
		//_NoiseSize("Noise Size", Float) = 1
		//_ThresholdOffset("Threshold Offset", Float) = 0

		//_LightColor("Light Color", Color) = (1, 1, 1, 1)
		//_DarkColor("Dark Color", Color) = (0, 0, 0, 0)
    }
    SubShader
    {
        Tags 
		{ 
			"RenderType" = "Opaque" 
			"RenderPipeline" = "UniversalPipeline"
		}

		HLSLINCLUDE
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

			CBUFFER_START(UnityPerMaterial)
			float4 _NoiseTex_TexelSize;
			float _NoiseSize;
			float _ThresholdOffset;

			float4 _LightColor;
			float4 _DarkColor;
			CBUFFER_END

			sampler2D _MainTex;
			sampler2D _NoiseTex;
		ENDHLSL

        Pass
        {
            HLSLPROGRAM

			#pragma vertex vert_img
			#pragma fragment frag
			#pragma multi_compile __ USE_SCENE_TEXTURE_ON

			struct appdata_img
			{
				float4 vertex : POSITION;
				half2 texcoord : TEXCOORD0;
			};

			struct v2f_img
			{
				float4 pos : SV_POSITION;
				half2 uv : TEXCOORD0;
			};

			v2f_img vert_img(appdata_img v)
			{
				v2f_img o;

				o.pos = TransformObjectToHClip(v.vertex.xyz);
				o.uv = v.texcoord;
				return o;
			}

            float4 frag (v2f_img i) : SV_Target
            {
				float3 col = tex2D(_MainTex, i.uv).xyz;
				float lum = dot(col, float3(0.3f, 0.59f, 0.11f));

				float2 noiseUV = i.uv * _NoiseTex_TexelSize.xy * _ScreenParams.xy * 2.0f / _NoiseSize;
				float3 noiseColor = tex2D(_NoiseTex, noiseUV).xyz;
				float threshold = dot(noiseColor, float3(0.3f, 0.59f, 0.11f)) + _ThresholdOffset;

#ifdef USE_SCENE_TEXTURE_ON
				col = lum < threshold ? _DarkColor.xyz : col;
#else
				col = lum < threshold ? _DarkColor.xyz : _LightColor.xyz;
#endif

				return float4(col, 1.0f);
            }
            ENDHLSL
        }
    }
}
