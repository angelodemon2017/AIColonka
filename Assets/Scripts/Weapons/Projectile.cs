using UnityEngine;

public class Projectile : AVWeapon
{
    [SerializeField] private AttackZone _exploseZone;
    [SerializeField] private Damage _damageExpl;
    [SerializeField] protected float _startSpeed;
    [SerializeField] private float _accelSpeed;
    [SerializeField] protected float _timeOut = 10f;
    [SerializeField] private bool _destroyAtCollision;
    [SerializeField] private float _accuracy;
    [SerializeField] private GameObject _dummyPrefab;

    private GameObject _dummy;
    protected float _startDistance;
    private float currentDistance;
    protected AttackDecal _attackDecal;
    private Vector3 _direction;

    internal override void StartAttack()
    {
        if (_avTransform)
        {
            transform.position = _avTransform.position;
        }
        transform.rotation = _rotate;
        _direction = transform.forward;

        if (_target && WhoIs.whoIs != EnumWhoIs.Player)
        {
            Debug.LogWarning($"!Dummy spawned");
            _dummy = Instantiate(_dummyPrefab);
            _dummy.transform.position = _target.position + 
                new Vector3(Random.Range(-_accuracy, _accuracy), 0f,
                Random.Range(-_accuracy, _accuracy));

            _target = _dummy.transform;
        }

        SpawnDecal();
        Destroy(gameObject, _timeOut);
    }

    private void Update()
    {
        UpdateDecal();
    }

    private void FixedUpdate()
    {
        Fly();
    }

    protected virtual void Fly()
    {
        _startSpeed += _accelSpeed;

        transform.position += _direction * _startSpeed * Time.fixedDeltaTime;
    }

    private float Progress;
    private void UpdateDecal()
    {
        if (_attackDecal)
        {
            currentDistance = Vector3.Distance(transform.position,
                _attackDecal.transform.position);
            var temp = (1.1f - currentDistance / _startDistance);
            if (temp > Progress)
            {
                Progress = temp;
                _attackDecal.UpdateProgress(Progress > 1 ? 1f : Progress);
            }
        }
    }

    public override void TakeCollision(WhoIs whoIs)
    {
        base.TakeCollision(whoIs);
        whoIs.TakeDamage(_damage);
        ShowHit();
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
        _attackDecal.Init(360f, _sizeDecal);
        _attackDecal.transform.position = transform.position;
        if (Mathf.Abs(transform.position.y - _target.position.y) < 1f)
        {
            _attackDecal.transform.rotation = transform.rotation;
        }
        if (_target)
        {
            _startDistance = Vector3.Distance(_target.position, transform.position);
            _attackDecal.Mover.SetVectTarget(_target.position);
//            _attackDecal.transform.position = _target.position;
        }
        else
        {
            ray = new Ray(transform.position, transform.position + transform.forward);
            Physics.Raycast(ray, out hit, 100f, layerMask);
            if (hit.collider)
            {
                _endPoint = hit.point;
                _attackDecal.Mover.SetVectTarget(_endPoint);
//                _attackDecal.transform.position = _endPoint;
            }
        }

        Destroy(_attackDecal.gameObject, _timeOut);
    }

    public void Explose()
    {
        if (_exploseZone)
        {
            var ez = Instantiate(_exploseZone);
            ez.SetDamage(_damageExpl);
            ez.SetAttackZone(_sizeDecal, 0.5f);
            ez.Init(WhoIs.whoIs);
            ez.transform.position = transform.position;
        }
        if (_destroyAtCollision)
        {
            _startSpeed = 0f;
            _accelSpeed = 0f;
            Debug.LogWarning($"destroyed curDist:{currentDistance},stDist:{_startDistance},Progress={Progress}");
            if (_attackDecal)
            {
                Destroy(_attackDecal.gameObject, 0.2f);
            }
            if (_dummy)
            {
                Destroy(_dummy);
            }
            Destroy(this.gameObject, 0.01f);
        }
    }
}