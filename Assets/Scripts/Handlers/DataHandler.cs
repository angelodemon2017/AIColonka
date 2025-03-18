using UnityEngine;
using Zenject;

public class DataHandler
{
    private SignalBus _signalBus;

    private DialogSO _currentDialog;
    private MainData _mainData;

    private int testParam = 0;

    internal MainData CurrentData => _mainData;
    internal DialogSO CurrentDialog => _currentDialog;

    [Inject]
    private void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;

        Init();
    }

    public void Init()
    {
        _signalBus.Subscribe<SetTaskSignal>(SetTask);
        _signalBus.Subscribe<SetNextDialogSignal>(SetNextDialog);
        _signalBus.Subscribe<SetRoomPresetSignal>(SetRoomPreset);
        //demo:
        _signalBus.Subscribe<DemoSignal>((x) => ChangeParam(x.testFlag));
        _signalBus.Subscribe<ShowSignal>(ShowTest);
    }

    internal void SetData(MainData mainData)
    {
        _mainData = mainData;
    }

    private void SetRoomPreset(SetRoomPresetSignal setRoomPresetSignal)
    {
        _mainData.progressHistory.RoomConfig = (int)setRoomPresetSignal.IdConfig;
    }

    private void SetNextDialog(SetNextDialogSignal setNextDialogSignal)
    {
        _currentDialog = setNextDialogSignal.NextDialog;
    }

    private void SetTask(SetTaskSignal setTaskSignal)
    {
        _mainData.SetTask(setTaskSignal.Task);
        _signalBus.Fire(new TaskUpdatedSignal());
    }

    internal bool IsCurrentTask(TaskSO taskSO)
    {
        return _mainData.progressHistory.KeyTitleMainTask == taskSO.KeyTitle;
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