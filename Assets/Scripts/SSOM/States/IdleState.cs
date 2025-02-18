using UnityEngine;

[CreateAssetMenu(menuName = "FSM/State/IdleState", order = 1)]
public class IdleState : State
{
    [SerializeField] private float _timeIdle;

    private float _timerIdle;

    protected override void Init()
    {
        _timerIdle = _timeIdle;
    }

    protected override void Run()
    {
        if (_timerIdle > 0f)
        {
            _timerIdle -= Time.deltaTime;
        }

        if (_timerIdle < 0f)
        {
            IsFinished = true;
        }
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}