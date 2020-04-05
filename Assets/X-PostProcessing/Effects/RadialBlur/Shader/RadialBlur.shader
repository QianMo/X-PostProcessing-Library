
//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// created by QianMo @ 2020
//----------------------------------------------------------------------------------------------------------

Shader "Hidden/X-PostProcessing/RadialBlur"
{
	HLSLINCLUDE

	#include "../../../Shaders/StdLib.hlsl"
	#include "../../../Shaders/XPostProcessing.hlsl"
	
	uniform half4 _Params;
	
	#define _BlurRadius _Params.x
	#define _Iteration _Params.y
	#define _RadialCenter _Params.zw
	
	
	half4 RadialBlur(VaryingsDefault i)
	{
		float2 blurVector = (_RadialCenter - i.texcoord.xy) * _BlurRadius;
		
		half4 acumulateColor = half4(0, 0, 0, 0);
		
		[unroll(30)]
		for (int j = 0; j < _Iteration; j ++)
		{
			acumulateColor += SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
			i.texcoord.xy += blurVector;
		}
		
		return acumulateColor / _Iteration;
	}
	
	half4 Frag(VaryingsDefault i): SV_Target
	{
		return RadialBlur(i);
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

