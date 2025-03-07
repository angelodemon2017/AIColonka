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

    internal async Task InitAsync(EnumLevels numLevel, bool isTargetLevel, Action<int> callBack)
    {
        _selfButton.onClick.AddListener(OnClickButton);
        _sceneId = (int)numLevel;
        ClickAction += callBack;

        string labelText = await Localizations.GetLocalizedText(
            Localizations.Tables.LevelsTable,
            Localizations.Levels.MapLevelKeys[numLevel]);
        if (isTargetLevel)
        {
            labelText = $"(!){labelText}";
        }
        _labelPresent.text = $"{labelText}";
    }

    private void OnClickButton()
    {
        ClickAction?.Invoke(_sceneId);
    }
}