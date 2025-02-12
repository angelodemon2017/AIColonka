using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    [SerializeField] private Transform _directPoint;
    public Transform target;
    [SerializeField] private Vector3 _centerOffset;
    public Vector3 offset = new Vector3(0, 5, -10);
    public float sensitivity = 10f;
    public float minYAngle = -20f;
    public float maxYAngle = 60f;
    private float _currentX = 0f;
    private float _currentY = 0f;

    private Camera _mainCamera;
    private Camera _secondCamera;
    private Transform _pivot;

    public Vector3 Direct => _directPoint.position - transform.position;

    private void Awake()
    {
        Instance = this;
        _mainCamera = GetComponent<Camera>();
//        Cursor.visible = false;
//        Cursor.lockState = CursorLockMode.Locked;
//        _pivot = target.parent;
    }

    internal void SetPivot(Transform tPivot)
    {
        _pivot = tPivot;
        target = tPivot;
    }

    internal void UpdateMouse(float xMouse, float yMouse)
    {
        _currentX += xMouse * sensitivity;
        _currentY -= yMouse * -sensitivity;

        _currentY = Mathf.Clamp(_currentY, minYAngle, maxYAngle);
    }

    void LateUpdate()
    {
        Quaternion rotation = Quaternion.Euler(_currentY, _currentX, 0);
        transform.position = target.position + rotation * offset;
        transform.LookAt(target.position + _centerOffset);
        _pivot.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0f);
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