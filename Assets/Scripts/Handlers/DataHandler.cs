using Zenject;

public class DataHandler
{
    private SignalBus _signalBus;

    private DialogSO _currentDialog;
    private MainData _mainData;

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
    }

    internal void SetData(MainData mainData)
    {
        _mainData = mainData;
    }

    private void SetNextDialog(SetNextDialogSignal setNextDialogSignal)
    {
        _currentDialog = setNextDialogSignal.NextDialog;
    }

    private void SetTask(SetTaskSignal setTaskSignal)
    {
        _mainData.SetTask(setTaskSignal.Task);
        _signalBus.Fire(new TaskUpdatedSignal());
        _signalBus.Fire(new BackTalkSignal(setTaskSignal.Task.KeyTitle, 2f, Localizations.Tables.Tasks));
    }

    internal void PickProp(EnumLevelProp levelProp)
    {
        _mainData.PickProp(levelProp);
        _signalBus.Fire(new BitUpgradedSignal());
    }

    internal void AddBits(int count)
    {
        _mainData.AddBits(count);
        _signalBus.Fire(new BitUpgradedSignal());
    }

    internal void AddWVs(int count)
    {
        _mainData.AddAVP(count);
        _signalBus.Fire(new BitUpgradedSignal());
    }

    internal bool IsCurrentTask(TaskSO taskSO)
    {
        return _mainData.progressHistory.KeyTitleMainTask == taskSO.KeyTitle;
    }

    internal bool WasDone(TaskSO taskSO)
    {
        return _mainData.progressHistory.IsWasDone(taskSO.KeyTitle);
    }
}