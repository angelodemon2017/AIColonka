using UnityEngine;

public class VirtualObject : MonoBehaviour
{
    [SerializeField] private ConditionOfVirtualObject _condition;

    public void ExecuteChange()
    {
        //TODO add vfx
        Instantiate(_condition.Prefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}