
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------
// reference: https://www.shadertoy.com/view/Xdf3Rf
//					 https://en.wikipedia.org/wiki/Sobel_operator

Shader "Hidden/X-PostProcessing/EdgeDetectionSobel"
{
	HLSLINCLUDE
	
	#include "../../../Shaders/StdLib.hlsl"
	#include "../../../Shaders/XPostProcessing.hlsl"
	

	half2 _Params;
	half4 _EdgeColor;
	half4 _BackgroundColor;

	#define _EdgeWidth _Params.x
	#define _BackgroundFade _Params.y
	

	float intensity(in float4 color)
	{
		return sqrt((color.x * color.x) + (color.y * color.y) + (color.z * color.z));
	}
	
	float sobel(float stepx, float stepy, float2 center)
	{
		// get samples around pixel
		float topLeft = intensity(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, center + float2(-stepx, stepy)));
		float midLeft = intensity(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, center + float2(-stepx, 0)));
		float bottomLeft = intensity(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, center + float2(-stepx, -stepy)));
		float midTop = intensity(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, center + float2(0, stepy)));
		float midBottom = intensity(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, center + float2(0, -stepy)));
		float topRight = intensity(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, center + float2(stepx, stepy)));
		float midRight = intensity(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, center + float2(stepx, 0)));
		float bottomRight = intensity(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, center + float2(stepx, -stepy)));
		
		// Sobel masks (see http://en.wikipedia.org/wiki/Sobel_operator)
		//        1 0 -1     -1 -2 -1
		//    X = 2 0 -2  Y = 0  0  0
		//        1 0 -1      1  2  1

		// Gx = sum(kernelX[i][j]*image[i][j])
		float Gx = topLeft + 2.0 * midLeft + bottomLeft - topRight - 2.0 * midRight - bottomRight;
		// Gy = sum(kernelY[i][j]*image[i][j]);
		float Gy = -topLeft - 2.0 * midTop - topRight + bottomLeft + 2.0 * midBottom + bottomRight;
		float sobelGradient = sqrt((Gx * Gx) + (Gy * Gy));
		return sobelGradient;
	}
	
	
	
	half4 Frag(VaryingsDefault i): SV_Target
	{
		
		half4 sceneColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
		
		float sobelGradient= sobel(_EdgeWidth /_ScreenParams.x, _EdgeWidth /_ScreenParams.y , i.texcoord);

		half4 backgroundColor = lerp(sceneColor, _BackgroundColor, _BackgroundFade);

		float3 edgeColor = lerp(backgroundColor.rgb, _EdgeColor.rgb, sobelGradient);

		return float4(edgeColor, 1);

	}
	
	ENDHLSL
	
	SubShader
	{
		Cull Off ZWrite Off ZTest Always
		
		Pass
		{
			HLSLPROGRAM
			
			#pragma vertex VertDefault
			#pragma fragment Frag
			
			ENDHLSL
			
		}
	}
}


