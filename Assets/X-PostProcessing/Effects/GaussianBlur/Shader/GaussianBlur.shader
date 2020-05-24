
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

Shader "Hidden/X-PostProcessing/GaussianBlur"
{
	HLSLINCLUDE
	
	#include "../../../Shaders/StdLib.hlsl"
	#include "../../../Shaders/XPostProcessing.hlsl"
	
	half4 _BlurOffset;
	
	struct v2f
	{
		float4 pos: POSITION;
		float2 uv: TEXCOORD0;	
		float4 uv01: TEXCOORD1;
		float4 uv23: TEXCOORD2;
		float4 uv45: TEXCOORD3;
	};
	
	v2f VertGaussianBlur(AttributesDefault v)
	{
		v2f o;
		o.pos = float4(v.vertex.xy, 0, 1);
		
		o.uv.xy = TransformTriangleVertexToUV(o.pos.xy);
		
		#if UNITY_UV_STARTS_AT_TOP
			o.uv = o.uv * float2(1.0, -1.0) + float2(0.0, 1.0);
		#endif
		o.uv = TransformStereoScreenSpaceTex(o.uv, 1.0);
		
		o.uv01 = o.uv.xyxy + _BlurOffset.xyxy * float4(1, 1, -1, -1);
		o.uv23 = o.uv.xyxy + _BlurOffset.xyxy * float4(1, 1, -1, -1) * 2.0;
		o.uv45 = o.uv.xyxy + _BlurOffset.xyxy * float4(1, 1, -1, -1) * 6.0;
		
		return o;
	}
	
	float4 FragGaussianBlur(v2f i): SV_Target
	{
		half4 color = float4(0, 0, 0, 0);
		
		color += 0.40 * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
		color += 0.15 * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv01.xy);
		color += 0.15 * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv01.zw);
		color += 0.10 * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv23.xy);
		color += 0.10 * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv23.zw);
		color += 0.05 * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv45.xy);
		color += 0.05 * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv45.zw);
		
		return color;
	}
	
	
	float4 FragCombine(VaryingsDefault i): SV_Target
	{
		return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoordStereo);
	}
	
	
	ENDHLSL
	
	SubShader
	{
		Cull Off ZWrite Off ZTest Always
		
		Pass
		{
			HLSLPROGRAM
			
			#pragma vertex VertGaussianBlur
			#pragma fragment FragGaussianBlur
			
			ENDHLSL
			
		}
		
		Pass
		{
			HLSLPROGRAM
			
			#pragma vertex VertDefault
			#pragma fragment FragCombine
			
			ENDHLSL
			
		}
	}
}


