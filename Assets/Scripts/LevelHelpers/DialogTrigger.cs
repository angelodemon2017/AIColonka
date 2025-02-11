using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    [SerializeField] private PanelDialogWithPeople _dialogWindow;

    private void OnTriggerEnter(Collider other)
    {
        ControllerDemoSaveFile.Instance.mainData.progressHistory.Dialog = 1;
        UIFSM.Instance.OpenWindow(_dialogWindow);
    }
}