using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WhoIs : MonoBehaviour
{
    [SerializeField] private HPComponent _hpComponent;
    [SerializeField] internal EnumWhoIs whoIs;
    [SerializeField] private List<DamageKoef> _damageProtected;
    [SerializeField] private List<CollisionEvent> _collisionEvents;
//    [SerializeField] private Collider _collider;

    private Dictionary<EnumCollisionResult, UnityEvent<WhoIs>> _cashEvents = new();
    private Dictionary<EnumDamageType, int> _cashDamages = new();

    private void Awake()
    {
        _damageProtected.ForEach(d => _cashDamages.Add(d.DamageType, d.PercentDamage));
        _collisionEvents.ForEach(e => _cashEvents.Add(e._collisionResult, e._collisionEvent));

        if (whoIs == EnumWhoIs.Player)
        {
            var chap = ControllerDemoSaveFile.Instance.mainData.chapter;
            _hpComponent.OverrideStats(chap.MaxHP, chap.HPRegenBySecond);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        CheckCollider(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
//        Debug.Log($"!!!OnTriggerEnter");
        CheckCollider(other.gameObject);
    }

    private void CheckCollider(GameObject checkGO)
    {
//        Debug.Log($"CheckCollider {checkGO.name}");
        if (checkGO.TryGetComponent(out WhoIs isWho))
        {
            if (_cashEvents.TryGetValue(whoIs.GetColResult(isWho.whoIs), out UnityEvent<WhoIs> _event))
            {
//                _collider.enabled = false;
                _event?.Invoke(isWho);
            }
        }
        else
        {
            if (_cashEvents.TryGetValue(EnumCollisionResult.Other, out UnityEvent<WhoIs> _event))
            {
//                _collider.enabled = false;
                _event?.Invoke(this);
            }
        }
    }

    internal void TakeDamage(Damage damage)
    {
        if (_hpComponent)
        {
            int totalDam = damage.ValueDamage;

            if (_cashDamages.TryGetValue(damage.DamageType, out int percent))
            {
                totalDam = totalDam * percent / 100;
            }

            _hpComponent.GetDamage(totalDam);
        }
    }
}

[System.Serializable]
public class DamageKoef
{
    public EnumDamageType DamageType;
    [Range(0, 200)]
    public int PercentDamage;
}

[System.Serializable]
public class CollisionEvent
{
    public EnumCollisionResult _collisionResult;
    public UnityEvent<WhoIs> _collisionEvent;
}