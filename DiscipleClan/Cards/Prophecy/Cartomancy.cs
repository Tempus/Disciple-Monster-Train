using DiscipleClan.CardEffects;
using MonsterTrainModdingAPI.Builders;
using System.Collections.Generic;

namespace DiscipleClan.Cards.Prophecy
{
    class Cartomancy
    {
        public static string IDName = "Cartomancy";

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
                        EffectStateName = typeof(CardEffectScryConsume).AssemblyQualifiedName,
                        ParamInt = 4,
                        AdditionalParamInt = 99,
                        TargetMode = TargetMode.Deck,
                    }
                },

                TraitBuilders = new List<CardTraitDataBuilder>
                {
                    new CardTraitDataBuilder
                    {
                        TraitStateName = "CardTraitExhaustState"
                    }
                }
            };

            Utils.AddSpell(railyard, IDName);
            Utils.AddImg(railyard, "Cartomancy.png");

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
