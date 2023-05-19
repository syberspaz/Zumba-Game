    #ifndef SOAR_COMMON_FUNCTIONS
#define SOAR_COMMON_FUNCTIONS

#pragma shader_feature_local ENABLE_RELIGHTING
#pragma shader_feature_local PER_PIXEL_LIGHTING
#pragma shader_feature_local TEXTURE_BLEED_REDUCTION

#pragma multi_compile_local \
    CAM_COUNT_1 CAM_COUNT_2 CAM_COUNT_3 CAM_COUNT_4 \
    CAM_COUNT_5 CAM_COUNT_6 CAM_COUNT_7 CAM_COUNT_8 \
    CAM_COUNT_9 CAM_COUNT_10 CAM_COUNT_11 CAM_COUNT_12 \
    CAM_COUNT_13 CAM_COUNT_14 CAM_COUNT_15 CAM_COUNT_16

#pragma multi_compile_local PCF_SIZE_1 PCF_SIZE_2 PCF_SIZE_3 PCF_SIZE_4 PCF_SIZE_5

#ifdef CAM_COUNT_1
#define MAXIMUM_COLOR_CAMERA_COUNT 1
#elif CAM_COUNT_2
#define MAXIMUM_COLOR_CAMERA_COUNT 2
#elif CAM_COUNT_3
#define MAXIMUM_COLOR_CAMERA_COUNT 3
#elif CAM_COUNT_4
#define MAXIMUM_COLOR_CAMERA_COUNT 4
#elif CAM_COUNT_5
#define MAXIMUM_COLOR_CAMERA_COUNT 5
#elif CAM_COUNT_6
#define MAXIMUM_COLOR_CAMERA_COUNT 6
#elif CAM_COUNT_7
#define MAXIMUM_COLOR_CAMERA_COUNT 7
#elif CAM_COUNT_8
#define MAXIMUM_COLOR_CAMERA_COUNT 8
#elif CAM_COUNT_9
#define MAXIMUM_COLOR_CAMERA_COUNT 9
#elif CAM_COUNT_10
#define MAXIMUM_COLOR_CAMERA_COUNT 10
#elif CAM_COUNT_11
#define MAXIMUM_COLOR_CAMERA_COUNT 11
#elif CAM_COUNT_12
#define MAXIMUM_COLOR_CAMERA_COUNT 12
#elif CAM_COUNT_13
#define MAXIMUM_COLOR_CAMERA_COUNT 13
#elif CAM_COUNT_14
#define MAXIMUM_COLOR_CAMERA_COUNT 14
#elif CAM_COUNT_15
#define MAXIMUM_COLOR_CAMERA_COUNT 15
#elif CAM_COUNT_16
#define MAXIMUM_COLOR_CAMERA_COUNT 16
#else
#define MAXIMUM_COLOR_CAMERA_COUNT 0
#endif


#ifdef PCF_SIZE_1
#define PCF_SIZE 1.0f
#elif PCF_SIZE_2
#define PCF_SIZE 2.0f
#elif PCF_SIZE_3
#define PCF_SIZE 3.0f
#elif PCF_SIZE_4
#define PCF_SIZE 4.0f
#elif PCF_SIZE_5
#define PCF_SIZE 5.0f
#else
#define PCF_SIZE 1.0f
#endif

UNITY_DECLARE_TEX2DARRAY( _CameraRGB );
UNITY_DECLARE_TEX2DARRAY( _CameraDepth );
UNITY_DECLARE_TEX2DARRAY( _Contours );

uniform float   _FadeDistance;
uniform float transitionStrength;

float  _Diffuse;
float4 _DifColor;

float  _Ambient;
float4 _AmbColor;
float4 _LightColor0;
float4 _LightColor;
float4 _LightPos;
float4 _TemperatureColor;

static const float samples = PCF_SIZE;
static const float bounds  = ( samples / 2.0f ) - 0.5f;

static const float n               = samples * samples;
static const float vfactor         = ( 1.0f / n ) / ( n - 1.0f );
static const float precisionFactor = 1.0f;


