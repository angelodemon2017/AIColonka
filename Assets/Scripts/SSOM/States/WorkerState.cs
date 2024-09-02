using UnityEngine;

[CreateAssetMenu]
public class WorkerState : State
{
    private UnityEngine.AI.NavMeshAgent _navMeshAgent;
    private GenerationPlatform _generationPlatform;
    private Transform _storeObject;
    private GameObject _keepObject;
    [SerializeField] private float _freeSpeed = 3.5f;
    [SerializeField] private float _busySpeed = 2f;

    [SerializeField] private float distanceToAction = 1f;

    private bool _isKeepingObject => _keepObject != null;

    protected override void Init()
    {
        _navMeshAgent = ((IMovableCharacter)Character).GetNavMeshAgent();
        _generationPlatform = GameObject.Find("GenerationObjects").GetComponent<GenerationPlatform>();
        _storeObject = GameObject.Find("StoreObjects").transform;
    }

    protected override void Run()
    {
        if (!_isKeepingObject)
        {
            _navMeshAgent.speed = _freeSpeed;
            _navMeshAgent.SetDestination(_generationPlatform.transform.position);
            if (Vector3.Distance(Character.GetTransform().position, _generationPlatform.transform.position) < distanceToAction)
            {
                if (_generationPlatform.ReadyGiveItem)
                {
                    TakeObject(_generationPlatform.Give());
                }
            }
        }

        if (_isKeepingObject)
        {
            _navMeshAgent.speed = _busySpeed;
            _navMeshAgent.SetDestination(_storeObject.position);

            if (Vector3.Distance(Character.GetTransform().position, _storeObject.position) < distanceToAction)
            {
                Destroy(_keepObject);
            }
        }
    }

    private void TakeObject(GameObject keepObj)
    {
        _keepObject = keepObj;
        Character.TakeObject(_keepObject);
    }

    public override bool CheckRules(IStatesCharacter character)
    {
        return character.IsFinishedCurrentState();
    }
}