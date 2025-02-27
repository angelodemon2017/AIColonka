//#define DEBUG_BEAUTIFY
#define USE_CAMERA_DEPTH_TEXTURE

/// <summary>
/// Copyright 2016-2024 Ramiro Oliva (Kronnect) - All rights reserved
/// </summary>
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace BeautifyEffect {

    public delegate float OnBeforeFocusEvent (float currentFocusDistance);

    public enum BEAUTIFY_QUALITY {
        BestQuality,
        BestPerformance,
        Basic
    }

    public enum BEAUTIFY_PRESET {
        Soft = 10,
        Medium = 20,
        Strong = 30,
        Exaggerated = 40,
        Custom = 999
    }


    public enum BEAUTIFY_COMPARE_STYLE {
        FreeAngle,
        VerticalLine,
        SameSide
    }

    public enum BEAUTIFY_TMO {
        Linear = 0,
        ACES = 10,
        AGX = 20
    }

    public enum BEAUTIFY_PRERENDER_EVENT {
        OnPreCull = 0,
        OnPreRender = 1
    }

    public enum BEAUTIFY_BLINK_STYLE {
        Cutscene = 0,
        Human = 1
    }

    public enum BEAUTIFY_OUTLINE_STAGE {
        BeforeBloom = 0,
        AfterBloom = 10
    }

    public enum BEAUTIFY_BOKEH_COMPOSITION {
        Integrated,
        Separated
    }

    public enum BEAUTIFY_DOF_CAMERA_SETTINGS {
        Classic,
        Real
    }


    [ExecuteInEditMode, RequireComponent(typeof(Camera))]
    [AddComponentMenu("Image Effects/Rendering/Beautify")]
    [HelpURL("https://kronnect.com/support")]
    [ImageEffectAllowedInSceneView]
    public partial class Beautify : MonoBehaviour {


        #region General settings

        [SerializeField]
        BEAUTIFY_PRESET
            _preset = BEAUTIFY_PRESET.Medium;

        public BEAUTIFY_PRESET preset {
            get { return _preset; }
            set {
                if (_preset != value) {
                    _preset = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        BEAUTIFY_QUALITY
            _quality = BEAUTIFY_QUALITY.BestQuality;

        public BEAUTIFY_QUALITY quality {
            get { return _quality; }
            set {
                if (_quality != value) {
                    _quality = value;
                    UpdateQualitySettings();
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        BeautifyProfile _profile;

        public BeautifyProfile profile {
            get { return _profile; }
            set {
                if (_profile != value) {
                    _profile = value;
                    if (_profile != null) {
                        _profile.Load(this);
                        _preset = BEAUTIFY_PRESET.Custom;
                    }
                }
            }
        }

        [SerializeField]
        bool _syncWithProfile = true;

        public bool syncWithProfile {
            get { return _syncWithProfile; }
            set {
                _syncWithProfile = value;
            }
        }

        [SerializeField]
        bool
            _compareMode;

        public bool compareMode {
            get { return _compareMode; }
            set {
                if (_compareMode != value) {
                    _compareMode = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        BEAUTIFY_COMPARE_STYLE
            _compareStyle = BEAUTIFY_COMPARE_STYLE.FreeAngle;

        public BEAUTIFY_COMPARE_STYLE compareStyle {
            get { return _compareStyle; }
            set {
                if (_compareStyle != value) {
                    _compareStyle = value;
                    UpdateMaterialProperties();
                }
            }
        }



        [SerializeField]
        [Range(0, 0.5f)]
        float
            _comparePanning = 0.25f;

        public float comparePanning {
            get { return _comparePanning; }
            set {
                if (_comparePanning != value) {
                    _comparePanning = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        [Range(-Mathf.PI, Mathf.PI)]
        float
            _compareLineAngle = 1.4f;

        public float compareLineAngle {
            get { return _compareLineAngle; }
            set {
                if (_compareLineAngle != value) {
                    _compareLineAngle = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0.0001f, 0.05f)]
        float
            _compareLineWidth = 0.002f;

        public float compareLineWidth {
            get { return _compareLineWidth; }
            set {
                if (_compareLineWidth != value) {
                    _compareLineWidth = value;
                    UpdateMaterialProperties();
                }
            }
        }

        #endregion

        #region RGB Dither

        [SerializeField]
        [Range(0, 0.2f)]
        float
            _dither = 0.02f;

        public float dither {
            get { return _dither; }
            set {
                if (_dither != value) {
                    _preset = BEAUTIFY_PRESET.Custom;
                    _dither = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0, 1f)]
        float
            _ditherDepth = 0f;

        public float ditherDepth {
            get { return _ditherDepth; }
            set {
                if (_ditherDepth != value) {
                    _preset = BEAUTIFY_PRESET.Custom;
                    _ditherDepth = value;
                    UpdateMaterialProperties();
                }
            }
        }

        #endregion

        #region Sharpen Settings


        [SerializeField]
        [Range(0, 1f)]
        float
            _sharpenMinDepth = 0f;

        public float sharpenMinDepth {
            get { return _sharpenMinDepth; }
            set {
                if (_sharpenMinDepth != value) {
                    _sharpenMinDepth = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0, 1.1f)]
        float
            _sharpenMaxDepth = 0.999f;

        public float sharpenMaxDepth {
            get { return _sharpenMaxDepth; }
            set {
                if (_sharpenMaxDepth != value) {
                    _sharpenMaxDepth = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0, 1f)]
        float
        _sharpenMinMaxDepthFallOff = 0f;

        public float sharpenMinMaxDepthFallOff {
            get { return _sharpenMinMaxDepthFallOff; }
            set {
                if (_sharpenMinMaxDepthFallOff != value) {
                    _sharpenMinMaxDepthFallOff = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        [Range(0f, 15f)]
        float
            _sharpen = 2f;

        public float sharpen {
            get { return _sharpen; }
            set {
                if (_sharpen != value) {
                    _preset = BEAUTIFY_PRESET.Custom;
                    _sharpen = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 0.05f)]
        float
            _sharpenDepthThreshold = 0.035f;

        public float sharpenDepthThreshold {
            get { return _sharpenDepthThreshold; }
            set {
                if (_sharpenDepthThreshold != value) {
                    _preset = BEAUTIFY_PRESET.Custom;
                    _sharpenDepthThreshold = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        Color
            _tintColor = new Color(1, 1, 1, 0);

        public Color tintColor {
            get { return _tintColor; }
            set {
                if (_tintColor != value) {
                    _tintColor = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 0.2f)]
        float
            _sharpenRelaxation = 0.08f;

        public float sharpenRelaxation {
            get { return _sharpenRelaxation; }
            set {
                if (_sharpenRelaxation != value) {
                    _preset = BEAUTIFY_PRESET.Custom;
                    _sharpenRelaxation = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0, 1f)]
        float
            _sharpenClamp = 0.45f;

        public float sharpenClamp {
            get { return _sharpenClamp; }
            set {
                if (_sharpenClamp != value) {
                    _preset = BEAUTIFY_PRESET.Custom;
                    _sharpenClamp = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0, 1f)]
        float
            _sharpenMotionSensibility = 0.5f;

        public float sharpenMotionSensibility {
            get { return _sharpenMotionSensibility; }
            set {
                if (_sharpenMotionSensibility != value) {
                    _sharpenMotionSensibility = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        [Range(0.01f, 5f)]
        float
            _sharpenMotionRestoreSpeed = 0.5f;

        public float sharpenMotionRestoreSpeed {
            get { return _sharpenMotionRestoreSpeed; }
            set {
                if (_sharpenMotionRestoreSpeed != value) {
                    _sharpenMotionRestoreSpeed = value;
                    UpdateMaterialProperties();
                }
            }
        }

        #endregion

        #region Color grading

        [SerializeField]
        [Range(-2f, 3f)]
        float
            _saturate = 1f;

        public float saturate {
            get { return _saturate; }
            set {
                if (_saturate != value) {
                    _preset = BEAUTIFY_PRESET.Custom;
                    _saturate = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0.5f, 1.5f)]
        float
            _contrast = 1.02f;

        public float contrast {
            get { return _contrast; }
            set {
                if (_contrast != value) {
                    _preset = BEAUTIFY_PRESET.Custom;
                    _contrast = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        float
            _brightness = 1.05f;

        public float brightness {
            get { return _brightness; }
            set {
                if (_brightness != value) {
                    _preset = BEAUTIFY_PRESET.Custom;
                    _brightness = Mathf.Max(0, value);
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 2f)]
        float
            _daltonize = 0f;

        public float daltonize {
            get { return _daltonize; }
            set {
                if (_daltonize != value) {
                    _preset = BEAUTIFY_PRESET.Custom;
                    _daltonize = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        [Range(0, 1f)]
        float
            _hardLightIntensity = 0.5f;

        public float hardLightIntensity {
            get { return _hardLightIntensity; }
            set {
                if (_hardLightIntensity != value) {
                    _preset = BEAUTIFY_PRESET.Custom;
                    _hardLightIntensity = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0, 1f)]
        float _hardLightBlend;

        public float hardLightBlend {
            get { return _hardLightBlend; }
            set {
                if (_hardLightBlend != value) {
                    _preset = BEAUTIFY_PRESET.Custom;
                    _hardLightBlend = value;
                    UpdateMaterialProperties();
                }
            }
        }


        #endregion

        #region Vignetting

        [SerializeField]
        bool
            _vignetting = false;

        public bool vignetting {
            get { return _vignetting; }
            set {
                if (_vignetting != value) {
                    _vignetting = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        Color
            _vignettingColor = new Color(0.3f, 0.3f, 0.3f, 0.05f);

        public Color vignettingColor {
            get { return _vignettingColor; }
            set {
                if (_vignettingColor != value) {
                    _vignettingColor = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0, 1)]
        float
            _vignettingFade = 0;

        public float vignettingFade {
            get { return _vignettingFade; }
            set {
                if (_vignettingFade != value) {
                    _vignettingFade = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        bool
            _vignettingCircularShape = false;

        public bool vignettingCircularShape {
            get { return _vignettingCircularShape; }
            set {
                if (_vignettingCircularShape != value) {
                    _vignettingCircularShape = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        float
            _vignettingAspectRatio = 1.0f;

        public float vignettingAspectRatio {
            get { return _vignettingAspectRatio; }
            set {
                if (_vignettingAspectRatio != value) {
                    _vignettingAspectRatio = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField, Range(0, 1f)]
        float
            _vignettingBlink;

        public float vignettingBlink {
            get { return _vignettingBlink; }
            set {
                if (_vignettingBlink != value) {
                    _vignettingBlink = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        BEAUTIFY_BLINK_STYLE _vignettingBlinkStyle = BEAUTIFY_BLINK_STYLE.Cutscene;

        public BEAUTIFY_BLINK_STYLE vignettingBlinkStyle {
            get { return _vignettingBlinkStyle; }
            set {
                if (_vignettingBlinkStyle != value) {
                    _vignettingBlinkStyle = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        Vector2 _vignettingCenter = new Vector2(0.5f, 0.5f);

        public Vector2 vignettingCenter {
            get { return _vignettingCenter; }
            set {
                if (_vignettingCenter != value) {
                    _vignettingCenter = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        Texture2D
            _vignettingMask;

        public Texture2D vignettingMask {
            get { return _vignettingMask; }
            set {
                if (_vignettingMask != value) {
                    _vignettingMask = value;
                    UpdateMaterialProperties();
                }
            }
        }

        #endregion

        #region Frame

        public enum FrameStyle {
            Border,
            CinematicBands
        }

        [SerializeField]
        bool
            _frame = false;

        public bool frame {
            get { return _frame; }
            set {
                if (_frame != value) {
                    _frame = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        FrameStyle _frameStyle = FrameStyle.Border;

        public FrameStyle frameStyle {
            get { return _frameStyle; }
            set {
                if (_frameStyle != value) {
                    _frameStyle = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        [Range(0, 0.5f)]
        float _frameBandHorizontalSize;

        public float frameBandHorizontalSize {
            get { return _frameBandHorizontalSize; }
            set {
                if (_frameBandHorizontalSize != value) {
                    _frameBandHorizontalSize = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        [Range(0, 1f)]
        float _frameBandHorizontalSmoothness;

        public float frameBandHorizontalSmoothness {
            get { return _frameBandHorizontalSmoothness; }
            set {
                if (_frameBandHorizontalSmoothness != value) {
                    _frameBandHorizontalSmoothness = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        [Range(0, 0.5f)]
        float _frameBandVerticalSize = 0.1f;

        public float frameBandVerticalSize {
            get { return _frameBandVerticalSize; }
            set {
                if (_frameBandVerticalSize != value) {
                    _frameBandVerticalSize = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        [Range(0, 1f)]
        float _frameBandVerticalSmoothness;

        public float frameBandVerticalSmoothness {
            get { return _frameBandVerticalSmoothness; }
            set {
                if (_frameBandVerticalSmoothness != value) {
                    _frameBandVerticalSmoothness = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        Color
            _frameColor = new Color(1, 1, 1, 0.047f);

        public Color frameColor {
            get { return _frameColor; }
            set {
                if (_frameColor != value) {
                    _frameColor = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        Texture2D
            _frameMask;

        public Texture2D frameMask {
            get { return _frameMask; }
            set {
                if (_frameMask != value) {
                    _frameMask = value;
                    UpdateMaterialProperties();
                }
            }
        }


        #endregion

        #region LUT

        [SerializeField]
        bool
            _lut = false;

        public bool lut {
            get { return _lut; }
            set {
                if (_lut != value) {
                    _lut = value;
                    if (_lut) {
                        _nightVision = false;
                        _thermalVision = false;
                    }
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 1f)]
        float
            _lutIntensity = 1f;

        public float lutIntensity {
            get { return _lutIntensity; }
            set {
                if (_lutIntensity != value) {
                    _lutIntensity = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        Texture2D _lutTexture;

        public Texture2D lutTexture {
            get { return _lutTexture; }
            set {
                if (_lutTexture != value) {
                    _lutTexture = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        Texture3D _lutTexture3D;

        public Texture3D lutTexture3D {
            get { return _lutTexture3D; }
            set {
                if (_lutTexture3D != value) {
                    _lutTexture3D = value;
                    UpdateMaterialProperties();
                }
            }
        }

        #endregion

        #region Night Vision

        [SerializeField]
        bool
            _nightVision = false;

        public bool nightVision {
            get { return _nightVision; }
            set {
                if (_nightVision != value) {
                    _nightVision = value;
                    if (_nightVision) {
                        _thermalVision = false;
                        _lut = false;
                        _vignetting = true;
                        _vignettingFade = 0;
                        _vignettingColor = new Color(0, 0, 0, 32f / 255f);
                        _vignettingCircularShape = true;
                    }
                    else {
                        _vignetting = false;
                    }
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        Color
            _nightVisionColor = new Color(0.5f, 1f, 0.5f, 0.5f);

        public Color nightVisionColor {
            get { return _nightVisionColor; }
            set {
                if (_nightVisionColor != value) {
                    _nightVisionColor = value;
                    UpdateMaterialProperties();
                }
            }
        }

        #endregion

        #region Outline

        [SerializeField]
        bool
            _outline;

        public bool outline {
            get { return _outline; }
            set {
                if (_outline != value) {
                    _outline = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField, ColorUsage(false, hdr: true)]
        Color
            _outlineColor = new Color(0, 0, 0, 0.8f);

        public Color outlineColor {
            get { return _outlineColor; }
            set {
                if (_outlineColor != value) {
                    _outlineColor = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        bool
            _outlineCustomize;

        public bool outlineCustomize {
            get { return _outlineCustomize; }
            set {
                if (_outlineCustomize != value) {
                    _outlineCustomize = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        BEAUTIFY_OUTLINE_STAGE _outlineStage = BEAUTIFY_OUTLINE_STAGE.BeforeBloom;

        public BEAUTIFY_OUTLINE_STAGE outlineStage {
            get { return _outlineStage; }
            set {
                if (_outlineStage != value) {
                    _outlineStage = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        [Range(0, 1.3f)]
        float
            _outlineSpread = 1f;

        public float outlineSpread {
            get { return _outlineSpread; }
            set {
                if (_outlineSpread != value) {
                    _outlineSpread = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        [Range(1, 5)]
        int
            _outlineBlurPassCount = 1;

        public int outlineBlurPassCount {
            get { return _outlineBlurPassCount; }
            set {
                if (_outlineBlurPassCount != value) {
                    _outlineBlurPassCount = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        [Range(0, 8)]
        float
            _outlineIntensityMultiplier = 1f;

        public float outlineIntensityMultiplier {
            get { return _outlineIntensityMultiplier; }
            set {
                if (_outlineIntensityMultiplier != value) {
                    _outlineIntensityMultiplier = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        bool
            _outlineBlurDownscale = true;

        public bool outlineBlurDownscale {
            get { return _outlineBlurDownscale; }
            set {
                if (_outlineBlurDownscale != value) {
                    _outlineBlurDownscale = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0, 1)]
        float
            _outlineMinDepthThreshold;

        public float outlineMinDepthThreshold {
            get { return _outlineMinDepthThreshold; }
            set {
                if (_outlineMinDepthThreshold != value) {
                    _outlineMinDepthThreshold = value;
                    UpdateMaterialProperties();
                }
            }
        }


        #endregion

        #region Thermal Vision

        [SerializeField]
        bool
            _thermalVision = false;

        public bool thermalVision {
            get { return _thermalVision; }
            set {
                if (_thermalVision != value) {
                    _thermalVision = value;
                    if (_thermalVision) {
                        _nightVision = false;
                        _lut = false;
                        _vignetting = true;
                        _vignettingFade = 0;
                        _vignettingColor = new Color(1f, 16f / 255f, 16f / 255f, 18f / 255f);
                        _vignettingCircularShape = true;
                    }
                    else {
                        _vignetting = false;
                    }
                    UpdateMaterialProperties();
                }
            }
        }

        #endregion

        #region Lens Dirt

        [SerializeField]
        bool
            _lensDirt = false;

        public bool lensDirt {
            get { return _lensDirt; }
            set {
                if (_lensDirt != value) {
                    _lensDirt = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 1f)]
        float
            _lensDirtThreshold = 0.5f;

        public float lensDirtThreshold {
            get { return _lensDirtThreshold; }
            set {
                if (_lensDirtThreshold != value) {
                    _lensDirtThreshold = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 1f)]
        float
            _lensDirtIntensity = 0.9f;

        public float lensDirtIntensity {
            get { return _lensDirtIntensity; }
            set {
                if (_lensDirtIntensity != value) {
                    _lensDirtIntensity = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        Texture2D
            _lensDirtTexture;

        public Texture2D lensDirtTexture {
            get { return _lensDirtTexture; }
            set {
                if (_lensDirtTexture != value) {
                    _lensDirtTexture = value;
                    UpdateMaterialProperties();
                }
            }
        }

        #endregion

        #region Bloom

        [SerializeField]
        bool
            _bloom;

        public bool bloom {
            get { return _bloom; }
            set {
                if (_bloom != value) {
                    _bloom = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        LayerMask
            _bloomCullingMask;

        public LayerMask bloomCullingMask {
            get { return _bloomCullingMask; }
            set {
                if (_bloomCullingMask != value) {
                    _bloomCullingMask = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(1f, 4f)]
        float
            _bloomLayerMaskDownsampling = 1f;

        public float bloomLayerMaskDownsampling {
            get { return _bloomLayerMaskDownsampling; }
            set {
                if (_bloomLayerMaskDownsampling != value) {
                    _bloomLayerMaskDownsampling = Mathf.Max(value, 1f);
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        float
            _bloomIntensity = 1f;

        public float bloomIntensity {
            get { return _bloomIntensity; }
            set {
                if (_bloomIntensity != value) {
                    _bloomIntensity = Mathf.Abs(value);
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        float
            _bloomMaxBrightness = 1000f;

        public float bloomMaxBrightness {
            get { return _bloomMaxBrightness; }
            set {
                if (_bloomMaxBrightness != value) {
                    _bloomMaxBrightness = Mathf.Abs(value);
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        [Range(0f, 3f)]
        float
            _bloomBoost0;

        public float bloomBoost0 {
            get { return _bloomBoost0; }
            set {
                if (_bloomBoost0 != value) {
                    _bloomBoost0 = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 3f)]
        float
            _bloomBoost1;

        public float bloomBoost1 {
            get { return _bloomBoost1; }
            set {
                if (_bloomBoost1 != value) {
                    _bloomBoost1 = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 3f)]
        float
            _bloomBoost2;

        public float bloomBoost2 {
            get { return _bloomBoost2; }
            set {
                if (_bloomBoost2 != value) {
                    _bloomBoost2 = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 3f)]
        float
            _bloomBoost3;

        public float bloomBoost3 {
            get { return _bloomBoost3; }
            set {
                if (_bloomBoost3 != value) {
                    _bloomBoost3 = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 3f)]
        float
            _bloomBoost4;

        public float bloomBoost4 {
            get { return _bloomBoost4; }
            set {
                if (_bloomBoost4 != value) {
                    _bloomBoost4 = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        [Range(0f, 3f)]
        float
            _bloomBoost5;

        public float bloomBoost5 {
            get { return _bloomBoost5; }
            set {
                if (_bloomBoost5 != value) {
                    _bloomBoost5 = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        bool
            _bloomAntiflicker;

        public bool bloomAntiflicker {
            get { return _bloomAntiflicker; }
            set {
                if (_bloomAntiflicker != value) {
                    _bloomAntiflicker = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        float
            _bloomAntiflickerMaxOutput = 10f;

        public float bloomAntiflickerMaxOutput {
            get { return _bloomAntiflickerMaxOutput; }
            set {
                if (_bloomAntiflickerMaxOutput != value) {
                    _bloomAntiflickerMaxOutput = Mathf.Abs(value);
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField, Range(3, 5)]
        int
            _bloomIterations = 4;

        public int bloomIterations {
            get { return _bloomIterations; }
            set {
                if (_bloomIterations != value) {
                    _bloomIterations = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        bool
            _bloomUltra;

        public bool bloomUltra {
            get { return _bloomUltra; }
            set {
                if (_bloomUltra != value) {
                    _bloomUltra = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField, Range(1, 10)]
        int
            _bloomUltraResolution = 10;

        public int bloomUltraResolution {
            get { return _bloomUltraResolution; }
            set {
                if (_bloomUltraResolution != value) {
                    _bloomUltraResolution = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        [Range(0f, 5f)]
        float
            _bloomThreshold = 0.75f;

        public float bloomThreshold {
            get { return _bloomThreshold; }
            set {
                if (_bloomThreshold != value) {
                    _bloomThreshold = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        bool
            _bloomConservativeThreshold;

        public bool bloomConservativeThreshold {
            get { return _bloomConservativeThreshold; }
            set {
                if (_bloomConservativeThreshold != value) {
                    _bloomConservativeThreshold = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        Color
            _bloomTint = new Color(1f, 1f, 1f, 0f);

        public Color bloomTint {
            get { return _bloomTint; }
            set {
                if (_bloomTint != value) {
                    _bloomTint = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        bool
            _bloomCustomize;

        public bool bloomCustomize {
            get { return _bloomCustomize; }
            set {
                if (_bloomCustomize != value) {
                    _bloomCustomize = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        bool
            _bloomDebug;

        public bool bloomDebug {
            get { return _bloomDebug; }
            set {
                if (_bloomDebug != value) {
                    _bloomDebug = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 1f)]
        float
            _bloomWeight0 = 0.5f;

        public float bloomWeight0 {
            get { return _bloomWeight0; }
            set {
                if (_bloomWeight0 != value) {
                    _bloomWeight0 = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 1f)]
        float
            _bloomWeight1 = 0.5f;

        public float bloomWeight1 {
            get { return _bloomWeight1; }
            set {
                if (_bloomWeight1 != value) {
                    _bloomWeight1 = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 1f)]
        float
            _bloomWeight2 = 0.5f;

        public float bloomWeight2 {
            get { return _bloomWeight2; }
            set {
                if (_bloomWeight2 != value) {
                    _bloomWeight2 = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 1f)]
        float
            _bloomWeight3 = 0.5f;

        public float bloomWeight3 {
            get { return _bloomWeight3; }
            set {
                if (_bloomWeight3 != value) {
                    _bloomWeight3 = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 1f)]
        float
            _bloomWeight4 = 0.5f;

        public float bloomWeight4 {
            get { return _bloomWeight4; }
            set {
                if (_bloomWeight4 != value) {
                    _bloomWeight4 = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 1f)]
        float
            _bloomWeight5 = 0.5f;

        public float bloomWeight5 {
            get { return _bloomWeight5; }
            set {
                if (_bloomWeight5 != value) {
                    _bloomWeight5 = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        Color _bloomTint0 = Color.white;

        public Color bloomTint0 {
            get { return _bloomTint0; }
            set {
                if (_bloomTint0 != value) {
                    _bloomTint0 = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        Color _bloomTint1 = Color.white;

        public Color bloomTint1 {
            get { return _bloomTint1; }
            set {
                if (_bloomTint1 != value) {
                    _bloomTint1 = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        Color _bloomTint2 = Color.white;

        public Color bloomTint2 {
            get { return _bloomTint2; }
            set {
                if (_bloomTint2 != value) {
                    _bloomTint2 = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        Color _bloomTint3 = Color.white;

        public Color bloomTint3 {
            get { return _bloomTint3; }
            set {
                if (_bloomTint3 != value) {
                    _bloomTint3 = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        Color _bloomTint4 = Color.white;

        public Color bloomTint4 {
            get { return _bloomTint4; }
            set {
                if (_bloomTint4 != value) {
                    _bloomTint4 = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        Color _bloomTint5 = Color.white;

        public Color bloomTint5 {
            get { return _bloomTint5; }
            set {
                if (_bloomTint5 != value) {
                    _bloomTint5 = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        bool
            _bloomBlur = true;

        public bool bloomBlur {
            get { return _bloomBlur; }
            set {
                if (_bloomBlur != value) {
                    _bloomBlur = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        bool
        _bloomQuickerBlur;

        public bool bloomQuickerBlur {
            get { return _bloomQuickerBlur; }
            set {
                if (_bloomQuickerBlur != value) {
                    _bloomQuickerBlur = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        float
            _bloomDepthAtten;

        public float bloomDepthAtten {
            get { return _bloomDepthAtten; }
            set {
                if (_bloomDepthAtten != value) {
                    _bloomDepthAtten = Mathf.Max(0, value);
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        float
            _bloomNearAtten;

        public float bloomNearAtten {
            get { return _bloomNearAtten; }
            set {
                if (_bloomNearAtten != value) {
                    _bloomNearAtten = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        [Range(-1f, 1f)]
        float
                        _bloomLayerZBias = 0.0001f;

        public float bloomLayerZBias {
            get { return _bloomLayerZBias; }
            set {
                if (_bloomLayerZBias != value) {
                    _bloomLayerZBias = Mathf.Clamp(value, -1f, 1f);
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        BEAUTIFY_PRERENDER_EVENT
                        _preRenderCameraEvent = BEAUTIFY_PRERENDER_EVENT.OnPreCull;

        public BEAUTIFY_PRERENDER_EVENT preRenderCameraEvent {
            get { return _preRenderCameraEvent; }
            set {
                if (_preRenderCameraEvent != value) {
                    _preRenderCameraEvent = value;
                }
            }
        }



        #endregion

        #region Anamorphic Flares


        [SerializeField]
        bool
            _anamorphicFlares;

        public bool anamorphicFlares {
            get { return _anamorphicFlares; }
            set {
                if (_anamorphicFlares != value) {
                    _anamorphicFlares = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        LayerMask _anamorphicFlaresCullingMask;

        public LayerMask anamorphicFlaresCullingMask {
            get { return _anamorphicFlaresCullingMask; }
            set {
                if (_anamorphicFlaresCullingMask != value) {
                    _anamorphicFlaresCullingMask = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(1f, 4f)]
        float
            _anamorphicFlaresLayerMaskDownsampling = 1f;

        public float anamorphicFlaresLayerMaskDownsampling {
            get { return _anamorphicFlaresLayerMaskDownsampling; }
            set {
                if (_anamorphicFlaresLayerMaskDownsampling != value) {
                    _anamorphicFlaresLayerMaskDownsampling = Mathf.Max(value, 1f);
                    UpdateMaterialProperties();
                }
            }
        }



        [SerializeField]
        float
            _anamorphicFlaresIntensity = 1f;

        public float anamorphicFlaresIntensity {
            get { return _anamorphicFlaresIntensity; }
            set {
                if (_anamorphicFlaresIntensity != value) {
                    _anamorphicFlaresIntensity = Mathf.Abs(value);
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        bool
            _anamorphicFlaresAntiflicker;

        public bool anamorphicFlaresAntiflicker {
            get { return _anamorphicFlaresAntiflicker; }
            set {
                if (_anamorphicFlaresAntiflicker != value) {
                    _anamorphicFlaresAntiflicker = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        float
            _anamorphicFlaresAntiflickerMaxOutput = 10f;

        public float anamorphicFlaresAntiflickerMaxOutput {
            get { return _anamorphicFlaresAntiflickerMaxOutput; }
            set {
                if (_anamorphicFlaresAntiflickerMaxOutput != value) {
                    _anamorphicFlaresAntiflickerMaxOutput = Mathf.Abs(value);
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        bool
            _anamorphicFlaresUltra;

        public bool anamorphicFlaresUltra {
            get { return _anamorphicFlaresUltra; }
            set {
                if (_anamorphicFlaresUltra != value) {
                    _anamorphicFlaresUltra = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField, Range(1, 10)]
        int
            _anamorphicFlaresUltraResolution = 10;

        public int anamorphicFlaresUltraResolution {
            get { return _anamorphicFlaresUltraResolution; }
            set {
                if (_anamorphicFlaresUltraResolution != value) {
                    _anamorphicFlaresUltraResolution = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 5f)]
        float
            _anamorphicFlaresThreshold = 0.75f;

        public float anamorphicFlaresThreshold {
            get { return _anamorphicFlaresThreshold; }
            set {
                if (_anamorphicFlaresThreshold != value) {
                    _anamorphicFlaresThreshold = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0.1f, 2f)]
        float
            _anamorphicFlaresSpread = 1f;

        public float anamorphicFlaresSpread {
            get { return _anamorphicFlaresSpread; }
            set {
                if (_anamorphicFlaresSpread != value) {
                    _anamorphicFlaresSpread = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        bool
            _anamorphicFlaresVertical;

        public bool anamorphicFlaresVertical {
            get { return _anamorphicFlaresVertical; }
            set {
                if (_anamorphicFlaresVertical != value) {
                    _anamorphicFlaresVertical = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        Color
            _anamorphicFlaresTint = new Color(0.5f, 0.5f, 1f, 0f);

        public Color anamorphicFlaresTint {
            get { return _anamorphicFlaresTint; }
            set {
                if (_anamorphicFlaresTint != value) {
                    _anamorphicFlaresTint = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        bool
            _anamorphicFlaresBlur = true;

        public bool anamorphicFlaresBlur {
            get { return _anamorphicFlaresBlur; }
            set {
                if (_anamorphicFlaresBlur != value) {
                    _anamorphicFlaresBlur = value;
                    UpdateMaterialProperties();
                }
            }
        }

        #endregion

        #region Depth of Field


        [SerializeField]
        bool
            _depthOfField;

        public bool depthOfField {
            get { return _depthOfField; }
            set {
                if (_depthOfField != value) {
                    _depthOfField = value;
                    dofPrevDistance = -1;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        bool
            _depthOfFieldTransparencySupport;

        public bool depthOfFieldTransparencySupport {
            get { return _depthOfFieldTransparencySupport; }
            set {
                if (_depthOfFieldTransparencySupport != value) {
                    _depthOfFieldTransparencySupport = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        LayerMask
            _depthOfFieldTransparencyLayerMask = -1;

        public LayerMask depthOfFieldTransparencyLayerMask {
            get { return _depthOfFieldTransparencyLayerMask; }
            set {
                if (_depthOfFieldTransparencyLayerMask != value) {
                    _depthOfFieldTransparencyLayerMask = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        UnityEngine.Rendering.CullMode
            _depthOfFieldTransparencyCullMode = UnityEngine.Rendering.CullMode.Back;

        public UnityEngine.Rendering.CullMode depthOfFieldTransparencyCullMode {
            get { return _depthOfFieldTransparencyCullMode; }
            set {
                if (_depthOfFieldTransparencyCullMode != value) {
                    _depthOfFieldTransparencyCullMode = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        Transform
            _depthOfFieldTargetFocus;

        public Transform depthOfFieldTargetFocus {
            get { return _depthOfFieldTargetFocus; }
            set {
                if (_depthOfFieldTargetFocus != value) {
                    _depthOfFieldTargetFocus = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        bool
            _depthOfFieldDebug;

        public bool depthOfFieldDebug {
            get { return _depthOfFieldDebug; }
            set {
                if (_depthOfFieldDebug != value) {
                    _depthOfFieldDebug = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        bool
            _depthOfFieldAutofocus;

        public bool depthOfFieldAutofocus {
            get { return _depthOfFieldAutofocus; }
            set {
                if (_depthOfFieldAutofocus != value) {
                    _depthOfFieldAutofocus = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        Vector2 _depthofFieldAutofocusViewportPoint = new Vector2(0.5f, 0.5f);

        public Vector2 depthofFieldAutofocusViewportPoint {
            get {
                return _depthofFieldAutofocusViewportPoint;
            }
            set {
                if (_depthofFieldAutofocusViewportPoint != value) {
                    _depthofFieldAutofocusViewportPoint = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        float
            _depthOfFieldAutofocusMinDistance;

        public float depthOfFieldAutofocusMinDistance {
            get { return _depthOfFieldAutofocusMinDistance; }
            set {
                if (_depthOfFieldAutofocusMinDistance != value) {
                    _depthOfFieldAutofocusMinDistance = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        float
            _depthOfFieldAutofocusDistanceShift;

        public float depthOfFieldAutofocusDistanceShift {
            get { return _depthOfFieldAutofocusDistanceShift; }
            set {
                if (_depthOfFieldAutofocusDistanceShift != value) {
                    _depthOfFieldAutofocusDistanceShift = value;
                    UpdateMaterialProperties();
                }
            }
        }



        [SerializeField]
        float
            _depthOfFieldAutofocusMaxDistance = 10000;

        public float depthOfFieldAutofocusMaxDistance {
            get { return _depthOfFieldAutofocusMaxDistance; }
            set {
                if (_depthOfFieldAutofocusMaxDistance != value) {
                    _depthOfFieldAutofocusMaxDistance = value;
                    UpdateMaterialProperties();
                }
            }
        }



        [SerializeField]
        LayerMask
            _depthOfFieldAutofocusLayerMask = -1;

        public LayerMask depthOfFieldAutofocusLayerMask {
            get { return _depthOfFieldAutofocusLayerMask; }
            set {
                if (_depthOfFieldAutofocusLayerMask != value) {
                    _depthOfFieldAutofocusLayerMask = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        LayerMask
            _depthOfFieldExclusionLayerMask;

        public LayerMask depthOfFieldExclusionLayerMask {
            get { return _depthOfFieldExclusionLayerMask; }
            set {
                if (_depthOfFieldExclusionLayerMask != value) {
                    _depthOfFieldExclusionLayerMask = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        UnityEngine.Rendering.CullMode
            _depthOfFieldExclusionCullMode = UnityEngine.Rendering.CullMode.Back;

        public UnityEngine.Rendering.CullMode depthOfFieldExclusionCullMode {
            get { return _depthOfFieldExclusionCullMode; }
            set {
                if (_depthOfFieldExclusionCullMode != value) {
                    _depthOfFieldExclusionCullMode = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        [Range(1, 4)]
        float
            _depthOfFieldExclusionLayerMaskDownsampling = 1f;

        public float depthOfFieldExclusionLayerMaskDownsampling {
            get { return _depthOfFieldExclusionLayerMaskDownsampling; }
            set {
                if (_depthOfFieldExclusionLayerMaskDownsampling != value) {
                    _depthOfFieldExclusionLayerMaskDownsampling = Mathf.Max(value, 1f);
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(1, 4)]
        float
            _depthOfFieldTransparencySupportDownsampling = 1f;

        public float depthOfFieldTransparencySupportDownsampling {
            get { return _depthOfFieldTransparencySupportDownsampling; }
            set {
                if (_depthOfFieldTransparencySupportDownsampling != value) {
                    _depthOfFieldTransparencySupportDownsampling = Mathf.Max(value, 1f);
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0.9f, 1f)]
        float
            _depthOfFieldExclusionBias = 0.99f;

        public float depthOfFieldExclusionBias {
            get { return _depthOfFieldExclusionBias; }
            set {
                if (_depthOfFieldExclusionBias != value) {
                    _depthOfFieldExclusionBias = Mathf.Clamp01(value);
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(1f, 100f)]
        float
            _depthOfFieldDistance = 1f;

        public float depthOfFieldDistance {
            get { return _depthOfFieldDistance; }
            set {
                if (_depthOfFieldDistance != value) {
                    _depthOfFieldDistance = Mathf.Max(value, 1f);
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0.001f, 5f)]
        float
            _depthOfFieldFocusSpeed = 1f;

        public float depthOfFieldFocusSpeed {
            get { return _depthOfFieldFocusSpeed; }
            set {
                if (_depthOfFieldFocusSpeed != value) {
                    _depthOfFieldFocusSpeed = Mathf.Clamp(value, 0.001f, 1f);
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(1, 5)]
        int
            _depthOfFieldDownsampling = 2;

        public int depthOfFieldDownsampling {
            get { return _depthOfFieldDownsampling; }
            set {
                if (_depthOfFieldDownsampling != value) {
                    _depthOfFieldDownsampling = Mathf.Max(value, 1);
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(2, 16)]
        int
            _depthOfFieldMaxSamples = 4;

        public int depthOfFieldMaxSamples {
            get { return _depthOfFieldMaxSamples; }
            set {
                if (_depthOfFieldMaxSamples != value) {
                    _depthOfFieldMaxSamples = Mathf.Max(value, 2);
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        BEAUTIFY_DOF_CAMERA_SETTINGS _depthOfFieldCameraSettings = BEAUTIFY_DOF_CAMERA_SETTINGS.Classic;

        public BEAUTIFY_DOF_CAMERA_SETTINGS depthOfFieldCameraSettings {
            get { return _depthOfFieldCameraSettings; }
            set {
                if (_depthOfFieldCameraSettings != value) {
                    _depthOfFieldCameraSettings = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        [Range(1f, 300f)]
        float
            _depthOfFieldFocalLengthReal = 50f;

        public float depthOfFieldFocalLengthReal {
            get { return _depthOfFieldFocalLengthReal; }
            set {
                if (_depthOfFieldFocalLengthReal != value) {
                    _depthOfFieldFocalLengthReal = Mathf.Abs(value);
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(1, 32)]
        float
            _depthOfFieldFStop = 2f;

        public float depthOfFieldFStop {
            get { return _depthOfFieldFStop; }
            set {
                if (_depthOfFieldFStop != value) {
                    _depthOfFieldFStop = Mathf.Abs(value);
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        [Range(1, 48)]
        float
            _depthOfFieldImageSensorHeight = 24f;

        public float depthOfFieldImageSensorHeight {
            get { return _depthOfFieldImageSensorHeight; }
            set {
                if (_depthOfFieldImageSensorHeight != value) {
                    _depthOfFieldImageSensorHeight = Mathf.Abs(value);
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0.005f, 0.5f)]
        float
            _depthOfFieldFocalLength = 0.050f;

        public float depthOfFieldFocalLength {
            get { return _depthOfFieldFocalLength; }
            set {
                if (_depthOfFieldFocalLength != value) {
                    _depthOfFieldFocalLength = Mathf.Abs(value);
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        float
            _depthOfFieldAperture = 2.8f;

        public float depthOfFieldAperture {
            get { return _depthOfFieldAperture; }
            set {
                if (_depthOfFieldAperture != value) {
                    _depthOfFieldAperture = Mathf.Abs(value);
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        bool
            _depthOfFieldForegroundBlur = true;

        public bool depthOfFieldForegroundBlur {
            get { return _depthOfFieldForegroundBlur; }
            set {
                if (_depthOfFieldForegroundBlur != value) {
                    _depthOfFieldForegroundBlur = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        bool
            _depthOfFieldForegroundBlurHQ;

        public bool depthOfFieldForegroundBlurHQ {
            get { return _depthOfFieldForegroundBlurHQ; }
            set {
                if (_depthOfFieldForegroundBlurHQ != value) {
                    _depthOfFieldForegroundBlurHQ = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField, Range(0, 32)]
        float
            _depthOfFieldForegroundBlurHQSpread = 16f;

        public float depthOfFieldForegroundBlurHQSpread {
            get { return _depthOfFieldForegroundBlurHQSpread; }
            set {
                if (_depthOfFieldForegroundBlurHQSpread != value) {
                    _depthOfFieldForegroundBlurHQSpread = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        float
            _depthOfFieldForegroundDistance = 0.25f;

        public float depthOfFieldForegroundDistance {
            get { return _depthOfFieldForegroundDistance; }
            set {
                if (_depthOfFieldForegroundDistance != value) {
                    _depthOfFieldForegroundDistance = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        bool
            _depthOfFieldBokeh = true;

        public bool depthOfFieldBokeh {
            get { return _depthOfFieldBokeh; }
            set {
                if (_depthOfFieldBokeh != value) {
                    _depthOfFieldBokeh = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        BEAUTIFY_BOKEH_COMPOSITION
            _depthOfFieldBokehComposition = BEAUTIFY_BOKEH_COMPOSITION.Integrated;

        public BEAUTIFY_BOKEH_COMPOSITION depthOfFieldBokehComposition {
            get { return _depthOfFieldBokehComposition; }
            set {
                if (_depthOfFieldBokehComposition != value) {
                    _depthOfFieldBokehComposition = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0.5f, 3f)]
        float
            _depthOfFieldBokehThreshold = 1f;

        public float depthOfFieldBokehThreshold {
            get { return _depthOfFieldBokehThreshold; }
            set {
                if (_depthOfFieldBokehThreshold != value) {
                    _depthOfFieldBokehThreshold = Mathf.Max(value, 0f);
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 8f)]
        float
            _depthOfFieldBokehIntensity = 2f;

        public float depthOfFieldBokehIntensity {
            get { return _depthOfFieldBokehIntensity; }
            set {
                if (_depthOfFieldBokehIntensity != value) {
                    _depthOfFieldBokehIntensity = Mathf.Max(value, 0);
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        float
            _depthOfFieldMaxBrightness = 1000f;

        public float depthOfFieldMaxBrightness {
            get { return _depthOfFieldMaxBrightness; }
            set {
                if (_depthOfFieldMaxBrightness != value) {
                    _depthOfFieldMaxBrightness = Mathf.Abs(value);
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField, Range(0, 1f)]
        float
            _depthOfFieldMaxDistance = 1f;

        public float depthOfFieldMaxDistance {
            get { return _depthOfFieldMaxDistance; }
            set {
                if (_depthOfFieldMaxDistance != value) {
                    _depthOfFieldMaxDistance = Mathf.Abs(value);
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        FilterMode _depthOfFieldFilterMode = FilterMode.Bilinear;

        public FilterMode depthOfFieldFilterMode {
            get { return _depthOfFieldFilterMode; }
            set {
                if (_depthOfFieldFilterMode != value) {
                    _depthOfFieldFilterMode = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [NonSerialized]
        public OnBeforeFocusEvent OnBeforeFocus;

        #endregion

        #region Eye Adaptation


        [SerializeField]
        bool
            _eyeAdaptation;

        public bool eyeAdaptation {
            get { return _eyeAdaptation; }
            set {
                if (_eyeAdaptation != value) {
                    _eyeAdaptation = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 1f)]
        float
            _eyeAdaptationMinExposure = 0.2f;

        public float eyeAdaptationMinExposure {
            get { return _eyeAdaptationMinExposure; }
            set {
                if (_eyeAdaptationMinExposure != value) {
                    _eyeAdaptationMinExposure = Mathf.Clamp01(value);
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(1f, 100f)]
        float
            _eyeAdaptationMaxExposure = 5f;

        public float eyeAdaptationMaxExposure {
            get { return _eyeAdaptationMaxExposure; }
            set {
                if (_eyeAdaptationMaxExposure != value) {
                    _eyeAdaptationMaxExposure = Mathf.Clamp(value, 1f, 100f);
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 1f)]
        float
            _eyeAdaptationSpeedToLight = 0.4f;

        public float eyeAdaptationSpeedToLight {
            get { return _eyeAdaptationSpeedToLight; }
            set {
                if (_eyeAdaptationSpeedToLight != value) {
                    _eyeAdaptationSpeedToLight = Mathf.Clamp01(value);
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 1f)]
        float
            _eyeAdaptationSpeedToDark = 0.2f;

        public float eyeAdaptationSpeedToDark {
            get { return _eyeAdaptationSpeedToDark; }
            set {
                if (_eyeAdaptationSpeedToDark != value) {
                    _eyeAdaptationSpeedToDark = Mathf.Clamp01(value);
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        bool
            _eyeAdaptationInEditor = true;

        public bool eyeAdaptationInEditor {
            get { return _eyeAdaptationInEditor; }
            set {
                if (_eyeAdaptationInEditor != value) {
                    _eyeAdaptationInEditor = value;
                    UpdateMaterialProperties();
                }
            }
        }

        #endregion

        #region Purkinje effect

        [SerializeField]
        bool
            _purkinje;

        public bool purkinje {
            get { return _purkinje; }
            set {
                if (_purkinje != value) {
                    _purkinje = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 5f)]
        float
            _purkinjeAmount = 1f;

        public float purkinjeAmount {
            get { return _purkinjeAmount; }
            set {
                if (_purkinjeAmount != value) {
                    _purkinjeAmount = Mathf.Clamp(value, 0f, 5f);
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 1f)]
        float
            _purkinjeLuminanceThreshold = 0.15f;

        public float purkinjeLuminanceThreshold {
            get { return _purkinjeLuminanceThreshold; }
            set {
                if (purkinjeLuminanceThreshold != value) {
                    _purkinjeLuminanceThreshold = Mathf.Clamp(value, 0f, 1f);
                    UpdateMaterialProperties();
                }
            }
        }

        #endregion

        #region Tonemapping


        [SerializeField]
        BEAUTIFY_TMO
            _tonemap = BEAUTIFY_TMO.Linear;

        public BEAUTIFY_TMO tonemap {
            get { return _tonemap; }
            set {
                if (_tonemap != value) {
                    _tonemap = value;
                    if (_tonemap == BEAUTIFY_TMO.ACES) {
                        _saturate = 0;
                        _contrast = 1f;
                    }
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0, 5f)]
        float _tonemapGamma = 2.5f;

        public float tonemapGamma {
            get { return _tonemapGamma; }
            set {
                if (_tonemapGamma != value) {
                    _tonemapGamma = Mathf.Clamp(value, 0, 5f);
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        float _tonemapExposurePre = 1f;

        public float tonemapExposurePre {
            get { return _tonemapExposurePre; }
            set {
                if (_tonemapExposurePre != value) {
                    _tonemapExposurePre = Mathf.Max(0, value);
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        float _tonemapBrightnessPost = 1f;

        public float tonemapBrightnessPost {
            get { return _tonemapBrightnessPost; }
            set {
                if (_tonemapBrightnessPost != value) {
                    _tonemapBrightnessPost = Mathf.Max(0, value);
                    UpdateMaterialProperties();
                }
            }
        }

        #endregion

        #region Sun Flares

        [SerializeField]
        bool
            _sunFlares;

        public bool sunFlares {
            get { return _sunFlares; }
            set {
                if (_sunFlares != value) {
                    _sunFlares = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        Transform
            _sun;

        public Transform sun {
            get { return _sun; }
            set {
                if (_sun != value) {
                    _sun = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        LayerMask
            _sunFlaresLayerMask = -1;

        public LayerMask sunFlaresLayerMask {
            get { return _sunFlaresLayerMask; }
            set {
                if (_sunFlaresLayerMask != value) {
                    _sunFlaresLayerMask = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        [Range(0f, 1f)]
        float
            _sunFlaresIntensity = 1.0f;

        public float sunFlaresIntensity {
            get { return _sunFlaresIntensity; }
            set {
                if (_sunFlaresIntensity != value) {
                    _sunFlaresIntensity = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        float
            _sunFlaresRevealSpeed = 1.0f;

        public float sunFlaresRevealSpeed {
            get { return _sunFlaresRevealSpeed; }
            set {
                if (_sunFlaresRevealSpeed != value) {
                    _sunFlaresRevealSpeed = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        float
            _sunFlaresHideSpeed = 1.0f;

        public float sunFlaresHideSpeed {
            get { return _sunFlaresHideSpeed; }
            set {
                if (_sunFlaresHideSpeed != value) {
                    _sunFlaresHideSpeed = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        [Range(0f, 1f)]
        float
            _sunFlaresSolarWindSpeed = 0.01f;

        public float sunFlaresSolarWindSpeed {
            get { return _sunFlaresSolarWindSpeed; }
            set {
                if (_sunFlaresSolarWindSpeed != value) {
                    _sunFlaresSolarWindSpeed = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        Color
            _sunFlaresTint = new Color(1, 1, 1);

        public Color sunFlaresTint {
            get { return _sunFlaresTint; }
            set {
                if (_sunFlaresTint != value) {
                    _sunFlaresTint = value;
                    UpdateMaterialProperties();
                }
            }
        }




        [SerializeField]
        [Range(1, 5)]
        int
            _sunFlaresDownsampling = 1;

        public int sunFlaresDownsampling {
            get { return _sunFlaresDownsampling; }
            set {
                if (_sunFlaresDownsampling != value) {
                    _sunFlaresDownsampling = Mathf.Max(value, 1);
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 1f)]
        float
            _sunFlaresSunIntensity = 0.1f;

        public float sunFlaresSunIntensity {
            get { return _sunFlaresSunIntensity; }
            set {
                if (_sunFlaresSunIntensity != value) {
                    _sunFlaresSunIntensity = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 1f)]
        float
            _sunFlaresSunDiskSize = 0.05f;

        public float sunFlaresSunDiskSize {
            get { return _sunFlaresSunDiskSize; }
            set {
                if (_sunFlaresSunDiskSize != value) {
                    _sunFlaresSunDiskSize = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 10f)]
        float
            _sunFlaresSunRayDiffractionIntensity = 3.5f;

        public float sunFlaresSunRayDiffractionIntensity {
            get { return _sunFlaresSunRayDiffractionIntensity; }
            set {
                if (_sunFlaresSunRayDiffractionIntensity != value) {
                    _sunFlaresSunRayDiffractionIntensity = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 1f)]
        float
            _sunFlaresSunRayDiffractionThreshold = 0.13f;

        public float sunFlaresSunRayDiffractionThreshold {
            get { return _sunFlaresSunRayDiffractionThreshold; }
            set {
                if (_sunFlaresSunRayDiffractionThreshold != value) {
                    _sunFlaresSunRayDiffractionThreshold = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 0.2f)]
        float
            _sunFlaresCoronaRays1Length = 0.02f;

        public float sunFlaresCoronaRays1Length {
            get { return _sunFlaresCoronaRays1Length; }
            set {
                if (_sunFlaresCoronaRays1Length != value) {
                    _sunFlaresCoronaRays1Length = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(2, 30)]
        int
            _sunFlaresCoronaRays1Streaks = 12;

        public int sunFlaresCoronaRays1Streaks {
            get { return _sunFlaresCoronaRays1Streaks; }
            set {
                if (_sunFlaresCoronaRays1Streaks != value) {
                    _sunFlaresCoronaRays1Streaks = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 0.1f)]
        float
            _sunFlaresCoronaRays1Spread = 0.001f;

        public float sunFlaresCoronaRays1Spread {
            get { return _sunFlaresCoronaRays1Spread; }
            set {
                if (_sunFlaresCoronaRays1Spread != value) {
                    _sunFlaresCoronaRays1Spread = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 2f * Mathf.PI)]
        float
            _sunFlaresCoronaRays1AngleOffset = 0f;

        public float sunFlaresCoronaRays1AngleOffset {
            get { return _sunFlaresCoronaRays1AngleOffset; }
            set {
                if (_sunFlaresCoronaRays1AngleOffset != value) {
                    _sunFlaresCoronaRays1AngleOffset = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 0.2f)]
        float
            _sunFlaresCoronaRays2Length = 0.05f;

        public float sunFlaresCoronaRays2Length {
            get { return _sunFlaresCoronaRays2Length; }
            set {
                if (_sunFlaresCoronaRays2Length != value) {
                    _sunFlaresCoronaRays2Length = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(2, 30)]
        int
            _sunFlaresCoronaRays2Streaks = 12;

        public int sunFlaresCoronaRays2Streaks {
            get { return _sunFlaresCoronaRays2Streaks; }
            set {
                if (_sunFlaresCoronaRays2Streaks != value) {
                    _sunFlaresCoronaRays2Streaks = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 0.1f)]
        float
            _sunFlaresCoronaRays2Spread = 0.1f;

        public float sunFlaresCoronaRays2Spread {
            get { return _sunFlaresCoronaRays2Spread; }
            set {
                if (_sunFlaresCoronaRays2Spread != value) {
                    _sunFlaresCoronaRays2Spread = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 2f * Mathf.PI)]
        float
            _sunFlaresCoronaRays2AngleOffset = 0f;

        public float sunFlaresCoronaRays2AngleOffset {
            get { return _sunFlaresCoronaRays2AngleOffset; }
            set {
                if (_sunFlaresCoronaRays2AngleOffset != value) {
                    _sunFlaresCoronaRays2AngleOffset = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 1f)]
        float
            _sunFlaresGhosts1Size = 0.03f;

        public float sunFlaresGhosts1Size {
            get { return _sunFlaresGhosts1Size; }
            set {
                if (_sunFlaresGhosts1Size != value) {
                    _sunFlaresGhosts1Size = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(-3f, 3f)]
        float
            _sunFlaresGhosts1Offset = 1.04f;

        public float sunFlaresGhosts1Offset {
            get { return _sunFlaresGhosts1Offset; }
            set {
                if (_sunFlaresGhosts1Offset != value) {
                    _sunFlaresGhosts1Offset = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 1f)]
        float
            _sunFlaresGhosts1Brightness = 0.037f;

        public float sunFlaresGhosts1Brightness {
            get { return _sunFlaresGhosts1Brightness; }
            set {
                if (_sunFlaresGhosts1Brightness != value) {
                    _sunFlaresGhosts1Brightness = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 1f)]
        float
            _sunFlaresGhosts2Size = 0.1f;

        public float sunFlaresGhosts2Size {
            get { return _sunFlaresGhosts2Size; }
            set {
                if (_sunFlaresGhosts2Size != value) {
                    _sunFlaresGhosts2Size = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(-3f, 3f)]
        float
            _sunFlaresGhosts2Offset = 0.71f;

        public float sunFlaresGhosts2Offset {
            get { return _sunFlaresGhosts2Offset; }
            set {
                if (_sunFlaresGhosts2Offset != value) {
                    _sunFlaresGhosts2Offset = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 1f)]
        float
            _sunFlaresGhosts2Brightness = 0.03f;

        public float sunFlaresGhosts2Brightness {
            get { return _sunFlaresGhosts2Brightness; }
            set {
                if (_sunFlaresGhosts2Brightness != value) {
                    _sunFlaresGhosts2Brightness = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 1f)]
        float
            _sunFlaresGhosts3Size = 0.24f;

        public float sunFlaresGhosts3Size {
            get { return _sunFlaresGhosts3Size; }
            set {
                if (_sunFlaresGhosts3Size != value) {
                    _sunFlaresGhosts3Size = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 1f)]
        float
            _sunFlaresGhosts3Brightness = 0.025f;

        public float sunFlaresGhosts3Brightness {
            get { return _sunFlaresGhosts3Brightness; }
            set {
                if (_sunFlaresGhosts3Brightness != value) {
                    _sunFlaresGhosts3Brightness = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 1f)]
        float
            _sunFlaresGhosts3Offset = 0.31f;

        public float sunFlaresGhosts3Offset {
            get { return _sunFlaresGhosts3Offset; }
            set {
                if (_sunFlaresGhosts3Offset != value) {
                    _sunFlaresGhosts3Offset = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 1f)]
        float
            _sunFlaresGhosts4Size = 0.016f;

        public float sunFlaresGhosts4Size {
            get { return _sunFlaresGhosts4Size; }
            set {
                if (_sunFlaresGhosts4Size != value) {
                    _sunFlaresGhosts4Size = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(-3f, 3f)]
        float
            _sunFlaresGhosts4Offset = 0f;

        public float sunFlaresGhosts4Offset {
            get { return _sunFlaresGhosts4Offset; }
            set {
                if (_sunFlaresGhosts4Offset != value) {
                    _sunFlaresGhosts4Offset = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 1f)]
        float
            _sunFlaresGhosts4Brightness = 0.017f;

        public float sunFlaresGhosts4Brightness {
            get { return _sunFlaresGhosts4Brightness; }
            set {
                if (_sunFlaresGhosts4Brightness != value) {
                    _sunFlaresGhosts4Brightness = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 1f)]
        float
            _sunFlaresHaloOffset = 0.22f;

        public float sunFlaresHaloOffset {
            get { return _sunFlaresHaloOffset; }
            set {
                if (_sunFlaresHaloOffset != value) {
                    _sunFlaresHaloOffset = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 50f)]
        float
            _sunFlaresHaloAmplitude = 15.1415f;

        public float sunFlaresHaloAmplitude {
            get { return _sunFlaresHaloAmplitude; }
            set {
                if (_sunFlaresHaloAmplitude != value) {
                    _sunFlaresHaloAmplitude = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0f, 1f)]
        float
            _sunFlaresHaloIntensity = 0.01f;

        public float sunFlaresHaloIntensity {
            get { return _sunFlaresHaloIntensity; }
            set {
                if (_sunFlaresHaloIntensity != value) {
                    _sunFlaresHaloIntensity = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        float
            _sunFlaresRadialOffset;

        public float sunFlaresRadialOffset {
            get { return _sunFlaresRadialOffset; }
            set {
                if (_sunFlaresRadialOffset != value) {
                    _sunFlaresRadialOffset = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        bool
            _sunFlaresRotationDeadZone;

        public bool sunFlaresRotationDeadZone {
            get { return _sunFlaresRotationDeadZone; }
            set {
                if (_sunFlaresRotationDeadZone != value) {
                    _sunFlaresRotationDeadZone = value;
                    UpdateMaterialProperties();
                }
            }
        }





        #endregion

        #region Blur


        [SerializeField]
        bool
            _blur = false;

        public bool blur {
            get { return _blur; }
            set {
                if (_blur != value) {
                    _blur = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        [Range(0, 4f)]
        float
            _blurIntensity = 1f;

        public float blurIntensity {
            get { return _blurIntensity; }
            set {
                if (_blurIntensity != value) {
                    _blurIntensity = value;
                    UpdateMaterialProperties();
                }
            }
        }


        #endregion

        #region Downscale

        [SerializeField]
        [Range(1, 8)]
        float
            _downscale = 1;

        public float downscale {
            get { return _downscale; }
            set {
                if (_downscale != value) {
                    _downscale = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        [Range(1, 3)]
        int _superSampling = 1;

        public int superSampling {
            get { return _superSampling; }
            set {
                if (_superSampling != value) {
                    _superSampling = value;
                    UpdateMaterialProperties();
                }
            }
        }


        float renderScale {
            get {
                if (_quality == BEAUTIFY_QUALITY.BestPerformance) {
                    return _downscale;
                }
                else if (_quality == BEAUTIFY_QUALITY.BestQuality && !Application.isMobilePlatform) {
                    return 1f / (0.5f + _superSampling / 2f);
                }
                return 1f;
            }
        }

        #endregion

        #region Pixelate

        [SerializeField]
        [Range(1, 256)]
        int
            _pixelateAmount = 1;

        public int pixelateAmount {
            get { return _pixelateAmount; }
            set {
                if (_pixelateAmount != value) {
                    _pixelateAmount = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        bool
            _pixelateDownscale;

        public bool pixelateDownscale {
            get { return _pixelateDownscale; }
            set {
                if (_pixelateDownscale != value) {
                    _pixelateDownscale = value;
                    UpdateMaterialProperties();
                }
            }
        }

        #endregion

        #region Antialias

        [SerializeField]
        [Range(0, 20)]
        float
            _antialiasStrength = 5f;

        public float antialiasStrength {
            get { return _antialiasStrength; }
            set {
                if (_antialiasStrength != value) {
                    _antialiasStrength = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0.1f, 8f)]
        float _antialiasMaxSpread = 3f;

        public float antialiasMaxSpread {
            get { return _antialiasMaxSpread; }
            set {
                if (_antialiasMaxSpread != value) {
                    _antialiasMaxSpread = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        [Range(0f, 0.001f)]
        float _antialiasDepthThreshold = 0.000001f;

        public float antialiasDepthThreshold {
            get { return _antialiasDepthThreshold; }
            set {
                if (_antialiasDepthThreshold != value) {
                    _antialiasDepthThreshold = value;
                    UpdateMaterialProperties();
                }
            }
        }


        [SerializeField]
        float _antialiasDepthAtten;

        public float antialiasDepthAtten {
            get { return _antialiasDepthAtten; }
            set {
                if (_antialiasDepthAtten != value) {
                    _antialiasDepthAtten = value;
                    UpdateMaterialProperties();
                }
            }
        }


        #endregion

        #region Chromatic Aberration

        [SerializeField]
        bool
            _chromaticAberration;

        public bool chromaticAberration {
            get { return _chromaticAberration; }
            set {
                if (_chromaticAberration != value) {
                    _chromaticAberration = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0, 0.05f)]
        float _chromaticAberrationIntensity;
        public float chromaticAberrationIntensity {
            get { return _chromaticAberrationIntensity; }
            set {
                if (_chromaticAberrationIntensity != value) {
                    _chromaticAberrationIntensity = value;
                    UpdateMaterialProperties();
                }
            }
        }

        [SerializeField]
        [Range(0, 32f)]
        float _chromaticAberrationSmoothing;
        public float chromaticAberrationSmoothing {
            get { return _chromaticAberrationSmoothing; }
            set {
                if (_chromaticAberrationSmoothing != value) {
                    _chromaticAberrationSmoothing = value;
                    UpdateMaterialProperties();
                }
            }
        }

        #endregion


        public static Beautify instance {
            get {
                if (_beautify == null) {
                    foreach (Camera camera in Camera.allCameras) {
                        _beautify = camera.GetComponent<Beautify>();
                        if (_beautify != null)
                            break;
                    }
                }
                return _beautify;
            }
        }

        public Camera cameraEffect { get { return currentCamera; } }

        // Internal stuff **************************************************************************************************************

        public bool isDirty;
        static Beautify _beautify;

        Material bMatDesktop, bMatMobile, bMatBasic;
        static Color ColorTransparent = new Color(0, 0, 0, 0);

        [SerializeField]
        Material bMat;
        Camera currentCamera;
        Vector3 camPrevPos;
        Quaternion camPrevRotation;
        float currSens;
        int renderPass;
        RenderTextureFormat rtFormat;
        RenderTexture[] rt, rtAF, rtEA;
        RenderTexture rtEAacum, rtEAHist;
        float dofPrevDistance, dofLastAutofocusDistance;
        Vector4 dofLastBokehData;
        Camera depthCam;
        GameObject depthCamObj;
        List<string> shaderKeywords;
        Shader depthShader, dofExclusionShader;
        bool shouldUpdateMaterialProperties;
        const string BEAUTIFY_BUILD_HINT = "BeautifyBuildHint22rc5";
        float sunFlareCurrentIntensity;
        bool sunIsSpotlight;
        Vector4 sunLastScrPos;
        float sunLastRot;
        Texture2D flareNoise;
        RenderTexture dofDepthTexture, dofExclusionTexture;
        RenderTexture bloomSourceTexture, bloomSourceDepthTexture, bloomSourceTextureRightEye, bloomSourceDepthTextureRightEye;
        RenderTexture anamorphicFlaresSourceTexture, anamorphicFlaresSourceDepthTexture, anamorphicFlaresSourceTextureRightEye, anamorphicFlaresSourceDepthTextureRightEye;
        RenderTexture pixelateTexture;
        RenderTextureDescriptor rtDescBase;
        float sunFlareTime;
        int dofCurrentLayerMaskValue, bloomCurrentLayerMaskValue, anamorphicFlaresCurrentLayerMaskValue;
        int eyeWidth, eyeHeight;
        bool isSuperSamplingActive;
        RenderTextureFormat rtOutlineColorFormat;
        bool linearColorSpace;

#if UNITY_EDITOR
        public static CameraType captureCameraType = CameraType.SceneView;
        public static bool requestScreenCapture;
        RenderTexture rtCapture;
#endif

        #region Game loop events

        // Creates a private material used to the effect
        void OnEnable () {
            CheckColorSpace();
            currentCamera = GetComponent<Camera>();

            VRCheck.Init();
            if (VRCheck.isVrRunning) {
                rtDescBase = UnityEngine.XR.XRSettings.eyeTextureDesc;
                rtDescBase.msaaSamples = Mathf.Max(1, rtDescBase.msaaSamples);
            }
            else {
                rtDescBase = GetDefaultRenderTextureDescriptor();
            }
            rtOutlineColorFormat = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.R8) ? RenderTextureFormat.R8 : rtDescBase.colorFormat;
            if (_syncWithProfile && _profile != null) {
                _profile.Load(this);
            }
            if (VRCheck.isVrRunning) {
                _downscale = 1;
                _pixelateDownscale = false;
            }

            UpdateMaterialPropertiesNow();

#if UNITY_EDITOR
            if (EditorPrefs.GetInt(BEAUTIFY_BUILD_HINT) == 0) {
                EditorPrefs.SetInt(BEAUTIFY_BUILD_HINT, 1);
                EditorUtility.DisplayDialog("Beautify Update", "Beautify shaders have been updated. Please check the 'Shader Options' button in Beautify's inspector for new shader capabilities and disable/enable features to optimize build size and compilation time.\n\nOtherwise when you build the game it will take a long time during shader compilation!", "Ok");
            }
#endif
            isDirty = false;
        }

        void OnDestroy () {
            CleanUpRT();
            if (depthCamObj != null) {
                DestroyImmediate(depthCamObj);
                depthCamObj = null;
            }
            if (rtEAacum != null)
                rtEAacum.Release();
            if (rtEAHist != null)
                rtEAHist.Release();
            if (bMatDesktop != null) {
                DestroyImmediate(bMatDesktop);
                bMatDesktop = null;
            }
            if (bMatMobile != null) {
                DestroyImmediate(bMatMobile);
                bMatMobile = null;
            }
            if (bMatBasic != null) {
                DestroyImmediate(bMatBasic);
                bMatBasic = null;
            }
            bMat = null;

#if UNITY_EDITOR
            if (rtCapture != null) {
                rtCapture.Release();
            }
#endif
        }

        void Reset () {
            UpdateMaterialPropertiesNow();
        }


        private void OnValidate () {
            _bloomIntensity = Mathf.Max(0, _bloomIntensity);
            _bloomAntiflickerMaxOutput = Mathf.Max(0, _bloomAntiflickerMaxOutput);
            _bloomDepthAtten = Mathf.Max(0, bloomDepthAtten);
            _anamorphicFlaresIntensity = Mathf.Max(0, _anamorphicFlaresIntensity);
            _anamorphicFlaresAntiflickerMaxOutput = Mathf.Max(0, _anamorphicFlaresAntiflickerMaxOutput);
            _antialiasDepthAtten = Mathf.Max(0, antialiasDepthAtten);
            _depthofFieldAutofocusViewportPoint.x = Mathf.Clamp01(_depthofFieldAutofocusViewportPoint.x);
            _depthofFieldAutofocusViewportPoint.y = Mathf.Clamp01(_depthofFieldAutofocusViewportPoint.y);
            _sunFlaresRadialOffset = Mathf.Max(0, sunFlaresRadialOffset);
        }

        void LateUpdate () {
            if (bMat == null || !Application.isPlaying || _sharpenMotionSensibility <= 0)
                return;

            // Motion sensibility v2
            Vector3 pos = currentCamera.transform.position;
            Quaternion q = currentCamera.transform.rotation;

            float dt = Time.deltaTime;
            if (pos != camPrevPos || q.x != camPrevRotation.x || q.y != camPrevRotation.y || q.z != camPrevRotation.z || q.w != camPrevRotation.w) {
                currSens = Mathf.Lerp(currSens, _sharpen * _sharpenMotionSensibility, 30f * _sharpenMotionSensibility * dt);
                camPrevPos = pos;
                camPrevRotation = q;
            }
            else {
                if (currSens <= 0.001f)
                    return;
                currSens -= 30f * _sharpenMotionRestoreSpeed * dt;
            }

            currSens = Mathf.Clamp(currSens, 0, _sharpen);
            float tempSharpen = _sharpen - currSens;

            UpdateSharpenParams(tempSharpen);
        }

        void OnPreCull () {   // Aquas issue with OnPreRender

            if (_preRenderCameraEvent == BEAUTIFY_PRERENDER_EVENT.OnPreCull) {
                DoOnPreRenderTasks();
            }

#if UNITY_2022_3_OR_NEWER
                ConfigureRenderScale();
#endif
        }

        void DoOnPreRenderTasks () {
            CleanUpRT();

            if (!enabled || !gameObject.activeSelf || currentCamera == null || bMat == null || (!_depthOfField && !_bloom && !_anamorphicFlares))
                return;

            if (dofCurrentLayerMaskValue != _depthOfFieldExclusionLayerMask.value)
                shouldUpdateMaterialProperties = true;

            if (depthOfField && (_depthOfFieldTransparencySupport || isUsingDepthOfFieldExclusionLayerMask)) {
                CheckDoFTransparencySupport();
                CheckDoFExclusionMask();
            }

            if (_bloomCullingMask.value != bloomCurrentLayerMaskValue || _anamorphicFlaresCullingMask.value != anamorphicFlaresCurrentLayerMaskValue) {
                shouldUpdateMaterialProperties = true;
            }

            if ((_bloom && isUsingBloomLayerMask) || (_anamorphicFlares && isUsingAnamorphicFlaresLayerMask)) {
                CheckBloomAndFlaresCulling();
            }
        }

        void OnPreRender () {

            if (_preRenderCameraEvent == BEAUTIFY_PRERENDER_EVENT.OnPreRender) {
                DoOnPreRenderTasks();
            }

#if !UNITY_2022_3_OR_NEWER
            ConfigureRenderScale();
#endif

        }

        void ConfigureRenderScale () {

            isSuperSamplingActive = false;

            float scaleFactor = renderScale;

            if (Camera.current.cameraType == CameraType.SceneView) return;

            if (scaleFactor != 1f && rtDescBase.width > 1) {
                _pixelateAmount = 1;
                RenderTextureDescriptor rtPixDesc = rtDescBase;
                if (scaleFactor <= 1) {
                    // preserve msaa setting when super sampling
                    rtPixDesc.msaaSamples = Mathf.Max(1, QualitySettings.antiAliasing);
                    isSuperSamplingActive = true;
                }
                float w = Screen.width / scaleFactor; // use screen.width instead of pixelwidth to account for custom viewport width
                float h = Screen.height / scaleFactor;
                rtPixDesc.width = Mathf.RoundToInt(Mathf.Clamp(w, 1, 8192));
                float aspectRatio = h / w;
                rtPixDesc.height = Mathf.RoundToInt(Mathf.Clamp(w * aspectRatio, 1, 8192));
                pixelateTexture = RenderTexture.GetTemporary(rtPixDesc);
                currentCamera.targetTexture = pixelateTexture;
            }
            else if (_pixelateDownscale && _pixelateAmount > 1 && rtDescBase.width > 1 && rtDescBase.height > 1) {
                RenderTextureDescriptor rtPixDesc = rtDescBase;
                rtPixDesc.width = Mathf.RoundToInt(Mathf.Max(1, currentCamera.pixelWidth / _pixelateAmount));
                float aspectRatio = (float)currentCamera.pixelHeight / currentCamera.pixelWidth;
                rtPixDesc.height = Mathf.Max(1, Mathf.RoundToInt(rtPixDesc.width * aspectRatio));
                pixelateTexture = RenderTexture.GetTemporary(rtPixDesc);
                currentCamera.targetTexture = pixelateTexture;
            }
        }

        void CleanUpRT () {
            if (dofDepthTexture != null) {
                RenderTexture.ReleaseTemporary(dofDepthTexture);
                dofDepthTexture = null;
            }
            if (dofExclusionTexture != null) {
                RenderTexture.ReleaseTemporary(dofExclusionTexture);
                dofExclusionTexture = null;
            }
            if (bloomSourceTexture != null) {
                RenderTexture.ReleaseTemporary(bloomSourceTexture);
                bloomSourceTexture = null;
            }
            if (bloomSourceDepthTexture != null) {
                RenderTexture.ReleaseTemporary(bloomSourceDepthTexture);
                bloomSourceDepthTexture = null;
            }
            if (bloomSourceTextureRightEye != null) {
                RenderTexture.ReleaseTemporary(bloomSourceTextureRightEye);
                bloomSourceTextureRightEye = null;
            }
            if (bloomSourceDepthTextureRightEye != null) {
                RenderTexture.ReleaseTemporary(bloomSourceDepthTextureRightEye);
                bloomSourceDepthTextureRightEye = null;
            }
            if (anamorphicFlaresSourceTexture != null) {
                RenderTexture.ReleaseTemporary(anamorphicFlaresSourceTexture);
                anamorphicFlaresSourceTexture = null;
            }
            if (anamorphicFlaresSourceDepthTexture != null) {
                RenderTexture.ReleaseTemporary(anamorphicFlaresSourceDepthTexture);
                anamorphicFlaresSourceDepthTexture = null;
            }
            if (anamorphicFlaresSourceTextureRightEye != null) {
                RenderTexture.ReleaseTemporary(anamorphicFlaresSourceTextureRightEye);
                anamorphicFlaresSourceTextureRightEye = null;
            }
            if (anamorphicFlaresSourceDepthTextureRightEye != null) {
                RenderTexture.ReleaseTemporary(anamorphicFlaresSourceDepthTextureRightEye);
                anamorphicFlaresSourceDepthTextureRightEye = null;
            }
            if (pixelateTexture != null) {
                RenderTexture.ReleaseTemporary(pixelateTexture);
                pixelateTexture = null;
            }
        }

        RenderTextureDescriptor GetDefaultRenderTextureDescriptor () {
            RenderTextureDescriptor desc = new RenderTextureDescriptor(currentCamera.pixelWidth, currentCamera.pixelHeight, RenderTextureFormat.ARGB32, 24);
            desc.msaaSamples = Math.Max(1, QualitySettings.antiAliasing);
            desc.sRGB = !linearColorSpace;
            return desc;
        }

        void CheckDoFTransparencySupport () {
            if (depthCam == null) {
                if (depthCamObj == null) {
                    depthCamObj = new GameObject("DepthCamera");
                    depthCamObj.hideFlags = HideFlags.HideAndDontSave;
                    depthCam = depthCamObj.AddComponent<Camera>();
                    depthCam.enabled = false;
                }
                else {
                    depthCam = depthCamObj.GetComponent<Camera>();
                    if (depthCam == null) {
                        DestroyImmediate(depthCamObj);
                        depthCamObj = null;
                        return;
                    }
                }
            }
            depthCam.CopyFrom(currentCamera);
            depthCam.rect = new Rect(0, 0, 1f, 1f);
            depthCam.depthTextureMode = DepthTextureMode.None;
            depthCam.renderingPath = RenderingPath.Forward;
            float downsampling = _depthOfFieldTransparencySupportDownsampling * _depthOfFieldDownsampling;
            dofDepthTexture = RenderTexture.GetTemporary((int)(currentCamera.pixelWidth / downsampling), (int)(currentCamera.pixelHeight / downsampling), 16, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
            dofDepthTexture.filterMode = FilterMode.Point;
            depthCam.backgroundColor = new Color(0.9882353f, 0.4470558f, 0.75f, 0f); // new Color (1, 1, 1, 1);
            depthCam.clearFlags = CameraClearFlags.SolidColor;
            depthCam.targetTexture = dofDepthTexture;
            depthCam.cullingMask = _depthOfFieldTransparencyLayerMask;
            if (depthShader == null) {
                depthShader = Shader.Find("Hidden/Kronnect/Beautify/CopyDepth");
            }
            depthCam.RenderWithShader(depthShader, "RenderType");
            bMat.SetTexture(ShaderParams.DepthTexture, dofDepthTexture);
        }

        void CheckDoFExclusionMask () {
            if (depthCam == null) {
                if (depthCamObj == null) {
                    depthCamObj = new GameObject("DepthCamera");
                    depthCamObj.hideFlags = HideFlags.HideAndDontSave;
                    depthCam = depthCamObj.AddComponent<Camera>();
                    depthCam.enabled = false;
                }
                else {
                    depthCam = depthCamObj.GetComponent<Camera>();
                    if (depthCam == null) {
                        DestroyImmediate(depthCamObj);
                        depthCamObj = null;
                        return;
                    }
                }
            }
            depthCam.CopyFrom(currentCamera);
            depthCam.rect = new Rect(0, 0, 1f, 1f);
            depthCam.depthTextureMode = DepthTextureMode.None;
            depthCam.renderingPath = RenderingPath.Forward;
            float downsampling = _depthOfFieldExclusionLayerMaskDownsampling * _depthOfFieldDownsampling;
            dofExclusionTexture = RenderTexture.GetTemporary((int)(currentCamera.pixelWidth / downsampling), (int)(currentCamera.pixelHeight / downsampling), 16, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
            dofExclusionTexture.filterMode = FilterMode.Point;
            depthCam.backgroundColor = new Color(0.9882353f, 0.4470558f, 0.75f, 0f); // new Color (1, 1, 1, 1);
            depthCam.clearFlags = CameraClearFlags.SolidColor;
            depthCam.targetTexture = dofExclusionTexture;
            depthCam.cullingMask = _depthOfFieldExclusionLayerMask;
            if (dofExclusionShader == null) {
                dofExclusionShader = Shader.Find("Hidden/Kronnect/Beautify/CopyDepthBiased");
            }
            depthCam.RenderWithShader(dofExclusionShader, null);
            bMat.SetTexture(ShaderParams.DoFExclusionTexture, dofExclusionTexture);
        }

        bool isUsingAnamorphicFlaresLayerMask => _anamorphicFlaresCullingMask != 0 && _anamorphicFlaresCullingMask != -1;
        bool isUsingBloomLayerMask => _bloomCullingMask != 0 && _bloomCullingMask != -1;
        bool isUsingDepthOfFieldExclusionLayerMask => _depthOfFieldExclusionLayerMask != 0 && _depthOfFieldExclusionLayerMask != -1;

        void CheckBloomAndFlaresCulling () {

            if (rtDescBase.volumeDepth == 0) {
                return;
            }

            // Reuses depth camera
            if (depthCam == null) {
                if (depthCamObj == null) {
                    depthCamObj = new GameObject("DepthCamera");
                    depthCamObj.hideFlags = HideFlags.HideAndDontSave;
                    depthCam = depthCamObj.AddComponent<Camera>();
                    depthCam.enabled = false;
                }
                else {
                    depthCam = depthCamObj.GetComponent<Camera>();
                    if (depthCam == null) {
                        DestroyImmediate(depthCamObj);
                        depthCamObj = null;
                        return;
                    }
                }
            }
            depthCam.CopyFrom(currentCamera);
            depthCam.rect = new Rect(0, 0, 1f, 1f);
            depthCam.depthTextureMode = DepthTextureMode.None;
            depthCam.allowMSAA = false;
            depthCam.allowHDR = false;
            depthCam.clearFlags = CameraClearFlags.SolidColor;
            depthCam.stereoTargetEye = StereoTargetEyeMask.None;
            depthCam.renderingPath = RenderingPath.Forward; // currently this feature does not work in deferred
            depthCam.backgroundColor = Color.black;
            if (_bloom && isUsingBloomLayerMask) {
                depthCam.cullingMask = _bloomCullingMask;
                if (_quality == BEAUTIFY_QUALITY.BestPerformance) {
                    eyeWidth = _bloomUltra ? (int)(Mathf.Lerp(256, currentCamera.pixelHeight, _bloomUltraResolution / 10f) / 4f) * 4 : 256;
                }
                else {
                    eyeWidth = _bloomUltra ? (int)(Mathf.Lerp(512, currentCamera.pixelHeight, _bloomUltraResolution / 10f) / 4f) * 4 : 512;
                }
                eyeWidth = (int)(eyeWidth * (1f / _bloomLayerMaskDownsampling) / 4) * 4;
            }
            else {
                depthCam.cullingMask = _anamorphicFlaresCullingMask;
                if (_quality == BEAUTIFY_QUALITY.BestPerformance) {
                    eyeWidth = _anamorphicFlaresUltra ? (int)(Mathf.Lerp(256, currentCamera.pixelHeight, _anamorphicFlaresUltraResolution / 10f) / 4f) * 4 : 256;
                }
                else {
                    eyeWidth = _anamorphicFlaresUltra ? (int)(Mathf.Lerp(512, currentCamera.pixelHeight, _anamorphicFlaresUltraResolution / 10f) / 4f) * 4 : 512;
                }
                eyeWidth = (int)(eyeWidth * (1f / _anamorphicFlaresLayerMaskDownsampling) / 4) * 4;
            }
            float aspectRatio = (float)currentCamera.pixelHeight / currentCamera.pixelWidth;
            eyeHeight = Mathf.Max(1, (int)(eyeWidth * aspectRatio));

            if (VRCheck.isVrRunning) {
                depthCam.projectionMatrix = currentCamera.GetStereoProjectionMatrix(Camera.StereoscopicEye.Left);
            }
            RenderLeftEyeDepth();
            if (VRCheck.isVrRunning) {
                depthCam.projectionMatrix = currentCamera.GetStereoProjectionMatrix(Camera.StereoscopicEye.Right);
                RenderRightEyeDepth();
            }

            if (_bloom && _anamorphicFlares && _anamorphicFlaresCullingMask != _bloomCullingMask) {
                depthCam.cullingMask = _anamorphicFlaresCullingMask;
                if (VRCheck.isVrRunning) {
                    depthCam.projectionMatrix = currentCamera.GetStereoProjectionMatrix(Camera.StereoscopicEye.Left);
                }
                if (_quality == BEAUTIFY_QUALITY.BestPerformance) {
                    eyeWidth = 256;
                }
                else {
                    eyeWidth = _anamorphicFlaresUltra ? (int)(Mathf.Lerp(512, currentCamera.pixelHeight, _anamorphicFlaresUltraResolution / 10f) / 4f) * 4 : 512;
                    eyeWidth = (int)(eyeWidth * (1f / _anamorphicFlaresLayerMaskDownsampling) / 4) * 4;
                }
                aspectRatio = (float)currentCamera.pixelHeight / currentCamera.pixelWidth;
                eyeHeight = Mathf.Max(1, (int)(eyeWidth * aspectRatio));
                RenderLeftEyeDepthAF();
                if (VRCheck.isVrRunning) {
                    depthCam.projectionMatrix = currentCamera.GetStereoProjectionMatrix(Camera.StereoscopicEye.Right);
                    RenderRightEyeDepthAF();
                }
            }
        }

        void RenderLeftEyeDepth () {

            RenderTextureDescriptor desc = rtDescBase;
            desc.width = eyeWidth;
            desc.height = eyeHeight;
            desc.depthBufferBits = 24;
            desc.colorFormat = RenderTextureFormat.Depth;
            bloomSourceDepthTexture = RenderTexture.GetTemporary(desc);
            desc.depthBufferBits = 0;
            desc.colorFormat = rtFormat;
            bloomSourceTexture = RenderTexture.GetTemporary(desc);

            depthCam.SetTargetBuffers(bloomSourceTexture.colorBuffer, bloomSourceDepthTexture.depthBuffer);
            depthCam.Render();

            bMat.SetTexture(ShaderParams.BloomSourceTexture, bloomSourceTexture);
            bMat.SetTexture(ShaderParams.BloomSourceDepthTexture, bloomSourceDepthTexture);
        }


        void RenderRightEyeDepth () {

            RenderTextureDescriptor desc = rtDescBase;
            desc.width = eyeWidth;
            desc.height = eyeHeight;
            desc.depthBufferBits = 24;
            desc.colorFormat = RenderTextureFormat.Depth;
            bloomSourceDepthTextureRightEye = RenderTexture.GetTemporary(desc);
            desc.depthBufferBits = 0;
            desc.colorFormat = rtFormat;
            bloomSourceTextureRightEye = RenderTexture.GetTemporary(desc);

            depthCam.SetTargetBuffers(bloomSourceTextureRightEye.colorBuffer, bloomSourceDepthTextureRightEye.depthBuffer);
            depthCam.Render();

            bMat.SetTexture(ShaderParams.BloomSourceRightEyeTexture, bloomSourceTextureRightEye);
            bMat.SetTexture(ShaderParams.BloomSourceRightEyeDepthTexture, bloomSourceDepthTextureRightEye);
        }

        void RenderLeftEyeDepthAF () {

            RenderTextureDescriptor desc = rtDescBase;
            desc.width = eyeWidth;
            desc.height = eyeHeight;
            desc.depthBufferBits = 24;
            desc.colorFormat = RenderTextureFormat.Depth;
            anamorphicFlaresSourceDepthTexture = RenderTexture.GetTemporary(desc);
            desc.depthBufferBits = 0;
            desc.colorFormat = rtFormat;
            anamorphicFlaresSourceTexture = RenderTexture.GetTemporary(desc);

            depthCam.SetTargetBuffers(anamorphicFlaresSourceTexture.colorBuffer, anamorphicFlaresSourceDepthTexture.depthBuffer);
            depthCam.Render();
            bMat.SetTexture(ShaderParams.BloomSourceTexture, bloomSourceTexture);
            bMat.SetTexture(ShaderParams.BloomSourceDepthTexture, bloomSourceDepthTexture);
        }

        void RenderRightEyeDepthAF () {

            RenderTextureDescriptor desc = rtDescBase;
            desc.width = eyeWidth;
            desc.height = eyeHeight;
            desc.depthBufferBits = 24;
            desc.colorFormat = RenderTextureFormat.Depth;
            anamorphicFlaresSourceDepthTextureRightEye = RenderTexture.GetTemporary(desc);
            desc.depthBufferBits = 0;
            desc.colorFormat = rtFormat;
            anamorphicFlaresSourceTextureRightEye = RenderTexture.GetTemporary(desc);

            depthCam.SetTargetBuffers(anamorphicFlaresSourceTextureRightEye.colorBuffer, anamorphicFlaresSourceDepthTextureRightEye.depthBuffer);
            depthCam.Render();
            bMat.SetTexture(ShaderParams.BloomSourceRightEyeTexture, bloomSourceTextureRightEye);
            bMat.SetTexture(ShaderParams.BloomSourceRightEyeDepthTexture, bloomSourceDepthTextureRightEye);
        }

        int GetRawCopyPass () { return _quality == BEAUTIFY_QUALITY.BestQuality ? 22 : 18; }

        protected virtual void OnRenderImage (RenderTexture source, RenderTexture destination) {

            Camera cam = Camera.current;

#if UNITY_EDITOR
            if (requestScreenCapture && cam != null && cam.cameraType == captureCameraType) {
                requestScreenCapture = false;
                if (rtCapture != null) {
                    rtCapture.Release();
                }
                rtCapture = new RenderTexture(source.descriptor);
                Graphics.Blit(source, rtCapture);
                Shader.SetGlobalTexture(ShaderParams.LUTPreview, rtCapture);
            }
#endif

            if (bMat == null || !enabled) {
                Graphics.Blit(source, destination);
                return;
            }

            if (shouldUpdateMaterialProperties) {
                UpdateMaterialPropertiesNow();
            }

            bool allowExtraEffects = (_quality != BEAUTIFY_QUALITY.Basic);

            // Copy source settings; RRTT will be created using descriptor to take advantage of the vrUsage field.
            rtDescBase = source.descriptor;
            if (isSuperSamplingActive) {
                rtDescBase.width = currentCamera.pixelWidth;
                rtDescBase.height = currentCamera.pixelHeight;
            }
            rtDescBase.msaaSamples = 1;
            rtDescBase.colorFormat = rtFormat;
            rtDescBase.depthBufferBits = 0;

            // Reverses upscale
            RenderTexture upscaleDownRT = null;
            if (_quality == BEAUTIFY_QUALITY.BestQuality && _superSampling > 1f) {
                RenderTextureDescriptor upscaleDownDesc = rtDescBase;
                upscaleDownDesc.width = currentCamera.pixelWidth;
                upscaleDownDesc.height = currentCamera.pixelHeight;
                upscaleDownRT = RenderTexture.GetTemporary(upscaleDownDesc);
                Graphics.Blit(source, upscaleDownRT, bMat, 22);
                source = upscaleDownRT;
            }

            // Prepare compare & final blur buffer
            RenderTexture rtBeauty = null;
            RenderTexture rtBlurTex = null;
            RenderTexture rtCustomBloom = null;

            float aspectRatio = (float)source.height / source.width;

            bool doFinalBlur = _blur && _blurIntensity > 0 && allowExtraEffects;
            if (renderPass == 0 || doFinalBlur) {
                if (doFinalBlur) {
                    int size;
                    if (_blurIntensity < 1f) {
                        size = (int)Mathf.Lerp(currentCamera.pixelWidth, 512, _blurIntensity);
                        if (_quality == BEAUTIFY_QUALITY.BestPerformance)
                            size /= 2;
                    }
                    else {
                        size = _quality == BEAUTIFY_QUALITY.BestQuality ? 512 : 256;
                        size = (int)(size / _blurIntensity);
                    }
                    RenderTextureDescriptor rtBlurDesc = rtDescBase;
                    rtBlurDesc.width = size;
                    rtBlurDesc.height = Mathf.Max(1, (int)(size * aspectRatio));
                    rtBlurTex = RenderTexture.GetTemporary(rtBlurDesc);
                    if (renderPass == 0) {
                        rtBeauty = RenderTexture.GetTemporary(rtBlurDesc);
                    }
                }
                else {
                    rtBeauty = RenderTexture.GetTemporary(rtDescBase);
                }
            }

            RenderTexture rtPixelated = null;
            RenderTexture rtDoF = null;
            RenderTexture rtSF = null;

            if (allowExtraEffects) {
                // Pixelate
                if (_pixelateAmount > 1) {
                    source.filterMode = FilterMode.Point;
                    if (!_pixelateDownscale) {
                        RenderTextureDescriptor rtPixDesc = rtDescBase;
                        rtPixDesc.width = Mathf.RoundToInt(Mathf.Max(1, source.width / _pixelateAmount));
                        rtPixDesc.height = Mathf.Max(1, Mathf.RoundToInt(rtPixDesc.width * aspectRatio));
                        rtPixelated = RenderTexture.GetTemporary(rtPixDesc);
                        rtPixelated.filterMode = FilterMode.Point;
                        Graphics.Blit(source, rtPixelated, bMat, _quality == BEAUTIFY_QUALITY.BestQuality ? 22 : 18);
                        source = rtPixelated;
                    }
                }


                // DoF!
                if (_depthOfField) {
#if UNITY_EDITOR
                    if (cam != null && cam.cameraType != CameraType.SceneView) {
                        if (!bMat.IsKeywordEnabled(ShaderParams.SKW_DEPTH_OF_FIELD)) {
                            bMat.EnableKeyword(ShaderParams.SKW_DEPTH_OF_FIELD);
                        }
                        if ((_depthOfFieldTransparencySupport || isUsingDepthOfFieldExclusionLayerMask) && !bMat.IsKeywordEnabled(ShaderParams.SKW_DEPTH_OF_FIELD_TRANSPARENT)) {
                            bMat.EnableKeyword(ShaderParams.SKW_DEPTH_OF_FIELD_TRANSPARENT);
                        }
#endif
                        UpdateDepthOfFieldData();

                        int pass = _quality == BEAUTIFY_QUALITY.BestQuality ? 12 : 6;
                        RenderTextureDescriptor rtDofDescriptor = rtDescBase;
                        rtDofDescriptor.width = source.width / _depthOfFieldDownsampling;
                        rtDofDescriptor.height = source.height / _depthOfFieldDownsampling;
                        rtDoF = RenderTexture.GetTemporary(rtDofDescriptor);
                        rtDoF.filterMode = _depthOfFieldFilterMode;
                        Graphics.Blit(source, rtDoF, bMat, pass);

                        if (_quality == BEAUTIFY_QUALITY.BestQuality && _depthOfFieldForegroundBlur && _depthOfFieldForegroundBlurHQ) {
                            BlurThisAlpha(rtDoF, _depthOfFieldForegroundBlurHQSpread);
                        }

                        if (depthOfFieldBokehComposition == BEAUTIFY_BOKEH_COMPOSITION.Integrated || _quality != BEAUTIFY_QUALITY.BestQuality || !depthOfFieldBokeh) {
                            if (_quality == BEAUTIFY_QUALITY.BestQuality) {
                                pass = _depthOfFieldBokeh ? 14 : 19;
                            }
                            else {
                                pass = _depthOfFieldBokeh ? 8 : 15;
                            }
                            BlurThisDoF(rtDoF, pass);
                        }
                        else {
                            BlurThisDoF(rtDoF, 19);

                            // separate & blend bokeh
                            RenderTexture rtBokeh = RenderTexture.GetTemporary(rtDofDescriptor);
                            Graphics.Blit(source, rtBokeh, bMat, 30);
                            BlurThisDoF(rtBokeh, 32);
                            Graphics.Blit(rtBokeh, rtDoF, bMat, 31);
                            RenderTexture.ReleaseTemporary(rtBokeh);
                        }

                        if (_depthOfFieldDebug) {
#if !UNITY_2021_1_OR_NEWER
                        source.MarkRestoreExpected();
#endif
                            pass = _quality == BEAUTIFY_QUALITY.BestQuality ? 13 : 7;
                            Graphics.Blit(rtDoF, destination, bMat, pass);
                            RenderTexture.ReleaseTemporary(rtDoF);
                            return;
                        }

                        Shader.SetGlobalTexture(ShaderParams.DoFTexture, rtDoF); // workaround due to a bug in Unity 2022.2.14
                                                                                 //bMat.SetTexture(ShaderParams.DoFTexture, rtDoF);
#if UNITY_EDITOR
                    }
                    else {
                        // Cancels DoF
                        if (bMat.IsKeywordEnabled(ShaderParams.SKW_DEPTH_OF_FIELD)) {
                            bMat.DisableKeyword(ShaderParams.SKW_DEPTH_OF_FIELD);
                        }
                        if (bMat.IsKeywordEnabled(ShaderParams.SKW_DEPTH_OF_FIELD_TRANSPARENT)) {
                            bMat.DisableKeyword(ShaderParams.SKW_DEPTH_OF_FIELD_TRANSPARENT);
                        }
                    }
#endif
                }

                // Separate Outline
                if (_outline && _outlineCustomize && _quality == BEAUTIFY_QUALITY.BestQuality && _outlineStage == BEAUTIFY_OUTLINE_STAGE.BeforeBloom) {
                    SeparateOutlinePass(source);
                }

            }

            bool sunFlareEnabled = _sunFlares && _sun != null;
            if (allowExtraEffects && (_lensDirt || _bloom || _anamorphicFlares || sunFlareEnabled)) {
                RenderTexture rtBloom = null;

                int pyramidCountBloom, pyramidCountAF, size, minSize;
                if (_quality == BEAUTIFY_QUALITY.BestPerformance) {
                    pyramidCountBloom = _bloomIterations;
                    pyramidCountAF = 4;
                    minSize = 256;
                }
                else {
                    pyramidCountBloom = pyramidCountAF = 5;
                    minSize = 512;
                }
                if (_bloomUltra) {
                    size = ((int)(source.width * _bloomUltraResolution / 10f) / 4) * 4;
                    size = Mathf.Max(size, minSize);
                }
                else {
                    size = minSize;
                }

                // Bloom buffers
                if (rt == null || rt.Length != pyramidCountBloom + 1) {
                    rt = new RenderTexture[pyramidCountBloom + 1];
                }
                // Anamorphic flare buffers
                if (rtAF == null || rtAF.Length != pyramidCountAF + 1) {
                    rtAF = new RenderTexture[pyramidCountAF + 1];
                }

                if (_bloom || (_lensDirt && !_anamorphicFlares)) {
                    UpdateMaterialBloomIntensityAndThreshold();
                    RenderTextureDescriptor rtBloomDescriptor = rtDescBase;
                    for (int k = 0; k <= pyramidCountBloom; k++) {
                        rtBloomDescriptor.width = Mathf.Max(1, size);
                        rtBloomDescriptor.height = Mathf.Max(1, (int)(size * aspectRatio));
                        rt[k] = RenderTexture.GetTemporary(rtBloomDescriptor);
                        size /= 2;
                    }
                    rtBloom = rt[0];

                    if (_bloomAntiflicker) {
                        Graphics.Blit(source, rt[0], bMat, _quality == BEAUTIFY_QUALITY.BestPerformance ? 22 : 9);
                    }
                    else {
                        Graphics.Blit(source, rt[0], bMat, 2);
                    }

                    if (!_bloomUltra || _bloomUltraResolution < 6) {
                        BlurThis(rt[0]);
                    }

                    for (int k = 0; k < pyramidCountBloom; k++) {
                        if (_quality == BEAUTIFY_QUALITY.BestPerformance) {
                            if (_bloomBlur) {
                                BlurThisDownscaling(rt[k], rt[k + 1]);
                            }
                            else {
                                Graphics.Blit(rt[k], rt[k + 1], bMat, 20);
                            }
                        }
                        else {
                            if (_bloomQuickerBlur) {
                                BlurThisDownscaling(rt[k], rt[k + 1]);
                            }
                            else {
                                Graphics.Blit(rt[k], rt[k + 1], bMat, 7);
                                BlurThis(rt[k + 1]);
                            }
                        }
                    }

                    if (_bloom) {
                        bMat.SetColor(ShaderParams.BloomTint, ColorTransparent);
                        bool customizesBloom = quality == BEAUTIFY_QUALITY.BestQuality && _bloomCustomize;
                        for (int k = pyramidCountBloom; k > 0; k--) {
#if !UNITY_2021_1_OR_NEWER
                            rt[k - 1].MarkRestoreExpected();
#endif
                            if (k == 1 && !customizesBloom) {
                                bMat.SetColor(ShaderParams.BloomTint, _bloomTint);
                            }
                            Graphics.Blit(rt[k], rt[k - 1], bMat, _quality == BEAUTIFY_QUALITY.BestQuality ? 8 : 21);
                        }
                        if (customizesBloom) {
                            bMat.SetColor(ShaderParams.BloomTint, _bloomTint);
                            bMat.SetTexture(ShaderParams.BloomTexture4, rt[4]);
                            bMat.SetTexture(ShaderParams.BloomTexture3, rt[3]);
                            bMat.SetTexture(ShaderParams.BloomTexture2, rt[2]);
                            bMat.SetTexture(ShaderParams.BloomTexture1, rt[1]);
                            Shader.SetGlobalTexture(ShaderParams.BloomTexture, rt[0]);
                            RenderTextureDescriptor rtCustomBloomDescriptor = rt[0].descriptor;
                            rtCustomBloom = RenderTexture.GetTemporary(rtCustomBloomDescriptor);
                            rtBloom = rtCustomBloom;
                            Graphics.Blit(rt[pyramidCountBloom], rtBloom, bMat, 6);
                        }
                        bMat.SetColor(ShaderParams.BloomTint, ColorTransparent);
                    }

                }

                // anamorphic flares
                if (_anamorphicFlares) {
                    UpdateMaterialAnamorphicIntensityAndThreshold();

                    int sizeAF;
                    if (_anamorphicFlaresUltra) {
                        sizeAF = ((int)(source.width * _anamorphicFlaresUltraResolution / 10f) / 4) * 4;
                        sizeAF = Mathf.Max(sizeAF, minSize);
                    }
                    else {
                        sizeAF = minSize;
                    }

                    RenderTextureDescriptor rtAFDescriptor = rtDescBase;
                    for (int origSize = sizeAF, k = 0; k <= pyramidCountAF; k++) {
                        int afSize = (int)(sizeAF / _anamorphicFlaresSpread);
                        if (_anamorphicFlaresVertical) {
                            rtAFDescriptor.width = origSize;
                            rtAFDescriptor.height = Mathf.Max(1, afSize);
                        }
                        else {
                            rtAFDescriptor.width = Mathf.Max(1, afSize);
                            rtAFDescriptor.height = origSize;
                        }
                        EnsureSafeDimensions(ref rtAFDescriptor);
                        rtAF[k] = RenderTexture.GetTemporary(rtAFDescriptor);
                        sizeAF /= 2;
                    }

                    if (isUsingAnamorphicFlaresLayerMask) {
                        if (_bloom) {
                            if (_anamorphicFlaresCullingMask != _bloomCullingMask) {
                                bMat.SetTexture(ShaderParams.BloomSourceTexture, anamorphicFlaresSourceTexture);
                                bMat.SetTexture(ShaderParams.BloomSourceDepthTexture, anamorphicFlaresSourceDepthTexture);
                                bMat.SetTexture(ShaderParams.BloomSourceRightEyeTexture, anamorphicFlaresSourceTextureRightEye);
                                bMat.SetTexture(ShaderParams.BloomSourceRightEyeDepthTexture, anamorphicFlaresSourceDepthTextureRightEye);
                            }
                        }
                        else {
                            bMat.SetTexture(ShaderParams.BloomSourceTexture, bloomSourceTexture);
                            bMat.SetTexture(ShaderParams.BloomSourceDepthTexture, bloomSourceDepthTexture);
                            bMat.SetTexture(ShaderParams.BloomSourceRightEyeTexture, bloomSourceTextureRightEye);
                            bMat.SetTexture(ShaderParams.BloomSourceRightEyeDepthTexture, bloomSourceDepthTextureRightEye);
                        }
                    }

                    if (_anamorphicFlaresAntiflicker) {
                        Graphics.Blit(source, rtAF[0], bMat, _quality == BEAUTIFY_QUALITY.BestPerformance ? 22 : 9);
                    }
                    else {
                        Graphics.Blit(source, rtAF[0], bMat, 2);
                    }

                    rtAF[0] = BlurThisOneDirection(rtAF[0], _anamorphicFlaresVertical);

                    for (int k = 0; k < pyramidCountAF; k++) {
                        if (_quality == BEAUTIFY_QUALITY.BestPerformance) {
                            Graphics.Blit(rtAF[k], rtAF[k + 1], bMat, 18);
                            if (_anamorphicFlaresBlur) {
                                rtAF[k + 1] = BlurThisOneDirection(rtAF[k + 1], _anamorphicFlaresVertical);
                            }
                        }
                        else {
                            Graphics.Blit(rtAF[k], rtAF[k + 1], bMat, 7);
                            rtAF[k + 1] = BlurThisOneDirection(rtAF[k + 1], _anamorphicFlaresVertical);
                        }
                    }

                    for (int k = pyramidCountAF; k > 0; k--) {
#if !UNITY_2021_1_OR_NEWER
                        rtAF[k - 1].MarkRestoreExpected();
#endif
                        if (k == 1) {
                            Graphics.Blit(rtAF[k], rtAF[k - 1], bMat, _quality == BEAUTIFY_QUALITY.BestQuality ? 10 : 14); // applies intensity in last stage
                        }
                        else {
                            Graphics.Blit(rtAF[k], rtAF[k - 1], bMat, _quality == BEAUTIFY_QUALITY.BestQuality ? 8 : 13);
                        }
                    }
                    if (_bloom) {
                        if (_lensDirt) {
#if !UNITY_2021_1_OR_NEWER
                            rt[3].MarkRestoreExpected();
#endif
                            Graphics.Blit(rtAF[3], rt[3], bMat, _quality == BEAUTIFY_QUALITY.BestQuality ? 11 : 13);
                        }
#if !UNITY_2021_1_OR_NEWER
                        rtBloom.MarkRestoreExpected();
#endif
                        Graphics.Blit(rtAF[0], rtBloom, bMat, _quality == BEAUTIFY_QUALITY.BestQuality ? 11 : 13);
                    }
                    else {
                        rtBloom = rtAF[0];
                    }
                    UpdateMaterialBloomIntensityAndThreshold();
                }

                if (sunFlareEnabled) {
                    // check if Sun is visible
                    Vector3 sunWorldPosition;
                    float maxDistance;
                    Vector3 toLightVector;
                    if (sunIsSpotlight) {
                        sunWorldPosition = _sun.transform.position;
                        toLightVector = sunWorldPosition - currentCamera.transform.position;
                        maxDistance = toLightVector.magnitude;
                        toLightVector /= maxDistance;
                    }
                    else {
                        sunWorldPosition = currentCamera.transform.position - _sun.transform.forward * 1000f;
                        maxDistance = currentCamera.farClipPlane;
                        toLightVector = -_sun.transform.forward;
                    }
                    Vector3 sunScrPos = currentCamera.WorldToViewportPoint(sunWorldPosition, VRCheck.isVrRunning ? Camera.MonoOrStereoscopicEye.Left : Camera.MonoOrStereoscopicEye.Mono);
                    float flareIntensity = 0;
                    if (sunScrPos.z > 0 && sunScrPos.x >= -0.1f && sunScrPos.x < 1.1f && sunScrPos.y >= -0.1f && sunScrPos.y < 1.1f) {
                        Ray ray = new Ray(currentCamera.transform.position, toLightVector);
                        if (!Physics.Raycast(ray, maxDistance, _sunFlaresLayerMask)) {
                            Vector2 dd = sunScrPos - Vector3.one * 0.5f;
                            flareIntensity = _sunFlaresIntensity * Mathf.Clamp01((0.6f - Mathf.Max(Mathf.Abs(dd.x), Mathf.Abs(dd.y))) / 0.6f);
                        }
                    }
                    if (flareIntensity > sunFlareCurrentIntensity) {
                        sunFlareCurrentIntensity = Mathf.Lerp(sunFlareCurrentIntensity, flareIntensity, Application.isPlaying ? 30 * Time.unscaledDeltaTime * _sunFlaresRevealSpeed : 1f);
                    }
                    else {
                        sunFlareCurrentIntensity = Mathf.Lerp(sunFlareCurrentIntensity, flareIntensity, Application.isPlaying ? 30 * Time.unscaledDeltaTime * _sunFlaresHideSpeed : 1f);
                    }
                    if (sunFlareCurrentIntensity > 0) {
                        sunLastScrPos = sunScrPos;
                        bMat.SetColor(ShaderParams.SFSunTint, _sunFlaresTint * sunFlareCurrentIntensity);
                        sunLastScrPos.z = 0.5f + sunFlareTime * _sunFlaresSolarWindSpeed;
                        Vector2 sfDist = new Vector2(0.5f - sunLastScrPos.y, sunLastScrPos.x - 0.5f);
                        if (!_sunFlaresRotationDeadZone || sfDist.sqrMagnitude > 0.00025f) {
                            sunLastRot = Mathf.Atan2(sfDist.x, sfDist.y);
                        }
                        sunLastScrPos.w = sunLastRot;
                        sunFlareTime += Time.deltaTime;
                        bMat.SetVector(ShaderParams.SFSunPos, sunLastScrPos);

                        if (VRCheck.isVrRunning) {
                            Vector3 sunScrPosRightEye = currentCamera.WorldToViewportPoint(sunWorldPosition, Camera.MonoOrStereoscopicEye.Right);
                            bMat.SetVector(ShaderParams.SFSunPosRightEye, sunScrPosRightEye);
                        }

                        RenderTextureDescriptor rtSFDescriptor = rtDescBase;
                        rtSFDescriptor.width /= _sunFlaresDownsampling;
                        rtSFDescriptor.height /= _sunFlaresDownsampling;
                        rtSF = RenderTexture.GetTemporary(rtSFDescriptor);
                        int sfRenderPass;
                        if (_quality == BEAUTIFY_QUALITY.BestQuality) {
                            sfRenderPass = rtBloom != null ? 21 : 20;
                        }
                        else {
                            sfRenderPass = rtBloom != null ? 17 : 16;
                        }
                        bMat.SetTexture(ShaderParams.SFMainTexture, source);
                        Graphics.Blit(rtBloom != null ? rtBloom : source, rtSF, bMat, sfRenderPass);
                        if (_lensDirt) {
                            if (_bloom) {
#if !UNITY_2021_1_OR_NEWER
                                rt[3].MarkRestoreExpected();
#endif
                                Graphics.Blit(rtSF, rt[3], bMat, _quality == BEAUTIFY_QUALITY.BestQuality ? 11 : 13);
                            }
                        }
                        rtBloom = rtSF;
                        if (!_bloom && !_anamorphicFlares) { // ensure _Bloom.x is 1 into the shader for sun flares to be visible if no bloom nor anamorphic flares are enabled
                            bMat.SetVector(ShaderParams.Bloom, Vector4.one);
                            if (!bMat.IsKeywordEnabled(ShaderParams.SKW_BLOOM)) {
                                bMat.EnableKeyword(ShaderParams.SKW_BLOOM);
                            }
                        }
                    }
                }

                if (rtBloom != null) {
                    Shader.SetGlobalTexture(ShaderParams.BloomTexture, rtBloom);
                    //bMat.SetTexture(ShaderParams.BloomTexture, rtBloom);
                }
                else {
                    if (bMat.IsKeywordEnabled(ShaderParams.SKW_BLOOM)) {
                        bMat.DisableKeyword(ShaderParams.SKW_BLOOM); // required to avoid Metal issue
                    }
                    bMat.SetVector(ShaderParams.Bloom, Vector4.zero);
                }

                if (_lensDirt) {
                    //bMat.SetTexture(ShaderParams.ScreenLum, (_anamorphicFlares && !_bloom) ? rtAF[3] : rt[3]);
                    Shader.SetGlobalTexture(ShaderParams.ScreenLum, (_anamorphicFlares && !_bloom) ? rtAF[3] : rt[3]);
                }

            }

            if (_lensDirt) {
                Vector4 dirtData = new Vector4(1.0f, 1.0f / (1.01f - _lensDirtIntensity), _lensDirtThreshold, Mathf.Max(_bloomIntensity, 1f));
                bMat.SetVector(ShaderParams.Dirt, dirtData);
            }

            // tonemap + eye adaptation + purkinje
#if UNITY_EDITOR
            bool requiresLuminanceComputation = (Application.isPlaying || _eyeAdaptationInEditor) && allowExtraEffects && (_eyeAdaptation || _purkinje);
#else
            bool requiresLuminanceComputation = allowExtraEffects && (_eyeAdaptation || _purkinje);
#endif
            if (requiresLuminanceComputation) {
                int rtEACount = _quality == BEAUTIFY_QUALITY.BestQuality ? 9 : 8;
                int sizeEA = (int)Mathf.Pow(2, rtEACount);
                if (rtEA == null || rtEA.Length < rtEACount)
                    rtEA = new RenderTexture[rtEACount];
                RenderTextureDescriptor rtLumDescriptor = rtDescBase;
                for (int k = 0; k < rtEACount; k++) {
                    rtLumDescriptor.width = sizeEA;
                    rtLumDescriptor.height = sizeEA;
                    rtEA[k] = RenderTexture.GetTemporary(rtLumDescriptor);
                    sizeEA /= 2;
                }
                Graphics.Blit(source, rtEA[0], bMat, _quality == BEAUTIFY_QUALITY.BestQuality ? 22 : 18);
                int lumRT = rtEACount - 1;
                int basePass = _quality == BEAUTIFY_QUALITY.BestQuality ? 15 : 9;
                for (int k = 0; k < lumRT; k++) {
                    Graphics.Blit(rtEA[k], rtEA[k + 1], bMat, k == 0 ? basePass : basePass + 1);
                }
                bMat.SetTexture(ShaderParams.EALumSrc, rtEA[lumRT]);
                if (rtEAacum == null) {
                    int rawCopyPass = GetRawCopyPass();
                    RenderTextureDescriptor rtEASmallDesc = rtDescBase;
                    rtEASmallDesc.width = 2;
                    rtEASmallDesc.height = 2;
                    rtEAacum = new RenderTexture(rtEASmallDesc);
                    Graphics.Blit(rtEA[lumRT], rtEAacum, bMat, rawCopyPass);
                    rtEAHist = new RenderTexture(rtEASmallDesc);
                    Graphics.Blit(rtEAacum, rtEAHist, bMat, rawCopyPass);
                }
                else {
#if !UNITY_2021_1_OR_NEWER
                    rtEAacum.MarkRestoreExpected();
#endif
                    Graphics.Blit(rtEA[lumRT], rtEAacum, bMat, basePass + 2);
                    Graphics.Blit(rtEAacum, rtEAHist, bMat, basePass + 3);
                }
                bMat.SetTexture(ShaderParams.EAHist, rtEAHist);
            }

            // Separate Outline
            if (_outline && _outlineCustomize && _quality == BEAUTIFY_QUALITY.BestQuality && _outlineStage == BEAUTIFY_OUTLINE_STAGE.AfterBloom) {
                SeparateOutlinePass(source);
            }


            // Final Pass
            RenderTexture dest = destination;
            RenderTexture rtCA = null;
            if (rtBeauty != null) {
                Graphics.Blit(source, rtBeauty, bMat, 1);
                Shader.SetGlobalTexture(ShaderParams.CompareTexture, rtBeauty);
                // Add Chromatic Aberration pass if depth of field is also active
                if (_chromaticAberration && _depthOfField) {
                    rtCA = RenderTexture.GetTemporary(rtDescBase);
                    Graphics.Blit(rtBeauty, rtCA, bMat, _quality == BEAUTIFY_QUALITY.BestQuality ? 29 : 19);
                    RenderTexture.ReleaseTemporary(rtBeauty);
                    rtBeauty = rtCA;
                }
            }
            else {
                if (_chromaticAberration && _depthOfField) {
                    dest = RenderTexture.GetTemporary(rtDescBase);
                }
            }

            if (rtBlurTex != null) {
                float blurScale = _blurIntensity > 1f ? 1f : _blurIntensity;
                if (rtBeauty != null) {
                    Graphics.Blit(rtBeauty, rtBlurTex, bMat, renderPass);
                    BlurThis(rtBlurTex, blurScale);
                }
                else {
                    BlurThisDownscaling(source, rtBlurTex, blurScale);
                }
                BlurThis(rtBlurTex, blurScale);
                if (_quality == BEAUTIFY_QUALITY.BestQuality) {
                    BlurThis(rtBlurTex, blurScale);
                }
                if (rtBeauty != null) {
                    Shader.SetGlobalTexture(ShaderParams.CompareTexture, rtBlurTex);
                    Graphics.Blit(source, dest, bMat, renderPass);
                }
                else {
                    Graphics.Blit(rtBlurTex, dest, bMat, renderPass);
                }
                RenderTexture.ReleaseTemporary(rtBlurTex);
            }
            else {
                Graphics.Blit(source, dest, bMat, renderPass);
            }

            // Add Chromatic Aberration at the end
            if (dest != destination) {
                Graphics.Blit(dest, destination, bMat, _quality == BEAUTIFY_QUALITY.BestQuality ? 29 : 19);
                RenderTexture.ReleaseTemporary(dest);
            }

            // Release RTs used in final pass
            if (rtEA != null) {
                for (int k = 0; k < rtEA.Length; k++) {
                    if (rtEA[k] != null) {
                        RenderTexture.ReleaseTemporary(rtEA[k]);
                        rtEA[k] = null;
                    }
                }
            }
            if (rt != null) {
                for (int k = 0; k < rt.Length; k++) {
                    if (rt[k] != null) {
                        RenderTexture.ReleaseTemporary(rt[k]);
                        rt[k] = null;
                    }
                }
            }
            if (rtAF != null) {
                for (int k = 0; k < rtAF.Length; k++) {
                    if (rtAF[k] != null) {
                        RenderTexture.ReleaseTemporary(rtAF[k]);
                        rtAF[k] = null;
                    }
                }
            }
            if (rtCustomBloom != null) {
                RenderTexture.ReleaseTemporary(rtCustomBloom);
            }
            if (rtDoF != null) {
                RenderTexture.ReleaseTemporary(rtDoF);
            }
            if (rtBeauty != null) {
                RenderTexture.ReleaseTemporary(rtBeauty);
            }
            if (rtPixelated != null) {
                RenderTexture.ReleaseTemporary(rtPixelated);
            }
            if (rtSF != null) {
                RenderTexture.ReleaseTemporary(rtSF);
            }
            if (upscaleDownRT != null) {
                RenderTexture.ReleaseTemporary(upscaleDownRT);
            }
        }

        void EnsureSafeDimensions (ref RenderTextureDescriptor desc) {
            desc.width = Mathf.Clamp(desc.width, 1, 8192);
            desc.height = Mathf.Clamp(desc.height, 1, 8192);
        }

        void SeparateOutlinePass (RenderTexture source) {
            RenderTextureDescriptor rtOutlineDescriptor = rtDescBase;
            rtOutlineDescriptor.colorFormat = rtOutlineColorFormat;
            RenderTexture rtOutline = RenderTexture.GetTemporary(rtOutlineDescriptor);
            Graphics.Blit(source, rtOutline, bMat, 25);
            for (int k = 1; k <= _outlineBlurPassCount; k++) {
                BlurThisOutline(rtOutline, _outlineSpread, _outlineBlurDownscale ? k : 1);
            }
            Graphics.Blit(rtOutline, source, bMat, 28);
            RenderTexture.ReleaseTemporary(rtOutline);
        }

        void OnPostRender () {

            if (Camera.current.cameraType == CameraType.SceneView) return;

            if (renderScale != 1 && pixelateTexture != null) {
                currentCamera.targetTexture = null;
                RenderTexture.active = null;
            }
            else if (_pixelateDownscale && _pixelateAmount > 1 && pixelateTexture != null) {
                RenderTexture.active = null;
                currentCamera.targetTexture = null;
                pixelateTexture.filterMode = FilterMode.Point;
            }
        }


        void BlurThis (RenderTexture rt, float blurScale = 1f) {
            RenderTextureDescriptor desc = rt.descriptor;
            RenderTexture rt2 = RenderTexture.GetTemporary(desc);
            rt2.filterMode = FilterMode.Bilinear;
            bMat.SetFloat(ShaderParams.BlurScale, blurScale);
            Graphics.Blit(rt, rt2, bMat, 4);
#if !UNITY_STANDALONE && (!UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS || UNITY_WII || UNITY_PS4 || UNITY_XBOXONE || UNITY_TIZEN || UNITY_TVOS || UNITY_WSA || UNITY_WSA_10_0 || UNITY_WEBGL)
            rt.DiscardContents();
#endif
            Graphics.Blit(rt2, rt, bMat, 5);
            RenderTexture.ReleaseTemporary(rt2);
        }


        void BlurThisOutline (RenderTexture rt, float blurScale = 1f, int downscale = 1) {
            RenderTextureDescriptor desc = rt.descriptor;
            desc.colorFormat = rtOutlineColorFormat;
            desc.width = desc.width / downscale;
            desc.height = desc.height / downscale;
            RenderTexture rt2 = RenderTexture.GetTemporary(desc);
            rt2.filterMode = FilterMode.Bilinear;
            float ratio = rt.width / desc.width;
            bMat.SetFloat(ShaderParams.BlurScale, blurScale * ratio);
            Graphics.Blit(rt, rt2, bMat, 26);
            bMat.SetFloat(ShaderParams.BlurScale, blurScale);
#if !UNITY_STANDALONE && (!UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS || UNITY_WII || UNITY_PS4 || UNITY_XBOXONE || UNITY_TIZEN || UNITY_TVOS || UNITY_WSA || UNITY_WSA_10_0 || UNITY_WEBGL)
            rt.DiscardContents();
#endif
            Graphics.Blit(rt2, rt, bMat, 27);
            RenderTexture.ReleaseTemporary(rt2);
        }

        void BlurThisDownscaling (RenderTexture rt, RenderTexture downscaled, float blurScale = 1f) {
            RenderTextureDescriptor desc = rt.descriptor;
            desc.width = downscaled.width;
            desc.height = downscaled.height;
            RenderTexture rt2 = RenderTexture.GetTemporary(desc);
            rt2.filterMode = FilterMode.Bilinear;
            float ratio = rt.width / desc.width;
            bMat.SetFloat(ShaderParams.BlurScale, blurScale * ratio);
            Graphics.Blit(rt, rt2, bMat, 4);
            bMat.SetFloat(ShaderParams.BlurScale, blurScale);
#if !UNITY_STANDALONE && (!UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS || UNITY_WII || UNITY_PS4 || UNITY_XBOXONE || UNITY_TIZEN || UNITY_TVOS || UNITY_WSA || UNITY_WSA_10_0 || UNITY_WEBGL)
            downscaled.DiscardContents();
#endif
            Graphics.Blit(rt2, downscaled, bMat, 5);
            RenderTexture.ReleaseTemporary(rt2);
        }

        RenderTexture BlurThisOneDirection (RenderTexture rt, bool vertical, float blurScale = 1f) {
            RenderTextureDescriptor desc = rt.descriptor;
            RenderTexture rt2 = RenderTexture.GetTemporary(desc);
            rt2.filterMode = FilterMode.Bilinear;
            bMat.SetFloat(ShaderParams.BlurScale, blurScale);
            Graphics.Blit(rt, rt2, bMat, vertical ? 5 : 4);
            RenderTexture.ReleaseTemporary(rt);
            return rt2;
        }

        void BlurThisDoF (RenderTexture rt, int renderPass) {
            RenderTextureDescriptor desc = rt.descriptor;
            RenderTexture rt2 = RenderTexture.GetTemporary(desc);
            RenderTexture rt3 = RenderTexture.GetTemporary(desc);
            rt2.filterMode = _depthOfFieldFilterMode;
            rt3.filterMode = _depthOfFieldFilterMode;
            UpdateDepthOfFieldBlurData(new Vector2(0.44721f, -0.89443f));
            Graphics.Blit(rt, rt2, bMat, renderPass);
            UpdateDepthOfFieldBlurData(new Vector2(-1f, 0f));
            Graphics.Blit(rt2, rt3, bMat, renderPass);
            UpdateDepthOfFieldBlurData(new Vector2(0.44721f, 0.89443f));
#if !UNITY_STANDALONE && (!UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS || UNITY_WII || UNITY_PS4 || UNITY_XBOXONE || UNITY_TIZEN || UNITY_TVOS || UNITY_WSA || UNITY_WSA_10_0 || UNITY_WEBGL)
            rt.DiscardContents();
#endif
            Graphics.Blit(rt3, rt, bMat, renderPass);
            RenderTexture.ReleaseTemporary(rt3);
            RenderTexture.ReleaseTemporary(rt2);
        }


        void BlurThisAlpha (RenderTexture rt, float blurScale = 1f) {
            RenderTextureDescriptor desc = rt.descriptor;
            RenderTexture rt2 = RenderTexture.GetTemporary(desc);
            rt2.filterMode = FilterMode.Bilinear;
            bMat.SetFloat(ShaderParams.BlurScale, blurScale);
            Graphics.Blit(rt, rt2, bMat, 23);
#if !UNITY_STANDALONE && (!UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS || UNITY_WII || UNITY_PS4 || UNITY_XBOXONE || UNITY_TIZEN || UNITY_TVOS || UNITY_WSA || UNITY_WSA_10_0 || UNITY_WEBGL)
            rt.DiscardContents();
#endif
            Graphics.Blit(rt2, rt, bMat, 24);
            RenderTexture.ReleaseTemporary(rt2);
        }

        #endregion

        #region Settings stuff


        public void OnDidApplyAnimationProperties () {   // support for animating property based fields
            shouldUpdateMaterialProperties = true;
        }

        public void UpdateQualitySettings () {
            switch (_quality) {
                case BEAUTIFY_QUALITY.BestPerformance:
                    _depthOfFieldDownsampling = 2;
                    _depthOfFieldMaxSamples = 4;
                    _sunFlaresDownsampling = 2;
                    break;
                case BEAUTIFY_QUALITY.BestQuality:
                    _depthOfFieldDownsampling = 1;
                    _depthOfFieldMaxSamples = 8;
                    _sunFlaresDownsampling = 1;
                    break;
                case BEAUTIFY_QUALITY.Basic:
                    break;
            }
            isDirty = true;
        }

        public void UpdateMaterialProperties () {
            if (Application.isPlaying) {
                shouldUpdateMaterialProperties = true;
            }
            else {
                UpdateMaterialPropertiesNow();
            }
        }

        void CheckColorSpace () {
            linearColorSpace = QualitySettings.activeColorSpace == ColorSpace.Linear;
        }

        public void UpdateMaterialPropertiesNow () {
            shouldUpdateMaterialProperties = false;
            CheckColorSpace();

            // Checks camera depth texture mode
#if USE_CAMERA_DEPTH_TEXTURE
            if (currentCamera != null && currentCamera.depthTextureMode == DepthTextureMode.None && _quality != BEAUTIFY_QUALITY.Basic) {
                currentCamera.depthTextureMode = DepthTextureMode.Depth;
            }
#endif

            string gpu = SystemInfo.graphicsDeviceName;
            if (gpu != null && gpu.ToUpper().Contains("MALI-T720")) {
                rtFormat = RenderTextureFormat.Default;
                _bloomBlur = false; // avoid artifacting due to low precision textures
                _anamorphicFlaresBlur = false;
            }
            else {
                rtFormat = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf) ? RenderTextureFormat.ARGBHalf : RenderTextureFormat.ARGB32;
            }

            switch (_quality) {
                case BEAUTIFY_QUALITY.BestQuality:
                    if (bMatDesktop == null) {
                        bMatDesktop = new Material(Shader.Find("Hidden/Kronnect/Beautify/Beautify"));
                        bMatDesktop.hideFlags = HideFlags.DontSave;
                    }
                    bMat = bMatDesktop;
                    break;
                case BEAUTIFY_QUALITY.BestPerformance:
                    if (bMatMobile == null) {
                        bMatMobile = new Material(Shader.Find("Hidden/Kronnect/Beautify/BeautifyMobile"));
                        bMatMobile.hideFlags = HideFlags.DontSave;
                    }
                    bMat = bMatMobile;
                    break;
                case BEAUTIFY_QUALITY.Basic:
                    if (bMatBasic == null) {
                        bMatBasic = new Material(Shader.Find("Hidden/Kronnect/Beautify/BeautifyBasic"));
                        bMatBasic.hideFlags = HideFlags.DontSave;
                    }
                    bMat = bMatBasic;
                    break;
            }

            switch (_preset) {
                case BEAUTIFY_PRESET.Soft:
                    _sharpen = 2.0f;
                    if (linearColorSpace)
                        _sharpen *= 2f;
                    _sharpenDepthThreshold = 0.035f;
                    _sharpenRelaxation = 0.065f;
                    _sharpenClamp = 0.4f;
                    _saturate = 0.5f;
                    _contrast = 1.005f;
                    _brightness = 1.05f;
                    _dither = 0.02f;
                    _ditherDepth = 0;
                    _daltonize = 0;
                    break;
                case BEAUTIFY_PRESET.Medium:
                    _sharpen = 3f;
                    if (linearColorSpace)
                        _sharpen *= 2f;
                    _sharpenDepthThreshold = 0.035f;
                    _sharpenRelaxation = 0.07f;
                    _sharpenClamp = 0.45f;
                    _saturate = 1.0f;
                    _contrast = 1.02f;
                    _brightness = 1.05f;
                    _dither = 0.02f;
                    _ditherDepth = 0;
                    _daltonize = 0;
                    break;
                case BEAUTIFY_PRESET.Strong:
                    _sharpen = 4.75f;
                    if (linearColorSpace)
                        _sharpen *= 2f;
                    _sharpenDepthThreshold = 0.035f;
                    _sharpenRelaxation = 0.075f;
                    _sharpenClamp = 0.5f;
                    _saturate = 1.5f;
                    _contrast = 1.03f;
                    _brightness = 1.05f;
                    _dither = 0.022f;
                    _ditherDepth = 0;
                    _daltonize = 0;
                    break;
                case BEAUTIFY_PRESET.Exaggerated:
                    _sharpen = 6f;
                    if (linearColorSpace)
                        _sharpen *= 2f;
                    _sharpenDepthThreshold = 0.035f;
                    _sharpenRelaxation = 0.08f;
                    _sharpenClamp = 0.55f;
                    _saturate = 2.25f;
                    _contrast = 1.035f;
                    _brightness = 1.05f;
                    _dither = 0.025f;
                    _ditherDepth = 0;
                    _daltonize = 0;
                    break;
            }
            isDirty = true;

            if (bMat == null)
                return;
            renderPass = 1;
            if (_pixelateAmount > 1) {
                if (QualitySettings.antiAliasing > 1) {
                    QualitySettings.antiAliasing = 1;
                }
                if (_pixelateDownscale) {
                    _dither = 0;
                }
            }

            if (shaderKeywords == null) {
                shaderKeywords = new List<string>();
            }
            else {
                shaderKeywords.Clear();
            }

            // sharpen settings
            UpdateSharpenParams(_sharpen);

            // dither settings
            bool isOrtho = (currentCamera != null && currentCamera.orthographic);
            bMat.SetVector(ShaderParams.Dither, new Vector4(_dither, isOrtho ? 0 : _ditherDepth, (_sharpenMaxDepth + _sharpenMinDepth) * 0.5f, Mathf.Abs(_sharpenMaxDepth - _sharpenMinDepth) * 0.5f + (isOrtho ? 1000.0f : 0f)));

            // AA
            bMat.SetVector(ShaderParams.AntialiasData, new Vector4(_antialiasStrength, _antialiasDepthThreshold, _antialiasDepthAtten * 10f, _antialiasMaxSpread));

            // color grading settings
            float cont = linearColorSpace ? 1.0f + (_contrast - 1.0f) / 2.2f : _contrast;
            bMat.SetVector(ShaderParams.ColorBoost, new Vector4(_brightness, cont, _saturate, _daltonize * 10f));
            bMat.SetVector(ShaderParams.HardLight, new Vector3(_hardLightBlend, _hardLightIntensity));
            bMat.SetVector(ShaderParams.ColorBoost2, new Vector4(_tonemapExposurePre, _tonemapBrightnessPost, 0, 0));

            // vignetting FX
            Color vignettingColorAdjusted = _vignettingColor;
            vignettingColorAdjusted.a *= _vignetting ? 32f : 0f;
            float vb = 1f - _vignettingBlink * 2f;
            if (vb < 0) {
                vb = 0;
            }
            vignettingColorAdjusted.r *= vb;
            vignettingColorAdjusted.g *= vb;
            vignettingColorAdjusted.b *= vb;
            bMat.SetColor(ShaderParams.Vignette, vignettingColorAdjusted);
            if (currentCamera != null) {
                bMat.SetFloat(ShaderParams.VignetteAspectRatio, (_vignettingCircularShape && _vignettingBlink <= 0) ? 1.0f / currentCamera.aspect : _vignettingAspectRatio + 1.001f / (1.001f - _vignettingBlink) - 1f);
            }

            // additional data
            float vignettingCenterY = _vignettingCenter.y;
            if (_vignettingBlinkStyle == BEAUTIFY_BLINK_STYLE.Human) {
                vignettingCenterY -= _vignettingBlink * 0.5f;
            }
            bMat.SetVector(ShaderParams.FXData, new Vector4(_vignettingCenter.x, vignettingCenterY, _sharpenMinMaxDepthFallOff));

            // frame FX
            if (_frame) {
                if (_frameMask != null) {
                    bMat.SetTexture(ShaderParams.FrameMaskTexture, _frameMask);
                    shaderKeywords.Add(ShaderParams.SKW_FRAME_MASK);
                }
                else {
                    shaderKeywords.Add(ShaderParams.SKW_FRAME);
                }
                if (_frameStyle == FrameStyle.Border) {
                    Vector4 frameColorAdjusted = new Vector4(_frameColor.r, _frameColor.g, _frameColor.b, _frameColor.a);
                    bMat.SetVector(ShaderParams.Frame, frameColorAdjusted);
                    float fparam = _frameMask != null ? _frameColor.a : (1.00001f - _frameColor.a) * 0.5f;
                    bMat.SetVector(ShaderParams.FrameData, new Vector4(fparam, 50, fparam, 50));
                }
                else {
                    bMat.SetVector(ShaderParams.Frame, Color.black);
                    bMat.SetVector(ShaderParams.FrameData, new Vector4(0.5f - _frameBandHorizontalSize, 1f / (0.0001f + _frameBandHorizontalSmoothness), 0.5f - _frameBandVerticalSize, 1f / (0.0001f + _frameBandVerticalSmoothness)));
                }
            }

            // outline FX
            bMat.SetFloat(ShaderParams.OutlineIntensityMultiplier, _outlineIntensityMultiplier);
            bMat.SetFloat(ShaderParams.OutlineMinDepthThreshold, _outlineMinDepthThreshold);
            bMat.SetColor(ShaderParams.OutlineColor, _outlineColor);

            // bloom
            float bloomWeightsSum = 0.00001f + _bloomWeight0 + _bloomWeight1 + _bloomWeight2 + _bloomWeight3 + _bloomWeight4 + _bloomWeight5;
            bMat.SetVector(ShaderParams.BloomWeights2, new Vector4(_bloomWeight4 / bloomWeightsSum + _bloomBoost4, _bloomWeight5 / bloomWeightsSum + _bloomBoost5, _bloomMaxBrightness, bloomWeightsSum));
            if (_bloomCustomize) {
                bMat.SetVector(ShaderParams.BloomWeights, new Vector4(_bloomWeight0 / bloomWeightsSum + _bloomBoost0, _bloomWeight1 / bloomWeightsSum + _bloomBoost1, _bloomWeight2 / bloomWeightsSum + _bloomBoost2, _bloomWeight3 / bloomWeightsSum + _bloomBoost3));
                bMat.SetColor(ShaderParams.BloomTint0, _bloomTint0);
                bMat.SetColor(ShaderParams.BloomTint1, _bloomTint1);
                bMat.SetColor(ShaderParams.BloomTint2, _bloomTint2);
                bMat.SetColor(ShaderParams.BloomTint3, _bloomTint3);
                bMat.SetColor(ShaderParams.BloomTint4, _bloomTint4);
                bMat.SetColor(ShaderParams.BloomTint5, _bloomTint5);
            }

            if (_bloomDebug && (_bloom || _anamorphicFlares || _sunFlares)) {
                renderPass = 3;
            }
            bloomCurrentLayerMaskValue = _bloomCullingMask;
            anamorphicFlaresCurrentLayerMaskValue = _anamorphicFlaresCullingMask;

            // sun flares
            if (_sunFlares) {
                _sunFlaresRevealSpeed = Mathf.Max(0.001f, _sunFlaresRevealSpeed);
                _sunFlaresHideSpeed = Mathf.Max(0.001f, _sunFlaresHideSpeed);
                bMat.SetVector(ShaderParams.SFSunData, new Vector4(_sunFlaresSunIntensity, _sunFlaresSunDiskSize, _sunFlaresSunRayDiffractionIntensity, _sunFlaresSunRayDiffractionThreshold));
                bMat.SetVector(ShaderParams.SFCoronaRays1, new Vector4(_sunFlaresCoronaRays1Length, Mathf.Max(_sunFlaresCoronaRays1Streaks / 2f, 1), Mathf.Max(_sunFlaresCoronaRays1Spread, 0.0001f), _sunFlaresCoronaRays1AngleOffset));
                bMat.SetVector(ShaderParams.SFCoronaRays2, new Vector4(_sunFlaresCoronaRays2Length, Mathf.Max(_sunFlaresCoronaRays2Streaks / 2f, 1), Mathf.Max(_sunFlaresCoronaRays2Spread + 0.0001f), _sunFlaresCoronaRays2AngleOffset));
                bMat.SetVector(ShaderParams.SFGhosts1, new Vector4(0, _sunFlaresGhosts1Size, _sunFlaresGhosts1Offset, _sunFlaresGhosts1Brightness));
                bMat.SetVector(ShaderParams.SFGhosts2, new Vector4(0, _sunFlaresGhosts2Size, _sunFlaresGhosts2Offset, _sunFlaresGhosts2Brightness));
                bMat.SetVector(ShaderParams.SFGhosts3, new Vector4(0, _sunFlaresGhosts3Size, _sunFlaresGhosts3Offset, _sunFlaresGhosts3Brightness));
                bMat.SetVector(ShaderParams.SFGhosts4, new Vector4(0, _sunFlaresGhosts4Size, _sunFlaresGhosts4Offset, _sunFlaresGhosts4Brightness));
                bMat.SetVector(ShaderParams.SFHalo, new Vector4(_sunFlaresHaloOffset, _sunFlaresHaloAmplitude, _sunFlaresHaloIntensity * 100f, _sunFlaresRadialOffset));
            }

            // lens dirt
            if (_lensDirtTexture == null) {
                _lensDirtTexture = Resources.Load<Texture2D>("Textures/dirt2") as Texture2D;
            }
            bMat.SetTexture(ShaderParams.OverlayTexture, _lensDirtTexture);

            // anamorphic flares
            bMat.SetColor(ShaderParams.AFTint, _anamorphicFlaresTint);

            // dof
            if (_depthOfField) {
                if (_depthOfFieldAutofocusLayerMask != 0) {
                    Shader.SetGlobalFloat(ShaderParams.DoFDepthBias, _depthOfFieldExclusionBias);
                }
                if (_depthOfFieldExclusionLayerMask != 0) {
                    Shader.SetGlobalInt(ShaderParams.DoFExclusionCullMode, (int)_depthOfFieldExclusionCullMode);
                }
                if (_depthOfFieldTransparencySupport) {
                    Shader.SetGlobalInt(ShaderParams.DoFTransparencyCullMode, (int)_depthOfFieldTransparencyCullMode);
                }
            }
            dofCurrentLayerMaskValue = _depthOfFieldExclusionLayerMask.value;

            // final config
            if (_compareMode) {
                renderPass = 0;
                float angle, panningValue;
                switch (_compareStyle) {
                    case BEAUTIFY_COMPARE_STYLE.FreeAngle:
                        angle = _compareLineAngle;
                        panningValue = -10;
                        break;
                    case BEAUTIFY_COMPARE_STYLE.SameSide:
                        angle = Mathf.PI * 0.5f;
                        panningValue = _comparePanning;
                        break;
                    default:
                        angle = Mathf.PI * 0.5f;
                        panningValue = -20f + _comparePanning * 2f;
                        break;
                }

                bMat.SetVector(ShaderParams.CompareData, new Vector4(Mathf.Cos(angle), Mathf.Sin(angle), panningValue, _compareLineWidth));
            }

            if (_quality != BEAUTIFY_QUALITY.Basic) {
                if (_lut && (_lutTexture != null || _lutTexture3D != null)) {
                    if (_lutTexture != null) {
                        shaderKeywords.Add(ShaderParams.SKW_LUT);
                        bMat.SetTexture(ShaderParams.LUT, _lutTexture);
                    }
                    else {
                        shaderKeywords.Add(ShaderParams.SKW_LUT3D);
                        bMat.SetTexture(ShaderParams.LUT3D, _lutTexture3D);
                        float x = 1f / _lutTexture3D.width;
                        float y = _lutTexture3D.width - 1f;
                        bMat.SetVector(ShaderParams.LUT3DParams, new Vector4(x * 0.5f, x * y, 0, 0));
                    }
                    bMat.SetColor(ShaderParams.FXColor, new Color(0, 0, 0, _lutIntensity));
                }
                else if (_nightVision) {
                    shaderKeywords.Add(ShaderParams.SKW_NIGHT_VISION);
                    Color nightVisionAdjusted = _nightVisionColor;
                    if (linearColorSpace) {
                        nightVisionAdjusted.a *= 5.0f * nightVisionAdjusted.a;
                    }
                    else {
                        nightVisionAdjusted.a *= 3.0f * nightVisionAdjusted.a;
                    }
                    nightVisionAdjusted.r *= nightVisionAdjusted.a;
                    nightVisionAdjusted.g *= nightVisionAdjusted.a;
                    nightVisionAdjusted.b *= nightVisionAdjusted.a;
                    bMat.SetColor(ShaderParams.FXColor, nightVisionAdjusted);
                }
                else if (_thermalVision) {
                    shaderKeywords.Add(ShaderParams.SKW_THERMAL_VISION);
                }
                else if (_daltonize > 0) {
                    shaderKeywords.Add(ShaderParams.SKW_DALTONIZE);
                }
                else { // set _FXColor for procedural sepia
                    bMat.SetColor(ShaderParams.FXColor, new Color(0, 0, 0, _lutIntensity));
                }
                bMat.SetColor(ShaderParams.TintColor, _tintColor);
                if (_sunFlares) {
                    if (flareNoise == null) {
                        flareNoise = Resources.Load<Texture2D>("Textures/flareNoise");
                    }
                    flareNoise.wrapMode = TextureWrapMode.Repeat;
                    bMat.SetTexture(ShaderParams.FlareTexture, flareNoise);
                    if (_sun == null) {
                        Light[] lights = FindObjectsOfType<Light>();
                        for (int k = 0; k < lights.Length; k++) {
                            Light light = lights[k];
                            if (light.type == LightType.Directional && light.enabled && light.gameObject.activeSelf) {
                                _sun = light.transform;
                                break;
                            }
                        }
                    }
                    if (_sun != null) {
                        Light light = _sun.GetComponentInChildren<Light>();
                        sunIsSpotlight = light.type == LightType.Spot;
                    }
                }

                if (_vignetting) {
                    if (_vignettingMask != null) {
                        bMat.SetTexture(ShaderParams.VignetteMaskTexture, _vignettingMask);
                        shaderKeywords.Add(ShaderParams.SKW_VIGNETTING_MASK);
                    }
                    else {
                        shaderKeywords.Add(ShaderParams.SKW_VIGNETTING);
                    }
                }

                if (_outline && !_outlineCustomize)
                    shaderKeywords.Add(ShaderParams.SKW_OUTLINE);
                if (_lensDirt)
                    shaderKeywords.Add(ShaderParams.SKW_DIRT);
                if (_bloom || _anamorphicFlares || _sunFlares) {
                    shaderKeywords.Add(ShaderParams.SKW_BLOOM);
                    if (_bloomDepthAtten > 0 || _bloomNearAtten > 0) {
                        bMat.SetFloat(ShaderParams.BloomDepthThreshold, _bloomDepthAtten);
                        bMat.SetFloat(ShaderParams.BloomDepthNearThreshold, _bloomNearAtten);
                        shaderKeywords.Add(ShaderParams.SKW_BLOOM_USE_DEPTH);
                    }
                    if ((_bloom && isUsingBloomLayerMask) || (_anamorphicFlares && isUsingAnamorphicFlaresLayerMask)) {
                        bMat.SetFloat(ShaderParams.BloomZDepthBias, _bloomLayerZBias);
                    }
                    if (_bloom && _bloomConservativeThreshold) {
                        shaderKeywords.Add(ShaderParams.SKW_BLOOM_CONSERVATIVE_THRESHOLD);
                    }
                }
                if (_depthOfField) {
                    if (_depthOfFieldTransparencySupport || isUsingDepthOfFieldExclusionLayerMask) {
                        shaderKeywords.Add(ShaderParams.SKW_DEPTH_OF_FIELD_TRANSPARENT);
                    }
                    else {
                        shaderKeywords.Add(ShaderParams.SKW_DEPTH_OF_FIELD);
                    }
                }
                if (_eyeAdaptation) {
                    Vector4 eaData = new Vector4(_eyeAdaptationMinExposure, _eyeAdaptationMaxExposure, _eyeAdaptationSpeedToDark, _eyeAdaptationSpeedToLight);
                    bMat.SetVector(ShaderParams.EyeAdaptation, eaData);
                    shaderKeywords.Add(ShaderParams.SKW_EYE_ADAPTATION);
#if UNITY_EDITOR
                    if (!Application.isPlaying && !_eyeAdaptationInEditor) {
                        bMat.SetTexture(ShaderParams.EALumSrc, Texture2D.whiteTexture);
                        bMat.SetTexture(ShaderParams.EAHist, Texture2D.whiteTexture);
                    }
#endif
                }
                if (_tonemap == BEAUTIFY_TMO.ACES) {
                    shaderKeywords.Add(ShaderParams.SKW_TONEMAP_ACES);
                }
                else if (_tonemap == BEAUTIFY_TMO.AGX) {
                    bMat.SetFloat(ShaderParams.TonemapGamma, linearColorSpace ? _tonemapGamma : _tonemapGamma * 0.5f);
                    shaderKeywords.Add(ShaderParams.SKW_TONEMAP_AGX);
                }
                if (_purkinje || _vignetting) {
                    float vd = _vignettingFade + _vignettingBlink * 0.5f;
                    if (_vignettingBlink > 0.99f)
                        vd = 1f;
                    Vector3 purkinjeData = new Vector3(_purkinjeAmount, _purkinjeLuminanceThreshold, vd);
                    bMat.SetVector(ShaderParams.Purkinje, purkinjeData);
                    shaderKeywords.Add(ShaderParams.SKW_PURKINJE);
                }

                // chromatic aberration
                if (_chromaticAberration) {
                    bMat.SetVector(ShaderParams.ChromaticAberration, new Vector4(_chromaticAberrationIntensity, _chromaticAberrationSmoothing, 0, 0));
                    if (!_depthOfField) {
                        shaderKeywords.Add(ShaderParams.SKW_CHROMATIC_ABERRATION);
                    }
                }

            }
#if UNITY_2021_3_OR_NEWER
            bMat.enabledKeywords = null;
#endif
            bMat.shaderKeywords = shaderKeywords.ToArray();

#if DEBUG_BEAUTIFY
												Debug.Log("*** DEBUG: Updating material properties...");
												Debug.Log("Linear color space: " + linearColorSpace.ToString());
												Debug.Log("Preset: " + _preset.ToString());
												Debug.Log("Sharpen: " + _sharpen.ToString());
												Debug.Log("Dither: " + _dither.ToString());
												Debug.Log("Contrast: " + cont.ToString());
												Debug.Log("Bloom: " + _bloom.ToString());
												Debug.Log("Bloom Intensity: " + _bloomIntensity.ToString());
												Debug.Log("Bloom Threshold: " + bloomThreshold.ToString());
												Debug.Log("Bloom Weight: " + bloomWeightsSum.ToString());
#endif
        }

        void UpdateMaterialBloomIntensityAndThreshold () {
            float threshold = _bloomThreshold;
            if (linearColorSpace) {
                threshold *= threshold;
            }
            bMat.SetVector(ShaderParams.Bloom, new Vector4(_bloomIntensity + (_anamorphicFlares ? 0.0001f : 0f), _bloomAntiflickerMaxOutput, 0, threshold));
            if (isUsingBloomLayerMask) {
                bMat.EnableKeyword(ShaderParams.SKW_BLOOM_USE_LAYER);
            }
            else {
                bMat.DisableKeyword(ShaderParams.SKW_BLOOM_USE_LAYER);
            }

        }

        void UpdateMaterialAnamorphicIntensityAndThreshold () {
            float threshold = _anamorphicFlaresThreshold;
            if (linearColorSpace) {
                threshold *= threshold;
            }
            float intensity = _anamorphicFlaresIntensity / (_bloomIntensity + 0.0001f);
            bMat.SetVector(ShaderParams.Bloom, new Vector4(intensity, _anamorphicFlaresAntiflickerMaxOutput, 0, threshold));
            if (_anamorphicFlares && isUsingAnamorphicFlaresLayerMask) {
                bMat.EnableKeyword(ShaderParams.SKW_BLOOM_USE_LAYER);
            }
            else {
                bMat.DisableKeyword(ShaderParams.SKW_BLOOM_USE_LAYER);
            }
        }

        void UpdateSharpenParams (float sharpen) {
            bMat.SetVector(ShaderParams.Sharpen, new Vector4(sharpen, _sharpenDepthThreshold, _sharpenClamp, _sharpenRelaxation));
        }

        void UpdateDepthOfFieldData () {
            // TODO: get focal length from camera FOV: FOV = 2 arctan (x/2f) x = diagonal of film (0.024mm)
            float d;
            if (_depthOfFieldAutofocus) {
                UpdateDoFAutofocusDistance();
                d = dofLastAutofocusDistance > 0 ? dofLastAutofocusDistance : currentCamera.farClipPlane;
            }
            else if (_depthOfFieldTargetFocus != null) {
                Vector3 spos = currentCamera.WorldToScreenPoint(_depthOfFieldTargetFocus.position);
                if (spos.z < 0) {
                    d = currentCamera.farClipPlane;
                }
                else {
                    d = Vector3.Distance(currentCamera.transform.position, _depthOfFieldTargetFocus.position);
                }
            }
            else {
                d = _depthOfFieldDistance;
            }
            if (OnBeforeFocus != null) {
                d = OnBeforeFocus(d);
            }
            if (dofPrevDistance < 0) {
                dofPrevDistance = d;
            }
            else {
                dofPrevDistance = Mathf.Lerp(dofPrevDistance, d, Application.isPlaying ? _depthOfFieldFocusSpeed * Time.unscaledDeltaTime * 30f : 1f);
            }
            float dofCoc;
            if (depthOfFieldCameraSettings == BEAUTIFY_DOF_CAMERA_SETTINGS.Real) {
                float focalLength = depthOfFieldFocalLengthReal;
                float aperture = focalLength / depthOfFieldFStop;
                dofCoc = aperture * (focalLength / Mathf.Max(dofPrevDistance * 1000f - focalLength, 0.001f)) * (1f / depthOfFieldImageSensorHeight) * currentCamera.pixelHeight;
            }
            else {
                dofCoc = _depthOfFieldAperture * (_depthOfFieldFocalLength / Mathf.Max(dofPrevDistance - _depthOfFieldFocalLength, 0.001f)) * (1f / 0.024f);
            }
            dofLastBokehData = new Vector4(dofPrevDistance, dofCoc, 0, 0);
            bMat.SetVector(ShaderParams.BokehData, dofLastBokehData);
            float bokehThreshold = _depthOfFieldBokehThreshold;
            if (!linearColorSpace) {
                bokehThreshold = Mathf.LinearToGammaSpace(bokehThreshold);
            }
            bMat.SetVector(ShaderParams.BokehData2, new Vector4(_depthOfFieldForegroundBlur ? _depthOfFieldForegroundDistance : currentCamera.farClipPlane, _depthOfFieldMaxSamples, bokehThreshold, _depthOfFieldBokehIntensity * _depthOfFieldBokehIntensity));
            bMat.SetVector(ShaderParams.BokehData3, new Vector3(_depthOfFieldMaxBrightness, _depthOfFieldMaxDistance * (currentCamera.farClipPlane + 1f), 0));
        }

        void UpdateDepthOfFieldBlurData (Vector2 blurDir) {
            float downsamplingRatio = 1f / (float)_depthOfFieldDownsampling;
            blurDir *= downsamplingRatio;
            dofLastBokehData.z = blurDir.x;
            dofLastBokehData.w = blurDir.y;
            bMat.SetVector(ShaderParams.BokehData, dofLastBokehData);
        }

        void UpdateDoFAutofocusDistance () {
            Vector3 p = _depthofFieldAutofocusViewportPoint;
            p.z = 10f;
            Ray r = currentCamera.ViewportPointToRay(p);
            RaycastHit hit;
            if (Physics.Raycast(r, out hit, currentCamera.farClipPlane, _depthOfFieldAutofocusLayerMask)) {
                // we don't use hit.distance as ray origin has a small shift from camera
                float distance = Vector3.Distance(currentCamera.transform.position, hit.point);
                distance += _depthOfFieldAutofocusDistanceShift;
                dofLastAutofocusDistance = Mathf.Clamp(distance, _depthOfFieldAutofocusMinDistance, _depthOfFieldAutofocusMaxDistance);
            }
            else {
                dofLastAutofocusDistance = currentCamera.farClipPlane;
            }
        }

        #endregion

        #region API

        /// <summary>
        /// Animates blink parameter
        /// </summary>
        /// <returns>The blink.</returns>
        /// <param name="duration">Duration.</param>
        public void Blink (float duration, float maxValue = 1) {
            if (duration <= 0)
                return;
            StartCoroutine(DoBlink(duration, maxValue));
        }

        IEnumerator DoBlink (float duration, float maxValue) {

            float start = Time.time;
            float t = 0;
            WaitForEndOfFrame w = new WaitForEndOfFrame();

            // Close
            do {
                t = (Time.time - start) / duration;
                if (t > 1f)
                    t = 1f;
                float easeOut = t * (2f - t);
                vignettingBlink = easeOut * maxValue;
                yield return w;
            } while (t < 1f);

            // Open
            start = Time.time;
            do {
                t = (Time.time - start) / duration;
                if (t > 1f)
                    t = 1f;
                float easeIn = t * t;
                vignettingBlink = (1f - easeIn) * maxValue;
                yield return w;
            } while (t < 1f);
        }


        public float depthOfFieldCurrentFocalPointDistance {
            get { return dofLastAutofocusDistance; }
        }

        #endregion



    }

}
