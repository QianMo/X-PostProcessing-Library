
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

Shader "Hidden/X-PostProcessing/Glitch/ImageBlockV4"
{
	HLSLINCLUDE

	#include "../../../Shaders/StdLib.hlsl"
	#include "../../../Shaders/XPostProcessing.hlsl"

	uniform half4 _Params;
	#define _Speed _Params.x
	#define _BlockSize _Params.y
	#define _MaxOffsetX _Params.z
	#define _MaxOffsetY _Params.w


	inline float randomNoise(float2 seed)
	{
		return frac(sin(dot(seed * floor(_Time.y * _Speed), float2(127.1, 311.7))) * 43758.5453123);
	}


	inline float randomNoise(float seed)
	{
		return randomNoise(float2(seed, 1.0));
	}

	float4 Frag(VaryingsDefault i) : SV_Target
	{
		float2 block = randomNoise(floor(i.texcoord * _BlockSize));
		float OffsetX = pow(block.x, 8.0) * pow(block.x, 3.0) - pow(randomNoise(7.2341), 17.0) * _MaxOffsetX;
		float OffsetY = pow(block.x, 8.0) * pow(block.x, 3.0) - pow(randomNoise(7.2341), 17.0) * _MaxOffsetY;
		float4 r = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
		float4 g = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + half2(OffsetX * 0.05 * randomNoise(7.0), OffsetY*0.05*randomNoise(12.0)));
		float4 b = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord - half2(OffsetX * 0.05 * randomNoise(13.0), OffsetY*0.05*randomNoise(12.0)));

		return half4(r.x, g.g, b.z, (r.a + g.a + b.a));
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

