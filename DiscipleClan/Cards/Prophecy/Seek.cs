using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Enums.MTCardPools;
using MonsterTrainModdingAPI.Enums.MTClans;
using MonsterTrainModdingAPI.Enums.MTStatusEffects;
using MonsterTrainModdingAPI.Managers;
using DiscipleClan.CardEffects;
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
                Cost = 1,
                Rarity = CollectableRarity.Uncommon,
                Targetless = true,

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

            Utils.AddSpell(railyard, IDName);
            Utils.AddImg(railyard, "image0.jpg");

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
