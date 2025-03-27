using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PointMover : MonoBehaviour
{
    [SerializeField] private PeriodicActivator _periodicActivator;
    [SerializeField] private PlayerDashState _playerDashState;
    [SerializeField] private float _period;
    [SerializeField] private List<Transform> _points;

    private GameplayHandler _gameplayHandler;

    [Inject]
    private void Construct(
        GameplayHandler gameplayHandler)
    {
        _gameplayHandler = gameplayHandler;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, _points[0].position);
        for (int i = 0; i < _points.Count; i++)
        {
            if (i >= _points.Count - 1)
            {
                break;
            }
            Gizmos.DrawLine(_points[i].position, _points[i+1].position);
        }
    }

    public void MovePlayer()
    {
        _periodicActivator.InitAndStart(MoveObject, _points.Count, _period);
    }

    private void MoveObject(int order)
    {
        var ps = Instantiate(_playerDashState);
        ps.SetCustomTargetPoint(_points[_points.Count - order].position);
        _gameplayHandler.PlayerInstance.SetPreparedState(ps);
    }
}