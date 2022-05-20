// 光与材质交互的计算地方
#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED

#include "Common.hlsl"
#include "Brdf.hlsl"

// 入射光照
float3 IncomingLight(Surface surface, Light light)
{
    return saturate(dot(surface.normal, light.direction)) * light.color;
}

float3 GetLighting(Surface surface, Brdf brdf, Light light)
{
    return IncomingLight(surface, light) * DirectBrdf(surface, brdf, light);
}

float3 GetLighting(Surface surface, Brdf brdf)
{
    return GetLighting(surface, brdf, GetDirectionalLight());
}


#endif