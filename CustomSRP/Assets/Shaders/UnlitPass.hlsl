#ifndef CUSTOM_UNLIT_PASS_INCLUDED
#define CUSTOM_UNLIT_PASS_INCLUDED

#include  "../ShaderLibrary/Common.hlsl"

CBUFFER_START(UnityPerMaterial)
    float4 _BaseColor;
CBUFFER_END

void UnlitPassVertex(float3 positionOS) 
{
    
}

float4 UnlitPassFrag() :SV_TARGET
{
    return _BaseColor;
}


#endif