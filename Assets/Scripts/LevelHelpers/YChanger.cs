using UnityEngine;

public class YChanger : MonoBehaviour
{
    [SerializeField] private AnimationCurve _curveChanging;
    [SerializeField] private float _speedChanging = 1;
    [SerializeField] private float _multAmplitude = 1;

    private float _progress = 0;
    private Vector3 changer;

    private void FixedUpdate()
    {
        changer = transform.position;
        _progress += Time.fixedDeltaTime * _speedChanging;
        if (_progress > 1f)
        {
            _progress -= 1f;
        }
        changer.y = _curveChanging.Evaluate(_progress) * _multAmplitude;
        transform.position = changer;
    }
}