using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuUI : MAINWindow
{
    [SerializeField] private Toggle _debugToggle;
    [SerializeField] private Button _returnBTN;
    [SerializeField] private MAINWindow _windowByReturn;
    [SerializeField] private CheatsPanel _cheatsPanel;

    private Settings _settings => ControllerDemoSaveFile.Instance.Settings;

    public override void StartWindow()
    {
        base.StartWindow();

        _debugToggle.onValueChanged.AddListener(ChangeDebug);
        _returnBTN.onClick.AddListener(Return);

        SetUI(_settings);
    }

    private void Return()
    {
        UIFSM.Instance.OpenWindow(_windowByReturn);
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