using UnityEngine;
using UnityEngine.Events;

public class LevepProp : MonoBehaviour
{
    [SerializeField] private EnumLevelProp _levelProp;
    [SerializeField] private UnityEvent _eventWasPick;

    private void Awake()
    {
        CheckPick();
    }

    private void CheckPick()
    {
        if (ControllerDemoSaveFile.Instance.mainData.WasPick(_levelProp))
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
        ControllerDemoSaveFile.Instance.mainData.PickProp(_levelProp);
        Destroy(gameObject);
    }
}