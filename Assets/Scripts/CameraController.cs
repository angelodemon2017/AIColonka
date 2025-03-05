using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    [SerializeField] private Transform _directPoint;

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

    internal void UnParrent(float time)
    {
        _looker.SetTarget(null);
        transform.SetParent(null);
        _lerpTime = 0f;
        _timeOfMove = time;
    }

    private float _lerpTime = 0f;
    private float _timeOfMove;

    private void FixedUpdate()
    {
        FlexMoveCamera(Time.fixedDeltaTime);
    }

    private void FlexMoveCamera(float deltaTime)
    {
        if (_timeOfMove > 0)
        {
            _lerpTime += deltaTime;
            LerpToPivot(_lerpTime / _timeOfMove);
            if (_lerpTime > _timeOfMove)
            {
                ReturnParent();
            }
        }
    }

    private void LerpToPivot(float lerpPos)
    {
        transform.position = Vector3.Lerp(transform.position, _pivot.position, lerpPos);
    }

    private void ReturnParent()
    {
        _timeOfMove = 0f;
        transform.position = _pivot.position;
        transform.SetParent(_pivot);
        _looker.SetTarget(_pLook);
    }
}