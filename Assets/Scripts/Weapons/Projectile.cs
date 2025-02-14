using UnityEngine;

public class Projectile : Weapon
{
    [SerializeField] private float _startSpeed;
    [SerializeField] private float _accelSpeed;

    private Vector3 _direction;
    private float _timeOut = 10f;

    internal override void StartAttack()
    {
        _direction = transform.forward;

        Destroy(gameObject, _timeOut);
    }

    private void FixedUpdate()
    {
        Fly();
    }

    private void Fly()
    {
        _startSpeed += _accelSpeed;

        transform.position += _direction * _startSpeed * Time.deltaTime;
    }

    public override void TakeCollision(WhoIs whoIs)
    {
        base.TakeCollision(whoIs);
        whoIs.TakeDamage(_damage);
        Explose();
    }

    public void Explose()
    {
        _startSpeed = 0f;
        _accelSpeed = 0f;
        Destroy(gameObject, 0.1f);
    }
}