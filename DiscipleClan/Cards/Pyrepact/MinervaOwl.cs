using MonsterTrainModdingAPI.Builders;
using System.Collections.Generic;
using static MonsterTrainModdingAPI.Constants.VanillaStatusEffectIDs;


namespace DiscipleClan.Cards.Pyrepact
{
    class MinervaOwl
    {
        public static string IDName = "Minerva Owl";
        public static string imgName = "Owlit";
        public static void Make()
        {

            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 1,
                Rarity = CollectableRarity.Rare,
            };

            Utils.AddUnit(railyard, IDName, BuildUnit());
            Utils.AddCardPortrait(railyard, "Minerva");

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
                SubtypeKeys = new List<string> { "ChronoSubtype_Seer" },

                Size = 1,
                Health = 3,
                AttackDamage = 0,
            };
            // Unit art asset, complex stuff!
            characterDataBuilder.AddStartingStatusEffect(Sweep, 1);
            characterDataBuilder.AddStartingStatusEffect("pyreboost", 1);

            Utils.AddUnitAnim(characterDataBuilder, "minerva");
            return characterDataBuilder.BuildAndRegister();
        }
    }
}
