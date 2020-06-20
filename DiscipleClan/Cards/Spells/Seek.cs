using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Enums.MTCardPools;
using MonsterTrainModdingAPI.Enums.MTClans;
using MonsterTrainModdingAPI.Enums.MTStatusEffects;
using MonsterTrainModdingAPI.Managers;
using DiscipleClan.Cards.CardEffects;
using ShinyShoe;

namespace DiscipleClan.Cards.Spells
{
    class Seek
    {
        public static string IDName = "Seek";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                CardID = IDName,
                Name = IDName,
                Description = "Choose a card from your deck",
                Cost = 1,
                Rarity = CollectableRarity.Uncommon,
                ClanID = MTClanIDs.GetIDForType(typeof(MTClan_Hellhorned)),
                CardPoolIDs = new List<string> { MTCardPoolIDs.GetIDForType(typeof(MTCardPool_MegaPool)) },

                TargetsRoom = false,
                Targetless = true,

                AssetPath = "netstandard2.0/chrono/image0.jpg",

                EffectBuilders = new List<CardEffectDataBuilder>
                {
                    new CardEffectDataBuilder
                    {
                        EffectStateName = typeof(CardEffectChooseDraw).AssemblyQualifiedName,
                        ParamInt = 1,
                        TargetMode = TargetMode.Deck,
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

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
