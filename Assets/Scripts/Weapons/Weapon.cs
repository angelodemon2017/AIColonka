using UnityEngine;

[RequireComponent(typeof(WhoIs))]
public class Weapon : MonoBehaviour
{
    [SerializeField] protected Damage _damage;
    [SerializeField] protected WhoIs WhoIs;
    
    private void OnValidate()
    {
        WhoIs = GetComponent<WhoIs>();
    }

    internal void Init(EnumWhoIs whoIs)
    {
        WhoIs.whoIs = whoIs;
        StartAttack();
    }

    internal virtual void StartAttack()
    {

    }

    public virtual void TakeCollision(WhoIs whoIs)
    {

    }
}