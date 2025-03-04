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
    private Transform _pLook;

    public Vector3 Direct => _directPoint.position - transform.position;
    internal bool IsLookingDown => transform.forward.y < 0f;

    private void Awake()
    {
        Instance = this;
    }

    internal void SetPivot(Transform tPivot, Transform pLook)
    {
        _pivot = tPivot;
        _pLook = pLook;

        ReturnParent();
    }

    internal void UnParrent()
    {
        _looker.SetTarget(null);
        transform.SetParent(null);
    }

    internal void LerpToPivot(float lerpPos)
    {
        transform.position = Vector3.Lerp(transform.position, _pivot.position, lerpPos);
    }

    internal void ReturnParent()
    {
        transform.position = _pivot.position;
        transform.SetParent(_pivot);
        _looker.SetTarget(_pLook);
    }
}