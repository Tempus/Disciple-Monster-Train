using DiscipleClan.Triggers;
using MonsterTrainModdingAPI.Builders;
using System.Collections.Generic;
using static MonsterTrainModdingAPI.Constants.VanillaStatusEffectIDs;

namespace DiscipleClan.Cards.Speedtime
{
    class AncientSavant
    {
        public static string IDName = "Ancient Savant";
        public static string imgName = "KnittedSnail";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 2,
                Rarity = CollectableRarity.Uncommon,
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
                SubtypeKeys = new List<string> { "ChronoSubtype_Eternal" },

                Size = 3,
                Health = 50,
                AttackDamage = 0,

                TriggerBuilders = new List<CharacterTriggerDataBuilder> {
                    new CharacterTriggerDataBuilder {
                        Trigger = CharacterTriggerData.Trigger.CardMonsterPlayed,
                        DescriptionKey = IDName + "_Desc",
                        EffectBuilders = new List<CardEffectDataBuilder>
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateName = "CardEffectEnchant",
                                ParamStatusEffects = new StatusEffectStackData[] { new StatusEffectStackData { count=1, statusId="ambush" } },
                                ParamTrigger = CharacterTriggerData.Trigger.OnDeath,
                                TargetMode = TargetMode.Room,
                                TargetTeamType = Team.Type.Monsters,
                            }
                        }
                    },
                    new CharacterTriggerDataBuilder {
                        Trigger = OnRelocate.OnRelocateCharTrigger.GetEnum(),
                        DescriptionKey = IDName + "_Desc",
                        EffectBuilders = new List<CardEffectDataBuilder>
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateName = "CardEffectEnchant",
                                ParamStatusEffects = new StatusEffectStackData[] { new StatusEffectStackData { count=1, statusId="ambush" } },
                                ParamTrigger = CharacterTriggerData.Trigger.OnDeath,
                                TargetMode = TargetMode.Room,
                                TargetTeamType = Team.Type.Monsters,
                            }
                        }
                    },
                    new CharacterTriggerDataBuilder {
                        Trigger = CharacterTriggerData.Trigger.PreCombat,
                        DescriptionKey = IDName + "_Desc",
                        EffectBuilders = new List<CardEffectDataBuilder>
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateName = "CardEffectEnchant",
                                ParamStatusEffects = new StatusEffectStackData[] { new StatusEffectStackData { count=1, statusId="ambush" } },
                                ParamTrigger = CharacterTriggerData.Trigger.OnDeath,
                                TargetMode = TargetMode.Room,
                                TargetTeamType = Team.Type.Monsters,
                            }
                        }
                    }
                },
            };

            characterDataBuilder.AddStartingStatusEffect(Immobile, 1);

            Utils.AddUnitImg(characterDataBuilder, imgName + ".png");
            return characterDataBuilder.BuildAndRegister();
        }
    }
}