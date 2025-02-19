using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;

public class BattleZone : MonoBehaviour
{
    [SerializeField] private List<VariantSpawn> _variantPrefabs = new();
    [SerializeField] private List<Transform> _pointVariants;
    [SerializeField] private List<GameObject> _onOffObjects;
    [SerializeField] private UnityEvent _eventByEndBattle;

    private List<BattleZoneActivator> _battleZoneActivators = new();

    public void Activate()
    {
        OnOffGOs();
        SpawnVariants();
    }

    private void SpawnVariants()
    {
        int idPoint = 0;
        _battleZoneActivators.Clear();
        foreach (var vp in _variantPrefabs)
        {
            for (int i = 0; i < vp.Count; i++)
            {
                var newGO = Instantiate(vp._variantPrefab, _pointVariants[idPoint].position, Quaternion.identity);
                newGO.Init(this);
                _battleZoneActivators.Add(newGO.GetComponent<BattleZoneActivator>());

                idPoint++;
                if (idPoint >= _pointVariants.Count)
                {
                    idPoint = 0;
                }
            }
        }
    }

    internal void CheckZone()
    {
        if (!_battleZoneActivators.Any(z => !z.IsDone))
        {
            Deactivate();
        }
    }

    private void Deactivate()
    {
        OnOffGOs();
        _battleZoneActivators?.ForEach(a => Destroy(a?.gameObject));
        _battleZoneActivators.Clear();
        _eventByEndBattle?.Invoke();
    }

    private void OnOffGOs()
    {
        _onOffObjects.ForEach(o => o?.gameObject?.SetActive(!o.gameObject.activeSelf));
    }

    [System.Serializable]
    public class VariantSpawn
    {
        public BattleZoneActivator _variantPrefab;
        public int Count;
    }
}