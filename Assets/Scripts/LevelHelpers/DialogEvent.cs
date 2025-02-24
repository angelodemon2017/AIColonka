using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class DialogEvent : MonoBehaviour
{
    [SerializeField] private DialogSO dialog;
    [SerializeField] private List<KeyPresentAndEvent> eventBies;

    public void CheckEvent(string key)
    {
        eventBies.FirstOrDefault(e => e.variant.KeyVariant == key).UE?.Invoke();
    }
}


[System.Serializable]
public class KeyPresentAndEvent
{
    public EventByVariant variant = new();
    public UnityEvent UE;
}

[System.Serializable]
public class EventByVariant
{
    public string KeyVariant;
}