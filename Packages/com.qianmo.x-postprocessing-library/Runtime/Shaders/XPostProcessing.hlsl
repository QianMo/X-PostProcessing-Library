

#include "Sampling.hlsl"

//Always present in every shader
TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex); //Present in every shader

TEXTURE2D_SAMPLER2D(_CameraDepthNormalsTexture, sampler_CameraDepthNormalsTexture);
float4 _MainTex_TexelSize;



#define fixed half
#define fixed2 half2
#define fixed3 half3
#define fixed4 half4
#define fixed4x4 half4x4
#define fixed3x3 half3x3
#define fixed2x2 half2x2
#define sampler2D_half sampler2D
#define sampler2D_float sampler2D
#define samplerCUBE_half samplerCUBE
#define samplerCUBE_float samplerCUBE


//------------------------------------------------------------------------------------------------------
// Blend Functions
//------------------------------------------------------------------------------------------------------


half4 BlendOperation_Burn(half4 Base, half4 Blend, half Opacity)
{
	half4 Out = 1.0 - (1.0 - Blend) / Base;
	Out = lerp(Base, Out, Opacity);
	return Out;
}


half4 BlendOperation_Darken(half4 Base, half4 Blend, half Opacity)
{
	half4 Out = min(Blend, Base);
	Out = lerp(Base, Out, Opacity);
	return Out;
}


half4 BlendOperation_Difference(half4 Base, half4 Blend, half Opacity)
{
	half4 Out = abs(Blend - Base);
	Out = lerp(Base, Out, Opacity);
	return Out;
}


half4 BlendOperation_Dodge(half4 Base, half4 Blend, half Opacity)
{
	half4 Out = Base / (1.0 - Blend);
	Out = lerp(Base, Out, Opacity);
	return Out;
}


half4 BlendOperation_Divide(half4 Base, half4 Blend, half Opacity)
{
	half4 Out = Base / (Blend + 0.000000000001);
	Out = lerp(Base, Out, Opacity);
	return Out;
}


half4 BlendOperation_Exclusion(half4 Base, half4 Blend, half Opacity)
{
	half4 Out = Blend + Base - (2.0 * Blend * Base);
	Out = lerp(Base, Out, Opacity);
	return Out;
}


half4 BlendOperation_HardLight(half4 Base, half4 Blend, half Opacity)
{
	float4 result1 = 1.0 - 2.0 * (1.0 - Base) * (1.0 - Blend);
	float4 result2 = 2.0 * Base * Blend;
	float4 zeroOrOne = step(Blend, 0.5);
	half4 Out = result2 * zeroOrOne + (1 - zeroOrOne) * result1;
	Out = lerp(Base, Out, Opacity);
	return Out;
}


half4 BlendOperation_HardMix(half4 Base, half4 Blend, half Opacity)
{
	half4 Out = step(1 - Base, Blend);
	Out = lerp(Base, Out, Opacity);
	return Out;
}


half4 BlendOperation_Lighten(half4 Base, half4 Blend, half Opacity)
{
	half4 Out = max(Blend, Base);
	Out = lerp(Base, Out, Opacity);
	return Out;
}


half4 BlendOperation_LinearBurn(half4 Base, half4 Blend, half Opacity)
{
	half4 Out = Base + Blend - 1.0;
	Out = lerp(Base, Out, Opacity);
	return Out;
}


half4 BlendOperation_LinearDodge(half4 Base, half4 Blend, half Opacity)
{
	half4 Out = Base + Blend;
	Out = lerp(Base, Out, Opacity);
	return Out;
}


half4 BlendOperation_LinearLight(half4 Base, half4 Blend, half Opacity)
{
	half4 Out = Blend < 0.5 ? max(Base + (2 * Blend) - 1, 0): min(Base + 2 * (Blend - 0.5), 1);
	Out = lerp(Base, Out, Opacity);
	return Out;
}


half4 BlendOperation_LinearLightAddSub(half4 Base, half4 Blend, half Opacity)
{
	half4 Out = Blend + 2.0 * Base - 1.0;
	Out = lerp(Base, Out, Opacity);
	return Out;
}


