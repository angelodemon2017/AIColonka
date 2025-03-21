using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class DialogEvent : MonoBehaviour
{
    [SerializeField] private List<KeyPresentAndEvent> eventBies;

    private SignalBus _signalBus;

    [Inject]
    private void Construct(
        SignalBus signalBus)
    {
        _signalBus = signalBus;

        _signalBus.Subscribe<DialogTriggerKeySignal>(CheckTrigger);
    }

    private void CheckTrigger(DialogTriggerKeySignal dialogTriggerKeySignal)
    {
        var eb = eventBies.FirstOrDefault(e => e.variant.KeyVariant == dialogTriggerKeySignal.TriggerKey);
        if (eb != null)
        {
            eb.UE?.Invoke();
            eb.signalAgregator.FireAll(_signalBus);
        }
    }

    private void OnDrawGizmos()
    {
        DrawGizmosHelper.DrawLabel(transform, 3f, "DialogEvent");
    }

    private void OnDestroy()
    {
        _signalBus.Unsubscribe<DialogTriggerKeySignal>(CheckTrigger);
    }
}

[System.Serializable]
public class KeyPresentAndEvent
{
    public EventByVariant variant = new();
    public UnityEvent UE;
    public SignalAgregator signalAgregator;
}

[System.Serializable]
public class EventByVariant
{
    public string KeyVariant;
}