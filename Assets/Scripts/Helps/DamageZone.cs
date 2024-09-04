using UnityEngine;

public class DamageZone : MonoBehaviour
{
    [SerializeField]private MeshRenderer _meshRenderer;
    private Damage _damage;

    public void Init(Damage damage)
    {
        _damage = damage;
    }

    private void Awake()
    {
        _meshRenderer.enabled = false;
        Destroy(gameObject, 0.1f);
    }
}