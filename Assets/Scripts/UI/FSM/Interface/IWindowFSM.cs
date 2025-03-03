using UnityEngine;

public interface IWindowFSM
{
    void StartWindow();

    void Run();

    void FixedRun();

    void ExitWindow();
}