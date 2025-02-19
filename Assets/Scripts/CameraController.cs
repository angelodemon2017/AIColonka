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

    [SerializeField] private Looker _looker;
    [SerializeField] private Mover _mover;

    private Transform _pivot;
    private WhoIs _targetEnemy;
    private Vector3 _tempPosit;

    internal bool EnemyInTarget => _targetEnemy;
    public Vector3 Direct => _directPoint.position - transform.position;

    private void Awake()
    {
        Instance = this;
    }

    internal void SetPivot(Transform tPivot, Transform pLook)
    {
        _pivot = tPivot;
        target = pLook;

        _mover.SetPosition(_pivot);
        _looker.SetTarget(target);
        _mover.SetTarget(_pivot);
    }

    internal void UpdateMouse(float xMouse, float yMouse)
    {
        _currentX += xMouse * sensitivity;
        _currentY -= yMouse * -sensitivity;

        _currentY = Mathf.Clamp(_currentY, minYAngle, maxYAngle);
    }

    internal void SetEnemyTarget(WhoIs whoIsTarget)
    {
        _targetEnemy = whoIsTarget;
    }

    internal void CancelEnemyTarget()
    {
        _targetEnemy = null;
    }

    void LateUpdate()
    {
/*        Quaternion rotation = Quaternion.Euler(_currentY, _currentX, 0);
        transform.position = target.position + rotation * offset;
        transform.LookAt(target.position + _centerOffset);

        if (_targetEnemy)
        {
            _tempPosit = _targetEnemy.transform.position;
            _tempPosit.y = _pivot.position.y;
            _pivot.LookAt(_tempPosit);
        }
        else
        {
            _pivot.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0f);
        }/**/
    }
}