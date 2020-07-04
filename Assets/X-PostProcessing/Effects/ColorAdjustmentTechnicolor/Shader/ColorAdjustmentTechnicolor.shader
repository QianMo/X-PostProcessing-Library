
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

Shader "Hidden/X-PostProcessing/ColorAdjustment/Technicolor"
{
	HLSLINCLUDE
	
	#include "../../../Shaders/StdLib.hlsl"
	#include "../../../Shaders/XPostProcessing.hlsl"
	

	half _Exposure;
	half3 _ColorBalance;
	half _Indensity;


	// reference : https://github.com/crosire/reshade-shaders/blob/master/Shaders/Technicolor.fx
	half4 Frag(VaryingsDefault i): SV_Target
	{
		const half3 cyanfilter = float3(0.0, 1.30, 1.0);
		const half3 magentafilter = float3(1.0, 0.0, 1.05);
		const half3 yellowfilter = float3(1.6, 1.6, 0.05);
		const half2 redorangefilter = float2(1.05, 0.620); // RG_
		const half2 greenfilter = float2(0.30, 1.0);       // RG_
		const half2 magentafilter2 = magentafilter.rb;     // R_B


		half4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);

		half3 balance = 1.0 / (_ColorBalance.rgb * _Exposure);

		half negative_mul_r = dot(redorangefilter, color.rg * balance.rr);
		half negative_mul_g = dot(greenfilter, color.rg * balance.gg);
		half negative_mul_b = dot(magentafilter2, color.rb * balance.bb);

		half3 output_r = negative_mul_r.rrr + cyanfilter;
		half3 output_g = negative_mul_g .rrr + magentafilter;
		half3 output_b = negative_mul_b.rrr + yellowfilter;

		half3 result = output_r  * output_g * output_b;
		return half4(lerp(color.rgb, result.rgb, _Indensity), 1.0);

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

    
