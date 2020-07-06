using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using MonsterTrainModdingAPI.Enums.MTCardPools;
using MonsterTrainModdingAPI.Enums.MTStatusEffects;
using DiscipleClan.Triggers;

namespace DiscipleClan.Cards.Units
{
    class LashLizard
    {
        public static string IDName = "Lash Lizard";
        public static string imgName = "SadNewt";
        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 1,
                Rarity = CollectableRarity.Uncommon,
            };

            Utils.AddUnit(railyard, IDName, BuildUnit());
            Utils.AddImg(railyard, imgName + ".png");

            // Do this to complete
            railyard.BuildAndRegister();
        }

        // Builds the unit
        public static CharacterData BuildUnit()
        {
            // Monster card, so we build an attached unit
            CharacterDataBuilder characterDataBuilder = new CharacterDataBuilder
            {
                CharacterID = IDName,
                NameKey = IDName + "_Name",

                Size = 2,
                Health = 10,
                AttackDamage = 5,

                TriggerBuilders = new List<CharacterTriggerDataBuilder>
                {
                    // Relocate
                    new CharacterTriggerDataBuilder {
                        Trigger = CustomTriggerManager.GetTrigger(typeof(MTCharacterTrigger_Relocate)),
                        EffectBuilders = new List<CardEffectDataBuilder>
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateName = "CardEffectAddStatusEffect",
                                TargetMode = TargetMode.Self
                            }
                        }
                    },

                    // Lose Sweep
                    new CharacterTriggerDataBuilder
                    {
                        Trigger = CharacterTriggerData.Trigger.EndTurnPreHandDiscard,
                        EffectBuilders = new List<CardEffectDataBuilder>
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateName = "CardEffectRemoveStatusEffect",
                                TargetMode = TargetMode.Self
                            }
                        }
                    }
                }
            };

            characterDataBuilder.TriggerBuilders[0].EffectBuilders[0].AddStatusEffect(typeof(MTStatusEffect_Sweep), 1);
            characterDataBuilder.TriggerBuilders[1].EffectBuilders[0].AddStatusEffect(typeof(MTStatusEffect_Sweep), 1);

            Utils.AddUnitImg(characterDataBuilder, imgName + ".png");
            return characterDataBuilder.BuildAndRegister();
        }
    }
}
