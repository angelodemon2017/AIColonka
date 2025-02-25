using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogVariant : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _numLabel;
    [SerializeField] private TextMeshProUGUI _textVariant;
    [SerializeField] private Button _buttonSelf;

    private int _numVariant;

    public Action<int> ClickAction;

    internal void Init(int dialogVariant, string descr, float speed, Action<int> callBack)
    {
        _numLabel.transform.parent.gameObject.SetActive(dialogVariant >= 0);
        _numLabel.text = $"{dialogVariant + 1}";
        _textVariant.text = string.Empty;
        _numVariant = dialogVariant < 0 ? 0 : dialogVariant;
        ClickAction += callBack;
        _buttonSelf.onClick.AddListener(OnClick);
        StartCoroutine(Writer(descr, 0, speed, _numVariant));
    }

    IEnumerator Writer(string fullText, int chars, float speed, float order = 0)
    {
        yield return new WaitForSeconds(order * 0.5f + speed);
        string space = string.Empty;
        for (int i = 0; i < fullText.Length - chars; i++)
        {
            space += " ";
        }
        _textVariant.text = fullText.Substring(0, chars) + space;
        if (chars < fullText.Length)
        {
            StartCoroutine(Writer(fullText, chars + 1, speed));
        }
    }

    private void OnClick()
    {
        ClickAction?.Invoke(_numVariant);
    }

    private void OnDestroy()
    {
        ClickAction = null;
    }
}