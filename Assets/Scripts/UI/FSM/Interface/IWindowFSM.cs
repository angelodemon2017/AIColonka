public interface IWindowFSM
{
    bool NeedTransition { get; }

    void StartWindow();

    void Run();

    void FixedRun();

    void ExitWindow();
}