using DiscipleClan.Triggers;
using MonsterTrainModdingAPI.Builders;
using System.Collections.Generic;


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
                Cost = 1,
                Rarity = CollectableRarity.Uncommon,
            };

            Utils.AddUnit(railyard, IDName, BuildUnit());
            Utils.AddCardPortrait(railyard, "FortuneTeller");

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
                SubtypeKeys = new List<string> { "ChronoSubtype_Pythian" },

                Size = 2,
                Health = 10,
                AttackDamage = 2,

                // Relocate
                TriggerBuilders = new List<CharacterTriggerDataBuilder>
                {
                    new CharacterTriggerDataBuilder {
                        Trigger = OnRelocate.OnRelocateCharTrigger.GetEnum(),
                        EffectBuilders = new List<CardEffectDataBuilder>
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateName = "CardEffectRewardGold",
                                ParamInt = 20,
                                TargetMode = TargetMode.Self
                            }
                        }
                    },
                }
            };

            Utils.AddUnitAnim(characterDataBuilder, "fortuneteller");
            return characterDataBuilder.BuildAndRegister();
        }
    }
}
