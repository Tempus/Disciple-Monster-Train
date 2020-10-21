using DiscipleClan.CardEffects;
using Trainworks.Builders;
using System.Collections.Generic;

namespace DiscipleClan.Cards.Prophecy
{
    class Seek
    {
        public static string IDName = "Seek";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 0,
                Rarity = CollectableRarity.Uncommon,
                Targetless = true,

                EffectBuilders = new List<CardEffectDataBuilder>
                {
                    new CardEffectDataBuilder
                    {
                        EffectStateName = typeof(CardEffectChooseDraw).AssemblyQualifiedName,
                        ParamInt = 1,
                        TargetMode = TargetMode.Deck,
                    }
                },
                TraitBuilders = new List<CardTraitDataBuilder>
                {
                    new CardTraitDataBuilder
                    {
                        TraitStateName = "CardTraitExhaustState"
                    },
                }

            };

            Utils.AddSpell(railyard, IDName);
            Utils.AddImg(railyard, "Seek.png");

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
