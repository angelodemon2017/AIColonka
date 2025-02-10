using UnityEngine;

public class InputFSM : MonoBehaviour
{
    private float _mouseX;
    private float _mouseY;

    void Update()
    {
        _mouseX = Input.GetAxis("Mouse X");
        _mouseY = Input.GetAxis("Mouse Y");

        CameraController.Instance.UpdateMouse(_mouseX, _mouseY);
    }
}