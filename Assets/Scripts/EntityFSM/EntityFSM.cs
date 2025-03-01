using UnityEngine;

public class EntityFSM : MonoBehaviour, IStatesCharacter
{
    [SerializeField] private ArmorVisualizator _armorVisualizator;
    [SerializeField] private PanelHP _UIpanelHP;
    [SerializeField] private HPComponent _hpComponent;
    [SerializeField] private State _startState;
    [SerializeField] private EntityModule _entityModule;

    public State _currentState;

    internal EntityModule GetModule => _entityModule;
    internal ArmorVisualizator GetArmorVisualizator => _armorVisualizator;

    private void Awake()
    {
        if (_armorVisualizator)
        {
            EntityRepository.Instance.AddWho(_armorVisualizator.GetWhoIs);
        }

        _hpComponent.ChangeHP += _UIpanelHP.UpdateHP;
        _hpComponent.OnChangeHP();

        _entityModule?.Init();

        SetState(_startState);
    }

    private void Update()
    {
        _currentState.RunState();
    }

    private void FixedUpdate()
    {
        _currentState.FixedRun();
    }

    public void SetState(State state)
    {
        SetState(state, false);
    }

    public void SetState(State state, bool ignoreEqual = false)
    {
        if (_currentState == state)
        {
            return;
        }

        _currentState?.ExitState();

        _currentState = Instantiate(state);
        _currentState.InitState(this);
    }

    private void OnDestroy()
    {
        _hpComponent.ChangeHP -= _UIpanelHP.UpdateHP;
    }

    public bool IsFinishedCurrentState()
    {
        return _currentState.IsFinished;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void PlayAnimation(EnumAnimations animation) { }

    public bool CheckProp(EnumProps prop) { return true; }

    public void TakeObject(GameObject keepObj) { }

    public void InitAttackZone(GameObject attackZone) { }
}