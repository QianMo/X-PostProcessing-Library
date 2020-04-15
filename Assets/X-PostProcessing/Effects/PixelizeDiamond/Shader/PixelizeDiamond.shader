//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// created by QianMo @ 2020
//----------------------------------------------------------------------------------------------------------

Shader "Hidden/X-PostProcessing/PixelizeDiamond"
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
			
			
			
			float _PixelSize;
			
			
			float2 DiamondPixelizeUV(float2 uv)
			{
				half2 pixelSize = 10 / _PixelSize;
				
				half2 coord = uv * pixelSize;
				
				//计算当前Diamond的朝向
				int direction = int(dot(frac(coord), half2(1, 1)) >= 1.0) + 2 * int(dot(frac(coord), half2(1, -1)) >= 0.0);
				
				//进行向下取整
				coord = floor(coord);
				
				//处理Diamond的四个方向
				if (direction == 0) coord += half2(0, 0.5);
				if(direction == 1) coord += half2(0.5, 1);
				if(direction == 2) coord += half2(0.5, 0);
				if(direction == 3) coord += half2(1, 0.5);
				
				//最终缩放uv
				coord /= pixelSize;
				
				return coord;
			}
			
			
			
			float4 Frag(VaryingsDefault i): SV_Target
			{
				float2 uv = DiamondPixelizeUV(i.texcoord);
				
				return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
			}
			
			ENDHLSL
			
		}
	}
}
