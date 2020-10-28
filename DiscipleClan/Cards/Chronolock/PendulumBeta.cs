using DiscipleClan.CardEffects;
using Trainworks.Builders;
using Trainworks.Managers;
using System.Collections.Generic;

namespace DiscipleClan.Cards.Chronolock
{
    class PendulumBeta
    {
        public static string IDName = "PendulumBeta";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                CostType = CardData.CostType.ConsumeRemainingEnergy,
                Rarity = CollectableRarity.Uncommon,
                TargetsRoom = true,

                EffectBuilders = new List<CardEffectDataBuilder>
                {
                    new CardEffectDataBuilder
                    {
                        EffectStateName = typeof(CardEffectIncreaseStatusEffects).AssemblyQualifiedName,
                        TargetMode = TargetMode.Room,
                        ParamBool = true,
                        TargetTeamType = Team.Type.Monsters
                    },
                    new CardEffectDataBuilder
                    {
                        EffectStateName = typeof(CardEffectIncreaseStatusEffects).AssemblyQualifiedName,
                        TargetMode = TargetMode.Room,
                        ParamBool = false,
                        TargetTeamType = Team.Type.Heroes
                    },
                },

                TraitBuilders = new List<CardTraitDataBuilder>
                {
                    new CardTraitDataBuilder
                    {
                        TraitStateType = typeof(CardTraitScalingEveryStatusEffect),
                        ParamTrackedValue = CardStatistics.TrackedValueType.PlayedCost,
                        ParamEntryDuration = CardStatistics.EntryDuration.ThisBattle,
                        ParamUseScalingParams = true,
                        ParamInt = 1,
                    },
                    new CardTraitDataBuilder
                    {
                        TraitStateName = "CardTraitExhaustState"
                    }
                }
            };

            Utils.AddSpell(railyard, IDName);
            Utils.AddImg(railyard, "Pendulum.png");

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