UNITY_INSTANCING_BUFFER_START( Props )
    UNITY_DEFINE_INSTANCED_PROP( float4x4, _CameraMatrices[ MAXIMUM_COLOR_CAMERA_COUNT ] )
    UNITY_DEFINE_INSTANCED_PROP( uniform float, nearCamera )
    UNITY_DEFINE_INSTANCED_PROP( uniform float, farCamera )
    UNITY_DEFINE_INSTANCED_PROP( uniform int, _CameraCount )
    UNITY_DEFINE_INSTANCED_PROP( uniform float4, cameraIntrinsics[ MAXIMUM_COLOR_CAMERA_COUNT * 6 ] )
    UNITY_DEFINE_INSTANCED_PROP( uniform float, cameraDepthNear[ MAXIMUM_COLOR_CAMERA_COUNT ] )
    UNITY_DEFINE_INSTANCED_PROP( uniform float, cameraDepthFar[ MAXIMUM_COLOR_CAMERA_COUNT ] )
    UNITY_DEFINE_INSTANCED_PROP( uniform float4, cameraDepthSize[ MAXIMUM_COLOR_CAMERA_COUNT ] )
    UNITY_DEFINE_INSTANCED_PROP( uniform float4, chromaKey );
UNITY_INSTANCING_BUFFER_END( Props )

float sampleDepth( int cameraIndex, float2 offsetTexel )
{
    return UNITY_SAMPLE_TEX2DARRAY( _CameraDepth, float3( offsetTexel, cameraIndex ) ).x;
}

float2 rgbToUV( float3 rgb )
{
    return float2( dot( rgb.rgb, float3( -0.169f, -0.331f, 0.5f ) ), dot( rgb.rgb, float3( 0.5f, -0.419f, -0.081f ) ) ) + 0.5f;
}

float chromaWeight( float3 rgb )
{
    float similarity = distance( rgbToUV( rgb ), chromaKey.xy );
    float whiteCos   = chromaKey.w * dot( normalize( rgb ), float3( 0.577f, 0.577f, 0.577f ) ); // use a chroma space colour hemisphere (which is the equivalent to hue + saturation) to evaluate similarity to "white" (i.e. perfect desaturation).

    return pow( clamp( ( ( whiteCos + 1 ) * similarity - chromaKey.w ) / ( chromaKey.z ), 0.0f, 1.0f ), chromaKey.z );
}

float linearize_depth( int cameraIndex, float depth )
{
    nearCamera = cameraDepthNear[ cameraIndex ];
    farCamera  = cameraDepthFar[ cameraIndex ];

    return ( 2.0 * nearCamera * farCamera ) / ( nearCamera + farCamera - ( depth * 2.0 - 1.0 ) * ( farCamera - nearCamera ) );
}

// START OF DISTORT FUNCTION //

float2 distort( float2 undistorted, float2 rgbSize, int intrinsicsIndex )
{
    int baseIndex = intrinsicsIndex * 6;

    float4 cf               = cameraIntrinsics[ baseIndex ];
    float4 codp21           = cameraIntrinsics[ baseIndex + 1 ];
    float4 k123x            = cameraIntrinsics[ baseIndex + 2 ];
    float4 k456x            = cameraIntrinsics[ baseIndex + 3 ];
    float4 pinholeiFP       = cameraIntrinsics[ baseIndex + 4 ];
    float2 cameraResolution = cameraIntrinsics[ baseIndex + 5 ].xy;

    undistorted *= cameraResolution;
    undistorted -= pinholeiFP.zw;
    undistorted *= pinholeiFP.xy;

    float2 p  = undistorted - codp21.xy;

    float  rs = dot( p, p );

    /* note, max radius check here deliberately removed, it'll just hit the texture borders */

    float rss = rs * rs;

    float3 rFactor = float3( rs, rss, rss * rs );

    float a  = 1.0f + dot( rFactor, k123x.xyz );
    float b  = 1.0f + dot( rFactor, k456x.xyz );
    float bi = ( b != 0.0f ) ? ( 1.0f / b ) : 1.0f;

    float d = a * bi;

    float offset = rgbSize.y - cameraResolution.y;

    float2 fp   = p * d;
    float2 fp2  = fp * fp;
    float  fxyp = fp.x * fp.y;

    float2 rs2fp2 = 2.0f * fp2 + rs;
    float2 fpd    = fp + rs2fp2 * codp21.zw + 2.0f * fxyp * codp21.wz;

    return ( ( float2( 0.5f, offset + 0.5f ) + min( ( fpd + codp21.xy ) * cf.zw + cf.xy, cameraResolution ) ) / rgbSize ) * float2( 1, -1 ) + float2( 0, 1 );
}

