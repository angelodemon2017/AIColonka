using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class SceneController : MonoBehaviour
{
    [SerializeField] private Image _blackImage;
    [SerializeField] private TextMeshProUGUI _testLoading;

    private Color _transColor;
    private SignalBus _signalBus;
    private DataHandler _dataHandler;

    [Inject]
    private void Construct(
        SignalBus signalBus,
        DataHandler dataHandler)
    {
        _signalBus = signalBus;
        _dataHandler = dataHandler;

        Init();
    }

    private void Init()
    {
//        _signalBus.Subscribe<SetLevelSignal>(LoadLevelBySignal);
//        _signalBus.Subscribe<RestartLevelSignal>(Restart);

        DontDestroyOnLoad(gameObject);

        _transColor = _blackImage.color;
        //LoadAsync(1);
        SceneManager.LoadSceneAsync(1);
        SetBlack(false, null);
    }

    private void LoadLevelBySignal(SetLevelSignal setLevelSignal)
    {
        if (setLevelSignal.Level != EnumLevels.MainMenu && setLevelSignal.Level != EnumLevels.DialogsHub)
        {
            _dataHandler.CurrentData.SetLevel(setLevelSignal.Level);
        }
        LoadScene((int)setLevelSignal.Level);
    }

    internal void LoadLevelByEnum(EnumLevels level)
    {
        if (level != EnumLevels.MainMenu && level != EnumLevels.DialogsHub)
        {
            _dataHandler.CurrentData.SetLevel(level);
        }
        LoadScene((int)level);
    }

    internal void Restart()
    {
        LoadScene(_dataHandler.CurrentData.progressHistory.CurrentScene);
    }

    private void LoadScene(int level)
    {
        SetBlack(true, () => StartCoroutine(LoadAsync(level + 1)));
    }

    internal void SetBlack(bool fadeIn, Action onDone)
    {
        Color targetColor = _transColor;
        targetColor.a = fadeIn ? 1f : 0f;

        DOTween.To(() => _transColor, x => _blackImage.color = x, targetColor, 1f)
            .OnComplete(() => 
            {
                onDone?.Invoke();
            });
    }

    private IEnumerator LoadAsync(int sceneIndex)
    {
        _testLoading.text = "0";
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            _testLoading.text = $"{progress * 100}";

            if (operation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(0f);
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
        _testLoading.text = string.Empty;
        SetBlack(false, null);
    }
}