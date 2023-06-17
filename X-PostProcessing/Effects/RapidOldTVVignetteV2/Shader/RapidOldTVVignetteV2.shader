
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

Shader "Hidden/X-PostProcessing/RapidOldTVVignetteV2"
{
	
	HLSLINCLUDE
	
	
	#include "../../../Shaders/StdLib.hlsl"
	#include "../../../Shaders/XPostProcessing.hlsl"
	
	uniform half _VignetteSize;
	uniform half _SizeOffset;
	uniform half4 _VignetteColor;
	
	
	float4 Frag(VaryingsDefault i): SV_Target
	{
		half2 uv = -i.texcoord * i.texcoord + i.texcoord;	     //MAD
		half VignetteIndensity = saturate(uv.x * uv.y * _VignetteSize + _SizeOffset);
		return VignetteIndensity * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
	}
	
	float4 Frag_ColorAdjust(VaryingsDefault i): SV_Target
	{
		half2 uv = -i.texcoord * i.texcoord + i.texcoord;    //MAD
		half VignetteIndensity = saturate(uv.x * uv.y * _VignetteSize + _SizeOffset);
		
		return lerp(_VignetteColor, SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord), VignetteIndensity);
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
		
		
		Pass
		{
			HLSLPROGRAM
			
			#pragma vertex VertDefault
			#pragma fragment Frag_ColorAdjust
			
			ENDHLSL
			
		}
	}
}

