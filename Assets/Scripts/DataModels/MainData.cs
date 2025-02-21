using System;

[Serializable]
public class MainData
{
    public int testSaveParam = 0;
    public ProgressHistory progressHistory = new ProgressHistory();
    public Chapter chapter = new Chapter();
    public GamePlayProgress gamePlayProgress = new GamePlayProgress();
    public LevelsState levelsState = new LevelsState();

    public Action BitUpgrade;

    public bool EmptyData => progressHistory.CurrentTask == 0;

    public void SetTask(int newTask)
    {
        progressHistory.SetTask(newTask);
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
            gamePlayProgress.BitUpgrade();

            BitUpgrade?.Invoke();
        }
        if ((levelProp | EnumLevelProp.IsAVs) == EnumLevelProp.IsAVs)
        {
            gamePlayProgress.AVPUpgrade();
        }

        SaveController.Save(this);
    }
}

[Serializable]
public class ProgressHistory
{
    public Action TaskUpdate;

    public int CurrentScene;
    public int RoomConfig = 0;
    public int CurrentTask = 0;

    public void SetTask(int newTask)
    {
        CurrentTask = newTask;
        TaskUpdate?.Invoke();
    }
}

[Serializable]
public class GamePlayProgress
{
    public int BattleBits;
    public int AVPower;

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