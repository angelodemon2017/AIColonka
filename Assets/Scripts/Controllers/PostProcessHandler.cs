using UnityEngine;
using UnityEngine.Rendering;
using DG.Tweening;
using Zenject;

public class PostProcessHandler : MonoBehaviour
{
    [Inject] private SignalBus _signalBus;
    [SerializeField] private Volume volume;

    private UnityEngine.Rendering.Universal.Vignette _vignette;
    private Beautify.Universal.Beautify _beautify;

    private void Awake()
    {
        if (volume.profile.TryGet(out _vignette))
        {
            _vignette.intensity.Override(0);
        }
        if (volume.profile.TryGet(out _beautify))
        {
            SetDownsampling(1);
        }
    }

    private void OnEnable()
    {
        _signalBus.Subscribe<DemoSignal>(CustomEffect);
    }

    private void OnDisable()
    {
        _signalBus.Unsubscribe<DemoSignal>(CustomEffect);
    }

    [ContextMenu("BeforeLoadOtherScene")]
    private void BeforeLoadOtherScene()
    {
        DOTween.To(() => 1f, x => SetDownsampling(x), 64, 2f);
    }

    [ContextMenu("AfterLoadNewScenes")]
    private void AfterLoadNewScene()
    {
        DOTween.To(() => 64, x => SetDownsampling(x), 1, 2f);
    }

    [ContextMenu("PlayEffect")]
    private void CustomEffect()
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(DOTween.To(() => 0f, x => SetVignetteIntensity(x), 1f, 1f));

        sequence.Append(DOTween.To(() => 1f, x => SetVignetteIntensity(x), 0f, 1f));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="multiplier">1 - 64</param>
    private void SetDownsampling(float multiplier)
    {
        _beautify?.downsamplingMultiplier.Override(multiplier);
    }

    private void SetVignetteIntensity(float intensity)
    {
        _vignette?.intensity.Override(intensity);
    }
}