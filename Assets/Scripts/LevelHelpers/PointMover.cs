using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PointMover : MonoBehaviour
{
    [SerializeField] private PeriodicActivator _periodicActivator;
    [SerializeField] private PlayerDashState _playerDashState;
    [SerializeField] private float _period;
    [SerializeField] private List<Transform> _points;

    [Inject]
    protected DiContainer _diContainer;

    public void MovePlayer()
    {
        _periodicActivator.InitAndStart(MoveObject, _points.Count, _period);
    }

    private void MoveObject(int order)
    {
        var ps = Instantiate(_playerDashState);
        ps.SetCustomTargetPoint(_points[_points.Count - order].position);
        PlayerFSM.Instance.SetPreparedState(ps);
    }
}