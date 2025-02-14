using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WhoIs))]
public class ArmorVisualizator : MonoBehaviour
{
    [SerializeField] private WhoIs _whoIs;

    [SerializeField] private List<Transform> _points1;
    [SerializeField] private List<Transform> _points2;
    [SerializeField] private List<Transform> _points3;

    [SerializeField] private Weapon _weaponFrefab;

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
        var tempWeapon = Instantiate(_weaponFrefab,
            tempTrans.position,
            _whoIs.whoIs == EnumWhoIs.Player ?
                CameraController.Instance.transform.transform.rotation :
                tempTrans.rotation);
//            tempTrans);
        tempWeapon.Init(_whoIs.whoIs);
    }

    public enum TypeVisualAttack
    { 
        Near,
        Middle,
        Spec,
        //*super*?
    }
}