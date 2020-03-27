

// accuracy to 5 samples
bool IsInDepth(SamplerComparisonState samShadow,
	   	       Texture2D shadowMap,
			   float4 shadowPosH)
{
    float percentLit = 0.0f;


	// Complete projection by doing division by w.
    shadowPosH.xyz /= shadowPosH.w;

	// Depth in NDC space.
    float depth = shadowPosH.z - 0.001f;

	percentLit += shadowMap.SampleCmpLevelZero(samShadow, shadowPosH.xy , depth).r;
    
	if (percentLit > 0.0f)
		return false;


	return true;	
}



