using UnityEngine;

[CreateAssetMenu(menuName = "FSM/BossesState/MoveToPointState", order = 1)]
public class MoveToPointState : State
{
    [SerializeField] private float _distance;
    [SerializeField] private float _speedMove;
    [SerializeField] private string CustomKey;

    private EntityFSM _entityFSM;
    private Transform _target;

    internal override string Key => CustomKey;

    protected override void Init()
    {
        base.Init();

        if (Character is EntityFSM entfsm)
        {
            _entityFSM = entfsm;            
            var points = _entityFSM.GetModule.GetPoints(this);
            if (points.Count == 0)
            {
                IsFinished = true;
            }
            else
            {
                _target = points.GetRandom();
            }
        }
        else
        {
            IsFinished = true;
        }
    }

    internal override void FixedRun()
    {
        base.FixedRun();

        _entityFSM.transform.position = Vector3.Lerp(_entityFSM.transform.position, _target.position, _speedMove * Time.fixedDeltaTime);
        if (Vector3.Distance(_entityFSM.transform.position, _target.position) < _distance)
        {
            IsFinished = true;
        }
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}