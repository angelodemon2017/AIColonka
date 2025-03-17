using UnityEngine;
using Zenject;

public class DataHandler
{
    private SignalBus _signalBus;

    private MainData _mainData;

    private int testParam = 0;

    internal MainData CurrentData => _mainData;

    [Inject]
    private void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;

        Init();
    }

    public void Init()
    {
        _signalBus.Subscribe<DemoSignal>((x) => ChangeParam(x.testFlag));
        _signalBus.Subscribe<ShowSignal>(ShowTest);
    }

    internal void SetData(MainData mainData)
    {
        _mainData = mainData;
    }

    internal bool IsCurrentTask(TaskSO taskSO)
    {
        return _mainData.progressHistory.IsWasDone(taskSO.KeyTitle);
    }

    internal bool WasDone(TaskSO taskSO)
    {
        return _mainData.progressHistory.IsWasDone(taskSO.KeyTitle);
    }

    #region tests
    internal void ChangeParam(bool ds)
    {
        testParam += ds ? 1 : -1;
    }

    internal void ShowTest()
    {
        Debug.Log($"testParam={testParam}");
    }
    #endregion
}