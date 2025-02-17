using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MainData
{
    public int testSaveParam = 0;
    public ProgressHistory progressHistory = new ProgressHistory();
    public Chapter chapter = new Chapter();

    public bool EmptyData => progressHistory.CurrentTask == 0;

    public void SetTask(int newTask)
    {
        progressHistory.SetTask(newTask);
        SaveController.Save(this);
    }

    internal void SetLevel(EnumLevels enumLevel)
    {
        testSaveParam++;
//        Debug.LogWarning($"SetLevel {enumLevel}");
        progressHistory.CurrentScene = (int)enumLevel;
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

public class GamePlayProgress
{
    //???
}

[Serializable]
public class Chapter
{
    public int MaxHP = 100;
    public int HPRegenBySecond = 2;
    public int BaseDamage = 10;

    public int GetMaxHP => MaxHP;
}