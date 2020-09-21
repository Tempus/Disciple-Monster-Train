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
                Cost = 0,
                Rarity = CollectableRarity.Common,
                Targetless = true,

                EffectBuilders = new List<CardEffectDataBuilder>
                {
                    new CardEffectDataBuilder
                    {
                        EffectStateType = typeof(CardEffectAddPyreStatusEmpowered),
                    },
                },
            };
            railyard.EffectBuilders[0].AddStatusEffect(Armor, 1);

            Utils.AddSpell(railyard, IDName);
            Utils.AddCardPortrait(railyard, "Firewall");

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
