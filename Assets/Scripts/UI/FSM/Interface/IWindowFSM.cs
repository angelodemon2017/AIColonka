using UnityEngine;

public interface IWindowFSM
{
    bool IsGamePlayState { get; }

    void StartWindow();

    void PressedKey(EnumUIEvent keyCode);

    void Run();

    void ExitWindow();
}