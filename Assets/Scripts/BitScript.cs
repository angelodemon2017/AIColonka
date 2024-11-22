using UnityEngine;

public class BitScript : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private DamageZone _damageZone;
    private Damage _damage;

    private void Awake()
    {
        Destroy(gameObject, 5f);
    }

    public void Init(Vector3 puls, Damage damage)
    {
        _rigidbody.AddForce(puls);
        _damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        SpawnDamage();
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        SpawnDamage();
        Destroy(gameObject);
    }

    private void SpawnDamage()
    {
        var dz = Instantiate(_damageZone);
        dz.transform.position = transform.position;
        dz.Init(_damage);
    }
}