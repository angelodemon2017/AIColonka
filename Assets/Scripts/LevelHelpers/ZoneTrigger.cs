using UnityEngine;
using UnityEngine.Events;

public class ZoneTrigger : MonoBehaviour
{
    [SerializeField] private TaskSO _needTask;
    [SerializeField] private UnityEvent _unityEvent;
    [SerializeField] private UnityEvent<GameObject> _enterObject;

    private void Awake()
    {
//        ControllerDemoSaveFile.Instance.mainData.progressHistory.TaskUpdate += CheckAvailable;
        CheckAvailable();
    }

    public void CheckAvailable()
    {
/*        if (_needTask == null)
        {
            ControllerDemoSaveFile.Instance.mainData.progressHistory.TaskUpdate -= CheckAvailable;
            return;
        }
        else
        {
            gameObject.SetActive(ControllerDemoSaveFile.Instance.IsCurrentTask(_needTask));
        }/**/
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == Dicts.SpecNames.Player)
        //??if (other.tag == Dicts.Tags.Player)
        {
            RunScript();
            _enterObject?.Invoke(other.gameObject);
        }
    }

    private void RunScript()
    {
        _unityEvent?.Invoke();
    }

    private void OnDestroy()
    {
//        ControllerDemoSaveFile.Instance.mainData.progressHistory.TaskUpdate -= CheckAvailable;
    }
}