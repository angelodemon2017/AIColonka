using Zenject;

public class DataHandler
{
    private TaskConfig _taskConfig;
    private SignalBus _signalBus;
    private DialogSO _currentDialog;

    private MainData _mainData;
    private Settings _settings = new Settings();

    internal MainData CurrentData => _mainData;
    internal DialogSO CurrentDialog => _currentDialog;
    internal Settings Settings => _settings;

    private string _lastTaskSO;
    internal TaskSO GetNotifTask()
    {
        if (_lastTaskSO == _mainData.progressHistory.KeyTitleMainTask)
        {
            return null;
        }
        else
        {
            _lastTaskSO = _mainData.progressHistory.KeyTitleMainTask;
            return _taskConfig.GetTaskByKey(_lastTaskSO);
        }
    }

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

        _settings = SaveController.Load<Settings>(Settings.Prefix);
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