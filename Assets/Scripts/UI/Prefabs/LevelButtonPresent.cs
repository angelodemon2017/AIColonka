using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Threading.Tasks;

public class LevelButtonPresent : MonoBehaviour
{
    [SerializeField] private Image _iconPresent;
    [SerializeField] private TextMeshProUGUI _labelPresent;
    [SerializeField] private Button _selfButton;
    [SerializeField] private Image progress;

    private int _sceneId;

    private Action<int> ClickAction;

    internal async Task InitAsync(EnumLevels numLevel, Action<int> callBack)
    {
        _selfButton.onClick.AddListener(OnClickButton);
        _sceneId = (int)numLevel;
        ClickAction += callBack;

        _labelPresent.text = await Localizations.GetLocalizedText(
            Localizations.Tables.LevelsTable,
            Localizations.Levels.MapLevelKeys[numLevel]);
    }

    private void OnClickButton()
    {
        ClickAction?.Invoke(_sceneId);
    }
}