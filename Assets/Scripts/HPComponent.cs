using UnityEngine;
using UnityEngine.Events;

public class HPComponent : MonoBehaviour
{
    [SerializeField] private float TimeoutRegen;
    [SerializeField] private UnityEvent _eventByDeath;

    private int MaxHP;
    private int CurrentHP;

    private Chapter _chapter;
    private float _timeOut;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        //Edit for available with mobs
        _chapter = ControllerDemoSaveFile.Instance.mainData.chapter;

        MaxHP = _chapter.GetMaxHP;
        CurrentHP = _chapter.GetMaxHP;
    }

    internal void GetDamage(int damageCount)
    {
        CurrentHP -= damageCount;
        _timeOut = TimeoutRegen;
        if (CurrentHP <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        CurrentHP = 0;
        _eventByDeath?.Invoke();
    }

    private void Update()
    {
        if (CurrentHP > 0 && _chapter.HPRegenBySecond > 0)
        {
            if (_timeOut > 0f)
            {
                _timeOut -= Time.deltaTime;
            }
            else if (CurrentHP < MaxHP)
            {
                CurrentHP += _chapter.HPRegenBySecond;
            }
        }
    }
}