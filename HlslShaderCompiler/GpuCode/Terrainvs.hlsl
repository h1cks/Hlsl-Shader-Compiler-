#include "CommonDefinitions\\BufferDefinitions.hlsl"

struct VS_IN
{
    float4 pos : POSITION;
    float4 normal : NORMAL;
    float4 basetexureUV : TEXCOORD0;
    float4 texture1 : TEXCOORD1;
  

};

struct PS_IN
{
    float4 pos : SV_POSITION;
    float3 normal : NORMAL;
    float4 basetexureUV : TEXCOORD;
    float4 texture1UV : TEXCOORD1;
    // add new variable in for smoothstep
    float4 shadowTextureUV : TEXCOORD2;
    float4 decalTextureUV : POSITION;
};

// Vertex Shader
PS_IN VSMain(VS_IN input)
{
    PS_IN output; // = (PS_IN)0;

    output.basetexureUV = input.basetexureUV;
    output.texture1UV = input.texture1;
    
    output.basetexureUV.xy = input.pos.xz;

    output.normal = input.normal.xyz;
    //output.normal.w = input.pos.y - SeaLevel;
    
 //   output.texture1UV.w = smoothstep(input.basetexureUV.z, input.basetexureUV.w, output.texture1UV.w);

    output.basetexureUV.z = input.pos.y - SeaLevel;
    output.basetexureUV.w = input.normal.w;
    
    // input.basetexureUV.z and basetexureUV.W are now free for use 
    
    output.shadowTextureUV = mul(input.pos, shadowInverseMatrix);
    output.decalTextureUV = mul(input.pos, TerrainDecalInverseMatrix);
    output.decalTextureUV.xyz /= output.decalTextureUV.w; // if we lose accuracy on decals, we might need to move this back to PS.
    output.shadowTextureUV.xyz /= output.shadowTextureUV.w;
    output.pos = mul(input.pos, wvpMatrix);

    return output;
}


