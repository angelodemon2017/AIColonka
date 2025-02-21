using System.Collections.Generic;
using UnityEngine;

public class AVWeaponSpawner : AVWeapon
{
    [SerializeField] private PeriodicActivator _periodicActivator;
    [SerializeField] private List<Transform> _points;
    [SerializeField] private List<AVWeapon> _weaponVariants;
    [SerializeField] private int _countSpawn;
    [SerializeField] private float _intervalSpawn;

    internal override void StartAttack()
    {
        transform.position = _target.position;

        _periodicActivator.InitAndStart(SpawnAndShoot, _countSpawn, _intervalSpawn, EndSpawning);
    }

    private void SpawnAndShoot(int count)
    {
        var rp = _points.GetRandom();
        var avw = Instantiate(_weaponVariants.GetRandom(), rp.position, transform.rotation);
        avw.InitAVW(_levelAVW, _damage, true);
        avw.Init(WhoIs.whoIs, rp, _target);
    }

    private void EndSpawning()
    {
        Destroy(gameObject);
    }
}