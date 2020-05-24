
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

Shader "Hidden/X-PostProcessing/Glitch/RGBSplitV5"
{
	HLSLINCLUDE
	
	#include "../../../Shaders/XPostProcessing.hlsl"
	
	
	TEXTURE2D_SAMPLER2D(_NoiseTex, sampler_NoiseTex);

	uniform half2 _Params;
	#define _Amplitude _Params.x
	#define _Speed _Params.y
	

	inline float4 Pow4(float4 v, float p)
	{
		return float4(pow(v.x, p), pow(v.y, p), pow(v.z, p), v.w);
	}

	inline float4 Noise(float2 p)
	{
		return SAMPLE_TEXTURE2D(_NoiseTex, sampler_NoiseTex, p);
	}

	half4 Frag(VaryingsDefault i): SV_Target
	{
		float4 splitAmount = Pow4(Noise(float2(_Speed * _Time.y, 2.0 * _Speed * _Time.y / 25.0)), 8.0) * float4(_Amplitude, _Amplitude, _Amplitude, 1.0);

		splitAmount *= 2.0 * splitAmount.w - 1.0;

		half colorR = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, (i.texcoord.xy + float2(splitAmount.x, -splitAmount.y))).r;
		half colorG = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, (i.texcoord.xy + float2(splitAmount.y, -splitAmount.z))).g;
		half colorB = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, (i.texcoord.xy + float2(splitAmount.z, -splitAmount.x))).b;

		half3 finalColor = half3(colorR, colorG, colorB);
		return half4(finalColor,1);

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

    
