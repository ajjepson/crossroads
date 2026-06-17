Shader "Universal Render Pipeline/Custom/WindLit"
{
    Properties
    {
        [MainTexture] _BaseMap("Base Map", 2D) = "white" {}
        [MainColor] _BaseColor("Base Color", Color) = (1,1,1,1)
        _Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5
        _Smoothness("Smoothness", Range(0.0, 1.0)) = 0.5
        _BumpMap("Normal Map", 2D) = "bump" {}
        
        [Header(Wind Animation)]
        _WindSpeed("Wind Speed", Range(0.0, 10.0)) = 2.0
        _WindStrength("Wind Strength", Range(0.0, 2.0)) = 0.15
        _WindFrequency("Wind Height Weight", Range(0.0, 1.0)) = 0.4
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" "Queue" = "Geometry" }
        LOD 100

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }

            Cull Off
            AlphaToMask On

            HLSLPROGRAM
            #pragma exclude_renderers gles gles3 glcore
            #pragma target 4.5

            // -------------------------------------
            // Material Keywords
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON

            // -------------------------------------
            // Universal Pipeline keywords
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT

            // -------------------------------------
            // Unity defined keywords
            #pragma multi_compile_fog
            #pragma multi_compile_instancing

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
                float4 tangentOS    : TANGENT;
                float2 uv           : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
                float3 normalWS     : TEXCOORD0;
                float2 uv           : TEXCOORD1;
                float3 positionWS   : TEXCOORD3;
                float fogFactor     : TEXCOORD5;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            Texture2D _BaseMap;
            SamplerState sampler_BaseMap;
            Texture2D _BumpMap;
            SamplerState sampler_BumpMap;

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                float4 _BaseColor;
                float _Cutoff;
                float _Smoothness;
                float _WindSpeed;
                float _WindStrength;
                float _WindFrequency;
            CBUFFER_END

            // Simple vertex sway based on time and height
            float4 ApplyWind(float4 positionOS)
            {
                float3 worldPos = TransformObjectToWorld(positionOS.xyz);
                float sway = sin(_Time.y * _WindSpeed + worldPos.x * 0.5 + worldPos.z * 0.5) * _WindStrength;
                // Height-based weight using local Y
                float weight = max(0.0, positionOS.y * _WindFrequency);
                positionOS.x += sway * weight;
                positionOS.z += sway * weight * 0.5;
                return positionOS;
            }

            Varyings vert(Attributes input)
            {
                Varyings output = (Varyings)0;

                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                // Apply wind deformation to object space position
                float4 windPositionOS = ApplyWind(input.positionOS);

                VertexPositionInputs vertexInput = GetVertexPositionInputs(windPositionOS.xyz);
                VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS, input.tangentOS);

                output.positionCS = vertexInput.positionCS;
                output.positionWS = vertexInput.positionWS;
                output.normalWS = normalInput.normalWS;
                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                
                output.fogFactor = ComputeFogFactor(vertexInput.positionCS.z);

                return output;
            }

            float4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                float4 baseColor = _BaseColor * _BaseMap.Sample(sampler_BaseMap, input.uv);
                
                #if defined(_ALPHATEST_ON)
                clip(baseColor.a - _Cutoff);
                #endif

                // Simple shading/lighting calculation using URP's Standard Lighting
                float3 normalWS = normalize(input.normalWS);
                
                // Get main light
                Light mainLight = GetMainLight(TransformWorldToShadowCoord(input.positionWS));
                
                // Diffuse lighting
                float NdotL = saturate(dot(normalWS, mainLight.direction));
                float3 diffuse = NdotL * mainLight.color * mainLight.shadowAttenuation;
                
                // Ambient lighting
                float3 ambient = SampleSH(normalWS);

                float3 finalColor = (diffuse + ambient) * baseColor.rgb;
                
                // Add fog
                finalColor = MixFog(finalColor, input.fogFactor);

                return float4(finalColor, baseColor.a);
            }
            ENDHLSL
        }

        Pass
        {
            Name "ShadowCaster"
            Tags { "LightMode" = "ShadowCaster" }

            Cull Off

            HLSLPROGRAM
            #pragma exclude_renderers gles gles3 glcore
            #pragma target 4.5

            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma multi_compile_instancing

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
                float2 uv           : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
                float2 uv           : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            Texture2D _BaseMap;
            SamplerState sampler_BaseMap;

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                float4 _BaseColor;
                float _Cutoff;
                float _Smoothness;
                float _WindSpeed;
                float _WindStrength;
                float _WindFrequency;
            CBUFFER_END

            float4 ApplyWind(float4 positionOS)
            {
                float3 worldPos = TransformObjectToWorld(positionOS.xyz);
                float sway = sin(_Time.y * _WindSpeed + worldPos.x * 0.5 + worldPos.z * 0.5) * _WindStrength;
                float weight = max(0.0, positionOS.y * _WindFrequency);
                positionOS.x += sway * weight;
                positionOS.z += sway * weight * 0.5;
                return positionOS;
            }

            Varyings vert(Attributes input)
            {
                Varyings output = (Varyings)0;
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);

                float4 windPositionOS = ApplyWind(input.positionOS);
                float3 positionWS = TransformObjectToWorld(windPositionOS.xyz);
                output.positionCS = TransformWorldToHClip(positionWS);

                #if UNITY_REVERSED_Z
                output.positionCS.z = min(output.positionCS.z, output.positionCS.w * UNITY_NEAR_CLIP_VALUE);
                #else
                output.positionCS.z = max(output.positionCS.z, output.positionCS.w * UNITY_NEAR_CLIP_VALUE);
                #endif

                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);

                return output;
            }

            float4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);

                float4 baseColor = _BaseColor * _BaseMap.Sample(sampler_BaseMap, input.uv);
                #if defined(_ALPHATEST_ON)
                clip(baseColor.a - _Cutoff);
                #endif

                return 0;
            }
            ENDHLSL
        }
    }
}
