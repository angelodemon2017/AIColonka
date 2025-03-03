using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FuryModule : EntityModule
{
    [SerializeField] private Transform _pointUp;
    [SerializeField] private Transform _pointDown;
    [SerializeField] private HPComponent _hPComponent;
    [SerializeField] private List<PhaseByHp> phases;

    internal Transform PointUp => _pointUp;
    internal Transform PointDown => _pointDown;


    internal override void Init()
    {
        base.Init();

    }

    internal int GetPhase()
    {
        var perc = _hPComponent.GetPercentHP;
        var phas = phases.FirstOrDefault(p => p.percentHPMax > perc && p.percentHPMin < perc);
        if (phas != null)
        {
            return phas.Phase;
        }
        return 0;
    }

    [System.Serializable]
    public class PhaseByHp
    {
        [Range(0f, 1f)] public float percentHPMin;
        [Range(0f, 1f)] public float percentHPMax;
        public int Phase;
    }
}