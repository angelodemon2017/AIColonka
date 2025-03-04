using UnityEngine;

public class AVBallisticBullet : Projectile
{
    [SerializeField] private AnimationCurve _ballisticHigh;
    [SerializeField] private float _highMult;

    private Vector3 _startPoint;
    private Vector3 _finalPoint;
    private float _timeFly;
    private float _endTime;

    private float _lerpPos => _timeFly / _endTime;

    internal override void StartAttack()
    {
        base.StartAttack();
        _startPoint = transform.position;
        _finalPoint = _target.position;
        _endTime = (_startDistance + _highMult) / _startSpeed;
    }

    protected override void SpawnDecal()
    {
        if (WhoIs.whoIs == EnumWhoIs.Player)
        {
            return;
        }

        if (_target)
        {
            _attackDecal = Instantiate(_attackDecalPrefab, transform.position, Quaternion.LookRotation(Vector3.up));
            _attackDecal.Init(360f, _sizeDecal);
            _attackDecal.Mover.SetVectTarget(_target.position);
            _startDistance = Vector3.Distance(_target.position, transform.position);

            Destroy(_attackDecal.gameObject, _timeOut);
        }
    }

/*    protected override void UpdateDecal()
    {
        if (_attackDecal)
        {
            currentDistance = Vector3.Distance(transform.position,
                _attackDecal.transform.position);
            Progress = (1.1f - currentDistance / _startDistance);
            _attackDecal.UpdateProgress(Progress > 1 ? 1f : Progress);
        }
    }/**/

    protected override void Fly()
    {
        _timeFly += Time.fixedDeltaTime;

        var tempPos = Vector3.Lerp(_startPoint, _finalPoint, _lerpPos);
        tempPos.y += _ballisticHigh.Evaluate(_lerpPos) * _highMult;

        transform.position = tempPos;

        if (_lerpPos > 1)
        {
            ShowHit();
            Explose();
        }
    }/**/
}