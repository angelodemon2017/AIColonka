using UnityEngine;

public class KillerZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out HPComponent hP))
        {
            hP.Kill();
        }
    }
}