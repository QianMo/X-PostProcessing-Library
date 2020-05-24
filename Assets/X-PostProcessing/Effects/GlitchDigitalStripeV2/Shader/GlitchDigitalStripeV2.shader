
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

Shader "Hidden/X-PostProcessing/Glitch/DigitalStripeV2"
{
	HLSLINCLUDE
	
	#include "../../../Shaders/StdLib.hlsl"
	#include "../../../Shaders/XPostProcessing.hlsl"
	
	
	TEXTURE2D_SAMPLER2D(_NoiseTex, sampler_NoiseTex);
	TEXTURE2D_SAMPLER2D(_TrashTex, sampler_TrashTex);
	float _Intensity;
	float _ColorIntensity;
	
	float4 Frag(VaryingsDefault i): SV_Target
	{
		float4 glitch = SAMPLE_TEXTURE2D(_NoiseTex, sampler_NoiseTex, i.texcoord);
		float thresh = 1.001 - _Intensity * 1.001;
		float w_d = step(thresh * _ColorIntensity, pow(abs(glitch.z), 2.5));
		float w_f = step((thresh), (pow(abs(glitch.w), 2.5)));
		float w_c = step(thresh, pow(abs(glitch.z), 3.5));
		float2 uv = frac(i.texcoord + glitch.xy * w_d);
		float4 source = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
		float3 color = lerp(source, SAMPLE_TEXTURE2D(_TrashTex, sampler_TrashTex, uv), w_f).rgb;
		float3 neg = saturate(color.grb + (1 - dot(color, 1)) * 0.1);
		color = lerp(color, neg, w_c);
		
		return float4(color, source.a);
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


