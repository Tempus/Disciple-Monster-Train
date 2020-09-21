using DiscipleClan.Triggers;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Utilities;
using System.Collections.Generic;
using static MonsterTrainModdingAPI.Constants.VanillaStatusEffectIDs;

namespace DiscipleClan.Cards.Pyrepact
{
    class AncientPyresnail
    {
        public static string IDName = "Ancient Pyresnail";
        public static string imgName = "KnittedSnail";

        public static void Make()
        {
            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                Cost = 2,
                Rarity = CollectableRarity.Rare,
            };

            Utils.AddUnit(railyard, IDName, BuildUnit());
            Utils.AddCardPortrait(railyard, "Pyresnail");

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

                Size = 5,
                Health = 33,
                AttackDamage = 0,

                BundleLoadingInfo = new BundleAssetLoadingInfo
                {
                    FilePath = "chrono/arcadian_units",
                    SpriteName = "assets/plr_ancient.png",
                    ObjectName = "assets/plr_ancient.prefab",
                    AssetType = AssetRefBuilder.AssetTypeEnum.Character
                },

                TriggerBuilders = new List<CharacterTriggerDataBuilder> {
                    new CharacterTriggerDataBuilder {
                        Trigger = CharacterTriggerData.Trigger.AfterSpawnEnchant,
                        DescriptionKey = "Ancient Pyresnail_Trigger",
                        EffectBuilders = new List<CardEffectDataBuilder>
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateName = "CardEffectEnchant",
                                ParamStatusEffects = new StatusEffectStackData[] { new StatusEffectStackData { count=1, statusId="pyreboost" } },
                                TargetMode = TargetMode.Room,
                                TargetTeamType = Team.Type.Monsters,
                            }
                        }
                    },
                },
            };

            characterDataBuilder.AddStartingStatusEffect(Immobile, 1);

            Utils.AddUnitAnim(characterDataBuilder, "pyresnail");
            return characterDataBuilder.BuildAndRegister();
        }
    }
}
