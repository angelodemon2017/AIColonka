using UnityEngine;

public class AVWPeriodic : AVWeapon
{
    [SerializeField] private PeriodicActivator _periodicActivator;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private AVWeapon _bullet;
    [SerializeField] private int _count;
    [SerializeField] private float _periodic;
    [SerializeField] private float _speed;

    protected override void Shoot()
    {
        base.Shoot();
        InitAndStartShoot();
    }

    private void InitAndStartShoot()
    {
        _periodicActivator.InitAndStart(PeriodicShoot, _count, _periodic, EndShooting);
    }

    protected virtual void PeriodicShoot(int order)
    {
        var bul = Instantiate(_bullet, _spawnPoint.position, _spawnPoint.rotation);
        bul.InitAVW(_levelAVW, _damage);
        bul.Init(WhoIs.whoIs, transform, _target, transform.rotation);
    }

    private void EndShooting()
    {
        Destroy(gameObject, 0.1f);
    }

    private void FixedUpdate()
    {
        transform.LookAt(_target);
    }
}