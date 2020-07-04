
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

Shader "Hidden/X-PostProcessing/ColorAdjustment/BleachBypass"
{
	HLSLINCLUDE
	
	#include "../../../Shaders/StdLib.hlsl"
	#include "../../../Shaders/XPostProcessing.hlsl"
	
	uniform half _Indensity;
	
	half luminance(half3 color)
	{
		return dot(color, half3(0.222, 0.707, 0.071));
	}
	
	//reference : https://developer.download.nvidia.com/shaderlibrary/webpages/shader_library.html
	half4 Frag(VaryingsDefault i): SV_Target
	{	
		half4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
		half lum = luminance(color.rgb);
		half3 blend = half3(lum, lum, lum);
		half L = min(1.0, max(0.0, 10.0 * (lum - 0.45)));
		half3 result1 = 2.0 * color.rgb * blend;
		half3 result2 = 1.0 - 2.0 * (1.0 - blend) * (1.0 - color.rgb);
		half3 newColor = lerp(result1, result2, L);
		
		return lerp(color, half4(newColor, color.a), _Indensity);
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