// END OF DISTORT FUNCTION //

float IsNan_float( float In )
{
    return ( In < 0.0 || In > 0.0 || In == 0.0 ) ? 0 : 1;
}


struct appdata
{
    float4 vertex : POSITION;
    float3 normal : NORMAL;
};

struct v2f
{
    float4          position : SV_POSITION;
    centroid float3 capturePosition : TEXCOORD0;
    centroid float4 screenPosition : TEXCOORD1;
    float3          normal : NORMAL;
#ifndef PER_PIXEL_LIGHTING
    float4 light : COLOR0;
#else
    float4 worldSpace : TEXCOORD2;
#endif
    float3 projections[ MAXIMUM_COLOR_CAMERA_COUNT ] : TEXCOORD3;
};

float4 lighting( float4 lightPos, float4 worldPos, float3 normal, float4 lightColor, float4 lightTemp = float4(0,0,0,0) )
{

    float3 lightDir = -normalize( worldPos - lightPos.xyz ); 

    float4 unityAmbient = float4( unity_SHAr.w, unity_SHAg.w, unity_SHAb.w, 1.0f );
    fixed4 amb          = unityAmbient + _Ambient * _AmbColor;
    float4 NdotL        = max( 0., dot( normal, lightDir ) * (lightColor + lightTemp) );
    float4 dif          = NdotL * _Diffuse * lightColor * _DifColor;
    return dif + amb;
}

v2f vert( appdata inputVertex )
{
    v2f outputVertex;
    outputVertex.position            = UnityObjectToClipPos( inputVertex.vertex );
    outputVertex.capturePosition.xyz = inputVertex.vertex.xyzw;
    outputVertex.screenPosition      = ComputeScreenPos( outputVertex.position );
    outputVertex.normal              = normalize( mul( inputVertex.normal, unity_WorldToObject ).xyz );

    for ( int cameraIndex = 0; cameraIndex < MAXIMUM_COLOR_CAMERA_COUNT; cameraIndex++ )
    {
        // DONE - we did make this per vertex to save some shader power - did just require some rejig - CS - SM

        float4 clipSpace = mul( float4( outputVertex.capturePosition, 1.0f ), _CameraMatrices[ cameraIndex ] );
        outputVertex.projections[cameraIndex].xy = clipSpace.xy / clipSpace.w;
        outputVertex.projections[cameraIndex].z = linearize_depth( cameraIndex, clipSpace.z / clipSpace.w );
    }

#ifndef PER_PIXEL_LIGHTING
    #ifdef HDRP_LIGHT_SETTINGS
    outputVertex.light = lighting( _LightPos, inputVertex.vertex, outputVertex.normal, _LightColor, _TemperatureColor );
    #else
    outputVertex.light  = lighting( _WorldSpaceLightPos0, inputVertex.vertex, outputVertex.normal, _LightColor0);
    #endif
#else
    outputVertex.worldSpace = inputVertex.vertex; 
#endif
    return outputVertex;
}

