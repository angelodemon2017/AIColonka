using UnityEngine;

public class Scaler : MonoBehaviour
{
    [SerializeField] private float _targetScale;
    [SerializeField] private float _speed;

    private void FixedUpdate()
    {
        if (transform.localScale.x < _targetScale)
        {
            transform.localScale *= _speed * Time.fixedDeltaTime + 1f;
        }         
    }
}