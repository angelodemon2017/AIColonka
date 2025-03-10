using UnityEngine;

public class DialogSetter : MonoBehaviour
{
    [SerializeField] private EnumDialogRoomPreset dialogRoomPreset;

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
    {
        ControllerDemoSaveFile.Instance.SetTask(task);
    }

    public void SetWindow(MAINWindow window)
    {
        UIFSM.Instance.OpenWindow(window);
    }

    public void CallBackGroundTalk(string keyTalk, float time)
    {
        _ = ControllerDemoSaveFile.Instance.backTalk.SetTalkAsync(keyTalk, time, Localizations.Tables.BackTalksTable);
    }

    private void OnDrawGizmos()
    {
        DrawGizmosHelper.DrawLabel(transform, 2.5f, "DialogSetter");
    }
}