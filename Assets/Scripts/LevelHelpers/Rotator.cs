using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private float _speed;

    internal void AddSpeed(float percent)
    {
        _speed += _speed * percent / 100f;
    }

    internal void SetSpeed(float speed)
    {
        _speed = speed;
    }

    private void FixedUpdate()
    {
        transform.Rotate(0f, _speed, 0f);
    }
}