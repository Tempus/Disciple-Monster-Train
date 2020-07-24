using DiscipleClan.CardEffects;
using DiscipleClan.Triggers;
using MonsterTrainModdingAPI.Builders;
using System.Collections.Generic;

namespace DiscipleClan.Cards.Pyrepact
{
    class Refractor
    {
        public static string IDName = "Refractor";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Rarity = CollectableRarity.Rare,

                //TriggerBuilders = new List<CardTriggerEffectDataBuilder>
                //{
                //    new CardTriggerEffectDataBuilder
                //    {
                //        trigger = OnGainEmber.OnGainEmberCardTrigger.GetEnum(),
                //        CardEffectBuilders = new List<CardEffectDataBuilder>
                //        {
                //            new CardEffectDataBuilder
                //            {
                //                EffectStateName = "CardEffectBuffDamage",
                //                ParamInt = 5,
                //                TargetMode = TargetMode.Pyre,
                //            }
                //        }
                //    }
                //},

                TraitBuilders = new List<CardTraitDataBuilder>
                {
                    new CardTraitDataBuilder
                    {
                        TraitStateName = "CardTraitUnplayable"
                    },
                    new CardTraitDataBuilder
                    {
                        TraitStateType = typeof(CardTraitSuppressExhaust),
                    },
                }
            };

            Utils.AddSpell(railyard, IDName);
            Utils.AddImg(railyard, "Refractor.png");

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
