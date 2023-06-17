
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------


// reference 1 : https://www.shadertoy.com/view/4d2Xzw
// reference 2 : https://github.com/imclab/TiltShift

Shader "Hidden/X-PostProcessing/TiltShiftBlurV2"
{
	HLSLINCLUDE
	
	#include "../../../Shaders/StdLib.hlsl"
	#include "../../../Shaders/XPostProcessing.hlsl"
	
	
	half3 _Gradient;
	half4 _GoldenRot;
	half4 _Distortion;
	half4 _Params;
	
	#define _Offset _Gradient.x
	#define _Area _Gradient.y
	#define _Spread _Gradient.z
	#define _CubicDistortion _Distortion.x
	#define _DistortionScale _Distortion.y
	#define _Iteration _Params.x
	#define _Radius _Params.y
	#define _PixelSize _Params.zw
	
	float TiltShiftMask(float2 uv)
	{
		float centerY = uv.y * 2.0 - 1.0 + _Offset; // [0,1] -> [-1,1]
		return pow(abs(centerY * _Area), _Spread);
	}
	
	half4 FragPreview(VaryingsDefault i): SV_Target
	{
		return TiltShiftMask(i.texcoord);
	}
	
	half4 TiltShiftBlur(VaryingsDefault i)
	{
		half2x2 rot = half2x2(_GoldenRot);
		half4 accumulator = 0.0;
		half4 divisor = 0.0;
		
		half r = 1.0;
		half2 angle = half2(0.0, _Radius * saturate(TiltShiftMask(i.texcoord)));
		
		for (int j = 0; j < _Iteration; j ++)
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
		return TiltShiftBlur(i);
	}
	
	ENDHLSL
	
	SubShader
	{
		Cull Off ZWrite Off ZTest Always
		
		// Pass 0 - Tilt Shift
		Pass
		{
			HLSLPROGRAM
			
			#pragma vertex VertDefault
			#pragma fragment Frag
			
			ENDHLSL
			
		}
		
		// Pass 1 - Preview
		Pass
		{
			HLSLPROGRAM
			
			#pragma vertex VertDefault
			#pragma fragment FragPreview
			
			ENDHLSL
			
		}
	}
}


