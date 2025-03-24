using UnityEngine;

[System.Serializable]
public class SwitchCameraSignal
{
    public Camera NewCamera;

    public SwitchCameraSignal(Camera newCamera)
    {
        NewCamera = newCamera;
    }
}