using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Builders;
using MonsterTrainModdingAPI.Managers;
using MonsterTrainModdingAPI.Enums.MTCardPools;

namespace MonsterTrainTestMod.Cards.Units
{
    class Sklink
    {
        private static string IDName = "Sklink";
        public static void Make()
        {

            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                CardID = IDName,
                Name = IDName,
                Cost = 1,
                Rarity = CollectableRarity.Uncommon,
                CardPoolIDs = new List<string> { MTCardPoolIDs.GetIDForType(typeof(MTCardPool_UnitsAllBanner)) },
                Description = "Relocate: Do 2 damage to all enemies.",

                CardType = CardType.Monster,
                TargetsRoom = true,
                Targetless = false
            };

            // Art Prefab, we can probably instantiate this ourselves later
            railyard.CreateAndSetCardArtPrefabVariantRef(
                "Assets/GameData/CardArt/Portrait_Prefabs/CardArt_TrainSteward.prefab",
                "a21c55c24d2e5d645a01230d874e26a9"
            );

            // Add special effects, triggers, and other things to cards
            var spawnEffectBuilder = new CardEffectDataBuilder
            {
                EffectStateName = "CardEffectSpawnMonster",
                TargetMode = TargetMode.DropTargetCharacter,
                ParamCharacterData = BuildUnit()
            };
            railyard.Effects.Add(spawnEffectBuilder.Build());

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

                Size = 2,
                Health = 10,
                AttackDamage = 0
            };
            // Unit art asset, complex stuff!
            characterDataBuilder.CreateAndSetCharacterArtPrefabVariantRef(
                "Assets/GameData/CharacterArt/Character_Prefabs/Character_TrainSteward.prefab",
                "8a96184904fce5745ab5139b620b4d31"
            );

            // This is relocate, basically! But I think it will only work for this character
            var ascendTrigger = new CharacterTriggerDataBuilder {
                Trigger = CharacterTriggerData.Trigger.PostAscension};
            var descendTrigger = new CharacterTriggerDataBuilder {
                Trigger = CharacterTriggerData.Trigger.PostDescension};

            var damageEffectBuilder = new CardEffectDataBuilder
            {
                EffectStateName = "CardEffectDamage",
                ParamInt = 2,
                TargetMode = TargetMode.Room
            };
            ascendTrigger.Effects.Add(damageEffectBuilder.Build());
            descendTrigger.Effects.Add(damageEffectBuilder.Build());

            characterDataBuilder.Triggers.Add(ascendTrigger.Build());
            characterDataBuilder.Triggers.Add(descendTrigger.Build());

            return characterDataBuilder.BuildAndRegister();
        }
    }
}
