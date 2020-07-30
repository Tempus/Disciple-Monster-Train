using DiscipleClan.CardEffects;
using MonsterTrainModdingAPI.Builders;
using System.Collections.Generic;
using static MonsterTrainModdingAPI.Constants.VanillaStatusEffectIDs;

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
                        EffectStateName = "CardEffectAddStatusEffect",
                        TargetMode = TargetMode.Pyre,
                        ShouldTest = false,
                    },
                    new CardEffectDataBuilder
                    {
                        EffectStateName = "CardEffectBuffDamage",
                        ParamInt = 5,
                        TargetMode = TargetMode.Pyre,
                        ShouldTest = true,
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
                         ParamInt = 5,
                         ParamStatusEffects = new StatusEffectStackData[] { new StatusEffectStackData { statusId=Armor, count=0} }
                    },
                    new CardTraitDataBuilder
                    {
                         TraitStateName = "CardTraitScalingBuffDamage",
                         ParamUseScalingParams = true,
                         ParamTrackedValue = CardStatistics.TrackedValueType.PlayedCost,
                         ParamEntryDuration = CardStatistics.EntryDuration.ThisBattle,
                         ParamInt = 5,
                    },
                    new CardTraitDataBuilder
                    {
                         TraitStateName = "CardTraitExhaustState",
                    },
                    new CardTraitDataBuilder
                    {
                         TraitStateName = typeof(CardTraitJustPyreka).AssemblyQualifiedName,
                    },
                }
            };

            railyard.EffectBuilders[0].AddStatusEffect(Armor, 0);

            Utils.AddSpell(railyard, IDName);
            Utils.AddImg(railyard, "Pyrespike.png");

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