half4 BlendOperation_Multiply(half4 Base, half4 Blend, half Opacity)
{
	half4 Out = Base * Blend;
	Out = lerp(Base, Out, Opacity);
	return Out;
}


half4 BlendOperation_Negation(half4 Base, half4 Blend, half Opacity)
{
	half4 Out = 1.0 - abs(1.0 - Blend - Base);
	Out = lerp(Base, Out, Opacity);
	return Out;
}


half4 BlendOperation_Overlay(half4 Base, half4 Blend, half Opacity)
{
	half4 result1 = 1.0 - 2.0 * (1.0 - Base) * (1.0 - Blend);
	half4 result2 = 2.0 * Base * Blend;
	half4 zeroOrOne = step(Base, 0.5);
	half4 Out = result2 * zeroOrOne + (1 - zeroOrOne) * result1;
	Out = lerp(Base, Out, Opacity);
	return Out;
}


half4 BlendOperation_PinLight(half4 Base, half4 Blend, half Opacity)
{
	half4 check = step(0.5, Blend);
	half4 result1 = check * max(2.0 * (Base - 0.5), Blend);
	half4 Out = result1 + (1.0 - check) * min(2.0 * Base, Blend);
	Out = lerp(Base, Out, Opacity);
	return Out;
}


half4 BlendOperation_Screen(half4 Base, half4 Blend, half Opacity)
{
	half4 Out = 1.0 - (1.0 - Blend) * (1.0 - Base);
	Out = lerp(Base, Out, Opacity);
	return Out;
}


half4 BlendOperation_SoftLight(half4 Base, half4 Blend, half Opacity)
{
	half4 result1 = 2.0 * Base * Blend + Base * Base * (1.0 - 2.0 * Blend);
	half4 result2 = sqrt(Base) * (2.0 * Blend - 1.0) + 2.0 * Base * (1.0 - Blend);
	half4 zeroOrOne = step(0.5, Blend);
	half4 Out = result2 * zeroOrOne + (1 - zeroOrOne) * result1;
	Out = lerp(Base, Out, Opacity);
	return Out;
}

half4 BlendOperation_Subtract(half4 Base, half4 Blend, half Opacity)
{
	half4 Out = Base - Blend;
	Out = lerp(Base, Out, Opacity);
	return Out;
}


half4 BlendOperation_VividLight(half4 Base, half4 Blend, half Opacity)
{
	half4 result1 = 1.0 - (1.0 - Blend) / (2.0 * Base);
	half4 result2 = Blend / (2.0 * (1.0 - Base));
	half4 zeroOrOne = step(0.5, Base);
	half4 Out = result2 * zeroOrOne + (1 - zeroOrOne) * result1;
	Out = lerp(Base, Out, Opacity);
	return Out;
}

half4 BlendOperation_Overwrite(half4 Base, half4 Blend, half Opacity)
{
	half4 Out = lerp(Base, Blend, Opacity);
	return Out;
}




//------------------------------------------------------------------------------------------------------
// Generic functions
//------------------------------------------------------------------------------------------------------

float rand(float n)
{
	return frac(sin(n) * 13758.5453123 * 0.01);
}

float rand(float2 n)
{
	return frac(sin(dot(n, float2(12.9898, 78.233))) * 43758.5453);
}

float2 RotateUV(float2 uv, float rotation)
{
	float cosine = cos(rotation);
	float sine = sin(rotation);
	float2 pivot = float2(0.5, 0.5);
	float2 rotator = (mul(uv - pivot, float2x2(cosine, -sine, sine, cosine)) + pivot);
	return saturate(rotator);
}

