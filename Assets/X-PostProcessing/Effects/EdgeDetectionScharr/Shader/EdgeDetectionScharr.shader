
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

Shader "Hidden/X-PostProcessing/EdgeDetectionScharr"
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

	float scharr(float stepx, float stepy, float2 center)
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

		// scharr masks ( http://en.wikipedia.org/wiki/Sobel_operator#Alternative_operators)
		//        3 0 -3        3 10   3
		//    X = 10 0 -10  Y = 0  0   0
		//        3 0 -3        -3 -10 -3

		// Gx = sum(kernelX[i][j]*image[i][j]);
		float Gx = 3.0* topLeft + 10.0 * midLeft + 3.0 * bottomLeft -3.0* topRight - 10.0 * midRight - 3.0* bottomRight;
		// Gy = sum(kernelY[i][j]*image[i][j]);
		float Gy = 3.0 * topLeft + 10.0 * midTop + 3.0 * topRight -3.0* bottomLeft - 10.0 * midBottom -3.0* bottomRight;

		float scharrGradient = sqrt((Gx * Gx) + (Gy * Gy));
		return scharrGradient;
	}



	half4 Frag(VaryingsDefault i) : SV_Target
	{

		half4 sceneColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);

		float scharrGradient = scharr(_EdgeWidth / _ScreenParams.x, _EdgeWidth / _ScreenParams.y , i.texcoord);

		//return sceneColor * scharrGradient;
		//BackgroundFading
		sceneColor = lerp(sceneColor, _BackgroundColor, _BackgroundFade);

		//Edge Opacity
		float3 edgeColor = lerp(sceneColor.rgb, _EdgeColor.rgb, scharrGradient);

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


