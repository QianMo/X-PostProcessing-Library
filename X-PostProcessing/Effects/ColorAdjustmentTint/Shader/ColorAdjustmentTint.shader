

//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

Shader "Hidden/X-PostProcessing/ColorAdjustment/Tint"
{
	HLSLINCLUDE
	
	#include "../../../Shaders/StdLib.hlsl"
	#include "../../../Shaders/XPostProcessing.hlsl"
	
	uniform half _Indensity;
	uniform half4 _ColorTint;
	
	half4 Frag(VaryingsDefault i): SV_Target
	{	
		half4 sceneColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
		
		half3 finalColor = lerp(sceneColor.rgb, sceneColor.rgb * _ColorTint.rgb, _Indensity);
		
		return half4(finalColor, 1.0);
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

    
