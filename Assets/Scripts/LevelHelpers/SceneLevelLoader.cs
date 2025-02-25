using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLevelLoader : MonoBehaviour
{
    [SerializeField] private EnumLevels _selectedLevel;

    public static Action<float> LoadProgress;

    public void LoadLevel()
    {
        LoadLevel(_selectedLevel);
    }

    public void LoadLevel(EnumLevels level)
    {
        StartCoroutine(LoadLevelCoroutine(level));
    }

    public void RestartLevel()
    {
        LoadLevel(ControllerDemoSaveFile.Instance.CurrentLevel);
    }

    IEnumerator LoadLevelCoroutine(EnumLevels level)
    {
        ControllerDemoSaveFile.Instance.SetLevel(level);
        while (!ControllerDemoSaveFile.Instance.IsBlackEnd)
        {
            yield return null;
        }
        AsyncOperation operation = SceneManager.LoadSceneAsync((int)level);
        EventBus.ResetSubs();
        LoadProgress?.Invoke(operation.progress);
    }
}