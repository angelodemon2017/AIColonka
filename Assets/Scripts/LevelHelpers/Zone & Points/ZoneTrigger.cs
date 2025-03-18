using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class ZoneTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent _unityEvent;
    [SerializeField] private UnityEvent<GameObject> _enterObject;
    [SerializeField] private SignalAgregator _signalAgregator;

    [Inject] private SignalBus _signalBus;

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == Dicts.SpecNames.Player)
        {
            RunScript();
            _enterObject?.Invoke(other.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        DrawGizmosHelper.DrawLabel(transform, 1f, "ZoneTrigger");
    }

    private void RunScript()
    {
        _signalAgregator.FireAll(_signalBus);
//        _unityEvent?.Invoke();
    }
}