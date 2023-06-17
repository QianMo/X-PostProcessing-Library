
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

Shader "Hidden/X-PostProcessing/Glitch/AnalogNoise"
{
	HLSLINCLUDE
	
	#include "../../../Shaders/StdLib.hlsl"
	#include "../../../Shaders/XPostProcessing.hlsl"
	

	uniform half4 _Params;
	#define _Speed _Params.x
	#define _Fading _Params.y
	#define _LuminanceJitterThreshold _Params.z
	#define _TimeX _Params.w


	//uniform half _Fading;
	//uniform half _TimeX;
	//uniform half _LuminanceJitterThreshold;

	float randomNoise(float2 c)
	{
		return frac(sin(dot(c.xy, float2(12.9898, 78.233))) * 43758.5453);
	}

	half4 Frag(VaryingsDefault i): SV_Target
	{

		half4 sceneColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
		half4 noiseColor = sceneColor;

		half luminance = dot(noiseColor.rgb, fixed3(0.22, 0.707, 0.071));
		if (randomNoise(float2(_TimeX * _Speed, _TimeX * _Speed)) > _LuminanceJitterThreshold)
		{
			noiseColor = float4(luminance, luminance, luminance, luminance);
		}

		float noiseX = randomNoise(_TimeX * _Speed + i.texcoord / float2(-213, 5.53));
		float noiseY = randomNoise(_TimeX * _Speed - i.texcoord / float2(213, -5.53));
		float noiseZ = randomNoise(_TimeX * _Speed + i.texcoord / float2(213, 5.53));

		noiseColor.rgb += 0.25 * float3(noiseX,noiseY,noiseZ) - 0.125;

		noiseColor = lerp(sceneColor, noiseColor, _Fading);
		
		return noiseColor;
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

    
