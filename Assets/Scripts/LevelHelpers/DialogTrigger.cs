using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    [SerializeField] private PanelDialogWithPeople _dialogWindow;
    [SerializeField] private ScriptScene scriptScene;

    private void OnTriggerEnter(Collider other)
    {
        RunScript();
    }

    private void RunScript()
    {
        ControllerDemoSaveFile.Instance.mainData.progressHistory.Dialog = 1;
        UIFSM.Instance.OpenWindow(_dialogWindow);
        if (scriptScene != null)
        {
            scriptScene.RunScene();
        }
    }
}