float4 frag( v2f inputVertex, uniform float4x4 cameraViewProjection[ MAXIMUM_COLOR_CAMERA_COUNT ] )
    : SV_TARGET
{
    float4 sumColors     = 0.0;
    float4 fragmentColor = 0.0;
    float  sumConfidence = 0.0;
    float  fadeDistance  = _FadeDistance * precisionFactor;


    centroid float2 colorDistorted;
    float           confidence;
    float           depthValue;
    float2          dx;
    float2          offsetTexel;
    float4          colorRead;
    float           difference;

    float3 textureDimensions;
    _CameraRGB.GetDimensions( textureDimensions.x, textureDimensions.y, textureDimensions.z );

    float3 contourDimensions;
    _Contours.GetDimensions( contourDimensions.x, contourDimensions.y, contourDimensions.z );

    float2 contourMapTexelOffset = 1.0f / contourDimensions.xy;

    float4 outputs[ MAXIMUM_COLOR_CAMERA_COUNT ];

    [unroll] for ( int cameraIndex = 0; cameraIndex < MAXIMUM_COLOR_CAMERA_COUNT; cameraIndex++ )
    {
        // TODO - we should make this per vertex to save some shader power - will just require some rejig - CS

        float2 projected = inputVertex.projections[cameraIndex].xy;
        float projectionDepth = inputVertex.projections[cameraIndex].z;

        float2 depthMapTexelOffset = 1.0f / cameraDepthSize[ cameraIndex ].xy;
        confidence = 0.0f;

        for ( float y = -bounds; y <= bounds; y++ )
        {
            for ( float x = -bounds; x <= bounds; x++ )
            {
                offsetTexel = float2( x, y ) * depthMapTexelOffset + projected.xy;

                offsetTexel.y = 1 - offsetTexel.y;

                depthValue = UNITY_SAMPLE_TEX2DARRAY( _CameraDepth, float3( offsetTexel, cameraIndex ) ).x;

                difference = abs( depthValue - projectionDepth ) * precisionFactor;

                confidence += precisionFactor * clamp( 1.0f - ( difference / fadeDistance ), 0.0f, 1.0f ) / pow( depthValue, 2.0f );
            }
        }

        colorDistorted = distort( projected, textureDimensions.xy, cameraIndex );

        dx        = ddx_fine( colorDistorted * precisionFactor );
        float2 dy = ddy_fine( colorDistorted * precisionFactor );

        float dArea      = abs( dx.x * dy.y - dy.x * dx.y );
        float areaWeight = 1.0f + dArea;

        confidence *= areaWeight;

        colorRead = UNITY_SAMPLE_TEX2DARRAY( _CameraRGB, float3( colorDistorted, cameraIndex ) );

        confidence *= colorRead.w;

        if ( chromaKey.z != 0 )
        {
            float chromaWeighting = chromaWeight( colorRead );
            float colorY          = min( dot( colorRead, float3( 0.2126f, 0.7152f, 0.0722f ) ), 1 );

            colorRead = lerp( float4( colorY, colorY, colorY, 0.0f ), colorRead, clamp( pow( chromaWeighting / chromaKey.w, 1.0f / chromaKey.z ), 0, 1 ) );

            confidence *= chromaWeighting;
        }

        confidence = max( confidence, 0 );

        #ifdef TEXTURE_BLEED_REDUCTION
        float averageContourConfidence = 0;
        for ( float yy = -bounds; yy <= bounds; yy++ )
        {
            for ( float xx = -bounds; xx <= bounds; xx++ )
            {
                offsetTexel = float2( xx, yy ) * contourMapTexelOffset + projected.xy;

                #if !UNITY_UV_STARTS_AT_TOP
                offsetTexel.y = 1 - offsetTexel.y;
                #endif

                averageContourConfidence += UNITY_SAMPLE_TEX2DARRAY( _Contours, float3( offsetTexel, cameraIndex ) ).x;
            }
        }
        confidence *= averageContourConfidence / n;
        #endif

        outputs[ cameraIndex ] = float4( colorRead.xyz, confidence );
        sumConfidence += confidence;
    }

    if ( sumConfidence > 0.0005f )
    {
        float invSumConfidence = 1.0f / sumConfidence;

        /*[unroll]*/ for ( int cameraIndex = 0; cameraIndex < MAXIMUM_COLOR_CAMERA_COUNT; ++cameraIndex )
        {
            float reweightedConfidence = exp( transitionStrength * outputs[ cameraIndex ].w * invSumConfidence ) - 1.0f;

            sumColors += float4( outputs[ cameraIndex ].xyz * reweightedConfidence, reweightedConfidence );
        }
    }

    float4 finalResult = float4( 0, 0, 0, 1.0f );
    if ( sumConfidence > 0.0005f )
    {
#ifdef ENABLE_RELIGHTING
#ifdef PER_PIXEL_LIGHTING
        #ifdef HDRP_LIGHT_SETTINGS
        float4 light = lighting( _LightPos, inputVertex.worldSpace, inputVertex.normal, _LightColor, _TemperatureColor );
        #else
        float4 light = lighting( _WorldSpaceLightPos0, inputVertex.worldSpace, inputVertex.normal, _LightColor0 );
        #endif
#else
        float4 light = inputVertex.light;
#endif
        finalResult = float4( ( sumColors.xyz / sumColors.w ) * light.rgb, 1.0f );
#else
        finalResult = float4( ( sumColors.xyz / sumColors.w ), 1.0f );
#endif
    }
    #if UNITY_COLORSPACE_GAMMA
    return finalResult;
    #else
    return float4( GammaToLinearSpace( finalResult.rgb ), finalResult.a );
    #endif
}

#endif