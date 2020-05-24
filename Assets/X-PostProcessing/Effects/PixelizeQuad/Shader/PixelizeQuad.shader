//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
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

			half4 _Params;
			#define _PixelSize _Params.x
			#define _PixelRatio _Params.y
			#define _PixelScaleX _Params.z
			#define _PixelScaleY _Params.w	

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
