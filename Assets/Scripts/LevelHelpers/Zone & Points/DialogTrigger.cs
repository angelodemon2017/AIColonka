using UnityEngine;
using Zenject;

public class DialogTrigger : MonoBehaviour
{
    [SerializeField] private PanelDialogWithPeople _dialogWindow;
    [SerializeField] private ScriptScene scriptScene;
    [SerializeField] private DialogSO dialog;

    private SignalBus _signalBus;
    private UIFSM _uifsm;

    private PanelDialogWithPeople tempWindow;

    [Inject]
    private void Construct(
        SignalBus signalBus,
        UIFSM uifsm)
    {
        _signalBus = signalBus;
        _uifsm = uifsm;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Dicts.Tags.Player)
        {
            RunScript();
        }
    }

    public void RunScript()
    {
        _signalBus.Fire(new SetNextDialogSignal(dialog));
        tempWindow = (PanelDialogWithPeople)_uifsm.OpenWindow(_dialogWindow);
        tempWindow.EndDialog += EndDialog;
        if (scriptScene != null)
        {
            tempWindow.NextStep += scriptScene.RunNextStep;
            scriptScene.RunScene();
        }
    }

    private void OnDrawGizmos()
    {
        DrawGizmosHelper.DrawLabel(transform, 3.5f, $"DialogTrigger{(dialog == null ? "" : $": {dialog.name}")}");
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