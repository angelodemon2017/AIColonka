using UnityEngine;

public class DamageZone : MonoBehaviour
{
    [SerializeField]private MeshRenderer _meshRenderer;
    
    private void Awake()
    {
        _meshRenderer.enabled = false;
        Destroy(gameObject, 0.1f);
    }
}