using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _directPoint;
    [SerializeField] private Looker _looker;
    [SerializeField] private LayerMask _layerMaskGroundAndWalls;
    [SerializeField] private CustomCameraSwitcher _customCameraSwitcher;

    private Transform _pivot;
    private Transform _pLook;
    private RaycastHit hit;
    private Ray ray;

    internal Vector3 PointOfLookGround => GetPointOfLook();
    public Vector3 Direct => _directPoint.position - transform.position;
    internal bool IsLookingDown => transform.forward.y < 0f;
    internal Transform GetTransform => _customCameraSwitcher.GetTransform;
    internal Camera CurrentCamera => _customCameraSwitcher.CurrentCamera;

    private Vector3 GetPointOfLook()
    {
        ray = new Ray(transform.position, _directPoint.position);
        Physics.Raycast(ray, out hit, 1000f, _layerMaskGroundAndWalls);
        if (hit.collider)
        {
            return hit.point;
        }
        return Vector3.zero;
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

    internal void SwitchCamera(Camera camera)
    {
        _customCameraSwitcher.SwitchCamera(camera);
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

    internal void ResetCamera()
    {
        _customCameraSwitcher.ResetCamera();
    }
}