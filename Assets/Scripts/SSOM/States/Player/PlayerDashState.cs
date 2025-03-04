using UnityEngine;

[CreateAssetMenu(menuName = "FSM/PlayerState/PlayerDashState", order = 1)]
public class PlayerDashState : PlayerState
{
    [SerializeField] private ParticleSystem _prefabEffectDash;
    [SerializeField] private float _distance;
    [SerializeField] private int _instOfStep;
    [SerializeField] private float _cameraTime;

    private Vector3 _startPosition;
    private Transform _transform;
    private float _stateTime;
    private bool _doneDash;

    protected override void Init()
    {
        base.Init();

        CameraController.Instance.UnParrent();
        _transform = Character.GetTransform();
        _startPosition = _transform.position;
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
        _transform.GetComponent<CharacterController>().Move(desiredMoveDirection * _distance);
        SpawnSteps();
        _doneDash = true;
    }

    internal override void FixedRun()
    {
        _stateTime += Time.fixedDeltaTime;
        CameraController.Instance.LerpToPivot(_stateTime / _cameraTime);
        if (_stateTime >= _cameraTime)
        {
            IsFinished = true;
        }
    }

    public override void ExitState()
    {
        CameraController.Instance.ReturnParent();
    }

    private void SpawnSteps()
    {
        for (float i = 0; i < _instOfStep; i++)
        {
            var otn = i / _instOfStep;
            var part = Instantiate(_prefabEffectDash, Vector3.Lerp(_startPosition, _transform.position, otn), Quaternion.identity);
            Destroy(part.gameObject, part.main.duration * otn);
        }
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}