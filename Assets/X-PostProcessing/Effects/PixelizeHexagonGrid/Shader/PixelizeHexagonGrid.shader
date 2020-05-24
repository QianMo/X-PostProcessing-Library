//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

//reference : 
//	1. https://www.shadertoy.com/view/ls23Dc
//	2. http://coding-experiments.blogspot.nl/2010/06/pixelation.html

Shader "Hidden/X-PostProcessing/PixelizeHexagonGrid"
{
	
	HLSLINCLUDE
	
	#include "../../../Shaders/StdLib.hlsl"
	#include "../../../Shaders/XPostProcessing.hlsl"
	
	half2 _Params;
	#define _PixelSize _Params.x
	#define _GridWidth _Params.y


	float HexDist(float2 a, float2 b)
	{
		float2 p = abs(b - a);
		float s = 0.5;
		float c = 0.8660254;
		
		float diagDist = s * p.x + c * p.y;
		return max(diagDist, p.x) / c;
	}
	
	float2 NearestHex(float s, float2 st)
	{
		float h = 0.5 * s;
		float r = 0.8660254 * s;
		float b = s + 2.0 * h;
		float a = 2.0 * r;
		float m = h / r;
		
		float2 sect = st / float2(2.0 * r, h + s);
		float2 sectPxl = fmod(st, float2(2.0 * r, h + s));
		
		float aSection = fmod(floor(sect.y), 2.0);
		
		float2 coord = floor(sect);
		if (aSection > 0.0)
		{
			if(sectPxl.y < (h - sectPxl.x * m))
			{
				coord -= 1.0;
			}
			else if(sectPxl.y < (-h + sectPxl.x * m))
			{
				coord.y -= 1.0;
			}
		}
		else
		{
			if(sectPxl.x > r)
			{
				if(sectPxl.y < (2.0 * h - sectPxl.x * m))
				{
					coord.y -= 1.0;
				}
			}
			else
			{
				if(sectPxl.y < (sectPxl.x * m))
				{
					coord.y -= 1.0;
				}
				else
				{
					coord.x -= 1.0;
				}
			}
		}
		
		float xoff = fmod(coord.y, 2.0) * r;
		return float2(coord.x * 2.0 * r - xoff, coord.y * (h + s)) + float2(r * 2.0, s);
	}
	
	
	

	float4 FragHexGrid(VaryingsDefault i) : SV_Target
	{
		//cal hexagon uv
		float pixelSize = _PixelSize * _ScreenParams.x * 0.2;
		float2 nearest = NearestHex(pixelSize, i.texcoord * _ScreenParams.xy);

		float4 finalColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, nearest / _ScreenParams.xy);

		float dist = HexDist(i.texcoord * _ScreenParams.xy, nearest);

		float interiorSize = pixelSize;
		float interior = 1.0 - smoothstep(interiorSize - 0.8, interiorSize, dist * _GridWidth);

		return float4(finalColor.rgb * interior, 1.0);

	}

	float2 HexPixelizeUV(float2 hexIndex)
	{
		int i = hexIndex.x;
		int j = hexIndex.y;
		float2 r;
		r.x = i * _Params.x;
		r.y = j * _Params.y + (i % 2.0) * _Params.y / 2.0;
		return r;
	}

	//Solve index
	float2 HexIndex(float2 uv, float size)
	{
		float2 r;

		int it = int(floor(uv.x / size));
		float yts = uv.y - float(it % 2.0) * _Params.y / 2.0;
		int jt = int(floor((1.0 / _Params.y) * yts));
		float xt = uv.x - it * size;
		float yt = yts - jt * _Params.y;
		int deltaj = (yt > _Params.y / 2.0) ? 1 : 0;
		float fcond = size * (2.0 / 3.0) * abs(0.5 - yt / _Params.y);

		if (xt > fcond)
		{
			r.x = it;
			r.y = jt;
		}
		else
		{
			r.x = it - 1;
			r.y = jt - (r.x % 2) + deltaj;
		}

		return r;
	}


	
	ENDHLSL
	
	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			HLSLPROGRAM

			#pragma vertex VertDefault
			#pragma fragment FragHexGrid		

			ENDHLSL

		}

	}
}
