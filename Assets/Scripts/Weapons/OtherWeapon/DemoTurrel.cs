using System.Collections;
using UnityEngine;

public class DemoTurrel : MonoBehaviour
{
    [SerializeField] private ArmorVisualizator _armorVisualizator;
    [SerializeField] private float _minInterval;
    [SerializeField] private float _maxInterval;

    private float GetInterval => Random.Range(_minInterval, _maxInterval);

    private void Awake()
    {
    }

    public void StartAttack()
    {
        StartCoroutine(Attack());
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(GetInterval);

        _armorVisualizator.SetTarget(PlayerFSM.Instance.PointOfTargetForEnemy);
        _armorVisualizator.CallAttack(ArmorVisualizator.TypeVisualAttack.Near);

        StartCoroutine(Attack());
    }
}