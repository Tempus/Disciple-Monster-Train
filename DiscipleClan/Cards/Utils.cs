using MonsterTrainModdingAPI;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Enums.MTCardPools;
using MonsterTrainModdingAPI.Enums.MTClans;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscipleClan.Cards
{
    class Utils
    {
        public static string rootPath = "Disciple/chrono/";
        public static string ucardPath = "Card Assets/ProtoUnitCardArt/";
        public static string scardPath = "Card Assets/";
        public static string unitPath = "Unit Assets/";

        public static void AddSpell(CardDataBuilder r, string IDName)
        {
            r.CardID = IDName;
            r.NameKey = IDName + "_Name";
            r.OverrideDescriptionKey = IDName + "_Desc";
            r.LinkedClass = DiscipleClan.getClan();

            r.ClanID = Clan.IDName;
            r.CardPoolIDs = new List<string> { MTCardPoolIDs.GetIDForType(typeof(MTCardPool_ChronoPool)), MTCardPoolIDs.GetIDForType(typeof(MTCardPool_MegaPool)) };

            r.AssetPath = rootPath + scardPath;

            if (!r.NameKey.HasTranslation())
                API.Log(BepInEx.Logging.LogLevel.All, r.NameKey + ",Text,,,,," + r.CardID + ",,,,,");
            if (!r.OverrideDescriptionKey.HasTranslation())
                API.Log(BepInEx.Logging.LogLevel.All, r.OverrideDescriptionKey + ",Text,,,,,<desc>,,,,,");
        }

        public static void AddUnit(CardDataBuilder r, string IDName, CharacterData character)
        {
            r.CardID = IDName;
            r.NameKey = IDName + "_Name";
            r.OverrideDescriptionKey = IDName + "_Desc";
            r.LinkedClass = DiscipleClan.getClan();
            r.ClanID = Clan.IDName;

            r.CardPoolIDs = new List<string> { MTCardPoolIDs.GetIDForType(typeof(MTCardPool_ChronoPool)), MTCardPoolIDs.GetIDForType(typeof(MTCardPool_UnitsAllBanner)) };
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

            r.CardPoolIDs = new List<string> { MTCardPoolIDs.GetIDForType(typeof(MTCardPool_ChronoPool)), MTCardPoolIDs.GetIDForType(typeof(MTCardPool_MegaPool)) };
            r.CardType = CardType.Spell;
            r.TargetsRoom = true;

            r.AssetPath = rootPath + scardPath;
            r.EffectBuilders.Add(
                new CardEffectDataBuilder
                {
                    EffectStateName = "CardEffectSpawnMonster",
                    TargetMode = TargetMode.BackInRoom,
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
