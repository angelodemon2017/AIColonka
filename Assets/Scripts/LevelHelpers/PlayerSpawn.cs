using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private List<Transform> _points;

    private int CurrentIndex => 0;

    private void Start()
    {
        InitPlayer(_points.Count > 0 ? _points[CurrentIndex] : transform);
    }

    private void InitPlayer(Transform pointPlayer)
    {
        Instantiate(_player, pointPlayer.position, pointPlayer.rotation);
    }
}