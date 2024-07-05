using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private AnimationAdapter _animationAdapter;
    [SerializeField] private ColliderController _colliderController;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private float radiusWalkings;
    [SerializeField] private float IdleTime;
    private Vector3 target;
    private float _timerForIdle = 0f;

    private void Awake()
    {
        target = transform.position;
        _animationAdapter.endAnimation += OnTriggerAnimation;
        if (_colliderController)
        {
            _colliderController.ColliderAction += OnColliderEvent;
        }
    }

    void Update()
    {
        CheckWalking();
    }

    private void CheckWalking()
    {
        if (Vector3.Distance(transform.position, target) < 1f || _navMeshAgent.velocity == Vector3.zero)
        {
            _timerForIdle -= Time.deltaTime;
            if (_timerForIdle < 0f)
            {
                GoToNewPoint();
                _timerForIdle = IdleTime;
            }

            if (_navMeshAgent.velocity != Vector3.zero)
            {
                _animationAdapter.PlayAnimationEvent(EnumAnimations.idle);
            }
        }
    }

    private void GoToNewPoint()
    {
        if (_navMeshAgent.speed == 0)
        {
            return;
        }

        SearchNewTarget();
        _navMeshAgent.SetDestination(target);
        _animationAdapter.PlayAnimationEvent(EnumAnimations.walk);
    }

    private void SearchNewTarget()
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