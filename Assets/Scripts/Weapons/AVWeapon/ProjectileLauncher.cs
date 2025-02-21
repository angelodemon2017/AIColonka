using System.Collections;
using UnityEngine;

public class ProjectileLauncher : AVWeapon
{
    [SerializeField] private AVWeapon _projectile;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _beforeShot = 0.5f;
    [SerializeField] private float _afterShot = 0.5f;

    protected override void Shoot()
    {
        base.Shoot();

        StartCoroutine(Launch());
    }

    IEnumerator Launch()
    {
        yield return new WaitForSeconds(_beforeShot);
        var tempProj = Instantiate(_projectile, _spawnPoint.position, _spawnPoint.rotation);
        tempProj.InitAVW(_levelAVW, _damage);
        tempProj.Init(WhoIs.whoIs, transform, _target, transform.rotation);
        yield return new WaitForSeconds(_afterShot);
        Destroy(gameObject);
    }
}