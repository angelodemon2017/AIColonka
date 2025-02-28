using UnityEngine;
using TMPro;
using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;

public class PanelDialogWithPeople : MAINWindow
{
    [SerializeField] private float _charInterval;
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
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        Init();
    }

    private void Init()
    {
        _currentDialog = ControllerDemoSaveFile.Instance.CurrentDialog;
        SelectVariant(0);
    }

    public override void Run()
    {
        base.Run();
        ButtonChecker();
    }

    private void ButtonChecker()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _currentDialog.dialogSteps[_currentStep].dialogVariants.Count == 1)
        {
            SelectedVariant(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectVariantByKey(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectVariantByKey(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectVariantByKey(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectVariantByKey(3);
        }
    }

    private void SelectVariant(int dialogStep)
    {
        _currentStep = dialogStep;
        _ = ShowDialogStepAsync();
    }

    private async Task ShowDialogStepAsync()
    {
        var dialogStep = _currentDialog.dialogSteps[_currentStep];
        _textNamePerson.text = string.Empty;
        _textPerson.text = string.Empty;

        ActionByKey?.Invoke(dialogStep.KeyPersonTextV0);
        _textNamePerson.text = await Localizations.GetLocalizedText(
            Localizations.Tables.Characters, dialogStep.Chapter.ToString());

        var textPerson = await Localizations.GetLocalizedText(
            Localizations.Tables.Dialogs, dialogStep.KeyPersonTextV0);
        StartCoroutine(Writer(textPerson, 0));

        _parentVariants.DestroyChildrens();
        bool onlyOne = dialogStep.dialogVariants.Count == 1;
        for (int i = 0; i < dialogStep.dialogVariants.Count; i++)
        {
            var diaVarPan = Instantiate(_dialogVariantPrefab, _parentVariants);
            var textVariant = await Localizations.GetLocalizedText(
                Localizations.Tables.Dialogs, dialogStep.dialogVariants[i].KeyVariant);

            diaVarPan.Init(onlyOne ? -1 : i, textVariant, _charInterval, SelectedVariant);
        }
    }

    IEnumerator Writer(string fullText, int chars)
    {
        yield return new WaitForSeconds(_charInterval);
        string space = string.Empty;
        for (int i = 0; i < fullText.Length - chars; i++)
        {
            space += " ";
        }
        _textPerson.text = fullText.Substring(0, chars) + space;
        if (chars < fullText.Length)
        {
            StartCoroutine(Writer(fullText, chars + 1));
        }
    }

    private void SelectVariantByKey(int num)
    {
        if (num < _currentDialog.dialogSteps[_currentStep].dialogVariants.Count)
        {
            SelectedVariant(num);
        }
    }

    private void SelectedVariant(int num)
    {
        var dialogVariant = _currentDialog.dialogSteps[_currentStep].dialogVariants[num];
        ActionByKey?.Invoke(dialogVariant.KeyVariant);
        if (dialogVariant.IdStepDialog >= _currentDialog.dialogSteps.Count - 1)
        {
            ActionByKey?.Invoke(_currentDialog.dialogSteps.LastOrDefault().KeyPersonTextV0);
            NextWindow();
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