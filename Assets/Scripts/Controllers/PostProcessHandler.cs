using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class PostProcessHandler : MonoBehaviour
{
    [SerializeField] private Volume volume;
    [Range(-1f, 1f)]
    [SerializeField] private float _testFloat;

    private Vignette vignette;
    
    private void Awake()
    {
        if (volume.profile.TryGet<Vignette>(out vignette))
        {
            vignette.intensity.Override(0);
        }
    }

    [ContextMenu("PlayEffect")]
    private void CustomEffect()
    {
        Sequence sequence = DOTween.Sequence();
        _testFloat = -1f;
        sequence.Append(DOTween.To(() => _testFloat, x => _testFloat = x, 1f, 1f))
            .OnUpdate(() =>
            {
                SetVignetteIntensity(_testFloat);
            });

        sequence.Append(DOTween.To(() => _testFloat, x => _testFloat = x, 0f, 1f))
            .OnUpdate(() =>
            {
                SetVignetteIntensity(_testFloat);
            });
    }

    public void SetVignetteIntensity(float intensity)
    {
        if (vignette != null)
        {
            vignette.intensity.Override(intensity);
        }
    }
}