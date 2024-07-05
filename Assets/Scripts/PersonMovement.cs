using UnityEngine;

public class PersonMovement : MonoBehaviour
{
    private int _currentAnimation;
    const int idleAnimation = 1;
    const int walkingAnimation = 10;
    const int jumpAnimation = 35;//13;
    const int attackAnimation1 = 32;
    const string attackAnimationStr1 = "Atk1";
    const float attack1Time = 0.8f;
    const int attackAnimation2 = 33;
    const string attackAnimationStr2 = "Atk2";
    const float attack2Time = 0.75f;
    const int attackAnimation3 = 34;
    const string attackAnimationStr3 = "Atk3";
    const float attack3Time = 1.25f;

    public Animator animator;
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    private CharacterController _characterController;
    private Transform _cameraTransform;
    private Vector3 desiredMoveDirection;

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

    private bool _isAttacking => _timeAttackAnimation > 0f;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        MovePlayer();
        Jump();
        Attack();
        Animation();
    }

    private void MovePlayer()
    {
        _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 forward = _cameraTransform.forward;
        Vector3 right = _cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        desiredMoveDirection = (forward * vertical + right * horizontal).normalized;
        _characterController.Move(desiredMoveDirection * moveSpeed * (_isAttacking ? 0.3f : 1f));

        if (desiredMoveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(desiredMoveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        _velocity.y += gravity * Time.deltaTime;
        _characterController.Move(_velocity * jumpScaleFall * Time.deltaTime);
    }

    private void Jump()
    {
        if (_isAttacking)
            return;

        if (Input.GetButtonDown("Jump") && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void Attack()
    {
        if (!_isGrounded)
            return;

        if (Input.GetButtonDown("Fire1"))
        {
            if (_stepsAttacks < 2)
            {
                _stepsAttacks++;
                if (!_isAttacking)
                {
                    CalcAttack();
                }
            }
        }

        if (_timeAttackAnimation > 0f)
        {
            _timeAttackAnimation -= Time.deltaTime;
            if (_timeAttackAnimation < 0f)
            {
                CalcAttack();
            }
        }
    }

    private void CalcAttack()
    {
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
                    //                            PlayAnimate(attackAnimation1);
                    animator.Play(attackAnimationStr1);
                    break;
                case 2:
                    _timeAttackAnimation = attack2Time;
                    //                            PlayAnimate(attackAnimation2);
                    animator.Play(attackAnimationStr2);
                    break;
                case 3:
                    _timeAttackAnimation = attack3Time;
                    //                            PlayAnimate(attackAnimation3);
                    animator.Play(attackAnimationStr3);
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
    
    private void Animation()
    {
        if (_isAttacking)
            return;

        if (!_isGrounded)
        {
            PlayAnimate(jumpAnimation);
        }
        else
        {
            if (desiredMoveDirection != Vector3.zero)
            {
                PlayAnimate(walkingAnimation);
            }
            else
            {
                PlayAnimate(idleAnimation);
            }
        }
    }

    private void PlayAnimate(int numAnimation)
    {
        if (_currentAnimation != numAnimation)
        {
            animator.SetInteger("animation", numAnimation);
            _currentAnimation = numAnimation;
        }
    }
}