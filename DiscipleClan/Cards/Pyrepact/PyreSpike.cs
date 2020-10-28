using DiscipleClan.CardEffects;
using Trainworks.Builders;
using System.Collections.Generic;
using static Trainworks.Constants.VanillaStatusEffectIDs;

namespace DiscipleClan.Cards.Pyrepact
{
    class PyreSpike
    {
        public static string IDName = "PyreSpike";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                CostType = CardData.CostType.ConsumeRemainingEnergy,
                Rarity = CollectableRarity.Rare,
                Targetless = true,
                TargetsRoom = true,

                EffectBuilders = new List<CardEffectDataBuilder>
                {
                    new CardEffectDataBuilder
                    {
                        EffectStateType = typeof(CardEffectAddPyreStatus),
                        ParamStatusEffects = new StatusEffectStackData[] { new StatusEffectStackData { count=0, statusId="armor" } },
                    },
                    new CardEffectDataBuilder
                    {
                        EffectStateName = "CardEffectBuffDamage",
                        ParamInt = 2,
                        TargetMode = TargetMode.Pyre,
                    },
                },

                TraitBuilders = new List<CardTraitDataBuilder>
                {
                    new CardTraitDataBuilder
                    {
                         TraitStateName = "CardTraitScalingAddStatusEffect",
                         ParamTrackedValue = CardStatistics.TrackedValueType.PlayedCost,
                         ParamEntryDuration = CardStatistics.EntryDuration.ThisBattle,
                         ParamUseScalingParams = true,
                         ParamInt = 2,
                         ParamStatusEffects = new StatusEffectStackData[] { new StatusEffectStackData { statusId=Armor, count=0} }
                    },
                    new CardTraitDataBuilder
                    {
                         TraitStateName = "CardTraitScalingBuffDamage",
                         ParamUseScalingParams = true,
                         ParamTrackedValue = CardStatistics.TrackedValueType.PlayedCost,
                         ParamEntryDuration = CardStatistics.EntryDuration.ThisBattle,
                         ParamInt = 2,
                    },
                    new CardTraitDataBuilder
                    {
                         TraitStateName = "CardTraitExhaustState",
                    },
                }
            };

            Utils.AddSpell(railyard, IDName);
            Utils.AddImg(railyard, "Pyrespike.png");

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
