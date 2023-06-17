
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

Shader "Hidden/X-PostProcessing/RapidVignetteV2"
{
	
	HLSLINCLUDE
	
	#include "../../../Shaders/StdLib.hlsl"
	#include "../../../Shaders/XPostProcessing.hlsl"
	
	struct VertexOutput
	{
		float4 vertex: SV_POSITION;
		float4 texcoord: TEXCOORD0;
	};
	
	half _VignetteIndensity;
	half _VignetteSharpness;
	half2 _VignetteCenter;
	half4 _VignetteColor;
	
	
	float4 Frag(VertexOutput i): SV_Target
	{
		
		float4 sceneColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord.xy);
		
		half indensity = distance(i.texcoord.xy, _VignetteCenter.xy);
		indensity = smoothstep(0.8, _VignetteSharpness * 0.799, indensity * (_VignetteIndensity + _VignetteSharpness));
		return sceneColor * indensity;
	}
	
	
	float4 Frag_ColorAdjust(VertexOutput i): SV_Target
	{
		
		float4 sceneColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord.xy);
		
		half indensity = distance(i.texcoord.xy, _VignetteCenter.xy);
		indensity = smoothstep(0.8, _VignetteSharpness * 0.799, indensity * (_VignetteIndensity + _VignetteSharpness));
		
		half3 finalColor = lerp(_VignetteColor.rgb, sceneColor.rgb, indensity);
		
		return float4(finalColor.rgb, _VignetteColor.a);

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
		
		Pass
		{
			HLSLPROGRAM
			
			#pragma vertex VertDefault
			#pragma fragment Frag_ColorAdjust
			
			ENDHLSL
			
		}
	}
}
