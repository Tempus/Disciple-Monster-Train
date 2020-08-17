using DiscipleClan.CardEffects;
using MonsterTrainModdingAPI.Builders;
using System.Collections.Generic;

namespace DiscipleClan.Cards.Unused
{
    class PyromancyWard
    {
        public static string IDName = "PyromancyWard";

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
                    },
                }
            };

            Utils.AddWard(railyard, IDName, BuildUnit());
            Utils.AddImg(railyard, "Webcam.png");

            //railyard.EffectBuilders.Add(
            //new CardEffectDataBuilder
            //{
            //    EffectStateName = "CardEffectSpawnMonster",
            //    TargetMode = TargetMode.FrontInRoom,
            //    ParamInt = 6,
            //    ParamCharacterData = BuildFillerUnit(),
            //});

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

                Size = 4,
                Health = 1,
                AttackDamage = 0,
                CanAttack = false,
                PriorityDraw = false,
                TriggerBuilders = new List<CharacterTriggerDataBuilder> {
                    new CharacterTriggerDataBuilder {
                        Trigger = CharacterTriggerData.Trigger.AfterSpawnEnchant,
                        Description = "Pyreboost",
                        EffectBuilders = new List<CardEffectDataBuilder>
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateName = "CardEffectEnchant",
                                ParamStatusEffects = new StatusEffectStackData[] 
                                { 
                                    new StatusEffectStackData { count=1, statusId="pyreboost" }, 
                                },
                                TargetMode = TargetMode.Room,
                                TargetTeamType = Team.Type.Monsters,
                            }
                        }
                    },
                    new CharacterTriggerDataBuilder
                    {
                        Trigger = CharacterTriggerData.Trigger.OnSpawn,
                        EffectBuilders = new List<CardEffectDataBuilder>
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateName = "CardEffectSpawnMonster",
                                TargetMode = TargetMode.FrontInRoom,
                                ParamInt = 6,
                                ParamCharacterData = BuildFillerUnit(),
                            }
                        }
                    }
                },
                StartingStatusEffects = new StatusEffectStackData[] { new StatusEffectStackData { count = 1, statusId = "immobile" } },
            };

            Utils.AddUnitImg(characterDataBuilder, "SwordWard.png");
            characterDataBuilder.SubtypeKeys = new List<string> { "ChronoSubtype_Ward" };
            return characterDataBuilder;
        }

        public static CharacterData BuildFillerUnit()
        {
            // Monster card, so we build an attached unit
            CharacterDataBuilder characterDataBuilder = new CharacterDataBuilder
            {
                CharacterID = "Slag",
                NameKey = "Slag" + "_Name",

                Size = 1,
                Health = 1,
                AttackDamage = 0,
                CanAttack = false,
                PriorityDraw = false,
                CanBeHealed = false,
                StartingStatusEffects = new StatusEffectStackData[] { 
                    new StatusEffectStackData { count=1, statusId="fragile" },
                    new StatusEffectStackData { count=1, statusId="cardless" },
                },
            };

            Utils.AddUnitImg(characterDataBuilder, "StasisWard.png");
            characterDataBuilder.SubtypeKeys = new List<string> { "ChronoSubtype_Ward" };
            return characterDataBuilder.BuildAndRegister();
        }
    }
}
