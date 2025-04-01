using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

public class MainMenuPanel : MAINWindow
{
    [SerializeField] private Button _newGameBTN;
    [SerializeField] private Button _loadGameBTN;
    [SerializeField] private Button _settingsBTN;
    [SerializeField] private Button _aboutBTN;
    [SerializeField] private TextMeshProUGUI _textLoadBTN;

    [SerializeField] private DialogSO _startDialog;
    [SerializeField] private MAINWindow _settingWindow;

    private DataHandler _dataHandler;
    private SceneController _sceneController;
    private MainData _tempData;

    private bool IsEmptyData => _tempData == null || _tempData.EmptyData;

    [Inject]
    private void Construct(
        DataHandler dataHandler,
        SceneController sceneController)
    {
        _dataHandler = dataHandler;
        _sceneController = sceneController;
    }

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
        _dataHandler.SetData(new MainData());
        _signalBus.Fire(new SetNextDialogSignal(_startDialog));
        _sceneController.LoadLevelByEnum(EnumLevels.DialogsHub);
    }

    private void LoadGame()
    {
        _dataHandler.SetData(_tempData);
        _sceneController.LoadLevelByEnum((EnumLevels)_tempData.progressHistory.CurrentScene);
    }

    private void Settings()
    {
        _signalBus.Fire(new SetWindowSignal(_settingWindow));
    }

    private void About()
    {

    }

    public override void ExitWindow()
    {
        base.ExitWindow();

        _newGameBTN.onClick.RemoveAllListeners();
        _loadGameBTN.onClick.RemoveAllListeners();
        _settingsBTN.onClick.RemoveAllListeners();
        _aboutBTN.onClick.RemoveAllListeners();
    }
}