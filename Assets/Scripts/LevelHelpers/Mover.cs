using System;
using UnityEditor;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private Transform _moveTarget;
    [SerializeField] private float _speedMove;

    public Action EndMove;

    private Mover()
    {
        EditorApplication.update += JustMove;
    }

    private void FixedUpdate()
    {
        JustMove();
    }

    public void SetTarget(Transform newTarget)
    {
        _moveTarget = newTarget;
    }

    public void SetPosition(Transform newTarget)
    {
        _moveTarget = null;
        transform.position = newTarget.position;
    }

    private void JustMove()
    {
        if (_moveTarget && Vector3.Distance(transform.position, _moveTarget.position) > 0.1f)
        {
//            transform.localPosition = Vector3.Lerp(transform.localPosition, _moveTarget.localPosition, _speedMove);
            transform.position = Vector3.Lerp(transform.position, _moveTarget.position, _speedMove);
            if (Vector3.Distance(transform.position, _moveTarget.position) < 0.1f)
            {
                EndMove?.Invoke();
            }
        }
    }
}