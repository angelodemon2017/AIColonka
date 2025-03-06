using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class DialogEvent : MonoBehaviour
{
    [SerializeField] private List<KeyPresentAndEvent> eventBies;

    private void Awake()
    {
        PanelDialogWithPeople.ActionByKey += CheckEvent;
    }

    public void CheckEvent(string key)
    {
        eventBies.FirstOrDefault(e => e.variant.KeyVariant == key)?.UE?.Invoke();
    }

    private void OnDrawGizmos()
    {
        DrawGizmosHelper.DrawLabel(transform, 3f, "DialogEvent");
    }

    private void OnDestroy()
    {
        PanelDialogWithPeople.ActionByKey -= CheckEvent;
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