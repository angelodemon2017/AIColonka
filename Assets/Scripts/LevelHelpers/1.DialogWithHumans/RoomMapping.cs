using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class RoomMapping : MonoBehaviour
{
    [SerializeField] private List<RoomConfig> _roomConfigs;
    [SerializeField] private PanelDialogWithPeople _panelDialogWithPeople;
    [SerializeField] private UIFSM _uiFSM;

    private void Awake()
    {
        if (ControllerDemoSaveFile.Instance.mainData.progressHistory.RoomConfig < _roomConfigs.Count)
        {
            RunScene(_roomConfigs.FirstOrDefault(r => (int)r.DialogRoomPreset == ControllerDemoSaveFile.Instance.mainData.progressHistory.RoomConfig));
        }
    }

    private void RunScene(RoomConfig roomConfig)
    {
        roomConfig.unityEvent?.Invoke();
        _panelDialogWithPeople.gameObject.SetActive(true);
        _uiFSM.StartWindow();
    }
}

[System.Serializable]
public class RoomConfig
{
    public string Name;
    public EnumDialogRoomPreset DialogRoomPreset;
    public UnityEvent unityEvent;
}