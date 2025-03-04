using System.Collections.Generic;
using UnityEngine;

public class AttackZone : Weapon
{
    [SerializeField] private SphereCollider _sphereCollider;
    [SerializeField] private float _size = 1f;
    [SerializeField] private bool _multHit;
    [SerializeField] private float _timeOut = 1f;

    private HashSet<WhoIs> _wasAttacked = new();

    protected override Vector3 _hitPosition => _sphereCollider.transform.position;
    protected override Quaternion _hitRotate => _sphereCollider.transform.rotation;

    internal void SetAttackZone(float size, float timeOut = 0.5f)
    {
        _size = size;
        _timeOut = timeOut;
    }

    internal override void StartAttack()
    {
        base.StartAttack();

        transform.localScale = Vector3.one * _size;

        Destroy(gameObject, _timeOut);
    }

    public override void TakeCollision(WhoIs whoIs)
    {
        base.TakeCollision(whoIs);

        ShowHit();
        if (_wasAttacked.Add(whoIs) || _multHit)
        {
            whoIs.TakeDamage(_damage);
        }
    }

    public void DestroyBullet(WhoIs whoIs)
    {
        Destroy(whoIs.gameObject);
    }
}