using UnityEngine;

[CreateAssetMenu(menuName = "FSM/State/WalkRandomState", order = 1)]
public class WalkRandomState : State
{
    [SerializeField] private float _distanceForDone;
    [SerializeField] private float _distanceRandomPoint;
    [SerializeField] private float _speed = 3.5f;

    private UnityEngine.AI.NavMeshAgent _navMeshAgent;
    private Vector3 _target;
    private float _timeProblem = 0.6f;
    private float _timerProblem = 0f;

    protected override void Init()
    {
        _timerProblem = _timeProblem;
        _navMeshAgent = ((IMovableCharacter)Character).GetNavMeshAgent();
        _navMeshAgent.speed = _speed;
        _target = SearchNewRandomTarget(Character.GetTransform().position, _distanceRandomPoint);
        ((IMovableCharacter)Character).GoToPoint(_target);
    }

    private Vector3 SearchNewRandomTarget(Vector3 centerPoint, float radius)
    {
        return new Vector3(
            Random.Range(centerPoint.x - radius, centerPoint.x + radius),
            centerPoint.y,
            Random.Range(centerPoint.z - radius, centerPoint.z + radius));
    }

    protected override void Run()
    {
        var distance = Vector3.Distance(Character.GetTransform().position, _target);

        if (distance < _distanceForDone)
        {
            IsFinished = true;
        }

        if (_navMeshAgent.velocity != Vector3.zero)
        {
            _timerProblem = _timeProblem;
        }
        else
        {
            _timerProblem -= Time.deltaTime;
            if (_timerProblem <= 0f)
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