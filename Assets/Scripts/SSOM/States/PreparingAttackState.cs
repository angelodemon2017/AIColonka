using UnityEngine;

[CreateAssetMenu(menuName = "FSM/State/PreparingAttackState", order = 1)]
public class PreparingAttackState : State
{
    [SerializeField] private float DistanceForTrigger;
    [SerializeField] private float TimeForPreparing;
    private float _timerPreparing;

    private float _currentDistance(IStatesCharacter chr) =>
        Vector3.Distance(chr.GetTransform().position, PlayerFSM.Instance.transform.position);

    protected override void Init()
    {
        _timerPreparing = TimeForPreparing;
    }

    protected override void Run()
    {
        if (_timerPreparing > 0)
        {
            _timerPreparing -= Time.deltaTime;
            if (_timerPreparing <= 0f)
            {
                IsFinished = true;
            }
        }
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        var curDis = _currentDistance(character);
        return curDis < DistanceForTrigger;
    }
}