
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

		// Data prepare
		float threshold = 1.001 - _Intensity * 1.001;
		float displacement = step(threshold * _ColorIntensity, pow(abs(glitch.z), 2.5));
		float frame = step((threshold), (pow(abs(glitch.w), 2.5)));
		float glitchValue = step(threshold, pow(abs(glitch.z), 3.5));

		// Displacement
		float2 uv = frac(i.texcoord + glitch.xy * displacement);
		float4 source = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);

		// Lerp with trash frame
		float3 color = lerp(source, SAMPLE_TEXTURE2D(_TrashTex, sampler_TrashTex, uv), frame).rgb;

		float3 negative = saturate(color.grb + (1 - dot(color, 1)) * 0.1);
		color = lerp(color, negative, glitchValue);
		
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


