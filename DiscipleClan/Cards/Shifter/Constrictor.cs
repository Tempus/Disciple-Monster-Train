using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using MonsterTrainModdingAPI.Enums.MTCardPools;

// TODO - Can't target floor above/below easily? Look into target filters, also 'how to tell which floor I'm on'. Otherwise, COMPRESS EVERYTHING chronoTicked

namespace DiscipleClan.Cards.Shifter
{
    class Constrictor
    {
        public static string IDName = "Constrictor";
        public static string imgName = "Cutezacotl";
        public static void Make()
        {

            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 1,
                Rarity = CollectableRarity.Uncommon,

                TraitBuilders = new List<CardTraitDataBuilder>
                {
                    new CardTraitDataBuilder
                    {
                        TraitStateName = "CardTraitIntrinsicState"
                    }
                }
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
                Health = 15,
                AttackDamage = 12,

                // Pulls everything that it can to this floor on summon
                TriggerBuilders = new List<CharacterTriggerDataBuilder>
                {
                    new CharacterTriggerDataBuilder
                    {
                        Trigger = CharacterTriggerData.Trigger.OnSpawn,
                        EffectBuilders = new List<CardEffectDataBuilder>
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateName = "CardEffectBump",
                                ParamInt = -1,
                                TargetMode = TargetMode.Tower
                            },
                            new CardEffectDataBuilder
                            {
                                EffectStateName = "CardEffectBump",
                                ParamInt = 1,
                                TargetMode = TargetMode.Tower
                            },
                        }
                    }
                }
            };

            Utils.AddUnitImg(characterDataBuilder, imgName + ".png");
            return characterDataBuilder.BuildAndRegister();
        }
    }
}
