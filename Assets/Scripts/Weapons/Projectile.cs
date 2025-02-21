using UnityEngine;

public class Projectile : AVWeapon
{
    [SerializeField] private float _startSpeed;
    [SerializeField] private float _accelSpeed;
    [SerializeField] private float _timeOut = 10f;
    [SerializeField] private bool _destroyAtCollision;

    private Vector3 _direction;

    internal override void StartAttack()
    {
        if (_avTransform)
        {
            transform.position = _avTransform.position;
        }
        transform.rotation = _rotate;
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
        if (_destroyAtCollision)
        {
            _startSpeed = 0f;
            _accelSpeed = 0f;
            Destroy(gameObject, 0.1f);
        }
    }
}