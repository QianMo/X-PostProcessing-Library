//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

Shader "Hidden/X-PostProcessing/PixelizeCircle"
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
			half2 _Params2;
			half4 _BackgroundColor;

			#define _PixelIntervalX _Params2.x
			#define _PixelIntervalY _Params2.y


			float4 CirclePixelize(float2 uv)
			{
				float pixelScale = 1.0 / _Params.x;

				float ratio = _ScreenParams.y / _ScreenParams.x;
				uv.x = uv.x / ratio;

				//x和y坐标分别除以缩放系数，在用floor向下取整，再乘以缩放系数，得到分段UV
				float2 coord = half2(_PixelIntervalX *  floor(uv.x / (pixelScale * _PixelIntervalX)), (_PixelIntervalY)* floor(uv.y / (pixelScale * _PixelIntervalY)));

				//求解圆心坐标
				float2 circleCenter = coord * pixelScale + pixelScale * 0.5;

				//计算当前uv值隔圆心的距离，并乘以缩放系数
				float dist = length(uv - circleCenter) * _Params.x;
				//圆心坐标乘以缩放系数
				circleCenter.x *= ratio;

				//采样
				float4 screenColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, circleCenter);

				//对于距离大于半径的像素，替换为背景色
				if (dist > _Params.z)  screenColor = _BackgroundColor;

				return screenColor;
			}
		
			
			
			float4 Frag(VaryingsDefault i): SV_Target
			{

				return CirclePixelize(i.texcoord);
			}
			
			ENDHLSL
			
		}
	}
}
