[System.Flags]
public enum EnumLevelProp : int
{
    None = 0,

    BitUpgrade1 = 1 << 0,
    BitUpgrade2 = 1 << 1,
    BitUpgrade3 = 1 << 2,
    BitUpgrade4 = 1 << 3,
    BitUpgrade5 = 1 << 4,
    BitUpgrade6 = 1 << 5,
    BitUpgrade7 = 1 << 6,
    BitUpgrade8 = 1 << 7,
    IsBits = BitUpgrade1 | BitUpgrade2 | BitUpgrade3 | BitUpgrade4 | BitUpgrade5 | BitUpgrade6 | BitUpgrade7 | BitUpgrade8,

    AVUpgrade1 = 1 << 8,
    AVUpgrade2 = 1 << 9,
    AVUpgrade3 = 1 << 10,
    AVUpgrade4 = 1 << 11,
    IsAVs = AVUpgrade1 | AVUpgrade2 | AVUpgrade3 | AVUpgrade4,

    LastBit = 1 << 32,
}