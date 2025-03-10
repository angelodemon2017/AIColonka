using UnityEngine;

public class EffectHelper : MonoBehaviour
{
    [SerializeField] private ParticleSystem _prefabEffectMove;
    [SerializeField] private int _instOfStep;

    internal void Init(Vector3 start, Vector3 finish)
    {
        for (float i = 0; i < _instOfStep; i++)
        {
            var otn = i / _instOfStep;
            var part = Instantiate(_prefabEffectMove, Vector3.Lerp(start, finish, otn), Quaternion.identity);
            Destroy(part.gameObject, part.main.duration * otn);
        }
        Destroy(gameObject);
    }
}