using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PanelHP : MonoBehaviour
{
    [SerializeField] private Image _hpImage;
    [SerializeField] private Image _tempHPpImage;
    [SerializeField] private TextMeshProUGUI _textHP;

    private float _lastHP;
    private float _tempHP;
    private float _currentHP;
    private float _maxHP;

    private float _timeShow;

    internal void UpdateHP(float lastHP, float currentHP, float max)
    {
        _timeShow = 2f;
        _lastHP = lastHP;
        _currentHP = currentHP;
        _maxHP = max;

        UpdateUI();
    }

    internal void UpdateUI()
    {
        _textHP.text = $"{(int)_currentHP}/{(int)_maxHP} HP";
        _tempHPpImage.fillAmount = _tempHP / _maxHP;
        _hpImage.fillAmount = _currentHP / _maxHP;
    }

    private void FixedUpdate()
    {
        if (_timeShow > 0)
        {
            _timeShow -= Time.fixedDeltaTime;
            if (_timeShow < 1f)
            {
                _tempHP = Mathf.Lerp(_lastHP, _currentHP, 1f - _timeShow);
                UpdateUI();
                if (_timeShow < 0f)
                {
                    _tempHP = _currentHP;
                }
            }
        }
    }
}