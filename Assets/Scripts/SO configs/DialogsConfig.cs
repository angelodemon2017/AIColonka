using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/DialogsConfig", order = 1)]
public class DialogsConfig : ScriptableObject
{
    public List<Dialog> dialogs = new();
    public DialogSO dialogSO;
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
    public string KeyPersonTextV0;
    public int IdStep;
    public EnumChapter Chapter;
    public List<DialogVariantSO> dialogVariants = new();
}

[System.Serializable]
public class DialogVariantSO : IEvent
{
    public string TextVariant;
    public string KeyVariant;
    public int IdStepDialog;
    public int IdStepDialog2;
    public SpecEndDialog specEndDialog;
}

[System.Serializable]
public class SpecEndDialog
{
    public EnumLevels moveToLevel;
    public MAINWindow moveWindow;
    public bool RunNextScriptStep;
}