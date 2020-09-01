using DiscipleClan.CardEffects;
using MonsterTrainModdingAPI.Builders;
using System.Collections.Generic;

namespace DiscipleClan.Cards.Prophecy
{
    class PalmReading
    {
        public static string IDName = "PalmReading";

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
                        EffectStateName = typeof(CardEffectScryDiscard).AssemblyQualifiedName,
                        ParamBool = true,
                        ParamInt = 2,
                        AdditionalParamInt = 99,
                        TargetMode = TargetMode.Deck,
                    }
                },
            };

            Utils.AddSpell(railyard, IDName);
            Utils.AddImg(railyard, "Palm-Reading.png");

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
