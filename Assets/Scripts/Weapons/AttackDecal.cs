using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AttackDecal : MonoBehaviour
{
    private const string FillRadius = "_FillRadius";
    private const string ArcAngle = "_ArcAngle";
    private const string MainTexture = "_DecalTexture";
    private const string ColorEffect = "_ColorEffect";
    private const string ColorFilling = "_ColorFilling";

    [SerializeField] private Mover _mover;
    [SerializeField] private DecalProjector _decalProjector;
    [SerializeField] private Texture _textureEffect;
    [SerializeField] private Color _startColor;
    [SerializeField] private Color _doneColor;
    [SerializeField] private Material _instMaterial;

    internal Mover Mover => _mover;
    private Material _material => _decalProjector.material;

    private void Awake()
    {
        LocalInit();
    }

    internal void Init(float wide, float size)
    {
        _decalProjector.size = Vector3.one * size;
        transform.localScale = Vector3.one * size;
        _material.SetFloat(ArcAngle, wide);
    }

    private void LocalInit()
    {
        _decalProjector.material = Instantiate(_instMaterial);
        _material.SetFloat(FillRadius, 0f);
        _material.SetTexture(MainTexture, _textureEffect);
        _material.SetColor(ColorEffect, _startColor);
        _material.SetColor(ColorFilling, _doneColor);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="progress">0-1</param>
    internal void UpdateProgress(float progress)
    {
        _material.SetFloat(FillRadius, progress);
    }

    private void OnDestroy()
    {
//        Debug.LogWarning($"destroyed {_material.GetFloat(FillRadius)}");
    }
}