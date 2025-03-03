﻿/// <summary>
/// Copyright 2016-2021 Ramiro Oliva (Kronnect) - All rights reserved
/// </summary>
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using static BeautifyEffect.Beautify;

namespace BeautifyEffect {

    [CreateAssetMenu(fileName = "BeautifyProfile", menuName = "Beautify Profile", order = 101)]
    public class BeautifyProfile : ScriptableObject {

        #region RGB Dither

        [Range(0, 0.2f)] public float dither = 0.02f;
        [Range(0, 1f)] public float ditherDepth = 0f;

        #endregion

        #region Sharpen Settings

        [Range(0, 1f)] public float sharpenMinDepth = 0f;
        [Range(0, 1.1f)] public float sharpenMaxDepth = 0.999f;
        [Range(0f, 15f)] public float sharpen = 2f;
        [Range(0f, 1f)] public float sharpenMinMaxDepthFallOff = 0f;
        [Range(0f, 0.05f)] public float sharpenDepthThreshold = 0.035f;
        public Color tintColor = new Color(1, 1, 1, 0);
        [Range(0f, 0.2f)] public float sharpenRelaxation = 0.08f;
        [Range(0, 1f)] public float sharpenClamp = 0.45f;
        [Range(0, 1f)] public float sharpenMotionSensibility = 0.5f;
        [Range(0.01f, 5f)] public float sharpenMotionRestoreSpeed = 0.5f;

        #endregion

        #region Antialias & Render scale

        [Header("Best performance mode only")]
        [Range(1, 8)] public float downscale = 1;

        [Header("Best quality mode only")]
        [Range(1, 3)] public int superSampling = 1;
        [Range(0, 20)] public float antialiasStrength = 5f;
        [Range(0.1f, 8f)] public float antialiasMaxSpread = 3f;
        [Range(0f, 0.05f)] public float antialiasDepthThreshold = 0.000001f;
        public float antialiasDepthAtten;

        #endregion

        #region Color grading

        [Range(-2f, 3f)] public float saturate = 1f;
        [Range(0.5f, 1.5f)] public float contrast = 1.02f;
        [Range(0f, 2f)] public float brightness = 1.05f;
        [Range(0f, 2f)] public float daltonize = 0f;
        [Range(0, 1f)] public float hardLightIntensity = 0.5f;
        [Range(0, 1f)] public float hardLightBlend;

        #endregion

        #region Vignetting

        public bool vignetting;
        public Color vignettingColor = new Color(0.3f, 0.3f, 0.3f, 0.05f);
        public float vignettingFade;
        public bool vignettingCircularShape;
        public float vignettingAspectRatio = 1f;
        [Range(0, 1f)] public float vignettingBlink;
        public BEAUTIFY_BLINK_STYLE vignettingBlinkStyle = BEAUTIFY_BLINK_STYLE.Cutscene;
        public Texture2D vignettingMask;
        public Vector2 vignettingCenter = new Vector2(0.5f, 0.5f);

        #endregion

        #region Frame

        public bool frame = false;
        public Color frameColor = new Color(1, 1, 1, 0.047f);
        public Texture2D frameMask;
        public FrameStyle frameStyle = FrameStyle.Border;
        [Range(0, 0.5f)] public float frameBandHorizontalSize;
        [Range(0, 1f)] public float frameBandHorizontalSmoothness;
        [Range(0, 0.5f)] public float frameBandVerticalSize = 0.1f;
        [Range(0, 1f)] public float frameBandVerticalSmoothness;

        #endregion

        #region LUT

        public bool lut = false;
        [Range(0f, 1f)] public float lutIntensity = 1f;
        public Texture2D lutTexture;
        public Texture3D lutTexture3D;


        #endregion

        #region Night Vision

        public bool nightVision = false;
        public Color nightVisionColor = new Color(0.5f, 1f, 0.5f, 0.5f);

        #endregion

        #region Outline

        public bool outline = false;
        [ColorUsage(false, hdr: true)] public Color outlineColor = new Color(0, 0, 0, 0.8f);
        public bool outlineCustomize;
        public bool outlineBlurDownscale = true;
        public BEAUTIFY_OUTLINE_STAGE outlineStage = BEAUTIFY_OUTLINE_STAGE.BeforeBloom;
        [Range(0, 2)] public float outlineSpread = 1f;
        [Range(0, 5)] public int outlineBlurPassCount = 1;
        [Range(0, 8)] public float outlineIntensityMultiplier = 1f;
        [Range(0, 1)] public float outlineMinDepthThreshold;

        #endregion

        #region Thermal Vision

        public bool thermalVision = false;

        #endregion

        #region Lens Dirt

        public bool lensDirt = false;
        [Range(0f, 1f)] public float lensDirtThreshold = 0.5f;
        [Range(0f, 1f)] public float lensDirtIntensity = 0.9f;
        public Texture2D lensDirtTexture;

        #endregion

        #region Chromatic Aberration

        public bool chromaticAberration;
        [Range(0, 0.05f)] public float chromaticAberrationIntensity;
        [Range(0, 32f)] public float chromaticAberrationSmoothing;

        #endregion

        #region Bloom

