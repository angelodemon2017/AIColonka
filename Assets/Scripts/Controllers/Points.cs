using UnityEngine;

[System.Serializable]
public class Points
{
    public float Xsensitivity = 10f;
    public float Ysensitivity = 1f;

    [SerializeField] private float _totalSensitiv;
    [SerializeField] private float _totalHorSensitiv;

    public Transform PointOfLookCamera;
    public Transform PointOfMoveCamera;
    public Transform PointOfCenterOrbit;
    public Transform PointOfTargetForEnemy;

    [SerializeField] private Transform _bottomPoint;
    [SerializeField] private Transform _topPoint;

    private float _currentX = 0f;
    private float _verticalPosit = 0.5f;
    private float _cashMinDistance = 0f;

    private Vector3 _middlePoint;
    private Vector3 _middlePoint2;

    private WhoIs _holdTarget;
    private Vector3 _tempPosit;

    private float minDistance
    {
        get
        {
            if (_cashMinDistance == 0f)
            {
                _cashMinDistance = Vector3.Distance(PointOfLookCamera.position, _bottomPoint.position);
            }

            return _cashMinDistance;
        }
    }
    private float _totalDistance => _transCol ?
        _distanceToWall :
        (_holdTarget ?
        minDistance / 2 :
        minDistance);
    internal bool EnemyIsTarget => _holdTarget;
    internal WhoIs TargetEnemy => _holdTarget;

    internal void SetHoldTarget(WhoIs newTarget)
    {
        _holdTarget = newTarget;
        _holdTarget.OnDeath += WindowGameplay.Instance.CancelTarget;
    }

    internal void CancelTarget()
    {
        if (_holdTarget)
        {
            _holdTarget.OnDeath -= WindowGameplay.Instance.CancelTarget;
            _holdTarget = null;
        }
        _currentX = PointOfCenterOrbit.rotation.eulerAngles.y;
        _verticalPosit = 0.2f;
    }

    internal void Move(float xMouse, float yMouse)
    {
        _currentX += xMouse * Xsensitivity;

        _verticalPosit += yMouse * -Ysensitivity;
        _verticalPosit = Mathf.Clamp(_verticalPosit, 0f, 1f);
    }

    Quaternion _targetRotation;

    RaycastHit hit;
    Ray ray;
    Transform _transCol;
    float _distanceToWall;
    internal void FixUpd()
    {
        ray = new Ray(PointOfTargetForEnemy.position, PointOfMoveCamera.position - PointOfTargetForEnemy.position);
        Physics.Raycast(ray, out hit, minDistance);
        if (hit.collider && hit.collider.tag != Dicts.Tags.Player)
        {
            _transCol = hit.collider.transform;
            _distanceToWall = Vector3.Distance(PointOfLookCamera.position, hit.point);
        }
        CalcPointOfCamera();
    }

    private void CalcPointOfCamera()
    {
        if (_holdTarget)
        {
            _middlePoint = PointOfLookCamera.position * 2 - _holdTarget.transform.position;

            _tempPosit = _holdTarget.transform.position;
            _tempPosit.y = PointOfCenterOrbit.position.y;

            _targetRotation = Quaternion.LookRotation(_tempPosit - PointOfCenterOrbit.position, Vector3.up);
        }
        else
        {
            _targetRotation = Quaternion.Euler(0, _currentX, 0f);

            _middlePoint = Vector3.Lerp(
                _bottomPoint.position,
                _topPoint.position,
                _verticalPosit);
        }

        _middlePoint2 = Vector3.MoveTowards(PointOfLookCamera.position, _middlePoint, _totalDistance);

        PointOfCenterOrbit.rotation = Quaternion.Slerp(PointOfCenterOrbit.rotation, _targetRotation, _totalHorSensitiv * Time.fixedDeltaTime);
        PointOfMoveCamera.position = Vector3.Lerp(PointOfMoveCamera.position, _middlePoint2, _totalSensitiv * Time.fixedDeltaTime);
    }
}