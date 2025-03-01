using UnityEngine;

public class MoverWithBorder : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _swift;
    [SerializeField] private float _speed;
    [Range(0f, 1f)]
    [SerializeField] private float _lerpSpeed;

    private Vector3 _tempPos;

    public void SetPlayerTarget()
    {
        _target = PlayerFSM.Instance.transform;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (_target)
        {
            _tempPos = transform.position;

            var xDist = _target.position.x - transform.position.x;
            if (xDist * _swift.x > 0 &&
                Mathf.Abs(xDist) > Mathf.Abs(_swift.x))
            {
                _tempPos.x += _speed * Time.fixedDeltaTime * _swift.x / Mathf.Abs(_swift.x);
                //                transform.Translate(_speed * Time.fixedDeltaTime * _swift.x / Mathf.Abs(_swift.x), 0f, 0f);
            }

            var yDist = _target.position.y - transform.position.y;
            if (yDist * _swift.y > 0 &&
                Mathf.Abs(yDist) > Mathf.Abs(_swift.y))
            {
                _tempPos.y += _speed * Time.fixedDeltaTime * _swift.y / Mathf.Abs(_swift.y);
//                transform.Translate(0f, _speed * Time.fixedDeltaTime * _swift.y / Mathf.Abs(_swift.y), 0f);
            }

            var zDist = _target.position.z - transform.position.z;
            if (zDist * _swift.z > 0 &&
                Mathf.Abs(zDist) > Mathf.Abs(_swift.z))
            {
                _tempPos.z += _speed * Time.fixedDeltaTime * _swift.z / Mathf.Abs(_swift.z);

//                transform.Translate(0f, 0f, _speed * Time.fixedDeltaTime * _swift.z / Mathf.Abs(_swift.z));
            }

//            transform.position = _tempPos;
            transform.position = Vector3.Lerp(transform.position, _tempPos, _lerpSpeed);
        }
    }
}