using System;
using UnityEngine;

public class ColliderController : MonoBehaviour
{
    public Action<EnumAIStates> ColliderAction;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"anyTrigger {other.gameObject.name}");
        if (other.gameObject.tag == Dicts.Tags.Mellee)
        {
            ColliderAction?.Invoke(EnumAIStates.TakingDamage);
            Debug.Log("TakeDamage");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("anyCollision");
        Debug.Log("TakeDamage");
        if (collision.gameObject.tag == Dicts.Tags.Mellee)
        {
            ColliderAction?.Invoke(EnumAIStates.TakingDamage);
            Debug.Log("TakeDamage");
        }
    }
}