using UnityEngine;
using Zenject;

public class DialogSetter : MonoBehaviour
{
//    [Inject] 
    private SignalBus _signalBus;
    [SerializeField] private EnumDialogRoomPreset dialogRoomPreset;

    [Inject]
    private void Construct(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    public void SetDialogPreset()
    {
        SetRoomConfig(dialogRoomPreset);
    }

    public void SetCurrentDialog(DialogSO dialog)
    {
        ControllerDemoSaveFile.Instance.CurrentDialog = dialog;
    }

    public void SetRoomConfig(EnumDialogRoomPreset idConfig)
    {
        ControllerDemoSaveFile.Instance.mainData.progressHistory.RoomConfig = (int)idConfig;
    }

    public void SetTask(TaskSO task)
    {//TODO call from files without injected
        _signalBus?.Fire(new SetTaskSignal(task));
        ControllerDemoSaveFile.Instance.SetTask(task);
    }

    public void SetWindow(MAINWindow window)
    {
        UIFSM.Instance.OpenWindow(window);
    }

    public void CallBackGroundTalk(string keyTalk, float time)
    {
        _signalBus?.Fire(new BackTalkSignal(keyTalk, time, Localizations.Tables.BackTalksTable));
//        _ = ControllerDemoSaveFile.Instance.backTalk.SetTalkAsync(keyTalk, time, Localizations.Tables.BackTalksTable);
    }

    private void OnDrawGizmos()
    {
        DrawGizmosHelper.DrawLabel(transform, 2.5f, "DialogSetter");
    }

    public void DashPlayerTo(Mover mp)
    {

    }
}