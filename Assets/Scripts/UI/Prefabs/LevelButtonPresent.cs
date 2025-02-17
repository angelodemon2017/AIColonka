using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Localization.Settings;

public class LevelButtonPresent : MonoBehaviour
{
    [SerializeField] private Image _iconPresent;
    [SerializeField] private TextMeshProUGUI _labelPresent;
    [SerializeField] private Button _selfButton;
    [SerializeField] private Image progress;

    private int _sceneId;

    internal void Init(EnumLevels numLevel)
    {
        var textKey = Localizations.Levels.MapLevelKeys[numLevel];
        var op = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(
            Localizations.Levels.LevelsTable, textKey);
        if (op.IsDone)
            Debug.Log(op.Result);
        else
            op.Completed += (op) => 
            {
                _labelPresent.text = op.Result;
//                Debug.Log(op.Result); 
            };

        _selfButton.onClick.AddListener(OnClickButton);
//        _labelPresent.text = numLevel.ToString();
        _sceneId = (int)numLevel;
    }

    private void OnClickButton()
    {
        StartCoroutine(LoadLevel());
    }

    /// <summary>
    /// Move out method
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadLevel()
    {
        ControllerDemoSaveFile.Instance.CurrentLevel = (EnumLevels)_sceneId;
        AsyncOperation operation = SceneManager.LoadSceneAsync(_sceneId);
        EventBus.ResetSubs();
        progress.fillAmount = operation.progress;
        yield return null;
    }
}