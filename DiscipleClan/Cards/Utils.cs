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
        public static string cardPath = "Card Assets/";

        public static void AddSpell(CardDataBuilder r, string IDName)
        {
            r.CardID = IDName;
            r.Name = IDName + "_Name";
            r.OverrideDescriptionKey = IDName + "_Desc";

            r.ClanID = Clan.IDName;
            r.CardPoolIDs = new List<string> { MTCardPoolIDs.GetIDForType(typeof(MTCardPool_ChronoPool)) };
        }

        public static void AddUnit(CardDataBuilder r, string IDName, CharacterData character)
        {
            r.CardID = IDName;
            r.Name = IDName + "_Name";
            r.OverrideDescriptionKey = IDName + "_Desc";

            r.CardPoolIDs = new List<string> { MTCardPoolIDs.GetIDForType(typeof(MTCardPool_UnitsAllBanner)) };
            r.CardType = CardType.Monster;
            r.TargetsRoom = true;

            r.EffectBuilders.Add(
                new CardEffectDataBuilder
                {
                    EffectStateName = "CardEffectSpawnMonster",
                    TargetMode = TargetMode.DropTargetCharacter,
                    ParamCharacterData = character,
                });
        }

        public static void AddImg(CardDataBuilder r, string imgName)
        {
            r.AssetPath = rootPath + cardPath + imgName;
        }
    }
}
