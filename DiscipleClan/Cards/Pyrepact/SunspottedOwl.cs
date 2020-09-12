using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Utilities;
using System.Collections.Generic;
using static MonsterTrainModdingAPI.Constants.VanillaCardPoolIDs;

// TODO - Icarian, Pyre attacks whole tower (we can fake it though)

namespace DiscipleClan.Cards.Pyrepact
{
    class SunspottedOwl
    {
        public static string IDName = "SunspottedOwl";
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
            Utils.AddImg(railyard, imgName + ".png");
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
                Health = 1,
                AttackDamage = 3,

                BundleLoadingInfo = new BundleAssetLoadingInfo
                {
                    FilePath = "chrono/arcadian_units",
                    SpriteName = "assets/morsowlpreview.png",
                    ObjectName = "assets/morsowl.prefab",
                    AssetType = AssetRefBuilder.AssetTypeEnum.Character
                },

                TriggerBuilders = new List<CharacterTriggerDataBuilder>
                {
                    new CharacterTriggerDataBuilder
                    {
                        Trigger = CharacterTriggerData.Trigger.OnTurnBegin,
                        EffectBuilders = new List<CardEffectDataBuilder>
                        {
                            new CardEffectDataBuilder
                            {
                                EffectStateName = "CardEffectGainEnergy",
                                ParamInt = 1,
                            },
                        }
                    },
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

            //Utils.AddUnitImg(characterDataBuilder, imgName + ".png");
            return characterDataBuilder.BuildAndRegister();
        }
    }
}