        public bool bloom;
        public LayerMask bloomCullingMask;
        [Range(1f, 4f)] public float bloomLayerMaskDownsampling = 1f;
        public float bloomIntensity = 1f;
        public float bloomMaxBrightness = 1000f;
        [Range(0f, 3f)] public float bloomBoost0;
        [Range(0f, 3f)] public float bloomBoost1;
        [Range(0f, 3f)] public float bloomBoost2;
        [Range(0f, 3f)] public float bloomBoost3;
        [Range(0f, 3f)] public float bloomBoost4;
        [Range(0f, 3f)] public float bloomBoost5;
        public bool bloomAntiflicker;
        public float bloomAntiflickerMaxOutput = 10f;
        public bool bloomUltra;
        [Range(1, 10)] public int bloomUltraResolution = 10;
        [Range(0f, 5f)] public float bloomThreshold = 0.75f;
        public bool bloomConservativeThreshold;
        public bool bloomCustomize;
        [Range(0f, 1f)] public float bloomWeight0 = 0.5f;
        [Range(0f, 1f)] public float bloomWeight1 = 0.5f;
        [Range(0f, 1f)] public float bloomWeight2 = 0.5f;
        [Range(0f, 1f)] public float bloomWeight3 = 0.5f;
        [Range(0f, 1f)] public float bloomWeight4 = 0.5f;
        [Range(0f, 1f)] public float bloomWeight5 = 0.5f;
        public bool bloomBlur = true;
        [Range(3, 5)] public int bloomIterations = 4;
        public bool bloomQuickerBlur;
        public float bloomDepthAtten;
        public float bloomNearAtten;
        [Range(-1f, 1f)] public float bloomLayerZBias;
        public Color bloomTint = new Color(1f, 1f, 1f, 0f);
        public Color bloomTint0 = Color.white;
        public Color bloomTint1 = Color.white;
        public Color bloomTint2 = Color.white;
        public Color bloomTint3 = Color.white;
        public Color bloomTint4 = Color.white;
        public Color bloomTint5 = Color.white;

        #endregion

        #region Anamorphic Flares

        public bool anamorphicFlares;
        public LayerMask anamorphicFlaresCullingMask;
        [Range(1f, 4f)] public float anamorphicFlaresLayerMaskDownsampling = 1f;
        public float anamorphicFlaresIntensity = 1f;
        public bool anamorphicFlaresAntiflicker;
        public float anamorphicFlaresAntiflickerMaxOutput = 10f;
        public bool anamorphicFlaresUltra;
        [Range(1, 10)] public int anamorphicFlaresUltraResolution = 10;
        [Range(0f, 5f)] public float anamorphicFlaresThreshold = 0.75f;
        public bool anamorphicFlaresConservativeThreshold;
        [Range(0.1f, 2f)] public float anamorphicFlaresSpread = 1f;
        public bool anamorphicFlaresVertical;
        public Color anamorphicFlaresTint = new Color(0.5f, 0.5f, 1f, 0f);
        public bool anamorphicFlaresBlur = true;

        #endregion

        #region Depth of Field

        public bool depthOfField;
        public bool depthOfFieldTransparencySupport;
        public Transform depthOfFieldTargetFocus;
        public bool depthOfFieldAutofocus;
        public Vector2 depthofFieldAutofocusViewportPoint = new Vector2(0.5f, 0.5f);
        public LayerMask depthOfFieldAutofocusLayerMask = -1;
        public float depthOfFieldAutofocusMinDistance;
        public float depthOfFieldAutofocusMaxDistance = 10000;
        public float depthOfFieldAutofocusDistanceShift;
        public LayerMask depthOfFieldExclusionLayerMask = 0;
        [Range(1, 4)] public float depthOfFieldExclusionLayerMaskDownsampling = 1f;
        public UnityEngine.Rendering.CullMode depthOfFieldTransparencyCullMode = UnityEngine.Rendering.CullMode.Back;
        [Range(1, 4)] public float depthOfFieldTransparencySupportDownsampling = 1f;
        [Range(0.9f, 1f)] public float depthOfFieldExclusionBias = 0.99f;
        [Range(1f, 100f)] public float depthOfFieldDistance = 1f;
        [Range(0.001f, 1f)] public float depthOfFieldFocusSpeed = 1f;
        [Range(1, 5)] public int depthOfFieldDownsampling = 2;
        [Range(2, 16)] public int depthOfFieldMaxSamples = 4;
        public BEAUTIFY_DOF_CAMERA_SETTINGS depthOfFieldCameraSettings = BEAUTIFY_DOF_CAMERA_SETTINGS.Classic;
        [Range(0.005f, 0.5f)] public float depthOfFieldFocalLength = 0.050f;
        public float depthOfFieldAperture = 2.8f;
        [Range(1f, 300f)] public float depthOfFieldFocalLengthReal = 50f;
        [Range(1, 32)] public float depthOfFieldFStop = 2f;
        [Range(1, 48)] public float depthOfFieldImageSensorHeight = 24f;
        public bool depthOfFieldForegroundBlur = true;
        public bool depthOfFieldForegroundBlurHQ;
        [Range(0, 32)] public float depthOfFieldForegroundBlurHQSpread = 16;
        public float depthOfFieldForegroundDistance = 0.25f;
        public bool depthOfFieldBokeh = true;
        public BEAUTIFY_BOKEH_COMPOSITION depthOfFieldBokehComposition = BEAUTIFY_BOKEH_COMPOSITION.Integrated;
        [Range(0.5f, 3f)] public float depthOfFieldBokehThreshold = 1f;
        [Range(0f, 8f)] public float depthOfFieldBokehIntensity = 2f;
        public float depthOfFieldMaxBrightness = 1000f;
        public float depthOfFieldMaxDistance = 1f;
        public FilterMode depthOfFieldFilterMode = FilterMode.Bilinear;
        public LayerMask depthOfFieldTransparencyLayerMask = -1;
        public UnityEngine.Rendering.CullMode depthOfFieldExclusionCullMode = UnityEngine.Rendering.CullMode.Back;

