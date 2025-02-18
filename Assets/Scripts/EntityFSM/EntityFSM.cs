using UnityEngine;

public class EntityFSM : MonoBehaviour, IStatesCharacter
{
    [SerializeField] private ArmorVisualizator _armorVisualizator;
    [SerializeField] private PanelHP _UIpanelHP;
    [SerializeField] private HPComponent _hpComponent;
    [SerializeField] private State _startState;

    private State _currentState;

    internal ArmorVisualizator GetArmorVisualizator => _armorVisualizator;

    private void Awake()
    {
        _hpComponent.ChangeHP += _UIpanelHP.UpdateHP;
        _hpComponent.OnChangeHP();

        SetState(_startState);
    }

    private void Update()
    {
        _currentState.RunState();
    }

    public void SetState(State state)
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