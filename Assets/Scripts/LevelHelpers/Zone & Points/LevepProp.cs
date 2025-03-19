using UnityEngine;
using UnityEngine.Events;
using Zenject;

public class LevepProp : MonoBehaviour
{
    [SerializeField] private EnumLevelProp _levelProp;
    [SerializeField] private UnityEvent _eventWasPick;

    private DataHandler _dataHandler;

    [Inject]
    private void Construct(
        DataHandler dataHandler)
    {
        _dataHandler = dataHandler;

        Init();
    }

    private void Init()
    {
        CheckPick();
    }

    private void CheckPick()
    {
        if (_dataHandler.CurrentData.WasPick(_levelProp))
        {
            Destroy(gameObject);
            _eventWasPick?.Invoke();
        }
    }

    private void OnDrawGizmos()
    {
        DrawGizmosHelper.DrawLabel(transform, 1.5f, $"LevepProp:{_levelProp}");
    }

    public void PickUpProp()
    {
        _dataHandler.PickProp(_levelProp);
        Destroy(gameObject);
    }
}