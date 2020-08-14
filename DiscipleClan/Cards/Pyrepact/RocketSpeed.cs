using DiscipleClan.CardEffects;
using MonsterTrainModdingAPI.Builders;
using System.Collections.Generic;
using static MonsterTrainModdingAPI.Constants.VanillaStatusEffectIDs;

namespace DiscipleClan.Cards.Pyrepact
{
    class RocketSpeed
    {
        public static string IDName = "Rocket Speed";

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
                        EffectStateType = typeof(CardEffectAddPyreStatus),
                    },
                },

                TraitBuilders = new List<CardTraitDataBuilder>
                {
                    new CardTraitDataBuilder
                    {
                        TraitStateName = "CardTraitExhaustState"
                    },
                }
            };

            railyard.EffectBuilders[0].AddStatusEffect(Quick, 1);


            Utils.AddSpell(railyard, IDName);
            Utils.AddImg(railyard, "Rocket-Speed.png");

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
