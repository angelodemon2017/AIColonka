using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;
using Zenject;
using System;

public class PostProcessHandler : MonoBehaviour
{
    [Inject] private SignalBus _signalBus;
    [SerializeField] private Volume volume;
    [SerializeField] private Color _healEffect;

    private UnityEngine.Rendering.Universal.Vignette _vignette;
    private UnityEngine.Rendering.Universal.LensDistortion _lensDistortion;
    private Beautify.Universal.Beautify _beautify;

    private void Awake()
    {
        if (volume.profile.TryGet(out _vignette))
        {
            _vignette.intensity.Override(0);
        }
        if (volume.profile.TryGet(out _lensDistortion))
        {
            _lensDistortion.intensity.Override(0);
        }
        if (volume.profile.TryGet(out _beautify))
        {
            SetDownsampling(1);
            SetFinalBlur(0);
        }
    }

    private void OnEnable()
    {
        _signalBus.Subscribe<EnterToSceneSignal>(EnterToScene);
        _signalBus.Subscribe<ExitFromSceneSignal>(ExitFromScene);
        _signalBus.Subscribe<PlayerDamageSignal>(GetDamage);
        _signalBus.Subscribe<PlayerDashSignal>(DashEffect);
        _signalBus.Subscribe<PlayerHealSignal>(HealEffect);
        _signalBus.Subscribe<TransitionSignal>(TransitionForAct);
    }

private void OnDisable()
    {
        _signalBus.Unsubscribe<EnterToSceneSignal>(EnterToScene);
        _signalBus.Unsubscribe<ExitFromSceneSignal>(ExitFromScene);
        _signalBus.Unsubscribe<PlayerDamageSignal>(GetDamage);
        _signalBus.Unsubscribe<PlayerDashSignal>(DashEffect);
        _signalBus.Unsubscribe<PlayerHealSignal>(HealEffect);
        _signalBus.Unsubscribe<TransitionSignal>(TransitionForAct);
    }

    private void GetDamage()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(DOTween.To(() => 0f, x => SceneTransition(x), 0.5f, 0.1f));
        sequence.Append(DOTween.To(() => 0.5f, x => SceneTransition(x), 0f, 1.5f));

        Sequence sequence2 = DOTween.Sequence();
        sequence2.Append(DOTween.To(() => Color.white, x => _beautify?.tintColor.Override(x), Color.red, 0.15f));
        sequence2.Append(DOTween.To(() => Color.red, x => _beautify?.tintColor.Override(x), Color.white, 2.2f));
    }

    [ContextMenu("HealEffect")]
    private void HealEffect()
    {
        Sequence sequence = DOTween.Sequence();

        _vignette?.color.Override(_healEffect);
        sequence.Append(DOTween.To(() => 0, x => SetVignetteIntensity(x), 0.4f, 0.2f));
        sequence.Append(DOTween.To(() => 0.4f, x => SetVignetteIntensity(x), 0, 0.2f));
        //        sequence.Append(DOTween.To(() => Color.white, x => _beautify?.tintColor.Override(x), _healEffect, 0.1f));
        //        sequence.Append(DOTween.To(() => _healEffect, x => _beautify?.tintColor.Override(x), Color.white, 0.4f));
    }

    private void DashEffect()
    {
        Sequence sequence = DOTween.Sequence();

        _vignette?.color.Override(Color.cyan);
        sequence.Append(DOTween.To(() => 0, x => SetVignetteIntensity(x), 0.4f, 0.3f));
        sequence.Append(DOTween.To(() => 0.4f, x => SetVignetteIntensity(x), 0, 0.1f));
        //        sequence.Append(DOTween.To(() => Color.white, x => _beautify?.tintColor.Override(x), Color.cyan, 0.1f));
        //        sequence.Append(DOTween.To(() => Color.cyan, x => _beautify?.tintColor.Override(x), Color.white, 0.3f));

        Sequence sequence2 = DOTween.Sequence();
        sequence2.Append(DOTween.To(() => 0, x => _lensDistortion.intensity.Override(x), -0.8f, 0.3f));
        sequence2.Append(DOTween.To(() => -0.8f, x => _lensDistortion.intensity.Override(x), 0, 0.1f));
    }

    private void ExitFromScene()
    {
        _beautify.downsampling.Override(true);
        _beautify.outline.Override(false);
        AnimProcess(SceneTransition);
    }

    private void EnterToScene()
    {
        AnimProcess(SceneTransition, true, 
            () => 
            {
                _beautify.downsampling.Override(false);
                _beautify.outline.Override(true); 
            });
    }

    private void TransitionForAct(TransitionSignal transSign)
    {
        DOTween.To(() => 0f, x => SceneTransition(transSign.IsInverse ? 0.5f - x : x), 0.5f, 0.5f);
    }

    private void AnimProcess(Action<float> act, bool isInverse = false, Action actByEnd = null)
    {
        DOTween.To(() => 0f, x => act.Invoke(isInverse ? 1f - x : x), 1f, 1f)
            .OnComplete(() => actByEnd?.Invoke());
    }

    private void SceneTransition(float progress)//0-1
    {
        SetDownsampling(progress * 64f);
        SetFinalBlur(progress);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="multiplier">1 - 64</param>
    private void SetDownsampling(float multiplier)
    {
        _beautify?.downsamplingMultiplier.Override(multiplier);
    }

    private void SetFinalBlur(float intensity)
    {
        _beautify.blurIntensity.Override(intensity * 10);
    }

    private void SetVignetteIntensity(float intensity)
    {
        _vignette?.intensity.Override(intensity);
    }
}