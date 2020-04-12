//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// created by QianMo @ 2020
//----------------------------------------------------------------------------------------------------------


Shader "Hidden/X-PostProcessing/PixelizeTriangle"
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
			float _PixelRatio;
			float _PixelScaleX;
			float _PixelScaleY;
			//half4 _BackgroundColor;



			float2 TrianglePixelizeUV(float2 uv)
			{

				float2 pixelScale = _PixelSize * float2(_PixelScaleX, _PixelScaleY / _PixelRatio);

				//乘以缩放，向下取整，再除以缩放，得到分段UV
				float2 coord = floor(uv * pixelScale) / pixelScale;

				uv -= coord;
				uv *= pixelScale;

				//进行三角形像素偏移处理
				coord += 
					float2(step(1.0 - uv.y, uv.x) / (2.0 * pixelScale.x),//X
					step(uv.x, uv.y) / (2.0 * pixelScale.y)//Y
					);

				return  coord;
			}


			float4 Frag(VaryingsDefault i) : SV_Target
			{
				float2 uv = TrianglePixelizeUV(i.texcoord);

				return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);		
			}

			ENDHLSL

		}
	}
}
