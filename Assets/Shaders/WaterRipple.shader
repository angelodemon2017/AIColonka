Shader "Universal Render Pipeline/Custom/WaterRipple"
{
    Properties
    {
        [MainColor] _BaseColor("Base Color", Color) = (0.1, 0.3, 0.5, 0.8)
        [MainTexture] _BaseMap("Base Map", 2D) = "white" {}
        _NormalMap("Normal Map", 2D) = "bump" {}
        _NormalStrength("Normal Strength", Range(0,2)) = 1.0
        
        _WaveSpeed("Wave Speed", Float) = 0.5
        _WaveAmplitude("Wave Amplitude", Float) = 0.1
        _WaveFrequency("Wave Frequency", Float) = 1.0
        
        _RippleScale("Ripple Scale", Float) = 1.0
        _RippleSpeed("Ripple Speed", Float) = 2.0
        
        _Specular("Specular", Range(0,1)) = 0.5
        _Smoothness("Smoothness", Range(0,1)) = 0.7
        
        _RippleOrigin("Ripple Origin", Vector) = (0,0,0,0)
        _RippleTime("Ripple Time", Float) = -100
        _RippleDecay("Ripple Decay", Float) = 3.0
    }

    SubShader
    {
        Tags 
        { 
            "RenderType" = "Transparent" 
            "Queue" = "Transparent" 
            "RenderPipeline" = "UniversalPipeline"
        }
        
        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        ENDHLSL

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }
            
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            
            struct Attributes
            {
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
                float4 tangentOS    : TANGENT;
                float2 texcoord     : TEXCOORD0;
            };

            struct Varyings
            {
                float2 uv           : TEXCOORD0;
                float3 positionWS   : TEXCOORD1;
                float3 normalWS     : TEXCOORD2;
                float4 tangentWS    : TEXCOORD3;
                float4 positionHCS  : SV_POSITION;
            };

            TEXTURE2D(_BaseMap);    SAMPLER(sampler_BaseMap);
            TEXTURE2D(_NormalMap);  SAMPLER(sampler_NormalMap);
            
            CBUFFER_START(UnityPerMaterial)
            float4 _BaseMap_ST;
            float4 _BaseColor;
            float _NormalStrength;
            float _WaveSpeed;
            float _WaveAmplitude;
            float _WaveFrequency;
            float _RippleScale;
            float _RippleSpeed;
            float _Specular;
            float _Smoothness;
            float3 _RippleOrigin;
            float _RippleTime;
            float _RippleDecay;
            CBUFFER_END

            float GetRippleEffect(float3 worldPos)
            {
                float distanceToRipple = distance(worldPos.xz, _RippleOrigin.xz);
                float timeSinceRipple = _Time.y - _RippleTime;
                
                if (timeSinceRipple < 0 || distanceToRipple > timeSinceRipple * _RippleSpeed)
                    return 0.0;
                    
                float rippleProgress = distanceToRipple / (timeSinceRipple * _RippleSpeed);
                float ripple = sin(rippleProgress * 20 - _Time.y * 5) * exp(-rippleProgress * _RippleDecay);
                
                return ripple * (1 - rippleProgress);
            }

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                
                // ќсновные волны
                float wave = sin(_Time.y * _WaveSpeed + IN.positionOS.x * _WaveFrequency) * _WaveAmplitude;
                float wave2 = cos(_Time.y * _WaveSpeed * 0.7 + IN.positionOS.z * _WaveFrequency * 1.3) * _WaveAmplitude * 0.5;
                
                // Ёффект р€би
                float3 positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                float ripple = GetRippleEffect(positionWS);
                
                // —мещение вершин
                IN.positionOS.y += (wave + wave2) * 0.3 + ripple * 0.2;
                
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.positionWS = positionWS;
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                OUT.tangentWS = float4(TransformObjectToWorldDir(IN.tangentOS.xyz), IN.tangentOS.w);
                OUT.uv = TRANSFORM_TEX(IN.texcoord, _BaseMap);
                
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // Ќормали
                float3 normalTS = UnpackNormalScale(SAMPLE_TEXTURE2D(_NormalMap, sampler_NormalMap, IN.uv * 2.0), _NormalStrength);
                float3 normalWS = TransformTangentToWorld(normalTS, 
                    float3x3(IN.tangentWS.xyz, cross(IN.normalWS, IN.tangentWS.xyz) * IN.tangentWS.w, IN.normalWS));
                
                // ќсвещение
                Light mainLight = GetMainLight();
                float3 lightDir = mainLight.direction;
                float NdotL = saturate(dot(normalWS, lightDir));
                
                // —пекул€р (Phong)
                float3 viewDir = normalize(_WorldSpaceCameraPos - IN.positionWS);
                float3 reflectDir = reflect(-lightDir, normalWS);
                float spec = pow(saturate(dot(viewDir, reflectDir)), _Glossiness * 128);
                
                // »тоговый цвет
                half4 baseColor = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv) * _BaseColor;
                half3 color = baseColor.rgb * NdotL + spec * _Specular;
                
                return half4(color, baseColor.a);
            }
            ENDHLSL
        }
    }
    
    FallBack "Universal Render Pipeline/Transparent"
}