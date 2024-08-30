using System.Collections.Generic;
using UnityEngine;

public abstract class State : ScriptableObject
{
    public bool IsFinished { get; protected set; }
    [HideInInspector] public IStatesCharacter Character;
    [SerializeField] protected List<State> AvailableStates;
    [SerializeField] protected EnumAnimations Animation;

    public void InitState(IStatesCharacter character)
    {
        IsFinished = false;
        Character = character;
        Character.PlayAnimation(Animation);
        Init();
    }

    protected virtual void Init() { }

    public void RunState()
    {
        Run();
        CheckTransitions();
    }

    protected virtual void Run() { }

    protected void CheckTransitions()
    {
        for (int i = 0; i < AvailableStates.Count; i++)
        {
            if (AvailableStates[i].CheckRules(Character))
            {
                Character.SetState(AvailableStates[i]);
            }
        }
    }

    public abstract bool CheckRules(IStatesCharacter character);

    public virtual void ExitState() { }
}