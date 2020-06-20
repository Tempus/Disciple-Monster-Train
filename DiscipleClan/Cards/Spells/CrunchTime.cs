using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Enums.MTCardPools;
using MonsterTrainModdingAPI.Enums.MTClans;
using MonsterTrainModdingAPI.Enums.MTStatusEffects;
using MonsterTrainModdingAPI.Managers;
using ShinyShoe;

namespace DiscipleClan.Cards.Spells
{
    class CrunchTime
    {
        public static string IDName = "Crunch Time";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                CardID = IDName,
                Name = IDName,
                Cost = 2,
                Description = "All Enemies gain Dazed 1 and Quick",
                Rarity = CollectableRarity.Uncommon,
                ClanID = MTClanIDs.GetIDForType(typeof(MTClan_Hellhorned)),
                CardPoolIDs = new List<string> { MTCardPoolIDs.GetIDForType(typeof(MTCardPool_MegaPool)) },

                TargetsRoom = true,
                Targetless = true,

                AssetPath = "netstandard2.0/chrono/body-building-fitness-sports-athlete-implementation-person-royalty-free-thumbnail.png",

                EffectBuilders = new List<CardEffectDataBuilder>
                {
                    new CardEffectDataBuilder
                    {
                        EffectStateName = "CardEffectAddStatusEffect",
                        TargetMode = TargetMode.Room,
                        TargetTeamType = Team.Type.Heroes,
                    }
                },
            };

            railyard.EffectBuilders[0].AddStatusEffect(typeof(MTStatusEffect_Dazed), 1);
            railyard.EffectBuilders[0].AddStatusEffect(typeof(MTStatusEffect_Quick), 1);


            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
