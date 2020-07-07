using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using MonsterTrainModdingAPI.Enums.MTCardPools;
using DiscipleClan.Triggers;


namespace DiscipleClan.Cards.Shifter
{
    class FortuneFinder
    {
        public static string IDName = "Fortune Finder";
        public static string imgName = "ChestDude";
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
                Health = 10,
                AttackDamage = 2,

                // Relocate
                TriggerBuilders = new List<CharacterTriggerDataBuilder>
                {
                    new CharacterTriggerDataBuilder {
                        Trigger = CustomTriggerManager.GetTrigger(typeof(MTCharacterTrigger_Relocate)),
                        EffectBuilders = new List<CardEffectDataBuilder>
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateName = "CardEffectRewardGold",
                                ParamInt = 10,
                                TargetMode = TargetMode.Self
                            }
                        }
                    },
                }
            };

            Utils.AddUnitImg(characterDataBuilder, imgName + ".png");
            return characterDataBuilder.BuildAndRegister();
        }
    }
}
