using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponVisualizator : MonoBehaviour
{
    [SerializeField] private PeriodicActivator _periodicActivator;
    [SerializeField] private List<PointsByState> _pointsByStates;

    private SpawnWeaponState _currentState;
    private Dictionary<string, List<Transform>> _cashPointsByStates = new();

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _pointsByStates.ForEach(p => _cashPointsByStates.Add(p.WeaponState.Key, p.Points));
    }

    internal void CallAttack(SpawnWeaponState state)
    {
        _currentState = state;
    }

    internal void CallAttack(int count, float periodic)
    {
        _periodicActivator.InitAndStart(SomeAction, count, periodic, EndActions);
    }

    private void SomeAction(int order)
    {

    }

    private void EndActions()
    {
        _currentState.Finish();
        _currentState = null;
    }

    [System.Serializable]
    public class PointsByState
    {
        public SpawnWeaponState WeaponState;
        public List<Transform> Points;
    }
}