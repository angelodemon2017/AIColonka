using UnityEngine;

public class PersonMovement : MonoBehaviour
{
    public static PersonMovement Instance;
    [SerializeField] private Transform _pivotForCamera;
    [SerializeField] private AnimationAdapter _animationAdapter;
    [SerializeField] private BitsController _bitsController;
    [SerializeField] private ArmorVisualizator _armorVisualizator;
    const float attack1Time = 0.8f;
    const float attack2Time = 0.75f;
    const float attack3Time = 1.25f;

    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    private CharacterController _characterController;
    private Transform _cameraTransform;
    private Vector3 desiredMoveDirection;

    public Transform AttackPoint;
    public GameObject AttackZone;

    public LayerMask groundMask;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public float jumpHeight;
    private float gravity = -9.81f;
    public float jumpScaleFall;
    private Vector3 _velocity;
    private bool _isGrounded;

    public int _currentAttackStep = 0;
    public int _stepsAttacks = 0;
    public float _timeAttackAnimation = 0;

    [SerializeField] private bool _takingDamage = false;
    private bool _isAttacking => _timeAttackAnimation > 0f;

    private void Awake()
    {
        Instance = this;
        _animationAdapter.triggerPropAction += TrigAnimate;
        CameraController.Instance.SetPivot(_pivotForCamera);
        _cameraTransform = Camera.main.transform;
        _characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        Animation();
    }

    private void FixedUpdate()
    {
        JustMove();
        CalcAttack();
    }

    public void OnMovePlayer(float hor, float vert)
    {
        if (_takingDamage || _cameraTransform == null)
        {
            return;
        }

        Vector3 forward = _cameraTransform.forward;
        Vector3 right = _cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        desiredMoveDirection = (forward * vert + right * hor).normalized;
        _characterController.Move(desiredMoveDirection * moveSpeed * 
            (_isAttacking ? 0.1f : 1f) * 
            (_isGrounded ? 1f : 1.5f));

        if (desiredMoveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(desiredMoveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }
    }

    private void JustMove()
    {
        _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        _velocity.y += gravity * Time.deltaTime;
        _characterController.Move(_velocity * jumpScaleFall * Time.deltaTime);
    }

    public void OnJump()
    {
        if (_isAttacking || _takingDamage)
            return;

        if (_isGrounded)
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    public void OnAttack()
    {
        if (!_isGrounded || _takingDamage)
            return;

        if (_stepsAttacks < 2)
        {
            _stepsAttacks++;
            if (!_isAttacking)
            {
                CalcAttack();
            }
        }
    }

    private void CalcAttack()
    {
        if (_timeAttackAnimation > 0f)
        {
            _timeAttackAnimation -= Time.deltaTime;
            if (_timeAttackAnimation < 0f)
            {
                CalcAttack();
            }
        }

        if (_stepsAttacks > 0)
        {
            _stepsAttacks--;
            _currentAttackStep++;
            if (_currentAttackStep > 3)
            {
                _currentAttackStep = 1;
            }

            switch (_currentAttackStep)
            {
                case 1:
                    _timeAttackAnimation = attack1Time;
                    _animationAdapter.PlayAnimationEvent(EnumAnimations.attack1);
                    Instantiate(AttackZone, AttackPoint.position, transform.rotation);
                    break;
                case 2:
                    _timeAttackAnimation = attack2Time;
                    _animationAdapter.PlayAnimationEvent(EnumAnimations.attack2);
                    Instantiate(AttackZone, AttackPoint.position, transform.rotation);
                    break;
                case 3:
                    _timeAttackAnimation = attack3Time;
                    _animationAdapter.PlayAnimationEvent(EnumAnimations.attack3);
                    Instantiate(AttackZone, AttackPoint.position, transform.rotation);
                    break;
                default:
                    break;
            }
        }
        else
        {
            _currentAttackStep = 0;
        }
    }

    public void OnSecondAttack()
    {
        if (_takingDamage)
            return;

        _bitsController.Shoot();
    }

    private void Animation()
    {
        if (_isAttacking || _takingDamage)
            return;

        if (!_isGrounded)
        {
            _animationAdapter.PlayAnimationEvent(EnumAnimations.jump);
        }
        else
        {
            if (desiredMoveDirection != Vector3.zero)
            {
                _animationAdapter.PlayAnimationEvent(EnumAnimations.walk);
            }
            else
            {
                _animationAdapter.PlayAnimationEvent(EnumAnimations.idle);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == Dicts.Tags.DangerZone)
        {
            TakingDamage();
        }
    }

    private void TakingDamage()
    {
        _takingDamage = true;
        _animationAdapter.PlayAnimationEvent(EnumAnimations.takeDamage);
        Debug.Log("Player get damage");
    }

    internal void CallArmor()
    {
        _armorVisualizator.CallAttack(ArmorVisualizator.TypeVisualAttack.Near);
    }

    private void TrigAnimate(EnumProps prop)
    {
        switch (prop)
        {
            case EnumProps.EndAnimate:
                if (_animationAdapter.CurrentAnimation == EnumAnimations.takeDamage)
                {
                    _takingDamage = false;
                }
                break;
        }
    }

    private void OnDestroy()
    {
        _animationAdapter.triggerPropAction -= TrigAnimate;
    }
}