using DiscipleClan.CardEffects;
using Trainworks.Builders;
using System.Collections.Generic;
using static Trainworks.Constants.VanillaStatusEffectIDs;

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
                Cost = 0,
                Rarity = CollectableRarity.Common,
                Targetless = true,

                EffectBuilders = new List<CardEffectDataBuilder>
                {
                    new CardEffectDataBuilder
                    {
                        EffectStateType = typeof(CardEffectAddPyreStatusEmpowered),
                        ParamStatusEffects = new StatusEffectStackData[] { new StatusEffectStackData { count=1, statusId="armor" } },
                    },
                },
            };

            Utils.AddSpell(railyard, IDName);
            Utils.AddCardPortrait(railyard, "Firewall");

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
