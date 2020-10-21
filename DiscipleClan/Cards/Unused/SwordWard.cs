using DiscipleClan.CardEffects;
using Trainworks.Builders;
using System.Collections.Generic;

namespace DiscipleClan.Cards.Unused
{
    class SwordWard
    {
        public static string IDName = "SwordWard";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 8,
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
        public static CharacterDataBuilder BuildUnit()
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
                        ParamStatusEffects = new StatusEffectStackData[]
                        {
                            new StatusEffectStackData { count = 1, statusId = "multistrike"},
                        }
                    }
                }
            };

            Utils.AddUnitImg(characterDataBuilder, IDName + ".png");
            return characterDataBuilder;
        }
    }
}
