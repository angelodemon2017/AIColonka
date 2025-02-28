using UnityEngine;

public class Projectile : AVWeapon
{
    [SerializeField] private float _startSpeed;
    [SerializeField] private float _accelSpeed;
    [SerializeField] private float _timeOut = 10f;
    [SerializeField] private bool _destroyAtCollision;

    private float _startDistance;
    private AttackDecal _attackDecal;
    private Vector3 _direction;

    internal override void StartAttack()
    {
        if (_avTransform)
        {
            transform.position = _avTransform.position;
        }
        transform.rotation = _rotate;
        _direction = transform.forward;

        SpawnDecal();
        Destroy(gameObject, _timeOut);
    }

    private void FixedUpdate()
    {
        Fly();
        UpdateDecal();
    }

    private void Fly()
    {
        _startSpeed += _accelSpeed;

        transform.position += _direction * _startSpeed * Time.fixedDeltaTime;
    }

    private void UpdateDecal()
    {
        if (_attackDecal)
        {
            var currentDistance = Vector3.Distance(transform.position,
                _target ? _target.position : _endPoint);
            _attackDecal.UpdateProgress(currentDistance / _startDistance);
        }
    }

    public override void TakeCollision(WhoIs whoIs)
    {
        base.TakeCollision(whoIs);
        whoIs.TakeDamage(_damage);
        Explose();
    }

    [SerializeField] private LayerMask layerMask;
    RaycastHit hit;
    Ray ray;
    Vector3 _endPoint;
    protected override void SpawnDecal()
    {
        if (WhoIs.whoIs == EnumWhoIs.Player)
        {
            return;
        }

        _attackDecal = Instantiate(_attackDecalPrefab);
        if (_target)
        {
            _startDistance = Vector3.Distance(_target.position, transform.position);
            _attackDecal.transform.position = _target.position;
        }
        else
        {
            ray = new Ray(transform.position, transform.position + transform.forward);
            Physics.Raycast(ray, out hit, 100f, layerMask);
            if (hit.collider)
            {
                _endPoint = hit.point;
                _attackDecal.transform.position = _endPoint;
            }
        }

        Destroy(_attackDecal.gameObject, _timeOut);
    }

    public void Explose()
    {
        if (_destroyAtCollision)
        {
            _startSpeed = 0f;
            _accelSpeed = 0f;
            Destroy(gameObject, 0.1f);
            if (_attackDecal)
            {
                Destroy(_attackDecal.gameObject);
            }
        }
    }
}