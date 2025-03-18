[System.Serializable]
public class SetNextDialogSignal
{
    public DialogSO NextDialog;

    public SetNextDialogSignal(DialogSO nextDialog)
    {
        NextDialog = nextDialog;
    }
}