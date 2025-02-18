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
    private bool _isGrounded;
    private float _multGravity = 1f;

    private bool _gravity = true;

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
    }

    internal void ResetFalling()
    {
        _multGravity = 1f;
        _gravity = true;
    }

    private void CalcFall()
    {
        _isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, _groundMask);

        if (_gravity)
        {
            _velocity.y += gravity * Time.deltaTime * _multGravity;
            _characterController.Move(_velocity * JumpScaleFall * Time.deltaTime);
        }
    }
}