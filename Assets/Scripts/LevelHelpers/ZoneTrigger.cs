using UnityEngine;
using UnityEngine.Events;

public class ZoneTrigger : MonoBehaviour
{
    [SerializeField] private UnityEvent _unityEvent;
    [SerializeField] private UnityEvent<GameObject> _enterObject;

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == Dicts.SpecNames.Player)
        {
            RunScript();
            _enterObject?.Invoke(other.gameObject);
        }
    }

    private void RunScript()
    {
        _unityEvent?.Invoke();
    }
}