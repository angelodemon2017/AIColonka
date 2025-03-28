using System.Collections.Generic;
using UnityEngine;
using Zenject;
using DG.Tweening;

public class AlphaSphereController : MonoBehaviour
{
    private const string RADIUS_SPHERE = "_RadiusSphere";
    private const string CENTER_SPHERE = "_CenterSphere";

    [SerializeField] private Vector3 _startCenter;
    [SerializeField] private List<Material> _materials;
    [SerializeField] private List<GameObject> _inversePbjects;
    [SerializeField] private Transform _sphere;
    [SerializeField] private float _radius;
    [SerializeField] private float _maxRad;
    [SerializeField] private float _timeTrans;

    private GameplayHandler _gameplayHandler;

    [Inject]
    private void Construct(
        GameplayHandler gameplayHandler)
    {
        _gameplayHandler = gameplayHandler;
    }

    private void OnValidate()
    {
        SetRadius(_radius);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Launch(_gameplayHandler.PlayerInstance.transform.position);
        }
    }

    [ContextMenu("DemoLaunch")]
    private void DemoLaunch()
    {
        SetCenter(_startCenter);
        DOTween.To(() => 0, x => _radius = x, _maxRad, _timeTrans);
    }

    private void Launch(Vector3 vector)
    {
        SetCenter(vector);
        DOTween.To(() => 0, x => SetRadius(x), _maxRad, _timeTrans)
            .OnComplete(() =>
            {
                _inversePbjects.ForEach(o => o.SetActive(!o.activeSelf));
                _sphere.localScale = Vector3.zero;
            });
    }

    [ContextMenu("SetDemoCenter")]
    private void SetDemoCenter()
    {
        SetCenter(_startCenter);
        _sphere.localScale = Vector3.zero;
    }

    internal void SetCenter(Vector3 vector)
    {
        _sphere.position = vector;
        _materials.ForEach(m => m.SetVector(CENTER_SPHERE, vector));
    }

    internal void SetRadius(float rad)
    {
        _sphere.localScale = Vector3.one * rad;
        _materials.ForEach(m => m.SetFloat(RADIUS_SPHERE, rad));
    }
}