
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

Shader "Hidden/X-PostProcessing/ColorAdjustment/Hue"
{
	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			HLSLPROGRAM

			#pragma vertex VertDefault
			#pragma fragment Frag

			#include "../../../Shaders/StdLib.hlsl"
			#include "../../../Shaders/XPostProcessing.hlsl"

			
            
			uniform half _HueDegree;


			half3 Hue_Degree(float3 In, float Offset)
			{
				float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
				float4 P = lerp(float4(In.bg, K.wz), float4(In.gb, K.xy), step(In.b, In.g));
				float4 Q = lerp(float4(P.xyw, In.r), float4(In.r, P.yzx), step(P.x, In.r));
				float D = Q.x - min(Q.w, Q.y);
				float E = 1e-10;
				float3 hsv = float3(abs(Q.z + (Q.w - Q.y) / (6.0 * D + E)), D / (Q.x + E), Q.x);

				float hue = hsv.x + Offset / 360;
				hsv.x = (hue < 0)
					? hue + 1
					: (hue > 1)
					? hue - 1
					: hue;

				float4 K2 = float4(1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0);
				float3 P2 = abs(frac(hsv.xxx + K2.xyz) * 6.0 - K2.www);
				half3 Out = hsv.z * lerp(K2.xxx, saturate(P2 - K2.xxx), hsv.y);

				return Out;
			}




			half4 Frag(VaryingsDefault i) : SV_Target
			{

				//half3 col = 0.5 + 0.5 * cos(_Time.y + i.texcoord.xyx + float3(0, 2, 4));

				half4 sceneColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);

				//half3 finalColor = lerp(sceneColor.rgb, col, _Float1 *0.1);

				return half4(Hue_Degree(sceneColor, _HueDegree), 1.0);
			}

			ENDHLSL
		}
	}
}
        
