
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// created by QianMo @ 2020
//----------------------------------------------------------------------------------------------------------

Shader "Hidden/X-PostProcessing/DirectionalBlur"
{
	HLSLINCLUDE
	
	#include "../../../Shaders/StdLib.hlsl"
	#include "../../../Shaders/XPostProcessing.hlsl"

	uniform half _Iteration;
	uniform half2 _Direction;
	
	half4 DirectionalBlur(VaryingsDefault i)
	{
		half4 color = half4(0.0, 0.0, 0.0, 0.0);

		for (int k = -_Iteration; k < _Iteration; k++)
		{
			color += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord - _Direction * k);
		}
		half4 finalColor = color / (_Iteration * 2.0);

		return finalColor;
	}

	half4 Frag(VaryingsDefault i): SV_Target
	{
		return DirectionalBlur(i);
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

    
