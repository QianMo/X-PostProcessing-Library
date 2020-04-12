//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// created by QianMo @ 2020
//----------------------------------------------------------------------------------------------------------


Shader "Hidden/X-PostProcessing/PixelizeQuad"
{
	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			HLSLPROGRAM

			#pragma vertex VertDefault
			#pragma fragment Frag

			#include "../../../Shaders/StdLib.hlsl"
			#include "../../../Shaders/XPostProcessing.hlsl"

			

			float _PixelSize;
			float _PixelRatio;
			float _PixelScaleX;
			float _PixelScaleY;
			half4 _BackgroundColor;

			float2 RectPixelizeUV( half2 uv)
			{
				float pixelScale = 1.0 / _PixelSize;
				// Divide by the scaling factor, round up, and multiply by the scaling factor to get the segmented UV
				float2 coord = half2(pixelScale * _PixelScaleX * floor(uv.x / (pixelScale *_PixelScaleX)), (pixelScale * _PixelRatio *_PixelScaleY) * floor(uv.y / (pixelScale *_PixelRatio * _PixelScaleY)));

				return  coord;
			}

		

			float4 Frag(VaryingsDefault i) : SV_Target
			{

				float2 uv = RectPixelizeUV(i.texcoord);

				float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);

				return color;

			}

			ENDHLSL

		}
	}
}
