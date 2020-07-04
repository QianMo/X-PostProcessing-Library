
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

Shader "Hidden/X-PostProcessing/ColorAdjustment/Saturation"
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


			uniform half _Saturation;

			half3 Saturation(half3 In, half Saturation)
			{
				half luma = dot(In, half3(0.2126729, 0.7151522, 0.0721750));
				half3 Out = luma.xxx + Saturation.xxx * (In - luma.xxx);
				return Out;
			}

			half4 Frag(VaryingsDefault i) : SV_Target
			{

				half3 col = 0.5 + 0.5 * cos(_Time.y + i.texcoord.xyx + half3(0, 2, 4));

				half4 sceneColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);

				return half4(Saturation(sceneColor.rgb, _Saturation), 1.0);
			}

			ENDHLSL
		}
	}
}
        
