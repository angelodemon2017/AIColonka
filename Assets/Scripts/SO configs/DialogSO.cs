using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Dialog", order = 1)]
public class DialogSO : ScriptableObject
{
    public string Name;
    public List<DialogStep> dialogSteps = new();
}