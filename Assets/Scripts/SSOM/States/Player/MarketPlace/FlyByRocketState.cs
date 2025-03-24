using UnityEngine;

[CreateAssetMenu(menuName = "FSM/PlayerState/Levels/FlyByRocketState", order = 1)]
public class FlyByRocketState : PlayerState
{
    [SerializeField] private float _speed;

    private Transform _leftBorder;
    private Transform _rightBorder;
    private Transform _chasingPoint;

    private float _positionPoint;

    protected override void Init()
    {
        base.Init();
    }

    internal void CustomInit(Transform leftBorder, Transform rightBorder, Transform chasingPoint)
    {
        _leftBorder = leftBorder;
        _rightBorder = rightBorder;
        _chasingPoint = chasingPoint;
    }

    internal override void CallAxisHorVer(float hor, float ver)
    {
        _positionPoint += hor * _speed * Time.fixedDeltaTime;
        _positionPoint = Mathf.Clamp(_positionPoint, 0f, 1f);
        _chasingPoint.position = Vector3.Lerp(_leftBorder.position, _rightBorder.position, _positionPoint);
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}