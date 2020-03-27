


static const float3 upVector = float3(0.0f, 1.0f, 0.0f);
static const float3 upVector2 = float3(0.1f, 0.9f, 0.0f);
static const float specularShininess = 50.0f;
const static float4 waterLine = float4(0.95f, 0.95f, 0.9f, 1.0f);
const static float4 darkWater = float4(0.02f, 0.25f, 0.50f, 1.0f);
const static float4 fresnelColour = float4(0.16f, 0.42f, 0.70f, 1.0f);
const static float4 lightWater = float4(1, 1.0f, 0.75f, 0.5f);
const static float4 whitewater = float4(1.0f, 1.0f, 1.0f, 0.7f);
const static float c_waterDensityConstant = 0.001f;
const static float3 s_upVector = float3(0, 1, 0);

const static float wetSand = 0.9f;

static const float2 gTexC[4] =
{
    float2(0.0f, 1.0f),
  	float2(1.0f, 1.0f),
	float2(0.0f, 0.0f),
    float2(1.0f, 0.0f)
};

static const int XZ_PLANE = 0;
static const int CAMERA_PLANE_MATRIX = 1;
static const int CAMERA_PLANE_FLAT = 2;
static const int XY_PLANE = 3;
static const int MODEL_USED = 4;
static const int SINGLESTRIP_XZ = 5;
static const int NORMAL_PLANE_Y = 7;
