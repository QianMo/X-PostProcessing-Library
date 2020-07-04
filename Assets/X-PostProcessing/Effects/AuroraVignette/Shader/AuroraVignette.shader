
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

Shader "Hidden/X-PostProcessing/AuroraVignette"
{
	HLSLINCLUDE
	
	#include "../../../Shaders/StdLib.hlsl"
	#include "../../../Shaders/XPostProcessing.hlsl"
	
	uniform half _VignetteArea;
	uniform half _VignetteSmothness;
	uniform half _ColorChange;
	uniform half4 _Color;
	uniform half _TimeX;
	uniform half3 _ColorFactor;
	uniform half _Fading;
	
	half4 Frag(VaryingsDefault i): SV_Target
	{	
		float2 uv = i.texcoord;
		float2 uv0 = uv - float2(0.5 + 0.5 * sin(1.4 * 6.28 * uv.x + 2.8 * _TimeX), 0.5);
		float3 wave = float3(0.5 * (cos(sqrt(dot(uv0, uv0)) * 5.6) + 1.0), cos(4.62 * dot(uv, uv) + _TimeX), cos(distance(uv, float2(1.6 * cos(_TimeX * 2.0), 1.0 * sin(_TimeX * 1.7))) * 1.3));
		half waveFactor = dot(wave, _ColorFactor) / _ColorChange;
		half vignetteIndensity = 1.0 - smoothstep(_VignetteArea, _VignetteArea - 0.05 - _VignetteSmothness, length(float2(0.5, 0.5) - uv));
		half3 AuroraColor = half3
		(
			_ColorFactor.r * 0.5 * (sin(1.28 * waveFactor + _TimeX * 3.45) + 1.0),
			_ColorFactor.g * 0.5 * (sin(1.28 * waveFactor + _TimeX * 3.15) + 1.0),
			_ColorFactor.b * 0.4 * (sin(1.28 * waveFactor + _TimeX * 1.26) + 1.0)
		);
		half3 finalColor = lerp(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv).rgb, AuroraColor, vignetteIndensity * _Fading);
		return half4(finalColor, 1.0);
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


