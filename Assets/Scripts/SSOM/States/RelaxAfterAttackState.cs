using UnityEngine;

[CreateAssetMenu]
public class RelaxAfterAttackState : State
{
    [SerializeField] private float TimeForRelax;
    private float _timerRelax;

    protected override void Init()
    {
        _timerRelax = TimeForRelax;
    }

    protected override void Run()
    {
        if (_timerRelax > 0)
        {
            _timerRelax -= Time.deltaTime;
            if (_timerRelax <= 0f)
            {
                IsFinished = true;
            }
        }
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}