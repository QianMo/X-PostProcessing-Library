
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

Shader "Hidden/X-PostProcessing/RadialBlurV2"
{
	
	HLSLINCLUDE
	
	#include "../../../Shaders/StdLib.hlsl"
	#include "../../../Shaders/XPostProcessing.hlsl"
	
	uniform half3 _Params;
	
	#define _BlurRadius _Params.x
	#define _RadialCenter _Params.yz
	
	half4 Frag_4Tap(VaryingsDefault i): SV_Target
	{
		
		float2 uv = i.texcoord - _RadialCenter;
		
		half scale = 1;
		half4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = _BlurRadius + 1;  //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 2 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 3 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		color *= 0.25f; // 1/4
		
		return  color;
	}
	
	
	half4 Frag_6Tap(VaryingsDefault i): SV_Target
	{
		
		float2 uv = i.texcoord - _RadialCenter;
		
		half scale = 1;
		half4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = _BlurRadius + 1;  //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 2 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 3 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 4 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 5 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		
		color *= 0.1667f; // 1/6
		
		return  color;
	}
	
	
	half4 Frag_8Tap(VaryingsDefault i): SV_Target
	{
		
		float2 uv = i.texcoord - _RadialCenter;
		
		half scale = 1;
		half4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = _BlurRadius + 1;  //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 2 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 3 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 4 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 5 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 6 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 7 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		color *= 0.125f;  // 1/8
		
		return  color;
	}
	
	half4 Frag_10Tap(VaryingsDefault i): SV_Target
	{
		
		float2 uv = i.texcoord - _RadialCenter;
		
		half scale = 1;
		half4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = _BlurRadius + 1;  //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 2 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 3 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 4 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 5 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 6 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 7 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 8 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 9 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		color *= 0.1f;  // 1/10
		
		return  color;
	}
	
	
	
	half4 Frag_12Tap(VaryingsDefault i): SV_Target
	{
		
		float2 uv = i.texcoord - _RadialCenter;
		
		half scale = 1;
		half4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = _BlurRadius + 1;  //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 2 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 3 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 4 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 5 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 6 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 7 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 8 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 9 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 10 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 11 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		color *= 0.0833f;  // 1/12
		
		return  color;
	}
	
	half4 Frag_20Tap(VaryingsDefault i): SV_Target
	{
		
		float2 uv = i.texcoord - _RadialCenter;
		
		half scale = 1;
		half4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = _BlurRadius + 1;  //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 2 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 3 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 4 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 5 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 6 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 7 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 8 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 9 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 10 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 11 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 12 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 13 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 14 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 15 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 16 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 17 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 18 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 19 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		
		color *= 0.05f;  // 1/20
		
		return  color;
	}
	
	
	half4 Frag_30Tap(VaryingsDefault i): SV_Target
	{
		
		float2 uv = i.texcoord - _RadialCenter;
		
		half scale = 1;
		half4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = _BlurRadius + 1;  //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 2 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 3 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 4 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 5 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 6 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 7 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 8 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 9 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 10 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 11 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 12 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 13 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 14 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 15 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 16 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 17 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 18 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 19 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 20 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 21 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 22 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 23 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 24 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 25 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 26 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 27 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 28 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		scale = 29 * _BlurRadius + 1; //1 MAD
		color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv * scale + _RadialCenter); //1 MAD
		
		color *= 0.0333f;  // 1/30
		
		return  color;
	}
	
	
	
	ENDHLSL
	
	SubShader
	{
		Cull Off ZWrite Off ZTest Always
		
		Pass
		{
			HLSLPROGRAM
			
			#pragma vertex VertDefault
			#pragma fragment Frag_4Tap
			
			ENDHLSL
			
		}
		
		
		Pass
		{
			HLSLPROGRAM
			
			#pragma vertex VertDefault
			#pragma fragment Frag_6Tap
			
			ENDHLSL
			
		}
		
		Pass
		{
			HLSLPROGRAM
			
			#pragma vertex VertDefault
			#pragma fragment Frag_8Tap
			
			ENDHLSL
			
		}
		
		Pass
		{
			HLSLPROGRAM
			
			#pragma vertex VertDefault
			#pragma fragment Frag_10Tap
			
			ENDHLSL
			
		}
		
		Pass
		{
			HLSLPROGRAM
			
			#pragma vertex VertDefault
			#pragma fragment Frag_12Tap
			
			ENDHLSL
			
		}
		
		Pass
		{
			HLSLPROGRAM
			
			#pragma vertex VertDefault
			#pragma fragment Frag_20Tap
			
			ENDHLSL
			
		}
		
		Pass
		{
			HLSLPROGRAM
			
			#pragma vertex VertDefault
			#pragma fragment Frag_30Tap
			
			ENDHLSL
			
		}
	}
}

