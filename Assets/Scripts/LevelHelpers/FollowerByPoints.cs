using System.Collections.Generic;
using UnityEngine;

public class FollowerByPoints : MonoBehaviour
{
//    [SerializeField] private List<Transform> _followPoints = new();
//    [SerializeField] private Transform _currentTarget;
    [SerializeField] private PointOfFollow _currentTarget;
    [SerializeField] private float _speedRotate;
    [SerializeField] private float _speedMove;
    [SerializeField] private float _verticalSpeed;
    [SerializeField] private float _distanceTrigger;

    private Dictionary<Transform, Transform> _mapNextPoints = new();

    internal float DistanceTrigger => _distanceTrigger;

    private void Awake()
    {
        /*        for (int i = 0; i < _followPoints.Count; i++)
                {
                    _mapNextPoints.Add(_followPoints[i],
                        _followPoints[i == _followPoints.Count - 1 ? 0 : i + 1]);
                }/**/
        SetTarget(_currentTarget);
    }

    internal void SetTarget(PointOfFollow newTarget)
    {
        if(_currentTarget)
            _currentTarget.isTracking = false;
        _currentTarget = newTarget;
        _currentTarget.SetFollower(this);
        _currentTarget.isTracking = true;
    }

    private void FixedUpdate()
    {
        if (_currentTarget)
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
    }

    [System.Serializable]
    private class FollowingPoint
    {
        public Transform Target;
        public Transform NextPoint;
    }
}