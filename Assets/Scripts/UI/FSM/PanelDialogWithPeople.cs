using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PanelDialogWithPeople : MAINWindow
{
    [SerializeField] private DialogVariant _dialogVariantPrefab;
    [SerializeField] private Transform _parentVariants;
    [SerializeField] private TextMeshProUGUI _textNamePerson;
    [SerializeField] private TextMeshProUGUI _textPerson;

    private Dialog _currentDialog;
    private int _currentStep = 0;

    public override void StartWindow()
    {
        base.StartWindow();
        //TODO replace event bus or fix
        EventBus.Subscribe<DialogVariantSO>(SelectVariant);
        Init();
    }

    private void Init()
    {
        var currentDialog = ControllerDemoSaveFile.Instance.mainData.progressHistory.Dialog;
        _currentDialog = ControllerDemoSaveFile.Instance.DialogsConfig.dialogs[currentDialog];
        SelectVariant(0);
    }

    private void SelectVariant(DialogVariantSO variant)
    {
        SelectVariant(variant.IdStepDialog);
    }

    private void SelectVariant(int dialogStep)
    {
        _currentStep = dialogStep;
        ShowDialogStep();
    }

    private void ShowDialogStep()
    {
        var dialogStep = _currentDialog.dialogSteps[_currentStep];

        _textNamePerson.text = dialogStep.Chapter.ToString();
        _textPerson.text = dialogStep.TextPerson;

        _parentVariants.DestroyChildrens();
        foreach (var vart in dialogStep.dialogVariants)
        {
            var diaVarPan = Instantiate(_dialogVariantPrefab, _parentVariants);
            diaVarPan.Init(vart);
        }
    }
}