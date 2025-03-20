using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SettingsMenuUI : MAINWindow
{
    [SerializeField] private Toggle _debugToggle;
    [SerializeField] private Button _returnBTN;
    [SerializeField] private MAINWindow _windowByReturn;
    [SerializeField] private CheatsPanel _cheatsPanel;

    private DataHandler _dataHandler;
    private UIFSM _uifsm;

    private Settings _settings => _dataHandler.Settings;

    [Inject]
    private void Construct(
        DataHandler dataHandler,
        UIFSM uifsm)
    {
        _dataHandler = dataHandler;
        _uifsm = uifsm;

        SetUI(_settings);
    }

    public override void StartWindow()
    {
        base.StartWindow();

        _debugToggle.onValueChanged.AddListener(ChangeDebug);
        _returnBTN.onClick.AddListener(Return);
    }

    private void Return()
    {
        _uifsm.OpenWindow(_windowByReturn);
    }

    private void SetUI(Settings settings)
    {
        _debugToggle.isOn = settings.IsDebug;

        _cheatsPanel.gameObject.SetActive(settings.IsDebug);
    }

    private void ChangeDebug(bool isOn)
    {
        _settings.IsDebug = isOn;
        _cheatsPanel.gameObject.SetActive(isOn);
    }

    public override void ExitWindow()
    {
        base.ExitWindow();

        _debugToggle.onValueChanged.RemoveAllListeners();
        _returnBTN.onClick.RemoveAllListeners();

        SaveController.Save(_settings, Settings.Prefix);
    }
}