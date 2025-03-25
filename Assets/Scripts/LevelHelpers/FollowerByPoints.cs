using UnityEngine;

public class FollowerByPoints : MonoBehaviour
{
    [SerializeField] private PointOfFollow _currentTarget;
    [SerializeField] private float _speedRotate;
    [SerializeField] private float _speedMove;
    [SerializeField] private float _verticalSpeed;
    [SerializeField] private float _distanceTrigger;

    internal float DistanceTrigger => _distanceTrigger;

    private void Awake()
    {
        SetTarget(_currentTarget);
    }

    internal void SetTarget(PointOfFollow newTarget)
    {
        _currentTarget = newTarget;
    }

    private void FixedUpdate()
    {
        if (_currentTarget)
        {
            MoveToPoint();
            CheckPoint();
        }
    }

    private void MoveToPoint()
    {
        var tempVect = _currentTarget.transform.position;
        tempVect.y = transform.position.y;
        Vector3 directionToTarget = tempVect - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, _speedRotate * Time.fixedDeltaTime);

        transform.position += transform.forward * _speedMove * Time.fixedDeltaTime;

        if (_currentTarget.transform.position.y - _distanceTrigger / 3f > transform.position.y)
        {
            transform.position += Vector3.up * _verticalSpeed * Time.fixedDeltaTime;
        }
        if (_currentTarget.transform.position.y + _distanceTrigger / 3f < transform.position.y)
        {
            transform.position -= Vector3.up * _verticalSpeed * Time.fixedDeltaTime;
        }
    }

    private void CheckPoint()
    {
        if (Vector3.Distance(_currentTarget.transform.position, transform.position) < DistanceTrigger)
        {
            SetTarget(_currentTarget.NextPoint);
        }
    }

    [System.Serializable]
    private class FollowingPoint
    {
        public Transform Target;
        public Transform NextPoint;
    }
}