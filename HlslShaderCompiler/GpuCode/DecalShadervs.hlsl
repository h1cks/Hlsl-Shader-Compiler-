#include "CommonDefinitions\\BufferDefinitions.hlsl"

//========================================================================================================================

struct VertexIn
{
    float4 pos : POSITION;
    float4 uv : TEXCOORD0;
    float4 settings : TEXCOORD1;
};

struct VertexOut
{
    float4 pos : SV_POSITION;
    float4 uv : TEXCOORD0;

};

// Vertex Shader
VertexOut VSMain(VertexIn input)
{
    VertexOut output; // = (VertexOut)0;
    input.pos.w = 1.0f;
    output.pos = mul(input.pos, TerrainDecalWVPMatrix);

    output.uv = input.uv;
    
    return output;
}

//========================================================================================================================


