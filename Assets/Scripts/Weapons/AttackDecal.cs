using UnityEngine;

public class AttackDecal : MonoBehaviour
{
    private const string FillRadius = "_FillRadius";
    private const string ArcAngle = "_ArcAngle";
    private const string MainTexture = "_MainTexture";
    private const string ColorEffect = "_ColorEffect";
    private const string ColorFilling = "_ColorFilling";

    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private Texture _textureEffect;
    [SerializeField] private Color _startColor;
    [SerializeField] private Color _doneColor;

    private void Awake()
    {
        LocalInit();
    }

    internal void Init(float wide, float size)
    {
        transform.localScale = Vector3.one * size;
        _meshRenderer.material.SetFloat(ArcAngle, wide);
    }

    private void LocalInit()
    {
        _meshRenderer.material.SetTexture(MainTexture, _textureEffect);
        _meshRenderer.material.SetColor(ColorEffect, _startColor);
        _meshRenderer.material.SetColor(ColorFilling, _doneColor);
        _meshRenderer.material.SetFloat(FillRadius, 0f);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="progress">0-1</param>
    internal void UpdateProgress(float progress)
    {
        _meshRenderer.material.SetFloat(FillRadius, progress);
    }
}