        #endregion

        #region Eye Adaptation

        public bool eyeAdaptation = false;
        [Range(0f, 1f)] public float eyeAdaptationMinExposure = 0.2f;
        [Range(1f, 100f)] public float eyeAdaptationMaxExposure = 5f;
        [Range(0f, 1f)] public float eyeAdaptationSpeedToLight = 1f;
        [Range(0f, 1f)] public float eyeAdaptationSpeedToDark = 0.7f;

        #endregion

        #region Purkinje effect

        public bool purkinje = false;
        [Range(0f, 5f)] public float purkinjeAmount = 1f;
        [Range(0f, 1f)] public float purkinjeLuminanceThreshold = 0.15f;

        #endregion

        #region Tonemapping

        public BEAUTIFY_TMO tonemap = BEAUTIFY_TMO.Linear;
        [Range(0, 5f)] public float tonemapGamma = 2.5f;
        public float tonemapExposurePre = 1f;
        public float tonemapBrightnessPost = 1f;

        #endregion

        #region Sun Flares

        public bool sunFlares = false;
        [Range(0f, 1f)] public float sunFlaresIntensity = 1.0f;
        public float sunFlaresRevealSpeed = 1f;
        public float sunFlaresHideSpeed = 1f;
        [Range(0f, 1f)] public float sunFlaresSolarWindSpeed = 0.01f;
        public Color sunFlaresTint = new Color(1, 1, 1);
        [Range(1, 5)] public int sunFlaresDownsampling = 1;
        [Range(0f, 1f)] public float sunFlaresSunIntensity = 0.1f;
        [Range(0f, 1f)] public float sunFlaresSunDiskSize = 0.05f;
        [Range(0f, 10f)] public float sunFlaresSunRayDiffractionIntensity = 3.5f;
        [Range(0f, 1f)] public float sunFlaresSunRayDiffractionThreshold = 0.13f;
        [Range(0f, 0.2f)] public float sunFlaresCoronaRays1Length = 0.02f;
        [Range(2, 30)] public int sunFlaresCoronaRays1Streaks = 12;
        [Range(0f, 0.1f)] public float sunFlaresCoronaRays1Spread = 0.001f;
        [Range(0f, 2f * Mathf.PI)] public float sunFlaresCoronaRays1AngleOffset = 0f;
        [Range(0f, 0.2f)] public float sunFlaresCoronaRays2Length = 0.05f;
        [Range(2, 30)] public int sunFlaresCoronaRays2Streaks = 12;
        [Range(0f, 0.1f)] public float sunFlaresCoronaRays2Spread = 0.1f;
        [Range(0f, 2f * Mathf.PI)] public float sunFlaresCoronaRays2AngleOffset = 0f;
        [Range(0f, 1f)] public float sunFlaresGhosts1Size = 0.03f;
        [Range(-3f, 3f)] public float sunFlaresGhosts1Offset = 1.04f;
        [Range(0f, 1f)] public float sunFlaresGhosts1Brightness = 0.037f;
        [Range(0f, 1f)] public float sunFlaresGhosts2Size = 0.1f;
        [Range(-3f, 3f)] public float sunFlaresGhosts2Offset = 0.71f;
        [Range(0f, 1f)] public float sunFlaresGhosts2Brightness = 0.03f;
        [Range(0f, 1f)] public float sunFlaresGhosts3Size = 0.24f;
        [Range(-3f, 3f)] public float sunFlaresGhosts3Brightness = 0.025f;
        [Range(0f, 1f)] public float sunFlaresGhosts3Offset = 0.31f;
        [Range(0f, 1f)] public float sunFlaresGhosts4Size = 0.016f;
        [Range(-3f, 3f)] public float sunFlaresGhosts4Offset = 0f;
        [Range(0f, 1f)] public float sunFlaresGhosts4Brightness = 0.017f;
        [Range(0f, 1f)] public float sunFlaresHaloOffset = 0.22f;
        [Range(0f, 50f)] public float sunFlaresHaloAmplitude = 15.1415f;
        [Range(0f, 1f)] public float sunFlaresHaloIntensity = 0.01f;
        public bool sunFlaresRotationDeadZone = false;
        public float sunFlaresRadialOffset;
        #endregion

        #region Blur

        public bool blur = false;
        [Range(0f, 4f)] public float blurIntensity = 1f;

        #endregion

        #region Pixelate

        public int pixelateAmount = 1;
        public bool pixelateDownscale = false;

        #endregion

