Shader "Soar/contours"
{
	Properties
    {
        _CameraDepth( "Depth Tex", 2DArray ) = "" {}
		_Ztest("Depth Test", Float) = 5 
        _CameraId( "Camera ID", Int ) = 0
	}

	SubShader
    {

		Tags { "RenderType"="Opaque" }

		Pass
        {
            Lighting Off
            Blend Off
            ZWrite Off
			ZTest Always
            Cull Off
            ColorMask R
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
            #pragma target 5.0
			#include "UnityCG.cginc"

            #pragma multi_compile_local FILTER_SIZE_3 FILTER_SIZE_5 FILTER_SIZE_7 FILTER_SIZE_11

            #ifdef FILTER_SIZE_3
            #define FILTER_KERNEL_SIZE 3
            #elif FILTER_SIZE_5
            #define FILTER_KERNEL_SIZE 5
            #elif FILTER_SIZE_7
            #define FILTER_KERNEL_SIZE 7
            #elif FILTER_SIZE_11
            #define FILTER_KERNEL_SIZE 11
            #else
            #define FILTER_KERNEL_SIZE 11
            #endif

            #define UNITY_GATHER_TEX2DARRAY( tex, coord ) tex.GatherRed( sampler##tex, coord )
            UNITY_DECLARE_TEX2DARRAY( _CameraDepth );


			#define f_05 float3(.5,.5,.5)
			#define f_10 float3(1.,1.,1.)

            uniform float cameraDepthNear;
            uniform float cameraDepthFar;
            uniform float CameraId;
            uniform float4 texScale;

			struct v2f
            {
			    float4 pos : SV_POSITION;
                float2 uv : TEXTURE0;
			};

            float linearize_depth( float depth )
            {
                return ( 2 * cameraDepthNear ) / ( cameraDepthNear + cameraDepthFar - ( depth * 2.0 - 1.0 ) * ( cameraDepthFar - cameraDepthNear ) );
            }

            float delinearize(float depth) {
                return ( cameraDepthFar * ( depth - cameraDepthNear ) ) / ( depth * ( cameraDepthFar - cameraDepthNear ) );
            }

            float relinearize(float depth) {
                return depth / cameraDepthFar;
            }

			static const float contours_threshold = 0.0f;
			static const float minimum_probability = 0.8f;
			static const float contours_gradient = 80.0f;
            static const float two_sigma_range_squared = 0.001f;
			
			float contourProbability( float value, float center, float gradient, float minimum_probability )
            {
                return 1.0f / ( 1.0f + pow( 2.f, gradient * ( value - center ) ) );
            }

			static const float3x3 laplacian = float3x3(
                -1.0, -1.0, -1.0,
                -1.0, 8.0, -1.0,
                -1.0, -1.0, -1.0 );

			v2f vert (appdata_base v)
            {
			   v2f o;
               o.pos       = UnityObjectToClipPos( float4( v.vertex.xyz, 1.0f ) );
               o.uv        = o.pos.xy;
			   return o;
			}

			float4 frag(v2f inputVertex) : SV_Target
			{
                const int kernel_size      = FILTER_KERNEL_SIZE;
                const int kernel_remainder = kernel_size % 2;
                const int kernel_offset = -(kernel_size / 2);
                const int kernel_target = ( kernel_size / 2 );
                const int half_ks     = kernel_size / 2;
                float     depth_values[ kernel_size ][ kernel_size ];
                float2    projected = inputVertex.uv.xy;
                projected += float2( 1, 1 );
                projected /= 2.0f;

                /*[unroll] for ( float i = 0; i < kernel_size; ++i )
                {
                    [unroll] for ( float j = 0; j < kernel_size; ++j )
                    {
                        float2 offsetTexel                                     = float2( j - half_ks, i - half_ks ) * texScale + projected.xy;
                        float depth                                            = relinearize( UNITY_SAMPLE_TEX2DARRAY( _CameraDepth, float3( offsetTexel, CameraId ) ).r );
                        depth_values[ i ][ j ] = depth;
                    }
                }*/

                bool sample_j = true;
                bool sample_i = true;

                [unroll] for ( float i = 0; i < kernel_size; i += 2 )
                {
                    [unroll] for ( float j = 0; j < kernel_size; j += 2 )
                    {
                        float3 coordinates = float3( projected.xy + float2( j - half_ks, i - half_ks ) * texScale, CameraId );
                        float4 depth = UNITY_GATHER_TEX2DARRAY( _CameraDepth, coordinates );

                        depth_values[ i ][ j ] = relinearize( depth.w );
                        sample_j               = j + kernel_remainder < kernel_size;
                        sample_i               = i + kernel_remainder < kernel_size;

                        if ( sample_i )
                        {
                            depth_values[ i + 1 ][ j ] = relinearize( depth.x );
                        }
                        if ( sample_j )
                        {
                            depth_values[ i ][ j + 1 ] = relinearize( depth.z );
                        }
                        if ( sample_i && sample_j )
                        {
                            depth_values[ i + 1 ][ j + 1 ] = relinearize( depth.y );
                        }
                    }
                }

                float3x3  I;
                float closest_contour_distance           = 100000.0f; //really big number, idealy wanted INT_MAX
                float closest_contour_depth_diff_squared = 0.0f;
                bool  contour_found                      = false;
                float contour_value                      = 0.f;
                float max_found_probability              = -1.f;
                float max_depth_distance                 = -1000.f;
                float contour_depth                      = 0.0f;

                float current_depth = depth_values[ half_ks ][ half_ks ];
                [unroll] for ( int i = 1; i < ( kernel_size - 1 ); ++i )
                {
                    [unroll] for ( int j = 1; j < ( kernel_size - 1 ); ++j )
                    {
                        [unroll] for ( int y = -1; y < 2; ++y )
                        {
                            [unroll] for ( int x = -1; x < 2; ++x )
                            {
                                I[ y + 1 ][ x + 1 ] = depth_values[ i + y ][ j + x ];
                            }
                        }
                        
                        float g = 0.0;
                        [unroll] for ( int k = 0; k < 3; ++k )
                        {
                            g += dot( laplacian[ k ], I[ k ] );
                        }
                        float contour_probability = contourProbability( g, contours_threshold, contours_gradient, minimum_probability );
                        if ( contour_probability > minimum_probability )
                        {
                            contour_found                    = true;
                            int   dy                         = ( half_ks - i );
                            int   dx                         = ( half_ks - j );
                            float distance                   = sqrt( dx * dx + dy * dy );
                            float closest_contour_depth_diff = current_depth - depth_values[ i ][ j ];
                            if ( distance < closest_contour_distance )
                            {
                                max_found_probability              = contour_probability;
                                max_depth_distance                 = closest_contour_depth_diff;
                                closest_contour_distance           = distance;
                                closest_contour_depth_diff_squared = closest_contour_depth_diff * closest_contour_depth_diff;
                                contour_value                      = contour_probability;
                                contour_depth                      = depth_values[ i ][ j ];
                            }
                        }
                    }
                }

                float weight_range    = exp( -sqrt( closest_contour_depth_diff_squared ) / two_sigma_range_squared );
                float weight_distance = 1;
                float weight          = weight_range * weight_distance * ( 1 - contour_value );
                return                float4( float3( weight, weight, weight ), 1.0 );
			}
			ENDCG
		}
	}
	//FallBack "Diffuse"
}