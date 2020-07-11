using MonsterTrainModdingAPI;
using MonsterTrainModdingAPI.Builders;
using System.Collections.Generic;
using static MonsterTrainModdingAPI.Constants.VanillaCardPoolIDs;

namespace DiscipleClan.Cards
{
    class Utils
    {
        public static string rootPath = "Disciple/chrono/";
        public static string ucardPath = "Card Assets/ProtoUnitCardArt/";
        public static string scardPath = "Card Assets/";
        public static string unitPath = "Unit Assets/";
        public static string relicPath = "Relic/";

        public static void AddSpell(CardDataBuilder r, string IDName)
        {
            r.CardID = IDName;
            r.NameKey = IDName + "_Name";
            r.OverrideDescriptionKey = IDName + "_Desc";
            r.LinkedClass = DiscipleClan.getClan();

            r.ClanID = Clan.IDName;
            r.CardPoolIDs = new List<string> { "Chrono", MegaPool };

            r.AssetPath = rootPath + scardPath;

            if (!r.NameKey.HasTranslation())
                API.Log(BepInEx.Logging.LogLevel.All, r.NameKey + ",Text,,,,," + r.CardID + ",,,,,");
            if (!r.OverrideDescriptionKey.HasTranslation())
                API.Log(BepInEx.Logging.LogLevel.All, r.OverrideDescriptionKey + ",Text,,,,,<desc>,,,,,");
        }

        public static void AddRelic(CollectableRelicDataBuilder r, string ID)
        {
            r.CollectableRelicID = ID;
            r.NameKey = ID + "_Name";
            r.DescriptionKey = ID + "_Desc";
            r.RelicActivatedKey = ID + "_Active";
            r.RelicLoreTooltipKeys = new List<string> { ID + "_Lore" };
            r.ClanID = "Chrono";
            r.AssetPath = rootPath + relicPath + r.AssetPath;
            r.Rarity = CollectableRarity.Common;

            if (!r.NameKey.HasTranslation())
                API.Log(BepInEx.Logging.LogLevel.All, r.NameKey + ",Text,,,,," + ID + ",,,,,");
            if (!r.DescriptionKey.HasTranslation())
                API.Log(BepInEx.Logging.LogLevel.All, r.DescriptionKey + ",Text,,,,,<desc>,,,,,");
            if (!r.RelicActivatedKey.HasTranslation())
                API.Log(BepInEx.Logging.LogLevel.All, r.RelicActivatedKey + ",Text,,,,,<desc>,,,,,");
            if (!r.RelicLoreTooltipKeys[0].HasTranslation())
                API.Log(BepInEx.Logging.LogLevel.All, r.RelicLoreTooltipKeys[0] + ",Text,,,,,<desc>,,,,,");
        }

        public static void AddUnit(CardDataBuilder r, string IDName, CharacterData character)
        {
            r.CardID = IDName;
            r.NameKey = IDName + "_Name";
            r.OverrideDescriptionKey = IDName + "_Desc";
            r.LinkedClass = DiscipleClan.getClan();
            r.ClanID = Clan.IDName;

            r.CardPoolIDs = new List<string> { "Chrono", UnitsAllBanner };
            r.CardType = CardType.Monster;
            r.TargetsRoom = true;

            r.AssetPath = rootPath + ucardPath;
            r.EffectBuilders.Add(
                new CardEffectDataBuilder
                {
                    EffectStateName = "CardEffectSpawnMonster",
                    TargetMode = TargetMode.DropTargetCharacter,
                    ParamCharacterData = character,
                });

            if (!r.NameKey.HasTranslation())
                API.Log(BepInEx.Logging.LogLevel.All, r.NameKey + ",Text,,,,," + r.CardID + ",,,,,");
            if (!r.OverrideDescriptionKey.HasTranslation())
                API.Log(BepInEx.Logging.LogLevel.All, r.OverrideDescriptionKey + ",Text,,,,,<desc>,,,,,");
        }

        public static void AddWard(CardDataBuilder r, string IDName, CharacterData character)
        {
            r.CardID = IDName;
            r.NameKey = IDName + "_Name";
            r.OverrideDescriptionKey = IDName + "_Desc";
            r.LinkedClass = DiscipleClan.getClan();
            r.ClanID = Clan.IDName;

            r.CardPoolIDs = new List<string> { "Chrono", MegaPool };
            r.CardType = CardType.Monster;
            r.TargetsRoom = true;

            r.AssetPath = rootPath + scardPath;
            r.EffectBuilders.Add(
                new CardEffectDataBuilder
                {
                    EffectStateName = "CardEffectSpawnMonster",
                    TargetMode = TargetMode.DropTargetCharacter,
                    ParamCharacterData = character,
                });

            if (!r.NameKey.HasTranslation())
                API.Log(BepInEx.Logging.LogLevel.All, r.NameKey + ",Text,,,,," + r.CardID + ",,,,,");
            if (!r.OverrideDescriptionKey.HasTranslation())
                API.Log(BepInEx.Logging.LogLevel.All, r.OverrideDescriptionKey + ",Text,,,,,<desc>,,,,,");
        }

        public static void AddImg(CardDataBuilder r, string imgName)
        {
            r.AssetPath = r.AssetPath + imgName;
        }

        public static void AddUnitImg(CharacterDataBuilder r, string imgName)
        {
            r.AssetPath = rootPath + unitPath + imgName;
        }
    }
}
