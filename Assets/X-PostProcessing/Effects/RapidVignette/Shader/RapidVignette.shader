
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

Shader "Hidden/X-PostProcessing/RapidVignette"
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
	half2 _VignetteCenter;
	half4 _VignetteColor;


	VertexOutput Vert(AttributesDefault v)
	{
		VertexOutput o;
		o.vertex = float4(v.vertex.xy, 0.0, 1.0);
		o.texcoord.xy = TransformTriangleVertexToUV(v.vertex.xy);

		#if UNITY_UV_STARTS_AT_TOP
			o.texcoord.xy = o.texcoord.xy * float2(1.0, -1.0) + float2(0.0, 1.0);
		#endif

		// uv [0, 1] ->[-0.5, 0.5]
		o.texcoord.zw = o.texcoord.xy - _VignetteCenter;

		return o;
	}

	float4 Frag(VertexOutput i): SV_Target
	{
		float4 finalColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord.xy);

		//求解vignette强度
		float vignetteIndensity = saturate(1.0 - dot(i.texcoord.zw, i.texcoord.zw) * _VignetteIndensity);

		return vignetteIndensity * finalColor;
	}


	float4 Frag_ColorAdjust(VertexOutput i): SV_Target
	{
		float4 finalColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord.xy);

		//求解vignette强度
		float vignetteIndensity = saturate(1.0 - dot(i.texcoord.zw, i.texcoord.zw) * _VignetteIndensity);

		//基于vignette强度，插值VignetteColor颜色和场景颜色
		finalColor.rgb = lerp(_VignetteColor.rgb, finalColor.rgb, vignetteIndensity);

		return half4(finalColor.rgb, _VignetteColor.a);
	}
	
	ENDHLSL
	
	SubShader
	{
		Cull Off ZWrite Off ZTest Always
		Pass
		{
			HLSLPROGRAM
			
			#pragma vertex Vert
			#pragma fragment Frag
			
			ENDHLSL
			
		}

		Pass
		{
			HLSLPROGRAM
			
			#pragma vertex Vert
			#pragma fragment Frag_ColorAdjust
			ENDHLSL
			
		}
	}
}

