using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainData
{
    public ProgressHistory progressHistory = new ProgressHistory();
}

public class ProgressHistory
{
    public Action TaskUpdate;

    public int Scene; //Or room
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