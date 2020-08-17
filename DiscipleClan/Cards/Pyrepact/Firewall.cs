using DiscipleClan.CardEffects;
using MonsterTrainModdingAPI.Builders;
using System.Collections.Generic;
using static MonsterTrainModdingAPI.Constants.VanillaStatusEffectIDs;

namespace DiscipleClan.Cards.Pyrepact
{
    class Firewall
    {
        public static string IDName = "Firewall";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 1,
                Rarity = CollectableRarity.Starter,
                Targetless = true,

                EffectBuilders = new List<CardEffectDataBuilder>
                {
                    new CardEffectDataBuilder
                    {
                        EffectStateType = typeof(CardEffectAddPyreStatus),
                    },
                },

                //TraitBuilders = new List<CardTraitDataBuilder>
                //{
                //    new CardTraitDataBuilder
                //    {
                //         TraitStateName = typeof(CardTraitJustPyreka).AssemblyQualifiedName,
                //    },
                //}
            };
            railyard.EffectBuilders[0].AddStatusEffect(Armor, 3);

            Utils.AddSpell(railyard, IDName);
            Utils.AddImg(railyard, "Emberwave.png");
            railyard.CardPoolIDs = new List<string>();

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
