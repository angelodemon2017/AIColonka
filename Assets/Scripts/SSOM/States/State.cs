using System.Collections.Generic;
using UnityEngine;

public abstract class State : ScriptableObject
{
    public bool IsFinished { get; protected set; }
    [HideInInspector] public IStatesCharacter Character;
    [SerializeField] protected List<State> AvailableStates;
    [SerializeField] protected EnumAnimations Animation;

    internal virtual string Key => name;
    protected virtual EnumAnimations GetAnimation => Animation;

    public void InitState(IStatesCharacter character)
    {
        IsFinished = false;
        Character = character;
        Init();
        Character.PlayAnimation(GetAnimation);
    }

    protected virtual void Init() { }

    internal virtual void CheckAndUpdateState() { }

    public void RunState()
    {
        Run();
        CheckTransitions();
    }

    protected virtual void Run() { }

    internal virtual void FixedRun() { }

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