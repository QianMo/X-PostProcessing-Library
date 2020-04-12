//----------------------------------------------------------------------------------------------------------
// X-PostProcessing Library
// created by QianMo @ 2020
//----------------------------------------------------------------------------------------------------------

Shader "Hidden/X-PostProcessing/PixelizeSector"
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
			
			
			
			float4 _Params;
			float _PixelIntervalX;
			float _PixelIntervalY;
			half4 _BackgroundColor;

			//float2 CirclePixelizeUV(float2 uv)
			//{
			//	float pixelScale = 1.0 / _Params.x;

			//	half2 coord = half2(pixelScale *_PixelIntervalX* floor(uv.x / (pixelScale * _PixelIntervalX)), (pixelScale * _PixelIntervalY) * floor(uv.y / (pixelScale * _PixelIntervalY)));

			//	return coord;
			//}
			
			float4 SectorPixelize(float2 uv)
			{
				float pixelScale = 1.0 / _Params.x;

				float ratio = _ScreenParams.y / _ScreenParams.x;
				uv.x = uv.x / ratio;

				//x和y坐标分别除以缩放系数，在用floor向下取整，再乘以缩放系数，得到分段UV
				float2 coord = half2(_PixelIntervalX *  floor(uv.x / (pixelScale * _PixelIntervalX)), (_PixelIntervalY)* floor(uv.y / (pixelScale * _PixelIntervalY)));

				//设定扇形坐标
				float2 circleCenter = coord * pixelScale;

				//计算当前uv值隔圆心的距离，并乘以缩放系数
				float dist = length(uv - circleCenter) * _Params.x;
				//圆心坐标乘以缩放系数
				circleCenter.x *= ratio;

				//采样
				float4 screenColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, circleCenter);

				//对于距离大于半径的像素，替换为背景色
				if (dist > _Params.z)  screenColor = _BackgroundColor;

				return screenColor;
			}

			
			
			float4 Frag(VaryingsDefault i): SV_Target
			{

					return SectorPixelize(i.texcoord);

					////float circleSize = 1.0 / _Params.x;

					////校正UV，防止UV因分辨率因长宽比而形变
					////float aspect = _ScreenParams.y / _ScreenParams.x;

					//float2 uv = i.texcoord;
					////调整UV长宽比
					////uv.x = uv.x * _PixelInterval;
					////uv.x = uv.x;

					////获取分段UV
					//float2 circlePixelizeUV = CirclePixelizeUV(uv);

					//float dist = length(uv - circlePixelizeUV) * _Params.x;

					////circlePixelizeUV.x /= _ScreenParams.x / _ScreenParams.y;

					//float4 screenColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, circlePixelizeUV);

					//if (dist > _Params.z)
					//{
					//	screenColor = _BackgroundColor;
					//}
					//return screenColor;
			}
			
			ENDHLSL
			
		}
	}
}
