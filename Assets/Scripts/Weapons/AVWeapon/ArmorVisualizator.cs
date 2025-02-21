using System.Collections.Generic;
using UnityEngine;

public class ArmorVisualizator : MonoBehaviour
{
    [SerializeField] private WhoIs _whoIs;

    [SerializeField] private FallingController _fallingController;
    [SerializeField] private List<Transform> _points1;
    [SerializeField] private List<Transform> _points2;
    [SerializeField] private List<Transform> _points3;

    [SerializeField] private List<AVWeapon> _avWeapon;
    [SerializeField] private Transform _target;

    private Points _points;

    private int levelAVPower => ControllerDemoSaveFile.Instance.mainData.gamePlayProgress.AVPower;
    internal WhoIs GetWhoIs => _whoIs;

    internal void SetTarget(Transform newTarget)
    {
        _target = newTarget;
    }

    internal void SetPoints(Points points)
    {
        _points = points;
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
        var tempWeapon = Instantiate(_avWeapon.GetRandom());
        tempWeapon.InitAVW(levelAVPower, isAir: !_fallingController.IsGrounded);
        tempWeapon.Init(_whoIs.whoIs,
            tempTrans,
            _points.EnemyIsTarget ?
                _points.TargetEnemy.transform :
                null,
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