using UnityEngine;

public class VirtualObjectChecker : MonoBehaviour
{
    [SerializeField] private GameObject _uiLabelHint;
    [SerializeField] private VirtualObject _virtualObjectTemp;
    private bool _isVirtualObjectNear => _virtualObjectTemp != null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Dicts.Tags.VirtualObject)
        {
            Debug.Log($"OnTriggerExit={other.name}");
            _virtualObjectTemp = other.GetComponent<VirtualObject>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == Dicts.Tags.VirtualObject)
        {
            Debug.Log($"OnTriggerExit={other.name}");
            _virtualObjectTemp = null;
        }
    }

    private void Update()
    {
//        _uiLabelHint.SetActive(_isVirtualObjectNear);
        if (Input.GetKeyDown(KeyCode.R) &&
            _isVirtualObjectNear)
        {
            _virtualObjectTemp.ExecuteChange();
        }
    }
}