using UnityEngine;

[CreateAssetMenu(menuName = "FSM/PlayerState/PlayerDashState", order = 1)]
public class PlayerDashState : PlayerState
{
    [SerializeField] private EffectHelper _effectPrefab;
    [SerializeField] private float _distance;
    [SerializeField] private float _cameraTime;
    [SerializeField] private float _pauseTime;

    private Vector3 _startPosition;
    private Transform _transform;
    private float _stateTime;
    private bool _doneDash;
    private Vector3 _customFinish = Vector3.zero;

    protected override void Init()
    {
        base.Init();

        CameraController.Instance.UnParrent(_cameraTime);
        _transform = Character.GetTransform();
        _startPosition = _transform.position;

        if (_customFinish != Vector3.zero)
        {
            _characterController.Move(_customFinish - _transform.position);
            ApplyDashAndEffect();
        }
    }

    internal void SetCustomTargetPoint(Vector3 targetP)
    {
        _customFinish = targetP;
    }

    internal override void CallAxisHorVer(float hor, float ver)
    {
        if (_doneDash)
        {
            return;
        }

        var cameraTransform = CameraController.Instance.transform;
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        var desiredMoveDirection = (forward * ver + right * hor).normalized;
        _characterController.Move(desiredMoveDirection * _distance);

        ApplyDashAndEffect();
    }

    private void ApplyDashAndEffect()
    {
        var ep = Instantiate(_effectPrefab);
        ep.Init(_startPosition, _transform.position);

        _doneDash = true;
    }

    internal override void FixedRun()
    {
        _stateTime += Time.fixedDeltaTime;
        if (_stateTime >= _pauseTime)
        {
            IsFinished = true;
        }
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}