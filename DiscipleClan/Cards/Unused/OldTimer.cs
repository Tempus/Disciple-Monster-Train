using MonsterTrainModdingAPI.Builders;
using System.Collections.Generic;

namespace DiscipleClan.Cards.Unused
{
    class OldTimer
    {
        public static string IDName = "OldTimer";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 2,
                Rarity = CollectableRarity.Uncommon,
                TargetsRoom = true,

                EffectBuilders = new List<CardEffectDataBuilder>
                {
                    new CardEffectDataBuilder
                    {
                        EffectStateName = "CardEffectDamage",
                        ParamInt = 10,
                        TargetMode = TargetMode.FrontInRoom,
                        TargetTeamType = Team.Type.Heroes,
                    }
                },

                TriggerBuilders = new List<CardTriggerEffectDataBuilder>
                {
                    new CardTriggerEffectDataBuilder
                    {
                        trigger = CardTriggerType.OnUnplayed,
                        cardTriggers = new List<CardTriggerData>
                        {
                            new CardTriggerData
                            {
                                 persistenceMode = PersistenceMode.SingleBattle,
                                 cardTriggerEffect = "CardTriggerEffectBuffSpellDamage",
                                 paramInt = 10,
                                 buffEffectType = "CardEffectBuffDamage",
                            }
                        }
                    }
                },

                TraitBuilders = new List<CardTraitDataBuilder>
                {
                    new CardTraitDataBuilder
                    {
                        TraitStateName = "CardTraitMagneticState"
                    }
                }
            };

            Utils.AddSpell(railyard, IDName);
            Utils.AddImg(railyard, "image0.jpg");

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
