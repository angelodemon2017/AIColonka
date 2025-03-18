using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class RoomMapping : MonoBehaviour
{
    [SerializeField] private List<DialogRoomMap> _dialogRoomMaps;
    [SerializeField] private DialogRoomMap _defaultRoomConfig;
    [SerializeField] private PanelDialogWithPeople _panelDialogWithPeople;
    [SerializeField] private UIFSM _uiFSM;

    [Inject] private DataHandler _dataHandler;
    [Inject] private SignalBus _signalBus;

    [Inject]
    private void Construct(
        DataHandler dataHandler,
        SignalBus signalBus)
    {
        _dataHandler = dataHandler;
        _signalBus = signalBus;

        Init();
    }

    private void Init()
    {
        var currentDRM = _dialogRoomMaps.FirstOrDefault(r => r.KeyDialog == _dataHandler.CurrentDialog);
        if (currentDRM == null)
        {
            currentDRM = _defaultRoomConfig;
        }
        RunRoom(currentDRM);
    }

    private void RunRoom(DialogRoomMap dialogRoomMap)
    {
        dialogRoomMap.unityEvent?.Invoke();
        dialogRoomMap._signalAgregator.FireAll(_signalBus);
        _panelDialogWithPeople.gameObject.SetActive(true);
        _uiFSM.StartWindow();
    }
}

[System.Serializable]
public class DialogRoomMap
{
    public string Name;
    public DialogSO KeyDialog;
    public UnityEvent unityEvent;
    public SignalAgregator _signalAgregator;
}