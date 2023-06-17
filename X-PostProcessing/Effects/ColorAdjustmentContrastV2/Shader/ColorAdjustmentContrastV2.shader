
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

Shader "Hidden/X-PostProcessing/ColorAdjustment/ContrastV2"
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
			uniform half3 _ContrastFactorRGB;

			half3 ColorAdjustment_Contrast_V2(float3 In, half3 ContrastFactor, float Contrast)
			{
				half3 Out = (In - ContrastFactor) * Contrast + ContrastFactor;
				return Out;
			}

			half4 Frag(VaryingsDefault i) : SV_Target
			{

				half4 finalColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);

				finalColor.rgb = ColorAdjustment_Contrast_V2(finalColor.rgb , _ContrastFactorRGB,_Contrast);

				return finalColor;
			}

			ENDHLSL
		}
	}
}
        
