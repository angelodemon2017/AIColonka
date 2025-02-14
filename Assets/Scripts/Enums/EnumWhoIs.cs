[System.Flags]
public enum EnumWhoIs : byte
{
    None = 0b_0000,
    Player = 0b_0000_0001,
    FriendPlayer = 0b_0000_0010,
    Enemy = 0b_0000_0100,
    Wall = 0b_0000_1000,
}