using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Dialog", order = 1)]
public class DialogSO : ScriptableObject
{
    public bool IsAutoValidate;
    public int IndexDialog;
    public string Name;
    public List<DialogStep> dialogSteps = new();

    private void OnValidate()
    {
        if (IsAutoValidate)
        {
            for (var i = 0; i < dialogSteps.Count; i++)
            {
                dialogSteps[i].IdStep = i;
                dialogSteps[i].KeyPersonTextV0 = $"D{IndexDialog}_S{dialogSteps[i].IdStep}_V0";
                for (var i2 = 0; i2 < dialogSteps[i].dialogVariants.Count; i2++)
                {
                    dialogSteps[i].dialogVariants[i2].KeyVariant = $"D{IndexDialog}_S{dialogSteps[i].IdStep}_V{i2 + 1}";
                }
            }
        }
    }
}