using System.Collections;
using UnityEngine;

public class ProjectileLauncher : Weapon
{
    [SerializeField] private Projectile _projectile;
    [SerializeField] private Transform _spawnPoint;

    internal override void StartAttack()
    {
        StartCoroutine(Launch());
    }

    IEnumerator Launch()
    {
        yield return new WaitForSeconds(0.5f);
        var tempProj = Instantiate(_projectile, _spawnPoint.position, _spawnPoint.rotation);
        tempProj.Init(WhoIs.whoIs);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}