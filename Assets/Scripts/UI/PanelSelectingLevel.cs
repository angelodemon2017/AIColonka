using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PanelSelectingLevel : MAINWindow
{
    [SerializeField] private SceneLevelLoader _sceneLevelLoader;
    [SerializeField] private LevelButtonPresent _prefabLevelButtonPresent;
    [SerializeField] private Transform _parentButtons;

    [SerializeField] private List<EnumLevels> _availableLevels;

    [Inject] private TaskConfig _taskConfig;

    private TaskSO currentTask => _taskConfig.GetTaskByKey(ControllerDemoSaveFile.Instance.mainData.progressHistory.KeyTitleMainTask);

    private IEnumerable<EnumLevels> AvailableLevels =>
        ControllerDemoSaveFile.Instance.IsDebug ?
        _availableLevels :
        currentTask.AvailableLevels;

    private void Awake()
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

    private void OnDestroy()
    {
//        EventBus.Unsubscribe<EventKey>(CheckKey);
    }
}