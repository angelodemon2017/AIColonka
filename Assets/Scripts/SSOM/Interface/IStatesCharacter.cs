using UnityEngine;

public interface IStatesCharacter
{
    bool IsFinishedCurrentState();

    Transform GetTransform();

    void PlayAnimation(EnumAnimations animation);

    void SetState(State state);

    bool CheckProp(EnumProps prop);

    void TakeObject(GameObject keepObj);
}