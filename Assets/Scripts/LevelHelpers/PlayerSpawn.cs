using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private List<ConditionPoint> _points;

    private void Start()
    {
        InitPlayer(GetPoint());
    }

    private void InitPlayer(Transform pointPlayer)
    {
        Instantiate(_player, pointPlayer.position, pointPlayer.rotation);
    }

    private Transform GetPoint()
    {
        foreach (var cp in _points)
        {
            if (!ControllerDemoSaveFile.Instance.WasDone(cp.Task))
            {
                return cp.Point;
            }
        }

        return transform;
    }

    [System.Serializable]
    public class ConditionPoint
    {
        public TaskSO Task;
        public Transform Point;
    }
}