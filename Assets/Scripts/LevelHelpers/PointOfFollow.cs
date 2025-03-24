using UnityEngine;

public class PointOfFollow : MonoBehaviour
{
    [SerializeField] private PointOfFollow _nextPoint;

    internal PointOfFollow NextPoint => _nextPoint;

    private void OnDrawGizmos()
    {
        if (_nextPoint)
        {
            Gizmos.DrawLine(transform.position, _nextPoint.transform.position);
        }
    }
}