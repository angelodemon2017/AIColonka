using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class PanelDialogWithPeople : MAINWindow
{
    [SerializeField] private DialogVariant _dialogVariantPrefab;
    [SerializeField] private Transform _parentVariants;
    [SerializeField] private TextMeshProUGUI _textNamePerson;
    [SerializeField] private TextMeshProUGUI _textPerson;

    private Dialog _currentDialog;
    private int _currentStep = 0;

    public Action NextStep;
    public Action EndDialog;

    public override void StartWindow()
    {
        base.StartWindow();
        //TODO replace event bus or fix
//        EventBus.Subscribe<DialogVariantSO>(SelectVariant);
        Init();
    }

    private void Init()
    {
        var currentDialog = ControllerDemoSaveFile.Instance.mainData.progressHistory.Dialog;
        _currentDialog = ControllerDemoSaveFile.Instance.DialogsConfig.dialogs[currentDialog];
        SelectVariant(0);
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
        for (int i = 0; i < dialogStep.dialogVariants.Count; i++)
        //        foreach (var vart in dialogStep.dialogVariants)
        {
            var diaVarPan = Instantiate(_dialogVariantPrefab, _parentVariants);
            diaVarPan.Init(i, dialogStep.dialogVariants[i].TextVariant, SelectedVariant);
        }
    }

    private void SelectedVariant(int num)
    {
        var dialogVariant = _currentDialog.dialogSteps[_currentStep].dialogVariants[num];

        if (dialogVariant.specEndDialog.RunNextScriptStep)
        {
            NextStep?.Invoke();
        }

        if (dialogVariant.specEndDialog.moveToLevel != EnumLevels.MainMenu)
        {
            // is trash, need screen loader
            EventBus.ResetSubs();
            SceneManager.LoadSceneAsync((int)dialogVariant.specEndDialog.moveToLevel);
        }
        else if (dialogVariant.specEndDialog.moveWindow != null)
        {
            UIFSM.Instance.OpenWindow(dialogVariant.specEndDialog.moveWindow);
        }
        else
        {
            SelectVariant(dialogVariant.IdStepDialog);
        }
    }

    public override void ExitWindow()
    {
        base.ExitWindow();
        EndDialog?.Invoke();
//        EventBus.Unsubscribe<DialogVariantSO>(SelectVariant);
    }
}