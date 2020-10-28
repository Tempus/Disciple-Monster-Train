using Trainworks.Builders;
using System.Collections.Generic;


namespace DiscipleClan.Cards.Pyrepact
{
    class EmberMaker
    {
        public static string IDName = "EmberMaker";
        public static string imgName = "Embermaker";
        public static void Make()
        {

            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 1,
                Rarity = CollectableRarity.Uncommon,
            };

            Utils.AddUnit(railyard, IDName, BuildUnit());
            Utils.AddCardPortrait(railyard, "Embermaker");

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
                SubtypeKeys = new List<string> { "ChronoSubtype_Eternal" },

                Size = 2,
                Health = 20,
                AttackDamage = 2,

                TriggerBuilders = new List<CharacterTriggerDataBuilder>
                {
                    new CharacterTriggerDataBuilder
                    {
                        Trigger = CharacterTriggerData.Trigger.OnAttacking,
                        EffectBuilders = new List<CardEffectDataBuilder>
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateName = "CardEffectAddStatusEffect",
                                TargetMode = TargetMode.LastAttackedCharacter,
                                ParamStatusEffects = new StatusEffectStackData[] { new StatusEffectStackData { count = 1, statusId = "emberboost" } }
                            }
                        }
                    }
                }
            };
            // Unit art asset, complex stuff!

            Utils.AddUnitAnim(characterDataBuilder, "embermaker");
            return characterDataBuilder.BuildAndRegister();
        }
    }
}
