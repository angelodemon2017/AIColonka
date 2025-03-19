using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using Zenject;

public class Looker : MonoBehaviour
{
    [SerializeField] private bool IsSettings;
    [SerializeField] private Transform _target;
    [SerializeField] private bool AtCamera;

    [Inject]
    private CameraController _cameraController;

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
            StartCoroutine(InitCamera());
        }
    }

    private IEnumerator InitCamera()
    {
        while (!_cameraController)
        {
            yield return null;
        }
        _target = _cameraController.transform;
    }

    private void FixedUpdate()
    {
        Look();
    }

    public void SetTarget(Transform newTarget)
    {
        _target = newTarget;
    }

    [ContextMenu("LookUpdate")]
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