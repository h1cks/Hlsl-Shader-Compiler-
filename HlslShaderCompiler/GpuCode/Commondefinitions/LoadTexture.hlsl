
static const float textureMultiplier = 1.0f / 512.0f;

//===========================================================================================================================

Texture2D HeightMap : register(t1);
//===========================================================================================================================

float CalculateWind(int posX, int posY)
{
    

    float xPos = posX * textureMultiplier;
    float yPos = posY * textureMultiplier;

    posX = frac(xPos) * 511;
    posY = frac(yPos) * 511; 

    float4 val = HeightMap.Load(int3(posX, posY, 0));
    
    return (val.r);
}