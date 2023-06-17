

float3 BlendAdditive(float3 a, float3 b) 
{
	return a + b;
}

float3 BlendExclusion(float3 a, float3 b)
{
	return a + b - 2.0 * a * b;
}

float3 BlendLighten(float3 a, float3 b)
{
	return max(a, b);
}

//single channel overlay
float BlendOverlay(float a, float b)
{
	return (b < 0.5) ? 2.0 * a * b : 1.0 - 2.0 * (1.0 - a) * (1.0 - b);
}

//RGB overlay
float3 BlendOverlay(float3 a, float3 b)
{
	float3 color;
	color.r = BlendOverlay(a.r, b.r);
	color.g = BlendOverlay(a.g, b.g);
	color.b = BlendOverlay(a.b, b.b);
	return color;
}

float3 BlendScreen(float3 a, float3 b)
{
	return a + b - a * b;
}

