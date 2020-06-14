using System;
using System.Collections.Generic;
using System.Text;
using HarmonyLib;
using MonsterTrainModdingAPI.Builder;
using MonsterTrainModdingAPI.Managers;
using MonsterTrainModdingAPI.Enum;

// TODO - Icarian

namespace MonsterTrainTestMod.Cards.Units
{
    class SampatiOwl
    {
        public static void Make()
        {
            private static string IDName = "Sampati Owl";

            // Basic Card Stats 
            CardDataBuilder railyard = new CardDataBuilder
            {
                CardID = IDName,
                Name = IDName,
                Cost = 1,
                Rarity = CollectableRarity.Rare,

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

                Size = 1,
                Health = 3,
                AttackDamage = 20
            };
            // Unit art asset, complex stuff!
            characterDataBuilder.CreateAndSetCharacterArtPrefabVariantRef(
                "Assets/GameData/CharacterArt/Character_Prefabs/Character_TrainSteward.prefab",
                "8a96184904fce5745ab5139b620b4d31"
            );

            // Resolve
            var resolveTrigger = new CharacterTriggerBuilder {
                Trigger = Trigger.PostCombat};
            var resolveBuilder = new CardEffectDataBuilder
            {
                EffectStateName = "CardEffectBuffDamage",
                ParamInt = 10,
                TargetMode = TargetMode.Self
            };
            resolveTrigger.Effects.Add(resolveBuilder.Build());

            var healthBuilder = new CardEffectDataBuilder
            {
                EffectStateName = "CardEffectBuffMaxHealth",
                ParamInt = 3,
                TargetMode = TargetMode.Self
            };
            resolveTrigger.Effects.Add(healthBuilder.Build());

            var healthUpBuilder = new CardEffectDataBuilder
            {
                EffectStateName = "CardEffectHeal",
                ParamInt = 3,
                TargetMode = TargetMode.Self
            };
            resolveTrigger.Effects.Add(healthUpBuilder.Build());

            var icarianBuilder = new CardEffectDataBuilder
            {
                EffectStateName = "CardEffectBump",
                ParamInt = 1,
                TargetMode = TargetMode.Self
            };
            resolveTrigger.Effects.Add(icarianBuilder.Build());

            characterDataBuilder.Triggers.Add(resolveTrigger.Build());

            return characterDataBuilder.BuildAndRegister();
        }
    }
}
