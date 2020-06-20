using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using MonsterTrainModdingAPI.Enums.MTCardPools;

// TODO - look into CardTraitRetain and CardTraitFreeze, no builder for CardTriggerEffectData, couldn't find Pyrebound, Chronolock unimplemented (but can be faked)

namespace DiscipleClan.Cards.Units
{
    class DivineroftheInfinite
    {
        public static string IDName = "Diviner of the Infinite";
        public static void Make()
        {

            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                CardID = IDName,
                Name = IDName,
                Cost = 5,
                Rarity = CollectableRarity.Uncommon,
                CardPoolIDs = new List<string> { MTCardPoolIDs.GetIDForType(typeof(MTCardPool_UnitsAllBanner)) },
                Description = "Permafrost. Pyrebound. Reserve: Apply 1 (TODO)Chronolock, (TODO)Cost -1.",
                AssetPath = "netstandard2.0/chrono/zyzzy.png",

                CardType = CardType.Monster,
                TargetsRoom = true,
                Targetless = false,

                TraitBuilders = new List<CardTraitDataBuilder>
                {
                    new CardTraitDataBuilder
                    {
                        TraitStateName = "CardTraitRetain"
                    },
                    new CardTraitDataBuilder
                    {
                        TraitStateName = "CardTraitLimitedRange"
                    }
                },

                TriggerBuilders = new List<CardTriggerEffectDataBuilder>
                {
                    new CardTriggerEffectDataBuilder
                    {
                        trigger = CardTriggerType.OnUnplayed,
                        //EffectBuilders = new List<CardEffectDataBuilder>
                        //{
                        //    new CardEffectDataBuilder
                        //    {
                        //        EffectStateName = "CardEffectAdjustEnergy",
                        //        ParamInt = -1,
                        //    }
                        //}
                    }
                }
            };

            // Add special effects, triggers, and other things to cards
            var spawnEffectBuilder = new CardEffectDataBuilder
            {
                EffectStateName = "CardEffectSpawnMonster",
                TargetMode = TargetMode.DropTargetCharacter,
                ParamCharacterData = BuildUnit()
            };
            railyard.Effects.Add(spawnEffectBuilder.Build());

            // CardTriggerEffectData needs to add a trigger for OnUnplayed here, that activates the cost and stasis effects


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
                Name = IDName,

                Size = 3,
                Health = 60,
                AttackDamage = 40
            };
            // Unit art asset, complex stuff!
            characterDataBuilder.CreateAndSetCharacterArtPrefabVariantRef(
                "Assets/GameData/CharacterArt/Character_Prefabs/Character_TrainSteward.prefab",
                "8a96184904fce5745ab5139b620b4d31"
            );

            return characterDataBuilder.BuildAndRegister();
        }
    }
}
