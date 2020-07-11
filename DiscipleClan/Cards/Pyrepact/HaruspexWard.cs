using MonsterTrainModdingAPI.Builders;
using System.Collections.Generic;

namespace DiscipleClan.Cards.Pyrepact
{
    class HaruspexWard
    {
        public static string IDName = "HaruspexWard";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 1,
                Rarity = CollectableRarity.Uncommon,

                TraitBuilders = new List<CardTraitDataBuilder>
                {
                    new CardTraitDataBuilder
                    {
                        TraitStateName = "CardTraitExhaustState",
                    },
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
                TriggerBuilders = new List<CharacterTriggerDataBuilder>
                {
                    new CharacterTriggerDataBuilder
                    {
                        Trigger = CharacterTriggerData.Trigger.OnAnyUnitDeathOnFloor,
                        EffectBuilders = new List<CardEffectDataBuilder>
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateName = "CardEffectBuffDamage",
                                TargetMode = TargetMode.Pyre,
                                TargetTeamType = Team.Type.Heroes | Team.Type.Monsters,
                                ParamInt = 3,
                            }
                        }
                    },
                }
            };

            Utils.AddUnitImg(characterDataBuilder, IDName + ".png");
            characterDataBuilder.SubtypeKeys = new List<string> { "ChronoSubtype_Ward" };
            return characterDataBuilder.BuildAndRegister();
        }
    }
}
