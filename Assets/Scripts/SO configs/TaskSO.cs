﻿using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/TaskSO", order = 1)]
public class TaskSO : ScriptableObject
{
    public string KeyTitle;
    public string KeyLocDesc;
//    public string Name;
//    public string Description;
    public List<EnumLevels> AvailableLevels;

//    public string GetDescription => Description;
}