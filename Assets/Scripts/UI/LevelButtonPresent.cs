using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButtonPresent : MonoBehaviour
{
    [SerializeField] private Image _iconPresent;
    [SerializeField] private TextMeshProUGUI _labelPresent;
    [SerializeField] private Button _selfButton;

    private int _level;

    internal void Init(int numLevel)
    {
        _selfButton.onClick.AddListener(OnClickButton);
        _level = numLevel;
    }

    private void OnClickButton()
    {

    }
}