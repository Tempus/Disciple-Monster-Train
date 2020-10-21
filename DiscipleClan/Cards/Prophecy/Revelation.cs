using DiscipleClan.CardEffects;
using Trainworks.Builders;
using System.Collections.Generic;

namespace DiscipleClan.Cards.Prophecy
{
    class Revelation
    {
        public static string IDName = "Revelation";

        public static void Make()
        {
            // Basic Card State
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 1,
                Rarity = CollectableRarity.Uncommon,
                Targetless = true,

                EffectBuilders = new List<CardEffectDataBuilder>
                {
                    new CardEffectDataBuilder
                    {
                        EffectStateName = typeof(CardEffectScryCopy).AssemblyQualifiedName,
                        ParamInt = 4,
                        AdditionalParamInt = 1,
                        TargetMode = TargetMode.Deck,
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
            Utils.AddImg(railyard, "Revelation.png");

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
