using System.Collections.Generic;
using UnityEngine;
using Zenject;

public abstract class State : ScriptableObject
{
    public bool IsFinished { get; protected set; }
    [HideInInspector] public IStatesCharacter Character;
    [SerializeField] protected List<State> AvailableStates;
    [SerializeField] protected EnumAnimations Animation;

    protected DiContainer _diContainer;
    protected GameplayHandler _gameplayHandler;

    internal virtual string Key => name;
    protected virtual EnumAnimations GetAnimation => Animation;

    [Inject]
    private void Construct(
        DiContainer diContainer,
        GameplayHandler gameplayHandler)
    {
        _diContainer = diContainer;
        _gameplayHandler = gameplayHandler;
    }

    public void InitState(IStatesCharacter character)
    {
        IsFinished = false;
        Character = character;
        Character.Container.Inject(this);
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

    internal void Finish()
    {
        IsFinished = true;
    }

    public virtual void ExitState() { }
}