using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogVariant : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _textVariant;
    [SerializeField] private Button _buttonSelf;

//    private DialogVariantSO tempVart;
    private int _numVariant;

    public Action<int> ClickAction;

    internal void Init(int dialogVariant, string descr, Action<int> callBack)
    {
        _numVariant = dialogVariant;
//           tempVart = dialogVariant;
        ClickAction += callBack;
//        _textVariant.text = dialogVariant.TextVariant;
        _textVariant.text = descr;
        _buttonSelf.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        ClickAction?.Invoke(_numVariant);
/*        if (tempVart.specEndDialog.moveToLevel != EnumLevels.MainMenu)
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
            ClickAction?.Invoke(tempVart);
//            EventBus.Publish(tempVart);
        }/**/
    }

    private void OnDestroy()
    {
        ClickAction = null;
    }
}