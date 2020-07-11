using DiscipleClan.CardEffects;
using MonsterTrainModdingAPI.Builders;
using System.Collections.Generic;

namespace DiscipleClan.Cards.Unused
{
    class StasisWard
    {
        public static string IDName = "StasisWard";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 2,
                Rarity = CollectableRarity.Rare,

                TraitBuilders = new List<CardTraitDataBuilder>
                {
                    new CardTraitDataBuilder
                    {
                        TraitStateName = "CardTraitExhaustState",
                    }
                }
            };

            Utils.AddWard(railyard, IDName, BuildUnit());
            Utils.AddImg(railyard, "Webcam.png");

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

                Size = 0,
                Health = 1,
                AttackDamage = 0,
                RoomModifierBuilders = new List<RoomModifierDataBuilder>
                {
                    new RoomModifierDataBuilder
                    {
                        roomStateModifierClassName = typeof(RoomStateModifierRelocateStatusEffect).AssemblyQualifiedName,
                        paramStatusEffects = new StatusEffectStackData[]
                        {
                            new StatusEffectStackData { count = 1, statusId = "dazed"},
                        }
                    }
                }
            };

            Utils.AddUnitImg(characterDataBuilder, IDName + ".png");
            return characterDataBuilder.BuildAndRegister();
        }
    }
}
