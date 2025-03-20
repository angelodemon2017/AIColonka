using UnityEngine.UI;
using TMPro;
using UnityEngine;
using Zenject;

public class PauseMenuPanel : MAINWindow
{
    [SerializeField] private Button _continueBTN;
    [SerializeField] private Button _settingsBTN;
    [SerializeField] private Button _toMainMenuBTN;

    [SerializeField] private MAINWindow _windowByContinue;
    [SerializeField] private MAINWindow _settingWindow;

    private SceneController _sceneController;
    private UIFSM _uifsm;

    [Inject]
    private void Construct(
        SceneController sceneController,
        UIFSM uifsm)
    {
        _sceneController = sceneController;
        _uifsm = uifsm;
    }

    public override void StartWindow()
    {
        base.StartWindow();

        _continueBTN.onClick.AddListener(Continue);
        _settingsBTN.onClick.AddListener(Settings);
        _toMainMenuBTN.onClick.AddListener(ToMainMenu);
    }

    private void Continue()
    {
        _uifsm.OpenWindow(_windowByContinue);
    }

    private void Settings()
    {
        _uifsm.OpenWindow(_settingWindow);
    }

    private void ToMainMenu()
    {
        _sceneController.LoadLevelByEnum(EnumLevels.MainMenu);
    }

    public override void ExitWindow()
    {
        base.ExitWindow();

        _continueBTN.onClick.RemoveAllListeners();
        _settingsBTN.onClick.RemoveAllListeners();
        _toMainMenuBTN.onClick.RemoveAllListeners();
    }
}