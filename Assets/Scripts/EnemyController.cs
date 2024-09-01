using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private AnimationAdapter _animationAdapter;
    [SerializeField] private ColliderController _colliderController;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private float radiusWalkings;
    [SerializeField] private float IdleTime;
    [SerializeField] private EnumAITasks _aITask;
    private Vector3 target;
    private float _timerForIdle = 0f;

    [SerializeField] private Transform _keepObjectPoint;
    [SerializeField] private GameObject _keepObject;
    [SerializeField] private Transform _generatorObjects;
    [SerializeField] private Transform _storeObjects;
    public bool canGetKeepObject => _aITask == EnumAITasks.beLoader 
        && _keepObject == null;
    public bool readyGiveItem => _keepObject != null;

    private void Awake()
    {
        target = transform.position;
//        _animationAdapter.endAnimation += OnTriggerAnimation;
        if (_colliderController)
        {
//            _colliderController.ColliderAction += OnColliderEvent;
        }
    }

    public void TakeObject(GameObject keepObj)
    {
        _keepObject = keepObj;
        _keepObject.transform.SetParent(_keepObjectPoint);
        _keepObject.transform.position = _keepObjectPoint.position;
        CaclTask();
    }

    public GameObject DropKeepItem()
    {
        return _keepObject;
    }

    void Update()
    {
        CheckWalking();
        RunTask();
    }

    private void CheckWalking()
    {
        if (Vector3.Distance(transform.position, target) < 1f || _navMeshAgent.velocity == Vector3.zero)
        {
            _timerForIdle -= Time.deltaTime;
            if (_timerForIdle < 0f)
            {
                CaclTask();
                _timerForIdle = IdleTime;
            }

            if (_navMeshAgent.velocity != Vector3.zero)
            {
                _animationAdapter.PlayAnimationEvent(EnumAnimations.idle);
            }
        }
    }

    private void CaclTask()
    {
        switch (_aITask)
        {
            case EnumAITasks.hangAround:
                GoToNewPoint();
                break;
            case EnumAITasks.beLoader:
                GoToLoader();
                break;
            case EnumAITasks.battle:
                break;
        }
    }

    private void RunTask()
    {
        switch (_aITask)
        {
            case EnumAITasks.hangAround:
                break;
            case EnumAITasks.beLoader:
                RunLoader();
                break;
            case EnumAITasks.battle:
                break;
        }
    }

    private void GoToNewPoint()
    {
        if (_navMeshAgent.speed == 0)
        {
            return;
        }

        SearchNewRandomTarget();
        _navMeshAgent.SetDestination(target);
        _animationAdapter.PlayAnimationEvent(EnumAnimations.walk);
    }

    private void RunLoader()
    {
        if (Vector3.Distance(transform.position, _generatorObjects.position) < 1f &&
            canGetKeepObject)
        {
            var gp = _generatorObjects.GetComponent<GenerationPlatform>();

            if (gp.ReadyGiveItem)
            {
                TakeObject(gp.Give());
            }
        }

        if (readyGiveItem &&
            Vector3.Distance(transform.position, _storeObjects.position) < 1f)
        {
            Destroy(DropKeepItem());
        }
    }

    private void GoToLoader()
    {
        if (_keepObject == null)
        {
            target = _generatorObjects.position;
            _navMeshAgent.SetDestination(target);
        }
        else
        {
            target = _storeObjects.position;
            _navMeshAgent.SetDestination(target);
        }

        if (Vector3.Distance(transform.position, target) < 1f)
        {
            _animationAdapter.PlayAnimationEvent(EnumAnimations.idle);
        }
        else
        {
            _animationAdapter.PlayAnimationEvent(EnumAnimations.walk);
        }
    }

    private void SearchNewRandomTarget()
    {
        target = new Vector3(Random.Range(transform.position.x - radiusWalkings, transform.position.x + radiusWalkings),
            transform.position.y,
            Random.Range(transform.position.z - radiusWalkings, transform.position.z + radiusWalkings));
    }

    private void OnColliderEvent(EnumAIStates enumAIState)
    {
        switch (enumAIState)
        {
            case EnumAIStates.TakingDamage:
                _animationAdapter.PlayAnimationEvent(EnumAnimations.takeDamage);
                break;
            default:
                break;

        }
    }

    private void OnTriggerAnimation(EnumAnimations animation)
    {
        switch (animation)
        {
            case EnumAnimations.takeDamage:
                _animationAdapter.PlayAnimationEvent(EnumAnimations.idle);
                break;
            default:
                break;
        }
    }
}