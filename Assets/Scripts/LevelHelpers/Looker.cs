using UnityEngine;
using Zenject;

public class Looker : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private bool AtCamera;

    private SignalBus _signalBus;
    private CameraController _cameraController;

    [Inject]
    private void Construct(
        CameraController cameraController,
        SignalBus signalBus)
    {
        _signalBus = signalBus;
        _cameraController = cameraController;

        Init();
    }

    private void Init()
    {
        if (AtCamera)
        {
            _signalBus.Subscribe<SwitchCameraSignal>(SwitchCamera);
            SetTarget(_cameraController.GetTransform);
        }
    }

    private void SwitchCamera(SwitchCameraSignal switchCameraSignal)
    {
        SetTarget(switchCameraSignal.NewCamera.transform);
    }

    private void FixedUpdate()
    {
        Look();
    }

    public void SetTarget(Transform newTarget)
    {
        _target = newTarget;
    }

    [ContextMenu("LookUpdate")]
    private void Look()
    {
        if (_target)
        {
            transform.LookAt(_target);
        }
    }

    private void OnDisable()
    {
        if (AtCamera)
            _signalBus.Unsubscribe<SwitchCameraSignal>(SwitchCamera);
    }
}