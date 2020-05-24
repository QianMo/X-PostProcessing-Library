
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

Shader "Hidden/X-PostProcessing/BoxBlur"
{
	HLSLINCLUDE
	
	#include "../../../Shaders/StdLib.hlsl"
	#include "../../../Shaders/XPostProcessing.hlsl"
	
	half4 _BlurOffset;
	
	half4 BoxFilter_4Tap(TEXTURE2D_ARGS(tex, samplerTex), float2 uv, float2 texelSize)
	{
		float4 d = texelSize.xyxy * float4(-1.0, -1.0, 1.0, 1.0);
		
		half4 s = 0;
		s = SAMPLE_TEXTURE2D(tex, samplerTex, uv + d.xy) * 0.25h;  // 1 MUL
		s += SAMPLE_TEXTURE2D(tex, samplerTex, uv + d.zy) * 0.25h; // 1 MAD
		s += SAMPLE_TEXTURE2D(tex, samplerTex, uv + d.xw) * 0.25h; // 1 MAD
		s += SAMPLE_TEXTURE2D(tex, samplerTex, uv + d.zw) * 0.25h; // 1 MAD
		
		return s;
	}
	
	
	float4 FragBoxBlur(VaryingsDefault i): SV_Target
	{
		return BoxFilter_4Tap(TEXTURE2D_PARAM(_MainTex, sampler_MainTex), i.texcoord, _BlurOffset.xy).rgba;
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
			#pragma fragment FragBoxBlur
			
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


