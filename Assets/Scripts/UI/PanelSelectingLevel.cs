using System.Collections.Generic;
using UnityEngine;

public class PanelSelectingLevel : MAINWindow
{
    [SerializeField] private SceneLevelLoader _sceneLevelLoader;
    [SerializeField] private LevelButtonPresent _prefabLevelButtonPresent;
    [SerializeField] private Transform _parentButtons;

    [SerializeField] private List<EnumLevels> _availableLevels;

    private IEnumerable<EnumLevels> AvailableLevels =>
        ControllerDemoSaveFile.Instance.IsDebug ?
        _availableLevels :
        ControllerDemoSaveFile.Instance.GetCurrentTask().AvailableLevels;

    private void Awake()
    {
        InitButtons();
    }

    private async void InitButtons()
    {
        _parentButtons.DestroyChildrens();
        foreach (var lev in AvailableLevels)
        {
            var newLev = Instantiate(_prefabLevelButtonPresent, _parentButtons);
            await newLev.InitAsync(lev, SelectVariant);
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