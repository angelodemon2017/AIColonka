using UnityEngine;

[RequireComponent(typeof(WhoIs))]
public class Weapon : MonoBehaviour
{
    [SerializeField] protected Damage _damage;
    [SerializeField] protected WhoIs WhoIs;
    [SerializeField] private GameObject hitPrefab;
    [SerializeField] private float _hitSize = 1f;
    protected Transform _avTransform;
    protected Transform _target;
    protected Quaternion _rotate;

    protected virtual float _hitTimeout => 0.1f;
    protected virtual Vector3 _hitPosition => transform.position;
    protected virtual Quaternion _hitRotate => Quaternion.identity;

    private void OnValidate()
    {
        WhoIs = GetComponent<WhoIs>();
    }

    internal void Init(EnumWhoIs whoIs, Transform AVtransform = null, Transform target = null, Quaternion rotation = new Quaternion())
    {
        WhoIs.whoIs = whoIs;
        _avTransform = AVtransform;
        _target = target;
        _rotate = rotation;

        StartAttack();
    }

    internal void SetDamage(Damage damage)
    {
        _damage = new Damage(damage.DamageType, damage.ValueDamage);
    }/**/

    internal virtual void StartAttack() { }

    public virtual void TakeCollision(WhoIs whoIs) { }

    public void ShowHit()
    {
        if (hitPrefab != null)
        {
            var hitVFX = Instantiate(hitPrefab, _hitPosition, _hitRotate);
            hitVFX.transform.localScale = Vector3.one * _hitSize;
            var psHit = hitVFX.GetComponent<ParticleSystem>();
            if (psHit != null)
            {
                Destroy(hitVFX, psHit.main.duration);
            }
            else
            {
                Destroy(hitVFX, _hitTimeout);
            }
        }
    }
}