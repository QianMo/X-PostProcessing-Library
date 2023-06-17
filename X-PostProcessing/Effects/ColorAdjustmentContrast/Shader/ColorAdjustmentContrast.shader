﻿
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

Shader "Hidden/X-PostProcessing/ColorAdjustment/Contrast"
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


			uniform half _Contrast;


			half3 ColorAdjustment_Contrast(half3 In, half Contrast)
			{
				half midpoint = 0.21763h;//pow(0.5, 2.2);
				half3 Out = (In - midpoint) * Contrast + midpoint;
				return Out;
			}

			half4 Frag(VaryingsDefault i) : SV_Target
			{

				half4 finalColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);

				finalColor.rgb = ColorAdjustment_Contrast(finalColor.rgb , _Contrast);

				return finalColor;

			}

			ENDHLSL
		}
	}
}
        
