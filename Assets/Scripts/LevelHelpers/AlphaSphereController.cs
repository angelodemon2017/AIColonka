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
    [SerializeField] private List<Transform> _scalers;
    [SerializeField] private float _radius;
    [SerializeField] private float _maxRad;
    [SerializeField] private float _timeTrans;

    private SignalBus _signalBus;
    private GameplayHandler _gameplayHandler;
    private CameraController _cameraController;

    [Inject]
    private void Construct(
        CameraController cameraController,
        GameplayHandler gameplayHandler,
        SignalBus signalBus)
    {
        _cameraController = cameraController;
        _gameplayHandler = gameplayHandler;
        _signalBus = signalBus;
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

    private void Launch(Vector3 vector)
    {
        SetCenter(vector);
        DOTween.To(() => 0, x => SetRadius(x), _maxRad, _timeTrans)
            .OnComplete(() =>
            {
                _inversePbjects.ForEach(o => o.SetActive(!o.activeSelf));
                _cameraController.EffectFinalScene();
            });
        _signalBus.Fire(new EffectFinalSceneSignal());
    }

    [ContextMenu("SetDemoCenter")]
    private void SetDemoCenter()
    {
        SetCenter(_startCenter);
        _scalers.ForEach(s => s.localScale = Vector3.zero);
    }

    internal void SetCenter(Vector3 vector)
    {
        _scalers.ForEach(s => s.position = vector);
        _materials.ForEach(m => m.SetVector(CENTER_SPHERE, vector));
    }

    internal void SetRadius(float rad)
    {
        _scalers.ForEach(s => s.localScale = rad == 0f ? Vector3.zero : Vector3.one * rad);
        _materials.ForEach(m => m.SetFloat(RADIUS_SPHERE, rad));
    }
}