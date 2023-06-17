
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

Shader "Hidden/X-PostProcessing/EdgeDetectionSobelNeonV2"
{
	HLSLINCLUDE
	
	#include "../../../Shaders/StdLib.hlsl"
	#include "../../../Shaders/XPostProcessing.hlsl"

	half4 _Params;
	half4 _BackgroundColor;

	#define _EdgeWidth _Params.x
	#define _EdgeNeonFade _Params.y
	#define _Brigtness _Params.z
	#define _BackgroundFade _Params.w


	float intensity(in float4 color)
	{
		return sqrt((color.x * color.x) + (color.y * color.y) + (color.z * color.z));
	}

	float3 sobel(float stepx, float stepy, float2 center)
	{
		// get samples around pixel
		float3 topLeft = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, center + float2(-stepx, stepy)).rgb;
		float3 midLeft = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, center + float2(-stepx, 0)).rgb;
		float3 bottomLeft = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, center + float2(-stepx, -stepy)).rgb;
		float3 midTop = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, center + float2(0, stepy)).rgb;
		float3 midBottom = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, center + float2(0, -stepy)).rgb;
		float3 topRight = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, center + float2(stepx, stepy)).rgb;
		float3 midRight = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, center + float2(stepx, 0)).rgb;
		float3 bottomRight = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, center + float2(stepx, -stepy)).rgb;

		// Sobel masks (see http://en.wikipedia.org/wiki/Sobel_operator)
		//        1 0 -1     -1 -2 -1
		//    X = 2 0 -2  Y = 0  0  0
		//        1 0 -1      1  2  1

		// Gx = sum(kernelX[i][j]*image[i][j])
		float3 Gx = topLeft + 2.0 * midLeft + bottomLeft - topRight - 2.0 * midRight - bottomRight;
		// Gy = sum(kernelY[i][j]*image[i][j]);
		float3 Gy = -topLeft - 2.0 * midTop - topRight + bottomLeft + 2.0 * midBottom + bottomRight;
		float3 sobelGradient = sqrt((Gx * Gx) + (Gy * Gy));
		return sobelGradient;
	}



	half4 Frag(VaryingsDefault i) : SV_Target
	{
		half4 sceneColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);

		float3 sobelGradient = sobel(_EdgeWidth / _ScreenParams.x, _EdgeWidth / _ScreenParams.y , i.texcoord);

		half3 backgroundColor = lerp(_BackgroundColor.rgb, sceneColor.rgb, _BackgroundFade);

		float3 edgeColor = lerp(backgroundColor.rgb, sobelGradient.rgb, _EdgeNeonFade);

		return float4(edgeColor * _Brigtness, 1);

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

    
