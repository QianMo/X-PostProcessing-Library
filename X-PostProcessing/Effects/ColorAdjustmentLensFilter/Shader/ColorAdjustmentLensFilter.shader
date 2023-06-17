
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

Shader "Hidden/X-PostProcessing/ColorAdjustment/LensFilter"
{
	HLSLINCLUDE
	
	#include "../../../Shaders/StdLib.hlsl"
	#include "../../../Shaders/XPostProcessing.hlsl"

	uniform half _Indensity;
	uniform half4 _LensColor;
	
	half luminance(half3 color)
	{
		return dot(color, half3(0.222, 0.707, 0.071));
	}

	half4 Frag(VaryingsDefault i): SV_Target
	{
		half4 sceneColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);

		half lum = luminance(sceneColor.rgb);

		// Interpolate with half4(0.0, 0.0, 0.0, 0.0) based on luminance
		half4 filterColor = lerp(half4(0.0, 0.0, 0.0, 0.0), _LensColor, saturate(lum * 2.0));

		// Interpolate withhalf4(1.0, 1.0, 1.0, 1.0) based on luminance
		filterColor = lerp(filterColor, half4(1.0, 1.0, 1.0, 1.0), saturate(lum - 0.5) * 2.0);

		filterColor = lerp(sceneColor, filterColor, saturate(lum * _Indensity));

		return half4(filterColor.rgb, sceneColor.a);
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

    
