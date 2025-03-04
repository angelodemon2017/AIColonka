using System;
using System.Collections.Generic;

[Serializable]
public class MainData
{
    public int testSaveParam = 0;
    public ProgressHistory progressHistory = new ProgressHistory();
    public Chapter chapter = new Chapter();
    public GamePlayProgress gamePlayProgress = new GamePlayProgress();
    public LevelsState levelsState = new LevelsState();

    public Action BitUpgrade;

    public bool EmptyData => string.IsNullOrWhiteSpace(progressHistory.KeyMainTask);

    public void SetTask(TaskSO task)
    {
        progressHistory.SetTask(task.KeyTitle);
        SaveController.Save(this);
    }

    internal void SetLevel(EnumLevels enumLevel)
    {
        testSaveParam++;
        progressHistory.CurrentScene = (int)enumLevel;
        SaveController.Save(this);
    }

    internal bool WasPick(EnumLevelProp levelProp)
    {
        return levelsState.WasPick(levelProp);
    }

    internal void PickProp(EnumLevelProp levelProp)
    {
        levelsState.PickProp(levelProp);

        if ((levelProp | EnumLevelProp.IsBits) == EnumLevelProp.IsBits)
        {
            AddBits(1);
        }
        if ((levelProp | EnumLevelProp.IsAVs) == EnumLevelProp.IsAVs)
        {
            AddAVP(1);
        }

        SaveController.Save(this);
    }

    internal void AddBits(int bits)
    {
        gamePlayProgress.BitAdd(bits);

        BitUpgrade?.Invoke();
    }

    internal void AddAVP(int power)
    {
        gamePlayProgress.AVPUpgrade();

        BitUpgrade?.Invoke();
    }
}

[Serializable]
public class ProgressHistory
{
    public Action TaskUpdate;

    public int CurrentScene;
    public int RoomConfig = 0;
    public int CurrentTask = 0;
    public string KeyMainTask;
    public List<string> WasDones = new();

    public void SetTask(string keyTask)
    {
        if (!WasDones.Contains(KeyMainTask))
        {
            WasDones.Add(KeyMainTask);
        }
        KeyMainTask = keyTask;
        TaskUpdate?.Invoke();
    }

    public bool IsWasDone(string keyTask)
    {
        return WasDones.Contains(keyTask);
    }
}

[Serializable]
public class GamePlayProgress
{
    public int BattleBits;
    public int AVPower;

    public void BitAdd(int bits)
    {
        BattleBits += bits;
        BattleBits = Math.Clamp(BattleBits, 0, 9);
    }

    public void BitUpgrade()
    {
        BattleBits++;
    }

    public void AVPUpgrade()
    {
        AVPower++;
    }
}

[Serializable]
public class LevelsState
{
    public EnumLevelProp LevelProps = EnumLevelProp.None;

    public bool WasPick(EnumLevelProp prop)
    {
        return (LevelProps & prop) == prop;
    }

    public void PickProp(EnumLevelProp prop)
    {
        LevelProps |= prop;
    }
}

[Serializable]
public class Chapter
{
    public int MaxHP = 100;
    public int HPRegenBySecond = 2;
    public int BaseDamage = 10;

    public int GetMaxHP => MaxHP;
}