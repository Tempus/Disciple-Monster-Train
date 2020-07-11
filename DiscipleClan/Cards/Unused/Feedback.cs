using MonsterTrainModdingAPI.Builders;
using System.Collections.Generic;
using static MonsterTrainModdingAPI.Constants.VanillaStatusEffectIDs;

namespace DiscipleClan.Cards.Unused
{
    class Feedback
    {
        public static string IDName = "Feedback";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                CostType = CardData.CostType.ConsumeRemainingEnergy,
                Rarity = CollectableRarity.Uncommon,
                Targetless = true,

                EffectBuilders = new List<CardEffectDataBuilder>
                {
                    new CardEffectDataBuilder
                    {
                        EffectStateName = "CardEffectDamage",
                        ParamInt = 0,
                        TargetMode = TargetMode.Pyre,
                    },
                },

                TraitBuilders = new List<CardTraitDataBuilder>
                {
                    new CardTraitDataBuilder
                    {
                         TraitStateName = "CardTraitScalingAddEnergy",
                         ParamUseScalingParams = true,
                         ParamInt = 3,

                    },
                    new CardTraitDataBuilder
                    {
                         TraitStateName = "CardTraitScalingAddDamage",
                         ParamUseScalingParams = true,
                         ParamInt = 2,
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
