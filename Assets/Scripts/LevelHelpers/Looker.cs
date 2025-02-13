using System;
using UnityEditor;
using UnityEngine;

public class Looker : MonoBehaviour
{
    [SerializeField] private Transform _target;

    private Looker()
    {
        EditorApplication.update += Look;
    }

    private void FixedUpdate()
    {
        Look();
    }

    public void SetTarget(Transform newTarget)
    {
        _target = newTarget;
    }

    private void Look()
    {
        try
        {
            if (_target)
            {
                transform.LookAt(_target);
            }
        }
        catch (Exception ex)
        {

        }
    }
}