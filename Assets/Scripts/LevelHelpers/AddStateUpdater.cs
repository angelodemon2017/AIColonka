using UnityEngine;

public class AddStateUpdater : MonoBehaviour
{
    [SerializeField] private bool _isSlow;

    public void SetAddState(GameObject gameObject)
    {
        var pfsm = gameObject.GetComponent<PlayerFSM>();
        pfsm.SetSlowState(_isSlow);
    }
}