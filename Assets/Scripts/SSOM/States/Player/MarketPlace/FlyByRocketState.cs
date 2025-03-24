using UnityEngine;

[CreateAssetMenu(menuName = "FSM/PlayerState/Levels/FlyByRocketState", order = 1)]
public class FlyByRocketState : PlayerState
{
    private PositionLerper _moving;
    private PositionLerper _looker;

    protected override void Init()
    {
        base.Init();
    }

    internal void CustomInit(PositionLerper moving, PositionLerper looker)
    {
        _moving = moving;
        _looker = looker;
    }

    internal override void CallAxisHorVer(float hor, float ver)
    {
        _moving.Move(hor);
//        _looker.Move(hor);
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}