
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

Shader "Hidden/X-PostProcessing/DualTentBlur"
{
	HLSLINCLUDE
	
	#include "../../../Shaders/StdLib.hlsl"
	#include "../../../Shaders/XPostProcessing.hlsl"
	
	half4 _BlurOffset;
	
	// 9-tap tent filter
	half4 TentFilter_9Tap(TEXTURE2D_ARGS(tex, samplerTex), float2 uv, float2 texelSize)
	{
		float4 d = texelSize.xyxy * float4(1.0, 1.0, -1.0, 0.0);
		
		half4 s;
		s = SAMPLE_TEXTURE2D(tex, samplerTex, uv - d.xy);
		s += SAMPLE_TEXTURE2D(tex, samplerTex, uv - d.wy) * 2.0; // 1 MAD
		s += SAMPLE_TEXTURE2D(tex, samplerTex, uv - d.zy); // 1 MAD
		
		s += SAMPLE_TEXTURE2D(tex, samplerTex, uv + d.zw) * 2.0; // 1 MAD
		s += SAMPLE_TEXTURE2D(tex, samplerTex, uv) * 4.0; // 1 MAD
		s += SAMPLE_TEXTURE2D(tex, samplerTex, uv + d.xw) * 2.0; // 1 MAD
		
		s += SAMPLE_TEXTURE2D(tex, samplerTex, uv + d.zy);
		s += SAMPLE_TEXTURE2D(tex, samplerTex, uv + d.wy) * 2.0; // 1 MAD
		s += SAMPLE_TEXTURE2D(tex, samplerTex, uv + d.xy);
		
		return s * (1.0 / 16.0);
	}
	
	float4 FragTentBlur(VaryingsDefault i): SV_Target
	{
		return TentFilter_9Tap(TEXTURE2D_PARAM(_MainTex, sampler_MainTex), i.texcoord, _BlurOffset.xy).rgba;
	}
	
	float4 FragCombine(VaryingsDefault i): SV_Target
	{
		return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo);
	}
	
	ENDHLSL
	
	SubShader
	{
		Cull Off ZWrite Off ZTest Always
		
		Pass
		{
			HLSLPROGRAM
			
			#pragma vertex VertDefault
			#pragma fragment FragTentBlur
			
			ENDHLSL
			
		}
		
		Pass
		{
			HLSLPROGRAM
			
			#pragma vertex VertDefault
			#pragma fragment FragCombine
			
			ENDHLSL
			
		}
	}
}