float3 ChromaticAberration(TEXTURE2D_ARGS(tex, samplerTex), float4 texelSize, float2 uv, float amount)
{
	float2 direction = normalize((float2(0.5, 0.5) - uv));
	float3 distortion = float3(-texelSize.x * amount, 0, texelSize.x * amount);
	
	float red = SAMPLE_TEXTURE2D(tex, samplerTex, uv + direction * distortion.r).r;
	float green = SAMPLE_TEXTURE2D(tex, samplerTex, uv + direction * distortion.g).g;
	float blue = SAMPLE_TEXTURE2D(tex, samplerTex, uv + direction * distortion.b).b;
	
	return float3(red, green, blue);
}


/*
float3 PositionFromDepth(float depth, float2 uv, float4 inverseViewMatrix) {
	
	float4 clip = float4((uv.xy * 2.0f - 1.0f) * float2(1, -1), 0.0f, 1.0f);
	float3 worldDirection = mul(inverseViewMatrix, clip) - _WorldSpaceCameraPos;
	
	float3 worldspace = worldDirection * depth + _WorldSpaceCameraPos;
	
	return float3(frac((worldspace.rgb)) + float3(0, 0, 0.1));
}
*/

// (returns 1.0 when orthographic)
float CheckPerspective(float x)
{
	return lerp(x, 1.0, unity_OrthoParams.w);
}

// Reconstruct view-space position from UV and depth.
float3 ReconstructViewPos(float2 uv, float depth)
{
	float3 worldPos = float3(0, 0, 0);
	worldPos.xy = (uv.xy * 2.0 - 1.0 - float2(unity_CameraProjection._13, unity_CameraProjection._23)) / float2(unity_CameraProjection._11, unity_CameraProjection._22) * CheckPerspective(depth);
	worldPos.z = depth;
	return worldPos;
}

float2 FisheyeUV(half2 uv, half amount, half zoom)
{
	half2 center = uv.xy - half2(0.5, 0.5);
	half CdotC = dot(center, center);
	half f = 1.0 + CdotC * (amount * sqrt(CdotC));
	return f * zoom * center + 0.5;
}

float2 Distort(float2 uv)
{
	#if DISTORT
		{
			uv = (uv - 0.5) * _Distortion_Amount.z + 0.5;
			float2 ruv = _Distortion_CenterScale.zw * (uv - 0.5 - _Distortion_CenterScale.xy);
			float ru = length(float2(ruv));
			
			UNITY_BRANCH
			if (_Distortion_Amount.w > 0.0)
			{
				float wu = ru * _Distortion_Amount.x;
				ru = tan(wu) * (1.0 / (ru * _Distortion_Amount.y));
				uv = uv + ruv * (ru - 1.0);
			}
			else
			{
				ru = (1.0 / ru) * _Distortion_Amount.x * atan(ru * _Distortion_Amount.y);
				uv = uv + ruv * (ru - 1.0);
			}
		}
	#endif
	
	return uv;
}

//----------------------------------------------------------------
// Common vertex functions
//--------------------------------------------------------------

float4 _BlurOffsets;

struct v2fGaussian
{
	float4 pos: POSITION;
	float2 uv: TEXCOORD0;
	
	float4 uv01: TEXCOORD1;
	float4 uv23: TEXCOORD2;
	float4 uv45: TEXCOORD3;
};

v2fGaussian VertGaussian(AttributesDefault v)
{
	v2fGaussian o;
	o.pos = float4(v.vertex.xy, 0, 1);
	
	o.uv.xy = TransformTriangleVertexToUV(o.pos.xy);
	
	#if UNITY_UV_STARTS_AT_TOP
		o.uv = o.uv * float2(1.0, -1.0) + float2(0.0, 1.0);
	#endif
	//UNITY_SINGLE_PASS_STEREO
	o.uv = TransformStereoScreenSpaceTex(o.uv, 1.0);
	
	o.uv01 = o.uv.xyxy + _BlurOffsets.xyxy * float4(1, 1, -1, -1);
	o.uv23 = o.uv.xyxy + _BlurOffsets.xyxy * float4(1, 1, -1, -1) * 2.0;
	o.uv45 = o.uv.xyxy + _BlurOffsets.xyxy * float4(1, 1, -1, -1) * 6.0;
	
	return o;
}

