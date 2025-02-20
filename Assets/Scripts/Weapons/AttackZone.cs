using System.Collections.Generic;
using UnityEngine;

public class AttackZone : Weapon
{
    [SerializeField] private SphereCollider _sphereCollider;
    [SerializeField] private float _size = 1f;
    [SerializeField] private bool _multHit;
    private float _timeOut = 1f;

    private HashSet<WhoIs> _wasAttacked = new();
    
    internal void SetAttackZone(float size, float timeOut = 0.5f)
    {
        _size = size;
        _timeOut = timeOut;
    }

    internal override void StartAttack()
    {
        base.StartAttack();

        _sphereCollider.radius = _size;

        Destroy(gameObject, _timeOut);
    }

    public override void TakeCollision(WhoIs whoIs)
    {
        base.TakeCollision(whoIs);

        if (_wasAttacked.Add(whoIs) || _multHit)
        {
            whoIs.TakeDamage(_damage);
        }
    }
}