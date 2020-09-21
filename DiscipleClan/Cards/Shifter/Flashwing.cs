using MonsterTrainModdingAPI.Builders;
using System.Collections.Generic;
using static MonsterTrainModdingAPI.Constants.VanillaStatusEffectIDs;
using static MonsterTrainModdingAPI.Constants.VanillaCardPoolIDs;

namespace DiscipleClan.Cards.Shifter
{
    class Flashwing
    {
        public static string IDName = "Flashwing";
        public static string imgName = "Hootiful";
        public static void Make()
        {

            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 1,
                Rarity = CollectableRarity.Common,
            };

            Utils.AddUnit(railyard, IDName, BuildUnit());
            Utils.AddCardPortrait(railyard, "Flashfeather");
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

                Size = 1,
                Health = 3,
                AttackDamage = 5,

                PriorityDraw = false,

                TriggerBuilders = new List<CharacterTriggerDataBuilder>
                {
                    new CharacterTriggerDataBuilder
                    {
                        Trigger = CharacterTriggerData.Trigger.OnDeath,
                        EffectBuilders = new List<CardEffectDataBuilder>
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateName = "CardEffectAddStatusEffect",
                                TargetMode = TargetMode.FrontInRoom,
                                TargetTeamType = Team.Type.Heroes,
                                ParamStatusEffects = new StatusEffectStackData[] {new StatusEffectStackData { count=2, statusId=Dazed } }
                            }
                        }
                    }

                }
            };
            characterDataBuilder.AddStartingStatusEffect("icarian", 1);

            Utils.AddUnitAnim(characterDataBuilder, "flashfeather");

            return characterDataBuilder.BuildAndRegister();
        }
    }
}
