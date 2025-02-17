using System.Collections;
using UnityEngine;

public class EntityFSM : MonoBehaviour
{
    [SerializeField] private ArmorVisualizator _armorVisualizator;
    [SerializeField] private PanelHP _UIpanelHP;
    [SerializeField] private HPComponent _hpComponent;

    private void Awake()
    {
        StartCoroutine(Launch());
        _hpComponent.ChangeHP += _UIpanelHP.UpdateHP;
        _hpComponent.OnChangeHP();
    }

    IEnumerator Launch()
    {
        _armorVisualizator.SetTarget(PersonMovement.Instance.PointOfTargetForEnemy);
        _armorVisualizator.CallAttack(ArmorVisualizator.TypeVisualAttack.Middle);

        yield return new WaitForSeconds(2f);

        StartCoroutine(Launch());
    }

    private void OnDestroy()
    {
        _hpComponent.ChangeHP -= _UIpanelHP.UpdateHP;
    }
}