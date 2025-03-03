using System;
using UnityEditor;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private bool IsSettings;
    [SerializeField] private Transform _moveTarget;
    [SerializeField] private float _speedMove;
    [SerializeField] private float _distanceToTarget = 0.1f;

    private Vector3 _targetPos = Vector3.zero;
    public Action EndMove;

    private Mover()
    {
        if (IsSettings)
        {
            EditorApplication.update += JustMove;
        }
    }

    private void FixedUpdate()
    {
        JustMove();
    }

    public void SetTarget(Transform newTarget)
    {
        _moveTarget = newTarget;
    }

    public void SetVectTarget(Vector3 newTrg)
    {
        _targetPos = newTrg;
    }

    public void SetPosition(Transform newTarget)
    {
        _moveTarget = null;
        transform.position = newTarget.position;
    }

    private void JustMove()
    {
        if (_moveTarget && Vector3.Distance(transform.position, _moveTarget.position) > _distanceToTarget)
        {
            transform.position = Vector3.Lerp(transform.position, _moveTarget.position, _speedMove * Time.fixedDeltaTime);
            if (Vector3.Distance(transform.position, _moveTarget.position) < _distanceToTarget)
            {
                EndMove?.Invoke();
            }
        }
        if (_targetPos != Vector3.zero && Vector3.Distance(transform.position, _targetPos) > _distanceToTarget)
        {
            transform.position = Vector3.Lerp(transform.position, _targetPos, _speedMove * Time.fixedDeltaTime);
            if (Vector3.Distance(transform.position, _targetPos) < _distanceToTarget)
            {
                EndMove?.Invoke();
            }
        }
    }
}