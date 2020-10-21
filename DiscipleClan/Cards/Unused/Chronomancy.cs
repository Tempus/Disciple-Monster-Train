using DiscipleClan.CardEffects;
using Trainworks.Builders;
using System.Collections.Generic;

namespace DiscipleClan.Cards.Unused
{
    class Chronomancy
    {
        public static string IDName = "Chronomancy";

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
                        EffectStateName = typeof(CardEffectScryApplyUpgrade).AssemblyQualifiedName,
                        ParamInt = 8,
                        AdditionalParamInt = 1,
                        TargetMode = TargetMode.Deck,
                        ParamCardUpgradeData = new CardUpgradeDataBuilder
                        {
                            HideUpgradeIconOnCard = true,
                            StatusEffectUpgrades = new List<StatusEffectStackData>
                            {
                                new StatusEffectStackData
                                {
                                    statusId = "ambush",
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
            Utils.AddImg(railyard, "Chronomancy.png");

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
