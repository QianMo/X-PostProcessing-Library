
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

Shader "Hidden/X-PostProcessing/Glitch/RGBSplitV3"
{
	HLSLINCLUDE
	
	#include "../../../Shaders/StdLib.hlsl"
	#include "../../../Shaders/XPostProcessing.hlsl"
	

	#pragma shader_feature USING_Frequency_INFINITE

	half3 _Params;
	#define _Frequency _Params.x
	#define _Amount _Params.y
	#define _Speed _Params.z

	
	float4 RGBSplit_Horizontal(float2 uv, float Amount, float time)
	{
		Amount *= 0.001;
		float3 splitAmountX = float3(uv.x, uv.x, uv.x);
		splitAmountX.r += sin(time * 0.2) * Amount;
		splitAmountX.g += sin(time * 0.1) * Amount;
		half4 splitColor = half4(0.0, 0.0, 0.0, 0.0);
		splitColor.r = (SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(splitAmountX.r, uv.y)).rgb).x;
		splitColor.g = (SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(splitAmountX.g, uv.y)).rgb).y;
		splitColor.b = (SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(splitAmountX.b, uv.y)).rgb).z;
		splitColor.a = 1;
		return splitColor;
	}
	
	float4 RGBSplit_Vertical(float2 uv, float Amount, float time)
	{
		Amount *= 0.001;
		float3 splitAmountY = float3(uv.y, uv.y, uv.y);
		splitAmountY.r += sin(time * 0.2) * Amount;
		splitAmountY.g += sin(time * 0.1) * Amount;
		half4 splitColor = half4(0.0, 0.0, 0.0, 0.0);
		splitColor.r = (SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(uv.x, splitAmountY.r)).rgb).x;
		splitColor.g = (SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(uv.x, splitAmountY.g)).rgb).y;
		splitColor.b = (SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(uv.x, splitAmountY.b)).rgb).z;
		splitColor.a = 1;
		return splitColor;
	}

	float4 RGBSplit_Horizontal_Vertical(float2 uv, float Amount, float time)
	{
		Amount *= 0.001;
		//float3 splitAmount = float3(uv.y, uv.y, uv.y);
		float splitAmountR= sin(time * 0.2) * Amount;
		float splitAmountG= sin(time * 0.1) * Amount;
		half4 splitColor = half4(0.0, 0.0, 0.0, 0.0);
		splitColor.r = (SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(uv.x + splitAmountR,uv.y +splitAmountR)).rgb).x;
		splitColor.g = (SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(uv.x , uv.y)).rgb).y;
		splitColor.b = (SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(uv.x + splitAmountG, uv.y + splitAmountG)).rgb).z;
		splitColor.a = 1;
		return splitColor;
	}
	
	
	float4 Frag_Horizontal(VaryingsDefault i): SV_Target
	{
		half strength = 0;
		#if USING_Frequency_INFINITE
			strength = 1;
		#else
			strength = 0.5 + 0.5 *cos(_Time.y * _Frequency);
		#endif
		half3 color = RGBSplit_Horizontal(i.texcoord.xy, _Amount * strength, _Time.y * _Speed).rgb;

		return half4(color, 1);
	}
	
	float4 Frag_Vertical(VaryingsDefault i): SV_Target
	{

		half strength = 0;
		#if USING_Frequency_INFINITE
			strength = 1;
		#else
			strength = 0.5 + 0.5 *cos(_Time.y * _Frequency);
		#endif
		half3 color = RGBSplit_Vertical(i.texcoord.xy, _Amount * strength, _Time.y * _Speed).rgb;

		return half4(color, 1);

	}

	float4 Frag_Horizontal_Vertical(VaryingsDefault i) : SV_Target
	{

		half strength = 0;
		#if USING_Frequency_INFINITE
			strength = 1;
		#else
			strength = 0.5 + 0.5 *cos(_Time.y * _Frequency);
		#endif
		half3 color = RGBSplit_Horizontal_Vertical(i.texcoord.xy, _Amount * strength, _Time.y * _Speed).rgb;

		return half4(color, 1);

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
			#pragma fragment Frag_Horizontal_Vertical

			ENDHLSL

		}
	}
}


