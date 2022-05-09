Shader "CutomRP/Unlit"
{
    Properties
    {
        _BaseColor("Color", Color) = (1.0, 1.0, 1.0, 1.0)
    }
    SubShader
    {
        Pass
        {
            HLSLPROGRAM

            #include "UnlitPass.hlsl"
            #pragma vertex UnlitPassVertex
            #pragma fragment UnlitPassFrag
            
            ENDHLSL
        }
    }
}
