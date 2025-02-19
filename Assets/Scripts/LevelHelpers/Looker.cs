using System;
using UnityEditor;
using UnityEngine;

public class Looker : MonoBehaviour
{
    [SerializeField] private bool IsSettings;
    [SerializeField] private Transform _target;
    [SerializeField] private bool AtCamera;

    private Looker()
    {
        if (IsSettings)
        {
            EditorApplication.update += Look;
        }
    }

    private void Awake()
    {
        if (AtCamera)
        {
            _target = CameraController.Instance.transform;
        }
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
        catch (Exception) { }
    }
}