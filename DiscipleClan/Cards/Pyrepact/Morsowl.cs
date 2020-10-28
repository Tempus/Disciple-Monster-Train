using Trainworks.Builders;
using Trainworks.Utilities;
using System.Collections.Generic;
using static Trainworks.Constants.VanillaCardPoolIDs;

// TODO - Icarian, Pyre attacks whole tower (we can fake it though)

namespace DiscipleClan.Cards.Pyrepact
{
    class Morsowl
    {
        public static string IDName = "Morsowl";
        public static string imgName = "YarnOwl";
        public static void Make()
        {

            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 1,
                Rarity = CollectableRarity.Uncommon,
            };

            Utils.AddUnit(railyard, IDName, BuildUnit());
            Utils.AddCardPortrait(railyard, "Morsowl");
            railyard.CardPoolIDs = new List<string> { "Chrono", MegaPool };

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
                PriorityDraw = false,

                Size = 1,
                Health = 1,
                AttackDamage = 3,

                RoomModifierBuilders = new List<RoomModifierDataBuilder>
                {
                    new RoomModifierDataBuilder
                    {
                        RoomStateModifierClassType = typeof(RoomStateEnergyModifier),
                        ParamInt = 1,
                    }
                },

                TriggerBuilders = new List<CharacterTriggerDataBuilder>
                {
                    new CharacterTriggerDataBuilder
                    {
                        Trigger = CharacterTriggerData.Trigger.OnDeath,
                        EffectBuilders = new List<CardEffectDataBuilder>
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateName = "CardEffectGainEnergy",
                                ParamInt = 3,
                            },
                        }
                    }

                }
            };
            characterDataBuilder.AddStartingStatusEffect("icarian", 1);

            Utils.AddUnitAnim(characterDataBuilder, "morsowl");
            return characterDataBuilder.BuildAndRegister();
        }
    }
}
