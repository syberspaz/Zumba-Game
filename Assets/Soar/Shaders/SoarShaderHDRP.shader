// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


Shader "Soar/SoarShaderHDRP"
{
    Properties
    {
        _FadeDistance ("Fade", Range (0.0005,0.45)) = 0.025

        [Header(Ambient)]
        _Ambient ("Intensity", Range(0., 2.)) = 1.5
        _AmbColor ("Ambient Color", color) = (1., 1., 1., 1.)

        [Header(Diffuse)]
         [HideInInspector]_Diffuse ("Val", Range(0., 1.)) = 1.
        _DifColor ("Diffuse Color", color) = (1., 1., 1., 1.)

         [HideInInspector]_LightPos ("Vector", Vector) = (0., -1., -1., 0.)
         [HideInInspector]_LightColor ("Color", color) = (1., 1., 1., 1.)

         [HideInInspector]_TemperatureColor ("Color", color) = (1., 1., 1., 1.)

    }
    SubShader 
    {
        Tags
        {
            "RenderPipeline" = "HDRenderPipeline"
        }

        LOD 100
        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma require 2darray
            #pragma target 5.0

            #include "UnityCG.cginc"

            #define HDRP_LIGHT_SETTINGS 1

            #include "SoarCommon.cginc"

            ENDCG
        }
    }
    Fallback "Diffuse"
}