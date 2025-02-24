using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.Localization.Settings;

public static class Localizations
{
    public static async Task<string> GetLocalizedText(string table, string key)
    {
        var op = LocalizationSettings.StringDatabase.GetLocalizedStringAsync(table, key);

        await op.Task;

        return op.Result;
    }

    public static class Tables
    {
        public static string LevelsTable = "Levels";
        public static string BackTalksTable = "BackTalks";
        public static string GamePlay = "GamePlay";
    }

    public static class Levels
    {
        public static Dictionary<EnumLevels, string> MapLevelKeys = new()
        {
            { EnumLevels.MainMenu, MAIN_MENU },
            { EnumLevels.DialogsHub, DIALOGS_ROOMS },
            { EnumLevels.HomeInStation, HOMES_STATION },
            { EnumLevels.CenterLicence, CENTER_LICENSE_UPDATE },
            { EnumLevels.BlackMarket, BLACK_MARKET },
            { EnumLevels.LibraryArchive, LIBRARY_ARCHIVE },
            { EnumLevels.WarPolygon, WAR_POLYGON },
            { EnumLevels.ArtHost, ART_HOST },
            { EnumLevels.MarketPlace, MARKET_PLACE },
            { EnumLevels.BDServer, BDSERVER },
            { EnumLevels.PsycologyRoom, PSYCOLOGY_ROOM },
            { EnumLevels.Subconscious, SUBCONSCIOUS },
        };

        public const string MAIN_MENU = "MAIN_MENU";
        public const string DIALOGS_ROOMS = "DIALOGS_ROOMS";
        public const string HOMES_STATION = "HOMES_STATION";
        public const string CENTER_LICENSE_UPDATE = "CENTER_LICENSE_UPDATE";
        public const string BLACK_MARKET = "BLACK_MARKET";
        public const string LIBRARY_ARCHIVE = "LIBRARY_ARCHIVE";
        public const string WAR_POLYGON = "WAR_POLYGON";
        public const string ART_HOST = "ART_HOST";
        public const string MARKET_PLACE = "MARKET_PLACE";
        public const string BDSERVER = "BDSERVER";
        public const string PSYCOLOGY_ROOM = "PSYCOLOGY_ROOM";
        public const string SUBCONSCIOUS = "SUBCONSCIOUS";
    }

    public static class BackGroundTalk
    {
        public const string WAR_TALK1 = "WAR_TALK1";
        public const string WAR_TALK2 = "WAR_TALK2";
        public const string WAR_TALK3 = "WAR_TALK3";
    }

    public static class GamePlay
    {
        public const string TALK = "TALK";
    }
}