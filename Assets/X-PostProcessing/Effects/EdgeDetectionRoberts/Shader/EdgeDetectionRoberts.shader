
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

Shader "Hidden/X-PostProcessing/EdgeDetectionRoberts"
{
	
	HLSLINCLUDE
	
	#include "../../../Shaders/StdLib.hlsl"
	#include "../../../Shaders/XPostProcessing.hlsl"
	
	
	half2 _Params;
	half4 _EdgeColor;
	half4 _BackgroundColor;

	#define _EdgeWidth _Params.x
	#define _BackgroundFade _Params.y

	
	float intensity(in float4 color)
	{
		return sqrt((color.x * color.x) + (color.y * color.y) + (color.z * color.z));
	}
	
	float sobel(float stepx, float stepy, float2 center)
	{
		// get samples around pixel
		float topLeft = intensity(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, center + float2(-stepx, stepy)));
		float bottomLeft = intensity(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, center + float2(-stepx, -stepy)));
		float topRight = intensity(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, center + float2(stepx, stepy)));
		float bottomRight = intensity(SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, center + float2(stepx, -stepy)));
		
		// Roberts Operator
		//X = -1   0      Y = 0  -1
		//     0   1          1   0
		
		// Gx = sum(kernelX[i][j]*image[i][j])
		float Gx = -1.0 * topLeft + 1.0 * bottomRight;
		
		// Gy = sum(kernelY[i][j]*image[i][j]);
		float Gy = -1.0 * topRight + 1.0 * bottomLeft;
		
		
		float sobelGradient = sqrt((Gx * Gx) + (Gy * Gy));
		return sobelGradient;
	}
	
	
	
	half4 Frag(VaryingsDefault i): SV_Target
	{
		
		half4 sceneColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
		
		float sobelGradient = sobel(_EdgeWidth / _ScreenParams.x, _EdgeWidth / _ScreenParams.y, i.texcoord);
		
		half4 backgroundColor = lerp(sceneColor, _BackgroundColor, _BackgroundFade);
		
		float3 edgeColor = lerp(backgroundColor.rgb, _EdgeColor.rgb, sobelGradient);
		
		return float4(edgeColor, 1);
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
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	//HLSLINCLUDE
	//
	//#include "../../../Shaders/StdLib.hlsl"
	//#include "../../../Shaders/XPostProcessing.hlsl"
	//
	//
	//
	//half _Float1;
	//half _Float2;
	//half _Float3;
	//half4 _Color1;
	//
	
	//struct v2f
	//{
		//	float2 uvRoberts[5] : TEXCOORD0;
		
		//	float4 vertex : SV_POSITION;
		//};
		
		
		//v2f vert_Roberts(appdata v)
		//{
			//	v2f o;
			//	o.vertex = UnityObjectToClipPos(v.vertex);
			//	o.uvRoberts[0] = v.uv + float2(-1, -1) * _MainTex_TexelSize * _SampleRange;
			//	o.uvRoberts[1] = v.uv + float2(1, -1) * _MainTex_TexelSize * _SampleRange;
			//	o.uvRoberts[2] = v.uv + float2(-1, 1) * _MainTex_TexelSize * _SampleRange;
			//	o.uvRoberts[3] = v.uv + float2(1, 1) * _MainTex_TexelSize * _SampleRange;
			//	o.uvRoberts[4] = v.uv;
			//	return o;
			//}
			
			//float Roberts(v2f i)
			//{
				//	const float Gx[4] =
				//	{
					//		-1,  0,
					//		0,  1
					//	};
					
					//	const float Gy[4] =
					//	{
						//		0, -1,
						//		1,  0
						//	};
						
						//	float edgex, edgey;
						//	for (int j = 0; j < 4; j++)
						//	{
							//		fixed4 col =  SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uvRoberts[j]);
							//		float lum = Luminance(col.rgb);
							
							//		edgex += lum * Gx[j];
							//		edgey += lum * Gy[j];
							//	}
							//	return 1 - abs(edgex) - abs(edgey);
							//}
							
							//fixed4 frag_Roberts(v2f i) : SV_Target
							//{
								//	fixed4 col =  SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uvRoberts[4]);
								//	float g = Roberts(i);
								//	g = pow(g, _EdgePower);
								//	col.rgb = lerp(_EdgeColor, _NonEdgeColor, g);
								
								//	return col;
								//}
								
								
								////half4 Frag(VaryingsDefault i): SV_Target
								////{
									////
									////	half4 sceneColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
									
									////	half3 col = 0.5 + 0.5 * cos(_Time.y + i.texcoord.xyx + float3(0, 2, 4));
									////
									////	half3 finalColor = lerp(sceneColor.rgb, col, _Float1 * 0.1);
									////
									////	return half4(finalColor, 1.0);
									////}
									////
									//ENDHLSL
									//
									
									//SubShader
									//{
										//	Cull Off ZWrite Off ZTest Always
										//
										//	Pass
										//	{
											//		HLSLPROGRAM
											//
											//		#pragma vertex vert_Roberts
											//		#pragma fragment frag_Roberts
											//
											//		ENDHLSL
											//
											//	}
											//}
										}
										
										
