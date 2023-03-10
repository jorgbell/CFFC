Shader "SnapshotProURP/FancyDither"
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

			float _Blend;
			CBUFFER_END

			sampler2D _MainTex;
			sampler2D _NoiseTex;

			sampler2D _CameraDepthTexture;
			sampler2D _CameraDepthNormalsTexture;

			float4x4 _ViewProjectInverse;

			// Credit to https://alexanderameye.github.io/outlineshader.html:
			float3 DecodeNormal(float4 enc)
			{
				float kScale = 1.7777;
				float3 nn = enc.xyz*float3(2 * kScale, 2 * kScale, 0) + float3(-kScale, -kScale, 1);
				float g = 2.0 / dot(nn.xyz, nn.xyz);
				float3 n;
				n.xy = g * nn.xy;
				n.z = g - 1;
				return n;
			}
		ENDHLSL

        Pass
        {
            HLSLPROGRAM

			#pragma vertex vert_img
			#pragma fragment frag

			struct appdata_img
			{
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f_img
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 worldDir : TEXCOORD1;
			};

			v2f_img vert_img(appdata_img v)
			{
				v2f_img o;

				o.pos = TransformObjectToHClip(v.vertex.xyz);
				o.uv = v.texcoord;

				float4 D = mul(_ViewProjectInverse, float4((o.uv.x) * 2 - 1, (o.uv.y) * 2 - 1, 0.5, 1));
				D.xyz /= D.w;
				D.xyz -= _WorldSpaceCameraPos;
				float4 D0 = mul(_ViewProjectInverse, float4(0, 0, 0.5, 1));
				D0.xyz /= D0.w;
				D0.xyz -= _WorldSpaceCameraPos;
				o.worldDir = D.xyz / length(D0.xyz);

				return o;
			}

            float4 frag (v2f_img i) : SV_Target
            {
				float3 col = tex2D(_MainTex, i.uv).xyz;

				float depth = tex2D(_CameraDepthTexture, i.uv).xyz;
				depth = LinearEyeDepth(depth, _ZBufferParams);
				float3 worldPos = i.worldDir * depth + _WorldSpaceCameraPos;

				float3 noiseUV = worldPos / _NoiseSize;

				float4 noiseX = tex2D(_NoiseTex, noiseUV.zy);
				float4 noiseY = tex2D(_NoiseTex, noiseUV.xz);
				float4 noiseZ = tex2D(_NoiseTex, noiseUV.xy);

				float3 normal = DecodeNormal(tex2D(_CameraDepthNormalsTexture, i.uv));

				float3 blend = pow(abs(normal), _Blend);
				blend /= dot(blend, 1.0f);

				float lum = dot(col, float3(0.3f, 0.59f, 0.11f));

				float3 noiseColor = noiseX * blend.x + noiseY * blend.y + noiseZ * blend.z;
				float threshold = dot(noiseColor, float3(0.3f, 0.59f, 0.11f)) + _ThresholdOffset;
				col = lum < threshold ? _DarkColor.xyz : _LightColor.xyz;

				return float4(col, 1.0f);
            }
            ENDHLSL
        }
    }
}
