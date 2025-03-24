using UnityEngine;
using Zenject;

public class CustomCameraSwitcher : MonoBehaviour
{
    [SerializeField] private Camera _mainCamera;
    private Camera _secondCamera;

    [Inject]
    private SignalBus _signalBus;

    internal Transform GetTransform => _mainCamera.enabled ? _mainCamera.transform : _secondCamera.transform;

    private void Awake()
    {
        _mainCamera = GetComponent<Camera>();
    }

    public void SwitchCamera(Camera secondCamera)
    {
        if (_secondCamera)
        {
            _secondCamera.enabled = false;
        }
        _secondCamera = secondCamera;
        _secondCamera.enabled = true;
        _mainCamera.enabled = false;
        _signalBus.Fire(new SwitchCameraSignal(_secondCamera));
    }

    public void ResetCamera()
    {
        if (_secondCamera != null)
        {
            _secondCamera.enabled = false;
            _secondCamera = null;
        }

        _mainCamera.enabled = true;
        _signalBus.Fire(new SwitchCameraSignal(_mainCamera));
    }
}