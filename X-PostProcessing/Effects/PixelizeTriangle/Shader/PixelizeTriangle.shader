//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------


Shader "Hidden/X-PostProcessing/PixelizeTriangle"
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


			float2 TrianglePixelizeUV(float2 uv)
			{

				float2 pixelScale = _PixelSize * float2(_PixelScaleX, _PixelScaleY / _PixelRatio);

				//乘以缩放，向下取整，再除以缩放，得到分段UV
				float2 coord = floor(uv * pixelScale) / pixelScale;

				uv -= coord;
				uv *= pixelScale;

				//进行三角形像素偏移处理
				coord += 
					float2(step(1.0 - uv.y, uv.x) / (2.0 * pixelScale.x),//X
					step(uv.x, uv.y) / (2.0 * pixelScale.y)//Y
					);

				return  coord;
			}


			float4 Frag(VaryingsDefault i) : SV_Target
			{
				float2 uv = TrianglePixelizeUV(i.texcoord);

				return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);		
			}

			ENDHLSL

		}
	}
}
