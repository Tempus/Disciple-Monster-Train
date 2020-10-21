using DiscipleClan.CardEffects;
using Trainworks.Builders;
using System.Collections.Generic;

namespace DiscipleClan.Cards.Unused
{
    class Scry
    {
        public static string IDName = "Scry";

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
                        EffectStateName = typeof(CardEffectScry).AssemblyQualifiedName,
                        ParamInt = 4,
                        AdditionalParamInt = 1,
                        TargetMode = TargetMode.Deck,
                    }
                },

                TraitBuilders = new List<CardTraitDataBuilder>
                {
                    new CardTraitDataBuilder {
                        TraitStateName = "CardTraitIntrinsicState",
                    }
                }
            };

            Utils.AddSpell(railyard, IDName);
            Utils.AddImg(railyard, "cutie.jpg");

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
