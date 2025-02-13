using UnityEngine;

public class CustomCameraSwitcher : MonoBehaviour
{
    private Camera _mainCamera;
    private Camera _secondCamera;

    private void Awake()
    {
        _mainCamera = GetComponent<Camera>();
    }

    public void SwitchCamera(Camera secondCamera)
    {
        _secondCamera = secondCamera;
        _secondCamera.enabled = true;
        _mainCamera.enabled = false;
    }

    public void ResetCamera()
    {
        if (_secondCamera != null)
        {
            _secondCamera.enabled = false;
            _secondCamera = null;
        }

        _mainCamera.enabled = true;
    }
}