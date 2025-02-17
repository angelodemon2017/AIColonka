using System.Collections.Generic;
using UnityEngine;

public class ArmorVisualizator : MonoBehaviour
{
    [SerializeField] private WhoIs _whoIs;

    [SerializeField] private List<Transform> _points1;
    [SerializeField] private List<Transform> _points2;
    [SerializeField] private List<Transform> _points3;

    [SerializeField] private Weapon _weaponFrefab;
    [SerializeField] private Transform _target;

    internal void SetTarget(Transform newTarget)
    {
        _target = newTarget;
    }

    internal void CallAttack(TypeVisualAttack typeAttack)
    {
        switch (typeAttack)
        {
            case TypeVisualAttack.Near:
            case TypeVisualAttack.Middle:
            case TypeVisualAttack.Spec:
                CallNearAttack();
                break;
        }
    }

    private void CallNearAttack()
    {
        var tempTrans = _points1.GetRandom();
        var tempWeapon = Instantiate(_weaponFrefab);
        tempWeapon.Init(_whoIs.whoIs,
            tempTrans,
            _target,
            _whoIs.whoIs == EnumWhoIs.Player ?
                CameraController.Instance.transform.transform.rotation :
                tempTrans.rotation);
    }

    public enum TypeVisualAttack
    { 
        Near,
        Middle,
        Spec,
        //*super*?
    }
}