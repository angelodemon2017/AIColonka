using UnityEngine;

public class AVWBallistic : AVWPeriodic
{
    protected override void Shoot()
    {
        transform.rotation = Quaternion.LookRotation(Vector3.up);
        base.Shoot();
    }

    private void FixedUpdate()
    {
        transform.LookAt(_target);
    }
}