using DiscipleClan.CardEffects;
using Trainworks.Builders;
using System.Collections.Generic;
using static Trainworks.Constants.VanillaStatusEffectIDs;

namespace DiscipleClan.Cards.Pyrepact
{
    class Dilation
    {
        public static string IDName = "Dilation";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                CostType = CardData.CostType.ConsumeRemainingEnergy,
                Rarity = CollectableRarity.Rare,

                EffectBuilders = new List<CardEffectDataBuilder>
                {
                    new CardEffectDataBuilder
                    {
                        EffectStateName = "CardEffectAddTempCardUpgradeToUnits",
                        TargetMode = TargetMode.DropTargetCharacter,
                        TargetTeamType = Team.Type.Heroes | Team.Type.Monsters,
                        ParamCardUpgradeData = new CardUpgradeDataBuilder
                        {
                            BonusDamage = 8,
                            BonusHP = 3,
                            BonusSize = 1,
                            HideUpgradeIconOnCard = true,
                        }.Build(),
                        ParamStatusEffects = new StatusEffectStackData[] { new StatusEffectStackData { count=0, statusId="armor" } },
                    },
                },

                TraitBuilders = new List<CardTraitDataBuilder>
                {
                    new CardTraitDataBuilder
                    {
                         TraitStateName = typeof(CardTraitMultiplyCharacterUpgrade).AssemblyQualifiedName,
                         ParamTrackedValue = CardStatistics.TrackedValueType.PlayedCost,
                         ParamEntryDuration = CardStatistics.EntryDuration.ThisBattle,
                         ParamUseScalingParams = true,
                         ParamInt = 1,
                    },
                    new CardTraitDataBuilder
                    {
                         TraitStateName = "CardTraitExhaustState",
                    },
                }
            };

            Utils.AddSpell(railyard, IDName);
            Utils.AddImg(railyard, "Dilation.png");

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
