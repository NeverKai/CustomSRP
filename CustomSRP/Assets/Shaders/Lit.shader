Shader "CustomRP/Lit"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BaseColor ("Color", Color) = (0.5, 0.5, 0.5, 1.0)
    }
    SubShader
    {
        Tags
        {
            "LightMode" = "CustomLit"
        }
        Pass
        {
            HLSLPROGRAM
            #pragma vertex LitPassVertex
            #pragma fragment LitPassFrag
        
            #include "LitPass.hlsl"

            ENDHLSL
        }
    }
}
