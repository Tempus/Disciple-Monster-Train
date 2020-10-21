using DiscipleClan.CardEffects;
using Trainworks.Builders;
using System.Collections.Generic;

namespace DiscipleClan.Cards.Speedtime
{
    class Chronomancyb
    {
        public static string IDName = "ChronomancyBeta";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Rarity = CollectableRarity.Rare,

                TraitBuilders = new List<CardTraitDataBuilder>
                {
                    new CardTraitDataBuilder
                    {
                        TraitStateName = typeof(CardTraitApplyStatusToPlayedUnits).AssemblyQualifiedName,
                        ParamStatusEffects = new StatusEffectStackData[] { new StatusEffectStackData { statusId="ambush", count=1 } }
                    },
                    new CardTraitDataBuilder
                    {
                        TraitStateName = "CardTraitUnplayable"
                    },
                }
            };

            Utils.AddSpell(railyard, IDName);
            Utils.AddImg(railyard, "Chronomancy.png");

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
