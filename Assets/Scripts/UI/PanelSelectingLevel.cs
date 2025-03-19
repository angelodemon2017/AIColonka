using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PanelSelectingLevel : MAINWindow
{
    [SerializeField] private SceneLevelLoader _sceneLevelLoader;
    [SerializeField] private LevelButtonPresent _prefabLevelButtonPresent;
    [SerializeField] private Transform _parentButtons;

    [SerializeField] private List<EnumLevels> _availableLevels;

    private DataHandler _dataHandler;
    private TaskConfig _taskConfig;

    private TaskSO currentTask => _taskConfig.GetTaskByKey(_dataHandler.CurrentData.progressHistory.KeyTitleMainTask);

    private IEnumerable<EnumLevels> AvailableLevels =>
        ControllerDemoSaveFile.Instance.IsDebug ?
        _availableLevels :
        currentTask.AvailableLevels;

    [Inject]
    private void Construct(
        TaskConfig taskConfig,
        DataHandler dataHandler)
    {
        _taskConfig = taskConfig;
        _dataHandler = dataHandler;

        Init();
    }

    private void Init()
    {
        InitButtons();
    }

    private async void InitButtons()
    {
        _parentButtons.DestroyChildrens();
        foreach (var lev in AvailableLevels)
        {
            if (lev == ControllerDemoSaveFile.Instance.CurrentLevel)
                continue;

            var newLev = Instantiate(_prefabLevelButtonPresent, _parentButtons);
            await newLev.InitAsync(lev, lev == currentTask.GetTargetLvl, SelectVariant);
        }
    }

    private void SelectVariant(int selectedVariant)
    {
        _sceneLevelLoader.LoadLevel((EnumLevels)selectedVariant);
    }
}