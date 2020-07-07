using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using MonsterTrainModdingAPI.Enums.MTCardPools;

namespace DiscipleClan.Cards.Speedtime
{
    class LakshimiOwl
    {
        public static string IDName = "Lakshimi Owl";
        public static string imgName = "Excalibird";
        public static void Make()
        {

            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 0,
                Rarity = CollectableRarity.Uncommon,
                TraitBuilders = new List<CardTraitDataBuilder>
                {
                    new CardTraitDataBuilder
                    {
                        TraitStateName = "CardTraitIntrinsicState"
                    }
                },
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
                Health = 20,
                AttackDamage = 10
            };

            Utils.AddUnitImg(characterDataBuilder, imgName + ".png");
            return characterDataBuilder.BuildAndRegister();
        }
    }
}
