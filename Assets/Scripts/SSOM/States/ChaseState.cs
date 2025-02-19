using UnityEngine;

[CreateAssetMenu(menuName = "FSM/State/ChaseState", order = 1)]
public class ChaseState : State
{
    [SerializeField] private float DistanceForTrigger;
    [SerializeField] private float DistanceForStopTrigger;
    [SerializeField] private float _chaseSpeed = 4;

    private UnityEngine.AI.NavMeshAgent _navMeshAgent;

    private float _currentDistance(IStatesCharacter chr) => 
        Vector3.Distance(chr.GetTransform().position, PlayerFSM.Instance.transform.position);

    protected override void Init()
    {
        _navMeshAgent = ((IMovableCharacter)Character).GetNavMeshAgent();
        _navMeshAgent.speed = _chaseSpeed;
    }

    protected override void Run() 
    {
        var curDis = _currentDistance(Character);
        _navMeshAgent.SetDestination(PlayerFSM.Instance.transform.position);

        if (curDis < DistanceForStopTrigger)
        {
            IsFinished = true;
        }

        if (curDis > DistanceForTrigger)
        {
            IsFinished = true;
        }
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        var curDis = _currentDistance(character);
        return curDis < DistanceForTrigger &&
            curDis > DistanceForStopTrigger;
    }

    public override void ExitState()
    {
        _navMeshAgent.SetDestination(Character.GetTransform().position);
    }
}