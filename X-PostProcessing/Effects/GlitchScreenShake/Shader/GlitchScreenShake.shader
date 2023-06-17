

//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

Shader "Hidden/X-PostProcessing/Glitch/ScreenShake"
{
	HLSLINCLUDE
	
	#include "../../../Shaders/StdLib.hlsl"
	#include "../../../Shaders/XPostProcessing.hlsl"
	
	uniform half _ScreenShake;
	
	
	float randomNoise(float x, float y)
	{
		return frac(sin(dot(float2(x, y), float2(127.1, 311.7))) * 43758.5453);
	}
	
	
	half4 Frag_Horizontal(VaryingsDefault i): SV_Target
	{
		float shake = (randomNoise(_Time.x, 2) - 0.5) * _ScreenShake;
		
		half4 sceneColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, frac(float2(i.texcoord.x + shake, i.texcoord.y)));
		
		return sceneColor;
	}
	
	half4 Frag_Vertical(VaryingsDefault i): SV_Target
	{
		
		float shake = (randomNoise(_Time.x, 2) - 0.5) * _ScreenShake;
		
		half4 sceneColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, frac(float2(i.texcoord.x, i.texcoord.y + shake)));
		
		return sceneColor;
	}
	
	ENDHLSL
	
	SubShader
	{
		Cull Off ZWrite Off ZTest Always
		
		Pass
		{
			HLSLPROGRAM
			
			#pragma vertex VertDefault
			#pragma fragment Frag_Horizontal
			
			ENDHLSL
			
		}
		
		Pass
		{
			HLSLPROGRAM
			
			#pragma vertex VertDefault
			#pragma fragment Frag_Vertical
			
			ENDHLSL
			
		}
	}
}


