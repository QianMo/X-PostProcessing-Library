
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

Shader "Hidden/X-PostProcessing/KawaseBlur"
{
	HLSLINCLUDE
	
	#include "../../../Shaders/StdLib.hlsl"
	#include "../../../Shaders/XPostProcessing.hlsl"
	
	uniform half _Offset;
	
	
	half4 KawaseBlur(TEXTURE2D_ARGS(tex, samplerTex), float2 uv, float2 texelSize, half pixelOffset)
	{
		half4 o = 0;
		o += SAMPLE_TEXTURE2D(tex, samplerTex, uv + float2(pixelOffset +0.5, pixelOffset +0.5) * texelSize); 
		o += SAMPLE_TEXTURE2D(tex, samplerTex, uv + float2(-pixelOffset -0.5, pixelOffset +0.5) * texelSize); 
		o += SAMPLE_TEXTURE2D(tex, samplerTex, uv + float2(-pixelOffset -0.5, -pixelOffset -0.5) * texelSize); 
		o += SAMPLE_TEXTURE2D(tex, samplerTex, uv + float2(pixelOffset +0.5, -pixelOffset -0.5) * texelSize); 
		return o * 0.25;
	}
	
	
	half4 Frag(VaryingsDefault i): SV_Target
	{
		return KawaseBlur(TEXTURE2D_PARAM(_MainTex, sampler_MainTex), i.texcoord.xy, _MainTex_TexelSize.xy, _Offset);
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


