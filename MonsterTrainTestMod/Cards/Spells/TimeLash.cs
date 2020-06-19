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
    class TimeLash
    {
        private static string IDName = "Time Lash";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                CardID = IDName,
                Name = IDName,
                Cost = 0,
                Description = "Enemy gains Rooted and Haste",
                Rarity = CollectableRarity.Rare,
                ClanID = MTClanIDs.GetIDForType(typeof(MTClan_Hellhorned)),
                CardPoolIDs = new List<string> { MTCardPoolIDs.GetIDForType(typeof(MTCardPool_MegaPool)) },

                TargetsRoom = true,
                Targetless = false,

                AssetPath = "netstandard2.0/chrono/hi.png",

                EffectBuilders = new List<CardEffectDataBuilder>
                {
                    new CardEffectDataBuilder
                    {
                        EffectStateName = "CardEffectAddStatusEffect",
                        TargetMode = TargetMode.DropTargetCharacter,
                        TargetTeamType = Team.Type.Heroes | Team.Type.Monsters,
                    }
                },

                TraitBuilders = new List<CardTraitDataBuilder>
                {
                    new CardTraitDataBuilder
                    {
                        TraitStateName = "CardTraitMagneticState"
                    }
                }
            };

            railyard.EffectBuilders[0].AddStatusEffect(typeof(MTStatusEffect_Rooted), 1);
            railyard.EffectBuilders[0].AddStatusEffect(typeof(MTStatusEffect_Haste), 1);

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
