using MonsterTrainModdingAPI.Builders;
using System.Collections.Generic;
using static MonsterTrainModdingAPI.Constants.VanillaStatusEffectIDs;

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
                Rarity = CollectableRarity.Uncommon,

                EffectBuilders = new List<CardEffectDataBuilder>
                {
                    new CardEffectDataBuilder
                    {
                        EffectStateName = "CardEffectAddTempCardUpgradeToUnits",
                        TargetMode = TargetMode.DropTargetCharacter,
                        TargetTeamType = Team.Type.Heroes | Team.Type.Monsters,
                        ParamCardUpgradeData = new CardUpgradeDataBuilder
                        {
                            bonusDamage = 8,
                            bonusHP = 4,
                            bonusSize = 1,
                            hideUpgradeIconOnCard = true,
                        }.Build(),
                    },
                },

                TraitBuilders = new List<CardTraitDataBuilder>
                {
                    new CardTraitDataBuilder
                    {
                         TraitStateName = "CardTraitMultiplyCharacterUpgrade",
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

            railyard.EffectBuilders[0].AddStatusEffect(Armor, 0);

            Utils.AddSpell(railyard, IDName);
            Utils.AddImg(railyard, "image0.jpg");

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
