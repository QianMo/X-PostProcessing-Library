
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

Shader "Hidden/X-PostProcessing/DirectionalBlur"
{
	HLSLINCLUDE
	
	#include "../../../Shaders/StdLib.hlsl"
	#include "../../../Shaders/XPostProcessing.hlsl"

	half3 _Params;	

	#define _Iteration _Params.x
	#define _Direction _Params.yz
	
	half4 DirectionalBlur(VaryingsDefault i)
	{
		half4 color = half4(0.0, 0.0, 0.0, 0.0);

		for (int k = -_Iteration; k < _Iteration; k++)
		{
			color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord - _Direction * k);
		}
		half4 finalColor = color / (_Iteration * 2.0);

		return finalColor;
	}

	half4 Frag(VaryingsDefault i): SV_Target
	{
		return DirectionalBlur(i);
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

    
