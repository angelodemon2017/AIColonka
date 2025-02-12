using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "SO/DialogsConfig", order = 1)]
public class DialogsConfig : ScriptableObject
{
    public List<Dialog> dialogs = new();
}

[System.Serializable]
public class Dialog
{
    public string Name;
    public List<DialogStep> dialogSteps = new();
}

[System.Serializable]
public class DialogStep
{
    public string TextPerson;
    public int IdStep;
    public EnumChapter Chapter;
    public List<DialogVariantSO> dialogVariants = new();
}

[System.Serializable]
public class DialogVariantSO : IEvent
{
    public string TextVariant;
    public int IdStepDialog;
    public SpecEndDialog specEndDialog;
}

public struct DialogVariantStruct : IEvent
{
    
}

[System.Serializable]
public class SpecEndDialog
{
    public EnumLevels moveToLevel;
    public MAINWindow moveWindow;
    public bool RunNextScriptStep;

//    public bool IsSpec => moveWindow != null || moveToLevel != EnumLevels.MainMenu;
}