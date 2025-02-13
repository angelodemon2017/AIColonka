using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RoomMapping : MonoBehaviour
{
    [SerializeField] private List<RoomConfig> _roomConfigs;
    [SerializeField] private GameObject _activateGO;

    private void Awake()
    {
        if (ControllerDemoSaveFile.Instance.mainData.progressHistory.RoomConfig < _roomConfigs.Count)
        {
            RunScene(_roomConfigs[ControllerDemoSaveFile.Instance.mainData.progressHistory.RoomConfig]);
        }
    }

    private void RunScene(RoomConfig roomConfig)
    {
        roomConfig.unityEvent?.Invoke();
        _activateGO.SetActive(true);
    }
}

[System.Serializable]
public class RoomConfig
{
    public string Name;
    public UnityEvent unityEvent;
}