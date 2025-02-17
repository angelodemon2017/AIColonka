using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityFSM : MonoBehaviour
{
    [SerializeField] private ArmorVisualizator _armorVisualizator;

    private void Awake()
    {
        StartCoroutine(Launch());
    }

    IEnumerator Launch()
    {
        _armorVisualizator.SetTarget(PersonMovement.Instance.PointOfTargetForEnemy);
        _armorVisualizator.CallAttack(ArmorVisualizator.TypeVisualAttack.Middle);

        yield return new WaitForSeconds(2f);

        StartCoroutine(Launch());
    }
}