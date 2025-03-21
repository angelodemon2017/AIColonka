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
    private GameplayHandler _gameplayHandler;

    [Inject]
    private void Construct(
        DataHandler dataHandler,
        GameplayHandler gameplayHandler,
        SignalBus signalBus)
    {
        _dataHandler = dataHandler;
        _gameplayHandler = gameplayHandler;
        _signalBus = signalBus;

        Init();
    }

    private void Init()
    {
        DontDestroyOnLoad(gameObject);

        _transColor = _blackImage.color;

        SceneManager.LoadSceneAsync(1);
        SetBlack(false, null);
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
        _signalBus.Fire(new ExitFromSceneSignal());
        SetBlack(true, () => StartCoroutine(LoadAsync(level + 1)));
    }

    private void SetBlack(bool fadeIn, Action onDone)
    {
        Color targetColor = _transColor;
        targetColor.a = fadeIn ? 1f : 0f;

        DOTween.To(() => _transColor, x => _blackImage.color = x, targetColor, 1f)
//            .SetDelay(1f)
            .OnComplete(() =>
            {
                _transColor = _blackImage.color;
                onDone?.Invoke();
            });
    }

    private IEnumerator LoadAsync(int sceneIndex)
    {
        _testLoading.text = "0";
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        operation.allowSceneActivation = false;
        _gameplayHandler.LevelUpdate();

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

        _signalBus.Fire(new EnterToSceneSignal());
        SetBlack(false, null);
    }
}