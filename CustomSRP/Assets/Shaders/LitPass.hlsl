#ifndef CUSTOM_LIT_PASS_INCLUDED
#define CUSTOM_LIT_PASS_INCLUDED

#include "../ShaderLibrary/Lighting.hlsl"
#include  "../ShaderLibrary/Common.hlsl"

UNITY_INSTANCING_BUFFER_START(UnityPerMaterial)
    UNITY_DEFINE_INSTANCED_PROP(float4, _BaseColor)
    UNITY_DEFINE_INSTANCED_PROP(float4, _BaseMap_ST)
    UNITY_DEFINE_INSTANCED_PROP(float, _Metallic)
    UNITY_DEFINE_INSTANCED_PROP(float, _Smoothness)
UNITY_INSTANCING_BUFFER_END(UnityPerMaterial)

TEXTURE2D(_BaseMap);
SAMPLER(sampler_BaseMap);

struct Attributes
{
    float3 positionOS : POSITION;
    float3 normalOS : TEXCOORD0;
    float uv : TEXCOORD1;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

// float4 LitPassVertex(Attributes input)
// {
//     UNITY_SETUP_INSTANCE_ID(input);
//     float3 positionWS = TransformObjectToWorld(input.positionOS);
//     return TransformObjectToHClip(positionWS);
// }

struct Varyings
{
    float4 positionCS : SV_POSITION;
    float3 positionWS : VAR_POSITION;
    float2 baseUV : VAR_BASE_UV;
    float3 normalWS : VAR_NORMAL;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

Varyings LitPassVertex(Attributes input)
{
    UNITY_SETUP_INSTANCE_ID(input)
    Varyings output;

    output.positionWS = TransformObjectToWorld(input.positionOS);
    output.positionCS = TransformObjectToHClip(output.positionWS);
    output.normalWS = TransformObjectToWorldNormal(input.normalOS);

    float4 baseST = UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, _BaseMap_ST);
    output.baseUV = baseST.xy * input.uv + baseST.zw;
    return output;
}

float4 LitPassFrag(Varyings input) : SV_TARGET
{
    UNITY_SETUP_INSTANCE_ID(input)
    float4 baseMap = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, input.baseUV);
    
    Surface surface;
    surface.position = normalize(_WorldSpaceCameraPos - input.positionWS);
    surface.normal = normalize(input.normalWS);
    float3 baseColor = UNITY_ACCESS_INSTANCED_PROP(UniryPerMaterial, _BaseColor).rgb * baseMap.rgb;
    surface.alpha = baseMap.a;
    surface.color = baseColor.rgb;
    float3 color = GetLighting(surface);
    
    
    return float4(color, 1);
}


#endif