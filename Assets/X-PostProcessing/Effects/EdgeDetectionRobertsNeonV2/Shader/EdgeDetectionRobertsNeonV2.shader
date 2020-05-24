
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

Shader "Hidden/X-PostProcessing/EdgeDetectionRobertsNeonV2"
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
	
	
	float3 sobel(float stepx, float stepy, float2 center)
	{
		// get samples around pixel
		float3 topLeft = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, center + float2(-stepx, stepy)).rgb;
		float3 bottomLeft = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, center + float2(-stepx, -stepy)).rgb;
		float3 topRight = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, center + float2(stepx, stepy)).rgb;
		float3 bottomRight = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, center + float2(stepx, -stepy)).rgb;
		
		// Roberts Operator
		//X = -1   0      Y = 0  -1
		//     0   1          1   0
		
		// Gx = sum(kernelX[i][j]*image[i][j])
		float3 Gx = -1.0 * topLeft + 1.0 * bottomRight;
		
		// Gy = sum(kernelY[i][j]*image[i][j]);
		float3 Gy = -1.0 * topRight + 1.0 * bottomLeft;
		
		
		float3 sobelGradient = sqrt((Gx * Gx) + (Gy * Gy));
		return sobelGradient;
	}
	
	
	half4 Frag(VaryingsDefault i): SV_Target
	{
		
		
		half4 sceneColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
		
		float3 sobelGradient = sobel(_EdgeWidth / _ScreenParams.x, _EdgeWidth / _ScreenParams.y, i.texcoord);
		
		half3 backgroundColor = lerp(_BackgroundColor.rgb, sceneColor.rgb, _BackgroundFade);
		
		//Edge Opacity
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


