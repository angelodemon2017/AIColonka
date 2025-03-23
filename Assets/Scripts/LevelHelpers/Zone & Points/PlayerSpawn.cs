using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] private PlayerFSM _playerFSM;
    [SerializeField] private List<ConditionPoint> _points;
    [SerializeField] private List<ConditionPoint> _pointsByCurrentTask;
    [SerializeField] private WindowGameplay _windowGameplay;

    private SignalBus _signalBus;
    private DiContainer _container;
    private DataHandler _dataHandler;
    private GameplayHandler _gameplayHandler;

    [Inject]
    private void Construct(
        SignalBus signalBus,
        GameplayHandler gameplayHandler,
        DiContainer container,
        DataHandler dataHandler)
    {
        _signalBus = signalBus;
        _gameplayHandler = gameplayHandler;
        _container = container;
        _dataHandler = dataHandler;

        Init();
    }

    private void Init()
    {
        InitPlayer(GetPoint());
    }

    private void InitPlayer(Transform pointPlayer)
    {
        var newInst = _container.InstantiatePrefabForComponent<PlayerFSM>(_playerFSM, pointPlayer.position, pointPlayer.rotation, pointPlayer);
        _gameplayHandler.UpdatePlayerInstance(newInst);
    }

    private void Start()
    {
        _signalBus.Fire(new SetWindowSignal(_windowGameplay));
    }

    private Transform GetPoint()
    {
        foreach (var cp in _points)
        {
            if (!_dataHandler.WasDone(cp.Task))
            {
                return cp.Point;
            }
        }

        foreach (var cp in _pointsByCurrentTask)
        {
            if (_dataHandler.IsCurrentTask(cp.Task))
            {
                return cp.Point;
            }
        }

        return transform;
    }

    private void OnDrawGizmos()
    {
        DrawGizmosHelper.DrawLabel(transform, 1f, "SpawnPoint");

        foreach (var p in _points)
        {
            if (p != null)
            {
                DrawGizmosHelper.DrawLabel(p.Point, 1f, $"SpawnPoint {(p.Task != null ? $"by {p.Task.name}"  : "NEED TASK")}");
            }
        }
    }

    [System.Serializable]
    public class ConditionPoint
    {
        public TaskSO Task;
        public Transform Point;
    }
}