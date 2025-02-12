using UnityEngine;

public class WindowGameplay : MAINWindow
{
    public override void StartWindow()
    {
        base.StartWindow();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
//        CameraController.Instance.ResetCamera();
    }

    public override void Run()
    {
        base.Run();

        CameraController.Instance.UpdateMouse(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y"));

        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        PersonMovement.Instance.OnMovePlayer(horizontal, vertical);

        if (Input.GetButtonDown("Jump"))
        {
            PersonMovement.Instance.OnJump();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            PersonMovement.Instance.OnAttack();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            PersonMovement.Instance.OnSecondAttack();
        }
    }

    public override void ExitWindow()
    {
        base.ExitWindow();
        PersonMovement.Instance.OnMovePlayer(0, 0);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
}