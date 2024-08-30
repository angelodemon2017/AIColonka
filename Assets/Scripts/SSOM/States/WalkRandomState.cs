using UnityEngine;

[CreateAssetMenu]
public class WalkRandomState : State
{
    [SerializeField] private float _distanceForDone;
    [SerializeField] private float _distanceRandomPoint;

    private Vector3 target;

    protected override void Init()
    {
        target = SearchNewRandomTarget(Character.GetTransform().position, _distanceRandomPoint);
        ((IMovableCharacter)Character).GoToPoint(target);
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
        if (Vector3.Distance(Character.GetTransform().position, target) < _distanceForDone)
        {
            IsFinished = true;
        }
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}