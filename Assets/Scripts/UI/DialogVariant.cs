using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogVariant : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textVariant;
    [SerializeField] private Button _buttonSelf;

    private DialogVariantSO tempVart;

    internal void Init(DialogVariantSO dialogVariant)
    {
        tempVart = dialogVariant;

        _textVariant.text = dialogVariant.TextVariant;
        _buttonSelf.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (tempVart.specEndDialog.moveToLevel != EnumLevels.MainMenu)
        {
            // is trash, need screen loader
            EventBus.ResetSubs();
            SceneManager.LoadSceneAsync((int)tempVart.specEndDialog.moveToLevel);
        }
        else if (tempVart.specEndDialog.moveWindow != null)
        {
            UIFSM.Instance.OpenWindow(tempVart.specEndDialog.moveWindow);
        }
        else
        {
            EventBus.Publish(tempVart);
        }
    }
}

public struct DialogSelect : IEvent
{
    public int IdVariant;
}