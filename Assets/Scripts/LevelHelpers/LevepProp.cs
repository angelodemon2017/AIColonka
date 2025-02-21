using UnityEngine;

public class LevepProp : MonoBehaviour
{
    [SerializeField] private EnumLevelProp _levelProp;

    private void Awake()
    {
        CheckPick();
    }

    private void CheckPick()
    {
        if (ControllerDemoSaveFile.Instance.mainData.WasPick(_levelProp))
        {
            Destroy(gameObject);
        }
    }

    public void PickUpProp()
    {
        ControllerDemoSaveFile.Instance.mainData.PickProp(_levelProp);
        Destroy(gameObject);
    }
}