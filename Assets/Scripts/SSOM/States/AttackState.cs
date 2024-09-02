using UnityEngine;

[CreateAssetMenu]
public class AttackState : State
{
    [SerializeField] private float DistanceForTrigger;
    [SerializeField] private GameObject _attackZone;

    private float _currentDistance(IStatesCharacter chr) =>
        Vector3.Distance(chr.GetTransform().position, PersonMovement.Instance.transform.position);

    protected override void Init()
    {
        var az = Instantiate(_attackZone);
        Character.InitAttackZone(az);
    }

    protected override void Run()
    {
        if (Character.CheckProp(EnumProps.EndAnimate))
        {
            Debug.Log("AttackState isFinished");
            IsFinished = true;
        }
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        var curDis = _currentDistance(character);
        return curDis < DistanceForTrigger;
    }
}