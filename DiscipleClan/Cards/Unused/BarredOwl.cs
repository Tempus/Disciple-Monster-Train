using DiscipleClan.Triggers;
using Trainworks.Builders;
using System.Collections.Generic;
using static Trainworks.Constants.VanillaStatusEffectIDs;

namespace DiscipleClan.Cards.Unused
{
    class BarredOwl
    {
        public static string IDName = "BarredOwl";
        public static string imgName = "SpectacledOwl";
        public static void Make()
        {

            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 1,
                Rarity = CollectableRarity.Uncommon,
                Description = "Relocate: Daze Floor.",
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
                Health = 20,
                AttackDamage = 12,

                // Relocate
                TriggerBuilders = new List<CharacterTriggerDataBuilder>
                {
                    new CharacterTriggerDataBuilder {
                        Trigger = OnRelocate.OnRelocateCharTrigger.GetEnum(),
                        EffectBuilders = new List<CardEffectDataBuilder>
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateName = "CardEffectAddStatusEffect",
                                TargetMode = TargetMode.Room,
                                TargetTeamType = Team.Type.Heroes,
                            }
                        }
                    },
                }
            };

            characterDataBuilder.TriggerBuilders[0].EffectBuilders[0].AddStatusEffect(Dazed, 1);

            Utils.AddUnitImg(characterDataBuilder, imgName + ".png");
            return characterDataBuilder.BuildAndRegister();
        }
    }
}
