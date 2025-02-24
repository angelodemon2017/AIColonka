using UnityEngine;

public class DialogSetter : MonoBehaviour
{
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
        ControllerDemoSaveFile.Instance.mainData.SetTask(task);
    }

    public void SetTask(int idTask)
    {
        ControllerDemoSaveFile.Instance.mainData.SetTask(idTask);
    }

    public void SetWindow(MAINWindow window)
    {
        UIFSM.Instance.OpenWindow(window);
    }

    public void CallBackGroundTalk(string keyTalk, float time)
    {
        _ = ControllerDemoSaveFile.Instance.backTalk.SetTalkAsync(keyTalk, time);
    }
}