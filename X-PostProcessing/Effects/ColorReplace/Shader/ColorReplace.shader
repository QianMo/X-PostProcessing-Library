
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// https://github.com/QianMo/X-PostProcessing-Library
// Copyright (C) 2020 QianMo. All rights reserved.
// Licensed under the MIT License 
// You may not use this file except in compliance with the License.You may obtain a copy of the License at
// http://opensource.org/licenses/MIT
//----------------------------------------------------------------------------------------------------------

Shader "Hidden/X-PostProcessing/ColorReplace"
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

			uniform half4 _FromColor;
			uniform half4 _ToColor;
			uniform half _Range;
			uniform half _Fuzziness;

			half3 ColorReplace(half3 In, half3 From, half3 To, half Range, half Fuzziness)
			{
				half Distance = distance(From, In);
				half3 Out = lerp(To, In, saturate((Distance - Range) / max(Fuzziness, 0.1)));
				return Out;
			}


			half4 Frag(VaryingsDefault i) : SV_Target
			{

				half4 sceneColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);

				half3 finalColor = ColorReplace(sceneColor.rgb, _FromColor.rgb , _ToColor.rgb , _Range, _Fuzziness);

				return half4(finalColor, 1.0);
			}

			ENDHLSL
		}
	}
}
        
