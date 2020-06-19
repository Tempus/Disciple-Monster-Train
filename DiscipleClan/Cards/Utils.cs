using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Enums.MTCardPools;
using MonsterTrainModdingAPI.Enums.MTClans;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonsterTrainTestMod.Cards
{
    class Utils
    {
        public static string rootPath = "netstandard2.0/chrono/";

        public static void AddSpell(CardDataBuilder r, string IDName)
        {
            r.CardID = IDName;
            r.Name = IDName + "_Name";
            r.OverrideDescriptionKey = IDName + "_Desc";

            r.ClanID = MTClanIDs.GetIDForType(typeof(MTClan_Hellhorned));
            r.CardPoolIDs = new List<string> { MTCardPoolIDs.GetIDForType(typeof(MTCardPool_MegaPool)) };
        }

        public static void AddImg(CardDataBuilder r, string imgName)
        {
            r.AssetPath = rootPath + imgName;
        }
    }
}
