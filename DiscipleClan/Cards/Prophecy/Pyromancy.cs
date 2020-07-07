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

namespace DiscipleClan.Cards.Prophecy
{
    class Pyromancy
    {
        public static string IDName = "Pyromancy";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 1,
                Rarity = CollectableRarity.Rare,
                Targetless = true,

                EffectBuilders = new List<CardEffectDataBuilder>
                {
                    new CardEffectDataBuilder
                    {
                        EffectStateName = typeof(CardEffectScryApplyUpgrade).AssemblyQualifiedName,
                        ParamInt = 4,
                        AdditionalParamInt = 1,
                        TargetMode = TargetMode.Deck,
                        ParamCardUpgradeData = new CardUpgradeDataBuilder
                        {
                            hideUpgradeIconOnCard = true,
                            statusEffectUpgrades = new List<StatusEffectStackData>
                            {
                                new StatusEffectStackData
                                {
                                    statusId = "pyreboost",
                                    count = 1,
                                }
                            }
                        }.Build(),
                    }
                },

                TraitBuilders = new List<CardTraitDataBuilder>
                {
                    new CardTraitDataBuilder {
                        TraitStateName = "CardTraitExhaustState",
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
