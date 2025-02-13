using UnityEngine;
using UnityEngine.Events;

public class MoveChecker : MonoBehaviour
{
    [SerializeField] private Transform _targetChecking;
    [SerializeField] private float _triggerDistance;
    [SerializeField] private UnityEvent unityEvent;

    private void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, _targetChecking.position) < _triggerDistance)
        {
            unityEvent?.Invoke();
        }
    }
}