using UnityEngine;

public class StorePlatform : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var ec = other.GetComponent<EnemyController>();
        if (ec != null && ec.readyGiveItem)
        {
            Destroy(ec.DropKeepItem());
        }
    }
}