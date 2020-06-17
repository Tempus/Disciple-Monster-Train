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

namespace MonsterTrainTestMod.Cards.Spells
{
    class Equilibrium
    {
        private static string IDName = "Equilibrium";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                CardID = IDName,
                Name = IDName,
                Description = "Ascend a random friendly unit and descend a random enemy unit",
                Cost = 1,
                Rarity = CollectableRarity.Common,
                ClanID = MTClanIDs.GetIDForType(typeof(MTClan_Hellhorned)),
                CardPoolIDs = new List<string> { MTCardPoolIDs.GetIDForType(typeof(MTCardPool_MegaPool)) },

                TargetsRoom = true,
                Targetless = true,

                AssetPath = "netstandard2.0/chrono/zyzzy.png",

                EffectBuilders = new List<CardEffectDataBuilder>
                {
                    new CardEffectDataBuilder
                    {
                        EffectStateName = "CardEffectBump",
                        ParamInt = -1,
                        TargetMode = TargetMode.RandomInRoom,
                        TargetTeamType = Team.Type.Heroes,
                    },
                    new CardEffectDataBuilder
                    {
                        EffectStateName = "CardEffectBump",
                        ParamInt = 1,
                        TargetMode = TargetMode.RandomInRoom,
                        TargetTeamType = Team.Type.Monsters,
                    }
                },
            };

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
