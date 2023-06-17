
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

Shader "Hidden/X-PostProcessing/SharpenV3"
{
	HLSLINCLUDE
	
	#include "../../../Shaders/StdLib.hlsl"
	#include "../../../Shaders/XPostProcessing.hlsl"
	
	uniform half _CentralFactor;
	uniform half _SideFactor;



	struct VertexOutput
	{
		float4 vertex: SV_POSITION;
		float2 texcoord: TEXCOORD0;
		float4 texcoord1  : TEXCOORD1;
	};

	VertexOutput Vert(AttributesDefault v)
	{
		VertexOutput o;
		o.vertex = float4(v.vertex.xy, 0.0, 1.0);
		o.texcoord = TransformTriangleVertexToUV(v.vertex.xy);
#if UNITY_UV_STARTS_AT_TOP
		o.texcoord = o.texcoord * float2(1.0, -1.0) + float2(0.0, 1.0);
#endif
		o.texcoord1 = half4(o.texcoord.xy - _MainTex_TexelSize.xy, o.texcoord.xy + _MainTex_TexelSize.xy);
		return o;
	}

	half4 Frag(VertexOutput i): SV_Target
	{
		//return i.texcoord1;
		half4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord.xy) * _CentralFactor;
		color -= SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord1.xy) * _SideFactor;
		color -= SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord1.xw) * _SideFactor;
		color -= SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord1.zy) * _SideFactor;
		color -= SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord1.zw) * _SideFactor;
		return color;
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
	}
}

    
