#ifndef CUSTOM_BRDF_INCLUDED
#define CUSTOM_BRDF_INCLUDED

#include "Common.hlsl"

struct Brdf
{
    float3 diffuse;
    float3 specular;
    float roughness;
};

#define MIN_REFLECTIVITY 0.04
float OneMinusReflectivity(float metallic)
{
    float range = 1.0 - MIN_REFLECTIVITY;
    return (1 - metallic) * range;
}

// 金属工作流 漫反射值 = （1 - 金属度） * light.color
// 能量守恒 入射光 = 漫反射 + 镜面反射(镜面反射 + 被吸收的部分)

Brdf GetBrdf(Surface surface, Brdf brdf, Light light)
{
    brdf.diffuse = OneMinusReflectivity(surface.metallic) * surface.color;
    brdf.specular = lerp(MIN_REFLECTIVITY, surface.color, surface.metallic);

    float perceptualRoughness = PerceptualSmoothnessToPerceptualRoughness(surface.smoothness);
    brdf.roughness = PerceptualRoughnessToRoughness(perceptualRoughness);
    return brdf;
}

float3 DirectBrdf(Surface surface, Brdf brdf, Light light)
{
    
}


#endif