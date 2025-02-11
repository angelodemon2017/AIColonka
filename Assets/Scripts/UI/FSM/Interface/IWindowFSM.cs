using UnityEngine;

public interface IWindowFSM
{
    void StartWindow();

    void PressedKey(EnumUIEvent keyCode);

    void Run();

    void ExitWindow();
}