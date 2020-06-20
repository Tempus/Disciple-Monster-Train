using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Enums.MTCardPools;
using DiscipleClan.Cards;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscipleClan.Cards.Units
{
    class Disciple
    {
        public static string IDName = "Disciple";
        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 1,
                Rarity = CollectableRarity.Champion,
            };

            Utils.AddUnit(railyard, IDName, BuildUnit());
            Utils.AddImg(railyard, "Disciple.png");

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
                Name = IDName + "_Name",

                Size = 2,
                Health = 10,
                AttackDamage = 10,
                AssetPath = "Disciple/chrono/Unit Assets/Disciple.png",
                SubtypeKeys = new List<string> { "SubtypesData_Champion_83f21cbe-9d9b-4566-a2c3-ca559ab8ff34" },
            };

            return characterDataBuilder.BuildAndRegister();
        }
    }
}
