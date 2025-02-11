using UnityEngine;

public class UIFSM : MonoBehaviour, IUIFSM
{
    private IWindowFSM _currentWindow;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            EventBus.Publish(new EventKey(KeyCode.L));
        }

    }

    private void CallUIEvent(EnumUIEvent uIEvent)
    {
        _currentWindow.PressedKey(uIEvent);
    }
}