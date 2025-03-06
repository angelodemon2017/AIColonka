using UnityEngine;
using UnityEngine.Events;

public class DisolveController : MonoBehaviour
{
    const string Dissolve = "_DissolveAmount";

    [SerializeField] private bool _isDebug;
    [SerializeField] private float _startDis;
    [SerializeField] private float _endDis;
    [SerializeField] private float _stepDis;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private float _dissolve;

    private bool _isRun;

    [SerializeField] private UnityEvent _endDissolve;

    private void OnValidate()
    {
        if (_isDebug)
        {
            UpdateMaterial();
        }
    }

    private void Awake()
    {
        _isRun = false;
        _dissolve = _startDis;
        UpdateMaterial();
    }

    private void UpdateMaterial()
    {
        _meshRenderer.material.SetFloat(Dissolve, _dissolve);
    }

    public void CallDissolve()
    {
        _dissolve = _startDis;
        _isRun = true;
    }

    private void Update()
    {
        if (_isRun)
        {
            bool isGreater = _endDis > _startDis;
            _dissolve += _stepDis * (isGreater ? 1 : -1);
            UpdateMaterial();
            if (isGreater && _dissolve > _endDis ||
                !isGreater && _dissolve < _endDis)
            {
                _isRun = false;
                _endDissolve?.Invoke();
            }
        }
    }
}