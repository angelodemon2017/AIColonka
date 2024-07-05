using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public static InputController Instance;

    public Action<Vector3> moveAction;

    private Vector3 moveVector = Vector3.zero;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        Vector3 moveVector = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            moveVector += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveVector += Vector3.left;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveVector += Vector3.back;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveVector += Vector3.right;
        }

        if (moveVector != Vector3.zero)
        {
            moveAction?.Invoke(moveVector);
        }
    }
}