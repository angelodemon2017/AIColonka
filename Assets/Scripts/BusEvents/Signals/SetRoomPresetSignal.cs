using System;

[Serializable]
public class SetRoomPresetSignal
{
    public EnumDialogRoomPreset IdConfig;

    public SetRoomPresetSignal(EnumDialogRoomPreset idConfig)
    {
        IdConfig = idConfig;
    }
}