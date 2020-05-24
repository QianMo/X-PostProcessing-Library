
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

Shader "Hidden/X-PostProcessing/Glitch/ImageBlockV3"
{
	HLSLINCLUDE

	#include "../../../Shaders/StdLib.hlsl"
	#include "../../../Shaders/XPostProcessing.hlsl"

	half3 _Params;

	#define _Speed _Params.x
	#define _Density _Params.y
	#define _MaxDisplace _Params.z

	inline float randomNoise(float2 seed)
	{
		return frac(sin(dot(seed * floor(_Time.y * _Speed), float2(127.1, 311.7))) * 43758.5453123);
	}

	inline float randomNoise(float seed)
	{
		return rand(float2(seed, 1.0));
	}

	half4 Frag(VaryingsDefault i) : SV_Target
	{

		float2 block = randomNoise(floor(i.texcoord * _Density));

		half displaceNoise = pow(block.x, 8.0) * pow(block.x, 3.0) - pow(randomNoise(7.2341), 17.0) * _MaxDisplace;

		half ColorR = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord).r;
		half ColorG = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + float2(displaceNoise * 0.05 * rand(7.0), 0.0)).g;
		half ColorB = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord - float2(displaceNoise * 0.05 * rand(13.0), 0.0)).b;

		return half4(ColorR, ColorG, ColorB, 1.0);

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


