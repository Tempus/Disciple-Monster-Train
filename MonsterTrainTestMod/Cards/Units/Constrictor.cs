using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Builder;
using MonsterTrainModdingAPI.Managers;
using MonsterTrainModdingAPI.Enum;

// TODO - Innate, Can't target floor above/below easily? Look into target filters, also 'how to tell which floor I'm on'. Otherwise, COMPRESS EVERYTHING chronoTicked

namespace MonsterTrainTestMod.Cards.Units
{
    class Constrictor
    {
        public static void Make()
        {
            private static string IDName = "Constrictor";

            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                CardID = IDName,
                Name = IDName,
                Cost = 1,
                Rarity = CollectableRarity.Uncommon,

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


            // Putting it in card pools... I feel like there's a better place for this
            railyard.SetCardClan(MTClan.Awoken);
            railyard.AddToCardPool(MTCardPool.AwokenBannerPool);

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
                Health = 15,
                AttackDamage = 12
            };
            // Unit art asset, complex stuff!
            characterDataBuilder.CreateAndSetCharacterArtPrefabVariantRef(
                "Assets/GameData/CharacterArt/Character_Prefabs/Character_TrainSteward.prefab",
                "8a96184904fce5745ab5139b620b4d31"
            );

            // Pulls everything that it can to this floor on summon
            var strikeTrigger = new CharacterTriggerBuilder {
                Trigger = Trigger.OnSpawn};
            var downBuilder = new CardEffectDataBuilder
            {
                EffectStateName = "CardEffectBump",
                ParamInt = -1,
                TargetMode = TargetMode.Tower
            };
            strikeTrigger.Effects.Add(downBuilder.Build());

            var upBuilder = new CardEffectDataBuilder
            {
                EffectStateName = "CardEffectBump",
                ParamInt = 1,
                TargetMode = TargetMode.Tower
            };
            strikeTrigger.Effects.Add(upBuilder.Build());

            characterDataBuilder.Triggers.Add(strikeTrigger.Build());


            return characterDataBuilder.BuildAndRegister();
        }
    }
}
