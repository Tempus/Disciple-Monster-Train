using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using MonsterTrainModdingAPI.Enums.MTCardPools;
using MonsterTrainModdingAPI.Enums.MTStatusEffects;

namespace DiscipleClan.Cards.Prophecy
{
    class TrashPickerOwl
    {
        public static string IDName = "TrashPickerOwl";
        public static string imgName = "Sowlette";
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

                Size = 1,
                Health = 5,
                AttackDamage = 20,

                TriggerBuilders = new List<CharacterTriggerDataBuilder>
                {
                    new CharacterTriggerDataBuilder
                    {
                        Trigger = CharacterTriggerData.Trigger.OnTurnBegin,
                    }
                },

                
            };
            characterDataBuilder.AddStartingStatusEffect(typeof(MTStatusEffect_Quick), 1);

            Utils.AddUnitImg(characterDataBuilder, imgName + ".png");
            return characterDataBuilder.BuildAndRegister();
        }
    }
}
