using UnityEngine;

public class DialogSetter : MonoBehaviour
{
    public void SetCurrentDialog(DialogSO dialog)
    {
        ControllerDemoSaveFile.Instance.CurrentDialog = dialog;
    }

    public void SetRoomConfig(int idConfig)
    {
        ControllerDemoSaveFile.Instance.mainData.progressHistory.RoomConfig = idConfig;
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
        ControllerDemoSaveFile.Instance.backTalk.SetTalkAsync(keyTalk, time);
    }
}