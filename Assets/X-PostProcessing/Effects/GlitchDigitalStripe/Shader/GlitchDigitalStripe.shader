

//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

//reference : https://github.com/keijiro/KinoGlitch

Shader "Hidden/X-PostProcessing/Glitch/DigitalStripe"
{
	HLSLINCLUDE
	
	#include "../../../Shaders/XPostProcessing.hlsl"
	
	TEXTURE2D_SAMPLER2D(_NoiseTex, sampler_NoiseTex);
	TEXTURE2D_SAMPLER2D(_TrashTex, sampler_TrashTex);
	uniform half _Indensity;

	
	half4 Frag(VaryingsDefault i): SV_Target
	{

		 half4 glitch = SAMPLE_TEXTURE2D(_NoiseTex, sampler_NoiseTex, i.texcoord);

		 // Data Prepare
		 half threshold = 1.001 - _Indensity * 1.001;
		 half displacement = step(threshold, pow(abs(glitch.z), 2.5));
		 half frame = step(threshold, pow(abs(glitch.w), 2.5));
		 half glitchValue = step(threshold, pow(abs(glitch.z), 3.5));

		// Displacement
		float2 uv = frac(i.texcoord + glitch.xy * displacement);
		half4 source = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);

		// Lerp with trash frame
		half3 color = lerp(source, SAMPLE_TEXTURE2D(_TrashTex, sampler_TrashTex, uv), frame).rgb;

		// Shift color components
		//half3 negative = saturate(color.rgb + (1 - dot(color, 1)) * 0.5);
		//color = lerp(color, negative, glitchValue );

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

    
