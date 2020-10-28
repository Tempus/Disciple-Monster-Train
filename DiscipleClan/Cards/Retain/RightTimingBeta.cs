using DiscipleClan.CardEffects;
using Trainworks.Builders;
using System.Collections.Generic;

namespace DiscipleClan.Cards.Retain
{
    class RightTimingBeta
    {
        public static string IDName = "RightTimingBeta";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 1,
                Rarity = CollectableRarity.Common,
                TargetsRoom = true,

                EffectBuilders = new List<CardEffectDataBuilder>
                {
                    new CardEffectDataBuilder
                    {
                        EffectStateName = "CardEffectDamage",
                        ParamInt = 5,
                        TargetMode = TargetMode.DropTargetCharacter,
                        TargetTeamType = Team.Type.Heroes | Team.Type.Monsters,
                    }
                },

                TriggerBuilders = new List<CardTriggerEffectDataBuilder>
                {
                    new CardTriggerEffectDataBuilder
                    {
                        trigger = CardTriggerType.OnUnplayed,
                        CardTriggerEffects = new List<CardTriggerData>
                        {
                            new CardTriggerData
                            {
                                 persistenceMode = PersistenceMode.SingleBattle,
                                 cardTriggerEffect = typeof(CardTriggerEffectBuffSpellDamageForEveryCardOfType).AssemblyQualifiedName,
                                 paramInt = 10,
                                 buffEffectType = "CardEffectBuffDamage",
                            }
                        }
                    }
                },
            };

            Utils.AddSpell(railyard, IDName);
            Utils.AddImg(railyard, "Right-Timing.png");

            // Do this to complete
            railyard.BuildAndRegister();
        }
    }
}
