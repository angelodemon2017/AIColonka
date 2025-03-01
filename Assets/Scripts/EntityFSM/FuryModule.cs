using UnityEngine;

public class FuryModule : EntityModule
{
    [SerializeField] private Transform _pointUp;
    [SerializeField] private Transform _pointDown;

    internal Transform PointUp => _pointUp;
    internal Transform PointDown => _pointDown;

    internal override void Init()
    {
        base.Init();

    }
}