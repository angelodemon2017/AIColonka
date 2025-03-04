[System.Flags]
public enum EnumAirGroundState : byte
{
    None = 0,

    Ground = 1 << 0,
    Air = 1 << 1,
    Any = Ground | Air,
}