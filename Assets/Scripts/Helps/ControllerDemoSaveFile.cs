using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Zenject;

public class ControllerDemoSaveFile : MonoBehaviour
{
    public static ControllerDemoSaveFile Instance;

    [SerializeField] private Image _blackImage;
    [SerializeField] private TextMeshProUGUI _testLoading;

    [Inject] private DataHandler _dataHandler;

    public EnumLevels CurrentLevel;

    private Color _transColor;

    [SerializeField] private Settings _settings = new Settings();

    internal Settings Settings => _settings;
    public bool IsDebug => _settings.IsDebug;
    public bool IsBlackEnd => _blackImage.color.a >= 1f;

    private void Awake()
    {
        if (!Instance)
        {
            _transColor = _blackImage.color;
            DontDestroyOnLoad(gameObject);
            Instance = this;
            _settings = SaveController.Load<Settings>(Settings.Prefix);
            SceneLevelLoader.LoadProgress += UpdateLoading;
        }
    }

    internal void SetLevel(EnumLevels level)
    {
        SetBlack(true);
        CurrentLevel = level;
        if (level != EnumLevels.MainMenu)
        {
            _dataHandler.CurrentData.SetLevel(level);
        }
    }

    private void UpdateLoading(float progres)
    {
        _testLoading.text = progres == 0 ? string.Empty : $"Load:{progres}";
    }

    internal void SetBlack(bool fadeIn)
    {
        Color targetColor = _transColor;
        targetColor.a = fadeIn ? 1f : 0f;

        DOTween.To(() => _transColor, x => _blackImage.color = x, targetColor, 1f);
    }
}
/*
LevelLoader:
1.  change UIState
2.  fadeIn display
3.  selected level
4.  sceneManager.LoadSceneAsync
5.  control save
6.  initScene:
7.  spawn player
8.  init Gameplay 
9.  fadeOut display
10. launch process
/**/