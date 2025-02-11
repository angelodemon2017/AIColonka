using UnityEngine;

public class WindowGameplay : MAINWindow
{
    public override void StartWindow()
    {
        base.StartWindow();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public override void Run()
    {
        base.Run();

        CameraController.Instance.UpdateMouse(
            Input.GetAxis("Mouse X"),
            Input.GetAxis("Mouse Y"));
    }

    public override void ExitWindow()
    {
        base.ExitWindow();
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
}