        /// <summary>
        /// Applies profile settings
        /// </summary>
        public void Load(Beautify b) {
            // down/upscale
            b.downscale = downscale;
            b.superSampling = superSampling;
            // dither
            b.dither = dither;
            b.ditherDepth = ditherDepth;
            // sharpen
            b.sharpenMinDepth = sharpenMinDepth;
            b.sharpenMaxDepth = sharpenMaxDepth;
            b.sharpenMinMaxDepthFallOff = sharpenMinMaxDepthFallOff;
            b.sharpen = sharpen;
            b.sharpenDepthThreshold = sharpenDepthThreshold;
            b.tintColor = tintColor;
            b.sharpenRelaxation = sharpenRelaxation;
            b.sharpenClamp = sharpenClamp;
            b.sharpenMotionSensibility = sharpenMotionSensibility;
            b.sharpenMotionRestoreSpeed = sharpenMotionRestoreSpeed;
            // aa
            b.antialiasStrength = antialiasStrength;
            b.antialiasMaxSpread = antialiasMaxSpread;
            b.antialiasDepthThreshold = antialiasDepthThreshold;
            b.antialiasDepthAtten = antialiasDepthAtten;
            // color grading
            b.saturate = saturate;
            b.contrast = contrast;
            b.brightness = brightness;
            b.daltonize = daltonize;
            b.hardLightBlend = hardLightBlend;
            b.hardLightIntensity = hardLightIntensity;
            // frame
            b.frame = frame;
            b.frameColor = frameColor;
            b.frameMask = frameMask;
            b.frameStyle = frameStyle;
            b.frameBandHorizontalSize = frameBandHorizontalSize;
            b.frameBandHorizontalSmoothness = frameBandHorizontalSmoothness;
            b.frameBandVerticalSize = frameBandVerticalSize;
            b.frameBandVerticalSmoothness = frameBandVerticalSmoothness;
            // lut
            b.lut = lut;
            b.lutTexture = lutTexture;
            b.lutTexture3D = lutTexture3D;
            b.lutIntensity = lutIntensity;
            // night vision
            b.nightVision = nightVision;
            b.nightVisionColor = nightVisionColor;
            // outline
            b.outline = outline;
            b.outlineColor = outlineColor;
            b.outlineCustomize = outlineCustomize;
            b.outlineBlurPassCount = outlineBlurPassCount;
            b.outlineBlurDownscale = outlineBlurDownscale;
            b.outlineIntensityMultiplier = outlineIntensityMultiplier;
            b.outlineSpread = outlineSpread;
            b.outlineStage = outlineStage;
            b.outlineMinDepthThreshold = outlineMinDepthThreshold;
            // thermal vision
            b.thermalVision = thermalVision;
            // vignetting (must load these settings after night vision & thermal vision)
            b.vignetting = vignetting;
            b.vignettingColor = vignettingColor;
            b.vignettingFade = vignettingFade;
            b.vignettingCircularShape = vignettingCircularShape;
            b.vignettingAspectRatio = vignettingAspectRatio;
            b.vignettingBlink = vignettingBlink;
            b.vignettingBlinkStyle = vignettingBlinkStyle;
            b.vignettingMask = vignettingMask;
            b.vignettingCenter = vignettingCenter;
            // lens dirt
            b.lensDirt = lensDirt;
            b.lensDirtThreshold = lensDirtThreshold;
            b.lensDirtIntensity = lensDirtIntensity;
            b.lensDirtTexture = lensDirtTexture;
            // chromatic aberration
            b.chromaticAberration = chromaticAberration;
            b.chromaticAberrationIntensity = chromaticAberrationIntensity;
            b.chromaticAberrationSmoothing = chromaticAberrationSmoothing;
            // bloom
            b.bloom = bloom;
            b.bloomCullingMask = bloomCullingMask;
            b.bloomLayerMaskDownsampling = bloomLayerMaskDownsampling;
            b.bloomIntensity = bloomIntensity;
            b.bloomMaxBrightness = bloomMaxBrightness;
            b.bloomBoost0 = bloomBoost0;
            b.bloomBoost1 = bloomBoost1;
            b.bloomBoost2 = bloomBoost2;
            b.bloomBoost3 = bloomBoost3;
            b.bloomBoost4 = bloomBoost4;
            b.bloomBoost5 = bloomBoost5;
            b.bloomAntiflicker = bloomAntiflicker;
            b.bloomAntiflickerMaxOutput = bloomAntiflickerMaxOutput;
            b.bloomUltra = bloomUltra;
            b.bloomUltraResolution = bloomUltraResolution;
            b.bloomThreshold = bloomThreshold;
            b.bloomConservativeThreshold = bloomConservativeThreshold;
            b.bloomCustomize = bloomCustomize;
            b.bloomWeight0 = bloomWeight0;
            b.bloomWeight1 = bloomWeight1;
            b.bloomWeight2 = bloomWeight2;
            b.bloomWeight3 = bloomWeight3;
            b.bloomWeight4 = bloomWeight4;
            b.bloomWeight5 = bloomWeight5;
            b.bloomBlur = bloomBlur;
            b.bloomQuickerBlur = bloomQuickerBlur;
            b.bloomDepthAtten = bloomDepthAtten;
            b.bloomNearAtten = bloomNearAtten;
            b.bloomLayerZBias = bloomLayerZBias;
            b.bloomTint = bloomTint;
            b.bloomTint0 = bloomTint0;
            b.bloomTint1 = bloomTint1;
            b.bloomTint2 = bloomTint2;
            b.bloomTint3 = bloomTint3;
            b.bloomTint4 = bloomTint4;
            b.bloomTint5 = bloomTint5;
            b.bloomIterations = bloomIterations;
            // anamorphic flares
            b.anamorphicFlares = anamorphicFlares;
            b.anamorphicFlaresCullingMask = anamorphicFlaresCullingMask;
            b.anamorphicFlaresLayerMaskDownsampling = anamorphicFlaresLayerMaskDownsampling;
            b.anamorphicFlaresIntensity = anamorphicFlaresIntensity;
            b.anamorphicFlaresAntiflicker = anamorphicFlaresAntiflicker;
            b.anamorphicFlaresAntiflickerMaxOutput = anamorphicFlaresAntiflickerMaxOutput;
            b.anamorphicFlaresUltra = anamorphicFlaresUltra;
            b.anamorphicFlaresUltraResolution = anamorphicFlaresUltraResolution;
            b.anamorphicFlaresThreshold = anamorphicFlaresThreshold;
            b.anamorphicFlaresSpread = anamorphicFlaresSpread;
            b.anamorphicFlaresVertical = anamorphicFlaresVertical;
            b.anamorphicFlaresTint = anamorphicFlaresTint;
            b.anamorphicFlaresBlur = anamorphicFlaresBlur;
            // dof
            b.depthOfField = depthOfField;
            b.depthOfFieldTransparencySupport = depthOfFieldTransparencySupport;
            b.depthOfFieldTargetFocus = depthOfFieldTargetFocus;
            b.depthOfFieldAutofocus = depthOfFieldAutofocus;
            b.depthofFieldAutofocusViewportPoint = depthofFieldAutofocusViewportPoint;
            b.depthOfFieldAutofocusLayerMask = depthOfFieldAutofocusLayerMask;
            b.depthOfFieldAutofocusMinDistance = depthOfFieldAutofocusMinDistance;
            b.depthOfFieldAutofocusMaxDistance = depthOfFieldAutofocusMaxDistance;
            b.depthOfFieldAutofocusDistanceShift = depthOfFieldAutofocusDistanceShift;
            b.depthOfFieldExclusionLayerMask = depthOfFieldExclusionLayerMask;
            b.depthOfFieldExclusionLayerMaskDownsampling = depthOfFieldExclusionLayerMaskDownsampling;
            b.depthOfFieldExclusionCullMode = depthOfFieldExclusionCullMode;
            b.depthOfFieldTransparencySupportDownsampling = depthOfFieldTransparencySupportDownsampling;
            b.depthOfFieldExclusionBias = depthOfFieldExclusionBias;
            b.depthOfFieldDistance = depthOfFieldDistance;
            b.depthOfFieldFocusSpeed = depthOfFieldFocusSpeed;
            b.depthOfFieldDownsampling = depthOfFieldDownsampling;
            b.depthOfFieldMaxSamples = depthOfFieldMaxSamples;
            b.depthOfFieldCameraSettings = depthOfFieldCameraSettings;
            b.depthOfFieldFocalLength = depthOfFieldFocalLength;
            b.depthOfFieldAperture = depthOfFieldAperture;
            b.depthOfFieldFocalLengthReal = depthOfFieldFocalLengthReal;
            b.depthOfFieldFStop = depthOfFieldFStop;
            b.depthOfFieldImageSensorHeight = depthOfFieldImageSensorHeight;
            b.depthOfFieldForegroundBlur = depthOfFieldForegroundBlur;
            b.depthOfFieldForegroundBlurHQ = depthOfFieldForegroundBlurHQ;
            b.depthOfFieldForegroundBlurHQSpread = depthOfFieldForegroundBlurHQSpread;
            b.depthOfFieldForegroundDistance = depthOfFieldForegroundDistance;
            b.depthOfFieldBokeh = depthOfFieldBokeh;
            b.depthOfFieldBokehComposition = depthOfFieldBokehComposition;
            b.depthOfFieldBokehThreshold = depthOfFieldBokehThreshold;
            b.depthOfFieldBokehIntensity = depthOfFieldBokehIntensity;
            b.depthOfFieldMaxBrightness = depthOfFieldMaxBrightness;
            b.depthOfFieldMaxDistance = depthOfFieldMaxDistance;
            b.depthOfFieldFilterMode = depthOfFieldFilterMode;
            b.depthOfFieldTransparencyLayerMask = depthOfFieldTransparencyLayerMask;
            b.depthOfFieldTransparencyCullMode = depthOfFieldTransparencyCullMode;
            // ea
            b.eyeAdaptation = eyeAdaptation;
            b.eyeAdaptationMaxExposure = eyeAdaptationMaxExposure;
            b.eyeAdaptationMinExposure = eyeAdaptationMinExposure;
            b.eyeAdaptationSpeedToDark = eyeAdaptationSpeedToDark;
            b.eyeAdaptationSpeedToLight = eyeAdaptationSpeedToLight;
            // purkinje
            b.purkinje = purkinje;
            b.purkinjeAmount = purkinjeAmount;
            b.purkinjeLuminanceThreshold = purkinjeLuminanceThreshold;
            // tonemap
            b.tonemap = tonemap;
            b.tonemapGamma = tonemapGamma;
            b.tonemapExposurePre = tonemapExposurePre;
            b.tonemapBrightnessPost = tonemapBrightnessPost;
            // flares
            b.sunFlares = sunFlares;
            b.sunFlaresIntensity = sunFlaresIntensity;
            b.sunFlaresRevealSpeed = sunFlaresRevealSpeed;
            b.sunFlaresHideSpeed = sunFlaresHideSpeed;
            b.sunFlaresSolarWindSpeed = sunFlaresSolarWindSpeed;
            b.sunFlaresTint = sunFlaresTint;
            b.sunFlaresDownsampling = sunFlaresDownsampling;
            b.sunFlaresSunIntensity = sunFlaresSunIntensity;
            b.sunFlaresSunDiskSize = sunFlaresSunDiskSize;
            b.sunFlaresSunRayDiffractionIntensity = sunFlaresSunRayDiffractionIntensity;
            b.sunFlaresSunRayDiffractionThreshold = sunFlaresSunRayDiffractionThreshold;
            b.sunFlaresCoronaRays1Length = sunFlaresCoronaRays1Length;
            b.sunFlaresCoronaRays1Spread = sunFlaresCoronaRays1Spread;
            b.sunFlaresCoronaRays1AngleOffset = sunFlaresCoronaRays1AngleOffset;
            b.sunFlaresCoronaRays1Streaks = sunFlaresCoronaRays1Streaks;
            b.sunFlaresCoronaRays2Length = sunFlaresCoronaRays2Length;
            b.sunFlaresCoronaRays2Spread = sunFlaresCoronaRays2Spread;
            b.sunFlaresCoronaRays2AngleOffset = sunFlaresCoronaRays2AngleOffset;
            b.sunFlaresCoronaRays2Streaks = sunFlaresCoronaRays2Streaks;
            b.sunFlaresGhosts1Size = sunFlaresGhosts1Size;
            b.sunFlaresGhosts1Offset = sunFlaresGhosts1Offset;
            b.sunFlaresGhosts1Brightness = sunFlaresGhosts1Brightness;
            b.sunFlaresGhosts2Size = sunFlaresGhosts2Size;
            b.sunFlaresGhosts2Offset = sunFlaresGhosts2Offset;
            b.sunFlaresGhosts2Brightness = sunFlaresGhosts2Brightness;
            b.sunFlaresGhosts3Size = sunFlaresGhosts3Size;
            b.sunFlaresGhosts3Offset = sunFlaresGhosts3Offset;
            b.sunFlaresGhosts3Brightness = sunFlaresGhosts3Brightness;
            b.sunFlaresGhosts4Size = sunFlaresGhosts4Size;
            b.sunFlaresGhosts4Offset = sunFlaresGhosts4Offset;
            b.sunFlaresGhosts4Brightness = sunFlaresGhosts4Brightness;
            b.sunFlaresHaloOffset = sunFlaresHaloOffset;
            b.sunFlaresHaloAmplitude = sunFlaresHaloAmplitude;
            b.sunFlaresHaloIntensity = sunFlaresHaloIntensity;
            b.sunFlaresRotationDeadZone = sunFlaresRotationDeadZone;
            b.sunFlaresRadialOffset = sunFlaresRadialOffset;
            // blur
            b.blur = blur;
            b.blurIntensity = blurIntensity;
            // pixelate
            b.pixelateAmount = pixelateAmount;
            b.pixelateDownscale = pixelateDownscale;
        }

