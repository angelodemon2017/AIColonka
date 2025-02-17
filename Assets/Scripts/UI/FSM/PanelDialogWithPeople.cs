using UnityEngine;
using TMPro;
using System;

public class PanelDialogWithPeople : MAINWindow
{
    [SerializeField] private SceneLevelLoader _sceneLevelLoader;
    [SerializeField] private DialogVariant _dialogVariantPrefab;
    [SerializeField] private Transform _parentVariants;
    [SerializeField] private TextMeshProUGUI _textNamePerson;
    [SerializeField] private TextMeshProUGUI _textPerson;

    private DialogSO _currentDialog;
    private int _currentStep = 0;

    public Action NextStep;
    public Action EndDialog;

    public override void StartWindow()
    {
        base.StartWindow();
        //TODO replace event bus or fix
        //        EventBus.Subscribe<DialogVariantSO>(SelectVariant);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        Init();
    }

    private void Init()
    {
        _currentDialog = ControllerDemoSaveFile.Instance.CurrentDialog;
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
            _sceneLevelLoader.LoadLevel(dialogVariant.specEndDialog.moveToLevel);
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