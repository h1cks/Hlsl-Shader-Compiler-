struct TriTess
{
    float EdgeTess[3] : SV_TessFactor;
    float InsideTess : SV_InsideTessFactor;
};

struct PatchTess
{
    float EdgeTess[3] : SV_TessFactor;
    float InsideTess : SV_InsideTessFactor;
};

static const float gMinTessDistance = 135.0f;
static const float gMaxTessDistance = 5.0f;
static const float gMinTessFactor = 1.0f;
static const float gMaxTessFactor = 3.0f;


