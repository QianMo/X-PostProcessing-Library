
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

Shader "Hidden/X-PostProcessing/ColorAdjustment/Brightness"
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

			uniform half _Brightness;


			half4 Frag(VaryingsDefault i) : SV_Target
			{

				half3 sceneColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord).rgb;
				return half4(sceneColor * _Brightness, 1.0);
			}

			ENDHLSL
		}
	}
}
        
