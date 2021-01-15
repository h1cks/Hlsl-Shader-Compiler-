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
    
    float3 cameraDirection;
    float padding3;
    
    float3 shadowCameraPosition;
    float padding2;
}

       //========================================================================================================================

cbuffer PerFrameLightBuffer : register(b2)
{
    float4 diffuseLightColour;
    float3 diffuseLightDirection;
    float padding4;
    float4 ambientColour;
    float textureOffset;
    float SeaLevel;
    float SeaFadeLevel;
    float TextureHighlight;
}

//========================================================================================================================

const static int MAXMOTION = 13;

cbuffer MotionBuffer : register(b5)
{
    float4 WindDirection;

    struct BufferData
    {
        float4 offsetVector;
    } MotionData[MAXMOTION]; //size is 16 bytes
}



cbuffer DecalModifiers : register(b7)
{

}


