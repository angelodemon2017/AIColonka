using UnityEngine;

public class BattleZoneActivator : MonoBehaviour
{
    internal bool IsDone;

    private BattleZone _battleZone;

    public void DoDone()
    {
        IsDone = true;
        CallCheck();
    }

    internal void Init(BattleZone battleZone)
    {
        _battleZone = battleZone;
    }

    internal void CallCheck()
    {
        _battleZone.CheckZone();
    }

    private void OnDestroy()
    {
        DoDone();
    }
}