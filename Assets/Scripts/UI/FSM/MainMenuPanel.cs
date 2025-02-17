using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuPanel : MAINWindow
{
    [SerializeField] private SceneLevelLoader _sceneLevelLoader;

    [SerializeField] private Button _newGameBTN;
    [SerializeField] private Button _loadGameBTN;
    [SerializeField] private Button _settingsBTN;
    [SerializeField] private Button _aboutBTN;
    [SerializeField] private TextMeshProUGUI _textLoadBTN;

    private MainData _tempData;

    private bool IsEmptyData => _tempData == null || _tempData.EmptyData;

    public override void StartWindow()
    {
        base.StartWindow();

        _tempData = SaveController.Load<MainData>();

        _newGameBTN.onClick.AddListener(NewGame);
        _loadGameBTN.onClick.AddListener(LoadGame);
        _settingsBTN.onClick.AddListener(Settings);
        _aboutBTN.onClick.AddListener(About);

        _loadGameBTN.interactable = !IsEmptyData;
        _textLoadBTN.text = $"Continue" + (IsEmptyData ? string.Empty : $"{_tempData.progressHistory.CurrentScene} scene");
    }

    private void NewGame()
    {
        ControllerDemoSaveFile.Instance.mainData = new MainData();
        _sceneLevelLoader.LoadLevel(EnumLevels.DialogsHub);
    }

    private void LoadGame()
    {
        ControllerDemoSaveFile.Instance.mainData = _tempData;
        _sceneLevelLoader.LoadLevel((EnumLevels)_tempData.progressHistory.CurrentScene);
    }

    private void Settings()
    {

    }

    private void About()
    {

    }
}