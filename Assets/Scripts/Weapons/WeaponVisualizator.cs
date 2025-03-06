using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PeriodicActivator))]
public class WeaponVisualizator : MonoBehaviour
{
    [SerializeField] private WhoIs _whoIs;
    [SerializeField] private PeriodicActivator _periodicActivator;
    [SerializeField] private List<PointsByState> _pointsByStates;

    private IWVState _currentIState;
    private Dictionary<string, List<Transform>> _cashPointsByStates = new();
    private Transform _tempSpawnPoint;

    private State _curState => _currentIState as State;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _pointsByStates.ForEach(p => _cashPointsByStates.Add(p.WeaponState.Key, p.Points));
    }

    internal void CallAttack(IWVState state)
    {
        _currentIState = state;
        CallAttack(_currentIState.GetCountLaunched, _currentIState.GetIntervalLaunched);
    }

    internal void CallAttack(int count, float periodic)
    {
        _periodicActivator.InitAndStart(SomeAction, count, periodic, EndActions);
    }

    private void SomeAction(int order)
    {
        if (_cashPointsByStates.TryGetValue(_curState.Key, out List<Transform> points))
        {
            _tempSpawnPoint = points.GetElementByOrder(order);
        }
        else
        {
            _tempSpawnPoint = transform;
        }

        var weapon = Instantiate(_currentIState.GetWeapon, _tempSpawnPoint.position, _tempSpawnPoint.rotation);
        weapon.Init(_whoIs.whoIs, _tempSpawnPoint, GetTarget(), _tempSpawnPoint.rotation);
    }

    private Transform GetTarget()
    {
        if (_whoIs.whoIs != EnumWhoIs.Player)
        {
            return PlayerFSM.Instance.PointOfTargetForEnemy;
        }
        else
        {
            return PlayerFSM.Instance.GetPoints.TransfTarget;
        }
    }

    private void EndActions()
    {
        _curState.Finish();
//        _currentState = null;
    }

    [System.Serializable]
    public class PointsByState
    {
        public State WeaponState;
        public List<Transform> Points;
    }
}