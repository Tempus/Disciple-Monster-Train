using DiscipleClan.CardEffects;
using Trainworks.Builders;
using System.Collections.Generic;

namespace DiscipleClan.Cards.Unused
{
    class NoetherCharge
    {
        public static string IDName = "NoetherCharge";

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
                        EffectStateName = typeof(CardEffectPyreAttack).AssemblyQualifiedName,
                        ParamInt = 0,
                        TargetMode = TargetMode.Room,
                        TargetTeamType = Team.Type.Heroes,
                    }
                },

                EffectTriggerBuilders = new List<CharacterTriggerDataBuilder>
                {
                    new CharacterTriggerDataBuilder
                    {
                        Trigger = CharacterTriggerData.Trigger.EndTurnPreHandDiscard,
                        EffectBuilders = new List<CardEffectDataBuilder>
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateName = "CardEffectBuffDamage",
                                TargetMode = TargetMode.Self,
                                ParamInt = 5,
                            }
                        }
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
