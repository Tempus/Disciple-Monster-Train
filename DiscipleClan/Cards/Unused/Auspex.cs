using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using MonsterTrainModdingAPI.Enums.MTCardPools;

// TODO Unclear about rearrangement indices here - we don't seem to be moving around at all

namespace DiscipleClan.Cards.Unused
{
    class Auspex
    {
        public static string IDName = "Auspex";
        public static string imgName = "FloatyA";
        public static void Make()
        {

            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 2,
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
                Health = 40,
                AttackDamage = 5,

                TriggerBuilders = new List<CharacterTriggerDataBuilder>
                {
                    // Revenge
                    new CharacterTriggerDataBuilder
                    {
                        Trigger = CharacterTriggerData.Trigger.OnHit,
                        EffectBuilders = new List<CardEffectDataBuilder>
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateName = "CardEffectFloorRearrange",
                                ParamInt = 1,
                                TargetMode = TargetMode.Self
                            },
                            new CardEffectDataBuilder
                            {
                                EffectStateName = "CardEffectDamage",
                                ParamInt = 5,
                                TargetMode = TargetMode.LastAttackerCharacter
                            },
                        }
                    },

                    new CharacterTriggerDataBuilder
                    {
                        Trigger = CharacterTriggerData.Trigger.PostCombat,
                        EffectBuilders = new List<CardEffectDataBuilder>
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateName = "CardEffectFloorRearrange",
                                ParamInt = 0,
                                TargetMode = TargetMode.Self
                            },
                        }
                    },
                }
            };

            Utils.AddUnitImg(characterDataBuilder, imgName + ".png");
            return characterDataBuilder.BuildAndRegister();
        }
    }
}
