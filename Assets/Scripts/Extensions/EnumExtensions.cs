using System.Collections.Generic;

public static class EnumExtensions
{
    private static Dictionary<EnumWhoIs, EnumWhoIs> _enemies = new()
    {
        { EnumWhoIs.Player, EnumWhoIs.Enemy },
        { EnumWhoIs.FriendPlayer, EnumWhoIs.Enemy },
        { EnumWhoIs.Enemy, EnumWhoIs.Player | EnumWhoIs.FriendPlayer },
        { EnumWhoIs.Wall, EnumWhoIs.None },
    };

    private static Dictionary<EnumWhoIs, EnumWhoIs> _friends = new()
    {
        { EnumWhoIs.Player, EnumWhoIs.Player | EnumWhoIs.FriendPlayer },
        { EnumWhoIs.FriendPlayer, EnumWhoIs.Player | EnumWhoIs.FriendPlayer },
        { EnumWhoIs.Enemy, EnumWhoIs.Enemy },
        { EnumWhoIs.Wall, EnumWhoIs.None },
    };

    public static EnumCollisionResult GetColResult(this EnumWhoIs whoIs, EnumWhoIs otherWhoIs)
    {
        if (whoIs.Equals(otherWhoIs))
        {
            return EnumCollisionResult.Friend;
        }

        if (!(_enemies[whoIs] & otherWhoIs).Equals(EnumWhoIs.None))
        {
            return EnumCollisionResult.Enemy;
        }
        if (!(_friends[whoIs] & otherWhoIs).Equals(EnumWhoIs.None))
        {
            return EnumCollisionResult.Friend;
        }

        return EnumCollisionResult.Other;
    }
}