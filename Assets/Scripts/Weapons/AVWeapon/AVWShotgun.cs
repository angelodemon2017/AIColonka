using System.Collections;
using UnityEngine;

public class AVWShotgun : AVWeapon
{
    protected override void Shoot()
    {
        base.Shoot();

        StartCoroutine(Launch());
    }

    RaycastHit hit;
    Ray ray;
    IEnumerator Launch()
    {
        yield return new WaitForSeconds(0.2f);

        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}