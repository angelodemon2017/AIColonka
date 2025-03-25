using UnityEngine;

public class PositionLerper : MonoBehaviour
{
    [SerializeField] private Transform _leftBorder;
    [SerializeField] private Transform _rightBorder;
    [SerializeField] private Transform _targetPoint;
    [SerializeField] private bool _isReturning;
    [SerializeField] private float _speed;

    private float _positionPoint = 0.5f;
    private float _timeout;

    private void FixedUpdate()
    {
        if (_isReturning && _timeout > 0)
        {
            _timeout -= Time.fixedDeltaTime;
            if (_timeout <= 0)
            {
                _targetPoint.position = Vector3.Lerp(_leftBorder.position, _rightBorder.position, 0.5f);
            }
        }
    }

    internal void Move(float hor)
    {
        _timeout = 0.1f;

        _positionPoint += hor * _speed * Time.fixedDeltaTime;
        _positionPoint = Mathf.Clamp(_positionPoint, 0f, 1f);
        _targetPoint.position = Vector3.Lerp(_leftBorder.position, _rightBorder.position, _positionPoint);
    }
}