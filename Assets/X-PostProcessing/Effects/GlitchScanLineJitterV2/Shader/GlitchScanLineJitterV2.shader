
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

Shader "Hidden/X-PostProcessing/Glitch/ScanLineJitterV2"
{
	HLSLINCLUDE
	
	#include "../../../Shaders/StdLib.hlsl"
	#include "../../../Shaders/XPostProcessing.hlsl"
	
	
	#pragma shader_feature USING_FREQUENCY_INFINITE
	
	uniform half4 _Params;
	#define _Amount _Params.x
	#define _Speed _Params.y
	#define _Frequency _Params.z

	half3 RGB2YIQ(half3 c)
	{
		return half3
		(
			(0.2989 * c.x + 0.5959 * c.y + 0.2115 * c.z),
			(0.5870 * c.x - 0.2744 * c.y - 0.5229 * c.z),
			(0.1140 * c.x - 0.3216 * c.y + 0.3114 * c.z)
		);
	};
	
	half3 YIQ2RGB(half3 c)
	{
		return half3
		(
			(1.0 * c.x + 1.0 * c.y + 1.0 * c.z),
			(0.956 * c.x - 0.2720 * c.y - 1.1060 * c.z),
			(0.6210 * c.x - 0.6474 * c.y + 1.7046 * c.z)
		);
	};
	
	float randomNoise(float2 c)
	{
		return frac(sin(dot(c.xy, float2(12.9898, 78.233))) * 43758.5453);
	}
	
	float4 Jitter_Horizontal(float2 uv, float amount, float time)
	{
		amount *= 0.001;
		float3 offsetX = float3(uv.x, uv.x, uv.x);
		offsetX.r += sin(randomNoise(float2(time * 0.2, uv.y))) * amount;
		offsetX.g += sin(time * 9.0) * amount;
		half4 signal = half4(0.0, 0.0, 0.0, 0.0);
		signal.r = RGB2YIQ(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(offsetX.r, uv.y)).rgb).x;
		signal.g = RGB2YIQ(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(offsetX.g, uv.y)).rgb).y;
		signal.b = RGB2YIQ(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(offsetX.b, uv.y)).rgb).z;
		signal.a = 1;
		return signal;
	}
	
	float4 Jitter_Vertical(float2 uv, float amount, float time)
	{
		amount *= 0.001;
		float3 offsetY = float3(uv.y, uv.y, uv.y);
		offsetY.r += sin(randomNoise(float2(time * 0.2, uv.x))) * amount;
		offsetY.g += sin(time * 9.0) * amount;
		half4 signal = half4(0.0, 0.0, 0.0, 0.0);
		signal.r = RGB2YIQ(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(uv.x, offsetY.r)).rgb).x;
		signal.g = RGB2YIQ(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(uv.x, offsetY.g)).rgb).y;
		signal.b = RGB2YIQ(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(uv.x, offsetY.b)).rgb).z;
		signal.a = 1;
		return signal;
	}
	
	
	float4 Frag_Horizontal(VaryingsDefault i): SV_Target
	{
		float2 uv = i.texcoord.xy;
		
		half strength = 0;
		#if USING_FREQUENCY_INFINITE
			strength = 1;
		#else
			strength = 0.5 + 0.5 * cos(_Time.y * _Frequency);
		#endif
		
		//RGB- > YIP, ²¢¼ÆËãYIP¿Õ¼äÆ«ÒÆ¾àÀë
		half4 signal = Jitter_Horizontal(uv, _Amount * strength, _Time.y * _Speed);
		
		//YIP -> RGB
		half3 color = YIQ2RGB(signal.rgb);
		
		return half4(color, 1);
	}
	
	float4 Frag_Vertical(VaryingsDefault i): SV_Target
	{
		float2 uv = i.texcoord.xy;
		
		half strength = 0;
		#if USING_FREQUENCY_INFINITE
			strength = 1;
		#else
			strength = 0.5 + 0.5 * cos(_Time.y * _Frequency);
		#endif
		
		//RGB- > YIP, ²¢¼ÆËãYIP¿Õ¼äÆ«ÒÆ¾àÀë
		half4 signal = Jitter_Vertical(uv, _Amount * strength, _Time.y * _Speed);
		
		//YIP -> RGB
		half3 color = YIQ2RGB(signal.rgb);
		
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
	}
}


