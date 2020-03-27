
static const float TEXTURE_MAP_SIZE = 512.0f;
static const float TEXTURE_DX = 1.0 / TEXTURE_MAP_SIZE;

static const float2 decaloffsets[5] =
{
    float2(0.0f, -TEXTURE_DX), float2(-TEXTURE_DX, 0.0f), float2(0.0f, 0.0f), float2(TEXTURE_DX, 0.0f), float2(0.0f, +TEXTURE_DX)
};


float4 CalcTextSample(SamplerState sample, Texture2DArray textureMap, float3 textureUV)
{
    float4 colour = 0;

	[unroll (5)]
    for (int i = 0; i < 5; ++i)
    {
        colour += textureMap.Sample(sample, float3(textureUV.xy + decaloffsets[i], textureUV.z));
    }

    colour = colour * 0.2f;

    return colour; // base shading is 55% as (20 * 0.05f = 1.0f).   Division IS TERRIBLE	
}

//---------------------------------------------------------------------------------------
// Transforms a normal map sample to world space.
//---------------------------------------------------------------------------------------
float3 NormalSampleToWorldSpace(float3 normalMapSample, float3 unitNormalW, float3 tangentW)
{
    // Uncompress each component from [0,1] to [-1,1].
    float3 normalT = 2.0f * normalMapSample - 1.0f;

    // Build orthonormal basis.
    float3 N = unitNormalW;
    float3 T = normalize(tangentW - dot(tangentW, N) * N);
    float3 B = cross(N, T);

    float3x3 TBN = float3x3(T, B, N);

    // Transform from tangent space to world space.
    float3 bumpedNormalW = mul(normalT, TBN);

    return bumpedNormalW;
}

//======================================================================================================================

inline float3 BlendNormalMaps(float3 n1, float3 n2)
{
	//Blend normal maps

    float a = 1 / (1 + n1.z);
    float b = -n1.x * n1.y * a;

	// Form a basis
    float3 b1 = float3(1 - n1.x * n1.x * a, b, -n1.x);
    float3 b2 = float3(b, 1 - n1.y * n1.y * a, -n1.y);
    float3 b3 = n1;

    if (n1.z < -0.9999999) // Handle the singularity
    {
        b1 = float3(0, -1, 0);
        b2 = float3(-1, 0, 0);
    }

	// Rotate n2 via the basis
    float3 r = n2.x * b1 + n2.y * b2 + n2.z * b3;
    r = clamp(r, -1, 1);
    return r;
}



