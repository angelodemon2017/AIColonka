using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PanelSelectingLevel : MAINWindow
{
    [SerializeField] private LevelButtonPresent _prefabLevelButtonPresent;
    [SerializeField] private Transform _parentButtons;
    [SerializeField] private MAINWindow _dummyWindow;
    [SerializeField] private List<EnumLevels> _availableLevels;

    private SignalBus _signalBus;
    private DataHandler _dataHandler;
    private TaskConfig _taskConfig;
    private SceneController _sceneController;

    private TaskSO currentTask => _taskConfig.GetTaskByKey(_dataHandler.CurrentData.progressHistory.KeyTitleMainTask);

    private IEnumerable<EnumLevels> AvailableLevels =>
        _dataHandler.Settings.IsDebug ?
        _availableLevels :
        currentTask.AvailableLevels;

    [Inject]
    private void Construct(
        SignalBus signalBus,
        TaskConfig taskConfig,
        DataHandler dataHandler,
        SceneController sceneController)
    {
        _signalBus = signalBus;
        _taskConfig = taskConfig;
        _dataHandler = dataHandler;
        _sceneController = sceneController;

        Init();
    }

    private void Init()
    {
        InitButtons();
    }

    private async void InitButtons()
    {
        EnumLevels curScene = (EnumLevels)_dataHandler.CurrentData.progressHistory.CurrentScene;
        _parentButtons.DestroyChildrens();
        foreach (var lev in AvailableLevels)
        {
            if (lev == curScene)
                continue;

            var newLev = Instantiate(_prefabLevelButtonPresent, _parentButtons);
            await newLev.InitAsync(lev, lev == currentTask.GetTargetLvl, SelectVariant);
        }
    }

    private void SelectVariant(int selectedVariant)
    {
        _signalBus.Fire(new SetWindowSignal(_dummyWindow));
        _sceneController.LoadLevelByEnum((EnumLevels)selectedVariant);
    }
}