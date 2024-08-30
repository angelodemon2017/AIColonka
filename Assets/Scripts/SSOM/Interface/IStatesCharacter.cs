using UnityEngine;

public interface IStatesCharacter
{
    void PlayAnimation(EnumAnimations animation);

    void SetState(State state);

    bool IsFinishedCurrentState();

    Transform GetTransform();
}