float4 FragBlurBox(VaryingsDefault i): SV_Target
{
	return DownsampleBox4Tap(TEXTURE2D_PARAM(_MainTex, sampler_MainTex), i.texcoord, _BlurOffsets.xy).rgba;
}

float4 FragBlurGaussian(v2fGaussian i): SV_Target
{
	half4 color = float4(0, 0, 0, 0);
	
	color += 0.40 * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
	color += 0.15 * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv01.xy);
	color += 0.15 * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv01.zw);
	color += 0.10 * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv23.xy);
	color += 0.10 * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv23.zw);
	color += 0.05 * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv45.xy);
	color += 0.05 * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv45.zw);
	
	return color;
}



half simpleNoise(half x, half y, half seed, half phase)
{
	half n = x * y * phase * seed;
	return fmod(n, 13) * fmod(n, 123);
}



half3 Lut2D(TEXTURE2D_ARGS(tex, samplerTex), float3 uvw, float2 texelSize, half tileAmount)
{
	uvw.z *= tileAmount;
	float shift = floor(uvw.z);
	uvw.xy = uvw.xy * tileAmount * texelSize.xy + texelSize.xy * 0.5;
	uvw.x += shift * texelSize.y;
	uvw.xyz = lerp(
		SAMPLE_TEXTURE2D(tex, samplerTex, uvw.xy).rgb,
		SAMPLE_TEXTURE2D(tex, samplerTex, uvw.xy + float2(texelSize.y, 0.0)).rgb,
		uvw.z - shift
	);
	return uvw;
}


half3 Lut2D_InvertY(TEXTURE2D_ARGS(tex, samplerTex), float3 uvw, float2 texelSize, half tileAmount)
{
	// Strip format where `height = sqrt(width)`
	uvw.z *= tileAmount;
	float shift = floor(uvw.z);
	uvw.xy = uvw.xy * tileAmount * texelSize.xy + texelSize.xy * 0.5;
	uvw.x += shift * texelSize.y;
	//uvw.y = 1 - uvw.y;
	uvw.xyz = lerp(
		SAMPLE_TEXTURE2D(tex, samplerTex, uvw.xy).rgb,
		SAMPLE_TEXTURE2D(tex, samplerTex, uvw.xy + float2(texelSize.y, 0.0)).rgb,
		uvw.z - shift
	);
	return uvw;
}

//-------------------------------------------------------------------------------------------
// Lift, Gamma (pre-inverted), Gain tuned for HDR use - best used with the ACES tonemapper as
// negative values will creep in the result
// Expected workspace: ACEScg (linear)
//-------------------------------------------------------------------------------------------
half3 LiftGammaGain_HDR(half3 c, half3 lift, float3 invgamma, half3 gain)
{
	c = c * gain + lift;
	
	// ACEScg will output negative values, as clamping to 0 will lose precious information we'll
	// mirror the gamma function instead
	return FastSign(c) * pow(abs(c), invgamma);
}

half3 Luminance_V1(half3 color)
{
	return(color.r * 0.3 + color.g * 0.59 + color.b * 0.11);
}

half Luminance_V2(half3 color)
{
	return dot(color, half3(0.222, 0.707, 0.071));
}

half4 LuminanceThreshold(half4 color, half threshold)
{
	half br = Max3(color.r, color.g, color.b);

	half contrib = max(0, br - threshold);

	contrib /= max(br, 0.001);

	return color * contrib;
}



float4 GetDepthNormal_ViewSpace(float2 uv)
{
	float4 cdn = SAMPLE_TEXTURE2D(_CameraDepthNormalsTexture, sampler_CameraDepthNormalsTexture, uv);
	float4 Normal_ViewSpace = float4(DecodeViewNormalStereo(cdn), 1);
	return Normal_ViewSpace;
}


float GetSinusoidWave(float len, float pi, float time)
{
	float wave = sin(8.0f * pi * len + time);
	wave = 0.5 * wave + 0.2;
	wave *= wave * wave;
	return wave;
}