        /// <summary>
        /// Replaces profile settings with current Beautify configuration
        /// </summary>
        public void Save(Beautify b) {
            // down/upscale
            downscale = b.downscale;
            superSampling = b.superSampling;
            // dither
            dither = b.dither;
            ditherDepth = b.ditherDepth;
            // sharpen
            sharpenMinDepth = b.sharpenMinDepth;
            sharpenMaxDepth = b.sharpenMaxDepth;
            sharpenMinMaxDepthFallOff = b.sharpenMinMaxDepthFallOff;
            sharpen = b.sharpen;
            sharpenDepthThreshold = b.sharpenDepthThreshold;
            tintColor = b.tintColor;
            sharpenRelaxation = b.sharpenRelaxation;
            sharpenClamp = b.sharpenClamp;
            sharpenMotionSensibility = b.sharpenMotionSensibility;
            sharpenMotionRestoreSpeed = b.sharpenMotionRestoreSpeed;
            // aa
            antialiasStrength = b.antialiasStrength;
            antialiasMaxSpread = b.antialiasMaxSpread;
            antialiasDepthThreshold = b.antialiasDepthThreshold;
            antialiasDepthAtten = b.antialiasDepthAtten;
            // color grading
            saturate = b.saturate;
            contrast = b.contrast;
            brightness = b.brightness;
            daltonize = b.daltonize;
            hardLightBlend = b.hardLightBlend;
            hardLightIntensity = b.hardLightIntensity;
            // vignetting
            vignetting = b.vignetting;
            vignettingColor = b.vignettingColor;
            vignettingFade = b.vignettingFade;
            vignettingCircularShape = b.vignettingCircularShape;
            vignettingMask = b.vignettingMask;
            vignettingAspectRatio = b.vignettingAspectRatio;
            vignettingBlink = b.vignettingBlink;
            vignettingCenter = b.vignettingCenter;
            vignettingBlinkStyle = b.vignettingBlinkStyle;
            // frame
            frame = b.frame;
            frameColor = b.frameColor;
            frameMask = b.frameMask;
            frameStyle = b.frameStyle;
            frameBandHorizontalSize = b.frameBandHorizontalSize;
            frameBandHorizontalSmoothness = b.frameBandHorizontalSmoothness;
            frameBandVerticalSize = b.frameBandVerticalSize;
            frameBandVerticalSmoothness = b.frameBandVerticalSmoothness;
            // lut
            lut = b.lut;
            lutTexture = b.lutTexture;
            lutTexture3D = b.lutTexture3D;
            lutIntensity = b.lutIntensity;
            // night vision
            nightVision = b.nightVision;
            nightVisionColor = b.nightVisionColor;
            // outline
            outline = b.outline;
            outlineColor = b.outlineColor;
            outlineCustomize = b.outlineCustomize;
            outlineBlurPassCount = b.outlineBlurPassCount;
            outlineIntensityMultiplier = b.outlineIntensityMultiplier;
            outlineSpread = b.outlineSpread;
            outlineBlurDownscale = b.outlineBlurDownscale;
            outlineStage = b.outlineStage;
            outlineMinDepthThreshold = b.outlineMinDepthThreshold;
            // thermal vision
            thermalVision = b.thermalVision;
            // lens dirt
            lensDirt = b.lensDirt;
            lensDirtThreshold = b.lensDirtThreshold;
            lensDirtIntensity = b.lensDirtIntensity;
            lensDirtTexture = b.lensDirtTexture;
            // chromatic aberration
            chromaticAberration = b.chromaticAberration;
            chromaticAberrationIntensity = b.chromaticAberrationIntensity;
            chromaticAberrationSmoothing = b.chromaticAberrationSmoothing;
            // bloom
            bloom = b.bloom;
            bloomCullingMask = b.bloomCullingMask;
            bloomLayerMaskDownsampling = b.bloomLayerMaskDownsampling;
            bloomIntensity = b.bloomIntensity;
            bloomMaxBrightness = b.bloomMaxBrightness;
            bloomBoost0 = b.bloomBoost0;
            bloomBoost1 = b.bloomBoost1;
            bloomBoost2 = b.bloomBoost2;
            bloomBoost3 = b.bloomBoost3;
            bloomBoost4 = b.bloomBoost4;
            bloomBoost5 = b.bloomBoost5;
            bloomAntiflicker = b.bloomAntiflicker;
            bloomUltra = b.bloomUltra;
            bloomUltraResolution = b.bloomUltraResolution;
            bloomThreshold = b.bloomThreshold;
            bloomConservativeThreshold = b.bloomConservativeThreshold;
            bloomCustomize = b.bloomCustomize;
            bloomWeight0 = b.bloomWeight0;
            bloomWeight1 = b.bloomWeight1;
            bloomWeight2 = b.bloomWeight2;
            bloomWeight3 = b.bloomWeight3;
            bloomWeight4 = b.bloomWeight4;
            bloomWeight5 = b.bloomWeight5;
            bloomBlur = b.bloomBlur;
            bloomQuickerBlur = b.bloomQuickerBlur;
            bloomDepthAtten = b.bloomDepthAtten;
            bloomNearAtten = b.bloomNearAtten;
            bloomLayerZBias = b.bloomLayerZBias;
            bloomTint = b.bloomTint;
            bloomTint0 = b.bloomTint0;
            bloomTint1 = b.bloomTint1;
            bloomTint2 = b.bloomTint2;
            bloomTint3 = b.bloomTint3;
            bloomTint4 = b.bloomTint4;
            bloomTint5 = b.bloomTint5;
            bloomIterations = b.bloomIterations;
            // anamorphic flares
            anamorphicFlares = b.anamorphicFlares;
            anamorphicFlaresCullingMask = b.anamorphicFlaresCullingMask;
            anamorphicFlaresLayerMaskDownsampling = b.anamorphicFlaresLayerMaskDownsampling;
            anamorphicFlaresIntensity = b.anamorphicFlaresIntensity;
            anamorphicFlaresAntiflicker = b.anamorphicFlaresAntiflicker;
            anamorphicFlaresUltraResolution = b.anamorphicFlaresUltraResolution;
            anamorphicFlaresUltra = b.anamorphicFlaresUltra;
            anamorphicFlaresThreshold = b.anamorphicFlaresThreshold;
            anamorphicFlaresSpread = b.anamorphicFlaresSpread;
            anamorphicFlaresVertical = b.anamorphicFlaresVertical;
            anamorphicFlaresTint = b.anamorphicFlaresTint;
            anamorphicFlaresBlur = b.anamorphicFlaresBlur;
            // dof
            depthOfField = b.depthOfField;
            depthOfFieldTransparencySupport = b.depthOfFieldTransparencySupport;
            depthOfFieldTargetFocus = b.depthOfFieldTargetFocus;
            depthOfFieldAutofocus = b.depthOfFieldAutofocus;
            depthofFieldAutofocusViewportPoint = b.depthofFieldAutofocusViewportPoint;
            depthOfFieldAutofocusLayerMask = b.depthOfFieldAutofocusLayerMask;
            depthOfFieldAutofocusMinDistance = b.depthOfFieldAutofocusMinDistance;
            depthOfFieldAutofocusMaxDistance = b.depthOfFieldAutofocusMaxDistance;
            depthOfFieldAutofocusDistanceShift = b.depthOfFieldAutofocusDistanceShift;
            depthOfFieldExclusionLayerMask = b.depthOfFieldExclusionLayerMask;
            depthOfFieldExclusionLayerMaskDownsampling = b.depthOfFieldExclusionLayerMaskDownsampling;
            depthOfFieldExclusionCullMode = b.depthOfFieldExclusionCullMode;
            depthOfFieldTransparencySupportDownsampling = b.depthOfFieldTransparencySupportDownsampling;
            depthOfFieldExclusionBias = b.depthOfFieldExclusionBias;
            depthOfFieldDistance = b.depthOfFieldDistance;
            depthOfFieldFocusSpeed = b.depthOfFieldFocusSpeed;
            depthOfFieldDownsampling = b.depthOfFieldDownsampling;
            depthOfFieldMaxSamples = b.depthOfFieldMaxSamples;
            depthOfFieldFocalLength = b.depthOfFieldFocalLength;
            depthOfFieldCameraSettings = b.depthOfFieldCameraSettings;
            depthOfFieldAperture = b.depthOfFieldAperture;
            depthOfFieldFocalLengthReal = b.depthOfFieldFocalLengthReal;
            depthOfFieldFStop = b.depthOfFieldFStop;
            depthOfFieldImageSensorHeight = b.depthOfFieldImageSensorHeight;
            depthOfFieldForegroundBlur = b.depthOfFieldForegroundBlur;
            depthOfFieldForegroundBlurHQ = b.depthOfFieldForegroundBlurHQ;
            depthOfFieldForegroundBlurHQSpread = b.depthOfFieldForegroundBlurHQSpread;
            depthOfFieldForegroundDistance = b.depthOfFieldForegroundDistance;
            depthOfFieldBokeh = b.depthOfFieldBokeh;
            depthOfFieldBokehComposition = b.depthOfFieldBokehComposition;
            depthOfFieldBokehThreshold = b.depthOfFieldBokehThreshold;
            depthOfFieldBokehIntensity = b.depthOfFieldBokehIntensity;
            depthOfFieldMaxBrightness = b.depthOfFieldMaxBrightness;
            depthOfFieldMaxDistance = b.depthOfFieldMaxDistance;
            depthOfFieldFilterMode = b.depthOfFieldFilterMode;
            depthOfFieldTransparencyLayerMask = b.depthOfFieldTransparencyLayerMask;
            depthOfFieldTransparencyCullMode = b.depthOfFieldTransparencyCullMode;
            // ea
            eyeAdaptation = b.eyeAdaptation;
            eyeAdaptationMaxExposure = b.eyeAdaptationMaxExposure;
            eyeAdaptationMinExposure = b.eyeAdaptationMinExposure;
            eyeAdaptationSpeedToDark = b.eyeAdaptationSpeedToDark;
            eyeAdaptationSpeedToLight = b.eyeAdaptationSpeedToLight;
            // purkinje
            purkinje = b.purkinje;
            purkinjeAmount = b.purkinjeAmount;
            purkinjeLuminanceThreshold = b.purkinjeLuminanceThreshold;
            // tonemap
            tonemap = b.tonemap;
            tonemapGamma = b.tonemapGamma;
            tonemapExposurePre = b.tonemapExposurePre;
            tonemapBrightnessPost = b.tonemapBrightnessPost;
            // flares
            sunFlares = b.sunFlares;
            sunFlaresIntensity = b.sunFlaresIntensity;
            sunFlaresRevealSpeed = b.sunFlaresRevealSpeed;
            sunFlaresHideSpeed = b.sunFlaresHideSpeed;
            sunFlaresSolarWindSpeed = b.sunFlaresSolarWindSpeed;
            sunFlaresTint = b.sunFlaresTint;
            sunFlaresDownsampling = b.sunFlaresDownsampling;
            sunFlaresSunIntensity = b.sunFlaresSunIntensity;
            sunFlaresSunDiskSize = b.sunFlaresSunDiskSize;
            sunFlaresSunRayDiffractionIntensity = b.sunFlaresSunRayDiffractionIntensity;
            sunFlaresSunRayDiffractionThreshold = b.sunFlaresSunRayDiffractionThreshold;
            sunFlaresCoronaRays1Length = b.sunFlaresCoronaRays1Length;
            sunFlaresCoronaRays1Spread = b.sunFlaresCoronaRays1Spread;
            sunFlaresCoronaRays1AngleOffset = b.sunFlaresCoronaRays1AngleOffset;
            sunFlaresCoronaRays1Streaks = b.sunFlaresCoronaRays1Streaks;
            sunFlaresCoronaRays2Length = b.sunFlaresCoronaRays2Length;
            sunFlaresCoronaRays2Spread = b.sunFlaresCoronaRays2Spread;
            sunFlaresCoronaRays2AngleOffset = b.sunFlaresCoronaRays2AngleOffset;
            sunFlaresCoronaRays2Streaks = b.sunFlaresCoronaRays2Streaks;
            sunFlaresGhosts1Size = b.sunFlaresGhosts1Size;
            sunFlaresGhosts1Offset = b.sunFlaresGhosts1Offset;
            sunFlaresGhosts1Brightness = b.sunFlaresGhosts1Brightness;
            sunFlaresGhosts2Size = b.sunFlaresGhosts2Size;
            sunFlaresGhosts2Offset = b.sunFlaresGhosts2Offset;
            sunFlaresGhosts2Brightness = b.sunFlaresGhosts2Brightness;
            sunFlaresGhosts3Size = b.sunFlaresGhosts3Size;
            sunFlaresGhosts3Offset = b.sunFlaresGhosts3Offset;
            sunFlaresGhosts3Brightness = b.sunFlaresGhosts3Brightness;
            sunFlaresGhosts4Size = b.sunFlaresGhosts4Size;
            sunFlaresGhosts4Offset = b.sunFlaresGhosts4Offset;
            sunFlaresGhosts4Brightness = b.sunFlaresGhosts4Brightness;
            sunFlaresHaloOffset = b.sunFlaresHaloOffset;
            sunFlaresHaloAmplitude = b.sunFlaresHaloAmplitude;
            sunFlaresHaloIntensity = b.sunFlaresHaloIntensity;
            sunFlaresRotationDeadZone = b.sunFlaresRotationDeadZone;
            sunFlaresRadialOffset = b.sunFlaresRadialOffset;
            // blur
            blur = b.blur;
            blurIntensity = b.blurIntensity;
            // pixelate
            pixelateAmount = b.pixelateAmount;
            pixelateDownscale = b.pixelateDownscale;
        }

        private void OnValidate() {
            bloomIntensity = Mathf.Max(0, bloomIntensity);
            bloomAntiflickerMaxOutput = Mathf.Max(0, bloomAntiflickerMaxOutput);
            bloomDepthAtten = Mathf.Max(0, bloomDepthAtten);
            anamorphicFlaresIntensity = Mathf.Max(0, anamorphicFlaresIntensity);
            anamorphicFlaresAntiflickerMaxOutput = Mathf.Max(0, anamorphicFlaresAntiflickerMaxOutput);
            antialiasDepthAtten = Mathf.Max(0, antialiasDepthAtten);
            sunFlaresRadialOffset = Mathf.Max(0, sunFlaresRadialOffset);
        }
    }

}