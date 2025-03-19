using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private PlayerFSM _playerFSM;
    [SerializeField] private List<ConditionPoint> _points;

    [Inject] private DataHandler _dataHandler;
    [Inject] private DiContainer _container;

    private void Start()
    {
        InitPlayer(GetPoint());
    }

    private void InitPlayer(Transform pointPlayer)
    {
        _container.InstantiatePrefabForComponent<PlayerFSM>(_playerFSM, pointPlayer.position, pointPlayer.rotation, pointPlayer);
//        Instantiate(_player, pointPlayer.position, pointPlayer.rotation, pointPlayer);
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