//========================================================================================================================
// PLEASE BIND THIS BUFFER TO THE CORRECT STAGE
cbuffer perFrameBuffer : register(b0)
{
    row_major matrix wvpMatrix;
    row_major matrix worldMatrix;

    row_major matrix TerrainDecalWVPMatrix;
    row_major matrix TerrainDecalInverseMatrix;
    row_major matrix WaterDecalWVPMatrix;
    row_major matrix WaterDecalWVPInverseMatrix;

    row_major matrix shadowWVPMatrix;
    row_major matrix shadowWorldMatrix;
    row_major matrix shadowInverseMatrix;
    
    row_major matrix AirBillboardMatrix;
    row_major matrix WaterBillboardMatrix;

    row_major matrix TextWriterMatrix;

    row_major matrix MinimapMatrix;
    row_major matrix WorldMapMatrix;

    float3 cameraPosition;
    float padding;

    float3 shadowCameraPosition;
    float padding2;
}

cbuffer PerFrameLightBuffer : register(b2)
{
    float4 diffuseLightColour;
    float3 diffuseLightDirection;
    float padding4;
    float4 ambientColour;
    float textureOffset;
    float SeaLevel;
    float SeaFadeLevel;
    float SpecularLocation;
}

const static int MAXMOTION = 13;

cbuffer MotionBuffer: register(b5)
{
    float4 WindDirection;

    struct BufferData
    {
        float4 offsetVector;
    } MotionData[MAXMOTION]; //size is 16 bytes
}


const static int MAXTEXTUREMATERIALS = 64;
                                                                                   
cbuffer MaterialBuffer : register(b4)
{
	struct MaterialType {
		float1 specularValue;
		float1 padding12;
		float1 TextureIndex;
		float1 padding14;
		// pad 12 bytes
	} Material[MAXTEXTUREMATERIALS]; //size is 16 bytes
}

struct WaveData {
	float1 Length;
	float1 Speed;    // starts a new vector
	float1 Amplitude;    // starts a new vector
	float1 Steep;    // starts a new vector
};

//everything aligns to 16 byte boundaries

cbuffer WaterBuffer : register(b3)
{
    float3 OffsetVector;
    float Time;
    float Oscellator;
    float SteepnessMult;
    float AmplitudeMult;
    float GeometryMult; // 32
    
    // 304 - 32 = 272 / 3272/3

    WaveData Wave[8];

	float2 WaveDirections[8]; //64

	//waveData waves[8];

}

cbuffer DecalModifiers : register(b7)
{

}