using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptScene : MonoBehaviour
{
    [SerializeField] private List<Animator> animators;
    [SerializeField] private Camera _camera;

    private void Awake()
    {
        _camera.enabled = false;
    }

    public void RunScene()
    {
        foreach (var a in animators)
        {
            a.Play(0);
        }

        CameraController.Instance.SwitchCamera(_camera);
    }
}