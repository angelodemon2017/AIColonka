using System;

[Serializable]
public class SetLevelSignal
{
    public EnumLevels Level;

    public SetLevelSignal(EnumLevels level)
    {
        Level = level;
    }
}