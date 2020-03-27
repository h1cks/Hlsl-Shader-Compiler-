
static const float SMAP_SIZE = 3072.0f;
static const float SMAP_DX = 1.0f / SMAP_SIZE;



// accuracy to 5 samples
float CalcShadowFactor2(SamplerComparisonState samShadow,
					   Texture2D shadowMap,
				  	   float4 shadowPosH, float offset)
{
    float percentLit = 0.0f;

    static const float2 offsets[15] =
    {
        float2(0.0f, -SMAP_DX * 3), float2(-SMAP_DX * 3, 0.0f), float2(0.0f, 0.0f), float2(SMAP_DX * 3, 0.0f), float2(0.0f, +SMAP_DX * 3),
        float2(0.0f, -SMAP_DX * 2), float2(-SMAP_DX * 2, 0.0f), float2(0.0f, 0.0f), float2(SMAP_DX * 2, 0.0f), float2(0.0f, +SMAP_DX * 2),
        float2(0.0f, -SMAP_DX), float2(-SMAP_DX, 0.0f), float2(0.0f, 0.0f), float2(SMAP_DX, 0.0f), float2(0.0f, +SMAP_DX)
    };


	// Complete projection by doing division by w.
  //  shadowPosH.xyz /= shadowPosH.w;

	// Depth in NDC space.
    float depth = shadowPosH.z - 0.001f;
    static const float _divisor = 1.0f / 5.0f;
	
	[unroll (5)]
    for (int i = 0; i < 5; ++i)
    {       
        percentLit += shadowMap.SampleCmpLevelZero(samShadow, shadowPosH.xy + offsets[i + offset], depth).r;
    }

    return max(percentLit * _divisor, saturate((shadowPosH.z - 0.995f) * 200.0f));
    //return percentLit * 0.1f; // base shading is 55% as (20 * 0.05f = 1.0f).   Division IS TERRIBLE
}




// accuracy to 9
float CalcShadowFactor(SamplerComparisonState samShadow,
	Texture2D shadowMap,
	float4 shadowPosH,
	float offset)
{
	static const float2 hqoffsets[27] =
	{
		float2(-SMAP_DX * 3, -SMAP_DX * 3),  float2(SMAP_DX * 3, -SMAP_DX * 3), float2(-SMAP_DX * 3, +SMAP_DX * 3),  float2(SMAP_DX * 3, +SMAP_DX * 3), float2(0.0f, 0.0f),
		float2(SMAP_DX * 3, 0.0f), float2(0.0f, +SMAP_DX * 3),float2(0.0f, -SMAP_DX * 3),float2(-SMAP_DX * 3, 0.0f),

		float2(-SMAP_DX * 2, -SMAP_DX * 2),  float2(SMAP_DX * 2, -SMAP_DX * 2), float2(SMAP_DX * 2, +SMAP_DX * 2),  float2(-SMAP_DX * 2, +SMAP_DX * 2), float2(0.0f, 0.0f),
		float2(0.0f, +SMAP_DX * 2),  float2(0.0f, -SMAP_DX * 2), float2(-SMAP_DX * 2, 0.0f), float2(SMAP_DX * 2, 0.0f),

		float2(-SMAP_DX, -SMAP_DX), float2(SMAP_DX, -SMAP_DX), float2(-SMAP_DX, +SMAP_DX),  float2(SMAP_DX, +SMAP_DX), float2(0.0f, 0.0f),
		float2(0.0f, +SMAP_DX),  float2(0.0f, -SMAP_DX),  float2(-SMAP_DX, 0.0f),  float2(SMAP_DX, 0.0f),
	};

	// Depth in NDC space.
	float depth = shadowPosH.z - 0.001f;
    static const float _basevalue = 11;
    float percentLit = _basevalue;
    
    static const float _divisor = 1.0f / (9 + _basevalue);

	[unroll(9)]
	for (int i = 0; i < 9; ++i)
	{
		percentLit += shadowMap.SampleCmpLevelZero(samShadow, shadowPosH.xy + hqoffsets[i + offset], depth).r;
	}

    return max(percentLit * _divisor, saturate((shadowPosH.z - 0.995f) * 200.0f));
}


// accuracy to 9
float CalcShadowFactor3_retires(SamplerComparisonState samShadow,
	Texture2D shadowMap,
	float4 shadowPosH,
	float a_offset, float dot)
{
	static const float2 hqoffsets[27] =
	{
		float2(-SMAP_DX * 3, -SMAP_DX * 3),  float2(SMAP_DX * 3, -SMAP_DX * 3), float2(-SMAP_DX * 3, +SMAP_DX * 3),  float2(SMAP_DX * 3, +SMAP_DX * 3), float2(0.0f, 0.0f),
		float2(SMAP_DX * 3, 0.0f), float2(0.0f, +SMAP_DX * 3),float2(0.0f, -SMAP_DX * 3),float2(-SMAP_DX * 3, 0.0f),

		float2(-SMAP_DX * 2, -SMAP_DX * 2),  float2(SMAP_DX * 2, -SMAP_DX * 2), float2(SMAP_DX * 2, +SMAP_DX * 2),  float2(-SMAP_DX * 2, +SMAP_DX * 2), float2(0.0f, 0.0f),
		float2(0.0f, +SMAP_DX * 2),  float2(0.0f, -SMAP_DX * 2), float2(-SMAP_DX * 2, 0.0f), float2(SMAP_DX * 2, 0.0f),

		float2(-SMAP_DX, -SMAP_DX), float2(SMAP_DX, -SMAP_DX), float2(-SMAP_DX, +SMAP_DX),  float2(SMAP_DX, +SMAP_DX), float2(0.0f, 0.0f),
		float2(0.0f, +SMAP_DX),  float2(0.0f, -SMAP_DX),  float2(-SMAP_DX, 0.0f),  float2(SMAP_DX, 0.0f),
	};


	// Depth in NDC space.
	float depth = shadowPosH.z - 0.001f;
	float percentLit = 11.0f;

	percentLit += shadowMap.SampleCmpLevelZero(samShadow, shadowPosH.xy + hqoffsets[a_offset], depth).r;
	percentLit += shadowMap.SampleCmpLevelZero(samShadow, shadowPosH.xy + hqoffsets[a_offset + 1], depth).r;
	percentLit += shadowMap.SampleCmpLevelZero(samShadow, shadowPosH.xy + hqoffsets[a_offset + 2], depth).r;
	percentLit += shadowMap.SampleCmpLevelZero(samShadow, shadowPosH.xy + hqoffsets[a_offset + 3], depth).r;
	percentLit += shadowMap.SampleCmpLevelZero(samShadow, shadowPosH.xy + hqoffsets[a_offset + 4], depth).r;

	if (percentLit > 15)
	{
		return 1;
	}
	else
	{
		percentLit += shadowMap.SampleCmpLevelZero(samShadow, shadowPosH.xy + hqoffsets[a_offset + 5], depth).r;
		percentLit += shadowMap.SampleCmpLevelZero(samShadow, shadowPosH.xy + hqoffsets[a_offset + 6], depth).r;
		percentLit += shadowMap.SampleCmpLevelZero(samShadow, shadowPosH.xy + hqoffsets[a_offset + 7], depth).r;
		percentLit += shadowMap.SampleCmpLevelZero(samShadow, shadowPosH.xy + hqoffsets[a_offset + 8], depth).r;

		return max(percentLit * 0.05f, saturate((shadowPosH.z - 0.995f) * 200.0f));
	}
}