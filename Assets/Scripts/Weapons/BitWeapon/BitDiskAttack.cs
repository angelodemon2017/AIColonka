using UnityEngine;

public class BitDiskAttack : FastAttackOrbit
{
    [SerializeField] private float _startSpeed;
    [SerializeField] private float _accelSpeed;
    [SerializeField] private Mover _mover;
    private Vector3 _direction;

    private bool _haveTarget;

    protected override float Yswift => 0f;
    protected override Transform spawnPoint => Points.PointOfLookCamera;

    internal override void StartAttack()
    {
        base.StartAttack();

        _haveTarget = _target;

        if (_haveTarget)
        {
            _mover.SetTarget(Points.TargetEnemy.transform);
        }
        else
        {
            _direction = _avTransform.forward.normalized;
        }
    }

    private void FixedUpdate()
    {
        if (!_haveTarget)
        {
            Fly();
        }
    }

    private void Fly()
    {
        _startSpeed += _accelSpeed;

        transform.position += _direction * _startSpeed * Time.deltaTime;
    }
}