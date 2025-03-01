using UnityEngine;

public class AVWPeriodic : AVWeapon
{
    [SerializeField] private PeriodicActivator _periodicActivator;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private AVWeapon _bullet;
    [SerializeField] private int _count;
    [SerializeField] private float _periodic;
    [SerializeField] private float _speed;

//    private float _directMove = 2f;

    protected override void Shoot()
    {
        base.Shoot();

        _periodicActivator.InitAndStart(SomeShoot, _count, _periodic, EndShooting);
        //        StartCoroutine(Launch());
    }

    private void SomeShoot(int order)
    {
        var bul = Instantiate(_bullet, _spawnPoint.position, _spawnPoint.rotation);
        bul.InitAVW(_levelAVW, _damage);
        bul.Init(WhoIs.whoIs, transform, _target, transform.rotation);
    }

    private void EndShooting()
    {
        Destroy(gameObject, 0.1f);
    }

/*    IEnumerator Launch()
    {
        yield return new WaitForSeconds(_beforeShot);
        var tempProj = Instantiate(_projectile, _spawnPoint.position, _spawnPoint.rotation);
        tempProj.InitAVW(_levelAVW, _damage);
        tempProj.Init(WhoIs.whoIs, transform, _target, transform.rotation);
        _directMove = -2f;
        yield return new WaitForSeconds(_afterShot);
        Destroy(gameObject);
    }/**/

    private void FixedUpdate()
    {
        transform.LookAt(_target);
//        transform.position += transform.forward * _directMove * _speed * Time.fixedDeltaTime;
    }
}