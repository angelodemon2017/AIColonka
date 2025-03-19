using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class SceneLevelLoader : MonoBehaviour
{
    private SignalBus _signalBus;

    public static Action<float> LoadProgress;

    [Inject]
    private void Construct(
        SignalBus signalBus)
    {
        _signalBus = signalBus;

        Init();
    }

    private void Init()
    {
        _signalBus.Subscribe<SetLevelSignal>(LoadLevelBySignal);
    }

    private void LoadLevelBySignal(SetLevelSignal setLevelSignal)
    {
        LoadLevel(setLevelSignal.Level);
    }

    public void LoadLevel(EnumLevels level)
    {
        StartCoroutine(LoadLevelCoroutine(level));
    }

    public void RestartLevel()
    {
        LoadLevel(ControllerDemoSaveFile.Instance.CurrentLevel);
    }

    private void OnDrawGizmos()
    {
        DrawGizmosHelper.DrawLabel(transform, 3f, $"Level loader");
    }

    IEnumerator LoadLevelCoroutine(EnumLevels level)
    {
        ControllerDemoSaveFile.Instance.SetLevel(level);
        while (!ControllerDemoSaveFile.Instance.IsBlackEnd)
        {
            yield return null;
        }
        AsyncOperation operation = SceneManager.LoadSceneAsync((int)level + 1);
        EventBus.ResetSubs();
        LoadProgress?.Invoke(operation.progress);
    }

    private void OnDestroy()
    {
        _signalBus.Unsubscribe<SetLevelSignal>(LoadLevelBySignal);
    }
}