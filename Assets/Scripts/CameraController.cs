using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    [SerializeField] private Transform _directPoint;
//    public Transform lookTarget;
//    [SerializeField] private Vector3 _centerOffset;
//    public Vector3 offset = new Vector3(0, 5, -10);
//    public float sensitivity = 10f;
//    public float minYAngle = -20f;
//    public float maxYAngle = 60f;
//    private float _currentX = 0f;
//    private float _currentY = 0f;

    [SerializeField] private Looker _looker;

    private Transform _pivot;

    public Vector3 Direct => _directPoint.position - transform.position;
    internal bool IsLookingDown => transform.forward.y < 0f;

    private void Awake()
    {
        Instance = this;
    }

    internal void SetPivot(Transform tPivot, Transform pLook)
    {
        _pivot = tPivot;
//        lookTarget = pLook;

        _looker.SetTarget(pLook);

        transform.position = _pivot.position;
        transform.SetParent(_pivot);
    }

/*    internal void UpdateMouse(float xMouse, float yMouse)
    {
        _currentX += xMouse * sensitivity;
        _currentY -= yMouse * -sensitivity;

        _currentY = Mathf.Clamp(_currentY, minYAngle, maxYAngle);
    }/**/
}