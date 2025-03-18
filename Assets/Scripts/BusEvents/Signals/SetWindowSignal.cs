[System.Serializable]
public class SetWindowSignal
{
    public MAINWindow SelectWindow;

    public SetWindowSignal(MAINWindow selectWindow)
    {
        SelectWindow = selectWindow;
    }
}