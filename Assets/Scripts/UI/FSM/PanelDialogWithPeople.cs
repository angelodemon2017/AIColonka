using UnityEngine;
using TMPro;
using System;
using System.Threading.Tasks;

public class PanelDialogWithPeople : MAINWindow
{
    [SerializeField] private SceneLevelLoader _sceneLevelLoader;
    [SerializeField] private DialogVariant _dialogVariantPrefab;
    [SerializeField] private Transform _parentVariants;
    [SerializeField] private TextMeshProUGUI _textNamePerson;
    [SerializeField] private TextMeshProUGUI _textPerson;
    [SerializeField] private MAINWindow _defaultNextWindow;

    private DialogSO _currentDialog;
    private int _currentStep = 0;

    public static Action<string> ActionByKey;
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
        _ = ShowDialogStepAsync();
    }

    private async Task ShowDialogStepAsync()
    {
        var dialogStep = _currentDialog.dialogSteps[_currentStep];

        ActionByKey?.Invoke(dialogStep.KeyPersonTextV0);
        _textNamePerson.text = await Localizations.GetLocalizedText(
            Localizations.Tables.Characters, dialogStep.Chapter.ToString());

        _textPerson.text = await Localizations.GetLocalizedText(
            Localizations.Tables.Dialogs, dialogStep.KeyPersonTextV0);

        _parentVariants.DestroyChildrens();
        for (int i = 0; i < dialogStep.dialogVariants.Count; i++)
        {
            var diaVarPan = Instantiate(_dialogVariantPrefab, _parentVariants);
            var textVariant = await Localizations.GetLocalizedText(
                Localizations.Tables.Dialogs, dialogStep.dialogVariants[i].KeyVariant);
            diaVarPan.Init(i, textVariant, SelectedVariant);
        }
    }

    private void SelectedVariant(int num)
    {
        var dialogVariant = _currentDialog.dialogSteps[_currentStep].dialogVariants[num];
        ActionByKey?.Invoke(dialogVariant.KeyVariant);
        if (dialogVariant.IdStepDialog >= _currentDialog.dialogSteps.Count - 1)
        {
            NextWindow();
        }
        else
       {
            SelectVariant(dialogVariant.IdStepDialog);
        }


        /*        if (dialogVariant.specEndDialog.RunNextScriptStep)
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
                }/**/
    }

    public override void ExitWindow()
    {
        base.ExitWindow();
        EndDialog?.Invoke();
    }

    private void NextWindow()
    {
        _currentDialog._eventByEnd?.Invoke();
        if (_currentDialog.levelByEndDialog != EnumLevels.MainMenu)
        {
            _sceneLevelLoader.LoadLevel(_currentDialog.levelByEndDialog);
        }
        else
        {
            UIFSM.Instance.OpenWindow(_currentDialog._nextWindow ? _currentDialog._nextWindow : _defaultNextWindow);
        }
    }
}