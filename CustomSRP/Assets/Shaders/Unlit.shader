Shader "CutomRP/Unlit"
{
    Properties
    {
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
