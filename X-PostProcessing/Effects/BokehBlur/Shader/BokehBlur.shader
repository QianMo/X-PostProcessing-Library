
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

// reference : https://www.shadertoy.com/view/4d2Xzw

Shader "Hidden/X-PostProcessing/BokehBlur"
{
	HLSLINCLUDE
	
	#include "../../../Shaders/StdLib.hlsl"
	#include "../../../Shaders/XPostProcessing.hlsl"
	
	half4 _GoldenRot;
	half4 _Params;
	
	#define _Iteration _Params.x
	#define _Radius _Params.y
	#define _PixelSize _Params.zw
	
	half4 BokehBlur(VaryingsDefault i)
	{
		half2x2 rot = half2x2(_GoldenRot);
		half4 accumulator = 0.0;
		half4 divisor = 0.0;

		half r = 1.0;
		half2 angle = half2(0.0, _Radius);

		for (int j = 0; j < _Iteration; j++)
		{
			r += 1.0 / r;
			angle = mul(rot, angle);
			half4 bokeh = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(i.texcoord + _PixelSize * (r - 1.0) * angle));
			accumulator += bokeh * bokeh;
			divisor += bokeh;
		}
		return accumulator / divisor;
	}

	half4 Frag(VaryingsDefault i): SV_Target
	{
		return BokehBlur(i);
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


