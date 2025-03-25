using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

[System.Serializable]
public class GameplayHandler : IFixedTickable
{
    #region Fields
    private SignalBus _signalBus;

    private PlayerFSM _instance;
    private WhoIs _inTarget;
    private HashSet<WhoIs> _whoAre = new();
    private string _focusHint;

    public int _combo;
    public int _hit;
    public float _hitUpdate;

    #endregion

    #region Properties

    internal PlayerFSM PlayerInstance => _instance;
    internal WhoIs InTarget => _inTarget;
    internal string FocusHint => _focusHint;
    internal bool FightMode => HaveEnemies();

    internal int Hit
    {
        get => _hit;
        set
        {
            _hit = value;
            if (_hit > 0)
            {
                _hitUpdate = 3f;
            }
            _signalBus.Fire(new MetaFightSignal());
        }
    }
    internal int Combo
    {
        get => _combo;
        set
        {
            _combo = value;
            _signalBus.Fire(new MetaFightSignal());
        }
    }

    #endregion

    [Inject]
    private void Construct(
        SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    internal void UpdatePlayerInstance(PlayerFSM instance)
    {
        _instance = instance;
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

    private bool HaveEnemies()
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
                _inTarget = GetNearestEnemy(_instance.transform.position);
            }
        }
        _signalBus.Fire(new WhoInTargetSignal(_inTarget));
    }

    internal WhoIs GetNearestEnemy(Vector3 position)
    {
        return _whoAre
            .Where(w => w.whoIs == EnumWhoIs.Enemy)
            .OrderBy(w => Vector3.Distance(position, w.transform.position))
            .FirstOrDefault();
    }

    #endregion

    public void FixedTick()
    {
        if (_hitUpdate > 0f)
        {
            _hitUpdate -= Time.fixedDeltaTime;
            if (_hitUpdate <= 0f)
            {
                Hit = 0;
            }
        }
    }

    internal void LevelUpdate()
    {
        _whoAre.Clear();
        _inTarget = null;
    }
}