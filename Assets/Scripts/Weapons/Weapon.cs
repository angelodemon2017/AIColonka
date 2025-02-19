using UnityEngine;

[RequireComponent(typeof(WhoIs))]
public class Weapon : MonoBehaviour
{
    [SerializeField] protected Damage _damage;
    [SerializeField] protected WhoIs WhoIs;
    protected Transform _avTransform;
    protected Transform _target;
    protected Quaternion _rotate;

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
    }

    internal virtual void StartAttack()
    {

    }

    public virtual void TakeCollision(WhoIs whoIs)
    {

    }
}