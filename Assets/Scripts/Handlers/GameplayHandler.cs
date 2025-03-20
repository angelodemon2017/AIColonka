using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class GameplayHandler : IFixedTickable
{
    private SignalBus _signalBus;

    private WhoIs _inTarget;
    private HashSet<WhoIs> _whoAre = new();
    private string _focusHint;

    internal WhoIs InTarget => _inTarget;
    internal string FocusHint => _focusHint;

    [Inject]
    private void Construct(
        SignalBus signalBus)
    {
        _signalBus = signalBus;

        Init();
    }

    private void Init()
    {

    }

    internal void SetHint(string totalHint)
    {
        _focusHint = totalHint;
        _signalBus.Fire(new FocusHintSignal());
    }

    #region targetSystem

    internal void AddWho(WhoIs whoIs)
    {
        _whoAre.Add(whoIs);
    }

    internal void RemoveWho(WhoIs whoIs)
    {
        _whoAre.Remove(whoIs);
        if (_inTarget == whoIs)
        {
            UpdateTarget();
        }
    }

    internal bool HaveEnemies()
    {
        return _whoAre
            .Any(w => w.whoIs == EnumWhoIs.Enemy && w.IsAlive);
    }

    internal void UpdateTarget()
    {
        if (_inTarget)
        {
            _inTarget = null;
        }
        else
        {
            if (HaveEnemies())
            {
                GetNearestEnemy(PlayerFSM.Instance.transform.position);
            }
        }
        _signalBus.Fire(new WhoInTargetSignal(_inTarget));
    }

    internal void GetNearestEnemy(Vector3 position)
    {
        _inTarget = _whoAre
            .Where(w => w.whoIs == EnumWhoIs.Enemy)
            .OrderBy(w => Vector3.Distance(position, w.transform.position))
            .FirstOrDefault();
    }

    #endregion

    public void FixedTick()
    {

    }

    internal void LevelUpdate()
    {
        _whoAre.Clear();
        _inTarget = null;
    }
}