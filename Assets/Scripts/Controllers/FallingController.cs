using UnityEngine;
public class FallingController : MonoBehaviour
{
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.4f;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private CharacterController _characterController;

    public float JumpScaleFall;

    private float gravity = -9.81f;
    private Vector3 _velocity;
    [SerializeField] private bool _isGrounded_;
    private bool _isGrounded 
    {
        get => _isGrounded_;
        set 
        {
            if (value)
            {
                _availableToActionInAir = true;
            }
            _isGrounded_ = value;
        }
    }
    private float _multGravity = 1f;

    [SerializeField] private bool _gravity = true;
    private bool _availableToActionInAir = true;

    internal bool AvailableActionInAir => _availableToActionInAir;
    internal bool IsGrounded => _isGrounded;
    internal bool IsFalling => _velocity.y < 0;

    private void FixedUpdate()
    {
        CalcFall();
    }

    internal void SetYVelocity(float y)
    {
        _velocity.y = y;
    }

    internal void SetMult(float newMult)
    {
        _multGravity = newMult;
    }

    internal void SwitchGravity()
    {
        _gravity = !_gravity;
        _availableToActionInAir = false;
    }

    internal void ResetFalling()
    {
        _multGravity = 1f;
        _gravity = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(groundCheck.position + Vector3.left, Vector3.down * groundDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundDistance);
    }

    RaycastHit hit;
    Ray ray;
    private void CalcFall()
    {
        if (!_gravity)
        {
            return;
        }

        ray = new Ray(groundCheck.position, groundCheck.position + Vector3.down * 10);
        _isGrounded = Physics.Raycast(ray, out hit, groundDistance, _groundMask);

        if (_velocity.y > 0 || !_isGrounded)
        {
            _velocity.y += gravity * Time.fixedDeltaTime * _multGravity;
            _characterController.Move(_velocity * JumpScaleFall * Time.fixedDeltaTime);
        }
    }
}