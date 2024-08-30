using System.Collections.Generic;
using UnityEngine;

public class GenerationPlatform : MonoBehaviour
{
    [SerializeField] private List<Transform> pointsSpawn;
    [SerializeField] private float TimeSpawn;
    [SerializeField] private GameObject generatingObject;

    public int _count = 0;
    private float _timerSpawn = 1f;
    public bool ReadyGiveItem => _count > 0;

    private void Awake()
    {
        
    }

    private void Update()
    {
        if (_timerSpawn > 0f)
        {
            _timerSpawn -= Time.deltaTime;
        }
        if (_timerSpawn < 0)
        {
            Spawn();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"OnCollisionEnter {collision.gameObject.name}");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OnTriggerEnter {other.name}");
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log($"OnTriggerStay {other.name}");

        if (_count < 1)
        {
            return;
        }

        var ec = other.GetComponent<EnemyController>();
        if (ec != null && ec.canGetKeepObject)
        {
            ec.TakeObject(Give());
        }
    }

    public GameObject Give()
    {
        _count--;
        _timerSpawn = TimeSpawn;
        return pointsSpawn[_count].GetChild(0).gameObject;
    }

    private void Spawn()
    {
        if (_count > pointsSpawn.Count - 1)
        {
            return;
        }

        var newgo = Instantiate(generatingObject, pointsSpawn[_count]);

        newgo.transform.position = pointsSpawn[_count].position;

        _count++;

        if (_count < pointsSpawn.Count)
        {
            _timerSpawn = TimeSpawn;
        }
    }
}