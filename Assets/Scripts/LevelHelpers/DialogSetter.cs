using UnityEngine;

public class DialogSetter : MonoBehaviour
{
    public void SetCurrentDialog(DialogSO dialog)
    {
        Debug.LogWarning($"SetCurrentDialog:{dialog.name}");
        ControllerDemoSaveFile.Instance.CurrentDialog = dialog;
    }

    public void SetRoomConfig(int idConfig)
    {
        ControllerDemoSaveFile.Instance.mainData.progressHistory.RoomConfig = idConfig;
    }

    public void SetTask(int idTask)
    {
        ControllerDemoSaveFile.Instance.mainData.progressHistory.SetTask(idTask);
    }
}