
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

Shader "Hidden/X-PostProcessing/Glitch/RGBSplitV2"
{
	HLSLINCLUDE
	
	#include "../../../Shaders/StdLib.hlsl"
	#include "../../../Shaders/XPostProcessing.hlsl"
	
	uniform half3 _Params;

	#define _TimeX _Params.x
	#define _Amount _Params.y
	#define _Amplitude _Params.z

	
	half4 Frag_Horizontal(VaryingsDefault i): SV_Target
	{
		float splitAmout = (1.0 + sin(_TimeX * 6.0)) * 0.5;
		splitAmout *= 1.0 + sin(_TimeX * 16.0) * 0.5;
		splitAmout *= 1.0 + sin(_TimeX * 19.0) * 0.5;
		splitAmout *= 1.0 + sin(_TimeX * 27.0) * 0.5;
		splitAmout = pow(splitAmout, _Amplitude);
		splitAmout *= (0.05 * _Amount);
		
		half3 finalColor;
		finalColor.r = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, fixed2(i.texcoord.x + splitAmout, i.texcoord.y)).r;
		finalColor.g = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord).g;
		finalColor.b = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, fixed2(i.texcoord.x - splitAmout, i.texcoord.y)).b;
		
		finalColor *= (1.0 - splitAmout * 0.5);
		
		return half4(finalColor, 1.0);
	}

	half4 Frag_Vertical(VaryingsDefault i): SV_Target
	{
		float splitAmout = (1.0 + sin(_TimeX * 6.0)) * 0.5;
		splitAmout *= 1.0 + sin(_TimeX * 16.0) * 0.5;
		splitAmout *= 1.0 + sin(_TimeX * 19.0) * 0.5;
		splitAmout *= 1.0 + sin(_TimeX * 27.0) * 0.5;
		splitAmout = pow(splitAmout, _Amplitude);
		splitAmout *= (0.05 * _Amount);
		
		half3 finalColor;
		finalColor.r = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, fixed2(i.texcoord.x , i.texcoord.y +splitAmout)).r;
		finalColor.g = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord).g;
		finalColor.b = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, fixed2(i.texcoord.x, i.texcoord.y - splitAmout)).b;
		
		finalColor *= (1.0 - splitAmout * 0.5);
		
		return half4(finalColor, 1.0);
	}

	half4 Frag_Vertical_Horizontal(VaryingsDefault i) : SV_Target
	{
		float splitAmout = (1.0 + sin(_TimeX * 6.0)) * 0.5;
		splitAmout *= 1.0 + sin(_TimeX * 16.0) * 0.5;
		splitAmout *= 1.0 + sin(_TimeX * 19.0) * 0.5;
		splitAmout *= 1.0 + sin(_TimeX * 27.0) * 0.5;
		splitAmout = pow(splitAmout, _Amplitude);
		splitAmout *= (0.05 * _Amount);

		half3 finalColor;
		finalColor.r = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, fixed2(i.texcoord.x+splitAmout, i.texcoord.y + splitAmout)).r;
		finalColor.g = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord).g;
		finalColor.b = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, fixed2(i.texcoord.x - splitAmout, i.texcoord.y + splitAmout)).b;

		finalColor *= (1.0 - splitAmout * 0.5);

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

		Pass
		{
			HLSLPROGRAM

			#pragma vertex VertDefault
			#pragma fragment Frag_Vertical_Horizontal

			ENDHLSL

		}
	}
}


