


static const float numWaves = 7;
const static float calcWave = 2.0 * 3.1416;

void GerstnerWave(float waveLength, float speed, float amplitude, float steepness, float2 direction, in float3 position, inout float3 result, inout float3 tangent, inout float3 bitangent, float aTime)
{
 //   direction = normalize(direction);
   // Wave wave = waves[i];
    float wi = calcWave / waveLength;
    float WA = wi * amplitude;
    float Qi = steepness / (WA * numWaves);
    float phi = speed * wi;
    float rad = dot(direction, position.xz) * wi + aTime * phi;

    float S0 = sin(rad);
    float C0 = cos(rad);
    result.y += S0 * amplitude;
    result.xz += C0 * amplitude * Qi * direction;
    
    S0 *= WA * Qi;
    C0 *= WA ;

    bitangent.x -=  direction.x * direction.x * S0;
    bitangent.z -=  direction.x * direction.y * S0;
    bitangent.y += direction.x * C0;

    tangent.x -= direction.x * direction.y * S0;
    tangent.z +=  direction.y * direction.y * S0;
    tangent.y += direction.y *C0;   
}



void GerstnerWaveTessendorf(float waveLength, float speed, float amplitude, float steepness, float2 direction, in float3 position, inout float3 result, inout float3 tangent, inout float3 bitangent)
{
    float waveMagnitude = calcWave / waveLength; // wave length
    float kA = waveMagnitude * amplitude;
    float2 D = normalize(direction); // normalized direction
    float2 directionWaveLength = D * waveMagnitude; // wave vector and magnitude (direction)



  //  float S = speed ; // Speed 1 =~ 2m/s so halve first
    float Phi = speed * waveMagnitude; // Phase/frequency
    float phase = Phi * Time;

    // Unoptimized:
    // float2 xz = position.xz - K/k*Q*A*sin(dot(K,position.xz)- wT);
    // float y = A*cos(dot(K,position.xz)- wT);

    // Calculate once instead of 4 times
    float KPwT = dot(directionWaveLength, position.xz) - phase;
    float S0 = sin(KPwT);
    float C0 = cos(KPwT);

    float2 xz = -directionWaveLength / Phi * D * (steepness * amplitude * S0);
    // Calculate the vertex offset along the Y (up/down) axis
    float y = amplitude * C0;

    // Calculate the tangent/bitangent/normal
    // Bitangent
    float3 B = float3(
        1 - (steepness * D.x * D.x * kA * C0),
        D.x * kA * S0,
        -(steepness * D.x * D.y * kA * C0));
    // Tangent
    float3 T = float3(
        -(steepness * D.x * D.y * kA * C0),
        D.y * kA * S0,
        1 - (steepness * D.y * D.y * kA * C0)
        );

    // Append the results
    result.xz += xz;
    result.y += y;

 //   normal += N;
 
    tangent += T;
    bitangent += B;
}
