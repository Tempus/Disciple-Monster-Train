using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    class Pendulum
    {
        public static string IDName = "Pendulum";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                CardID = IDName,
                Name = IDName,
                Cost = 3,
                Description = "Transfer all the status effects in the room onto the target",
                Rarity = CollectableRarity.Rare,
                ClanID = MTClanIDs.GetIDForType(typeof(MTClan_Hellhorned)),
                CardPoolIDs = new List<string> { MTCardPoolIDs.GetIDForType(typeof(MTCardPool_MegaPool)) },

                TargetsRoom = true,
                Targetless = false,

                AssetPath = "netstandard2.0/chrono/Good_art.png",

                EffectBuilders = new List<CardEffectDataBuilder>
                {
                    new CardEffectDataBuilder
                    {
                        EffectStateName = "CardEffectTransferAllStatusEffects",
                        TargetMode = TargetMode.DropTargetCharacter,
                        TargetTeamType = Team.Type.Heroes | Team.Type.Monsters,
                    }
                },

                TraitBuilders = new List<CardTraitDataBuilder>
                {
                    new CardTraitDataBuilder
                    {
                        TraitStateName = "CardTraitExhaustState"
                    }
                }
            };

            var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.GetInterface("IMTStatusEffect", true) != null);
            foreach (Type status in types) {
                railyard.EffectBuilders[0].AddStatusEffect(status, 1);
            }
            
            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
