using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EntityRepository : MonoBehaviour
{
    public static EntityRepository Instance;

    [SerializeField] private List<WhoIs> _whoAre = new();

    private void Awake()
    {
        Instance?.CleanList();
        Instance = this;
    }

    internal void AddWho(WhoIs whoIs)
    {
        _whoAre.Add(whoIs);
    }

    internal void RemoveWho(WhoIs whoIs)
    {
        _whoAre.Remove(whoIs);
    }

    internal bool HaveEnemies()
    {
        return _whoAre
            .Any(w => w.whoIs == EnumWhoIs.Enemy);
    }

    internal WhoIs GetNearestEnemy(Vector3 position)
    {
        return _whoAre
            .Where(w => w.whoIs == EnumWhoIs.Enemy)
            .OrderBy(w => Vector3.Distance(position, w.transform.position))
            .FirstOrDefault();
    }

    internal void CleanList()
    {
        _whoAre.Clear();
    }

    private void OnDestroy()
    {
        CleanList();
    }
}