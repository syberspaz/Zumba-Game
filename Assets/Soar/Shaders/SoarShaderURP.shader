// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


Shader "Soar/SoarShaderURP"
{
    Properties
    {
        _FadeDistance("Fade", Range(0.0005,0.45)) = 0.025

        [Header(Ambient)]
        _Ambient("Intensity", Range(0., 5.)) = 1.5
        _AmbColor("Color", color) = (1., 1., 1., 1.)
		_ColorBias("ColorBias",color)=(1.,1.,1.,1.)
        _ColorBiasMultiplier("ColorBiasMultiplier", Range(0., 1.)) = 0.13

        [Header(Diffuse)]
        _Diffuse("Val", Range(0., 1.)) = 1.
        _DifColor("Color", color) = (1., 1., 1., 1.)

    }
        SubShader
        {
            Tags
            {
                "RenderPipeline" = "UniversalPipeline"
            }

            LOD 100
            Pass
            {
                CGPROGRAM
                #pragma target 5.0
                #pragma vertex vert
                #pragma fragment frag
                #pragma require 2darray

                #include "UnityCG.cginc"
                #include "SoarCommon.cginc"

                ENDCG
            }
        }
            Fallback "Diffuse"
}