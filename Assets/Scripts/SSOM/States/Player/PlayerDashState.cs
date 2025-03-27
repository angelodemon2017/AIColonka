using UnityEngine;
using DG.Tweening;
using Zenject;

[CreateAssetMenu(menuName = "FSM/PlayerState/PlayerDashState", order = 1)]
public class PlayerDashState : PlayerState
{
    [SerializeField] private EffectHelper _effectPrefab;
    [SerializeField] private float _distance;
    [SerializeField] private float _cameraTime;
    [SerializeField] private float _pauseTime;

    [Inject]
    private SignalBus _signalBus;

    private FallingController _fallingController;
    private Vector3 _startPosition;
    private Transform _transform;
    private bool _doneDash;
    private Vector3 _customFinish = Vector3.zero;

    protected override void Init()
    {
        base.Init();

        _fallingController = _playerFSM.GetFallingController;
        _fallingController.SwitchOffGravity();
        _cameraController.UnParrent(_cameraTime);
        _transform = Character.GetTransform();
        _startPosition = _transform.position;

        if (_customFinish != Vector3.zero)
        {
            _playerFSM.MovePerson(_customFinish - _transform.position);
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

        var cameraTransform = _cameraController.transform;
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        var desiredMoveDirection = (forward * ver + right * hor).normalized;
        _playerFSM.MovePerson(desiredMoveDirection * _distance);

        ApplyDashAndEffect();
    }

    private void ApplyDashAndEffect()
    {
        _signalBus.Fire<PlayerDashSignal>();
        var ep = Instantiate(_effectPrefab);
        ep.Init(_startPosition, _transform.position);

        _doneDash = true;
        DOVirtual.DelayedCall(_pauseTime, SetFinished);
    }

    public override void ExitState()
    {
        base.ExitState();

        _fallingController.ResetFalling();
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}