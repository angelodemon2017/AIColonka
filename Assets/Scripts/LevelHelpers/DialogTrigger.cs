using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    [SerializeField] private PanelDialogWithPeople _dialogWindow;
    [SerializeField] private ScriptScene scriptScene;
    [SerializeField] private DialogSO dialog;

    private PanelDialogWithPeople tempWindow;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Dicts.Tags.Player)
        {
            RunScript();
        }
    }

    private void RunScript()
    {
        ControllerDemoSaveFile.Instance.CurrentDialog = dialog;
//            mainData.progressHistory.Dialog = IdDialog;
        tempWindow = (PanelDialogWithPeople)UIFSM.Instance.OpenWindow(_dialogWindow);
        tempWindow.EndDialog += EndDialog;
        if (scriptScene != null)
        {
            tempWindow.NextStep += scriptScene.RunNextStep;
            scriptScene.RunScene();
        }
    }

    private void EndDialog()
    {
        tempWindow.EndDialog -= EndDialog;
        if (scriptScene != null)
        {
            tempWindow.NextStep -= scriptScene.RunNextStep;
            scriptScene.EndScript();
        }
        tempWindow = null;
    }
}