using UnityEngine;

public class PointOfFollow : MonoBehaviour
{
    [SerializeField] private PointOfFollow _nextPoint;
    [SerializeField] private FollowerByPoints _follower;

    internal bool isTracking;

    private void OnDrawGizmos()
    {
        if (_nextPoint)
        {
            Gizmos.DrawLine(transform.position, _nextPoint.transform.position);
        }
    }

    internal void SetFollower(FollowerByPoints fbp)
    {
        _follower = fbp;
    }

    private void FixedUpdate()
    {
        if (isTracking &&
            Vector3.Distance(_follower.transform.position, transform.position) < _follower.DistanceTrigger)
        {
            _follower.SetTarget(_nextPoint);
        }
    }
}